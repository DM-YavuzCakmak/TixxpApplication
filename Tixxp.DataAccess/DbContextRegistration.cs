using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Abstract;
using Tixxp.Core.Utilities.Filters.SchemaProvider.Concrete;
using Tixxp.Infrastructure.DataAccess.Context;

namespace Tixxp.Infrastructure
{
    public static class DbContextRegistration
    {
        public static IServiceCollection AddTixxpDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ISchemaProvider, SchemaProvider>();

            services.AddScoped<TixappContext>(serviceProvider =>
            {
                var schemaProvider = serviceProvider.GetRequiredService<ISchemaProvider>();
                var schema = schemaProvider.GetSchema(); // Örnek: TixxpCommon
                var connStringTemplate = configuration.GetConnectionString("DefaultConnection");
                var fullConnectionString = connStringTemplate.Replace("{InitialCatalog}", schema);
                var optionsBuilder = new DbContextOptionsBuilder<TixappContext>();
                optionsBuilder.UseSqlServer(fullConnectionString);
                return new TixappContext(optionsBuilder.Options, "dbo");
            });

            return services;
        }
    }
}
