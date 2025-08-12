using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Business.Services.Extension;
using Tixxp.Entities.ProductSale;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.WebApp.Models.ProductSaleCheckOut;

namespace Tixxp.WebApp.Controllers
{
    public class ProductSaleCheckOutController : Controller
    {
        private readonly IProductSaleService _productSaleService;
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
            ILanguageService languageService)
        {
            _productSaleService = productSaleService;
            _productSaleDetailService = productSaleDetailService;
            _productPriceService = productPriceService;
            _stringLocalizer = stringLocalizer;
            _productTranslationService = productTranslationService;
            _languageService = languageService;
        }


        public IActionResult Index(long productSaleId)
        {
            if (productSaleId <= 0)
                return RedirectToAction("Index", "ProductSale");

            ViewBag.ProductSaleId = productSaleId;
            return View();
        }

        [HttpGet]
        public IActionResult GetOrderSummary(long productSaleId)
        {
            if (productSaleId <= 0)
                return Ok(Enumerable.Empty<ProductSaleSummaryDto>());

            // Geçerli dil bilgisi
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var languageResult = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            var languageId = languageResult.Success ? languageResult.Data?.Id : null;

            // Satış detaylarını (ürün dahil) tek seferde al
            var saleDetailsResult = _productSaleDetailService
                .GetListWithInclude(x => x.ProductSaleId == productSaleId, x => x.Product, a => a.CurrencyType);

            if (!saleDetailsResult.Success || saleDetailsResult.Data == null || saleDetailsResult.Data.Count == 0)
                return Ok(Enumerable.Empty<ProductSaleSummaryDto>());

            var details = saleDetailsResult.Data;

            // Tekil ürün Id listesi
            var productIds = details
                .Where(d => d.ProductId > 0)
                .Select(d => d.ProductId)
                .Distinct()
                .ToList();

            // Toplu çeviri (varsa)
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
                var name =
                    (translationByProductId.TryGetValue(pid, out var translated) ? translated : null)
                    ?? "Product";

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

        [HttpPost]
        public JsonResult Submit([FromBody] List<ProductSaleCheckOutItem> items)
        {
            if (items == null || items.Count == 0)
                return Json(new { isSuccess = false, message = _stringLocalizer["productSaleCheckOutController.PRODUCT_SELECTION"].ToString() });

            // Tüm kalemlerde aynı para birimi olmalı
            var currencyTypeIds = items.Select(x => x.CurrencyTypeId).Distinct().ToList();
            if (currencyTypeIds.Count > 1)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = _stringLocalizer["productSaleCheckOutController.CURRENCY.TYPE_VALIDATION"].ToString()
                });
            }

            var productSale = new ProductSaleEntity
            {
                CounterId = DefaultCounterId,
                CreatedBy = User.GetUserId().GetValueOrDefault()
            };

            var saleResult = _productSaleService.AddAndReturn(productSale);
            if (!saleResult.Success || saleResult.Data == null)
                return Json(new { isSuccess = false, message = _stringLocalizer["productSaleCheckOutController.SALE_NOT_BE_CREATED"].ToString() });

            var saleId = saleResult.Data.Id;

            foreach (var item in items)
            {
                var detail = new ProductSaleDetailEntity
                {
                    ProductSaleId = saleId,
                    ProductId = item.ProductId,
                    CurrencyTypeId = items.Select(x => x.CurrencyTypeId).FirstOrDefault(),
                    Quantity = item.Quantity
                };

                var addDetailResult = _productSaleDetailService.Add(detail);
                if (!addDetailResult.Success)
                {
                    return Json(new { isSuccess = false, message = _stringLocalizer["productSaleCheckOutController.COLUD_NOT_ADD_SALES_ITEM"].ToString() });
                }
            }

            return Json(new { isSuccess = true, productSaleId = saleId });
        }
    }
}
