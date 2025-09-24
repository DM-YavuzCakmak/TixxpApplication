namespace Tixxp.WebApp.Models.TicketSale.GetConfirmation
{
    /// <summary>
    /// Sepet satırı için ViewModel
    /// Hem bilet hem ürün ortak kullanacak.
    /// </summary>
    public class CartItemViewModel
    {
        public string Name { get; set; }           // Ürün/Bilet adı
        public int Piece { get; set; }             // Adet
        public decimal UnitPrice { get; set; }     // Birim fiyat
        public decimal SubTotal => UnitPrice * Piece; // Hesaplanan ara toplam
        public string CurrencySymbol { get; set; } // "₺", "$", "€" gibi
        public long CurrencyTypeId { get; set; }   // Para birimi id (opsiyonel)
        public bool IsProduct { get; set; }        // true=ürün, false=bilet
        public long ReferenceId { get; set; }      // Bilet için EventTicketPriceId, ürün için ProductId
    }
}
