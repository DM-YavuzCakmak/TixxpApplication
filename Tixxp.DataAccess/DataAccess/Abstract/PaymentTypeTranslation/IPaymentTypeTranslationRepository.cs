using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.PaymentTypeTranslation;

namespace Tixxp.Infrastructure.DataAccess.Abstract.PaymentTypeTranslation;

public interface IPaymentTypeTranslationRepository : IEntityRepository<PaymentTypeTranslationEntity>
{
}
