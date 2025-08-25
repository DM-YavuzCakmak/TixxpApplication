using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
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
using Tixxp.Core.Utilities.Results.Abstract;
using Tixxp.Entities.City;
using Tixxp.Entities.CityTranslation;
using Tixxp.Entities.County;
using Tixxp.Entities.CountyTranslation;
using Tixxp.Entities.Events;
using Tixxp.Entities.EventTicketPrice;
using Tixxp.Entities.PaymentType;
using Tixxp.Entities.PaymentTypeTranslation;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Entities.ReservationProductDetail;
using Tixxp.Entities.ReservationSaleInvoiceInfo;
using Tixxp.Entities.Session;
using Tixxp.Entities.SessionEventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionTypeTranslation;
using Tixxp.WebApp.Models.ProductPrice;
using Tixxp.WebApp.Models.TicketSale.GetConfirmation;
using Tixxp.WebApp.Models.TicketSale.GetDetail;
using Tixxp.WebApp.Models.TicketSale.GetSession;

namespace Tixxp.WebApp.Controllers;

public class TicketSaleController : Controller
{
    private readonly IReservationProductDetailService _reservationProductDetailService;
    private readonly ICurrentUser _currentUser;


    private readonly IProductService _productService;
    private readonly IProductPriceService _productPriceService;
    private readonly ICurrencyTypeService _currencyTypeService;
    private readonly IProductTranslationService _productTranslationService;

    private readonly IReservationSaleInvoiceInfoService _reservationSaleInvoiceInfoService;
    private readonly IReservationService _reservationService;
    private readonly IReservationDetailService _reservationDetailService;

    private readonly ITicketSubTypeService _ticketSubTypeService;

    private readonly IEventService _eventService;
    private readonly ISessionService _sessionService;
    private readonly IPaymentTypeService _paymentTypeService;
    private readonly IPaymentTypeTranslationService _paymentTypeTranslationService;
    private readonly ISessionEventTicketPriceService _sessionEventTicketPriceService;
    private readonly IEventTicketPriceService _eventTicketPriceService;

    private readonly ICountryTranslationService _countryTranslationService;

    private readonly ICityService _cityService;
    private readonly ICityTranslationService _cityTranslationService;

    private readonly ICountyTranslationService _countyTranslationService;
    private readonly ICountyService _countyService;

    private readonly ISessionTypeTranslationRepository _sessionTypeTranslationRepository;
    private readonly ILanguageService _languageService;

    public TicketSaleController(IEventService eventService, ISessionService sessionService, ILanguageService languageService, ISessionTypeTranslationRepository sessionTypeTranslationRepository, IPaymentTypeService paymentTypeService, ISessionEventTicketPriceService sessionEventTicketPriceService, IEventTicketPriceService eventTicketPriceService, IPaymentTypeTranslationService paymentTypeTranslationService, ICountryTranslationService countryTranslationService, ICityTranslationService cityTranslationService, ICityService cityService, ICountyTranslationService countyTranslationService, ICountyService countyService, IReservationSaleInvoiceInfoService reservationSaleInvoiceInfoService, IReservationService reservationService, ITicketSubTypeService ticketSubTypeService, IReservationDetailService reservationDetailService, IProductService productService, IProductPriceService productPriceService, ICurrencyTypeService currencyTypeService, IProductTranslationService productTranslationService, IReservationProductDetailService reservationProductDetailService, ICurrentUser currentUser)
    {
        _eventService = eventService;
        _sessionService = sessionService;
        _languageService = languageService;
        _sessionTypeTranslationRepository = sessionTypeTranslationRepository;
        _paymentTypeService = paymentTypeService;
        _sessionEventTicketPriceService = sessionEventTicketPriceService;
        _eventTicketPriceService = eventTicketPriceService;
        _paymentTypeTranslationService = paymentTypeTranslationService;
        _countryTranslationService = countryTranslationService;
        _cityTranslationService = cityTranslationService;
        _cityService = cityService;
        _countyTranslationService = countyTranslationService;
        _countyService = countyService;
        _reservationSaleInvoiceInfoService = reservationSaleInvoiceInfoService;
        _reservationService = reservationService;
        _ticketSubTypeService = ticketSubTypeService;
        _reservationDetailService = reservationDetailService;
        _productService = productService;
        _productPriceService = productPriceService;
        _currencyTypeService = currencyTypeService;
        _productTranslationService = productTranslationService;
        _reservationProductDetailService = reservationProductDetailService;
        _currentUser = currentUser;
    }

    public IActionResult Index()
    {
        IDataResult<List<EventEntity>> eventListDataResult = _eventService.GetList(x => !x.IsDeleted);
        if (eventListDataResult.Success && eventListDataResult.Data != null)
        {
            return View(eventListDataResult.Data);
        }

        return View(Enumerable.Empty<EventEntity>());
    }

    [HttpGet]
    public IActionResult GetSession(long eventId, string? date)
    {
        var getSessionViewModels = new List<GetSessionViewModel>();
        if (eventId <= 0)
            return PartialView("_SessionCards", Enumerable.Empty<GetSessionViewModel>());

        // Seçilen tarih (yyyy-MM-dd) => yoksa bugün
        var today = DateTime.Today;
        DateTime baseDate = today;

        if (!string.IsNullOrWhiteSpace(date) &&
            DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
        {
            baseDate = parsed.Date < today ? today : parsed.Date; // geçmişe izin yok
        }

        var endExclusive = baseDate.AddDays(2); // seçili gün + 1 sonraki gün

        var sessionListDataResult = _sessionService.GetList(x =>
            x.EventId == eventId &&
            !x.IsDeleted &&
            x.Capacity > 0 &&
            x.SessionDate.HasValue &&
            x.SessionDate.Value >= baseDate &&
            x.SessionDate.Value < endExclusive
        );

        if (sessionListDataResult.Success && sessionListDataResult.Data != null)
        {
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var languageResult = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = languageResult.Success ? languageResult.Data?.Id : null;

            var sessionTypeTranslations = _sessionTypeTranslationRepository
                .GetListWithInclude(x => x.LanguageId == languageId && !x.IsDeleted);

            foreach (var session in sessionListDataResult.Data
                         .OrderBy(s => s.SessionDate)
                         .ThenBy(s => s.StartTime))
            {
                var tr = sessionTypeTranslations.FirstOrDefault(x => x.SessionTypeId == session.TypeId);

                var vm = new GetSessionViewModel
                {
                    SessionId = session.Id,
                    SessionDate = session.SessionDate,
                    StarTime = session.StartTime,
                    EndTime = session.EndTime,
                    IsAvaliableOnB2B = session.IsAvailableOnB2B,
                    IsAvailableOnB2C = session.IsAvailableOnB2C,
                    Capacity = session.Capacity,
                    SessionTypeId = tr.SessionTypeId,
                    SessionTypeName = tr?.Name
                };

                getSessionViewModels.Add(vm);
            }

            return PartialView("_SessionCards", getSessionViewModels);
        }

        return PartialView("_SessionCards", Enumerable.Empty<GetSessionViewModel>());
    }

    [HttpGet]
    public IActionResult GetDetail(long sessionId)
    {
        // Dil
        var cultureCode = CultureInfo.CurrentUICulture.Name;
        var languageResult = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
        long? languageId = languageResult.Success ? languageResult.Data?.Id : null;

        // Ödeme tipi (dil bazlı)
        var paymentTypeListDr = _paymentTypeService.GetList(x => !x.IsDeleted);
        var paymentTypeTranslationsDr = (languageId.HasValue)
            ? _paymentTypeTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value)
            : _paymentTypeTranslationService.GetList(x => !x.IsDeleted);

        if (paymentTypeListDr.Success && paymentTypeListDr.Data != null)
        {
            var translations = paymentTypeTranslationsDr.Success && paymentTypeTranslationsDr.Data != null
                ? paymentTypeTranslationsDr.Data
                : new List<PaymentTypeTranslationEntity>();

            var paymentTypeOptions = paymentTypeListDr.Data
                .Select(pt =>
                {
                    var tr = translations.FirstOrDefault(t => t.PaymentTypeId == pt.Id);
                    return new { Id = pt.Id, Name = tr?.Name ?? $"#{pt.Id}" };
                })
                .OrderBy(x => x.Name)
                .ToList();

            ViewBag.PaymentTypes = paymentTypeOptions;
        }

        // ---------------------------
        // GEO: Country / City / County
        // ---------------------------
        var countriesTrDr = (languageId.HasValue)
            ? _countryTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value)
            : _countryTranslationService.GetList(x => !x.IsDeleted);

        if (countriesTrDr.Success && countriesTrDr.Data != null && countriesTrDr.Data.Any())
        {
            var countryIds = countriesTrDr.Data.Select(x => x.CountryId).Distinct().ToList();

            // Cities
            var citiesDr = _cityService.GetList(x => !x.IsDeleted && countryIds.Contains(x.CountryId));
            var cities = (citiesDr.Success && citiesDr.Data != null) ? citiesDr.Data.ToList() : new List<CityEntity>();

            // City translations
            var cityIds = cities.Select(c => c.Id).Distinct().ToList();
            var cityTrDr = (languageId.HasValue)
                ? _cityTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value && cityIds.Contains(x.CityId))
                : _cityTranslationService.GetList(x => !x.IsDeleted && cityIds.Contains(x.CityId));
            var cityTr = (cityTrDr.Success && cityTrDr.Data != null) ? cityTrDr.Data.ToList() : new List<CityTranslationEntity>();

            // Counties
            var countiesDr = _countyService.GetList(x => !x.IsDeleted && cityIds.Contains(x.CityId));
            var counties = (countiesDr.Success && countiesDr.Data != null) ? countiesDr.Data.ToList() : new List<CountyEntity>();

            var countyIds = counties.Select(k => k.Id).Distinct().ToList();
            var countyTrDr = (languageId.HasValue)
                ? _countyTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value && countyIds.Contains(x.CountyId))
                : _countyTranslationService.GetList(x => !x.IsDeleted && countyIds.Contains(x.CountyId));
            var countyTr = (countyTrDr.Success && countyTrDr.Data != null) ? countyTrDr.Data.ToList() : new List<CountyTranslationEntity>();

            // DTO'lar
            var countriesDto = countriesTrDr.Data
                .Select(ct => new { Id = ct.CountryId, Name = ct.Name })
                .OrderBy(x => x.Name)
                .ToList();

            var citiesDto = cities
                .Select(c =>
                {
                    var tr = cityTr.FirstOrDefault(t => t.CityId == c.Id);
                    return new { Id = c.Id, CountryId = c.CountryId, Name = tr?.Name ?? $"#{c.Id}" };
                })
                .OrderBy(x => x.Name)
                .ToList();

            var countiesDto = counties
                .Select(k =>
                {
                    var tr = countyTr.FirstOrDefault(t => t.CountyId == k.Id);
                    return new { Id = k.Id, CityId = k.CityId, Name = tr?.Name ?? $"#{k.Id}" };
                })
                .OrderBy(x => x.Name)
                .ToList();

            ViewBag.Countries = countriesDto;
            ViewBag.Cities = citiesDto;
            ViewBag.Counties = countiesDto;
        }
        else
        {
            ViewBag.Countries = Enumerable.Empty<object>();
            ViewBag.Cities = Enumerable.Empty<object>();
            ViewBag.Counties = Enumerable.Empty<object>();
        }

        // ------------------------------------
        // ÜRÜNLER (çeviri + fiyat + para birimi)
        // ------------------------------------
        try
        {
            var productDr = _productService.GetAll();
            var priceDr = _productPriceService.GetAll();

            var productVms = new List<ProductWithPriceViewModel>();

            if (productDr.Success && productDr.Data != null && priceDr.Success && priceDr.Data != null)
            {
                var productIds = productDr.Data.Select(p => p.Id).ToList();

                // Çeviriler (dil varsa dilde, yoksa boş)
                var trDr = (languageId.HasValue)
                    ? _productTranslationService.GetList(x => productIds.Contains(x.ProductId) && x.LanguageId == languageId.Value)
                    : _productTranslationService.GetList(x => productIds.Contains(x.ProductId));

                var translations = (trDr.Success && trDr.Data != null)
                    ? trDr.Data.ToList()
                    : new List<ProductTranslationEntity>();

                // VM join
                productVms = (from product in productDr.Data
                              join price in priceDr.Data on product.Id equals price.ProductId into pp
                              from price in pp.DefaultIfEmpty()
                              select new ProductWithPriceViewModel
                              {
                                  ProductId = product.Id,
                                  Name = translations.FirstOrDefault(t => t.ProductId == product.Id)?.Name ?? product.Code,
                                  Code = product.Code,
                                  CurrencyTypeId = price?.CurrencyTypeId ?? 0,
                                  CurrencyTypeSymbol = "",
                                  Price = price?.Price ?? 0m,
                                  VatRate = price?.VatRate ?? 0
                              }).ToList();

                // Para birimi sembolleri
                var currencyIds = productVms.Select(v => v.CurrencyTypeId).Where(id => id > 0).Distinct().ToList();
                if (currencyIds.Any())
                {
                    var currencyDr = _currencyTypeService.GetList(x => currencyIds.Contains(x.Id));
                    if (currencyDr.Success && currencyDr.Data != null)
                    {
                        foreach (var vm in productVms)
                        {
                            if (vm.CurrencyTypeId > 0)
                            {
                                vm.CurrencyTypeSymbol = currencyDr.Data.FirstOrDefault(c => c.Id == vm.CurrencyTypeId)?.Symbol ?? "";
                            }
                        }
                    }
                }
            }

            ViewBag.Products = productVms; // Detay sayfasında ürünleri buradan al
        }
        catch
        {
            ViewBag.Products = new List<ProductWithPriceViewModel>();
        }

        // -------------------------------
        // Ticket Prices (include'larla)
        // -------------------------------
        var sessionEventTicketPriceDr = _sessionEventTicketPriceService.GetList(x => x.SessionId == sessionId && !x.IsDeleted);
        if (sessionEventTicketPriceDr.Success && sessionEventTicketPriceDr.Data != null)
        {
            var eventTicketPriceIds = sessionEventTicketPriceDr.Data
                .Select(x => x.EventTicketPriceId)
                .Distinct()
                .ToList();

            var eventTicketPriceDr =
                _eventTicketPriceService.GetListWithInclude(
                    x => eventTicketPriceIds.Contains(x.Id) && !x.IsDeleted,
                    a => a.TicketType,
                    a => a.PriceCategory,
                    a => a.CurrencyType);

            if (eventTicketPriceDr.Success && eventTicketPriceDr.Data != null)
            {
                return PartialView("_Details", eventTicketPriceDr.Data);
            }
        }

        return PartialView("_Details", Enumerable.Empty<EventTicketPriceEntity>());
    }


    [HttpGet]
    public IActionResult GetConfirmation()
    {
        return PartialView("_Confirmation");
    }

    [HttpPost]
    public IActionResult Confirm([FromBody] GetConfirmation getConfirmation)
    {

        #region Reservation
        var reservationEntity = new ReservationEntity
        {
            StatusId = Convert.ToInt64(ReservationStatusEnum.Confirmed),
            ChannelId = 1,
            CurrencyId = 1,
            TotalPrice = getConfirmation.PaymentInformation.TotalAmount,
            PaidPrice = getConfirmation.PaymentInformation.TotalAmount,
            ChangePrice = getConfirmation.PaymentInformation.TotalAmount,
            IsInvoiced = false,
            IsDeleted = false,
            TotalTicket = getConfirmation.TicketInformations?.Sum(x => x.Piece) ?? 0,
            CreatedBy = _currentUser.GetRequiredUserId()
        };
        var reservationNewEntity = _reservationService.AddAndReturn(reservationEntity);
        #endregion

        #region Reservation Sale Invoice Info
        var r = reservationNewEntity.Data;
        var pi = getConfirmation.PersonalInformation;
        var pay = getConfirmation.PaymentInformation;

        var hasCounty = pi != null && pi.CountyId.HasValue && pi.CountyId.Value > 0;
        var hasPersonal =
            pi != null &&
            !string.IsNullOrWhiteSpace(pi.FirstName) &&
            !string.IsNullOrWhiteSpace(pi.Surname) &&
            !string.IsNullOrWhiteSpace(pi.Email) &&
            !string.IsNullOrWhiteSpace(pi.Phone) &&
            hasCounty;

        var hasPaymentType = pay != null && pay.PaymentTypeId > 0;
        if (hasPersonal || hasPaymentType)
        {
            var inv = new ReservationSaleInvoiceInfoEntity
            {
                ReservationId = r.Id,
                CreatedBy = _currentUser.GetRequiredUserId(),
                PaymentTypeId = hasPaymentType ? pay!.PaymentTypeId : (long?)null
            };
            if (hasPersonal)
            {
                inv.Name = pi!.FirstName;
                inv.Surname = pi.Surname;
                inv.Email = pi.Email;
                inv.Phone = pi.Phone;
                inv.CountyId = pi.CountyId;
            }

            inv.BankId = 1;
            inv.GuideId = 1;
            inv.InvoiceTypeId = 1;
            _reservationSaleInvoiceInfoService.Add(inv);
        }
        #endregion

        #region Reservation Detail (Tickets)
        if (getConfirmation.TicketInformations != null)
        {
            foreach (var ticketInformation in getConfirmation.TicketInformations)
            {
                var eventTicketPrice = _eventTicketPriceService.GetFirstOrDefault(x => x.Id == ticketInformation.EventTicketPriceId);
                var ticketSubType = _ticketSubTypeService.GetFirstOrDefault(x => x.TicketTypeId == eventTicketPrice.Data.TicketTypeId);

                var rd = new ReservationDetailEntity
                {
                    ReservationId = reservationNewEntity.Data.Id,
                    TicketTypeId = eventTicketPrice.Data.TicketTypeId,
                    TicketSubTypeId = ticketSubType.Data.Id,
                    NumberOfTickets = ticketInformation.Piece
                };
                _reservationDetailService.AddAndReturn(rd);
            }
        }
        #endregion

        #region Reservation Product Sales
        foreach (var p in getConfirmation.ProductInformations)
        {
            var line = new ReservationProductDetailEntity
            {
                ReservationId = reservationNewEntity.Data.Id,
                ProductId = p.ProductId,
                Piece = p.Piece,
                CreatedBy = _currentUser.GetRequiredUserId()
            };
            _reservationProductDetailService.Add(line);
        }
        return Ok();
        #endregion

        #region 
        //TODO:Ticket Add Logic
        #endregion
    }
}
