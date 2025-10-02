using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using Tixxp.Business.Services.Abstract.ChannelTranslation;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.EventTicketPrice;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.PaymentType;
using Tixxp.Business.Services.Abstract.PaymentTypeTranslation;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Business.Services.Abstract.ReservationDetail;
using Tixxp.Business.Services.Abstract.ReservationProductDetail;
using Tixxp.Business.Services.Abstract.ReservationSaleInvoiceInfo;
using Tixxp.Business.Services.Abstract.ReservationStatusTranslation; // <-- eklendi
using Tixxp.Business.Services.Abstract.TicketStatus;
using Tixxp.Business.Services.Abstract.TicketStatusTranslation;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.Events;
using Tixxp.Entities.PaymentTypeTranslation;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Entities.ReservationProductDetail;
using Tixxp.Entities.ReservationSaleInvoiceInfo;
using Tixxp.Entities.ReservationStatusTranslation; // <-- gerekiyorsa (entity kullanmıyoruz ama namespace tutarlılığı için)
using Tixxp.Entities.TicketStatusTranslation;
using Tixxp.WebApp.Models.ReservationManagement;

namespace Tixxp.WebApp.Controllers
{
    public class ReservationManagementController : Controller
    {
        // === services ===
        private readonly IEventService _eventService;
        private readonly ICurrencyTypeService _currencyTypeService;
        private readonly IChannelTranslationService _channelTranslationService;
        private readonly IProductTranslationService _productTranslationService;
        private readonly IReservationProductDetailService _reservationProductDetailService;
        private readonly IReservationService _reservationService;
        private readonly IReservationSaleInvoiceInfoService _invoiceInfoService;
        private readonly IReservationDetailService _reservationDetailService;
        private readonly IPaymentTypeService _paymentTypeService;
        private readonly IPaymentTypeTranslationService _paymentTypeTranslationService;
        private readonly ILanguageService _languageService;
        private readonly IEventTicketPriceService _eventTicketPriceService;
        private readonly IReservationStatusTranslationService _reservationStatusTranslationService; // <-- eklendi
        private readonly ITicketStatusService _ticketStatusService;
        private readonly ITicketStatusTranslationService _ticketStatusTranslationService;

        private const long STATUS_CANCELLED = 3; // iptal durum id’n (sende farklıysa değiştir)

        public ReservationManagementController(
            IReservationService reservationService,
            IReservationSaleInvoiceInfoService invoiceInfoService,
            IReservationDetailService reservationDetailService,
            IPaymentTypeService paymentTypeService,
            IPaymentTypeTranslationService paymentTypeTranslationService,
            ILanguageService languageService,
            IEventTicketPriceService eventTicketPriceService,
            IReservationStatusTranslationService reservationStatusTranslationService,
            IReservationProductDetailService reservationProductDetailService,
            IProductTranslationService productTranslationService,
            ICurrencyTypeService currencyTypeService,
            IEventService eventService,
            ITicketStatusService ticketStatusService,
            ITicketStatusTranslationService ticketStatusTranslationService,
            IChannelTranslationService channelTranslationService)
        {
            _reservationService = reservationService;
            _invoiceInfoService = invoiceInfoService;
            _reservationDetailService = reservationDetailService;
            _paymentTypeService = paymentTypeService;
            _paymentTypeTranslationService = paymentTypeTranslationService;
            _languageService = languageService;
            _eventTicketPriceService = eventTicketPriceService;
            _reservationStatusTranslationService = reservationStatusTranslationService; // <-- eklendi
            _reservationProductDetailService = reservationProductDetailService;
            _productTranslationService = productTranslationService;
            _currencyTypeService = currencyTypeService;
            _eventService = eventService;
            _ticketStatusService = ticketStatusService;
            _ticketStatusTranslationService = ticketStatusTranslationService;
            _channelTranslationService = channelTranslationService;
        }

        // INDEX
        [HttpGet]
        public IActionResult Index()
        {
            var eventList = _eventService.GetList(x => !x.IsDeleted);
            ViewBag.EventList = eventList.Success && eventList.Data != null ? eventList.Data : Enumerable.Empty<EventEntity>();

            var lookups = BuildFilterLookups();
            ViewBag.Channels = lookups.Channels;
            ViewBag.Statuses = lookups.Statuses;      // dil bazlı
            ViewBag.PaymentTypes = lookups.PaymentTypes;  // dil bazlı
            ViewBag.CurrencyTypes = lookups.CurrencyTypes;
            return View();
        }

        // SEARCH (partial)
        [HttpGet]
        public IActionResult Search([FromQuery] ReservationListFilter filter)
        {
            NormalizeFilter(ref filter);

            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langRes.Success ? langRes.Data?.Id : null;

            var baseResult = _reservationService.GetListWithInclude(x =>
                !x.IsDeleted
                && (!filter.ChannelId.HasValue || x.ChannelId == filter.ChannelId.Value)
                && (!filter.StatusId.HasValue || x.StatusId == filter.StatusId.Value)
                && (!filter.StartDate.HasValue || x.Created_Date >= filter.StartDate.Value)
                && (!filter.EndDateExclusive.HasValue || x.Created_Date < filter.EndDateExclusive.Value)
                && (!filter.ReservationId.HasValue || x.Id == filter.ReservationId.Value),
                x => x.Channel,
                x => x.Currency.CurrencyType
            );

            var list = (baseResult.Success && baseResult.Data != null)
                ? baseResult.Data
                : new List<ReservationEntity>();

            var reservationIds = list.Select(r => r.Id).ToList();

            var invResult = _invoiceInfoService.GetList(x => reservationIds.Contains(x.ReservationId) && !x.IsDeleted);
            var invoiceList = (invResult.Success && invResult.Data != null)
                ? invResult.Data
                : new List<ReservationSaleInvoiceInfoEntity>();

            // 🔍 Email filtresi
            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                var q = filter.Email.Trim().ToLowerInvariant();
                var hitIds = invoiceList
                    .Where(i => !string.IsNullOrWhiteSpace(i.Email) && i.Email.ToLower().Contains(q))
                    .Select(i => i.ReservationId)
                    .Distinct()
                    .ToHashSet();

                list = list.Where(r => hitIds.Contains(r.Id)).ToList();
                reservationIds = list.Select(r => r.Id).ToList();
                invoiceList = invoiceList.Where(i => reservationIds.Contains(i.ReservationId)).ToList();
            }

            // 🔍 PaymentType filtresi
            if (filter.PaymentTypeId.HasValue)
            {
                var hitIds = invoiceList
                    .Where(i => i.PaymentTypeId == filter.PaymentTypeId.Value)
                    .Select(i => i.ReservationId)
                    .Distinct()
                    .ToHashSet();

                list = list.Where(r => hitIds.Contains(r.Id)).ToList();
                reservationIds = list.Select(r => r.Id).ToList();
                invoiceList = invoiceList.Where(i => reservationIds.Contains(i.ReservationId)).ToList();
            }

            var paymentTypeNames = GetPaymentTypeNames();
            var statusNames = GetReservationStatusNames();

            // ✅ ChannelTranslation ile isimlendirme
            var channelIds = list.Where(x => x.ChannelId != null).Select(x => x.ChannelId).Distinct().ToList();
            var channelTranslations = (languageId.HasValue)
                ? _channelTranslationService.GetList(x => channelIds.Contains(x.ChannelId) && x.LanguageId == languageId.Value)
                : _channelTranslationService.GetList(x => channelIds.Contains(x.ChannelId));

            var channelTrMap = (channelTranslations.Success && channelTranslations.Data != null)
                ? channelTranslations.Data.ToDictionary(ct => ct.ChannelId, ct => ct.Name)
                : new Dictionary<long, string>();

            var rows = list
                .OrderByDescending(r => r.Created_Date)
                .Select(r =>
                {
                    var inv = invoiceList.FirstOrDefault(i => i.ReservationId == r.Id);
                    return new ReservationListItemVm
                    {
                        Id = r.Id,
                        CreatedDate = r.Created_Date,
                        ChannelId = r.ChannelId,
                        ChannelName = (r.ChannelId != null && channelTrMap.TryGetValue(r.ChannelId, out var cn))
                                      ? cn : $"#{r.ChannelId}",
                        TotalPrice = r.TotalPrice ?? 0,
                        CurrencySymbol = r.Currency?.CurrencyType?.Symbol ?? "",
                        PaymentTypeId = inv?.PaymentTypeId,
                        PaymentTypeName = (inv?.PaymentTypeId != null && paymentTypeNames.TryGetValue(inv.PaymentTypeId.Value, out var ptn))
                                          ? ptn : (inv?.PaymentTypeId != null ? $"#{inv.PaymentTypeId}" : "-"),
                        StatusId = r.StatusId,
                        StatusName = ResolveStatusName(r.StatusId, statusNames),
                        Email = inv?.Email,
                        FullName = FormatFullName(inv?.Name, inv?.Surname)
                    };
                })
                .ToList();

            var total = rows.Count;
            var page = Math.Max(1, filter.Page);
            var size = Math.Clamp(filter.PageSize, 1, 200);
            var items = rows.Skip((page - 1) * size).Take(size).ToList();

            var vm = new ReservationListResultVm
            {
                Items = items,
                Page = page,
                PageSize = size,
                TotalCount = total
            };

            return PartialView("_ReservationList", vm);
        }



        [HttpGet]
        public IActionResult Detail(long id)
        {
            // 1) Dil -> languageId
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langRes.Success ? langRes.Data?.Id : null;

            // 2) Rezervasyon (Channel + Currency -> CurrencyType include)
            var rDr = _reservationService.GetFirstOrDefaultWithInclude(
                x => x.Id == id,
                x => x.Channel,
                x => x.Currency.CurrencyType
            );
            if (!rDr.Success || rDr.Data == null)
                return NotFound("Reservation not found.");

            // 3) Fatura/kişisel bilgi
            var invDr = _invoiceInfoService.GetFirstOrDefault(x => x.ReservationId == id);
            var inv = invDr.Success ? invDr.Data : null;

            // 4) Status & PaymentType & TicketStatus çevirileri
            var statusNames = GetReservationStatusNames();
            var paymentTypeNames = GetPaymentTypeNames();
            var ticketStatusNames = GetTicketStatusNames();

            // 5) ✅ ChannelTranslation (dil bazlı)
            string channelName = "-";
            if (rDr.Data.ChannelId > 0)
            {
                var ctDr = (languageId.HasValue)
                    ? _channelTranslationService.GetFirstOrDefault(x => x.ChannelId == rDr.Data.ChannelId && x.LanguageId == languageId.Value)
                    : null;

                channelName = (ctDr != null && ctDr.Success && ctDr.Data != null)
                    ? ctDr.Data.Name
                    : ($"#{rDr.Data.ChannelId}");
            }

            // 6) Bilet satırları (ReservationDetail → TicketSubType, TicketType, Tickets)
            var detDr = _reservationDetailService.GetListWithInclude(
                x => x.ReservationId == id && !x.IsDeleted,
                x => x.TicketSubType,
                x => x.TicketType,
                x => x.Tickets
            );

            var ticketRows = new List<ReservationDetailTicketVm>();
            if (detDr.Success && detDr.Data != null)
            {
                foreach (var line in detDr.Data)
                {
                    var row = new ReservationDetailTicketVm
                    {
                        TicketTypeId = line.TicketTypeId,
                        TicketTypeName = line.TicketType?.Name ?? $"#{line.TicketTypeId}",
                        TicketSubTypeId = line.TicketSubTypeId,
                        TicketSubTypeName = line.TicketSubType?.Name ?? (line.TicketSubTypeId > 0 ? $"#{line.TicketSubTypeId}" : "-"),
                        Piece = line.NumberOfTickets,
                        Tickets = line.Tickets?.Select(t => new TicketMiniVm
                        {
                            TicketId = t.Id,
                            StatusId = t.TicketStatusId,
                            StatusName = ticketStatusNames.TryGetValue(t.TicketStatusId, out var tsn) ? tsn : $"#{t.TicketStatusId}",
                            CheckInDate = t.CheckInDate,
                            CheckOutDate = t.CheckOutDate,
                            QrText = t.QrText
                        }).ToList() ?? new List<TicketMiniVm>()
                    };
                    ticketRows.Add(row);
                }
            }

            // 7) Ürün satırları (ReservationProductDetail → Product + ProductTranslation)
            var rpdDr = _reservationProductDetailService.GetListWithInclude(
                x => x.ReservationId == id && !x.IsDeleted,
                x => x.Product
            );
            var productLines = (rpdDr.Success && rpdDr.Data != null)
                ? rpdDr.Data.ToList()
                : new List<ReservationProductDetailEntity>();

            var productRows = new List<ReservationDetailProductVm>();
            if (productLines.Any())
            {
                var productIds = productLines.Where(p => p.ProductId > 0).Select(p => p.ProductId).Distinct().ToList();

                var trDr = (languageId.HasValue)
                    ? _productTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value && productIds.Contains(x.ProductId))
                    : _productTranslationService.GetList(x => !x.IsDeleted && productIds.Contains(x.ProductId));

                var translations = (trDr.Success && trDr.Data != null)
                    ? trDr.Data.ToList()
                    : new List<ProductTranslationEntity>();

                var trMap = translations.GroupBy(t => t.ProductId).ToDictionary(g => g.Key, g => g.First().Name);

                productRows = productLines.Select(p =>
                {
                    var nameFromTr = trMap.TryGetValue(p.ProductId, out var trName) ? trName : null;
                    var fallback = p.Product?.Code ?? $"#{p.ProductId}";
                    var finalName = string.IsNullOrWhiteSpace(nameFromTr) ? fallback : nameFromTr;

                    return new ReservationDetailProductVm
                    {
                        ProductId = p.ProductId,
                        ProductName = finalName,
                        Piece = p.Piece
                    };
                }).ToList();
            }

            // 8) PaymentTypeName
            string paymentTypeName =
                (inv?.PaymentTypeId != null && paymentTypeNames.TryGetValue(inv.PaymentTypeId.Value, out var ptn))
                    ? ptn
                    : (inv?.PaymentTypeId != null ? $"#{inv.PaymentTypeId}" : "-");

            // 9) ViewModel
            var vm = new ReservationDetailVm
            {
                ReservationId = rDr.Data.Id,
                CreatedDate = rDr.Data.Created_Date,
                StatusId = rDr.Data.StatusId,
                StatusName = ResolveStatusName(rDr.Data.StatusId, statusNames),
                ChannelId = rDr.Data.ChannelId,
                ChannelName = channelName, // ✅ Translation’dan geliyor
                TotalPrice = rDr.Data.TotalPrice ?? 0,
                CurrencySymbol = rDr.Data.Currency?.CurrencyType?.Symbol ?? "",
                InvoiceInfo = new ReservationInvoiceMiniVm
                {
                    Name = inv?.Name,
                    Surname = inv?.Surname,
                    Email = inv?.Email,
                    Phone = inv?.Phone,
                    PaymentTypeId = inv?.PaymentTypeId,
                    PaymentTypeName = paymentTypeName
                },
                Tickets = ticketRows,
                Products = productRows
            };

            return PartialView("_ReservationDetail", vm);
        }




        // CANCEL GET
        [HttpGet]
        public IActionResult Cancel(long id)
        {
            var rDr = _reservationService.GetFirstOrDefault(x => x.Id == id);
            if (!rDr.Success || rDr.Data == null) return NotFound("Reservation not found.");

            var vm = new ReservationCancelRequest { ReservationId = id };
            return PartialView("_ReservationCancel", vm);
        }

        // CANCEL POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel([FromForm] ReservationCancelRequest model)
        {
            if (model == null || model.ReservationId <= 0)
                return BadRequest("Invalid payload.");

            var rDr = _reservationService.GetFirstOrDefault(x => x.Id == model.ReservationId);
            if (!rDr.Success || rDr.Data == null) return NotFound("Reservation not found.");

            var r = rDr.Data;
            if (r.StatusId == STATUS_CANCELLED)
                return BadRequest("Reservation already cancelled.");

            r.StatusId = STATUS_CANCELLED;
            r.UpdatedBy = 6;
            r.Updated_Date = DateTime.UtcNow;

            var upRes = _reservationService.Update(r);
            if (!upRes.Success) return StatusCode(500, "Cancel failed.");

            // TODO: model.Reason logla
            return Ok(new { message = "Reservation cancelled." });
        }

        // CHANGE PAYMENT TYPE GET
        [HttpGet]
        public IActionResult ChangePaymentType(long id)
        {
            var rDr = _reservationService.GetFirstOrDefault(x => x.Id == id);
            if (!rDr.Success || rDr.Data == null) return NotFound("Reservation not found.");

            var invDr = _invoiceInfoService.GetFirstOrDefault(x => x.ReservationId == id);
            var inv = invDr.Success ? invDr.Data : null;

            var paymentTypes = BuildFilterLookups().PaymentTypes;

            var vm = new ChangePaymentTypeVm
            {
                ReservationId = id,
                CurrentPaymentTypeId = inv?.PaymentTypeId,
                PaymentTypeOptions = paymentTypes.ToList()
            };

            return PartialView("_ChangePaymentType", vm);
        }

        // CHANGE PAYMENT TYPE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePaymentType([FromForm] ChangePaymentTypeRequest model)
        {
            if (model == null || model.ReservationId <= 0 || model.PaymentTypeId <= 0)
                return BadRequest("Invalid payload.");

            var invDr = _invoiceInfoService.GetFirstOrDefault(x => x.ReservationId == model.ReservationId);
            if (!invDr.Success) return StatusCode(500, "Invoice info read failed.");

            if (invDr.Data == null)
            {
                var newInv = new ReservationSaleInvoiceInfoEntity
                {
                    ReservationId = model.ReservationId,
                    PaymentTypeId = model.PaymentTypeId,
                    CreatedBy = 6
                };
                var addRes = _invoiceInfoService.Add(newInv);
                if (!addRes.Success) return StatusCode(500, "Payment type update failed (create).");
            }
            else
            {
                var inv = invDr.Data;
                inv.PaymentTypeId = model.PaymentTypeId;
                inv.UpdatedBy = 6;
                inv.Updated_Date = DateTime.UtcNow;

                var upRes = _invoiceInfoService.Update(inv);
                if (!upRes.Success) return StatusCode(500, "Payment type update failed (update).");
            }

            return Ok(new { message = "Payment type updated." });
        }

        // RESEND EMAIL
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResendEmail(long id)
        {
            var rDr = _reservationService.GetFirstOrDefault(x => x.Id == id);
            if (!rDr.Success || rDr.Data == null) return NotFound("Reservation not found.");
            // TODO: mail kuyruğa ekle
            return Ok(new { message = "Email queued." });
        }

        // ================== Helpers ==================

        private void NormalizeFilter(ref ReservationListFilter f)
        {
            if (f == null) f = new ReservationListFilter();
            if (f.StartDate.HasValue) f.StartDate = f.StartDate.Value.Date;
            if (f.EndDate.HasValue) f.EndDateExclusive = f.EndDate.Value.Date.AddDays(1);
            f.Page = Math.Max(1, f.Page);
            f.PageSize = Math.Clamp(f.PageSize, 1, 200);
        }

        private string FormatFullName(string name, string surname)
        {
            var n = (name ?? "").Trim();
            var s = (surname ?? "").Trim();
            return string.IsNullOrEmpty(n + s) ? "-" : $"{n} {s}".Trim();
        }

        // DİL BAZLI: ReservationStatusTranslation sözlüğü
        private Dictionary<long, string> GetReservationStatusNames()
        {
            var map = new Dictionary<long, string>();

            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langRes.Success ? langRes.Data?.Id : null;

            // İlgili dilde çeviriler
            var trListDr = (languageId.HasValue)
                ? _reservationStatusTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value)
                : _reservationStatusTranslationService.GetList(x => !x.IsDeleted);

            var trList = (trListDr.Success && trListDr.Data != null)
                ? trListDr.Data.ToList()
                : new List<ReservationStatusTranslationEntity>();

            foreach (var tr in trList)
            {
                map[tr.ReservationStatusId] = tr.Name;
            }

            return map;
        }

        private string ResolveStatusName(long statusId, Dictionary<long, string> dict)
            => (dict != null && dict.TryGetValue(statusId, out var name)) ? name : $"#{statusId}";

        private Dictionary<long, string> GetPaymentTypeNames()
        {
            var map = new Dictionary<long, string>();

            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langRes.Success ? langRes.Data?.Id : null;

            var ptList = _paymentTypeService.GetList(x => !x.IsDeleted);
            var trList = (languageId.HasValue)
                ? _paymentTypeTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value)
                : _paymentTypeTranslationService.GetList(x => !x.IsDeleted);

            var translations = (trList.Success && trList.Data != null)
                ? trList.Data.ToList()
                : new List<PaymentTypeTranslationEntity>();

            if (ptList.Success && ptList.Data != null)
            {
                foreach (var pt in ptList.Data)
                {
                    var tr = translations.FirstOrDefault(t => t.PaymentTypeId == pt.Id);
                    map[pt.Id] = tr?.Name ?? $"#{pt.Id}";
                }
            }

            return map;
        }

        private FilterLookups BuildFilterLookups()
        {
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langRes.Success ? langRes.Data?.Id : null;

            var baseRes = _reservationService.GetListWithInclude(
                x => !x.IsDeleted,
                x => x.Channel
            );

            var channels = new List<IdNameVm>();
            if (baseRes.Success && baseRes.Data != null)
            {
                var channelIds = baseRes.Data.Where(r => r.ChannelId != null).Select(r => r.ChannelId).Distinct().ToList();

                var channelTranslations = (languageId.HasValue)
                    ? _channelTranslationService.GetList(x => channelIds.Contains(x.ChannelId) && x.LanguageId == languageId.Value)
                    : _channelTranslationService.GetList(x => channelIds.Contains(x.ChannelId));

                var trMap = (channelTranslations.Success && channelTranslations.Data != null)
                    ? channelTranslations.Data.ToDictionary(ct => ct.ChannelId, ct => ct.Name)
                    : new Dictionary<long, string>();

                channels = channelIds
                    .Select(id => new IdNameVm
                    {
                        Id = id,
                        Name = trMap.TryGetValue(id, out var trName) ? trName : $"#{id}"
                    })
                    .OrderBy(c => c.Name)
                    .ToList();
            }

            var paymentTypeNames = GetPaymentTypeNames();
            var paymentTypes = paymentTypeNames
                .Select(kv => new IdNameVm { Id = kv.Key, Name = kv.Value })
                .OrderBy(x => x.Name)
                .ToList();

            var statusNames = GetReservationStatusNames();
            var statuses = statusNames
                .Select(kv => new IdNameVm { Id = kv.Key, Name = kv.Value })
                .OrderBy(x => x.Name)
                .ToList();

            List<IdNameVm> currencyTypeList = new List<IdNameVm>();
            var currencyTypes = _currencyTypeService.GetList(x => !x.IsDeleted);
            if (currencyTypes.Success && currencyTypes.Data != null)
            {
                foreach (var currencyType in currencyTypes.Data)
                {
                    currencyTypeList.Add(new IdNameVm
                    {
                        Id = currencyType.Id,
                        Name = currencyType.Name
                    });
                }
            }

            return new FilterLookups
            {
                Channels = channels,
                PaymentTypes = paymentTypes,
                Statuses = statuses,
                CurrencyTypes = currencyTypeList
            };
        }


        private Dictionary<long, string> GetTicketStatusNames()
        {
            var map = new Dictionary<long, string>();

            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langRes.Success ? langRes.Data?.Id : null;

            // TicketStatus listesi
            var tsList = _ticketStatusService.GetList(x => !x.IsDeleted);

            // Çeviriler
            var trList = (languageId.HasValue)
                ? _ticketStatusTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value)
                : _ticketStatusTranslationService.GetList(x => !x.IsDeleted);

            var translations = (trList.Success && trList.Data != null)
                ? trList.Data.ToList()
                : new List<TicketStatusTranslationEntity>();

            if (tsList.Success && tsList.Data != null)
            {
                foreach (var ts in tsList.Data)
                {
                    var tr = translations.FirstOrDefault(t => t.TicketStatusId == ts.Id);
                    map[ts.Id] = tr?.Name ?? $"#{ts.Id}";
                }
            }

            return map;
        }

    }
}
