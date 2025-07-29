using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductTranslation; // bunu ekle
using Tixxp.Business.Services.Abstract.Language; // bunu da ekle
using Tixxp.WebApp.Models.ProductPrice;

namespace Tixxp.WebApp.Controllers
{
    public class ProductSaleController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductPriceService _productPriceService;
        private readonly ICurrencyTypeService _currencyTypeService;
        private readonly IProductTranslationService _productTranslationService;
        private readonly ILanguageService _languageService;

        public ProductSaleController(
            IProductService productService,
            IProductPriceService productPriceService,
            ICurrencyTypeService currencyTypeService,
            IProductTranslationService productTranslationService,
            ILanguageService languageService)
        {
            _productService = productService;
            _productPriceService = productPriceService;
            _currencyTypeService = currencyTypeService;
            _productTranslationService = productTranslationService;
            _languageService = languageService;
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
                var translationList = _productTranslationService.GetList(x => productIds.Contains(x.ProductId) && x.LanguageId == languageId).Data;

                List<ProductWithPriceViewModel> productWithPriceViewModels = (from product in productResult.Data
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
                    var currencyTypeList = _currencyTypeService.GetList(x => productWithPriceViewModels.Select(v => v.CurrencyTypeId).Contains(x.Id));
                    if (currencyTypeList.Success && currencyTypeList.Data.Any())
                    {
                        foreach (var productWithPriceViewModel in productWithPriceViewModels)
                        {
                            productWithPriceViewModel.CurrencyTypeSymbol = currencyTypeList.Data
                                .FirstOrDefault(x => x.Id == productWithPriceViewModel.CurrencyTypeId)?.Symbol ?? "";
                        }
                    }
                }

                return View(productWithPriceViewModels);
            }

            return View(new List<ProductWithPriceViewModel>());
        }
    }
}
