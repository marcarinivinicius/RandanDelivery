using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Vehicle.Domain.Entities;

namespace Vehicle.Infra.Context
{
    public class VehicleContext : DbContext
    {
        public VehicleContext() { }

        public VehicleContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Moto> Motocycles { get; set; }

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
            optionsBuilder.UseNpgsql(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
