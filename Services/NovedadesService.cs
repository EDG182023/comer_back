using Microsoft.EntityFrameworkCore;
using TarifarioBackend.Data;
using TarifarioBackend.Models;
using TarifarioBackend.Helpers;
using System.Data;
using System.Data.SqlClient;

namespace TarifarioBackend.Services
{
    public class NovedadesService
    {
        private readonly GestionTarifasDbContext _gestionTarifasContext;
        private readonly SaadisNuevoDbContext _saadisNuevoContext;
        private readonly EmailService _emailService;
        private readonly PdfGenerator _pdfGenerator;
        private readonly ILogger<NovedadesService> _logger;

        public NovedadesService(
            GestionTarifasDbContext gestionTarifasContext,
            SaadisNuevoDbContext saadisNuevoContext,
            EmailService emailService,
            PdfGenerator pdfGenerator,
            ILogger<NovedadesService> logger)
        {
            _gestionTarifasContext = gestionTarifasContext;
            _saadisNuevoContext = saadisNuevoContext;
            _emailService = emailService;
            _pdfGenerator = pdfGenerator;
            _logger = logger;
        }

        public async Task<IEnumerable<Novedad>> GetAllNovedadesAsync()
        {
            return await _gestionTarifasContext.Novedades
                .Include(n => n.TipoNovedad)
                .Where(n => n.Activo)
                .ToListAsync();
        }

        public async Task<Novedad?> GetNovedadByIdAsync(int id)
        {
            return await _gestionTarifasContext.Novedades
                .Include(n => n.TipoNovedad)
                .FirstOrDefaultAsync(n => n.Id == id && n.Activo);
        }

        public async Task<Novedad> CreateNovedadAsync(NovedadRequest request)
        {
            var novedad = new Novedad
            {
                IdNovedad = request.IdNovedad,
                TipoNovedadId = request.TipoNovedadId,
                FechaNovedad = request.FechaNovedad,
                Descripcion = request.Descripcion,
                EmailDestino = request.EmailDestino,
                FechaCreacion = DateTime.Now,
                Activo = true
            };

            _gestionTarifasContext.Novedades.Add(novedad);
            await _gestionTarifasContext.SaveChangesAsync();

            // Generar PDF y enviar email si se especifica
            if (!string.IsNullOrEmpty(request.EmailDestino))
            {
                await ProcessNovedadEmailAndPdf(novedad);
            }

            return novedad;
        }

        public async Task<List<Novedad>> CreateMultipleNovedadesAsync(List<NovedadRequest> requests)
        {
            var createdNovedades = new List<Novedad>();
            foreach (var request in requests)
            {
                var novedad = new Novedad
                {
                    IdNovedad = request.IdNovedad,
                    TipoNovedadId = request.TipoNovedadId,
                    FechaNovedad = request.FechaNovedad,
                    Descripcion = request.Descripcion,
                    EmailDestino = request.EmailDestino,
                    FechaCreacion = DateTime.Now,
                    Activo = true
                };
                _gestionTarifasContext.Novedades.Add(novedad);
                createdNovedades.Add(novedad);
            }

            await _gestionTarifasContext.SaveChangesAsync();

            foreach (var novedad in createdNovedades)
            {
                if (!string.IsNullOrEmpty(novedad.EmailDestino))
                {
                    await ProcessNovedadEmailAndPdf(novedad);
                }
            }

            return createdNovedades;
        }

        public async Task<bool> UpdateNovedadAsync(UpdateNovedadRequest request)
        {
            var novedad = await _gestionTarifasContext.Novedades.FindAsync(request.Id);
            if (novedad == null)
            {
                return false;
            }

            novedad.IdNovedad = request.IdNovedad;
            novedad.TipoNovedadId = request.TipoNovedadId;
            novedad.FechaNovedad = request.FechaNovedad;
            novedad.Descripcion = request.Descripcion;
            novedad.EmailDestino = request.EmailDestino;
            novedad.Ignorada = request.Ignorada;
            novedad.UsuarioIgnora = request.UsuarioIgnora;
            novedad.EnviadoEmail = request.EnviadoEmail;
            novedad.Activo = request.Activo;
            novedad.FechaModificacion = DateTime.Now;

            _gestionTarifasContext.Novedades.Update(novedad);
            await _gestionTarifasContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteNovedadAsync(int id)
        {
            var novedad = await _gestionTarifasContext.Novedades.FindAsync(id);
            if (novedad == null)
            {
                return false;
            }

            novedad.Activo = false; // Soft delete
            novedad.FechaModificacion = DateTime.Now;
            await _gestionTarifasContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IgnorarNovedadAsync(int id, string usuarioIgnora)
        {
            var novedad = await _gestionTarifasContext.Novedades.FindAsync(id);
            if (novedad == null)
            {
                return false;
            }

            novedad.Ignorada = true;
            novedad.FechaIgnorada = DateTime.Now;
            novedad.UsuarioIgnora = usuarioIgnora;
            novedad.FechaModificacion = DateTime.Now;

            _gestionTarifasContext.Novedades.Update(novedad);
            await _gestionTarifasContext.SaveChangesAsync();
            return true;
        }

        public async Task<int> IgnorarMultiplesNovedadesAsync(List<int> ids, string usuarioIgnora)
        {
            var novedades = await _gestionTarifasContext.Novedades
                .Where(n => ids.Contains(n.Id) && n.Activo && !n.Ignorada)
                .ToListAsync();

            foreach (var novedad in novedades)
            {
                novedad.Ignorada = true;
                novedad.FechaIgnorada = DateTime.Now;
                novedad.UsuarioIgnora = usuarioIgnora;
                novedad.FechaModificacion = DateTime.Now;
            }

            _gestionTarifasContext.Novedades.UpdateRange(novedades);
            return await _gestionTarifasContext.SaveChangesAsync();
        }

        public async Task<bool> ReenviarEmailNovedadAsync(int id, string newEmailDestino)
        {
            var novedad = await _gestionTarifasContext.Novedades
                .Include(n => n.TipoNovedad)
                .FirstOrDefaultAsync(n => n.Id == id && n.Activo);

            if (novedad == null)
            {
                _logger.LogWarning($"Novedad con ID {id} no encontrada para reenviar email.");
                return false;
            }

            novedad.EmailDestino = newEmailDestino; // Actualiza el email de destino
            _gestionTarifasContext.Novedades.Update(novedad);
            await _gestionTarifasContext.SaveChangesAsync();

            return await ProcessNovedadEmailAndPdf(novedad);
        }

        public async Task<int> ReenviarMultiplesEmailsNovedadAsync(List<int> ids, string newEmailDestino)
        {
            var novedades = await _gestionTarifasContext.Novedades
                .Include(n => n.TipoNovedad)
                .Where(n => ids.Contains(n.Id) && n.Activo)
                .ToListAsync();

            int sentCount = 0;
            foreach (var novedad in novedades)
            {
                novedad.EmailDestino = newEmailDestino; // Actualiza el email de destino para cada novedad
                _gestionTarifasContext.Novedades.Update(novedad);
                await _gestionTarifasContext.SaveChangesAsync(); // Guardar cambios para cada novedad antes de procesar

                if (await ProcessNovedadEmailAndPdf(novedad))
                {
                    sentCount++;
                }
            }
            return sentCount;
        }

        private async Task<bool> ProcessNovedadEmailAndPdf(Novedad novedad)
        {
            try
            {
                // 1. Generar PDF
                var pdfPath = await _pdfGenerator.GenerateNovedadPdf(novedad);
                if (string.IsNullOrEmpty(pdfPath))
                {
                    _logger.LogError($"No se pudo generar el PDF para la novedad ID: {novedad.Id}");
                    return false;
                }
                novedad.ArchivoPdfPath = pdfPath;

                // 2. Enviar Email
                string subject = $"Novedad Registrada: {novedad.TipoNovedad?.Nombre}";
                string body = $"Se ha registrado una novedad en el sistema con ID {novedad.IdNovedad}.<br/>" +
                              $"Tipo: {novedad.TipoNovedad?.Nombre}<br/>" +
                              $"Fecha: {novedad.FechaNovedad.ToShortDateString()}<br/>" +
                              $"Descripción: {novedad.Descripcion}<br/><br/>" +
                              $"Adjunto encontrará el detalle de la novedad.";

                await _emailService.SendEmailAsync(novedad.EmailDestino!, subject, body, pdfPath);

                novedad.EnviadoEmail = true;
                novedad.FechaEnvioEmail = DateTime.Now;
                novedad.FechaModificacion = DateTime.Now;

                _gestionTarifasContext.Novedades.Update(novedad);
                await _gestionTarifasContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al procesar email y PDF para novedad ID: {novedad.Id}");
                return false;
            }
        }

        // Métodos para obtener datos de Saadis_nuevo (ejemplos)
        public async Task<DataTable> GetDatosDesdeSaadisNuevo(string query)
        {
            using (IDbConnection db = _saadisNuevoContext.CreateConnection())
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandText = query;
                    using (var reader = await Task.Run(() => cmd.ExecuteReader()))
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }
    }
}
