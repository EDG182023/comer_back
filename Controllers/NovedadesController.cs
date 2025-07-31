using Microsoft.AspNetCore.Mvc;
using TarifarioBackend.Models;
using TarifarioBackend.Services;
using TarifarioBackend.Helpers;

namespace TarifarioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NovedadesController : ControllerBase
    {
        private readonly NovedadesService _novedadesService;
        private readonly PdfGenerator _pdfGenerator;

        public NovedadesController(NovedadesService novedadesService, PdfGenerator pdfGenerator)
        {
            _novedadesService = novedadesService;
            _pdfGenerator = pdfGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNovedades()
        {
            var novedades = await _novedadesService.GetAllNovedadesAsync();
            return Ok(novedades);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNovedadById(int id)
        {
            var novedad = await _novedadesService.GetNovedadByIdAsync(id);
            if (novedad == null)
            {
                return NotFound();
            }
            return Ok(novedad);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNovedad([FromBody] NovedadRequest request)
        {
            var novedad = await _novedadesService.CreateNovedadAsync(request);
            return CreatedAtAction(nameof(GetNovedadById), new { id = novedad.Id }, novedad);
        }

        [HttpPost("multiple")]
        public async Task<IActionResult> CreateMultipleNovedades([FromBody] NovedadesMultiplesRequest request)
        {
            var novedades = await _novedadesService.CreateMultipleNovedadesAsync(request.Novedades);
            return Ok(novedades);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNovedad(int id, [FromBody] UpdateNovedadRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updated = await _novedadesService.UpdateNovedadAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNovedad(int id)
        {
            var deleted = await _novedadesService.DeleteNovedadAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("ignorar")]
        public async Task<IActionResult> IgnorarNovedad([FromBody] IgnorarNovedadRequest request)
        {
            var ignored = await _novedadesService.IgnorarNovedadAsync(request.Id, request.UsuarioIgnora);
            if (!ignored)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("ignorar-multiples")]
        public async Task<IActionResult> IgnorarMultiplesNovedades([FromBody] IgnorarMultiplesRequest request)
        {
            var ignoredCount = await _novedadesService.IgnorarMultiplesNovedadesAsync(request.Ids, request.UsuarioIgnora);
            return Ok(new { IgnoredCount = ignoredCount });
        }

        [HttpPost("reimprimir-pdf")]
        public async Task<IActionResult> ReimprimirPdf([FromBody] ReimprimirPdfRequest request)
        {
            var novedad = await _novedadesService.GetNovedadByIdAsync(request.Id);
            if (novedad == null)
            {
                return NotFound("Novedad no encontrada.");
            }

            var pdfPath = await _pdfGenerator.GenerateNovedadPdf(novedad);
            if (string.IsNullOrEmpty(pdfPath))
            {
                return StatusCode(500, "Error al generar el PDF.");
            }

            return Ok(new { PdfPath = pdfPath });
        }

        [HttpPost("reimprimir-pdf-masivo")]
        public async Task<IActionResult> ReimprimirPdfMasivo([FromBody] ReimprimirPdfMasivoRequest request)
        {
            var pdfPaths = new List<string>();
            foreach (var id in request.Ids)
            {
                var novedad = await _novedadesService.GetNovedadByIdAsync(id);
                if (novedad != null)
                {
                    var pdfPath = await _pdfGenerator.GenerateNovedadPdf(novedad);
                    if (!string.IsNullOrEmpty(pdfPath))
                    {
                        pdfPaths.Add(pdfPath);
                    }
                }
            }
            if (!pdfPaths.Any())
            {
                return StatusCode(500, "No se pudieron generar PDFs para ninguna de las novedades proporcionadas.");
            }
            return Ok(new { PdfPaths = pdfPaths });
        }

        [HttpPost("reenviar-mail")]
        public async Task<IActionResult> ReenviarMail([FromBody] ReenviarMailRequest request)
        {
            var success = await _novedadesService.ReenviarEmailNovedadAsync(request.Id, request.EmailDestino);
            if (!success)
            {
                return StatusCode(500, "Error al reenviar el correo electrónico.");
            }
            return Ok();
        }

        [HttpPost("reenviar-mail-masivo")]
        public async Task<IActionResult> ReenviarMailMasivo([FromBody] ReenviarMailMasivoRequest request)
        {
            var successCount = await _novedadesService.ReenviarMultiplesEmailsNovedadAsync(request.Ids, request.EmailDestino);
            if (successCount == 0)
            {
                return StatusCode(500, "No se pudieron reenviar correos electrónicos para ninguna de las novedades proporcionadas.");
            }
            return Ok(new { SentCount = successCount });
        }
    }
}
