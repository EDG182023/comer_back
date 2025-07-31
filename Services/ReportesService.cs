using Microsoft.EntityFrameworkCore;
using TarifarioBackend.Data;
using TarifarioBackend.Models;

namespace TarifarioBackend.Services
{
    public class ReportesService
    {
        private readonly GestionTarifasDbContext _context;

        public ReportesService(GestionTarifasDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarifa>> GetReporteTarifasAsync(ReporteTarifasRequest request)
        {
            var query = _context.Tarifas
                .Include(t => t.TipoTarifa)
                .Include(t => t.Categoria)
                .Where(t => t.Activo);

            if (request.FechaInicio.HasValue)
            {
                query = query.Where(t => t.FechaCreacion >= request.FechaInicio.Value);
            }
            if (request.FechaFin.HasValue)
            {
                query = query.Where(t => t.FechaCreacion <= request.FechaFin.Value);
            }
            if (request.TipoTarifaId.HasValue)
            {
                query = query.Where(t => t.TipoTarifaId == request.TipoTarifaId.Value);
            }
            if (request.CategoriaId.HasValue)
            {
                query = query.Where(t => t.CategoriaId == request.CategoriaId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Novedad>> GetReporteNovedadesAsync(ReporteNovedadesRequest request)
        {
            var query = _context.Novedades
                .Include(n => n.TipoNovedad)
                .Where(n => n.Activo);

            if (request.FechaInicio.HasValue)
            {
                query = query.Where(n => n.FechaNovedad >= request.FechaInicio.Value);
            }
            if (request.FechaFin.HasValue)
            {
                query = query.Where(n => n.FechaNovedad <= request.FechaFin.Value);
            }
            if (request.TipoNovedadId.HasValue)
            {
                query = query.Where(n => n.TipoNovedadId == request.TipoNovedadId.Value);
            }
            if (request.Ignorada.HasValue)
            {
                query = query.Where(n => n.Ignorada == request.Ignorada.Value);
            }
            if (request.EnviadoEmail.HasValue)
            {
                query = query.Where(n => n.EnviadoEmail == request.EnviadoEmail.Value);
            }

            return await query.ToListAsync();
        }
    }
}
