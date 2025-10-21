namespace Tixxp.WebApp.Models.ProductSaleManagement
{
    public class ProductSaleCancelRequest
    {
        public long ProductSaleId { get; set; }
        public string? Reason { get; set; }
    }
}
