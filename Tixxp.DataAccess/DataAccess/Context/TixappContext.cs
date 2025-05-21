using Microsoft.EntityFrameworkCore;
using Tixxp.Entities.Bank;
using Tixxp.Entities.Counter;

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

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CounterEntity> Counters { get; set; }
    }
}
