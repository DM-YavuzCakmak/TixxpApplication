namespace Tixxp.WebApp.Models.ProductSaleManagement
{
    public class ProductSaleListItemVm
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CounterId { get; set; }
        public string CounterName { get; set; } = "-";
        public long StatusId { get; set; }
        public string StatusName { get; set; } = "-";
        public string? FullName { get; set; }
        public string? IdentityNumber { get; set; }
    }
}
