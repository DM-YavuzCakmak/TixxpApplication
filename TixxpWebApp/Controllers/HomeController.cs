using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Security.Claims;
using Tixxp.Business.DataTransferObjects.Campaign;
using Tixxp.Business.Services.Abstract.Campaign;
using Tixxp.Business.Services.Abstract.CampaignCondition;
using Tixxp.Business.Services.Abstract.CounterTranslation;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Business.Services.Abstract.ProductSaleStatus;
using Tixxp.Business.Services.Abstract.ProductSaleStatusTranslation;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Business.Services.Concrete.CampaignCondition;
using Tixxp.Core.Utilities.Enums.ProductSaleStatusEnum;
using Tixxp.Entities.Personnel;
using Tixxp.WebApp.Models.Home.GetProductSaleDetail;

namespace Tixxp.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IPersonnelService _personnelService;
    private readonly IRoleService _roleService;
    private readonly IPersonnelRoleService _personnelRoleService;
    private readonly IProductService _productService;
    private readonly IProductSaleService _productSaleService;
    private readonly ICounterTranslationService _counterTranslationService;
    private readonly IProductPriceService _productPriceService;
    private readonly IProductSaleDetailService _productSaleDetailService;
    private readonly IProductTranslationService _productTranslationService;
    private readonly IReservationService _reservationService;
    private readonly ILanguageService _languageService;
    private readonly IProductSaleStatusService _productSaleStatusService;
    private readonly IProductSaleInvoiceInfoService _productSaleInvoiceInfoService;
    private readonly IProductSaleStatusTranslationService _productSaleStatusTranslationService;
    private readonly ICampaignService _campaignService;
    private readonly ICampaignConditionService _campaignConditionService;

    private readonly IStringLocalizer<HomeController> _localizer;


    public HomeController(
        IPersonnelService personnelService,
        IProductSaleService productSaleService,
        IProductSaleDetailService productSaleDetailService,
        IProductService productService,
        IProductPriceService productPriceService,
        IPersonnelRoleService personnelRoleService,
        IRoleService roleService,
        IProductTranslationService productTranslationService,
        ILanguageService languageService,
        IReservationService reservationService,
        ICounterTranslationService counterTranslationService,
        IProductSaleStatusService productSaleStatusService,
        IProductSaleStatusTranslationService productSaleStatusTranslationService,
        IProductSaleInvoiceInfoService productSaleInvoiceInfoService,
        ICampaignService campaignService,
        ICampaignConditionService campaignConditionService,
        IStringLocalizer<HomeController> localizer)
    {
        _personnelService = personnelService;
        _productSaleService = productSaleService;
        _productSaleDetailService = productSaleDetailService;
        _productService = productService;
        _productPriceService = productPriceService;
        _personnelRoleService = personnelRoleService;
        _roleService = roleService;
        _productTranslationService = productTranslationService;
        _languageService = languageService;
        _reservationService = reservationService;
        _counterTranslationService = counterTranslationService;
        _productSaleStatusService = productSaleStatusService;
        _productSaleStatusTranslationService = productSaleStatusTranslationService;
        _productSaleInvoiceInfoService = productSaleInvoiceInfoService;
        _campaignService = campaignService;
        _campaignConditionService = campaignConditionService;
        _localizer = localizer;
    }

    public async Task<IActionResult> Index()
    {
        #region Kullanıcı Giriş Kontrolü
        var personnelIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var companyIdentifierClaim = User.FindFirst("CompanyIdentifier");

        if (personnelIdClaim == null || companyIdentifierClaim == null)
            return RedirectToAction("Login", "Authorization");

        long personnelId = Convert.ToInt64(personnelIdClaim.Value);
        string companyIdentifier = companyIdentifierClaim.Value;

        var cultureCode = CultureInfo.CurrentUICulture.Name;
        var languageResult = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
        long? languageId = languageResult.Success ? languageResult.Data?.Id : null;

        var personnelResult = _personnelService.GetById(personnelId);
        if (!personnelResult.Success || personnelResult.Data == null)
            return RedirectToAction("Login", "Authorization");
        ViewBag.CurrentUser = personnelResult.Data;
        #endregion

        #region Kullanıcılar
        var personnelList = _personnelService
            .GetList(x => x.CompanyIdentifier == companyIdentifier && x.Id != personnelId && !x.IsDeleted)
            .Data?.ToList() ?? new();
        ViewBag.PersonnelList = personnelList;
        #endregion

        #region Gişe Satışları
        var productSalesResult = _productSaleService.GetListWithInclude(x => x.StatusId == 1, x => x.Counter);
        var counterIds = productSalesResult.Data
            .Where(x => x.CounterId != null)
            .Select(x => x.CounterId)
            .Distinct()
            .ToList();

        var counterTranslations = _counterTranslationService
            .GetList(x => counterIds.Contains(x.CounterId) && x.LanguageId == languageId)
            .Data;

        var salesGroupedByCounter = productSalesResult.Data
            .Where(x => x.CounterId != null)
            .GroupBy(x => x.CounterId)
            .Select(g =>
            {
                var translation = counterTranslations.FirstOrDefault(ct => ct.CounterId == g.Key);
                return new
                {
                    CounterName = translation?.Name ?? "Unknown Counter",
                    TotalSales = g.Count()
                };
            }).ToList();

        ViewBag.CounterSales = salesGroupedByCounter;
        #endregion


        #region Günlük Satış Yapan Personeller
        var today = DateTime.Today;
        var todaySales = _productSaleService.GetList(x => x.StatusId == (long)ProductSaleStatusEnum.Completed && x.Created_Date.Date == today).Data;
        var todayPersonnelIds = todaySales.Where(x => x.CreatedBy != null).Select(x => x.CreatedBy).Distinct().ToList();
        var todayPersonnelList = _personnelService.GetList(x => todayPersonnelIds.Contains(x.Id)).Data;

        var groupedDaily = todaySales.GroupBy(s => s.CreatedBy).Select(g =>
        {
            var personnel = todayPersonnelList.FirstOrDefault(p => p.Id == g.Key);
            return new
            {
                Id = g.Key,
                FirstName = personnel?.FirstName ?? "Bilinmiyor",
                LastName = personnel?.LastName ?? "",
                ProfilePhotoPath = personnel?.ProfilePhotoPath ?? "/assets/images/faces/default.jpg",
                Date = today,
                TotalSales = g.Count()
            };
        }).ToList();

        var maxSalesDaily = groupedDaily.Any() ? groupedDaily.Max(x => x.TotalSales) : 0;
        var dailySellers = groupedDaily.Select(x => new
        {
            x.Id,
            x.FirstName,
            x.LastName,
            x.ProfilePhotoPath,
            x.Date,
            x.TotalSales,
            Percent = maxSalesDaily > 0 ? (int)Math.Round((double)x.TotalSales / maxSalesDaily * 100) : 0
        }).ToList();
        ViewBag.DailySellers = dailySellers;
        #endregion

        #region Aylık Satış Yapan Personeller
        var now = DateTime.Now;
        var monthlySales = _productSaleService
            .GetList(x => x.StatusId == 1 &&
                          x.Created_Date.Year == now.Year &&
                          x.Created_Date.Month == now.Month).Data;

        var monthlyPersonnelIds = monthlySales.Where(x => x.CreatedBy != null).Select(x => x.CreatedBy).Distinct().ToList();
        var monthlyPersonnelList = _personnelService.GetList(x => monthlyPersonnelIds.Contains(x.Id)).Data;

        var groupedMonthly = monthlySales.GroupBy(s => s.CreatedBy).Select(g =>
        {
            var personnel = monthlyPersonnelList.FirstOrDefault(p => p.Id == g.Key);
            return new
            {
                Id = g.Key,
                FirstName = personnel?.FirstName ?? "Bilinmiyor",
                LastName = personnel?.LastName ?? "",
                ProfilePhotoPath = personnel?.ProfilePhotoPath ?? "/assets/images/faces/default.jpg",
                Date = now,
                TotalSales = g.Count()
            };
        }).ToList();

        var maxSalesMonthly = groupedMonthly.Any() ? groupedMonthly.Max(x => x.TotalSales) : 0;
        var monthlySellers = groupedMonthly.Select(x => new
        {
            x.Id,
            x.FirstName,
            x.LastName,
            x.ProfilePhotoPath,
            x.Date,
            x.TotalSales,
            Percent = maxSalesMonthly > 0 ? (int)Math.Round((double)x.TotalSales / maxSalesMonthly * 100) : 0
        }).ToList();
        ViewBag.MonthlyProductSellers = monthlySellers;
        #endregion


        #region Günlük Rezervasyon Yapan Personeller
        var todayReservations = _reservationService.GetList(x => x.StatusId == 1 && x.Created_Date.Date == today).Data;
        var todayReservationPersonnelIds = todayReservations.Where(x => x.CreatedBy != null).Select(x => x.CreatedBy).Distinct().ToList();
        var todayReservationPersonnelList = _personnelService.GetList(x => todayReservationPersonnelIds.Contains(x.Id)).Data;

        var groupedDailyRes = todayReservations.GroupBy(r => r.CreatedBy).Select(g =>
        {
            var personnel = todayReservationPersonnelList.FirstOrDefault(p => p.Id == g.Key);
            return new
            {
                Id = g.Key,
                FirstName = personnel?.FirstName ?? "Bilinmiyor",
                LastName = personnel?.LastName ?? "",
                ProfilePhotoPath = personnel?.ProfilePhotoPath ?? "/assets/images/faces/default.jpg",
                Date = today,
                TotalSales = g.Count()
            };
        }).ToList();

        var maxDailyRes = groupedDailyRes.Any() ? groupedDailyRes.Max(x => x.TotalSales) : 0;
        var dailyReservationSellers = groupedDailyRes.Select(x => new
        {
            x.Id,
            x.FirstName,
            x.LastName,
            x.ProfilePhotoPath,
            x.Date,
            x.TotalSales,
            Percent = maxDailyRes > 0 ? (int)Math.Round((double)x.TotalSales / maxDailyRes * 100) : 0
        }).ToList();

        ViewBag.DailyReservationSellers = dailyReservationSellers;
        #endregion

        #region Aylık Rezervasyon Yapan Personeller
        var monthlyReservations = _reservationService
            .GetList(x => x.StatusId == 1 &&
                          x.Created_Date.Year == now.Year &&
                          x.Created_Date.Month == now.Month).Data;

        var monthlyReservationPersonnelIds = monthlyReservations.Where(x => x.CreatedBy != null).Select(x => x.CreatedBy).Distinct().ToList();
        var monthlyReservationPersonnelList = _personnelService.GetList(x => monthlyReservationPersonnelIds.Contains(x.Id)).Data;

        var groupedMonthlyRes = monthlyReservations.GroupBy(r => r.CreatedBy).Select(g =>
        {
            var personnel = monthlyReservationPersonnelList.FirstOrDefault(p => p.Id == g.Key);
            return new
            {
                Id = g.Key,
                FirstName = personnel?.FirstName ?? "Bilinmiyor",
                LastName = personnel?.LastName ?? "",
                ProfilePhotoPath = personnel?.ProfilePhotoPath ?? "/assets/images/faces/default.jpg",
                Date = now,
                TotalSales = g.Count()
            };
        }).ToList();

        var maxMonthlyRes = groupedMonthlyRes.Any() ? groupedMonthlyRes.Max(x => x.TotalSales) : 0;
        var monthlyReservationSellers = groupedMonthlyRes.Select(x => new
        {
            x.Id,
            x.FirstName,
            x.LastName,
            x.ProfilePhotoPath,
            x.Date,
            x.TotalSales,
            Percent = maxMonthlyRes > 0 ? (int)Math.Round((double)x.TotalSales / maxMonthlyRes * 100) : 0
        }).ToList();

        ViewBag.MonthlyReservationSellers = monthlyReservationSellers;
        #endregion


        #region İstatistikler için tarih verileri
        var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
        var firstDayOfPrevMonth = firstDayOfMonth.AddMonths(-1);
        var lastDayOfPrevMonth = firstDayOfMonth.AddDays(-1);
        var yesterday = today.AddDays(-1);

        var allReservations = _reservationService.GetList(x => x.StatusId == 1).Data;

        var dailyReservationCount = allReservations.Count(x => x.Created_Date.Date == today);
        var yesterdayReservationCount = allReservations.Count(x => x.Created_Date.Date == yesterday);
        var monthlyReservationCount = allReservations.Count(x => x.Created_Date >= firstDayOfMonth);
        var prevMonthReservationCount = allReservations.Count(x => x.Created_Date >= firstDayOfPrevMonth && x.Created_Date <= lastDayOfPrevMonth);

        var allTicketSales = _productSaleService.GetList(x => x.StatusId == (long)ProductSaleStatusEnum.Completed).Data;

        var dailyTicketCount = allTicketSales.Count(x => x.Created_Date.Date == today);
        var yesterdayTicketCount = allTicketSales.Count(x => x.Created_Date.Date == yesterday);
        var monthlyTicketCount = allTicketSales.Count(x => x.Created_Date >= firstDayOfMonth);
        var prevMonthTicketCount = allTicketSales.Count(x => x.Created_Date >= firstDayOfPrevMonth && x.Created_Date <= lastDayOfPrevMonth);

        ViewBag.Stats = new
        {
            DailyReservation = new
            {
                Count = dailyReservationCount,
                ChangePercent = Math.Round(CalculateChange(dailyReservationCount, yesterdayReservationCount), 2),
                PercentOfPrevious = yesterdayReservationCount > 0
                    ? (int)Math.Round((double)dailyReservationCount / yesterdayReservationCount * 100)
                    : (dailyReservationCount > 0 ? 100 : 0)
            },
            MonthlyReservation = new
            {
                Count = monthlyReservationCount,
                ChangePercent = Math.Round(CalculateChange(monthlyReservationCount, prevMonthReservationCount), 2),
                PercentOfPrevious = prevMonthReservationCount > 0
                    ? (int)Math.Round((double)monthlyReservationCount / prevMonthReservationCount * 100)
                    : (monthlyReservationCount > 0 ? 100 : 0)
            },
            DailyTicket = new
            {
                Count = dailyTicketCount,
                ChangePercent = Math.Round(CalculateChange(dailyTicketCount, yesterdayTicketCount), 2),
                PercentOfPrevious = yesterdayTicketCount > 0
                    ? (int)Math.Round((double)dailyTicketCount / yesterdayTicketCount * 100)
                    : (dailyTicketCount > 0 ? 100 : 0)
            },
            MonthlyTicket = new
            {
                Count = monthlyTicketCount,
                ChangePercent = Math.Round(CalculateChange(monthlyTicketCount, prevMonthTicketCount), 2),
                PercentOfPrevious = prevMonthTicketCount > 0
                    ? (int)Math.Round((double)monthlyTicketCount / prevMonthTicketCount * 100)
                    : (monthlyTicketCount > 0 ? 100 : 0)
            }
        };

        #endregion

        return View();
    }

    double CalculateChange(int current, int previous)
    {
        if (previous == 0) return current > 0 ? 100 : 0;
        return ((double)(current - previous) / previous) * 100;
    }

    [HttpGet]
    public IActionResult GetSalesTable()
    {
        var cultureCode = CultureInfo.CurrentUICulture.Name;
        var languageResult = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
        long? languageId = languageResult.Success ? languageResult.Data?.Id : null;

        int start = Convert.ToInt32(Request.Query["start"]);
        int length = Convert.ToInt32(Request.Query["length"]);
        int page = (start / length) + 1;
        int pageSize = length;

        string sortColumnIndex = Request.Query["order[0][column]"];
        string sortColumnName = Request.Query[$"columns[{sortColumnIndex}][data]"];
        string sortDirection = Request.Query["order[0][dir]"];
        string searchValue = Request.Query["search[value]"];

        var statusTranslations = (languageId.HasValue)
            ? _productSaleStatusTranslationService.GetList(x => x.LanguageId == languageId.Value).Data
            : _productSaleStatusTranslationService.GetList(x => true).Data;

        var statusDict = statusTranslations?
            .GroupBy(st => st.ProductSaleStatusId)
            .ToDictionary(g => g.Key, g => g.First().Name)
            ?? new Dictionary<long, string>();


        var salesList = _productSaleService.GetList(x => x.StatusId == (long)ProductSaleStatusEnum.Completed).Data;
        var saleIds = salesList.Select(x => x.Id).ToList();
        var saleDetails = _productSaleDetailService.GetList(x => saleIds.Contains(x.ProductSaleId)).Data;
        var productIds = saleDetails.Select(x => x.ProductId).Distinct().ToList();

        var counterIds = salesList
            .Where(x => x.CounterId != null)
            .Select(x => x.CounterId)
            .Distinct()
            .ToList();

        var counterTranslations = _counterTranslationService
            .GetList(x => counterIds.Contains(x.CounterId) && x.LanguageId == languageId)
            .Data;

        var productTranslations = _productTranslationService
            .GetList(x => productIds.Contains(x.ProductId) && x.LanguageId == languageId).Data;

        var products = _productService.GetList(x => productIds.Contains(x.Id)).Data;
        var prices = _productPriceService.GetListWithInclude(x => productIds.Contains(x.ProductId), x => x.CurrencyType).Data;
        var salePersonnelIds = salesList.Select(x => x.CreatedBy).Distinct().ToList();
        var salePersonnel = _personnelService.GetList(x => salePersonnelIds.Contains(x.Id)).Data;

        var salesData = salesList.Select(sale =>
        {
            var person = salePersonnel.FirstOrDefault(p => p.Id == sale.CreatedBy);
            var detailItems = saleDetails.Where(d => d.ProductSaleId == sale.Id).ToList();
            decimal totalPrice = 0;
            var detailList = new List<object>();

            foreach (var d in detailItems)
            {
                var product = products.FirstOrDefault(p => p.Id == d.ProductId);
                var translation = productTranslations.FirstOrDefault(t => t.ProductId == d.ProductId);
                var productName = translation?.Name ?? product?.Code;
                var price = prices.FirstOrDefault(pr => pr.ProductId == d.ProductId);
                var linePrice = (price?.Price ?? 240) * d.Quantity;
                totalPrice += linePrice;

                detailList.Add(new
                {
                    ProductName = productName,
                    Quantity = d.Quantity
                });
            }

            var counterTranslation = counterTranslations.FirstOrDefault(ct => ct.CounterId == sale.CounterId);
            var paymentMethod = counterTranslation?.Name ?? "Unknown Counter";

            return new
            {
                CustomerName = $"{person?.FirstName} {person?.LastName}",
                Avatar = person?.ProfilePhotoPath ?? "/assets/images/faces/default.jpg",
                OrderId = sale.Id,
                OrderIdFormatted = $"#{sale.Id}",
                Date = sale.Created_Date.ToString("dd.MM.yyyy HH:mm"),
                DateFormatted = sale.Created_Date.ToString("dd.MM.yyyy"),
                TotalPriceRaw = totalPrice,
                TotalPrice = $"{totalPrice:0.00} {prices[0].CurrencyType.Symbol}",
                PaymentMethod = paymentMethod,
                Status = statusDict.ContainsKey(sale.StatusId) ? statusDict[sale.StatusId] : $"#Unknown",
                Details = detailList
            };
        }).OrderByDescending(x => x.Date).ToList();



        ViewBag.RecentSales = salesData;

        // 🔍 Filtreleme (search)
        if (!string.IsNullOrEmpty(searchValue))
        {
            searchValue = searchValue.ToLower();
            salesData = salesData.Where(x =>
                x.CustomerName?.ToLower().Contains(searchValue) == true ||
                x.OrderIdFormatted?.ToLower().Contains(searchValue) == true ||
                x.DateFormatted?.Contains(searchValue) == true ||
                x.TotalPrice?.ToLower().Contains(searchValue) == true ||
                x.PaymentMethod?.ToLower().Contains(searchValue) == true ||
                x.Status?.ToLower().Contains(searchValue) == true
            ).ToList();
        }

        // 🔃 Sıralama
        salesData = (sortColumnName, sortDirection) switch
        {
            ("customerName", "asc") => salesData.OrderBy(x => x.CustomerName).ToList(),
            ("customerName", "desc") => salesData.OrderByDescending(x => x.CustomerName).ToList(),

            ("orderId", "asc") => salesData.OrderBy(x => x.OrderId).ToList(),
            ("orderId", "desc") => salesData.OrderByDescending(x => x.OrderId).ToList(),

            ("date", "asc") => salesData.OrderBy(x => x.Date).ToList(),
            ("date", "desc") => salesData.OrderByDescending(x => x.Date).ToList(),

            ("totalPriceRaw", "asc") => salesData.OrderBy(x => x.TotalPriceRaw).ToList(),
            ("totalPriceRaw", "desc") => salesData.OrderByDescending(x => x.TotalPriceRaw).ToList(),

            ("paymentMethod", "asc") => salesData.OrderBy(x => x.PaymentMethod).ToList(),
            ("paymentMethod", "desc") => salesData.OrderByDescending(x => x.PaymentMethod).ToList(),

            ("status", "asc") => salesData.OrderBy(x => x.Status).ToList(),
            ("status", "desc") => salesData.OrderByDescending(x => x.Status).ToList(),

            _ => salesData.OrderByDescending(x => x.Date).ToList()
        };

        var totalFiltered = salesData.Count;

        var pagedData = salesData.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        return Json(new
        {
            success = true,
            total = totalFiltered,
            recordsTotal = totalFiltered,
            recordsFiltered = totalFiltered,
            page,
            pageSize,
            data = pagedData
        });
    }

    [HttpGet]
    public IActionResult GetProductSaleDetail(long productSaleId)
    {
        if (productSaleId <= 0)
            return PartialView("_ProductSaleDetail", null);

        var cultureCode = CultureInfo.CurrentUICulture.Name;
        var languageResult = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
        long? languageId = languageResult.Success ? languageResult.Data?.Id : null;

        // Ana satış
        var saleResult = _productSaleService.GetFirstOrDefault(x => x.Id == productSaleId);
        if (!saleResult.Success || saleResult.Data == null)
            return PartialView("_ProductSaleDetail", null);

        var sale = saleResult.Data;

        // Satış yapan personel
        var personnel = (sale.CreatedBy != null)
            ? _personnelService.GetFirstOrDefault(x => x.Id == sale.CreatedBy).Data
            : null;

        // Gişe bilgisi
        string counterName = "Unknown Counter";
        if (sale.CounterId != null)
        {
            var counterTranslation = _counterTranslationService.GetFirstOrDefault(
                x => x.CounterId == sale.CounterId && x.LanguageId == languageId
            );
            if (counterTranslation.Success && counterTranslation.Data != null)
                counterName = counterTranslation.Data.Name;
        }

        // Satış durumu
        var statusTranslation = _productSaleStatusTranslationService.GetFirstOrDefault(
            x => x.ProductSaleStatusId == sale.StatusId && x.LanguageId == languageId
        );
        string statusName = statusTranslation.Success ? statusTranslation.Data?.Name ?? "Unknown" : "Unknown";

        // Fatura / müşteri bilgisi
        var invoiceInfo = _productSaleInvoiceInfoService
            .GetFirstOrDefault(x => x.ProductSaleId == productSaleId).Data;

        // Ürün detayları
        var saleDetailsResult = _productSaleDetailService.GetListWithInclude(
            x => x.ProductSaleId == productSaleId,
            x => x.Product,
            x => x.CurrencyType
        );

        if (!saleDetailsResult.Success || saleDetailsResult.Data == null)
            return PartialView("_ProductSaleDetail", null);

        var saleDetails = saleDetailsResult.Data;
        var productIds = saleDetails.Select(d => d.ProductId).Distinct().ToList();

        var productTranslations = (languageId.HasValue)
            ? _productTranslationService.GetList(x =>
                productIds.Contains(x.ProductId) && x.LanguageId == languageId.Value).Data
            : new List<Tixxp.Entities.ProductTranslation.ProductTranslationEntity>();

        var productPrices = _productPriceService.GetList(x => productIds.Contains(x.ProductId)).Data;

        decimal subTotal = 0;
        string currencySymbol = "₺";
        var productList = new List<ProductSaleDetailItemVm>();

        foreach (var detail in saleDetails)
        {
            var product = detail.Product;
            var translation = productTranslations.FirstOrDefault(t => t.ProductId == detail.ProductId);
            var priceInfo = productPrices.FirstOrDefault(p => p.ProductId == detail.ProductId);
            var unitPrice = priceInfo?.Price ?? 0m;
            var lineTotal = Math.Round(unitPrice * detail.Quantity, 2);

            if (detail.CurrencyType != null && !string.IsNullOrWhiteSpace(detail.CurrencyType.Symbol))
                currencySymbol = detail.CurrencyType.Symbol;

            subTotal += lineTotal;

            productList.Add(new ProductSaleDetailItemVm
            {
                ProductId = detail.ProductId,
                ProductName = translation?.Name ?? product?.Code ?? "Unnamed Product",
                ProductCode = product?.Code,
                Quantity = detail.Quantity,
                UnitPrice = unitPrice,
                LineTotal = lineTotal,
                CurrencySymbol = currencySymbol,
                Image = product?.ImageFilePath
            });
        }

        // 🔥 Kampanya kontrolü
        decimal finalTotal = subTotal;
        decimal discountAmount = 0;
        string campaignName = "";

        if (sale.CampaignId.HasValue)
        {
            var campaignResult = _campaignService.GetById(sale.CampaignId.Value);
            if (campaignResult.Success && campaignResult.Data != null)
            {
                campaignName = campaignResult.Data.Name;

                var campaignConditionEntity = _campaignConditionService.GetFirstOrDefault(x => x.CampaignId == sale.CampaignId).Data;

                var applyDto = new ApplyCampaignForProductRequestDto
                {
                    CouponCode = campaignConditionEntity?.Value1 ?? ""
                };

                foreach (var item in productList)
                {
                    applyDto.Products.Add(new CartItemDto
                    {
                        ProductId = item.ProductId,
                        Price = item.UnitPrice,
                        Quantity = item.Quantity,
                        CurrencyTypeId = productPrices.FirstOrDefault(p => p.ProductId == item.ProductId).CurrencyTypeId 
                    });
                }

                var appliedPrice = _campaignService.ApplyCampaignsForProduct(applyDto, campaignResult.Data);
                if (appliedPrice < applyDto.SubTotal)
                {
                    discountAmount = Math.Round(applyDto.SubTotal - appliedPrice, 2);
                    finalTotal = Math.Round(appliedPrice, 2);
                }
            }
        }

        // 🔹 ViewModel
        var vm = new ProductSaleDetailVm
        {
            SaleId = sale.Id,
            StatusName = statusName,
            CounterName = counterName,
            CampaignName = campaignName,
            CreatedDate = sale.Created_Date,
            CreatedByName = personnel != null ? $"{personnel.FirstName} {personnel.LastName}" : "Unknown",
            CreatedByPhoto = personnel?.ProfilePhotoPath ?? "/assets/images/faces/default.jpg",
            CustomerFullName = $"{invoiceInfo?.FirstName} {invoiceInfo?.LastName}",
            CustomerIdentityNumber = invoiceInfo?.IdentityNumber,
            Products = productList,
            TotalPrice = finalTotal,
            OriginalTotalPrice = subTotal,
            DiscountAmount = discountAmount,
            CurrencySymbol = currencySymbol
        };

        return PartialView("_ProductSaleDetail", vm);
    }

    [HttpPost]
    public IActionResult CancelSale(long saleId)
    {
        try
        {
            if (saleId <= 0)
                return Json(new
                {
                    success = false,
                    message = _localizer["homeController.CANCEL.INVALID_SALE_ID"].ToString()
                });

            // 🔍 Satışı getir
            var saleResult = _productSaleService.GetFirstOrDefault(x => x.Id == saleId);
            if (!saleResult.Success || saleResult.Data == null)
                return Json(new
                {
                    success = false,
                    message = _localizer["homeController.CANCEL.SALE_NOT_FOUND"].ToString()
                });

            var sale = saleResult.Data;

            // ⛔ Zaten iptal edilmişse
            if (sale.StatusId == (long)ProductSaleStatusEnum.Cancelled)
                return Json(new
                {
                    success = false,
                    message = _localizer["homeController.CANCEL.ALREADY_CANCELLED"].ToString()
                });

            // ✅ Satışı iptal et
            sale.StatusId = (long)ProductSaleStatusEnum.Cancelled;
            sale.Updated_Date = DateTime.Now;

            // 🧑‍💼 Güncelleyen kullanıcıyı yaz
            var personnelIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (personnelIdClaim != null)
                sale.UpdatedBy = Convert.ToInt64(personnelIdClaim.Value);

            // 💾 Veritabanında güncelle
            var updateResult = _productSaleService.Update(sale);
            if (!updateResult.Success)
                return Json(new
                {
                    success = false,
                    message = _localizer["homeController.CANCEL.FAILED"].ToString()
                });

            return Json(new
            {
                success = true,
                message = _localizer["homeController.CANCEL.SUCCESS"].ToString(),
                saleId = sale.Id
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                success = false,
                message = _localizer["homeController.CANCEL.EXCEPTION"].ToString(),
                error = ex.Message
            });
        }
    }
}
