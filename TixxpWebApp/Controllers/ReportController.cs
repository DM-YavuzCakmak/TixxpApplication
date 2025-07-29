using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductTranslation;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.WebApp.Models.Report;

namespace Tixxp.WebApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly IProductSaleService _productSaleService;
        private readonly IProductSaleDetailService _productSaleDetailService;
        private readonly IProductTranslationService _productTranslationService;
        private readonly ILanguageService _languageService;

        public ReportController(
            IProductSaleService productSaleService,
            IProductSaleDetailService productSaleDetailService,
            IProductTranslationService productTranslationService,
            ILanguageService languageService)
        {
            _productSaleService = productSaleService;
            _productSaleDetailService = productSaleDetailService;
            _productTranslationService = productTranslationService;
            _languageService = languageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductSaleReport()
        {
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var languageResult = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = languageResult.Success ? languageResult.Data?.Id : null;

            var result = _productSaleDetailService.GetListWithInclude(x => !x.IsDeleted, x => x.Product);
            var productIds = result.Data.Select(x => x.ProductId).Distinct().ToList();
            var translationList = _productTranslationService
                .GetList(x => productIds.Contains(x.ProductId) && x.LanguageId == languageId).Data;

            // Günlük veriler
            var groupedDaily = result.Data
                .GroupBy(x => new
                {
                    Date = x.Created_Date.ToString("yyyy-MM-dd"),
                    ProductId = x.ProductId
                })
                .Select(g => new ProductSaleStackedReportDto
                {
                    Date = g.Key.Date,
                    ProductName = translationList.FirstOrDefault(t => t.ProductId == g.Key.ProductId)?.Name ??
                                  result.Data.FirstOrDefault(x => x.ProductId == g.Key.ProductId)?.Product?.Code,
                    Quantity = g.Sum(x => x.Quantity)
                }).ToList();

            // Aylık veriler
            var groupedMonthly = result.Data
                .GroupBy(x => new
                {
                    Date = x.Created_Date.ToString("yyyy-MM"),
                    ProductId = x.ProductId
                })
                .Select(g => new ProductSaleStackedReportDto
                {
                    Date = g.Key.Date,
                    ProductName = translationList.FirstOrDefault(t => t.ProductId == g.Key.ProductId)?.Name ??
                                  result.Data.FirstOrDefault(x => x.ProductId == g.Key.ProductId)?.Product?.Code,
                    Quantity = g.Sum(x => x.Quantity)
                }).ToList();

            ViewBag.ProductSaleChartData = groupedDaily;
            ViewBag.MonthlyProductSaleChartData = groupedMonthly;

            return View("ProductSaleReport");
        }
    }
}
