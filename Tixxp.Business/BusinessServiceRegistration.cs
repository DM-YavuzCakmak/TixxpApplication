using Microsoft.Extensions.DependencyInjection;
using Tixxp.Business.Services.Abstract.BankService;
using Tixxp.Business.Services.Abstract.Base;
using Tixxp.Business.Services.Abstract.Counter;
using Tixxp.Business.Services.Abstract.CurrencyType;
using Tixxp.Business.Services.Abstract.Event;
using Tixxp.Business.Services.Abstract.InvoiceType;
using Tixxp.Business.Services.Abstract.PersonnelRoleService;
using Tixxp.Business.Services.Abstract.PersonnelService;
using Tixxp.Business.Services.Abstract.Product;
using Tixxp.Business.Services.Abstract.ProductPrice;
using Tixxp.Business.Services.Abstract.ProductSale;
using Tixxp.Business.Services.Abstract.ProductSaleDetail;
using Tixxp.Business.Services.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Business.Services.Abstract.RoleService;
using Tixxp.Business.Services.Concrete.BankService;
using Tixxp.Business.Services.Concrete.Base;
using Tixxp.Business.Services.Concrete.Counter;
using Tixxp.Business.Services.Concrete.CurrencyType;
using Tixxp.Business.Services.Concrete.Event;
using Tixxp.Business.Services.Concrete.InvoiceType;
using Tixxp.Business.Services.Concrete.PersonnelRoleService;
using Tixxp.Business.Services.Concrete.PersonnelService;
using Tixxp.Business.Services.Concrete.Product;
using Tixxp.Business.Services.Concrete.ProductPrice;
using Tixxp.Business.Services.Concrete.ProductSale;
using Tixxp.Business.Services.Concrete.ProductSaleDetail;
using Tixxp.Business.Services.Concrete.ProductSaleInvoiceInfo;
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
            services.AddScoped<ICurrencyTypeService, CurrencyTypeService>();
            services.AddScoped<IInvoiceTypeService, InvoiceTypeService>();

            services.AddScoped<ICounterService, CounterService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductPriceService, ProductPriceService>();
            services.AddScoped<IProductSaleDetailService, ProductSaleDetailService>();
            services.AddScoped<IProductSaleService, ProductSaleService>();
            services.AddScoped<IProductSaleInvoiceInfoService, ProductSaleInvoiceInfoService>();
            services.AddScoped<IEventService, EventService>();
            return services;
        }
    }
}
