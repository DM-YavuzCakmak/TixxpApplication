namespace Tixxp.WebApp.Models.ProductSaleManagement
{
    public class ProductSaleDetailMainVm
    {
        public long SaleId { get; set; }
        public DateTime CreatedDate { get; set; }

        public long CounterId { get; set; }
        public string CounterName { get; set; } = "-";
        public long StatusId { get; set; }
        public string StatusName { get; set; } = "-";

        public ProductSaleInvoiceInfoVm? InvoiceInfo { get; set; }
        public List<ProductSaleDetailVm> Details { get; set; } = new();
    }

    public class ProductSaleDetailVm
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } = "-";
        public int Quantity { get; set; }
        public string CurrencySymbol { get; set; } = "";
    }

    public class ProductSaleInvoiceInfoVm
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? IdentityNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxOffice { get; set; }
    }
}
