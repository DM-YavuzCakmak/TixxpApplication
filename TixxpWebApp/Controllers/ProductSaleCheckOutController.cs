using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.InvoiceType;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductSaleInvoiceInfo;
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
        public ProductSaleCheckOutController(IInvoiceTypeService invoiceTypeService, IProductSaleService productSaleService, IProductSaleDetailService productSaleDetailService, IProductSaleInvoiceInfoService productSaleInvoiceInfoService, IProductPriceService productPriceService)
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
            List<ProductSaleSummaryDto> productSaleSummaryDtos = new List<ProductSaleSummaryDto>();
            var productSaleDetails = _productSaleDetailService.GetListWithInclude(x => x.ProductSaleId == productSaleId, x => x.Product);
            if (productSaleDetails.Success)
            {
                foreach (var productSaleDetailEntity in productSaleDetails.Data)
                {
                    ProductSaleSummaryDto productSaleSummaryDto = new ProductSaleSummaryDto();
                    productSaleSummaryDto.Price = _productPriceService.GetById(productSaleDetailEntity.ProductId).Data.Price;
                    productSaleSummaryDto.Quantity = productSaleDetailEntity.Quantity;
                    productSaleSummaryDto.ProductName = productSaleDetailEntity.Product.Name;
                    productSaleSummaryDto.ProductImageUrl = productSaleDetailEntity.Product.ImageFilePath;
                    productSaleSummaryDtos.Add(productSaleSummaryDto);
                }
            }

            return Ok(productSaleSummaryDtos);
        }

        /// <summary>
        /// Fatura Bilgilerini Kayıt eder
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddInvoiceInfo([FromBody] ProductSaleInvoiceInfoModel model)
        {
            if (model != null && model.ProductSaleId > 0)
            {
                var entity = new ProductSaleInvoiceInfoEntity
                {
                    ProductSaleId = model.ProductSaleId,
                    InvoiceTypeId = model.InvoiceTypeId,
                    IdentityNumber = model.IdentityNumber,
                    CompanyName = model.CompanyName,
                    TaxNumber = model.TaxNumber,
                    TaxOffice = model.TaxOffice,
                    CountyId = model.CountyId,
                    CreatedBy = 6, // TODO: login olan kullanıcının Id'si ile değiştir
                    Created_Date = DateTime.Now,
                    IsDeleted = false
                };

                var result = _productSaleInvoiceInfoService.Add(entity);
                if (result.Success)
                    return Json(new { isSuccess = true });

                return Json(new { isSuccess = false, message = result.Message });
            }

            return Json(new { isSuccess = false, message = "Geçersiz veri." });
        }

        /// <summary>
        /// İlk Tab'dan gelen Ürün bilgilerini kayıt eder ve Product Sale'ı oluşturur.
        /// </summary>
        /// <param name="productSaleCheckOutItems"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Submit([FromBody] List<ProductSaleCheckOutItem> productSaleCheckOutItems)
        {
            if (productSaleCheckOutItems.Any())
            {
                ProductSaleEntity productSaleEntity = new ProductSaleEntity();
                productSaleEntity.CounterId = 1;
                var newProductSaleEntity = _productSaleService.AddAndReturn(productSaleEntity);

                foreach (var productSaleCheckOutItem in productSaleCheckOutItems)
                {
                    ProductSaleDetailEntity productSaleDetailEntity = new ProductSaleDetailEntity();
                    productSaleDetailEntity.ProductSaleId = newProductSaleEntity.Data.Id;
                    productSaleDetailEntity.ProductId = productSaleCheckOutItem.ProductId;
                    productSaleDetailEntity.Quantity = productSaleCheckOutItem.Quantity;
                    _productSaleDetailService.Add(productSaleDetailEntity);
                }
                return Json(new { isSuccess = true, productSaleId = newProductSaleEntity.Data.Id });
            }

            return Json(new { isSuccess = false });
        }

    }
}
