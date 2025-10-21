namespace Tixxp.WebApp.Models.ProductSaleManagement
{
    public class ProductSaleListResultVm
    {
        public IEnumerable<ProductSaleListItemVm> Items { get; set; } = new List<ProductSaleListItemVm>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
