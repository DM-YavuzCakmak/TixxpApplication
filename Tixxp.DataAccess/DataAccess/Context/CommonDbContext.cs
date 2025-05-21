using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Personnel;
using Tixxp.Entities.PersonnelRole;
using Tixxp.Entities.Role;

namespace Tixxp.Infrastructure.DataAccess.Context
{
    public class CommonDbContext : DbContext
    {
        public CommonDbContext(DbContextOptions<CommonDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonnelEntity>().ToTable("Personnel", "dbo");
            modelBuilder.Entity<BankEntity>().ToTable("Bank", "dbo");
            modelBuilder.Entity<RoleEntity>().ToTable("Role", "dbo");
            modelBuilder.Entity<PersonnelRoleEntity>().ToTable("PersonnelRole", "dbo");

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<PersonnelEntity> Personnels { get; set; }
        public DbSet<BankEntity> Banks { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PersonnelRoleEntity> PersonnelRoles { get; set; }
    }
}
