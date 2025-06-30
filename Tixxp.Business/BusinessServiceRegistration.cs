using Microsoft.Extensions.DependencyInjection;
using Tixxp.Business.Services.Abstract.Agency;
using Tixxp.Business.Services.Abstract.AgencyContract;
using Tixxp.Business.Services.Abstract.BankService;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Business.Services.Abstract.Company;
using Tixxp.Business.Services.Abstract.Counter;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.Guide;
using Tixxp.Business.Services.Abstract.InvoiceType;
using Tixxp.Business.Services.Abstract.PersonnelRoleGroup;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Abstract.PriceCategory;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Business.Services.Abstract.RoleGroup;
using Tixxp.Business.Services.Abstract.RoleGroupRole;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Business.Services.Abstract.SeasonalPrice;
using Tixxp.Business.Services.Abstract.Session;
using Tixxp.Business.Services.Abstract.TicketSubType;
using Tixxp.Business.Services.Abstract.TicketType;
using Tixxp.Business.Services.Concrete.Agency;
using Tixxp.Business.Services.Concrete.AgencyContract;
using Tixxp.Business.Services.Concrete.BankService;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Business.Services.Concrete.Company;
using Tixxp.Business.Services.Concrete.Counter;
using Tixxp.Business.Services.Concrete.CurrencyType;
using Tixxp.Business.Services.Concrete.Event;
using Tixxp.Business.Services.Concrete.Guide;
using Tixxp.Business.Services.Concrete.InvoiceType;
using Tixxp.Business.Services.Concrete.PersonnelRoleGroup;
using Tixxp.Business.Services.Concrete.PersonnelRoleService;
using Tixxp.Business.Services.Concrete.PersonnelService;
using Tixxp.Business.Services.Concrete.PriceCategory;
using Tixxp.Business.Services.Concrete.Product;
using Tixxp.Business.Services.Concrete.ProductPrice;
using Tixxp.Business.Services.Concrete.ProductSale;
using Tixxp.Business.Services.Concrete.ProductSaleDetail;
using Tixxp.Business.Services.Concrete.ProductSaleInvoiceInfo;
using Tixxp.Business.Services.Concrete.RoleGroup;
using Tixxp.Business.Services.Concrete.RoleGroupRole;
using Tixxp.Business.Services.Concrete.RoleService;
using Tixxp.Business.Services.Concrete.SeasonalPrice;
using Tixxp.Business.Services.Concrete.Session;
using Tixxp.Business.Services.Concrete.TicketSubType;
using Tixxp.Business.Services.Concrete.TicketType;

namespace Tixxp.Business
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));


            services.AddScoped<IPersonnelService, PersonnelService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IRoleGroupService, RoleGroupService>();
            services.AddScoped<IRoleGroupRoleService, RoleGroupRoleService>();
            services.AddScoped<IPersonnelRoleService, PersonnelRoleService>();
            services.AddScoped<IPersonnelRoleGroupService, PersonnelRoleGroupService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<ICurrencyTypeService, CurrencyTypeService>();
            services.AddScoped<IInvoiceTypeService, InvoiceTypeService>();
            services.AddScoped<IAgencyService, AgencyService>();
            services.AddScoped<IPriceCategoryService, PriceCategoryService>();
            services.AddScoped<IAgencyContractService, AgencyContractService>();
            services.AddScoped<ITicketTypeService, TicketTypeService>();
            services.AddScoped<ITicketSubTypeService, TicketSubTypeService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ISeasonalPriceService, SeasonalPriceService>();

            services.AddScoped<ICounterService, CounterService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductPriceService, ProductPriceService>();
            services.AddScoped<IProductSaleDetailService, ProductSaleDetailService>();
            services.AddScoped<IProductSaleService, ProductSaleService>();
            services.AddScoped<IProductSaleInvoiceInfoService, ProductSaleInvoiceInfoService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IGuideService, GuideService>();
            return services;
        }
    }
}
