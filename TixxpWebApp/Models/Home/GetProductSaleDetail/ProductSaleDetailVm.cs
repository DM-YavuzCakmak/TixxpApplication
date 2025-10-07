namespace Tixxp.WebApp.Models.Home.GetProductSaleDetail
{
    public class ProductSaleDetailVm
    {
        // 🔹 Satış Temel Bilgileri
        public long SaleId { get; set; }
        public string StatusName { get; set; }
        public string CounterName { get; set; }
        public DateTime CreatedDate { get; set; }

        // 🔹 Personel Bilgileri
        public string CreatedByName { get; set; }
        public string CreatedByPhoto { get; set; }

        // 🔹 Müşteri / Fatura Bilgileri
        public string CustomerFullName { get; set; }
        public string CustomerIdentityNumber { get; set; }

        // 🔹 Kampanya Bilgileri
        public long? CampaignId { get; set; }            // Kampanya ID’si (varsa)
        public string CampaignName { get; set; }         // Kampanya adı
        public decimal? DiscountAmount { get; set; }     // İndirim tutarı
        public decimal? OriginalTotalPrice { get; set; } // Kampanyasız fiyat

        // 🔹 Ürün Listesi
        public List<ProductSaleDetailItemVm> Products { get; set; } = new();

        // 🔹 Fiyat Bilgileri
        public decimal TotalPrice { get; set; }          // Son toplam (kampanya sonrası)
        public string CurrencySymbol { get; set; }

        // 🔹 UI Durum Alanları
        public bool IsCancellable { get; set; } = true;  // İptal edilebilir mi (örneğin Completed değilse false)
        public bool IsCancelled { get; set; } = false;   // İptal edilmiş mi
    }

    public class ProductSaleDetailItemVm
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string CurrencySymbol { get; set; }
        public string? Image { get; set; }

        // 🔹 Kampanya sonrası birim fiyat (indirimli)
        public decimal? DiscountedUnitPrice { get; set; }

        // 🔹 Toplam indirimli satır tutarı
        public decimal? DiscountedLineTotal { get; set; }
    }
}
