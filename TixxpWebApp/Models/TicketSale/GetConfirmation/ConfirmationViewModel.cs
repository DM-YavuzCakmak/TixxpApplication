namespace Tixxp.WebApp.Models.TicketSale.GetConfirmation
{
    public class ConfirmationViewModel
    {
        public List<CartItemViewModel> CartItems { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CampaignInfoDto> AvailableCampaigns { get; set; }
    }
}
