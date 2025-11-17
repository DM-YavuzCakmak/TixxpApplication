using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.AgencyContract;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Campaign;
using Tixxp.Entities.CampaignAction;
using Tixxp.Entities.CampaignCondition;
using Tixxp.Entities.CampaignConditionType;
using Tixxp.Entities.Channel;
using Tixxp.Entities.ChannelTranslation;
using Tixxp.Entities.Counter;
using Tixxp.Entities.CounterTranslation;
using Tixxp.Entities.Currency;
using Tixxp.Entities.CurrencyType;
using Tixxp.Entities.Events;
using Tixxp.Entities.EventTicketPrice;
using Tixxp.Entities.EventTranslation;
using Tixxp.Entities.Guide;
using Tixxp.Entities.InvoiceType;
using Tixxp.Entities.Language;
using Tixxp.Entities.PersonnelRole;
using Tixxp.Entities.PersonnelRoleGroup;
using Tixxp.Entities.PriceCategory;
using Tixxp.Entities.Product;
using Tixxp.Entities.ProductPrice;
using Tixxp.Entities.ProductSale;
using Tixxp.Entities.ProductSaleDetail;
using Tixxp.Entities.ProductSaleInvoiceInfo;
using Tixxp.Entities.ProductSaleStatus;
using Tixxp.Entities.ProductSaleStatusTranslation;
using Tixxp.Entities.ProductTranslation;
using Tixxp.Entities.Reservation;
using Tixxp.Entities.ReservationDetail;
using Tixxp.Entities.ReservationProductDetail;
using Tixxp.Entities.ReservationSaleInvoiceInfo;
using Tixxp.Entities.ReservationStatus;
using Tixxp.Entities.ReservationStatusTranslation;
using Tixxp.Entities.RoleGroup;
using Tixxp.Entities.RoleGroupRole;
using Tixxp.Entities.SeasonalPrice;
using Tixxp.Entities.Session;
using Tixxp.Entities.SessionEventTicketPrice;
using Tixxp.Entities.SessionStatus;
using Tixxp.Entities.SessionStatusTranslation;
using Tixxp.Entities.SessionType;
using Tixxp.Entities.SessionTypeTranslation;
using Tixxp.Entities.Ticket;
using Tixxp.Entities.TicketStatus;
using Tixxp.Entities.TicketStatusTranslation;
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
            modelBuilder.Entity<ProductSaleStatusTranslationEntity>(entity =>
            {
                entity.ToTable("ProductSaleStatusTranslation", _schema);
            });
            modelBuilder.Entity<ProductSaleStatusEntity>(entity =>
            {
                entity.ToTable("ProductSaleStatus", _schema);
            });
            modelBuilder.Entity<CounterTranslationEntity>(entity =>
            {
                entity.ToTable("CounterTranslation", _schema);
            });
            modelBuilder.Entity<ChannelTranslationEntity>(entity =>
            {
                entity.ToTable("ChannelTranslation", _schema);
            });
            modelBuilder.Entity<CampaignConditionEntity>(entity =>
            {
                entity.ToTable("CampaignCondition", _schema);
            });
            modelBuilder.Entity<CampaignActionEntity>(entity =>
            {
                entity.ToTable("CampaignAction", _schema);
            });
            modelBuilder.Entity<CampaignConditionTypeEntity>(entity =>
            {
                entity.ToTable("CampaignConditionType", _schema);
            });
            modelBuilder.Entity<CampaignEntity>(entity =>
            {
                entity.ToTable("Campaign", _schema);
            });
            modelBuilder.Entity<TicketStatusTranslationEntity>(entity =>
            {
                entity.ToTable("TicketStatusTranslation", _schema);
            });
            modelBuilder.Entity<TicketStatusEntity>(entity =>
            {
                entity.ToTable("TicketStatus", _schema);
            });
            modelBuilder.Entity<TicketEntity>(entity =>
            {
                entity.ToTable("Ticket", _schema);
            });
            modelBuilder.Entity<ReservationProductDetailEntity>(entity =>
            {
                entity.ToTable("ReservationProductDetail", _schema);
            });
            modelBuilder.Entity<ReservationDetailEntity>(entity =>
            {
                entity.ToTable("ReservationDetail", _schema);
            });
            modelBuilder.Entity<ReservationSaleInvoiceInfoEntity>(entity =>
            {
                entity.ToTable("ReservationSaleInvoiceInfo", _schema);
            });
            modelBuilder.Entity<ReservationStatusTranslationEntity>(entity =>
            {
                entity.ToTable("ReservationStatusTranslation", _schema);
            });
            modelBuilder.Entity<ReservationStatusEntity>(entity =>
            {
                entity.ToTable("ReservationStatus", _schema);
            });
            modelBuilder.Entity<CurrencyEntity>(entity =>
            {
                entity.ToTable("Currency", _schema);
            });
            modelBuilder.Entity<ChannelEntity>(entity =>
            {
                entity.ToTable("Channel", _schema);
            });
            modelBuilder.Entity<SessionTypeTranslationEntity>(entity =>
            {
                entity.ToTable("SessionTypeTranslation", _schema);
            });
            modelBuilder.Entity<SessionTypeEntity>(entity =>
            {
                entity.ToTable("SessionType", _schema);
            });
            modelBuilder.Entity<SessionStatusTranslationEntity>(entity =>
            {
                entity.ToTable("SessionStatusTranslation", _schema);
            });
            modelBuilder.Entity<SessionStatusEntity>(entity =>
            {
                entity.ToTable("SessionStatus", _schema);
            });
            modelBuilder.Entity<PersonnelRoleGroupEntity>(entity =>
            {
                entity.ToTable("PersonnelRoleGroup", _schema);
            });
            modelBuilder.Entity<RoleGroupRoleEntity>(entity =>
            {
                entity.ToTable("RoleGroupRole", _schema);
            });
            modelBuilder.Entity<RoleGroupEntity>(entity =>
            {
                entity.ToTable("RoleGroup", _schema);
            });
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
            modelBuilder.Entity<EventTranslationEntity>(entity =>
            {
                entity.ToTable("EventTranslation", _schema);
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
        public DbSet<CampaignConditionEntity> CampaignConditions { get; set; }
        public DbSet<ProductSaleStatusEntity> ProductSaleStatuses { get; set; }
        public DbSet<ProductSaleStatusTranslationEntity> ProductSaleStatusTranslations { get; set; }
        public DbSet<CounterTranslationEntity> CounterTranslations { get; set; }
        public DbSet<ChannelTranslationEntity> ChannelTranslations { get; set; }
        public DbSet<CampaignConditionTypeEntity> CampaignConditionTypes { get; set; }
        public DbSet<CampaignActionEntity> CampaignActions { get; set; }
        public DbSet<CampaignEntity> Campaigns { get; set; }
        public DbSet<TicketEntity> Tickets { get; set; }
        public DbSet<TicketStatusEntity> TicketStatuses { get; set; }
        public DbSet<TicketStatusTranslationEntity> TicketStatusTranslations { get; set; }
        public DbSet<ChannelEntity> Channels { get; set; }
        public DbSet<ReservationProductDetailEntity> ReservationProductDetails { get; set; }
        public DbSet<SessionStatusEntity> SessionStatuses { get; set; }
        public DbSet<ReservationSaleInvoiceInfoEntity> ReservationSaleInvoiceInfos { get; set; }
        public DbSet<ReservationDetailEntity> ReservationDetails { get; set; }
        public DbSet<ReservationStatusTranslationEntity> ReservationStatusTranslations { get; set; }
        public DbSet<ReservationStatusEntity> ReservationStatuses { get; set; }
        public DbSet<SessionTypeTranslationEntity> SessionTypeTranslations { get; set; }
        public DbSet<SessionStatusTranslationEntity> SessionStatusTranslations { get; set; }
        public DbSet<LanguageEntity> Languages { get; set; }
        public DbSet<ProductTranslationEntity> ProductTranslations { get; set; }
        public DbSet<RoleGroupRoleEntity> RoleGroupRoles { get; set; }
        public DbSet<RoleGroupEntity> RoleGroups { get; set; }
        public DbSet<EventTicketPriceEntity> EventTicketPrices { get; set; }
        public DbSet<CurrencyTypeEntity> CurrencyTypes { get; set; }
        public DbSet<InvoiceTypeEntity> InvoiceTypes { get; set; }
        public DbSet<BankEntity> Banks { get; set; }
        public DbSet<GuideEntity> Guides { get; set; }
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<EventTranslationEntity> EventTranslations { get; set; }
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
