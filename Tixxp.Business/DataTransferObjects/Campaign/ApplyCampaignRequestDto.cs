using Tixxp.Entities.Reservation;
using Tixxp.Entities.Session;

namespace Tixxp.Business.DataTransferObjects.Campaign
{
    /// <summary>
    /// Kampanya uygulamak için gerekli request DTO
    /// </summary>
    public class ApplyCampaignRequestDto
    {
        public SessionEntity? SessionEntity { get; set; }
        public ReservationEntity? ReservationEntity { get; set; }
        public string CouponCode { get; set; }
    }

}
