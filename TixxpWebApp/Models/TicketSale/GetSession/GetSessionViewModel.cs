namespace Tixxp.WebApp.Models.TicketSale.GetSession
{
    public class GetSessionViewModel
    {
        public long SessionId { get; set; }
        public DateTime? SessionDate { get; set; }
        public TimeSpan? StarTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int Capacity { get; set; }
        public bool IsAvailableOnB2C { get; set; }
        public bool IsAvaliableOnB2B { get; set; }
        public long SessionTypeId { get; set; }
        public string SessionTypeName { get; set; }
    }
}
