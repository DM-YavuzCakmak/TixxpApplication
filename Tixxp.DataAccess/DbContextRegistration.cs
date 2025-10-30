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
            // Schema Provider
            services.AddScoped<ISchemaProvider, SchemaProvider>();

            // Dinamik veritabanı için: TixappContext
            services.AddScoped<TixappContext>(serviceProvider =>
            {
                var schemaProvider = serviceProvider.GetRequiredService<ISchemaProvider>();
                var schema = schemaProvider.GetSchema(); // Örnek: TixxpChora
                var connStringTemplate = configuration.GetConnectionString("DefaultConnection");
                var fullConnectionString = connStringTemplate.Replace("{InitialCatalog}", schema);

                var optionsBuilder = new DbContextOptionsBuilder<TixappContext>();
                optionsBuilder.UseSqlServer(fullConnectionString);

                // Tablo şeması dbo kalmalı; veritabanı adı tenant (ör. TixxpChora)
                return new TixappContext(optionsBuilder.Options, "dbo");
            });

            // Ortak veritabanı için: CommonDbContext (her zaman TixxpCommon)
            services.AddDbContext<CommonDbContext>(options =>
            {
                var commonConnection = configuration.GetConnectionString("CommonConnection");
                options.UseSqlServer(commonConnection);
            });

            return services;
        }
    }
}
