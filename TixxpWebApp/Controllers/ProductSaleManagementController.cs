using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tixxp.Business.Services.Abstract.CounterTranslation;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Business.Services.Abstract.ProductSaleStatusTranslation;
using Tixxp.Core.Utilities.Enums.ProductSaleStatusEnum;
using Tixxp.Entities.CounterTranslation;
using Tixxp.Entities.ProductSale;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.Entities.ProductSaleInvoiceInfo;
using Tixxp.Entities.ProductSaleStatusTranslation;
using Tixxp.WebApp.Models.ProductSaleManagement;
using Tixxp.WebApp.Models.ReservationManagement;

namespace Tixxp.WebApp.Controllers
{
    public class ProductSaleManagementController : Controller
    {
        private readonly IProductSaleService _productSaleService;
        private readonly IProductSaleDetailService _productSaleDetailService;
        private readonly IProductSaleInvoiceInfoService _invoiceInfoService;
        private readonly IProductSaleStatusTranslationService _statusTranslationService;
        private readonly ICounterTranslationService _counterTranslationService;
        private readonly ICurrencyTypeService _currencyTypeService;
        private readonly ILanguageService _languageService;

        private const long STATUS_CANCELLED = (long)ProductSaleStatusEnum.Cancelled; 

        public ProductSaleManagementController(
            IProductSaleService productSaleService,
            IProductSaleDetailService productSaleDetailService,
            IProductSaleInvoiceInfoService invoiceInfoService,
            IProductSaleStatusTranslationService statusTranslationService,
            ICounterTranslationService counterTranslationService,
            ICurrencyTypeService currencyTypeService,
            ILanguageService languageService)
        {
            _productSaleService = productSaleService;
            _productSaleDetailService = productSaleDetailService;
            _invoiceInfoService = invoiceInfoService;
            _statusTranslationService = statusTranslationService;
            _counterTranslationService = counterTranslationService;
            _currencyTypeService = currencyTypeService;
            _languageService = languageService;
        }

        // === INDEX ===
        [HttpGet]
        public IActionResult Index()
        {
            var filterLookups = BuildFilterLookups();
            ViewBag.Counters = filterLookups.Counters;
            ViewBag.Statuses = filterLookups.Statuses;
            ViewBag.CurrencyTypes = filterLookups.CurrencyTypes;
            return View();
        }

        // === SEARCH ===
        [HttpGet]
        public IActionResult Search([FromQuery] ProductSaleListFilter filter)
        {
            NormalizeFilter(ref filter);
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langRes.Success ? langRes.Data?.Id : null;

            // Ana liste
            var baseRes = _productSaleService.GetListWithInclude(
                x => !x.IsDeleted
                     && (!filter.CounterId.HasValue || x.CounterId == filter.CounterId.Value)
                     && (!filter.StatusId.HasValue || x.StatusId == filter.StatusId.Value)
                     && (!filter.StartDate.HasValue || x.Created_Date >= filter.StartDate.Value)
                     && (!filter.EndDateExclusive.HasValue || x.Created_Date < filter.EndDateExclusive.Value),
                x => x.Counter,
                x => x.ProductSaleStatus
            );

            var list = baseRes.Success && baseRes.Data != null
                ? baseRes.Data.ToList()
                : new List<ProductSaleEntity>();

            var saleIds = list.Select(x => x.Id).ToList();

            // Fatura bilgileri
            var invRes = _invoiceInfoService.GetList(x => saleIds.Contains(x.ProductSaleId) && !x.IsDeleted);
            var invoiceList = invRes.Success && invRes.Data != null
                ? invRes.Data.ToList()
                : new List<ProductSaleInvoiceInfoEntity>();

            // 🔍 İsim / Soyisim / TCKN filtreleme
            if (!string.IsNullOrWhiteSpace(filter.FirstName))
            {
                var q = filter.FirstName.Trim().ToLowerInvariant();
                var hitIds = invoiceList
                    .Where(i => !string.IsNullOrWhiteSpace(i.FirstName) && i.FirstName.ToLower().Contains(q))
                    .Select(i => i.ProductSaleId)
                    .Distinct()
                    .ToHashSet();
                list = list.Where(r => hitIds.Contains(r.Id)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.LastName))
            {
                var q = filter.LastName.Trim().ToLowerInvariant();
                var hitIds = invoiceList
                    .Where(i => !string.IsNullOrWhiteSpace(i.LastName) && i.LastName.ToLower().Contains(q))
                    .Select(i => i.ProductSaleId)
                    .Distinct()
                    .ToHashSet();
                list = list.Where(r => hitIds.Contains(r.Id)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.IdentityNumber))
            {
                var q = filter.IdentityNumber.Trim();
                var hitIds = invoiceList
                    .Where(i => !string.IsNullOrWhiteSpace(i.IdentityNumber) && i.IdentityNumber.Contains(q))
                    .Select(i => i.ProductSaleId)
                    .Distinct()
                    .ToHashSet();
                list = list.Where(r => hitIds.Contains(r.Id)).ToList();
            }

            // Çeviriler
            var statusMap = GetStatusNames(languageId);
            var counterMap = GetCounterNames(languageId);

            var rows = list
                .OrderByDescending(r => r.Created_Date)
                .Select(r =>
                {
                    var inv = invoiceList.FirstOrDefault(i => i.ProductSaleId == r.Id);
                    return new ProductSaleListItemVm
                    {
                        Id = r.Id,
                        CreatedDate = r.Created_Date,
                        CounterId = r.CounterId,
                        CounterName = counterMap.TryGetValue(r.CounterId, out var cn) ? cn : $"#{r.CounterId}",
                        StatusId = r.StatusId,
                        StatusName = statusMap.TryGetValue(r.StatusId, out var sn) ? sn : $"#{r.StatusId}",
                        FullName = FormatFullName(inv?.FirstName, inv?.LastName),
                        IdentityNumber = inv?.IdentityNumber
                    };
                }).ToList();

            var total = rows.Count;
            var page = Math.Max(1, filter.Page);
            var size = Math.Clamp(filter.PageSize, 1, 200);
            var items = rows.Skip((page - 1) * size).Take(size).ToList();

            var vm = new ProductSaleListResultVm
            {
                Items = items,
                Page = page,
                PageSize = size,
                TotalCount = total
            };

            return PartialView("_ProductSaleList", vm);
        }

        // === DETAIL ===
        [HttpGet]
        public IActionResult Detail(long id)
        {
            var cultureCode = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == cultureCode);
            long? languageId = langRes.Success ? langRes.Data?.Id : null;

            var saleDr = _productSaleService.GetFirstOrDefaultWithInclude(
                x => x.Id == id,
                x => x.Counter,
                x => x.ProductSaleStatus
            );

            if (!saleDr.Success || saleDr.Data == null)
                return NotFound("Product sale not found.");

            var sale = saleDr.Data;

            var invDr = _invoiceInfoService.GetFirstOrDefault(x => x.ProductSaleId == id);
            var inv = invDr.Success ? invDr.Data : null;

            var detailsDr = _productSaleDetailService.GetListWithInclude(
                x => x.ProductSaleId == id && !x.IsDeleted,
                x => x.Product,
                x => x.CurrencyType
            );

            var details = detailsDr.Success && detailsDr.Data != null
                ? detailsDr.Data.Select(d => new ProductSaleDetailVm
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product?.Code ?? $"#{d.ProductId}",
                    Quantity = d.Quantity,
                    CurrencySymbol = d.CurrencyType?.Symbol ?? ""
                }).ToList()
                : new List<ProductSaleDetailVm>();

            var counterMap = GetCounterNames(languageId);
            var statusMap = GetStatusNames(languageId);

            var vm = new ProductSaleDetailMainVm
            {
                SaleId = sale.Id,
                CreatedDate = sale.Created_Date,
                CounterId = sale.CounterId,
                CounterName = counterMap.TryGetValue(sale.CounterId, out var cn) ? cn : $"#{sale.CounterId}",
                StatusId = sale.StatusId,
                StatusName = statusMap.TryGetValue(sale.StatusId, out var sn) ? sn : $"#{sale.StatusId}",
                InvoiceInfo = inv == null ? null : new ProductSaleInvoiceInfoVm
                {
                    FirstName = inv.FirstName,
                    LastName = inv.LastName,
                    IdentityNumber = inv.IdentityNumber,
                    CompanyName = inv.CompanyName,
                    TaxNumber = inv.TaxNumber,
                    TaxOffice = inv.TaxOffice
                },
                Details = details
            };

            return PartialView("_ProductSaleDetail", vm);
        }

        // === CANCEL GET ===
        [HttpGet]
        public IActionResult Cancel(long id)
        {
            var sale = _productSaleService.GetFirstOrDefault(x => x.Id == id);
            if (!sale.Success || sale.Data == null)
                return NotFound("Product sale not found.");

            var vm = new ProductSaleCancelRequest { ProductSaleId = id };
            return PartialView("_ProductSaleCancel", vm);
        }

        // === CANCEL POST ===
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel([FromForm] ProductSaleCancelRequest model)
        {
            if (model == null || model.ProductSaleId <= 0)
                return BadRequest("Invalid payload.");

            var saleDr = _productSaleService.GetFirstOrDefault(x => x.Id == model.ProductSaleId);
            if (!saleDr.Success || saleDr.Data == null)
                return NotFound("Sale not found.");

            var sale = saleDr.Data;
            if (sale.StatusId == STATUS_CANCELLED)
                return BadRequest("Sale already cancelled.");

            sale.StatusId = STATUS_CANCELLED;
            sale.Updated_Date = DateTime.UtcNow;
            sale.UpdatedBy = 6;

            var res = _productSaleService.Update(sale);
            if (!res.Success)
                return StatusCode(500, "Cancel failed.");

            // TODO: model.Reason loglanabilir
            return Ok(new { message = "Sale cancelled." });
        }

        // === CHANGE STATUS GET ===
        [HttpGet]
        public IActionResult ChangeStatus(long id)
        {
            var saleDr = _productSaleService.GetFirstOrDefault(x => x.Id == id);
            if (!saleDr.Success || saleDr.Data == null)
                return NotFound("Sale not found.");

            var filter = BuildFilterLookups();
            var vm = new ChangeStatusVm
            {
                ProductSaleId = id,
                CurrentStatusId = saleDr.Data.StatusId,
                StatusOptions = filter.Statuses
            };

            return PartialView("_ChangeStatus", vm);
        }

        // === CHANGE STATUS POST ===
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeStatus([FromForm] ChangeStatusRequest model)
        {
            if (model == null || model.ProductSaleId <= 0 || model.StatusId <= 0)
                return BadRequest("Invalid payload.");

            var saleDr = _productSaleService.GetFirstOrDefault(x => x.Id == model.ProductSaleId);
            if (!saleDr.Success || saleDr.Data == null)
                return NotFound("Sale not found.");

            var sale = saleDr.Data;
            sale.StatusId = model.StatusId;
            sale.Updated_Date = DateTime.UtcNow;
            sale.UpdatedBy = 6;

            var res = _productSaleService.Update(sale);
            if (!res.Success)
                return StatusCode(500, "Status update failed.");

            return Ok(new { message = "Status updated." });
        }

        // === HELPERS ===
        private void NormalizeFilter(ref ProductSaleListFilter f)
        {
            if (f == null) f = new ProductSaleListFilter();
            if (f.StartDate.HasValue) f.StartDate = f.StartDate.Value.Date;
            if (f.EndDate.HasValue) f.EndDateExclusive = f.EndDate.Value.Date.AddDays(1);
            f.Page = Math.Max(1, f.Page);
            f.PageSize = Math.Clamp(f.PageSize, 1, 200);
        }

        private string FormatFullName(string? name, string? surname)
        {
            var n = (name ?? "").Trim();
            var s = (surname ?? "").Trim();
            return string.IsNullOrWhiteSpace(n + s) ? "-" : $"{n} {s}".Trim();
        }

        private Dictionary<long, string> GetStatusNames(long? languageId)
        {
            var map = new Dictionary<long, string>();
            var dr = (languageId.HasValue)
                ? _statusTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value)
                : _statusTranslationService.GetList(x => !x.IsDeleted);

            if (dr.Success && dr.Data != null)
                foreach (var t in dr.Data)
                    map[t.ProductSaleStatusId] = t.Name;

            return map;
        }

        private Dictionary<long, string> GetCounterNames(long? languageId)
        {
            var map = new Dictionary<long, string>();
            var dr = (languageId.HasValue)
                ? _counterTranslationService.GetList(x => !x.IsDeleted && x.LanguageId == languageId.Value)
                : _counterTranslationService.GetList(x => !x.IsDeleted);

            if (dr.Success && dr.Data != null)
                foreach (var t in dr.Data)
                    map[t.CounterId] = t.Name;

            return map;
        }

        private FilterLookups BuildFilterLookups()
        {
            var culture = CultureInfo.CurrentUICulture.Name;
            var langRes = _languageService.GetFirstOrDefault(x => x.Code == culture);
            long? langId = langRes.Success ? langRes.Data?.Id : null;

            var statuses = GetStatusNames(langId)
                .Select(kv => new IdNameVm { Id = kv.Key, Name = kv.Value })
                .OrderBy(x => x.Name).ToList();

            var counters = GetCounterNames(langId)
                .Select(kv => new IdNameVm { Id = kv.Key, Name = kv.Value })
                .OrderBy(x => x.Name).ToList();

            var currencyList = new List<IdNameVm>();
            var curDr = _currencyTypeService.GetList(x => !x.IsDeleted);
            if (curDr.Success && curDr.Data != null)
                currencyList.AddRange(curDr.Data.Select(c => new IdNameVm { Id = c.Id, Name = c.Name }));

            return new FilterLookups
            {
                Statuses = statuses,
                Counters = counters,
                CurrencyTypes = currencyList
            };
        }
    }
}
