using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.Agency;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Company;
using Tixxp.Entities.CurrencyType;
using Tixxp.Entities.InvoiceType;
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
            modelBuilder.Entity<PersonnelRoleEntity>().ToTable("PersonnelRole", "dbo");
            modelBuilder.Entity<RoleEntity>().ToTable("Role", "dbo");
            modelBuilder.Entity<AgencyEntity>().ToTable("Agency", "dbo");
            modelBuilder.Entity<CompanyEntity>().ToTable("Company", "dbo");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<PersonnelEntity> Personnels { get; set; }
        public DbSet<PersonnelRoleEntity> PersonnelRoles{ get; set; }
        public DbSet<CompanyEntity> Companies { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<AgencyEntity> Agencies { get; set; }
    }
}
