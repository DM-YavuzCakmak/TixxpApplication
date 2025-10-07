namespace Tixxp.WebApp.Models.ProductSaleCheckOut
{
    public class ProductSaleCheckOutItem
    {
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CurrencyTypeId { get; set; } 
    }

    public class CustomerInfo
    {
        public string FullName { get; set; } 
        public string Surname { get; set; }   
        public string Tckn { get; set; }      
    }

    // Submit sırasında gidecek model (ana DTO)
    public class ProductSaleCheckOutSubmitModel
    {
        public long? CampaignId { get; set; } // 🔥 yeni alan
        public long ProductSaleId { get; set; }  // Hangi satışa ait
        public CustomerInfo CustomerInfo { get; set; }  // Müşteri bilgileri
        public List<ProductSaleCheckOutItem> Items { get; set; } // Sepet ürünleri
    }
}
