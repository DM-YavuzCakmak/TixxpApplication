using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Language;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.PaymentType;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.Language;
using Tixxp.Entities.PaymentType;
using Tixxp.Infrastructure.DataAccess.Abstract.Language;
using Tixxp.Infrastructure.DataAccess.Abstract.PaymentType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.County;

namespace Tixxp.Business.Services.Concrete.PaymentType;

public class PaymentTypeService : BaseService<PaymentTypeEntity>, IPaymentTypeService
{
    private readonly IPaymentTypeRepository _paymentTypeRepository;


    public PaymentTypeService(IPaymentTypeRepository paymentTypeRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(paymentTypeRepository, logService, httpContextAccessor)
    {
        _paymentTypeRepository = paymentTypeRepository;
    }
}
