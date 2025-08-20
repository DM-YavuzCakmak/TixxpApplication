using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.PaymentType;

namespace Tixxp.Infrastructure.DataAccess.Abstract.PaymentType;

public interface IPaymentTypeRepository : IEntityRepository<PaymentTypeEntity>
{
}
