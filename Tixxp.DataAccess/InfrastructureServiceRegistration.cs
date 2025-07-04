﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Abstract;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Concrete;
using Tixxp.Infrastructure.DataAccess.Abstract.Agency;
using Tixxp.Infrastructure.DataAccess.Abstract.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Company;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Abstract.Event;
using Tixxp.Infrastructure.DataAccess.Abstract.Guide;
using Tixxp.Infrastructure.DataAccess.Abstract.InvoiceType;
using Tixxp.Infrastructure.DataAccess.Abstract.Personnel;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Abstract.Product;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSale;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Abstract.SeasonalPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.Session;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketSubType;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Agency;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Counter;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Event;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Guide;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.InvoiceType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Museum;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Personnel;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Product;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSale;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Role;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SeasonalPrice;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Session;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketSubType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Ortak veritabanı (TixxpCommon) için context
            services.AddDbContext<CommonDbContext>(options =>
            {
                var conn = configuration.GetConnectionString("CommonConnection");
                options.UseSqlServer(conn);
            });

            // Dinamik veritabanı (CompanyIdentifier ile değişen) için context
            services.AddScoped<ISchemaProvider, SchemaProvider>();

            services.AddScoped<TixappContext>(serviceProvider =>
            {
                var schemaProvider = serviceProvider.GetRequiredService<ISchemaProvider>();
                var schema = schemaProvider.GetSchema();
                var connectionTemplate = configuration.GetConnectionString("DefaultConnection");
                var connectionString = connectionTemplate.Replace("{InitialCatalog}", schema);

                var optionsBuilder = new DbContextOptionsBuilder<TixappContext>();
                optionsBuilder.UseSqlServer(connectionString);

                return new TixappContext(optionsBuilder.Options, "dbo");
            });

            // Ortak veritabanı (TixxpCommon) kullanan repositoryler
            services.AddScoped<IPersonnelRepository, PersonnelRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IRoleGroupRepository, RoleGroupRepository>();
            services.AddScoped<IRoleGroupRoleRepository, RoleGroupRoleRepository>();
            services.AddScoped<IPersonnelRoleRepository, PersonnelRoleRepository>();
            services.AddScoped<IPersonnelRoleGroupRepository, PersonnelRoleGroupRepository>();
            services.AddScoped<ICurrencyTypeRepository, CurrencyTypeRepository>();
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IInvoiceTypeRepository, InvoiceTypeRepository>();
            services.AddScoped<IAgencyRepository, AgencyRepository>();
            services.AddScoped<IPriceCategoryRepository, PriceCategoryRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ISeasonalPriceRepository, SeasonalPriceRepository>();


            // Dinamik veritabanı kullanan repositoryler
            services.AddScoped<ICounterRepository, CounterRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductPriceRepository, ProductPriceRepository>();
            services.AddScoped<IProductSaleDetailRepository, ProductSaleDetailRepository>();
            services.AddScoped<IProductSaleRepository, ProductSaleRepository>();
            services.AddScoped<IProductSaleInvoiceInfoRepository, ProductSaleInvoiceInfoRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IGuideRepository, GuideRepository>();
            services.AddScoped<IAgencyContractRepository, AgencyContractRepository>();
            services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();
            services.AddScoped<ITicketSubTypeRepository, TicketSubTypeRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            return services;
        }
    }
}
