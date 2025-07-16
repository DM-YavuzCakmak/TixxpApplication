using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Entities.Product;
using Tixxp.WebApp.Models.Product;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Core.Utilities.Results.Concrete;
using Tixxp.Infrastructure.DataAccess.Abstract.Language;
using Tixxp.Entities.Language;

namespace Tixxp.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductTranslationService _productTranslationService;
        private readonly ILanguageRepository _languageRepository;

        public ProductController(IProductService productService, IProductTranslationService productTranslationService, ILanguageRepository languageRepository)
        {
            _productService = productService;
            _productTranslationService = productTranslationService;
            _languageRepository = languageRepository;
        }

        public IActionResult Index()
        {
            var productsResult = _productService.GetAll();
            var translationsResult = _productTranslationService.GetListWithInclude(x => !x.IsDeleted, x => x.Language);

            if (!productsResult.Success || !translationsResult.Success)
                return View(new List<ProductWithTranslationViewModel>());

            var viewModelList = productsResult.Data.Select(p => new ProductWithTranslationViewModel
            {
                Product = p,
                Translations = translationsResult.Data
                    .Where(t => t.ProductId == p.Id)
                    .ToList()
            }).ToList();

            return View(viewModelList);
        }

        [HttpGet("GetAllLanguage")]
        public IActionResult GetAllLanguage()
        {
            IList<LanguageEntity> languageEntities = _languageRepository.GetList(x => !x.IsDeleted);
            return Json(languageEntities.ToList());
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _productService.GetById(id);
            if (!result.Success || result.Data == null)
                return NotFound("Ürün bulunamadı.");

            return Json(result.Data);
        }

        [HttpPost]
        public IActionResult Save(ProductEntity model)
        {
            // Not: Bu değerleri oturumdan (CurrentUser vs) almak daha iyidir
            const long currentUserId = 6;
            var currentTime = DateTime.UtcNow;

            if (model.Id > 0)
            {
                model.UpdatedBy = currentUserId;
                model.Updated_Date = currentTime;
                return Json(_productService.Update(model));
            }

            model.CreatedBy = currentUserId;
            model.Created_Date = currentTime;
            return Json(_productService.Add(model));
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var productResult = _productService.GetById(id);
            if (!productResult.Success || productResult.Data == null)
                return Json(new { Success = false, Message = "Ürün bulunamadı." });

            var entity = productResult.Data;
            entity.IsDeleted = true;
            entity.UpdatedBy = 6; // TODO: Get from logged-in user
            entity.Updated_Date = DateTime.UtcNow;

            return Json(_productService.Update(entity));
        }
    }
}
