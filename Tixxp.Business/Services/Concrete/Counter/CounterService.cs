using Tixxp.Business.Services.Abstract.Counter;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;

namespace Tixxp.Business.Services.Concrete.Counter;

public class CounterService : BaseService<CounterEntity>, ICounterService
{
    private readonly ICounterRepository _counterRepository;


    public CounterService(ICounterRepository counterRepository)
        : base(counterRepository)
    {
        _counterRepository = counterRepository;
    }
}
