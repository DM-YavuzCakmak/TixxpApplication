using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Bank;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Bank;

public interface IBankRepository : IEntityRepository<BankEntity>
{
}
