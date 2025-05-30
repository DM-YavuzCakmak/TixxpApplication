using Tixxp.Business.Services.Abstract.Museum;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Museum;
using Tixxp.Infrastructure.DataAccess.Abstract.Museum;

namespace Tixxp.Business.Services.Concrete.Museum;

public class MuseumService : BaseService<MuseumEntity>, IMuseumService
{
    private readonly IMuseumRepository _museumRepository;


    public MuseumService(IMuseumRepository museumRepository)
        : base(museumRepository)
    {
        _museumRepository = museumRepository;
    }
}
