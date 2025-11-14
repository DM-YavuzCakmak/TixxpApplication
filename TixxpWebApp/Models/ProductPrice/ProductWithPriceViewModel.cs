namespace Tixxp.WebApp.Models.ProductPrice
{
    public class ProductWithPriceViewModel
    {
        public long ProductId { get; set; }
        public long CurrencyTypeId { get; set; }
        public string CurrencyTypeSymbol { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public int VatRate { get; set; }
        public long Id { get; set; }
        public string ProductName { get; set; } = "";
        public string CurrencyName { get; set; } = "";
    }
}
