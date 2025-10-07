namespace Tixxp.WebApp.Models.Campaign
{
    public class ApplyCampaignForProductRequestDto
    {
        public List<CartItemDto> Products { get; set; } = new();
        public string CouponCode { get; set; }
        public decimal SubTotal => Products.Sum(p => p.Price * p.Quantity);
    }

    public class CartItemDto
    {
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CurrencyTypeId { get; set; }
    }
}
