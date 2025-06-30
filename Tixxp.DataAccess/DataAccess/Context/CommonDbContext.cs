using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.Agency;
using Tixxp.Entities.Bank;
using Tixxp.Entities.CurrencyType;
using Tixxp.Entities.InvoiceType;
using Tixxp.Entities.Museum;
using Tixxp.Entities.Personnel;
using Tixxp.Entities.PersonnelRole;
using Tixxp.Entities.PersonnelRoleGroup;
using Tixxp.Entities.Role;
using Tixxp.Entities.RoleGroup;
using Tixxp.Entities.RoleGroupRole;

namespace Tixxp.Infrastructure.DataAccess.Context
{
    public class CommonDbContext : DbContext
    {
        public CommonDbContext(DbContextOptions<CommonDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonnelEntity>().ToTable("Personnel", "dbo");
            modelBuilder.Entity<CurrencyTypeEntity>().ToTable("CurrencyType", "dbo");
            modelBuilder.Entity<RoleEntity>().ToTable("Role", "dbo");
            modelBuilder.Entity<RoleGroupEntity>().ToTable("RoleGroup", "dbo");
            modelBuilder.Entity<RoleGroupRoleEntity>().ToTable("RoleGroupRole", "dbo");
            modelBuilder.Entity<PersonnelRoleEntity>().ToTable("PersonnelRole", "dbo");
            modelBuilder.Entity<PersonnelRoleGroupEntity>().ToTable("PersonnelRoleGroup", "dbo");
            modelBuilder.Entity<InvoiceTypeEntity>().ToTable("InvoiceType", "dbo");
            modelBuilder.Entity<AgencyEntity>().ToTable("Agency", "dbo");
            modelBuilder.Entity<MuseumEntity>().ToTable("Museum", "dbo");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<PersonnelEntity> Personnels { get; set; }
        public DbSet<RoleGroupEntity> RoleGroups { get; set; }
        public DbSet<RoleGroupRoleEntity> RoleGroupRoles { get; set; }
        public DbSet<PersonnelRoleGroupEntity> PersonnelRoleGroups { get; set; }
        public DbSet<MuseumEntity> Museums { get; set; }
        public DbSet<InvoiceTypeEntity> InvoiceTypes { get; set; }
        public DbSet<CurrencyTypeEntity> CurrencyTypes { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PersonnelRoleEntity> PersonnelRoles { get; set; }
        public DbSet<AgencyEntity> Agencies { get; set; }
    }
}
