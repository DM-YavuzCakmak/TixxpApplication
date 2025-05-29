using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.Agency;
using Tixxp.Business.Services.Abstract.AgencyContract;
using Tixxp.Entities.Agency;
using Tixxp.Entities.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Abstract.PriceCategory;

namespace Tixxp.WebApp.Controllers.AgencyContract
{
    public class AgencyContractController : Controller
    {
        private readonly IAgencyService _agencyService;
        private readonly IAgencyContractService _agencyContractService;
        private readonly IPriceCategoryRepository _priceCategoryRepository;

        public AgencyContractController(
            IAgencyService agencyService,
            IAgencyContractService agencyContractService,
            IPriceCategoryRepository priceCategoryRepository)
        {
            _agencyService = agencyService;
            _agencyContractService = agencyContractService;
            _priceCategoryRepository = priceCategoryRepository;
        }

        public IActionResult Index()
        {
            // Tüm aktif acenteler
            var allAgencies = _agencyService.GetList(x => !x.IsDeleted).Data.ToList();

            // Aktif sözleşmeler
            var agencyContractList = _agencyContractService
                .GetListWithInclude(x => !x.IsDeleted, x => x.PriceCategory).Data.ToList();

            // Sözleşmelere Agency bilgisi dahil et
            foreach (var contract in agencyContractList)
            {
                contract.Agency = _agencyService.GetFirstOrDefault(x => x.Id == contract.AgencyId).Data;
            }

            // ViewBag: Tüm acenteler ve fiyat kategorileri (dropdown için)
            ViewBag.Agencies = allAgencies;
            ViewBag.PriceCategories = _priceCategoryRepository.GetList(x => !x.IsDeleted).ToList();

            return View(agencyContractList);
        }

        [HttpPost]
        public IActionResult Add([FromBody] AgencyContractCreateModel model)
        {
            if (model == null || model.PriceCategoryId <= 0)
            {
                return Json(new { isSuccess = false, message = "Geçersiz veri." });
            }

            var existing = _agencyContractService.GetFirstOrDefault(x =>
                x.AgencyId == model.AgencyId &&
                x.PriceCategoryId == model.PriceCategoryId &&
                !x.IsDeleted
            ).Data;

            if (existing != null)
            {
                return Json(new { isSuccess = false, message = "Bu acente için bu fiyat kategorisi zaten tanımlı." });
            }

            var result = _agencyContractService.Add(new AgencyContractEntity
            {
                AgencyId = model.AgencyId,
                PriceCategoryId = model.PriceCategoryId,
                IsDeleted = false,
                Created_Date = DateTime.Now
            });

            return Json(new
            {
                isSuccess = result.Success,
                message = result.Message ?? (result.Success ? "Kayıt başarılı." : "Kayıt sırasında hata oluştu.")
            });
        }

        [HttpPost("AgencyContract/Delete/{id}")]
        public IActionResult Delete(long id)
        {
            var contract = _agencyContractService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted).Data;

            if (contract == null)
                return Json(new { isSuccess = false, message = "Kayıt bulunamadı." });

            contract.IsDeleted = true;
            contract.Updated_Date = DateTime.Now;

            var result = _agencyContractService.Update(contract);

            return Json(new
            {
                isSuccess = result.Success,
                message = result.Message ?? "Silme işlemi başarıyla tamamlandı."
            });
        }
    }

    public class AgencyContractCreateModel
    {
        public long AgencyId { get; set; }
        public int PriceCategoryId { get; set; }
    }
}
