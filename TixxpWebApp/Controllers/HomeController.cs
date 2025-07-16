using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Entities.Personnel;

namespace Tixxp.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPersonnelService _personnelService;
        private readonly IProductService _productService;
        private readonly IProductSaleService _productSaleService;
        private readonly IProductPriceService _productPriceService;
        private readonly IProductSaleDetailService _productSaleDetailService;

        public HomeController(IPersonnelService personnelService, IProductSaleService productSaleService, IProductSaleDetailService productSaleDetailService, IProductService productService, IProductPriceService productPriceService)
        {
            _personnelService = personnelService;
            _productSaleService = productSaleService;
            _productSaleDetailService = productSaleDetailService;
            _productService = productService;
            _productPriceService = productPriceService;
        }

        public IActionResult Index()
        {
            #region Kullanıcıalr
            var personnelIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (personnelIdClaim == null)
                return RedirectToAction("Test", "Auth");

            long personnelId = Convert.ToInt64(personnelIdClaim.Value);

            var personnelResult = _personnelService.GetById(personnelId);
            if (!personnelResult.Success || personnelResult.Data == null)
                return RedirectToAction("Login", "Auth");

            var companyIdentifier = personnelResult.Data.CompanyIdentifier;

            var personnelListResult = _personnelService.GetList(x => x.CompanyIdentifier == companyIdentifier && x.Id != personnelId && !x.IsDeleted);
            var personnelList = personnelListResult?.Data?.ToList() ?? new List<PersonnelEntity>();
            ViewBag.PersonnelList = personnelList;
            #endregion

            #region Giriş Yapan Kullanıcı
            personnelResult.Data.Updated_Date = DateTime.Now;
            ViewBag.CurrentUser = personnelResult.Data;
            #endregion

            #region Gişe Satışarı
            var productSalesResult = _productSaleService.GetListWithInclude(x => x.Status == 1, x => x.Counter);
            var salesGroupedByCounter = productSalesResult
                .Data
                .Where(x => x.Counter != null)
                .GroupBy(x => x.Counter.CounterName)
                .Select(g => new
                {
                    CounterName = g.Key,
                    TotalSales = g.Count()
                }).ToList();

            ViewBag.CounterSales = salesGroupedByCounter;
            #endregion

            #region Günlük Satış Yapan Kullanıcılar
            var today = DateTime.Today;

            // 1. Bugünün satışlarını al
            var sales = _productSaleService.GetList(x =>
                x.Status == 1 &&
                x.Created_Date.Date == today).Data;

            // 2. Satış yapan personel ID'lerini al
            var personnelIds = sales
                .Where(x => x.CreatedBy != null)
                .Select(x => x.CreatedBy)
                .Distinct()
                .ToList();

            // 3. İlgili personelleri tek seferde çek
            var salerPersonnelList = _personnelService.GetList(x => personnelIds.Contains(x.Id)).Data;

            // 4. Satışları personel bazında grupla
            var grouped = sales
                .GroupBy(s => s.CreatedBy)
                .Select(g =>
                {
                    var personnel = salerPersonnelList.FirstOrDefault(p => p.Id == g.Key);
                    return new
                    {
                        Id = g.Key,
                        FirstName = personnel?.FirstName ?? "Bilinmiyor",
                        LastName = personnel?.LastName ?? "",
                        ProfilePhotoPath = personnel?.ProfilePhotoPath ?? "/assets/images/faces/default.jpg",
                        Date = today,
                        TotalSales = g.Count()
                    };
                })
                .ToList();

            // 5. Max satışa göre yüzde hesapla
            var maxSales = grouped.Any() ? grouped.Max(x => x.TotalSales) : 0;
            var viewData = grouped.Select(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.ProfilePhotoPath,
                x.Date,
                x.TotalSales,
                Percent = maxSales > 0 ? (int)Math.Round((double)x.TotalSales / maxSales * 100) : 0
            }).ToList();


            ViewBag.DailySellers = viewData;
            #endregion


            #region Satışlar Tablosu (ProductSale + Detaylar)
            var salesList = _productSaleService.GetList(x => x.Status == 1).Data;
            var saleIds = salesList.Select(x => x.Id).ToList();

            // Detaylar
            var saleDetails = _productSaleDetailService.GetList(x => saleIds.Contains(x.ProductSaleId)).Data;
            var productIds = saleDetails.Select(x => x.ProductId).Distinct().ToList();

            // Ürün ve fiyat verileri
            var products = _productService.GetList(x => productIds.Contains(x.Id)).Data;
            var prices = _productPriceService.GetList(x => productIds.Contains(x.ProductId)).Data;

            // Satışı yapan personeller
            var salePersonnelIds = salesList.Select(x => x.CreatedBy).Distinct().ToList();
            var salePersonnel = _personnelService.GetList(x => salePersonnelIds.Contains(x.Id)).Data;

            // Hazırla
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

                    //detailHtml += $"<div><strong>{product?.Name}</strong> x {d.Quantity}</div>";
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
}
