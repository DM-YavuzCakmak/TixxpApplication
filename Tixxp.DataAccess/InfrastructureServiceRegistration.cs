using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Abstract;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Concrete;
using Tixxp.Infrastructure.DataAccess.Abstract.Bank;
using Tixxp.Infrastructure.DataAccess.Abstract.Personnel;
using Tixxp.Infrastructure.DataAccess.Abstract.PersonnelRole;
using Tixxp.Infrastructure.DataAccess.Abstract.Role;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.Bank;
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

            services.AddScoped<IPersonnelRepository, PersonnelRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IPersonnelRoleRepository, PersonnelRoleRepository>();



            services.AddScoped<IBankRepository, BankRepository>();


            return services;
        }
    }
}
