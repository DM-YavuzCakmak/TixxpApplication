using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Personnel;
using Tixxp.Entities.PersonnelRole;
using Tixxp.Entities.Role;
using Tixxp.Infrastructure.DataAccess.Concrete.EntityFramework.PersonnelRole;

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
            modelBuilder.Entity<PersonnelEntity>(entity =>
            {
                entity.ToTable("Personnel", _schema);
            });
            modelBuilder.Entity<RoleEntity>(entity =>
            {
                entity.ToTable("Role", _schema);
            });
            modelBuilder.Entity<PersonnelRoleEntity>(entity =>
            {
                entity.ToTable("PersonnelRole", _schema);
            });
            modelBuilder.Entity<BankEntity>(entity =>
            {
                entity.ToTable("Bank", _schema);
            });
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<PersonnelEntity> Personnels { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PersonnelRoleEntity> PersonnelRoles { get; set; }
        public DbSet<BankEntity> Banks { get; set; }
    }
}
