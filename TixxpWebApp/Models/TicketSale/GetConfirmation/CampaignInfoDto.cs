namespace Tixxp.WebApp.Models.TicketSale.GetConfirmation
{
    public class CampaignInfoDto
    {
        public long CampaignId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? DiscountedPrice { get; set; } 
        public decimal? OriginalPrice { get; set; }
    }
}
