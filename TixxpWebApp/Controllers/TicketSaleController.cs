using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Tixxp.Business.DataTransferObjects.Campaign;
using Tixxp.Business.Services.Abstract;
using Tixxp.Business.Services.Abstract.Campaign;
using Tixxp.Business.Services.Abstract.CampaignAction;
using Tixxp.Business.Services.Abstract.City;
using Tixxp.Business.Services.Abstract.CityTranslation;
using Tixxp.Business.Services.Abstract.CountryTranslation;
using Tixxp.Business.Services.Abstract.County;
using Tixxp.Business.Services.Abstract.CountyTranslation;
using Tixxp.Business.Services.Abstract.CurrenctUser;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.EventTicketPrice;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.PaymentType;
using Tixxp.Business.Services.Abstract.PaymentTypeTranslation;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Business.Services.Abstract.ReservationDetail;
using Tixxp.Business.Services.Abstract.ReservationProductDetail;
using Tixxp.Business.Services.Abstract.ReservationSaleInvoiceInfo;
using Tixxp.Business.Services.Abstract.Session;
using Tixxp.Business.Services.Abstract.SessionEventTicketPrice;
using Tixxp.Business.Services.Abstract.TicketSubType;
using Tixxp.Core.Utilities.Enums.ReservationStatusEnum;
using Tixxp.Core.Utilities.Enums.TicketStatusEnum;
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.City;
using Tixxp.Entities.CityTranslation;
using Tixxp.Entities.County;
using Tixxp.Entities.CountyTranslation;
using Tixxp.Entities.Events;
using Tixxp.Entities.EventTicketPrice;
using Tixxp.Entities.PaymentTypeTranslation;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Entities.ReservationProductDetail;
using Tixxp.Entities.ReservationSaleInvoiceInfo;
using Tixxp.Entities.SessionEventTicketPrice;
using Tixxp.Entities.Ticket;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionTypeTranslation;
using Tixxp.WebApp.Models.ProductPrice;
using Tixxp.WebApp.Models.TicketSale.GetConfirmation;
using Tixxp.WebApp.Models.TicketSale.GetDetail;
using Tixxp.WebApp.Models.TicketSale.GetSession;

namespace Tixxp.WebApp.Controllers;

/// <summary>
/// Satış akışı (Etkinlik → Seans → Detay → Onay) kontrolörü
/// </summary>
public class TicketSaleController : Controller
{
    // ———————————————————————————
    //  DI Alanları
    // ———————————————————————————
    private readonly ICurrentUser _currentUser;
    private readonly ITicketService _ticketService;

    private readonly IEventService _eventService;
    private readonly ISessionService _sessionService;
    private readonly ILanguageService _languageService;
    private readonly ISessionTypeTranslationRepository _sessionTypeTranslationRepository;

    private readonly IPaymentTypeService _paymentTypeService;
    private readonly IPaymentTypeTranslationService _paymentTypeTranslationService;

    private readonly ISessionEventTicketPriceService _sessionEventTicketPriceService;
    private readonly IEventTicketPriceService _eventTicketPriceService;

    private readonly ICountryTranslationService _countryTranslationService;
    private readonly ICityService _cityService;
    private readonly ICityTranslationService _cityTranslationService;
    private readonly ICountyService _countyService;
    private readonly ICountyTranslationService _countyTranslationService;

    private readonly IProductService _productService;
    private readonly IProductPriceService _productPriceService;
    private readonly ICurrencyTypeService _currencyTypeService;
    private readonly IProductTranslationService _productTranslationService;

    private readonly IReservationSaleInvoiceInfoService _reservationSaleInvoiceInfoService;
    private readonly IReservationService _reservationService;
    private readonly IReservationDetailService _reservationDetailService;
    private readonly IReservationProductDetailService _reservationProductDetailService;
    private readonly ITicketSubTypeService _ticketSubTypeService;

    private readonly ICampaignService _campaignService;
    private readonly ICampaignActionService _campaignActionService;

    public TicketSaleController(
        IEventService eventService,
        ISessionService sessionService,
        ILanguageService languageService,
        ISessionTypeTranslationRepository sessionTypeTranslationRepository,
        IPaymentTypeService paymentTypeService,
        ISessionEventTicketPriceService sessionEventTicketPriceService,
        IEventTicketPriceService eventTicketPriceService,
        IPaymentTypeTranslationService paymentTypeTranslationService,
        ICountryTranslationService countryTranslationService,
        ICityTranslationService cityTranslationService,
        ICityService cityService,
        ICountyTranslationService countyTranslationService,
        ICountyService countyService,
        IReservationSaleInvoiceInfoService reservationSaleInvoiceInfoService,
        IReservationService reservationService,
        ITicketSubTypeService ticketSubTypeService,
        IReservationDetailService reservationDetailService,
        IProductService productService,
        IProductPriceService productPriceService,
        ICurrencyTypeService currencyTypeService,
        IProductTranslationService productTranslationService,
        IReservationProductDetailService reservationProductDetailService,
        ICurrentUser currentUser,
        ITicketService ticketService,
        ICampaignService campaignService,
        ICampaignActionService campaignActionService)
    {
        _eventService = eventService;
        _sessionService = sessionService;
        _languageService = languageService;
        _sessionTypeTranslationRepository = sessionTypeTranslationRepository;

        _paymentTypeService = paymentTypeService;
        _paymentTypeTranslationService = paymentTypeTranslationService;

        _sessionEventTicketPriceService = sessionEventTicketPriceService;
        _eventTicketPriceService = eventTicketPriceService;

        _countryTranslationService = countryTranslationService;
        _cityTranslationService = cityTranslationService;
        _cityService = cityService;
        _countyTranslationService = countyTranslationService;
        _countyService = countyService;

        _reservationSaleInvoiceInfoService = reservationSaleInvoiceInfoService;
        _reservationService = reservationService;
        _reservationDetailService = reservationDetailService;

        _productService = productService;
        _productPriceService = productPriceService;
        _currencyTypeService = currencyTypeService;
        _productTranslationService = productTranslationService;
        _reservationProductDetailService = reservationProductDetailService;

        _ticketSubTypeService = ticketSubTypeService;
        _currentUser = currentUser;
        _ticketService = ticketService;

        _campaignService = campaignService;
        _campaignActionService = campaignActionService;
    }

    // ———————————————————————————
    //  1) Index (Etkinlik listesi)
    // ———————————————————————————
    public IActionResult Index()
    {
        var eventsDr = _eventService.GetList(x => !x.IsDeleted);
        if (eventsDr.Success && eventsDr.Data is { Count: > 0 })
            return View(eventsDr.Data);

        return View(Enumerable.Empty<EventEntity>());
    }

    // ———————————————————————————
    //  2) GetSession (Seans kartları)
    // ———————————————————————————
    [HttpGet]
    public IActionResult GetSession(long eventId, string? date)
    {
        if (eventId <= 0)
            return PartialView("_SessionCards", Enumerable.Empty<GetSessionViewModel>());

        var today = DateTime.Today;
        var baseDate = ParseSelectedDateOrToday(date, today);
        var endExclusive = baseDate.AddDays(2); // seçili gün + ertesi

        var sessionsDr = _sessionService.GetList(x =>
            x.EventId == eventId &&
            !x.IsDeleted &&
            x.Capacity > 0 &&
            x.SessionDate.HasValue &&
            x.SessionDate.Value >= baseDate &&
            x.SessionDate.Value < endExclusive);

        if (!(sessionsDr.Success && sessionsDr.Data != null))
            return PartialView("_SessionCards", Enumerable.Empty<GetSessionViewModel>());

        var langId = ResolveLanguageId();
        var sessionTypeTr = _sessionTypeTranslationRepository
            .GetListWithInclude(x => x.LanguageId == langId && !x.IsDeleted);

        var vms = sessionsDr.Data
            .OrderBy(s => s.SessionDate)
            .ThenBy(s => s.StartTime)
            .Select(s =>
            {
                var tr = sessionTypeTr.FirstOrDefault(t => t.SessionTypeId == s.TypeId);
                return new GetSessionViewModel
                {
                    SessionId = s.Id,
                    SessionDate = s.SessionDate,
                    StarTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsAvaliableOnB2B = s.IsAvailableOnB2B,
                    IsAvailableOnB2C = s.IsAvailableOnB2C,
                    Capacity = s.Capacity,
                    SessionTypeId = tr?.SessionTypeId ?? 0,
                    SessionTypeName = tr?.Name
                };
            })
            .ToList();

        return PartialView("_SessionCards", vms);
    }

    // ———————————————————————————
    //  3) GetDetail (Bilet & Ürün & Ödeme & Geo)
    // ———————————————————————————
    [HttpGet]
    public IActionResult GetDetail(long sessionId)
    {
        // Ödeme Tipleri (dil)
        var langId = ResolveLanguageId();
        BindPaymentTypesToViewBag(langId);

        // GEO (ülke/şehir/ilçe)
        BindGeoToViewBag(langId);

        // Ürünler + fiyat + para birimi
        BindProductsToViewBag(langId);

        // Seansın bilet fiyatları
        var mapDr = _sessionEventTicketPriceService.GetList(x => x.SessionId == sessionId && !x.IsDeleted);
        if (!(mapDr.Success && mapDr.Data is { Count: > 0 }))
            return PartialView("_Details", Enumerable.Empty<EventTicketPriceEntity>());

        var etpIds = mapDr.Data.Select(m => m.EventTicketPriceId).Distinct().ToList();
        var etpDr = _eventTicketPriceService.GetListWithInclude(
            x => etpIds.Contains(x.Id) && !x.IsDeleted,
            a => a.TicketType,
            a => a.PriceCategory,
            a => a.CurrencyType);

        return (etpDr.Success && etpDr.Data != null)
            ? PartialView("_Details", etpDr.Data)
            : PartialView("_Details", Enumerable.Empty<EventTicketPriceEntity>());
    }

    // ———————————————————————————
    //  4) GetConfirmation (Kampanyalar)
    // ———————————————————————————
    [HttpGet]
    public IActionResult GetConfirmation(long sessionId, decimal totalAmount)
    {
        var vm = new ConfirmationViewModel
        {
            CartItems = new List<CartItemViewModel>(),
            AvailableCampaigns = new List<CampaignInfoDto>(),
            TotalPrice = totalAmount
        };

        // Seans bilgisini çek
        var sessionDr = _sessionService.GetFirstOrDefault(x => x.Id == sessionId && !x.IsDeleted);
        if (!(sessionDr.Success && sessionDr.Data != null))
            return PartialView("_Confirmation", vm);

        // Kampanya değerlendirme DTO’su
        var dto = new ApplyCampaignRequestDto
        {
            ReservationEntity = new ReservationEntity { TotalPrice = totalAmount },
            SessionEntity = sessionDr.Data
        };

        // Aktif kampanyaları çek
        var allCamps = _campaignService.GetList(
            x => x.IsActive && x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now
        );

        if (allCamps.Success && allCamps.Data != null)
        {
            foreach (var camp in allCamps.Data)
            {
                // Koşullar sağlanıyorsa servis üzerinden indirimi uygula
                if (_campaignService.CheckConditions(camp, dto))
                {
                    var discounted = _campaignService.ApplyCampaigns(dto, camp);

                    vm.AvailableCampaigns.Add(new CampaignInfoDto
                    {
                        CampaignId = camp.Id,
                        Name = camp.Name,
                        Description = camp.Description,
                        OriginalPrice = totalAmount,
                        DiscountedPrice = Math.Max(0, discounted)
                    });
                }
            }
        }
        return PartialView("_Confirmation", vm);
    }


    // ———————————————————————————
    //  5) Confirm (Rezervasyon + Bilet + Opsiyonel Kampanya)
    // ———————————————————————————
    [HttpPost]
    public IActionResult Confirm([FromBody] GetConfirmation payload)
    {
        if (payload is null)
            return BadRequest("Payload null.");

        // SeansId tespiti (ilk bilet fiyatından)
        long? sessionId = ResolveSessionIdFromFirstTicket(payload);
        if (sessionId is null)
            return BadRequest("Session could not be resolved.");

        // — Reservation —
        var res = CreateReservation(payload, sessionId.Value); 

        // — Fatura/Ödeme Bilgisi (opsiyonel) —
        TryCreateInvoiceInfo(res.Data.Id, payload);

        // — Ürün Satırları —
        CreateProductLines(res.Data.Id, payload);

        // — Bilet Satırları ve Ticket üretimi —
        CreateReservationTickets(res.Data.Id, payload);

        // — Kampanya uygula (toplam fiyatı düşürmek istiyorsan burada servisten geçebilirsin) —
        // İsteğe göre burada Reservation toplamı güncellenebilir.

        return Ok(new { ReservationId = res.Data.Id });
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // PRIVATE HELPERS
    // ─────────────────────────────────────────────────────────────────────────────

    private static DateTime ParseSelectedDateOrToday(string? date, DateTime today)
    {
        if (!string.IsNullOrWhiteSpace(date) &&
            DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            return parsed.Date < today ? today : parsed.Date;
        }
        return today;
    }

    private long? ResolveLanguageId()
    {
        var cultureCode = CultureInfo.CurrentUICulture.Name;
        var langDr = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
        return langDr.Success ? langDr.Data?.Id : null;
    }

    private void BindPaymentTypesToViewBag(long? langId)
    {
        var ptDr = _paymentTypeService.GetList(x => !x.IsDeleted);
        var trDr = (langId.HasValue)
            ? _paymentTypeTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == langId.Value)
            : _paymentTypeTranslationService.GetList(x => !x.IsDeleted);

        var translations = (trDr.Success && trDr.Data != null) ? trDr.Data : new List<PaymentTypeTranslationEntity>();

        if (ptDr.Success && ptDr.Data != null)
        {
            var list = ptDr.Data
                .Select(pt =>
                {
                    var tr = translations.FirstOrDefault(t => t.PaymentTypeId == pt.Id);
                    return new { Id = pt.Id, Name = tr?.Name ?? $"#{pt.Id}" };
                })
                .OrderBy(x => x.Name)
                .ToList();

            ViewBag.PaymentTypes = list;
        }
        else
        {
            ViewBag.PaymentTypes = Enumerable.Empty<object>();
        }
    }

    private void BindGeoToViewBag(long? langId)
    {
        var countriesTrDr = (langId.HasValue)
            ? _countryTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == langId.Value)
            : _countryTranslationService.GetList(x => !x.IsDeleted);

        if (!(countriesTrDr.Success && countriesTrDr.Data != null && countriesTrDr.Data.Any()))
        {
            ViewBag.Countries = ViewBag.Cities = ViewBag.Counties = Enumerable.Empty<object>();
            return;
        }

        var countryIds = countriesTrDr.Data.Select(x => x.CountryId).Distinct().ToList();

        // Cities
        var citiesDr = _cityService.GetList(x => !x.IsDeleted && countryIds.Contains(x.CountryId));
        var cities = (citiesDr.Success && citiesDr.Data != null) ? citiesDr.Data.ToList() : new List<CityEntity>();
        var cityIds = cities.Select(c => c.Id).Distinct().ToList();

        // City translations
        var cityTrDr = (langId.HasValue)
            ? _cityTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == langId.Value && cityIds.Contains(x.CityId))
            : _cityTranslationService.GetList(x => !x.IsDeleted && cityIds.Contains(x.CityId));
        var cityTr = (cityTrDr.Success && cityTrDr.Data != null) ? cityTrDr.Data.ToList() : new List<CityTranslationEntity>();

        // Counties
        var countiesDr = _countyService.GetList(x => !x.IsDeleted && cityIds.Contains(x.CityId));
        var counties = (countiesDr.Success && countiesDr.Data != null) ? countiesDr.Data.ToList() : new List<CountyEntity>();
        var countyIds = counties.Select(k => k.Id).Distinct().ToList();

        // County translations
        var countyTrDr = (langId.HasValue)
            ? _countyTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == langId.Value && countyIds.Contains(x.CountyId))
            : _countyTranslationService.GetList(x => !x.IsDeleted && countyIds.Contains(x.CountyId));
        var countyTr = (countyTrDr.Success && countyTrDr.Data != null) ? countyTrDr.Data.ToList() : new List<CountyTranslationEntity>();

        ViewBag.Countries = countriesTrDr.Data
            .Select(ct => new { Id = ct.CountryId, Name = ct.Name })
            .OrderBy(x => x.Name)
            .ToList();

        ViewBag.Cities = cities
            .Select(c => new
            {
                Id = c.Id,
                CountryId = c.CountryId,
                Name = cityTr.FirstOrDefault(t => t.CityId == c.Id)?.Name ?? $"#{c.Id}"
            })
            .OrderBy(x => x.Name)
            .ToList();

        ViewBag.Counties = counties
            .Select(k => new
            {
                Id = k.Id,
                CityId = k.CityId,
                Name = countyTr.FirstOrDefault(t => t.CountyId == k.Id)?.Name ?? $"#{k.Id}"
            })
            .OrderBy(x => x.Name)
            .ToList();
    }

    private void BindProductsToViewBag(long? langId)
    {
        try
        {
            var productDr = _productService.GetAll();
            var priceDr = _productPriceService.GetAll();

            if (!(productDr.Success && productDr.Data != null &&
                  priceDr.Success && priceDr.Data != null))
            {
                ViewBag.Products = new List<ProductWithPriceViewModel>();
                return;
            }

            var productIds = productDr.Data.Select(p => p.Id).ToList();
            var trDr = (langId.HasValue)
                ? _productTranslationService.GetList(x => productIds.Contains(x.ProductId) && x.LanguageId == langId.Value)
                : _productTranslationService.GetList(x => productIds.Contains(x.ProductId));

            var translations = (trDr.Success && trDr.Data != null)
                ? trDr.Data.ToList()
                : new List<ProductTranslationEntity>();

            var vms = (from p in productDr.Data
                       join pr in priceDr.Data on p.Id equals pr.ProductId into pp
                       from pr in pp.DefaultIfEmpty()
                       select new ProductWithPriceViewModel
                       {
                           ProductId = p.Id,
                           Name = translations.FirstOrDefault(t => t.ProductId == p.Id)?.Name ?? p.Code,
                           Code = p.Code,
                           CurrencyTypeId = pr?.CurrencyTypeId ?? 0,
                           CurrencyTypeSymbol = "",
                           Price = pr?.Price ?? 0m,
                           VatRate = pr?.VatRate ?? 0
                       }).ToList();

            var currencyIds = vms.Select(x => x.CurrencyTypeId).Where(id => id > 0).Distinct().ToList();
            if (currencyIds.Any())
            {
                var currencyDr = _currencyTypeService.GetList(x => currencyIds.Contains(x.Id));
                if (currencyDr.Success && currencyDr.Data != null)
                {
                    foreach (var vm in vms)
                    {
                        if (vm.CurrencyTypeId > 0)
                        {
                            vm.CurrencyTypeSymbol = currencyDr.Data
                                .FirstOrDefault(c => c.Id == vm.CurrencyTypeId)?.Symbol ?? "";
                        }
                    }
                }
            }

            ViewBag.Products = vms;
        }
        catch
        {
            ViewBag.Products = new List<ProductWithPriceViewModel>();
        }
    }
    private long? ResolveSessionIdFromFirstTicket(GetConfirmation payload)
    {
        var firstEtpId = payload.TicketInformations?.FirstOrDefault()?.EventTicketPriceId;
        if (!firstEtpId.HasValue) return null;

        var mapDr = _sessionEventTicketPriceService.GetFirstOrDefault(
            x => x.EventTicketPriceId == firstEtpId.Value && !x.IsDeleted);

        return (mapDr.Success && mapDr.Data != null) ? mapDr.Data.SessionId : null;
    }

    private IDataResult<ReservationEntity> CreateReservation(GetConfirmation payload, long sessionId)
    {
        var total = payload.PaymentInformation?.TotalAmount ?? 0m;

        var reservation = new ReservationEntity
        {
            StatusId = (long)ReservationStatusEnum.Confirmed,
            ChannelId = 1,
            CurrencyId = 1,
            TotalPrice = total,
            PaidPrice = total,
            ChangePrice = total,
            IsInvoiced = false,
            IsDeleted = false,
            TotalTicket = payload.TicketInformations?.Sum(x => x.Piece) ?? 0,
            CreatedBy = _currentUser.GetRequiredUserId(),
            SessionId = sessionId,
            CampaignId = payload.CampaignId
        };

        return _reservationService.AddAndReturn(reservation);
    }

    private void TryCreateInvoiceInfo(long reservationId, GetConfirmation payload)
    {
        var pi = payload.PersonalInformation;
        var pay = payload.PaymentInformation;

        var hasCounty = (pi?.CountyId ?? 0) > 0;
        var hasPersonal =
            pi != null &&
            !string.IsNullOrWhiteSpace(pi.FirstName) &&
            !string.IsNullOrWhiteSpace(pi.Surname) &&
            !string.IsNullOrWhiteSpace(pi.Email) &&
            !string.IsNullOrWhiteSpace(pi.Phone) &&
            hasCounty;

        var hasPaymentType = (pay?.PaymentTypeId ?? 0) > 0;

        if (!hasPersonal && !hasPaymentType) return;

        var inv = new ReservationSaleInvoiceInfoEntity
        {
            ReservationId = reservationId,
            CreatedBy = _currentUser.GetRequiredUserId(),
            PaymentTypeId = hasPaymentType ? pay!.PaymentTypeId : (long?)null,
            BankId = 1,
            GuideId = 1,
            InvoiceTypeId = 1
        };

        if (hasPersonal)
        {
            inv.Name = pi!.FirstName;
            inv.Surname = pi.Surname;
            inv.Email = pi.Email;
            inv.Phone = pi.Phone;
            inv.CountyId = pi.CountyId;
        }

        _reservationSaleInvoiceInfoService.Add(inv);
    }

    private void CreateProductLines(long reservationId, GetConfirmation payload)
    {
        if (payload.ProductInformations is null) return;

        foreach (var p in payload.ProductInformations)
        {
            var line = new ReservationProductDetailEntity
            {
                ReservationId = reservationId,
                ProductId = p.ProductId,
                Piece = p.Piece,
                CreatedBy = _currentUser.GetRequiredUserId()
            };
            _reservationProductDetailService.Add(line);
        }
    }

    private void CreateReservationTickets(long reservationId, GetConfirmation payload)
    {
        if (payload.TicketInformations is null) return;

        foreach (var ti in payload.TicketInformations)
        {
            if (ti.Piece <= 0) continue;

            var etpDr = _eventTicketPriceService.GetFirstOrDefault(x => x.Id == ti.EventTicketPriceId);
            if (!(etpDr.Success && etpDr.Data != null)) continue;

            var subTypeDr = _ticketSubTypeService.GetFirstOrDefault(x => x.TicketTypeId == etpDr.Data.TicketTypeId);

            // ReservationDetail
            var rd = new ReservationDetailEntity
            {
                ReservationId = reservationId,
                TicketTypeId = etpDr.Data.TicketTypeId,
                TicketSubTypeId = subTypeDr.Data?.Id ?? 0,
                NumberOfTickets = ti.Piece
            };

            var rdNew = _reservationDetailService.AddAndReturn(rd);
            if (!(rdNew.Success && rdNew.Data != null)) continue;

            var rdId = rdNew.Data.Id;

            // EventId fallback (Session → Event)
            var eventId = ResolveEventIdFallback(ti.EventTicketPriceId);

            // Biletleri üret
            for (int i = 1; i <= ti.Piece; i++)
            {
                var ticket = new TicketEntity
                {
                    ReservationDetailId = rdId,
                    EventId = eventId,
                    TicketStatusId = (long)TicketStatusEnum.Active,
                    CheckInDate = null,
                    CheckOutDate = null,
                    QrText = BuildQrText(reservationId, rdId, ti.EventTicketPriceId, i),
                    CreatedBy = _currentUser.GetRequiredUserId()
                };
                _ticketService.Add(ticket);
            }
        }
    }

    private long ResolveEventIdFallback(long eventTicketPriceId)
    {
        var mapDr = _sessionEventTicketPriceService.GetFirstOrDefault(x => x.EventTicketPriceId == eventTicketPriceId && !x.IsDeleted);
        if (mapDr.Success && mapDr.Data != null)
        {
            var sessDr = _sessionService.GetFirstOrDefault(x => x.Id == mapDr.Data.SessionId && !x.IsDeleted);
            if (sessDr.Success && sessDr.Data != null)
                return sessDr.Data.EventId;
        }
        return 0;
    }

    private static string BuildQrText(long reservationId, long reservationDetailId, long eventTicketPriceId, int seq)
        => $"TXXP|R:{reservationId}|RD:{reservationDetailId}|ETP:{eventTicketPriceId}|S:{seq}|TS:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}|{Guid.NewGuid():N}";
}
