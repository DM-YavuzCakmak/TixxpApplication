using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Session;
using Tixxp.Business.Services.Abstract.SessionEventTicketPrice;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Session;
using Tixxp.Entities.SessionEventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.Session;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionEventTicketPrice;

namespace Tixxp.Business.Services.Concrete.SessionEventTicketPrice;

public class SessionEventTicketPriceService : BaseService<SessionEventTicketPriceEntity>, ISessionEventTicketPriceService
{
    private readonly ISessionEventTicketPriceRepository _sessionEventTicketPriceRepository;
    public SessionEventTicketPriceService(ISessionEventTicketPriceRepository sessionEventTicketPriceRepository)
        : base(sessionEventTicketPriceRepository)
    {
        _sessionEventTicketPriceRepository = sessionEventTicketPriceRepository;
    }
}
