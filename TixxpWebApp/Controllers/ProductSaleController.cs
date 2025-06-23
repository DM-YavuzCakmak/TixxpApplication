using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.WebApp.Models.ProductPrice;

namespace Tixxp.WebApp.Controllers
{
    public class ProductSaleController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductPriceService _productPriceService;
        private readonly ICurrencyTypeService _currencyTypeService;

        public ProductSaleController(IProductService productService, IProductPriceService productPriceService, ICurrencyTypeService currencyTypeService)
        {
            _productService = productService;
            _productPriceService = productPriceService;
            _currencyTypeService = currencyTypeService;
        }

        public IActionResult Index()
        {
            var productResult = _productService.GetAll();
            var priceResult = _productPriceService.GetAll();

            if (productResult.Success && priceResult.Success)
            {
                List<ProductWithPriceViewModel> productWithPriceViewModels = (from product in productResult.Data
                                                                              join price in priceResult.Data on product.Id equals price.ProductId into pp
                                                                              from price in pp.DefaultIfEmpty()
                                                                              select new ProductWithPriceViewModel
                                                                              {
                                                                                  ProductId = product.Id,
                                                                                  Name = product.Name,
                                                                                  Code = product.Code,
                                                                                  CurrencyTypeId = price.CurrencyTypeId,
                                                                                  CurrencyTypeSymbol = "",
                                                                                  Price = price?.Price ?? 0,
                                                                                  VatRate = price?.VatRate ?? 0
                                                                              }).ToList();

                if (productWithPriceViewModels.Any())
                {
                    var currenctTypeList = _currencyTypeService.GetList(x => productWithPriceViewModels.Select(v => v.CurrencyTypeId).Contains(x.Id));
                    if (currenctTypeList.Success && currenctTypeList.Data.Any())
                    {
                        foreach (var productWithPriceViewModel in productWithPriceViewModels)
                        {
                            productWithPriceViewModel.CurrencyTypeSymbol = currenctTypeList.Data.Where(x => x.Id == productWithPriceViewModel.CurrencyTypeId).Select(x => x.Symbol).FirstOrDefault();
                        }
                    }
                }

                return View(productWithPriceViewModels);
            }

            return View(new List<ProductWithPriceViewModel>());
        }
    }
}
