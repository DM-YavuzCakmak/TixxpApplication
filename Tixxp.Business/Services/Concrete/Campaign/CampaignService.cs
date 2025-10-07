using System;
using System.Linq;
using Tixxp.Business.DataTransferObjects.Campaign;
using Tixxp.Business.Services.Abstract.Campaign;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Campaign;
using Tixxp.Entities.CampaignAction;
using Tixxp.Entities.CampaignCondition;
using Tixxp.Infrastructure.DataAccess.Abstract.Campaign;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignAction;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignCondition;
using Tixxp.Infrastructure.DataAccess.Abstract.CampaignConditionType;

namespace Tixxp.Business.Services.Concrete.Campaign
{
    /// <summary>
    /// Kampanya motoru servisi.
    /// Verilen rezervasyon ve session bilgisine göre aktif kampanyaları uygular.
    /// </summary>
    public class CampaignService : BaseService<CampaignEntity>, ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly ICampaignActionRepository _campaignActionRepository;
        private readonly ICampaignConditionRepository _campaignConditionRepository;
        private readonly ICampaignConditionTypeRepository _campaignConditionTypeRepository;

        public CampaignService(
            ICampaignRepository campaignRepository,
            ICampaignActionRepository campaignActionRepository,
            ICampaignConditionRepository campaignConditionRepository,
            ICampaignConditionTypeRepository campaignConditionTypeRepository
        ) : base(campaignRepository)
        {
            _campaignRepository = campaignRepository;
            _campaignActionRepository = campaignActionRepository;
            _campaignConditionRepository = campaignConditionRepository;
            _campaignConditionTypeRepository = campaignConditionTypeRepository;
        }

        /// <summary>
        /// Aktif kampanyaları uygular ve rezervasyonun son fiyatını hesaplar.
        /// </summary>
        public decimal ApplyCampaigns(ApplyCampaignRequestDto requestDto, CampaignEntity campaign)
        {
            if (requestDto.ReservationEntity == null || requestDto.SessionEntity == null)
                return requestDto.ReservationEntity?.TotalPrice ?? 0;

            decimal finalPrice = requestDto.ReservationEntity.TotalPrice ?? 0;


            if (CheckConditions(campaign, requestDto))
            {
                finalPrice = ApplyActions(campaign, finalPrice);
            }

            return Math.Max(finalPrice, 0); // negatif fiyat engeli
        }

        /// <summary>
        /// Kampanyanın tüm koşullarını kontrol eder.
        /// </summary>
        public bool CheckConditions(CampaignEntity campaign, ApplyCampaignRequestDto dto)
        {
            var conditions = _campaignConditionRepository.GetList(x => x.CampaignId == campaign.Id);

            foreach (CampaignConditionEntity cond in conditions)
            {
                var condType = _campaignConditionTypeRepository.GetFirstOrDefault(x => x.Id == cond.ConditionTypeId);
                if (condType == null) return false;

                if (!EvaluateCondition(condType.Code, cond, dto))
                    return false;
            }

            return true;
        }

        public decimal ApplyCampaignsForProduct(ApplyCampaignForProductRequestDto requestDto, CampaignEntity campaign)
        {
            if (requestDto == null || !requestDto.Products.Any())
                return 0;

            decimal finalPrice = requestDto.SubTotal;

            if (CheckConditionsForProduct(campaign, requestDto))
            {
                finalPrice = ApplyActions(campaign, finalPrice);
            }

            return Math.Max(finalPrice, 0);
        }

        private bool CheckConditionsForProduct(CampaignEntity campaign, ApplyCampaignForProductRequestDto dto)
        {
            var conditions = _campaignConditionRepository.GetList(x => x.CampaignId == campaign.Id);

            foreach (var cond in conditions)
            {
                var condType = _campaignConditionTypeRepository.GetFirstOrDefault(x => x.Id == cond.ConditionTypeId);
                if (condType == null) return false;

                switch (condType.Code.ToUpperInvariant())
                {
                    case "COUPON_CODE":
                        if (string.IsNullOrWhiteSpace(dto.CouponCode)) return false;

                        if (cond.Operator == "=")
                            return string.Equals(dto.CouponCode, cond.Value1, StringComparison.OrdinalIgnoreCase);

                        if (cond.Operator == "IN")
                            return cond.Value1.Split(',')
                                .Any(v => v.Trim().Equals(dto.CouponCode, StringComparison.OrdinalIgnoreCase));

                        return false;

                    case "MIN_SUBTOTAL":
                        if (!decimal.TryParse(cond.Value1, out var minSubtotal)) return false;
                        return dto.SubTotal >= minSubtotal;

                    // 🔥 ileride ProductId, CategoryId, Quantity bazlı şartları da buraya ekleyebiliriz

                    default:
                        return false;
                }
            }

            return true;
        }


        /// <summary>
        /// Tek bir koşulu değerlendirir.
        /// </summary>
        private bool EvaluateCondition(string code, CampaignConditionEntity cond, ApplyCampaignRequestDto dto)
        {
            switch (code.ToUpperInvariant())
            {
                case "EARLY_BIRD_BEFORE_DAYS":
                    if (!dto.SessionEntity.SessionDate.HasValue) return false;
                    var diffDays = (dto.SessionEntity.SessionDate.Value - DateTime.Now).Days;
                    if (!int.TryParse(cond.Value1, out int requiredDays)) return false;
                    return cond.Operator switch
                    {
                        ">=" => diffDays >= requiredDays,
                        "<=" => diffDays <= requiredDays,
                        ">" => diffDays > requiredDays,
                        "<" => diffDays < requiredDays,
                        "=" => diffDays == requiredDays,
                        _ => false
                    };

                case "DAY_OF_WEEK":
                    var sessionDay = dto.SessionEntity.SessionDate?.DayOfWeek.ToString();
                    if (string.IsNullOrWhiteSpace(sessionDay)) return false;
                    return cond.Value1.Split(',').Any(v => v.Equals(sessionDay, StringComparison.OrdinalIgnoreCase));

                case "TIME_RANGE":
                    if (!TimeSpan.TryParse(cond.Value1, out var start)) return false;
                    if (!TimeSpan.TryParse(cond.Value2, out var end)) return false;
                    return dto.SessionEntity.StartTime >= start && dto.SessionEntity.StartTime <= end;

                case "TICKET_SUBTYPE":
                    // TODO: TicketSubType ile rezervasyon detayına bağlandığında doldurulacak
                    return true;

                case "AGE":
                    // TODO: Kullanıcı bilgisi bağlanınca eklenecek
                    return true;

                case "COUPON_CODE": 
                    if (string.IsNullOrWhiteSpace(dto.CouponCode)) return false;

                    if (cond.Operator == "=")
                    {
                        return string.Equals(dto.CouponCode, cond.Value1, StringComparison.OrdinalIgnoreCase);
                    }
                    else if (cond.Operator == "IN")
                    {
                        return cond.Value1.Split(',')
                            .Any(v => v.Trim().Equals(dto.CouponCode, StringComparison.OrdinalIgnoreCase));
                    }
                    return false;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Kampanyaya bağlı aksiyonları uygular.
        /// </summary>
        private decimal ApplyActions(CampaignEntity campaign, decimal price)
        {
            var actions = _campaignActionRepository.GetList(x => x.CampaignId == campaign.Id);

            foreach (CampaignActionEntity act in actions)
            {
                switch (act.ActionType.ToUpperInvariant())
                {
                    case "DISCOUNT_PERCENT":
                        price -= price * (act.Value / 100m);
                        break;

                    case "DISCOUNT_AMOUNT":
                        price -= act.Value;
                        break;

                    case "FIXED_PRICE":
                        price = act.Value;
                        break;

                    case "FREE_ITEM":
                        // TODO: Sepet/ürün mantığı eklenince genişletilecek
                        break;
                }
            }
            return price;
        }
    }
}
