using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.PaymentType;
using Tixxp.Entities.PaymentTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.PaymentType;
using Tixxp.Infrastructure.DataAccess.Abstract.PaymentTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PaymentTypeTranslation;

public class PaymentTypeTranslationRepository : EfEntityRepositoryBase<PaymentTypeTranslationEntity, CommonDbContext>, IPaymentTypeTranslationRepository
{
    public PaymentTypeTranslationRepository(CommonDbContext context) : base(context)
    {
    }
}
