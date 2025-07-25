using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
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

    public HomeController(
        IPersonnelService personnelService,
        IProductSaleService productSaleService,
        IProductSaleDetailService productSaleDetailService,
        IProductService productService,
        IProductPriceService productPriceService,
        IPersonnelRoleService personnelRoleService,
        IRoleService roleService)
    {
        _personnelService = personnelService;
        _productSaleService = productSaleService;
        _productSaleDetailService = productSaleDetailService;
        _productService = productService;
        _productPriceService = productPriceService;
        _personnelRoleService = personnelRoleService;
        _roleService = roleService;
    }

    public async Task<IActionResult> Index()
    {
        var personnelIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        var companyIdentifierClaim = User.FindFirst("CompanyIdentifier");

        if (personnelIdClaim == null || companyIdentifierClaim == null)
            return RedirectToAction("Login", "Authorization");

        long personnelId = Convert.ToInt64(personnelIdClaim.Value);
        string companyIdentifier = companyIdentifierClaim.Value;

        var personnelResult = _personnelService.GetById(personnelId);
        if (!personnelResult.Success || personnelResult.Data == null)
            return RedirectToAction("Login", "Authorization");

        ViewBag.CurrentUser = personnelResult.Data;


        var personnelList = _personnelService
                .GetList(x => x.CompanyIdentifier == companyIdentifier && x.Id != personnelId && !x.IsDeleted)
                .Data?.ToList() ?? new();
        ViewBag.PersonnelList = personnelList;

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

        var grouped = todaySales.GroupBy(s => s.CreatedBy).Select(g =>
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

        var maxSales = grouped.Any() ? grouped.Max(x => x.TotalSales) : 0;
        var dailySellers = grouped.Select(x => new
        {
            x.Id,
            x.FirstName,
            x.LastName,
            x.ProfilePhotoPath,
            x.Date,
            x.TotalSales,
            Percent = maxSales > 0 ? (int)Math.Round((double)x.TotalSales / maxSales * 100) : 0
        }).ToList();

        ViewBag.DailySellers = dailySellers;
        #endregion

        #region Ürün Satış Tablosu
        var salesList = _productSaleService.GetList(x => x.Status == 1).Data;
        var saleIds = salesList.Select(x => x.Id).ToList();
        var saleDetails = _productSaleDetailService.GetList(x => saleIds.Contains(x.ProductSaleId)).Data;
        var productIds = saleDetails.Select(x => x.ProductId).Distinct().ToList();
        var products = _productService.GetList(x => productIds.Contains(x.Id)).Data;
        var prices = _productPriceService.GetList(x => productIds.Contains(x.ProductId)).Data;
        var salePersonnelIds = salesList.Select(x => x.CreatedBy).Distinct().ToList();
        var salePersonnel = _personnelService.GetList(x => salePersonnelIds.Contains(x.Id)).Data;

        var salesTableData = salesList.Select(sale =>
        {
            var person = salePersonnel.FirstOrDefault(p => p.Id == sale.CreatedBy);
            var detailItems = saleDetails.Where(d => d.ProductSaleId == sale.Id).ToList();

            decimal totalPrice = 0;
            string detailHtml = "";

            foreach (var d in detailItems)
            {
                var product = products.FirstOrDefault(p => p.Id == d.ProductId);
                var price = prices.FirstOrDefault(pr => pr.ProductId == d.ProductId);
                var linePrice = (price?.Price ?? 240) * d.Quantity;
                totalPrice += linePrice;
                // detailHtml += $"<div><strong>{product?.Name}</strong> x {d.Quantity}</div>";
            }

            return new
            {
                CustomerName = $"{person?.FirstName} {person?.LastName}",
                Avatar = person?.ProfilePhotoPath ?? "/assets/images/faces/default.jpg",
                OrderId = $"#{sale.Id}",
                Date = sale.Created_Date.ToString("dd.MM.yyyy"),
                TotalPrice = $"{totalPrice:0.00} ₺",
                PaymentMethod = "Gişe",
                Status = sale.Status == 1 ? "Satıldı" : "Bekliyor",
                DetailHtml = detailHtml
            };
        }).ToList();

        ViewBag.RecentSales = salesTableData;
        #endregion

        return View();
    }
}
