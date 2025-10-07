using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Tixxp.Business.DataTransferObjects.Campaign;
using Tixxp.Business.Services.Abstract.Campaign;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.WebApp.Models.ProductPrice;
using Tixxp.WebApp.Models.ProductSale;
using CartItemDto = Tixxp.Business.DataTransferObjects.Campaign.CartItemDto;

namespace Tixxp.WebApp.Controllers
{
    public class ProductSaleController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductPriceService _productPriceService;
        private readonly ICurrencyTypeService _currencyTypeService;
        private readonly IProductTranslationService _productTranslationService;
        private readonly ILanguageService _languageService;
        private readonly ICampaignService _campaignService;

        public ProductSaleController(
            IProductService productService,
            IProductPriceService productPriceService,
            ICurrencyTypeService currencyTypeService,
            IProductTranslationService productTranslationService,
            ILanguageService languageService,
            ICampaignService campaignService)
        {
            _productService = productService;
            _productPriceService = productPriceService;
            _currencyTypeService = currencyTypeService;
            _productTranslationService = productTranslationService;
            _languageService = languageService;
            _campaignService = campaignService;
        }

        public IActionResult Index()
        {
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var languageResult = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = languageResult.Success ? languageResult.Data?.Id : null;

            var productResult = _productService.GetAll();
            var priceResult = _productPriceService.GetAll();

            if (productResult.Success && priceResult.Success)
            {
                var productIds = productResult.Data.Select(p => p.Id).ToList();
                var translationList = _productTranslationService
                    .GetList(x => productIds.Contains(x.ProductId) && x.LanguageId == languageId)
                    .Data;

                List<ProductWithPriceViewModel> productWithPriceViewModels =
                    (from product in productResult.Data
                     join price in priceResult.Data on product.Id equals price.ProductId into pp
                     from price in pp.DefaultIfEmpty()
                     select new ProductWithPriceViewModel
                     {
                         ProductId = product.Id,
                         Name = translationList.FirstOrDefault(t => t.ProductId == product.Id)?.Name ?? product.Code,
                         Code = product.Code,
                         CurrencyTypeId = price?.CurrencyTypeId ?? 0,
                         CurrencyTypeSymbol = "",
                         Price = price?.Price ?? 0,
                         VatRate = price?.VatRate ?? 0
                     }).ToList();

                if (productWithPriceViewModels.Any())
                {
                    var currencyTypeList = _currencyTypeService.GetList(x =>
                        productWithPriceViewModels.Select(v => v.CurrencyTypeId).Contains(x.Id));

                    if (currencyTypeList.Success && currencyTypeList.Data.Any())
                    {
                        foreach (var vm in productWithPriceViewModels)
                        {
                            vm.CurrencyTypeSymbol = currencyTypeList.Data
                                .FirstOrDefault(x => x.Id == vm.CurrencyTypeId)?.Symbol ?? "";
                        }
                    }
                }

                return View(productWithPriceViewModels);
            }

            return View(new List<ProductWithPriceViewModel>());
        }

        [HttpPost]
        public IActionResult ValidateCoupon([FromBody] CouponRequestDto dto)
        {
            if (dto == null || dto.Products == null || !dto.Products.Any())
                return Json(new { isSuccess = false, message = "Sepet boş. Önce ürün ekleyiniz." });

            if (string.IsNullOrWhiteSpace(dto.CouponCode))
                return Json(new { isSuccess = false, message = "Kupon kodu boş olamaz." });

            var campaigns = _campaignService.GetList(x => x.IsActive && !x.IsDeleted);
            if (!campaigns.Success || !campaigns.Data.Any())
                return Json(new { isSuccess = false, message = "Geçerli kampanya bulunamadı." });

            var applyDto = new ApplyCampaignForProductRequestDto
            {
                CouponCode = dto.CouponCode
            };

            foreach (var cartItem in dto.Products)
            {
                applyDto.Products.Add(new CartItemDto
                {
                    ProductId = cartItem.ProductId,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity,
                    CurrencyTypeId = cartItem.CurrencyTypeId
                });
            }

            foreach (var campaign in campaigns.Data)
            {
                var finalPrice = _campaignService.ApplyCampaignsForProduct(applyDto, campaign);

                // 🔥 Kampanya başarılı şekilde indirim uyguladıysa
                if (finalPrice < applyDto.SubTotal)
                {
                    var discount = applyDto.SubTotal - finalPrice;

                    return Json(new
                    {
                        isSuccess = true,
                        message = campaign.Description,
                        discount = discount,
                        subTotal = applyDto.SubTotal,
                        finalPrice = finalPrice,
                        campaignId = campaign.Id 
                    });
                }
            }

            return Json(new { isSuccess = false, message = "Geçersiz veya süresi dolmuş kupon kodu." });
        }
    }
}
