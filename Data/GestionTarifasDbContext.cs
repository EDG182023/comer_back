using Microsoft.EntityFrameworkCore;
using TarifarioBackend.Models;

namespace TarifarioBackend.Data
{
    public class GestionTarifasDbContext : DbContext
    {
        public GestionTarifasDbContext(DbContextOptions<GestionTarifasDbContext> options) : base(options) { }

        public DbSet<Tarifa> Tarifas { get; set; }
        public DbSet<TipoTarifa> TiposTarifa { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<TipoDocumento> TiposDocumento { get; set; }
        public DbSet<Novedad> Novedades { get; set; }
        public DbSet<TipoNovedad> TiposNovedad { get; set; }
        public DbSet<Actualizacion> Actualizaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para Tarifa
            modelBuilder.Entity<Tarifa>()
                .HasOne(t => t.TipoTarifa)
                .WithMany()
                .HasForeignKey(t => t.TipoTarifaId)
                .OnDelete(DeleteBehavior.Restrict); // O Cascade, según tu lógica de negocio

            modelBuilder.Entity<Tarifa>()
                .HasOne(t => t.Categoria)
                .WithMany()
                .HasForeignKey(t => t.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict); // O Cascade

            // Configuración para Novedad
            modelBuilder.Entity<Novedad>()
                .HasOne(n => n.TipoNovedad)
                .WithMany()
                .HasForeignKey(n => n.TipoNovedadId)
                .OnDelete(DeleteBehavior.Restrict); // O Cascade

            // Asegurar que las fechas de creación se establezcan automáticamente
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.Name == "FechaCreacion" && property.ClrType == typeof(DateTime))
                    {
                        property.SetDefaultValueSql("GETDATE()");
                    }
                }
            }
        }
    }
}
