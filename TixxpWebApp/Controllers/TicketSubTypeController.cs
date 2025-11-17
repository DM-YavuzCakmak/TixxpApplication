using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Tixxp.Business.Services.Abstract.TicketSubType;
using Tixxp.Business.Services.Abstract.TicketType;
using Tixxp.Entities.TicketSubType;

namespace Tixxp.WebApp.Controllers
{
    public class TicketSubTypeController : Controller
    {
        private readonly ITicketSubTypeService _ticketSubTypeService;
        private readonly ITicketTypeService _ticketTypeService;
        private readonly IStringLocalizer<TicketSubTypeController> _stringLocalizer;

        public TicketSubTypeController(
            ITicketSubTypeService ticketSubTypeService, 
            ITicketTypeService ticketTypeService,
            IStringLocalizer<TicketSubTypeController> stringLocalizer)
        {
            _ticketSubTypeService = ticketSubTypeService;
            _ticketTypeService = ticketTypeService;
            _stringLocalizer = stringLocalizer;
        }

        public IActionResult Index()
        {
            var subTypes = _ticketSubTypeService.GetListWithInclude(x => !x.IsDeleted, x => x.TicketType).Data;
            ViewBag.TicketTypes = _ticketTypeService.GetList(x => !x.IsDeleted).Data;
            return View(subTypes);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _ticketSubTypeService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (result.Success)
            {
                return Json(new
                {
                    id = result.Data.Id,
                    name = result.Data.Name,
                    description = result.Data.Description,
                    ticketTypeId = result.Data.TicketTypeId
                });
            }

            return BadRequest(new { success = false, message = result.Message });
        }

        [HttpPost]
        public IActionResult Create([FromBody] TicketSubTypeEntity model)
        {
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;

            var result = _ticketSubTypeService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] TicketSubTypeEntity model)
        {
            var existing = _ticketSubTypeService.GetFirstOrDefault(x => x.Id == model.Id);
            if (!existing.Success)
                return Json(new { success = false, message = _stringLocalizer["ticketSubType.INDEX.RECORD_NOT_FOUND"].Value });

            existing.Data.Name = model.Name;
            existing.Data.Description = model.Description;
            existing.Data.TicketTypeId = model.TicketTypeId;
            existing.Data.Updated_Date = DateTime.Now;

            var result = _ticketSubTypeService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var existing = _ticketSubTypeService.GetFirstOrDefault(x => x.Id == id);
            if (!existing.Success)
                return Json(new { success = false, message = _stringLocalizer["ticketSubType.INDEX.RECORD_NOT_FOUND"].Value });

            existing.Data.IsDeleted = true;
            var result = _ticketSubTypeService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }
    }
}
