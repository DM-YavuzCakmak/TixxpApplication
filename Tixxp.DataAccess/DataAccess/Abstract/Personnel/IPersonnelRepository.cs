using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.Personnel;

namespace Tixxp.Infrastructure.DataAccess.Abstract.Personnel;

public interface IPersonnelRepository : IEntityRepository<PersonnelEntity>
{
}
