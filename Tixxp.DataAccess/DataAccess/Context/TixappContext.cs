using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Counter;
using Tixxp.Entities.Product;
using Tixxp.Entities.ProductPrice;
using Tixxp.Entities.ProductSale;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.Entities.ProductSaleInvoiceInfo;

namespace Tixxp.Infrastructure.DataAccess.Context
{
    public class TixappContext : DbContext
    {
        private readonly string _schema;

        public TixappContext(DbContextOptions<TixappContext> options, string? schema = null)
            : base(options)
        {
            _schema = string.IsNullOrWhiteSpace(schema) ? "dbo" : schema;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CounterEntity>(entity =>
            {
                entity.ToTable("Counter", _schema);
            });
            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.ToTable("Product", _schema);
            });
            modelBuilder.Entity<ProductPriceEntity>(entity =>
            {
                entity.ToTable("ProductPrice", _schema);
            });
            modelBuilder.Entity<ProductSaleEntity>(entity =>
            {
                entity.ToTable("ProductSale", _schema);
            });
            modelBuilder.Entity<ProductSaleDetailEntity>(entity =>
            {
                entity.ToTable("ProductSaleDetail", _schema);
            });
            modelBuilder.Entity<ProductSaleInvoiceInfoEntity>(entity =>
            {
                entity.ToTable("ProductSaleInvoiceInfo", _schema);
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CounterEntity> Counters { get; set; }
        public DbSet<ProductSaleEntity> ProductSales { get; set; }
        public DbSet<ProductPriceEntity> ProductPrices { get; set; }
        public DbSet<ProductSaleDetailEntity> ProductSaleDetails { get; set; }
        public DbSet<ProductSaleInvoiceInfoEntity> ProductSaleInvoiceInfos { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
    }
}
