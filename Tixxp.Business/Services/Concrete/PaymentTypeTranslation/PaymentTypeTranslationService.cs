using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Business.Services.Abstract.Log;
using Tixxp.Business.Services.Abstract.PaymentType;
using Tixxp.Business.Services.Abstract.PaymentTypeTranslation;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Entities.PaymentType;
using Tixxp.Entities.PaymentTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.PaymentType;
using Tixxp.Infrastructure.DataAccess.Abstract.PaymentTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.County;

namespace Tixxp.Business.Services.Concrete.PaymentTypeTranslation;

public class PaymentTypeTranslationService : BaseService<PaymentTypeTranslationEntity>, IPaymentTypeTranslationService
{
    private readonly IPaymentTypeTranslationRepository _paymentTypeTranslationRepository;


    public PaymentTypeTranslationService(IPaymentTypeTranslationRepository paymentTypeTranslationRepository, ILogService logService,
        IHttpContextAccessor httpContextAccessor)
        : base(paymentTypeTranslationRepository, logService, httpContextAccessor)
    {
        _paymentTypeTranslationRepository = paymentTypeTranslationRepository;
    }
}