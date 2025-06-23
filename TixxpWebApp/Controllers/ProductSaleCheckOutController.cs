using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.InvoiceType;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Business.Services.Extension;
using Tixxp.Entities.InvoiceType;
using Tixxp.Entities.ProductSale;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.Entities.ProductSaleInvoiceInfo;
using Tixxp.WebApp.Models.ProductSaleCheckOut;

namespace Tixxp.WebApp.Controllers
{
    public class ProductSaleCheckOutController : Controller
    {
        private readonly IInvoiceTypeService _invoiceTypeService;
        private readonly IProductSaleService _productSaleService;
        private readonly IProductPriceService _productPriceService;
        private readonly IProductSaleDetailService _productSaleDetailService;
        private readonly IProductSaleInvoiceInfoService _productSaleInvoiceInfoService;

        private const long DefaultCounterId = 1;

        public ProductSaleCheckOutController(
            IInvoiceTypeService invoiceTypeService,
            IProductSaleService productSaleService,
            IProductSaleDetailService productSaleDetailService,
            IProductSaleInvoiceInfoService productSaleInvoiceInfoService,
            IProductPriceService productPriceService)
        {
            _invoiceTypeService = invoiceTypeService;
            _productSaleService = productSaleService;
            _productSaleDetailService = productSaleDetailService;
            _productSaleInvoiceInfoService = productSaleInvoiceInfoService;
            _productPriceService = productPriceService;
        }

        public IActionResult Index(long productSaleId)
        {
            var invoiceTypesResult = _invoiceTypeService.GetAll();
            ViewBag.InvoiceTypes = invoiceTypesResult.Success
                ? invoiceTypesResult.Data
                : new List<InvoiceTypeEntity>();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderSummary(long productSaleId)
        {
            var summaries = new List<ProductSaleSummaryDto>();

            var saleDetailsResult = _productSaleDetailService
                .GetListWithInclude(x => x.ProductSaleId == productSaleId, x => x.Product);

            if (saleDetailsResult.Success)
            {
                foreach (var detail in saleDetailsResult.Data)
                {
                    var priceResult = _productPriceService.GetById(detail.ProductId);
                    var price = priceResult.Success ? priceResult.Data?.Price ?? 0 : 0;

                    summaries.Add(new ProductSaleSummaryDto
                    {
                        ProductName = detail.Product?.Name,
                        ProductImageUrl = detail.Product?.ImageFilePath,
                        Quantity = detail.Quantity,
                        Price = price
                    });
                }
            }

            return Ok(summaries);
        }

        [HttpPost]
        public JsonResult AddInvoiceInfo([FromBody] ProductSaleInvoiceInfoModel model)
        {
            if (model == null || model.ProductSaleId <= 0)
            {
                return Json(new { isSuccess = false, message = "Geçersiz veri." });
            }

            var entity = new ProductSaleInvoiceInfoEntity
            {
                ProductSaleId = model.ProductSaleId,
                InvoiceTypeId = model.InvoiceTypeId,
                IdentityNumber = model.IdentityNumber,
                CompanyName = model.CompanyName,
                TaxNumber = model.TaxNumber,
                TaxOffice = model.TaxOffice,
                CountyId = model.CountyId,
                CreatedBy = User.GetUserId().Value,
                Created_Date = DateTime.Now,
                IsDeleted = false
            };

            var result = _productSaleInvoiceInfoService.Add(entity);
            return Json(new
            {
                isSuccess = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public JsonResult Submit([FromBody] List<ProductSaleCheckOutItem> productSaleCheckOutItems)
        {
            if (productSaleCheckOutItems == null || !productSaleCheckOutItems.Any())
            {
                return Json(new { isSuccess = false, message = "Ürün seçimi yapılmadı." });
            }

            var currencyTypeIds = productSaleCheckOutItems.Select(x => x.CurrencyTypeId).Distinct().ToList();
            if (currencyTypeIds.Count > 1)
            {
                return Json(new
                {
                    isSuccess = false,
                    message = "Lütfen yalnızca tek bir para birimi seçerek satış işlemini gerçekleştirin."
                });
            }

            var productSale = new ProductSaleEntity
            {
                CounterId = DefaultCounterId,
                CreatedBy = User.GetUserId().Value
            };

            var saleResult = _productSaleService.AddAndReturn(productSale);
            if (!saleResult.Success || saleResult.Data == null)
            {
                return Json(new { isSuccess = false, message = "Satış oluşturulamadı." });
            }

            foreach (var item in productSaleCheckOutItems)
            {
                var detail = new ProductSaleDetailEntity
                {
                    ProductSaleId = saleResult.Data.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };
                _productSaleDetailService.Add(detail);
            }

            return Json(new { isSuccess = true, productSaleId = saleResult.Data.Id });
        }
    }
}
