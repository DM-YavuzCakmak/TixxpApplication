using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Tixxp.Business.Services.Abstract.CurrenctUser;
using Tixxp.Entities.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Abstract.Product;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductTranslation;
using Tixxp.WebApp.Models.ProductPrice;

namespace Tixxp.WebApp.Controllers
{
    public class ProductPriceController : Controller
    {
        private readonly ICurrentUser _currentUser;

        private readonly IProductPriceRepository _priceRepo;
        private readonly IProductSaleDetailRepository _productSaleDetailRepository;
        private readonly ICurrencyTypeRepository _currencyRepo;
        private readonly IProductRepository _productRepo;
        private readonly IProductTranslationRepository _translationRepo;

        private readonly IStringLocalizer<ProductPriceController> _localization;

        public ProductPriceController(
            IProductPriceRepository priceRepo,
            ICurrencyTypeRepository currencyRepo,
            IProductRepository productRepo,
            IProductTranslationRepository translationRepo,
            ICurrentUser currentUser,
            IStringLocalizer<ProductPriceController> localization,
            IProductSaleDetailRepository productSaleDetailRepository)
        {
            _priceRepo = priceRepo;
            _currencyRepo = currencyRepo;
            _productRepo = productRepo;
            _translationRepo = translationRepo;
            _currentUser = currentUser;
            _localization = localization;
            _productSaleDetailRepository = productSaleDetailRepository;
        }

        // --------------------------------------------------------------------
        // INDEX
        // --------------------------------------------------------------------
        public IActionResult Index()
        {
            var culture = CultureInfo.CurrentUICulture.Name;

            // -----------------------------------------
            // 1) Fiyatlı ürünleri + product include
            // -----------------------------------------
            var priceEntities = _priceRepo.GetListWithInclude(
                x => !x.IsDeleted,
                x => x.Product
            );

            var viewModelList = new List<ProductWithPriceViewModel>();

            foreach (var p in priceEntities)
            {
                var currency = _currencyRepo.Get(x => x.Id == p.CurrencyTypeId);
                var localizedName = GetLocalizedProductName(p.ProductId, culture);

                viewModelList.Add(new ProductWithPriceViewModel
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    ProductName = localizedName,
                    Code = p.Product?.Code ?? "",
                    CurrencyTypeId = p.CurrencyTypeId,
                    CurrencyName = currency?.Name ?? "",
                    CurrencyTypeSymbol = currency?.Symbol ?? "",
                    Price = p.Price,
                    VatRate = p.VatRate
                });
            }

            // -----------------------------------------
            // 2) Tüm Ürünleri Listeden Gönder (Fiyatı olsa da olmasa da)
            // -----------------------------------------
            var allProducts = _productRepo.GetList(x => !x.IsDeleted).ToList();

            foreach (var product in allProducts)
            {
                product.Name = GetLocalizedProductName(product.Id, culture);
            }

            ViewBag.Products = allProducts; // <-- artık tüm ürünler geliyor
            ViewBag.CurrencyTypes = _currencyRepo.GetList(x => !x.IsDeleted).ToList();

            return View(viewModelList);
        }


        // --------------------------------------------------------------------
        // PRODUCT NAME LOCALIZATION
        // --------------------------------------------------------------------
        private string GetLocalizedProductName(long productId, string culture)
        {
            // Önce aktif dil
            var name = _translationRepo.Get(
                x => x.ProductId == productId &&
                     x.Language.Code == culture
            );

            if (name != null)
                return name.Name;

            // Sonra TR fallback
            var tr = _translationRepo.Get(
                x => x.ProductId == productId &&
                     x.Language.Code == "tr-TR"
            );

            if (tr != null)
                return tr.Name;

            // Herhangi bir çeviri yoksa
            var any = _translationRepo.Get(x => x.ProductId == productId);
            return any?.Name ?? _localization["productPriceController.NO_NAME"].Value.ToString();
        }

        // --------------------------------------------------------------------
        // GET BY ID (MODAL)
        // --------------------------------------------------------------------
        [HttpGet]
        public JsonResult GetById(long id)
        {
            var entity = _priceRepo
                .GetListWithInclude(x => x.Id == id, x => x.Product)
                .FirstOrDefault();

            if (entity == null)
            {
                return Json(new
                {
                    success = false,
                    message = _localization["productPriceController.ERROR_NOT_FOUND"].Value.ToString()
                });
            }

            var culture = CultureInfo.CurrentUICulture.Name;

            return Json(new
            {
                success = true,
                id = entity.Id,
                productName = GetLocalizedProductName(entity.ProductId, culture),
                currencyTypeId = entity.CurrencyTypeId,
                price = entity.Price,
                vatRate = entity.VatRate
            });
        }

        // --------------------------------------------------------------------
        // SAVE (ADD)
        // --------------------------------------------------------------------
        [HttpPost]
        public JsonResult Save(ProductPriceEntity model)
        {
            // Daha önce bu ürün + para birimi için kayıt var mı?
            var existing = _priceRepo.Get(
                x => x.ProductId == model.ProductId && x.CurrencyTypeId == model.CurrencyTypeId
            );

            if (existing != null)
            {
                return Json(new
                {
                    success = false,
                    message = _localization["productPriceController.ADD_DUPLICATE"].Value.ToString()
                });
            }

            model.CreatedBy = _currentUser.UserId ?? 0;
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;

            var result = _priceRepo.AddAndReturn(model);

            if (result != null)
            {
                return Json(new
                {
                    success = true,
                    message = _localization["productPriceController.ADD_SUCCESS"].Value.ToString()
                });
            }

            return Json(new
            {
                success = false,
                message = _localization["productPriceController.ADD_FAIL"].Value.ToString()
            });
        }

        // --------------------------------------------------------------------
        // UPDATE
        // --------------------------------------------------------------------
        [HttpPost]
        public JsonResult Update(ProductPriceEntity model)
        {
            var entity = _priceRepo.Get(x => x.Id == model.Id);

            if (entity == null)
            {
                return Json(new
                {
                    success = false,
                    message = _localization["productPriceController.ERROR_NOT_FOUND"].Value.ToString()
                });
            }

            // Eğer satışa konu olmuşsa eski fiyatı güncellemeyelim — yeni satır açalım
            var existSale = _productSaleDetailRepository
                .GetFirstOrDefault(x => x.ProductPriceId == model.Id);

            if (existSale != null)
            {

                var newPrice = new ProductPriceEntity
                {
                    ProductId = entity.ProductId,
                    CurrencyTypeId = model.CurrencyTypeId,
                    Price = model.Price,
                    VatRate = model.VatRate,
                    CreatedBy = _currentUser.UserId.Value,
                    Created_Date = DateTime.Now,
                    IsDeleted = false
                };

                _priceRepo.Add(newPrice);

                entity.IsDeleted = true;
                entity.UpdatedBy = _currentUser.UserId;
                entity.Updated_Date = DateTime.Now;

                _priceRepo.Update(newPrice);

            }
            else
            {
                entity.CurrencyTypeId = model.CurrencyTypeId;
                entity.Price = model.Price;
                entity.VatRate = model.VatRate;
                entity.UpdatedBy = _currentUser.UserId ?? 0;
                entity.Updated_Date = DateTime.Now;

                _priceRepo.Update(entity);
            }

            return Json(new
            {
                success = true,
                message = _localization["productPriceController.UPDATE_SUCCESS"].Value.ToString()
            });
        }

        // --------------------------------------------------------------------
        // DELETE
        // --------------------------------------------------------------------
        [HttpPost]
        public JsonResult Delete(long id)
        {
            var entity = _priceRepo.Get(x => x.Id == id);
            if (entity == null)
            {
                return Json(new
                {
                    success = false,
                    message = _localization["productPriceController.ERROR_NOT_FOUND"].Value.ToString()
                });
            }

            entity.IsDeleted = true;
            entity.UpdatedBy = _currentUser.UserId;
            entity.Updated_Date = DateTime.Now;

            _priceRepo.Update(entity);

            return Json(new
            {
                success = true,
                message = _localization["productPriceController.DELETE_SUCCESS"].Value.ToString()
            });
        }
    }
}
