using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using Tixxp.Business.Services.Abstract.Counter;
using Tixxp.Business.Services.Abstract.CounterTranslation;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Entities.Counter;
using Tixxp.Entities.CounterTranslation;
using Tixxp.WebApp.Models.Counter;

namespace Tixxp.WebApp.Controllers
{
    [Authorize]
    public class CounterController : Controller
    {
        private readonly ICounterService _counterService;
        private readonly ICounterTranslationService _counterTranslationService;
        private readonly ILanguageService _languageService;
        private readonly IStringLocalizer<CounterController> _localizer;

        public CounterController(
            ICounterService counterService,
            ICounterTranslationService counterTranslationService,
            ILanguageService languageService,
            IStringLocalizer<CounterController> localizer)
        {
            _counterService = counterService;
            _counterTranslationService = counterTranslationService;
            _languageService = languageService;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langRes.Success ? langRes.Data?.Id : null;

            var counters = _counterService.GetAll();
            if (!counters.Success || counters.Data == null)
                return View(new List<CounterListVm>());

            var list = counters.Data.Select(c =>
            {
                string name = $"#{c.Id}";
                if (languageId.HasValue)
                {
                    var tr = _counterTranslationService.GetFirstOrDefault(
                        x => x.CounterId == c.Id && x.LanguageId == languageId.Value);

                    if (tr.Success && tr.Data != null)
                        name = tr.Data.Name;
                }

                return new CounterListVm
                {
                    Id = c.Id,
                    Code = c.OkcFiscalSerialNumber,
                    Name = name,
                    OkcFiscalSerialNumber = c.OkcFiscalSerialNumber,
                    IpAddress = c.IpAddress,
                    Port = c.Port,
                    Version = c.Version,
                    IsOkcIntegrated = c.IsOkcIntegrated,
                    TsmOpen = c.TsmOpen,
                    GmpOpen = c.GmpOpen,
                    OtpVerification = c.OtpVerification,
                    OkcBrand = c.OkcBrand,
                    OkcPassword = c.OkcPassword,
                    IsDeleted = c.IsDeleted
                };
            }).ToList();

            return View(list);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            if (id <= 0)
                return BadRequest(_localizer["counter.ERROR.INVALID_ID"]);

            var counter = _counterService.GetById(id);
            if (!counter.Success || counter.Data == null)
                return NotFound(_localizer["counter.ERROR.NOT_FOUND"]);

            var translations = _counterTranslationService.GetList(x => x.CounterId == id);

            return Json(new
            {
                counter = counter.Data,
                translations = translations.Success ? translations.Data : new List<CounterTranslationEntity>()
            });
        }

        [HttpPost]
        public IActionResult Update([FromBody] CounterEntity model)
        {
            if (model == null)
                return Json(new { success = false, message = _localizer["counter.ERROR.INVALID_DATA"] });

            var existingResult = _counterService.GetById(model.Id);
            if (!existingResult.Success || existingResult.Data == null)
                return Json(new { success = false, message = _localizer["counter.ERROR.NOT_FOUND"] });

            var entity = existingResult.Data;

            entity.OkcFiscalSerialNumber = model.OkcFiscalSerialNumber;
            entity.IpAddress = model.IpAddress;
            entity.Port = model.Port;
            entity.Version = model.Version;
            entity.IsOkcIntegrated = model.IsOkcIntegrated;
            entity.OkcBrand = model.OkcBrand;
            entity.OkcPassword = model.OkcPassword;
            entity.TsmOpen = model.TsmOpen;
            entity.GmpOpen = model.GmpOpen;
            entity.OtpVerification = model.OtpVerification;

            var result = _counterService.Update(entity);

            if (!result.Success)
                return Json(new { success = false, message = _localizer["counter.ERROR.UPDATE_FAILED"] });

            return Json(new { success = true, message = _localizer["counter.SUCCESS.UPDATE"] });
        }

        [HttpPost]
        public IActionResult UpdateTranslation([FromBody] CounterTranslationEntity model)
        {
            if (model == null)
                return Json(new { success = false, message = _localizer["counter.ERROR.INVALID_DATA"] });

            if (model.Id > 0)
            {
                var result = _counterTranslationService.Update(model);
                return Json(new
                {
                    success = result.Success,
                    message = result.Success
                        ? _localizer["counter.SUCCESS.TRANSLATION_UPDATED"]
                        : _localizer["counter.ERROR.TRANSLATION_UPDATE_FAILED"]
                });
            }
            else
            {
                var result = _counterTranslationService.Add(model);
                return Json(new
                {
                    success = result.Success,
                    message = result.Success
                        ? _localizer["counter.SUCCESS.TRANSLATION_ADDED"]
                        : _localizer["counter.ERROR.TRANSLATION_ADD_FAILED"]
                });
            }
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            if (id <= 0)
                return Json(new { success = false, message = _localizer["counter.ERROR.INVALID_ID"] });

            var counter = _counterService.GetById(id);
            if (!counter.Success || counter.Data == null)
                return Json(new { success = false, message = _localizer["counter.ERROR.NOT_FOUND"] });

            var entity = counter.Data;
            entity.IsDeleted = true;

            var result = _counterService.Update(entity);
            if (!result.Success)
                return Json(new { success = false, message = _localizer["counter.ERROR.DELETE_FAILED"] });

            var translations = _counterTranslationService.GetList(x => x.CounterId == id);
            if (translations.Success && translations.Data != null)
            {
                foreach (var t in translations.Data)
                {
                    t.IsDeleted = true;
                    _counterTranslationService.Update(t);
                }
            }

            return Json(new { success = true, message = _localizer["counter.SUCCESS.DELETE"] });
        }
    }
}
