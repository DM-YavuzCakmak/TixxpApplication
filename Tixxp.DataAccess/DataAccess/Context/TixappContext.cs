using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.AgencyContract;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Counter;
using Tixxp.Entities.CurrencyType;
using Tixxp.Entities.Events;
using Tixxp.Entities.EventTicketPrice;
using Tixxp.Entities.Guide;
using Tixxp.Entities.InvoiceType;
using Tixxp.Entities.Language;
using Tixxp.Entities.PriceCategory;
using Tixxp.Entities.Product;
using Tixxp.Entities.ProductPrice;
using Tixxp.Entities.ProductSale;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.Entities.ProductSaleInvoiceInfo;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.SeasonalPrice;
using Tixxp.Entities.Session;
using Tixxp.Entities.SessionEventTicketPrice;
using Tixxp.Entities.TicketSubType;
using Tixxp.Entities.TicketType;

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
            modelBuilder.Entity<SessionEventTicketPriceEntity>(entity =>
            {
                entity.ToTable("SessionEventTicketPrice", _schema);
            });
            modelBuilder.Entity<ReservationEntity>(entity =>
            {
                entity.ToTable("Reservation", _schema);
            });
            modelBuilder.Entity<ProductTranslationEntity>(entity =>
            {
                entity.ToTable("ProductTranslation", _schema);
            });
            modelBuilder.Entity<LanguageEntity>(entity =>
            {
                entity.ToTable("Language", _schema);
            });
            modelBuilder.Entity<EventTicketPriceEntity>(entity =>
            {
                entity.ToTable("EventTicketPrice", _schema);
            });
            modelBuilder.Entity<CurrencyTypeEntity>(entity =>
            {
                entity.ToTable("CurrencyType", _schema);
            });
            modelBuilder.Entity<InvoiceTypeEntity>(entity =>
            {
                entity.ToTable("InvoiceType", _schema);
            });
            modelBuilder.Entity<BankEntity>(entity =>
            {
                entity.ToTable("Bank", _schema);
            });
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
            modelBuilder.Entity<GuideEntity>(entity =>
            {
                entity.ToTable("Guide", _schema);
            });
            modelBuilder.Entity<EventEntity>(entity =>
            {
                entity.ToTable("Event", _schema);
            });
            modelBuilder.Entity<AgencyContractEntity>(entity =>
            {
                entity.ToTable("AgencyContract", _schema);
            });
            modelBuilder.Entity<PriceCategoryEntity>(entity =>
            {
                entity.ToTable("PriceCategory", _schema);
            });
            modelBuilder.Entity<TicketTypeEntity>(entity =>
            {
                entity.ToTable("TicketType", _schema);
            });
            modelBuilder.Entity<TicketSubTypeEntity>(entity =>
            {
                entity.ToTable("TicketSubType", _schema);
            });
            modelBuilder.Entity<SessionEntity>(entity =>
            {
                entity.ToTable("Session", _schema);
            });
            modelBuilder.Entity<SeasonalPriceEntity>(entity =>
            {
                entity.ToTable("SeasonalPrice", _schema);
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CounterEntity> Counters { get; set; }
        public DbSet<LanguageEntity> Languages { get; set; }
        public DbSet<ProductTranslationEntity> ProductTranslations { get; set; }
        public DbSet<EventTicketPriceEntity> EventTicketPrices { get; set; }
        public DbSet<CurrencyTypeEntity> CurrencyTypes { get; set; }
        public DbSet<InvoiceTypeEntity> InvoiceTypes { get; set; }
        public DbSet<BankEntity> Banks { get; set; }
        public DbSet<GuideEntity> Guides { get; set; }
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<ProductSaleEntity> ProductSales { get; set; }
        public DbSet<ProductPriceEntity> ProductPrices { get; set; }
        public DbSet<ProductSaleDetailEntity> ProductSaleDetails { get; set; }
        public DbSet<ProductSaleInvoiceInfoEntity> ProductSaleInvoiceInfos { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<PriceCategoryEntity> PriceCategories { get; set; }
        public DbSet<AgencyContractEntity> AgencyContracts { get; set; }
        public DbSet<TicketTypeEntity> TicketTypes { get; set; }
        public DbSet<TicketSubTypeEntity> TicketSubTypes { get; set; }
        public DbSet<SessionEntity> Sessions { get; set; }
        public DbSet<SeasonalPriceEntity> SeasonalPrices { get; set; }
    }
}
