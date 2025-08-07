using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Business.Services.Abstract.Reservation;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Entities.Personnel;

namespace Tixxp.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IPersonnelService _personnelService;
    private readonly IRoleService _roleService;
    private readonly IPersonnelRoleService _personnelRoleService;
    private readonly IProductService _productService;
    private readonly IProductSaleService _productSaleService;
    private readonly IProductPriceService _productPriceService;
    private readonly IProductSaleDetailService _productSaleDetailService;
    private readonly IProductTranslationService _productTranslationService;
    private readonly IReservationService _reservationService;
    private readonly ILanguageService _languageService;

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
        IReservationService reservationService)
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
        var productSalesResult = _productSaleService.GetListWithInclude(x => x.Status == 1, x => x.Counter);
        var salesGroupedByCounter = productSalesResult.Data
            .Where(x => x.Counter != null)
            .GroupBy(x => x.Counter.CounterName)
            .Select(g => new
            {
                CounterName = g.Key,
                TotalSales = g.Count()
            }).ToList();
        ViewBag.CounterSales = salesGroupedByCounter;
        #endregion

        #region Günlük Satış Yapan Personeller
        var today = DateTime.Today;
        var todaySales = _productSaleService.GetList(x => x.Status == 1 && x.Created_Date.Date == today).Data;
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
            .GetList(x => x.Status == 1 &&
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

        var allTicketSales = _productSaleService.GetList(x => x.Status == 1).Data;

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

        var salesList = _productSaleService.GetList(x => x.Status == 1).Data;
        var saleIds = salesList.Select(x => x.Id).ToList();
        var saleDetails = _productSaleDetailService.GetList(x => saleIds.Contains(x.ProductSaleId)).Data;
        var productIds = saleDetails.Select(x => x.ProductId).Distinct().ToList();

        var productTranslations = _productTranslationService
            .GetList(x => productIds.Contains(x.ProductId) && x.LanguageId == languageId).Data;

        var products = _productService.GetList(x => productIds.Contains(x.Id)).Data;
        var prices = _productPriceService.GetList(x => productIds.Contains(x.ProductId)).Data;
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

            return new
            {
                CustomerName = $"{person?.FirstName} {person?.LastName}",
                Avatar = person?.ProfilePhotoPath ?? "/assets/images/faces/default.jpg",
                OrderId = sale.Id,
                OrderIdFormatted = $"#{sale.Id}",
                Date = sale.Created_Date,
                DateFormatted = sale.Created_Date.ToString("dd.MM.yyyy"),
                TotalPriceRaw = totalPrice,
                TotalPrice = $"{totalPrice:0.00} ₺",
                PaymentMethod = "Gişe",
                Status = sale.Status == 1 ? "Satıldı" : "Bekliyor",
                Details = detailList
            };
        }).ToList();

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

}
