namespace Tixxp.WebApp.Models.ProductSaleCheckOut
{
    public class ProductSaleInvoiceInfoModel
    {
        public long ProductSaleId { get; set; }
        public long InvoiceTypeId { get; set; }
        public string? IdentityNumber { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxNumber { get; set; }
        public string? TaxOffice { get; set; }
        public long? CountyId { get; set; }
    }
}
