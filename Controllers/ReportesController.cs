using Microsoft.AspNetCore.Mvc;
using TarifarioBackend.Models;
using TarifarioBackend.Services;
using ClosedXML.Excel;
using System.IO;

namespace TarifarioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly ReportesService _reportesService;

        public ReportesController(ReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        [HttpGet("tarifas")]
        public async Task<IActionResult> GetReporteTarifas([FromQuery] ReporteTarifasRequest request)
        {
            var tarifas = await _reportesService.GetReporteTarifasAsync(request);
            return Ok(tarifas);
        }

        [HttpGet("novedades")]
        public async Task<IActionResult> GetReporteNovedades([FromQuery] ReporteNovedadesRequest request)
        {
            var novedades = await _reportesService.GetReporteNovedadesAsync(request);
            return Ok(novedades);
        }

        [HttpGet("tarifas/excel")]
        public async Task<IActionResult> ExportTarifasToExcel([FromQuery] ReporteTarifasRequest request)
        {
            var tarifas = await _reportesService.GetReporteTarifasAsync(request);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Tarifas");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "ID";
                worksheet.Cell(currentRow, 2).Value = "Código";
                worksheet.Cell(currentRow, 3).Value = "Descripción";
                worksheet.Cell(currentRow, 4).Value = "Valor";
                worksheet.Cell(currentRow, 5).Value = "Tipo Tarifa";
                worksheet.Cell(currentRow, 6).Value = "Categoría";
                worksheet.Cell(currentRow, 7).Value = "Activo";
                worksheet.Cell(currentRow, 8).Value = "Fecha Creación";
                worksheet.Cell(currentRow, 9).Value = "Fecha Modificación";

                // Data
                foreach (var tarifa in tarifas)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = tarifa.Id;
                    worksheet.Cell(currentRow, 2).Value = tarifa.Codigo;
                    worksheet.Cell(currentRow, 3).Value = tarifa.Descripcion;
                    worksheet.Cell(currentRow, 4).Value = tarifa.Valor;
                    worksheet.Cell(currentRow, 5).Value = tarifa.TipoTarifa?.Nombre;
                    worksheet.Cell(currentRow, 6).Value = tarifa.Categoria?.Nombre;
                    worksheet.Cell(currentRow, 7).Value = tarifa.Activo ? "Sí" : "No";
                    worksheet.Cell(currentRow, 8).Value = tarifa.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cell(currentRow, 9).Value = tarifa.FechaModificacion?.ToString("yyyy-MM-dd HH:mm:ss");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteTarifas.xlsx");
                }
            }
        }

        [HttpGet("novedades/excel")]
        public async Task<IActionResult> ExportNovedadesToExcel([FromQuery] ReporteNovedadesRequest request)
        {
            var novedades = await _reportesService.GetReporteNovedadesAsync(request);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Novedades");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "ID";
                worksheet.Cell(currentRow, 2).Value = "ID Novedad Externa";
                worksheet.Cell(currentRow, 3).Value = "Tipo Novedad";
                worksheet.Cell(currentRow, 4).Value = "Fecha Novedad";
                worksheet.Cell(currentRow, 5).Value = "Descripción";
                worksheet.Cell(currentRow, 6).Value = "Ignorada";
                worksheet.Cell(currentRow, 7).Value = "Fecha Ignorada";
                worksheet.Cell(currentRow, 8).Value = "Usuario Ignora";
                worksheet.Cell(currentRow, 9).Value = "Email Enviado";
                worksheet.Cell(currentRow, 10).Value = "Fecha Envío Email";
                worksheet.Cell(currentRow, 11).Value = "Email Destino";
                worksheet.Cell(currentRow, 12).Value = "Activo";
                worksheet.Cell(currentRow, 13).Value = "Fecha Creación";
                worksheet.Cell(currentRow, 14).Value = "Fecha Modificación";

                // Data
                foreach (var novedad in novedades)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = novedad.Id;
                    worksheet.Cell(currentRow, 2).Value = novedad.IdNovedad;
                    worksheet.Cell(currentRow, 3).Value = novedad.TipoNovedad?.Nombre;
                    worksheet.Cell(currentRow, 4).Value = novedad.FechaNovedad.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cell(currentRow, 5).Value = novedad.Descripcion;
                    worksheet.Cell(currentRow, 6).Value = novedad.Ignorada ? "Sí" : "No";
                    worksheet.Cell(currentRow, 7).Value = novedad.FechaIgnorada?.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cell(currentRow, 8).Value = novedad.UsuarioIgnora;
                    worksheet.Cell(currentRow, 9).Value = novedad.EnviadoEmail ? "Sí" : "No";
                    worksheet.Cell(currentRow, 10).Value = novedad.FechaEnvioEmail?.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cell(currentRow, 11).Value = novedad.EmailDestino;
                    worksheet.Cell(currentRow, 12).Value = novedad.Activo ? "Sí" : "No";
                    worksheet.Cell(currentRow, 13).Value = novedad.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cell(currentRow, 14).Value = novedad.FechaModificacion?.ToString("yyyy-MM-dd HH:mm:ss");
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteNovedades.xlsx");
                }
            }
        }
    }
}
