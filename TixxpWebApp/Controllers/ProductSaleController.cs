using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.Product;
using Tixxp.WebApp.Models;
using Tixxp.WebApp.Models.ProductPrice; // ViewModel burada olacak

namespace Tixxp.WebApp.Controllers
{
    public class ProductSaleController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductPriceService _productPriceService;

        public ProductSaleController(IProductService productService, IProductPriceService productPriceService)
        {
            _productService = productService;
            _productPriceService = productPriceService;
        }

        public IActionResult Index()
        {
            var productResult = _productService.GetAll();
            var priceResult = _productPriceService.GetAll();

            if (productResult.Success && priceResult.Success)
            {
                var viewModel = (from product in productResult.Data
                                 join price in priceResult.Data on product.Id equals price.ProductId into pp
                                 from price in pp.DefaultIfEmpty()
                                 select new ProductWithPriceViewModel
                                 {
                                     ProductId = product.Id,
                                     Name = product.Name,
                                     Code = product.Code,
                                     Price = price?.Price ?? 0,
                                     VatRate = price?.VatRate ?? 0
                                 }).ToList();

                return View(viewModel);
            }

            return View(new List<ProductWithPriceViewModel>());
        }
    }
}
