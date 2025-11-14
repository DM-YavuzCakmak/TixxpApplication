using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Tixxp.Business.Services.Abstract.CurrenctUser;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Core.Utilities.Results.Concrete;
using Tixxp.Entities.Language;
using Tixxp.Entities.Product;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Language;
using Tixxp.WebApp.Models.Product;

namespace Tixxp.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICurrentUser _currentUser;

        private readonly IProductService _productService;
        private readonly IProductTranslationService _productTranslationService;
        private readonly ILanguageRepository _languageRepository;
        private readonly IStringLocalizer<ProductController> _localizer;

        public ProductController(
            IProductService productService,
            IProductTranslationService productTranslationService,
            ILanguageRepository languageRepository,
            IStringLocalizer<ProductController> localizer,
            ICurrentUser currentUser)
        {
            _productService = productService;
            _productTranslationService = productTranslationService;
            _languageRepository = languageRepository; ;
            _localizer = localizer;
            _currentUser = currentUser;
        }

        // ----------------------------------------------------
        // INDEX
        // ----------------------------------------------------
        public IActionResult Index()
        {
            var productsResult = _productService.GetAll();
            var translationsResult = _productTranslationService.GetListWithInclude(
                x => !x.IsDeleted,
                x => x.Language
            );

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

        // ----------------------------------------------------
        // LANGUAGES
        // ----------------------------------------------------
        [HttpGet("GetAllLanguage")]
        public IActionResult GetAllLanguage()
        {
            var langs = _languageRepository.GetList(x => !x.IsDeleted);
            return Json(langs.ToList());
        }

        // ----------------------------------------------------
        // GET PRODUCT BY ID (Modal için düzenlendi)
        // ----------------------------------------------------
        [HttpGet("Product/GetById/{id}")]
        public IActionResult GetById(long id)
        {
            var productResult = _productService.GetById(id);
            if (!productResult.Success || productResult.Data == null)
                return Json(new ErrorResult(_localizer["product.ERROR.NOT_FOUND"]));

            var translations = _productTranslationService.GetListWithInclude(
                x => x.ProductId == id && !x.IsDeleted,
                x => x.Language
            );

            var response = new
            {
                Product = new
                {
                    Id = productResult.Data.Id,
                    Code = productResult.Data.Code
                },
                Translations = translations.Data.Select(t => new
                {
                    Name = t.Name,
                    Language = new { Code = t.Language.Code }
                })
            };

            return Json(response);
        }

        // ----------------------------------------------------
        // SAVE PRODUCT (Add / Update + Translations)
        // ----------------------------------------------------
        [HttpPost]
        public IActionResult Save(ProductEntity model)
        {
            if (model == null)
                return Json(new ErrorResult(_localizer["product.ERROR.MODEL_INVALID"]));

            // UserId — TODO: Oturumdan alınacak.
            var now = DateTime.UtcNow;

            // ----------------------------------------------
            // UPDATE
            // ----------------------------------------------
            if (model.Id > 0)
            {
                var existing = _productService.GetById(model.Id);
                if (!existing.Success || existing.Data == null)
                    return Json(new ErrorResult(_localizer["product.ERROR.NOT_FOUND"]));

                var entity = existing.Data;
                entity.Code = model.Code;
                entity.UpdatedBy = _currentUser.UserId;
                entity.Updated_Date = now;

                _productService.Update(entity);

                // --- TRANSLATION UPDATE ---
                foreach (var key in Request.Form.Keys.Where(k => k.StartsWith("Translations[")))
                {
                    var langCode = key.Replace("Translations[", "").Replace("]", "");
                    var name = Request.Form[key];

                    var lang = _languageRepository.Get(x => x.Code == langCode);
                    if (lang == null) continue;

                    var tr = _productTranslationService.GetFirstOrDefault(
                        x => x.ProductId == model.Id && x.LanguageId == lang.Id
                    );

                    if (tr.Success && tr.Data != null)
                    {
                        tr.Data.Name = name;
                        tr.Data.UpdatedBy = _currentUser.UserId;
                        tr.Data.Updated_Date = now;
                        _productTranslationService.Update(tr.Data);
                    }
                    else
                    {
                        var newTranslation = new ProductTranslationEntity
                        {
                            ProductId = model.Id,
                            LanguageId = lang.Id,
                            Name = name,
                            CreatedBy = _currentUser.UserId.Value,
                            Created_Date = now
                        };
                        _productTranslationService.Add(newTranslation);
                    }
                }

                return Json(new SuccessResult(_localizer["product.SUCCESS.SAVED"]));
            }

            // ----------------------------------------------
            // ADD NEW PRODUCT
            // ----------------------------------------------
            model.CreatedBy = _currentUser.UserId.Value;
            model.Created_Date = now;

            var addResult = _productService.AddAndReturn(model);

            if (!addResult.Success)
                return Json(new ErrorResult(_localizer["product.ERROR.SAVE_FAILED"]));

            long newProductId = addResult.Data.Id;

            // --- TRANSLATION ADD ---
            foreach (var key in Request.Form.Keys.Where(k => k.StartsWith("Translations[")))
            {
                var langCode = key.Replace("Translations[", "").Replace("]", "");
                var name = Request.Form[key];

                var lang = _languageRepository.Get(x => x.Code == langCode);
                if (lang == null) continue;

                var tr = new ProductTranslationEntity
                {
                    ProductId = newProductId,
                    LanguageId = lang.Id,
                    Name = name,
                    CreatedBy = _currentUser.UserId.Value,
                    Created_Date = now
                };

                _productTranslationService.Add(tr);
            }

            return Json(new SuccessResult(_localizer["product.SUCCESS.SAVED"]));
        }

        // ----------------------------------------------------
        // DELETE PRODUCT (Soft Delete)
        // ----------------------------------------------------
        [HttpPost("Product/Delete/{id}")]
        public IActionResult Delete(long id)
        {
            var result = _productService.GetById(id);
            if (!result.Success || result.Data == null)
                return Json(new ErrorResult(_localizer["product.ERROR.NOT_FOUND"]));

            var entity = result.Data;
            entity.IsDeleted = true;
            entity.UpdatedBy = _currentUser.UserId;
            entity.Updated_Date = DateTime.UtcNow;

            _productService.Update(entity);

            return Json(new SuccessResult(_localizer["product.SUCCESS.DELETED"]));
        }
    }
}
