using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Business.Services.Extension;
using Tixxp.Core.Utilities.Enums.ProductSaleStatusEnum;
using Tixxp.Entities.ProductSale;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.WebApp.Models.ProductSaleCheckOut;

namespace Tixxp.WebApp.Controllers
{
    public class ProductSaleCheckOutController : Controller
    {
        private readonly IProductSaleService _productSaleService;
        private readonly IProductSaleInvoiceInfoService _productSaleInvoiceInfoService;
        private readonly IProductTranslationService _productTranslationService;
        private readonly ILanguageService _languageService;
        private readonly IProductPriceService _productPriceService;
        private readonly IProductSaleDetailService _productSaleDetailService;
        private readonly IStringLocalizer<ProductSaleCheckOutController> _stringLocalizer;

        private const long DefaultCounterId = 1;

        public ProductSaleCheckOutController(
            IProductSaleService productSaleService,
            IProductSaleDetailService productSaleDetailService,
            IProductPriceService productPriceService,
            IStringLocalizer<ProductSaleCheckOutController> stringLocalizer,
            IProductTranslationService productTranslationService,
            ILanguageService languageService,
            IProductSaleInvoiceInfoService productSaleInvoiceInfoService)
        {
            _productSaleService = productSaleService;
            _productSaleDetailService = productSaleDetailService;
            _productPriceService = productPriceService;
            _stringLocalizer = stringLocalizer;
            _productTranslationService = productTranslationService;
            _languageService = languageService;
            _productSaleInvoiceInfoService = productSaleInvoiceInfoService;
        }

        // 🔥 Artık kampanya ve tutar bilgileri de alınıyor
        public IActionResult Index(long productSaleId, long? campaignId = null, decimal? subTotal = null, decimal? discount = null, decimal? finalTotal = null)
        {
            if (productSaleId <= 0)
                return RedirectToAction("Index", "ProductSale");

            ViewBag.ProductSaleId = productSaleId;
            ViewBag.CampaignId = campaignId;
            ViewBag.SubTotal = subTotal ?? 0;
            ViewBag.Discount = discount ?? 0;
            ViewBag.FinalTotal = finalTotal ?? 0;

            return View();
        }

        [HttpGet]
        public IActionResult GetOrderSummary(long productSaleId)
        {
            if (productSaleId <= 0)
                return Ok(Enumerable.Empty<ProductSaleSummaryDto>());

            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var languageResult = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            var languageId = languageResult.Success ? languageResult.Data?.Id : null;

            var saleDetailsResult = _productSaleDetailService
                .GetListWithInclude(x => x.ProductSaleId == productSaleId, x => x.Product, a => a.CurrencyType);

            if (!saleDetailsResult.Success || saleDetailsResult.Data == null || saleDetailsResult.Data.Count == 0)
                return Ok(Enumerable.Empty<ProductSaleSummaryDto>());

            var details = saleDetailsResult.Data;

            var productIds = details
                .Where(d => d.ProductId > 0)
                .Select(d => d.ProductId)
                .Distinct()
                .ToList();

            var translationByProductId = new Dictionary<long, string>();
            if (languageId.HasValue)
            {
                var translationsResult = _productTranslationService.GetList(
                    t => productIds.Contains(t.ProductId) && t.LanguageId == languageId.Value);

                if (translationsResult.Success && translationsResult.Data != null)
                {
                    foreach (var t in translationsResult.Data)
                    {
                        if (!translationByProductId.ContainsKey(t.ProductId))
                            translationByProductId[t.ProductId] = t.Name;
                    }
                }
            }

            var priceCache = new Dictionary<long, decimal>();
            decimal GetPrice(long pid)
            {
                if (priceCache.TryGetValue(pid, out var cached)) return cached;

                var priceResult = _productPriceService.GetById(pid);
                var price = priceResult.Success ? (priceResult.Data?.Price ?? 0m) : 0m;

                priceCache[pid] = price;
                return price;
            }

            var summaries = new List<ProductSaleSummaryDto>(details.Count);
            foreach (var detail in details)
            {
                var pid = detail.ProductId;
                var name = (translationByProductId.TryGetValue(pid, out var translated) ? translated : null) ?? "Product";

                summaries.Add(new ProductSaleSummaryDto
                {
                    ProductName = name,
                    CurrencyTypeSymbol = detail.CurrencyType?.Symbol,
                    ProductImageUrl = detail.Product?.ImageFilePath,
                    Quantity = detail.Quantity,
                    Price = GetPrice(pid)
                });
            }

            return Ok(summaries);
        }

        // 🔥 Artık indirimli tutarları da kaydeder
        [HttpPost]
        public JsonResult Submit([FromBody] ProductSaleCheckOutSubmitModel model)
        {
            if (model == null || model.Items == null || model.Items.Count == 0)
                return Json(new { isSuccess = false, message = _stringLocalizer["productSaleCheckOutController.PRODUCT_SELECTION"].ToString() });

            var currencyTypeIds = model.Items.Select(x => x.CurrencyTypeId).Distinct().ToList();
            if (currencyTypeIds.Count > 1)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = _stringLocalizer["productSaleCheckOutController.CURRENCY.TYPE_VALIDATION"].ToString()
                });
            }

            // 🔥 Kampanya + indirimli tutar kayıt
            var productSale = new ProductSaleEntity
            {
                InvoiceTypeId = 1,
                StatusId = (long)ProductSaleStatusEnum.Pending,
                CounterId = DefaultCounterId,
                CampaignId = model.CampaignId,
                //SubTotal = model.SubTotal,
                //DiscountAmount = model.DiscountAmount,
                //FinalTotal = model.FinalTotal,
                CreatedBy = User.GetUserId().GetValueOrDefault()
            };

            var saleResult = _productSaleService.AddAndReturn(productSale);
            if (!saleResult.Success || saleResult.Data == null)
                return Json(new { isSuccess = false, message = _stringLocalizer["productSaleCheckOutController.SALE_NOT_BE_CREATED"].ToString() });

            var saleId = saleResult.Data.Id;

            foreach (var item in model.Items)
            {
                var detail = new ProductSaleDetailEntity
                {
                    ProductSaleId = saleId,
                    ProductId = item.ProductId,
                    CurrencyTypeId = model.Items.Select(x => x.CurrencyTypeId).FirstOrDefault(),
                    Quantity = item.Quantity
                };

                var addDetailResult = _productSaleDetailService.Add(detail);
                if (!addDetailResult.Success)
                {
                    return Json(new { isSuccess = false, message = _stringLocalizer["productSaleCheckOutController.COLUD_NOT_ADD_SALES_ITEM"].ToString() });
                }
            }

            // JSON geri dönüşte de indirimli tutarlar gelsin
            return Json(new
            {
                isSuccess = true,
                productSaleId = saleId,
                subTotal = model.SubTotal,
                discount = model.DiscountAmount,
                finalTotal = model.FinalTotal
            });
        }

        [HttpPost]
        public JsonResult SubmitWithCustomer([FromBody] ProductSaleCheckOutSubmitModel model)
        {
            if (model == null)
                return Json(new { isSuccess = false, message = _stringLocalizer["productSaleCheckOutController.CUSTOMER_REQUIRED"].ToString() });

            if (model.CustomerInfo != null)
            {
                _productSaleInvoiceInfoService.Add(new Entities.ProductSaleInvoiceInfo.ProductSaleInvoiceInfoEntity
                {
                    InvoiceTypeId = 1,
                    ProductSaleId = model.ProductSaleId,
                    FirstName = model.CustomerInfo?.FullName,
                    LastName = model.CustomerInfo?.Surname,
                    IdentityNumber = model.CustomerInfo?.Tckn
                });
            }

            var productSale = _productSaleService.GetFirstOrDefault(x => x.Id == model.ProductSaleId);
            if (productSale.Success && productSale.Data != null)
            {
                productSale.Data.StatusId = (long)ProductSaleStatusEnum.Completed;
                _productSaleService.Update(productSale.Data);
            }

            return Json(new { isSuccess = true, productSaleId = model.ProductSaleId });
        }
    }
}
