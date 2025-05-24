using Microsoft.AspNetCore.Mvc;
using Tixxp.Entities.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.CurrencyType;
using Tixxp.Core.Utilities.Results.Concrete;
using Tixxp.Entities.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Abstract.Product;

namespace Tixxp.WebApp.Controllers
{
    public class ProductPriceController : Controller
    {
        private readonly IProductPriceRepository _productPriceRepository;
        private readonly ICurrencyTypeRepository _currencyTypeRepository;
        private readonly IProductRepository _productRepository;

        public ProductPriceController(
            IProductPriceRepository productPriceRepository,
            ICurrencyTypeRepository currencyTypeRepository,
            IProductRepository productRepository)
        {
            _productPriceRepository = productPriceRepository;
            _currencyTypeRepository = currencyTypeRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var allProducts = _productRepository.GetList(x => !x.IsDeleted).ToList();
            var pricedProductIds = _productPriceRepository.GetList(x => !x.IsDeleted)
                                     .Select(x => x.ProductId)
                                     .Distinct()
                                     .ToList();

            var productsWithoutPrice = allProducts
                .Where(p => !pricedProductIds.Contains(p.Id))
                .ToList();

            var productPriceList = _productPriceRepository
                .GetListWithInclude(x => !x.IsDeleted, x => x.Product).ToList();


            foreach (var productPriceEntity in productPriceList)
            {
                productPriceEntity.CurrencyType = _currencyTypeRepository.Get(x => x.Id == productPriceEntity.CurrencyTypeId);
            }

            var currencyTypes = _currencyTypeRepository.GetList(x => !x.IsDeleted);

            ViewBag.ProductsWithoutPrice = productsWithoutPrice;
            ViewBag.CurrencyTypes = currencyTypes.ToList();

            return View(productPriceList);
        }

        [HttpGet]
        public JsonResult GetById(long id)
        {
            var result = _productPriceRepository.GetListWithInclude(x => x.Id == id, x => x.Product);
            if (result.Any())
            {
                foreach (var productPriceEntity in result)
                {
                    productPriceEntity.CurrencyType = _currencyTypeRepository.Get(x => x.Id == productPriceEntity.CurrencyTypeId);
                }
            }
            return Json(result.FirstOrDefault());
        }

        [HttpPost]
        public JsonResult Save(ProductPriceEntity model)
        {
            if (model == null)
                return Json(new { success = false, message = "Geçersiz veri." });

            if (model.Id == 0)
            {
                model.CreatedBy = 6; // Giriş yapan kullanıcı ID
                model.Created_Date = DateTime.Now;
                model.IsDeleted = false;

                var result = _productPriceRepository.AddAndReturn(model);
                if (result != null)
                {
                    return Json(new { success = true, message = "Fiyat başarıyla eklendi." });
                }

                return Json(new { success = false, message = "Fiyat başarıyla eklenemedi." });
            }
            else
            {
                var existing = _productPriceRepository.Get(x => x.Id == model.Id);
                if (existing == null)
                    return Json(new { success = false, message = "Fiyat kaydı bulunamadı." });

                existing.CurrencyTypeId = model.CurrencyTypeId;
                existing.Price = model.Price;
                existing.VatRate = model.VatRate;
                existing.UpdatedBy = 6; // Giriş yapan kullanıcı ID
                existing.Updated_Date = DateTime.Now;

                _productPriceRepository.Update(existing);
                return Json(new { success = true, message = "Fiyat başarıyla güncellendi." });
            }
        }

        [HttpPost]
        public JsonResult Update(ProductPriceEntity model)
        {
            if (model == null || model.Id == 0)
                return Json(new { success = false, message = "Geçersiz veri." });

            var existing = _productPriceRepository.Get(x => x.Id == model.Id);
            if (existing == null)
                return Json(new { success = false, message = "Fiyat kaydı bulunamadı." });

            existing.CurrencyTypeId = model.CurrencyTypeId;
            existing.Price = model.Price;
            existing.VatRate = model.VatRate;
            existing.UpdatedBy = 6; // Giriş yapan kullanıcı ID
            existing.Updated_Date = DateTime.Now;

            _productPriceRepository.Update(existing);
            return Json(new { success = true, message = "Fiyat başarıyla güncellendi." });
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            var existing = _productPriceRepository.Get(x => x.Id == id);
            if (existing == null)
                return Json(new { success = false, message = "Fiyat kaydı bulunamadı." });

            existing.IsDeleted = true;
            existing.UpdatedBy = 6;
            existing.Updated_Date = DateTime.Now;

            _productPriceRepository.Update(existing);
            return Json(new { success = true, message = "Fiyat başarıyla silindi." });
        }
    }
}
