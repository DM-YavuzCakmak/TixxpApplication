using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tixxp.Business.DataTransferObjects.Campaign
{
    public class ApplyCampaignForProductRequestDto
    {
        public List<CartItemDto> Products { get; set; } = new();
        public string CouponCode { get; set; }

        // Hesap kolaylığı için subtotal
        public decimal SubTotal => Products.Sum(p => p.Price * p.Quantity);
    }

    public class CartItemDto
    {
        public long ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public long CurrencyTypeId { get; set; }
    }
}
