using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
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
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.PaymentTypeTranslation;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Entities.ReservationProductDetail;
using Tixxp.Entities.ReservationSaleInvoiceInfo;
using Tixxp.Entities.ReservationStatusTranslation; // <-- gerekiyorsa (entity kullanmıyoruz ama namespace tutarlılığı için)
using Tixxp.WebApp.Models.ReservationManagement;

namespace Tixxp.WebApp.Controllers
{
    public class ReservationManagementController : Controller
    {
        // === services ===
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
            IProductTranslationService productTranslationService)
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
        }

        // INDEX
        [HttpGet]
        public IActionResult Index()
        {
            var lookups = BuildFilterLookups();
            ViewBag.Channels = lookups.Channels;
            ViewBag.Statuses = lookups.Statuses;      // dil bazlı
            ViewBag.PaymentTypes = lookups.PaymentTypes;  // dil bazlı
            return View();
        }

        // SEARCH (partial)
        [HttpGet]
        public IActionResult Search([FromQuery] ReservationListFilter filter)
        {
            NormalizeFilter(ref filter);

            var baseResult = _reservationService.GetListWithInclude(x =>
                !x.IsDeleted
                && (!filter.ChannelId.HasValue || x.ChannelId == filter.ChannelId.Value)
                && (!filter.StatusId.HasValue || x.StatusId == filter.StatusId.Value)
                && (!filter.StartDate.HasValue || x.Created_Date >= filter.StartDate.Value)
                && (!filter.EndDateExclusive.HasValue || x.Created_Date < filter.EndDateExclusive.Value)
                && (!filter.ReservationId.HasValue || x.Id == filter.ReservationId.Value),
                x => x.Channel);

            var list = (baseResult.Success && baseResult.Data != null)
                ? baseResult.Data
                : new List<ReservationEntity>();

            var reservationIds = list.Select(r => r.Id).ToList();

            var invResult = _invoiceInfoService.GetList(x => reservationIds.Contains(x.ReservationId) && !x.IsDeleted);
            var invoiceList = (invResult.Success && invResult.Data != null)
                ? invResult.Data
                : new List<ReservationSaleInvoiceInfoEntity>();

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
            var statusNames = GetReservationStatusNames(); // <-- dil bazlı statü sözlüğü

            // basit kanal isimleri (gerçek kanal servisin varsa oradan al)
            var channelNames = list
                .Where(x => x.Channel != null)
                .GroupBy(x => x.ChannelId)
                .Select(g => g.First())
                .ToDictionary(x => x.ChannelId, x => x.Channel.Name);

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
                        ChannelName = channelNames.TryGetValue(r.ChannelId, out var cn) ? cn : $"#{r.ChannelId}",
                        TotalPrice = r.TotalPrice ?? 0,
                        PaymentTypeId = inv?.PaymentTypeId,
                        PaymentTypeName = (inv?.PaymentTypeId != null && paymentTypeNames.TryGetValue(inv.PaymentTypeId.Value, out var ptn))
                                          ? ptn : (inv?.PaymentTypeId != null ? $"#{inv.PaymentTypeId}" : "-"),
                        StatusId = r.StatusId,
                        StatusName = ResolveStatusName(r.StatusId, statusNames), // <-- burada kullanılıyor
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

            // 2) Rezervasyon (Channel include)
            var rDr = _reservationService.GetFirstOrDefaultWithInclude(x => x.Id == id, x => x.Channel);
            if (!rDr.Success || rDr.Data == null)
                return NotFound("Reservation not found.");

            // 3) Fatura/kişisel bilgi
            var invDr = _invoiceInfoService.GetFirstOrDefault(x => x.ReservationId == id);
            var inv = invDr.Success ? invDr.Data : null;

            // 4) Bilet satırları (TicketType & TicketSubType include) + adlar
            var detDr = _reservationDetailService.GetListWithInclude(
                x => x.ReservationId == id && !x.IsDeleted,
                x => x.TicketSubType,
                x => x.TicketType
            );
            var ticketLines = (detDr.Success && detDr.Data != null)
                ? detDr.Data.ToList()
                : new List<ReservationDetailEntity>();

            var ticketRows = ticketLines.Select(line => new ReservationDetailTicketVm
            {
                TicketTypeId = line.TicketTypeId,
                TicketTypeName = line.TicketType?.Name ?? $"#{line.TicketTypeId}",
                TicketSubTypeId = line.TicketSubTypeId,
                TicketSubTypeName = line.TicketSubType?.Name ?? (line.TicketSubTypeId > 0 ? $"#{line.TicketSubTypeId}" : "-"),
                Piece = line.NumberOfTickets
            }).ToList();

            // 5) ÜRÜN satırları (ReservationProductDetail -> Product include)
            var rpdDr = _reservationProductDetailService.GetListWithInclude(
                x => x.ReservationId == id && !x.IsDeleted,
                x => x.Product
            );
            var productLines = (rpdDr.Success && rpdDr.Data != null)
                ? rpdDr.Data.ToList()
                : new List<ReservationProductDetailEntity>();

            // 5.a) ProductTranslation’dan dil bazlı adları çek
            var productRows = new List<ReservationDetailProductVm>();
            if (productLines.Any())
            {
                var productIds = productLines
                    .Where(p => p.ProductId > 0)
                    .Select(p => p.ProductId)
                    .Distinct()
                    .ToList();

                var trDr = (languageId.HasValue)
                    ? _productTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value && productIds.Contains(x.ProductId))
                    : _productTranslationService.GetList(x => !x.IsDeleted && productIds.Contains(x.ProductId));

                var translations = (trDr.Success && trDr.Data != null)
                    ? trDr.Data.ToList()
                    : new List<ProductTranslationEntity>();

                // (ProductId -> Name) sözlüğü
                var trMap = translations
                    .GroupBy(t => t.ProductId)
                    .ToDictionary(g => g.Key, g => g.First().Name);

                productRows = productLines.Select(p =>
                {
                    // Çeviri -> Product.Code -> #Id
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

            // 6) Dil bazlı Status ve PaymentType adları
            var statusNames = GetReservationStatusNames();
            var paymentTypeNames = GetPaymentTypeNames();

            string paymentTypeName =
                (inv?.PaymentTypeId != null && paymentTypeNames.TryGetValue(inv.PaymentTypeId.Value, out var ptn))
                    ? ptn
                    : (inv?.PaymentTypeId != null ? $"#{inv.PaymentTypeId}" : "-");

            // 7) ViewModel
            var vm = new ReservationDetailVm
            {
                ReservationId = rDr.Data.Id,
                CreatedDate = rDr.Data.Created_Date,
                StatusId = rDr.Data.StatusId,
                StatusName = ResolveStatusName(rDr.Data.StatusId, statusNames),
                ChannelId = rDr.Data.ChannelId,
                ChannelName = rDr.Data.Channel?.Name,
                TotalPrice = rDr.Data.TotalPrice ?? 0,
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
                // ReservationStatusId -> Name
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
            var baseRes = _reservationService.GetListWithInclude(
                x => !x.IsDeleted,
                x => x.Channel
            );

            var channels = new List<IdNameVm>();
            if (baseRes.Success && baseRes.Data != null)
            {
                channels = baseRes.Data
                    .Where(r => r.Channel != null)
                    .GroupBy(r => r.ChannelId)
                    .Select(g => g.First().Channel)
                    .OrderBy(c => c.Name)
                    .Select(c => new IdNameVm { Id = c.Id, Name = c.Name })
                    .ToList();
            }

            var paymentTypeNames = GetPaymentTypeNames();
            var paymentTypes = paymentTypeNames
                .Select(kv => new IdNameVm { Id = kv.Key, Name = kv.Value })
                .OrderBy(x => x.Name)
                .ToList();

            // DİL BAZLI status lookupları
            var statusNames = GetReservationStatusNames();
            var statuses = statusNames
                .Select(kv => new IdNameVm { Id = kv.Key, Name = kv.Value })
                .OrderBy(x => x.Name)
                .ToList();

            // çeviri yoksa eldeki verilerden fallback
            if (!statuses.Any() && baseRes.Success && baseRes.Data != null)
            {
                statuses = baseRes.Data
                    .Select(r => r.StatusId)
                    .Distinct()
                    .OrderBy(id => id)
                    .Select(id => new IdNameVm { Id = id, Name = $"#{id}" })
                    .ToList();
            }

            return new FilterLookups
            {
                Channels = channels,
                PaymentTypes = paymentTypes,
                Statuses = statuses
            };
        }
    }
}
