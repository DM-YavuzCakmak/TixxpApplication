using Microsoft.Extensions.DependencyInjection;
using Tixxp.Business.Services.Abstract.BankService;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Business.Services.Concrete.BankService;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Business.Services.Concrete.PersonnelRoleService;
using Tixxp.Business.Services.Concrete.PersonnelService;
using Tixxp.Business.Services.Concrete.RoleService;

namespace Tixxp.Business
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));


            services.AddScoped<IPersonnelService, PersonnelService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPersonnelRoleService, PersonnelRoleService>();


            services.AddScoped<IBankService, BankService>();
            return services;
        }
    }
}
