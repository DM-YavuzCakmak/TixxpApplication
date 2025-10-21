using Tixxp.WebApp.Models.ReservationManagement;

namespace Tixxp.WebApp.Models.ProductSaleManagement
{
    public class ChangeStatusVm
    {
        public long ProductSaleId { get; set; }
        public long? CurrentStatusId { get; set; }
        public IEnumerable<IdNameVm> StatusOptions { get; set; } = new List<IdNameVm>();
    }
}
