namespace Tixxp.WebApp.Models.ProductSaleCheckOut
{
    public class ProductSaleSummaryDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; } // Birim fiyat
        public decimal Total => Quantity * Price; // Toplam tutar
        public string ProductImageUrl { get; set; } 
    }
}
