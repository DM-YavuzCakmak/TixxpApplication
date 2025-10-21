namespace Tixxp.WebApp.Models.ProductSaleManagement
{
    public class ProductSaleListFilter
    {
        public long? CounterId { get; set; }
        public long? StatusId { get; set; }
        public long? CurrencyTypeId { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? IdentityNumber { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateTime? EndDateExclusive { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 25;
    }
}
