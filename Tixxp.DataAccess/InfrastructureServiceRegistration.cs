using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Abstract;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Concrete;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Counter;
using Tixxp.Infrastructure.DataAccess.Abstract.Personnel;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Counter;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Personnel;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Role;
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
            services.AddScoped<IPersonnelRoleRepository, PersonnelRoleRepository>();

            // Dinamik veritabanı kullanan repositoryler
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<ICounterRepository, CounterRepository>();

            return services;
        }
    }
}
