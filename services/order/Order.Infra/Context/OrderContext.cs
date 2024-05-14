using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Order.Domain.Entities;

namespace Order.Infra.Context
{
    public class OrderContext : DbContext
    {
        public OrderContext() { }

        public OrderContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<OrderLocation> Orders { get; set; }

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
