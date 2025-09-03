using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Abstract;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Concrete;
using Tixxp.Infrastructure.DataAccess.Abstract.Agency;
using Tixxp.Infrastructure.DataAccess.Abstract.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Channel;
using Tixxp.Infrastructure.DataAccess.Abstract.City;
using Tixxp.Infrastructure.DataAccess.Abstract.CityTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Company;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.Country;
using Tixxp.Infrastructure.DataAccess.Abstract.CountryTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.County;
using Tixxp.Infrastructure.DataAccess.Abstract.CountyTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Currency;
using Tixxp.Infrastructure.DataAccess.Abstract.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Abstract.Event;
using Tixxp.Infrastructure.DataAccess.Abstract.EventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.Guide;
using Tixxp.Infrastructure.DataAccess.Abstract.InvoiceType;
using Tixxp.Infrastructure.DataAccess.Abstract.Language;
using Tixxp.Infrastructure.DataAccess.Abstract.PaymentType;
using Tixxp.Infrastructure.DataAccess.Abstract.PaymentTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Personnel;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Abstract.Product;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSale;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Abstract.ProductTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Reservation;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationProductDetail;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.ReservationStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Abstract.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Abstract.SeasonalPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.Session;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionEventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionType;
using Tixxp.Infrastructure.DataAccess.Abstract.SessionTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.Ticket;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketStatus;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketSubType;
using Tixxp.Infrastructure.DataAccess.Abstract.TicketType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Agency;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.AgencyContract;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Channel;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.City;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CityTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Counter;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Country;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CountryTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.County;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CountyTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Currency;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CurrencyType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Event;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.EventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Guide;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.InvoiceType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Language;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Museum;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PaymentType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PaymentTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Personnel;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRoleGroup;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PriceCategory;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Product;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductPrice;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSale;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSaleDetail;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ProductTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Reservation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationDetail;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationProductDetail;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationSaleInvoiceInfo;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationStatus;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.ReservationStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Role;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.RoleGroup;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SeasonalPrice;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Session;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionEventTicketPrice;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatus;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.SessionTypeTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Ticket;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketStatus;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketStatusTranslation;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketSubType;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.TicketType;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure;

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
        services.AddScoped<ITicketStatusTranslationRepository, TicketStatusTranslationRepository>();
        services.AddScoped<ITicketStatusRepository, TicketStatusRepository>();
        services.AddScoped<IPersonnelRepository, PersonnelRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IReservationProductDetailRepository, ReservationProductDetailRepository>();
        services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
        services.AddScoped<IReservationDetailRepository, ReservationDetailRepository>();
        services.AddScoped<IPaymentTypeTranslationRepository, PaymentTypeTranslationRepository>();
        services.AddScoped<IReservationStatusRepository, ReservationStatusRepository>();
        services.AddScoped<IReservationSaleInvoiceInfoRepository, ReservationSaleInvoiceInfoRepository>();
        services.AddScoped<IReservationStatusTranslationRepository, ReservationStatusTranslationRepository>();
        services.AddScoped<IProductTranslationRepository, ProductTranslationRepository>();
        services.AddScoped<ILanguageRepository, LanguageRepository>();
        services.AddScoped<IEventTicketPriceRepository, EventTicketPriceRepository>();
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
        services.AddScoped<ISessionEventTicketPriceRepository, SessionEventTicketPriceRepository>();

        services.AddScoped<ICounterRepository, CounterRepository>();
        services.AddScoped<IChannelRepository, ChannelRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<ISessionStatusRepository, SessionStatusRepository>();
        services.AddScoped<ISessionTypeRepository, SessionTypeRepository>();
        services.AddScoped<ISessionTypeTranslationRepository, SessionTypeTranslationRepository>();
        services.AddScoped<ISessionStatusTranslationRepository, SessionStatusTranslationRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
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
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICountyRepository, CountyRepository>();


        services.AddScoped<ICountryTranslationRepository, CountryTranslationRepository>();
        services.AddScoped<ICityTranslationRepository, CityTranslationRepository>();
        services.AddScoped<ICountyTranslationRepository, CountyTranslationRepository>();

        return services;
    }
}
