using Microsoft.AspNetCore.Mvc;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.WebApp.Models.Report;

namespace Tixxp.WebApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly IProductSaleService _productSaleService;
        private readonly IProductSaleDetailService _productSaleDetailService;

        public ReportController(IProductSaleService productSaleService, IProductSaleDetailService productSaleDetailService)
        {
            _productSaleService = productSaleService;
            _productSaleDetailService = productSaleDetailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductSaleReport()
        {
            var result = _productSaleDetailService.GetListWithInclude(x => !x.IsDeleted, x => x.Product);

            // Günlük veriler
            var groupedDaily = result.Data
                .GroupBy(x => new
                {
                    Date = x.Created_Date.ToString("yyyy-MM-dd"),
                    ProductName = x.Product.Name
                })
                .Select(g => new ProductSaleStackedReportDto
                {
                    Date = g.Key.Date,
                    ProductName = g.Key.ProductName,
                    Quantity = g.Sum(x => x.Quantity)
                }).ToList();

            // Aylık veriler
            var groupedMonthly = result.Data
                .GroupBy(x => new
                {
                    Date = x.Created_Date.ToString("yyyy-MM"),
                    ProductName = x.Product.Name
                })
                .Select(g => new ProductSaleStackedReportDto
                {
                    Date = g.Key.Date,
                    ProductName = g.Key.ProductName,
                    Quantity = g.Sum(x => x.Quantity)
                }).ToList();

            ViewBag.ProductSaleChartData = groupedDaily;
            ViewBag.MonthlyProductSaleChartData = groupedMonthly;

            return View("ProductSaleReport");
        }
    }
}
