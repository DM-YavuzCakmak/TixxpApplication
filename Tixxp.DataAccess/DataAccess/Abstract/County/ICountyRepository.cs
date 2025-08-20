using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.DataAccess.EntityFramework;
using Tixxp.Entities.County;

namespace Tixxp.Infrastructure.DataAccess.Abstract.County;

public interface ICountyRepository : IEntityRepository<CountyEntity>
{
}
