using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notify.Domain.Entities;

namespace Notify.Infra.Context
{
    public class NotificationContext : DbContext
    {
        public NotificationContext() { }

        public NotificationContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar um ValueConverter para converter todos os DateTime para UTC ao salvar no banco de dados
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                            v => v.ToUniversalTime(),
                            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        ));
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Host=pg_db;Username=postgres;Password=postgres;Database=catalog; PORT=5432";
            //var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=catalog; PORT=5432";
            optionsBuilder.UseNpgsql(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
