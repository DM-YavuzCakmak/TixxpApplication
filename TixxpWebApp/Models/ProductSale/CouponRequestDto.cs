namespace Tixxp.WebApp.Models.ProductSale
{
    public class CouponRequestDto
    {
        public string CouponCode { get; set; }
        public List<CartItemDto> Products { get; set; }
    }

    public class CartItemDto
    {
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CurrencyTypeId { get; set; }
    }
}
