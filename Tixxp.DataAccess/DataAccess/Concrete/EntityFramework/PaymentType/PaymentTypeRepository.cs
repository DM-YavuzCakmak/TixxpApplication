using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.PaymentType;
using Tixxp.Infrastructure.DataAccess.Abstract.PaymentType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PaymentType;

public class PaymentTypeRepository : EfEntityRepositoryBase<PaymentTypeEntity, CommonDbContext>, IPaymentTypeRepository
{
    public PaymentTypeRepository(CommonDbContext context) : base(context)
    {
    }
}
