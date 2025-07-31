using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace TarifarioBackend.Data
{
    public class SaadisNuevoDbContext : DbContext
    {
        private readonly string _connectionString;

        public SaadisNuevoDbContext(DbContextOptions<SaadisNuevoDbContext> options) : base(options)
        {
            // Extraer la cadena de conexión de las opciones
            _connectionString = Database.GetConnectionString() ?? throw new ArgumentNullException(nameof(_connectionString), "Connection string for SaadisNuevoDbContext is null.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // No se necesitan DbSet para tablas que solo se acceden directamente con Dapper/ADO.NET
            // Si tuvieras entidades de EF Core para Saadis_nuevo, irían aquí.
        }
    }
}
