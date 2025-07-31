using Microsoft.AspNetCore.Mvc;
using TarifarioBackend.Models;
using TarifarioBackend.Services;

namespace TarifarioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActualizacionController : ControllerBase
    {
        private readonly ActualizacionService _actualizacionService;

        public ActualizacionController(ActualizacionService actualizacionService)
        {
            _actualizacionService = actualizacionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActualizaciones()
        {
            var actualizaciones = await _actualizacionService.GetAllActualizacionesAsync();
            return Ok(actualizaciones);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActualizacionById(int id)
        {
            var actualizacion = await _actualizacionService.GetActualizacionByIdAsync(id);
            if (actualizacion == null)
            {
                return NotFound();
            }
            return Ok(actualizacion);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActualizacion([FromBody] ActualizacionRequest request)
        {
            var actualizacion = await _actualizacionService.CreateActualizacionAsync(request);
            return CreatedAtAction(nameof(GetActualizacionById), new { id = actualizacion.Id }, actualizacion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActualizacion(int id, [FromBody] ActualizacionRequest request)
        {
            var updated = await _actualizacionService.UpdateActualizacionAsync(id, request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActualizacion(int id)
        {
            var deleted = await _actualizacionService.DeleteActualizacionAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
