namespace Tixxp.WebApp.Models.TicketSale.GetConfirmation;

public class GetConfirmation
{
    public PersonalInformationDto PersonalInformation { get; set; } = new PersonalInformationDto();
    public List<TicketInformationDto> TicketInformations { get; set; } = new();
    public List<ProductInformationDto> ProductInformations { get; set; } = new(); // <-- YENİ
    public PaymentInformationDto PaymentInformation { get; set; }

    // 🔹 Kullanıcının seçtiği kampanya (opsiyonel olabilir)
    public long? CampaignId { get; set; }
}


public class ProductInformationDto
{
    public long ProductId { get; set; }
    public int Piece { get; set; }
    public decimal UnitPrice { get; set; }
    public long CurrencyTypeId { get; set; }
}

