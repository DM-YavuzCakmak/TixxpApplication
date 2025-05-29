using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.Agency;
using Tixxp.Entities.Bank;
using Tixxp.Entities.CurrencyType;
using Tixxp.Entities.InvoiceType;
using Tixxp.Entities.Personnel;
using Tixxp.Entities.PersonnelRole;
using Tixxp.Entities.PriceCategory;
using Tixxp.Entities.Role;

namespace Tixxp.Infrastructure.DataAccess.Context
{
    public class CommonDbContext : DbContext
    {
        public CommonDbContext(DbContextOptions<CommonDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonnelEntity>().ToTable("Personnel", "dbo");
            modelBuilder.Entity<CurrencyTypeEntity>().ToTable("CurrencyType", "dbo");
            modelBuilder.Entity<BankEntity>().ToTable("Bank", "dbo");
            modelBuilder.Entity<RoleEntity>().ToTable("Role", "dbo");
            modelBuilder.Entity<PersonnelRoleEntity>().ToTable("PersonnelRole", "dbo");
            modelBuilder.Entity<InvoiceTypeEntity>().ToTable("InvoiceType", "dbo");
            modelBuilder.Entity<AgencyEntity>().ToTable("Agency", "dbo");
            modelBuilder.Entity<PriceCategoryEntity>().ToTable("PriceCategory", "dbo");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<PersonnelEntity> Personnels { get; set; }
        public DbSet<InvoiceTypeEntity> InvoiceTypes { get; set; }
        public DbSet<CurrencyTypeEntity> CurrencyTypes { get; set; }
        public DbSet<BankEntity> Banks { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PersonnelRoleEntity> PersonnelRoles { get; set; }
        public DbSet<AgencyEntity> Agencies { get; set; }
        public DbSet<PriceCategoryEntity> PriceCategories { get; set; }
    }
}
