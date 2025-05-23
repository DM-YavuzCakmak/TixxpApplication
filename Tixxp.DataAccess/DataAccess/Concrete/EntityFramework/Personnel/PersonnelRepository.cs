using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Personnel;
using Tixxp.Infrastructure.DataAccess.Abstract.Personnel;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Personnel;

public class PersonnelRepository : EfEntityRepositoryBase<PersonnelEntity, CommonDbContext>, IPersonnelRepository
{
    public PersonnelRepository(CommonDbContext context) : base(context)
    {
    }
}
