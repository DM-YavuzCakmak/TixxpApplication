using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public CounterController(
            ICounterService counterService,
            ICounterTranslationService counterTranslationService,
            ILanguageService languageService)
        {
            _counterService = counterService;
            _counterTranslationService = counterTranslationService;
            _languageService = languageService;
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
                    IsDeleted = c.IsDeleted
                };
            }).ToList();

            return View(list);
        }


        [HttpGet]
        public IActionResult GetById(long id)
        {
            var counter = _counterService.GetById(id);
            if (!counter.Success || counter.Data == null)
                return NotFound();

            var translations = _counterTranslationService.GetList(x => x.CounterId == id);
            return Json(new
            {
                Counter = counter.Data,
                Translations = translations.Success ? translations.Data : new List<CounterTranslationEntity>()
            });
        }

        [HttpPost]
        public IActionResult Update([FromBody] CounterEntity model)
        {
            if (model == null) return BadRequest("Invalid data.");

            var result = _counterService.Update(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult UpdateTranslation([FromBody] CounterTranslationEntity model)
        {
            if (model == null) return BadRequest("Invalid data.");
            if (model.Id > 0)
            {
                var result = _counterTranslationService.Update(model);
                return Json(new { success = result.Success, message = result.Message });
            }
            else
            {
                var result = _counterTranslationService.Add(model);
                return Json(new { success = result.Success, message = result.Message });
            }
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var counter = _counterService.GetById(id);
            if (!counter.Success || counter.Data == null)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            var entity = counter.Data;
            entity.IsDeleted = true;

            var result = _counterService.Update(entity);

            // ✅ CounterTranslation kayıtlarını da sil
            var translations = _counterTranslationService.GetList(x => x.CounterId == id);
            if (translations.Success && translations.Data != null)
            {
                foreach (var tr in translations.Data)
                {
                    tr.IsDeleted = true;
                    _counterTranslationService.Update(tr);
                }
            }

            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
