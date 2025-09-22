using Tixxp.Business.DataTransferObjects.Campaign;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Entities.Campaign;
namespace Tixxp.Business.Services.Abstract.Campaign;

public interface ICampaignService : IBaseService<CampaignEntity>
{
    public decimal ApplyCampaigns(ApplyCampaignRequestDto requestDto);
}
