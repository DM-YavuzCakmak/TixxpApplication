using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Core.Utilities.Enums.ReservationStatusEnum;
using Tixxp.Entities.Reservation;

namespace Tixxp.WebApp.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        public IActionResult Index()
        {
            var reservations = _reservationService.GetList(x => !x.IsDeleted).Data;
            return View(reservations);
        }

        public IActionResult CancelReservation()
        {
            List<ReservationEntity> cancelledReservations = _reservationService.GetList(x => true).Data;
            return View("CancelReservation", cancelledReservations);
        }

        [HttpGet]
        public IActionResult GetById(long id)
        {
            var result = _reservationService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return BadRequest(new { success = false, message = result.Message });

            return Json(new
            {
                id = result.Data.Id,
                name = result.Data.Name,
                surname = result.Data.Surname,
                email = result.Data.Email,
                phone = result.Data.Phone,
                numberOfTickets = result.Data.NumberOfTickets,
                totalPrice = result.Data.TotalPrice,
                statusId = result.Data.StatusId
            });
        }

        [HttpPost]
        public IActionResult Create([FromBody] ReservationEntity model)
        {
            model.CreatedBy = 6; // sabit kullanıcı id
            model.Created_Date = DateTime.Now;
            model.IsDeleted = false;

            var result = _reservationService.Add(model);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Update([FromBody] ReservationEntity model)
        {
            var existing = _reservationService.GetFirstOrDefault(x => x.Id == model.Id && !x.IsDeleted);
            if (!existing.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            existing.Data.Name = model.Name;
            existing.Data.Surname = model.Surname;
            existing.Data.Email = model.Email;
            existing.Data.Phone = model.Phone;
            existing.Data.NumberOfTickets = model.NumberOfTickets;
            existing.Data.TotalPrice = model.TotalPrice;
            existing.Data.StatusId = model.StatusId;
            existing.Data.Updated_Date = DateTime.Now;

            var result = _reservationService.Update(existing.Data);
            return Json(new { success = result.Success, message = result.Message });
        }

        [HttpPost]
        public IActionResult Delete(long id)
        {
            var result = _reservationService.GetFirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (!result.Success)
                return Json(new { success = false, message = "Kayıt bulunamadı." });

            result.Data.IsDeleted = true;
            result.Data.Updated_Date = DateTime.Now;

            var updateResult = _reservationService.Update(result.Data);
            return Json(new { success = updateResult.Success, message = updateResult.Message });
        }


    }
}
