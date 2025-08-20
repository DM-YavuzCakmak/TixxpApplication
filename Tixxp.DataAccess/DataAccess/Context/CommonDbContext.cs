using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.Agency;
using Tixxp.Entities.Bank;
using Tixxp.Entities.City;
using Tixxp.Entities.CityTranslation;
using Tixxp.Entities.Company;
using Tixxp.Entities.Country;
using Tixxp.Entities.CountryTranslation;
using Tixxp.Entities.County;
using Tixxp.Entities.CountyTranslation;
using Tixxp.Entities.CurrencyType;
using Tixxp.Entities.InvoiceType;
using Tixxp.Entities.PaymentType;
using Tixxp.Entities.PaymentTypeTranslation;
using Tixxp.Entities.Personnel;
using Tixxp.Entities.PersonnelRole;
using Tixxp.Entities.PersonnelRoleGroup;
using Tixxp.Entities.Role;
using Tixxp.Entities.RoleGroup;
using Tixxp.Entities.RoleGroupRole;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.CountyTranslation;

namespace Tixxp.Infrastructure.DataAccess.Context
{
    public class CommonDbContext : DbContext
    {
        public CommonDbContext(DbContextOptions<CommonDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonnelEntity>().ToTable("Personnel", "dbo");
            modelBuilder.Entity<PaymentTypeTranslationEntity>().ToTable("PaymentTypeTranslation", "dbo");
            modelBuilder.Entity<PaymentTypeEntity>().ToTable("PaymentType", "dbo");
            modelBuilder.Entity<PersonnelRoleEntity>().ToTable("PersonnelRole", "dbo");
            modelBuilder.Entity<RoleEntity>().ToTable("Role", "dbo");
            modelBuilder.Entity<AgencyEntity>().ToTable("Agency", "dbo");
            modelBuilder.Entity<CompanyEntity>().ToTable("Company", "dbo");
            modelBuilder.Entity<CountryEntity>().ToTable("Country", "dbo");
            modelBuilder.Entity<CityEntity>().ToTable("City", "dbo");
            modelBuilder.Entity<CountyEntity>().ToTable("County", "dbo");
            modelBuilder.Entity<CountryTranslationEntity>().ToTable("CountryTranslation", "dbo");
            modelBuilder.Entity<CityTranslationEntity>().ToTable("CityTranslation", "dbo");
            modelBuilder.Entity<CountyTranslationEntity>().ToTable("CountyTranslation", "dbo");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<PersonnelEntity> Personnels { get; set; }
        public DbSet<PaymentTypeTranslationEntity> PaymentTypeTranslations { get; set; }
        public DbSet<PersonnelRoleEntity> PersonnelRoles { get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<CountryEntity> Countries { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<CountyEntity> Counties { get; set; }
        public DbSet<PaymentTypeEntity> PaymentTypes { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<AgencyEntity> Agencies { get; set; }



        public DbSet<CountryTranslationEntity> CountryTranslations { get; set; }
        public DbSet<CityTranslationEntity> CityTranslations { get; set; }
        public DbSet<CountyTranslationEntity> CountyTranslations{ get; set; }
    }
}
