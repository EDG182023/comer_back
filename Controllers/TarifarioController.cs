using Microsoft.AspNetCore.Mvc;
using TarifarioBackend.Models;
using TarifarioBackend.Services;

namespace TarifarioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarifarioController : ControllerBase
    {
        private readonly TarifarioService _tarifarioService;

        public TarifarioController(TarifarioService tarifarioService)
        {
            _tarifarioService = tarifarioService;
        }

        // Tarifas
        [HttpGet("tarifas")]
        public async Task<IActionResult> GetAllTarifas()
        {
            var tarifas = await _tarifarioService.GetAllTarifasAsync();
            return Ok(tarifas);
        }

        [HttpGet("tarifas/{id}")]
        public async Task<IActionResult> GetTarifaById(int id)
        {
            var tarifa = await _tarifarioService.GetTarifaByIdAsync(id);
            if (tarifa == null)
            {
                return NotFound();
            }
            return Ok(tarifa);
        }

        [HttpPost("tarifas")]
        public async Task<IActionResult> CreateTarifa([FromBody] TarifaRequest request)
        {
            var tarifa = await _tarifarioService.CreateTarifaAsync(request);
            return CreatedAtAction(nameof(GetTarifaById), new { id = tarifa.Id }, tarifa);
        }

        [HttpPut("tarifas/{id}")]
        public async Task<IActionResult> UpdateTarifa(int id, [FromBody] UpdateTarifaRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updated = await _tarifarioService.UpdateTarifaAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("tarifas/{id}")]
        public async Task<IActionResult> DeleteTarifa(int id)
        {
            var deleted = await _tarifarioService.DeleteTarifaAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Tipos de Tarifa
        [HttpGet("tipos-tarifa")]
        public async Task<IActionResult> GetAllTiposTarifa()
        {
            var tipos = await _tarifarioService.GetAllTiposTarifaAsync();
            return Ok(tipos);
        }

        [HttpGet("tipos-tarifa/{id}")]
        public async Task<IActionResult> GetTipoTarifaById(int id)
        {
            var tipo = await _tarifarioService.GetTipoTarifaByIdAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }
            return Ok(tipo);
        }

        [HttpPost("tipos-tarifa")]
        public async Task<IActionResult> CreateTipoTarifa([FromBody] TipoTarifaRequest request)
        {
            var tipo = await _tarifarioService.CreateTipoTarifaAsync(request);
            return CreatedAtAction(nameof(GetTipoTarifaById), new { id = tipo.Id }, tipo);
        }

        [HttpPut("tipos-tarifa/{id}")]
        public async Task<IActionResult> UpdateTipoTarifa(int id, [FromBody] UpdateTipoTarifaRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updated = await _tarifarioService.UpdateTipoTarifaAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("tipos-tarifa/{id}")]
        public async Task<IActionResult> DeleteTipoTarifa(int id)
        {
            var deleted = await _tarifarioService.DeleteTipoTarifaAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Categorias
        [HttpGet("categorias")]
        public async Task<IActionResult> GetAllCategorias()
        {
            var categorias = await _tarifarioService.GetAllCategoriasAsync();
            return Ok(categorias);
        }

        [HttpGet("categorias/{id}")]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            var categoria = await _tarifarioService.GetCategoriaByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost("categorias")]
        public async Task<IActionResult> CreateCategoria([FromBody] CategoriaRequest request)
        {
            var categoria = await _tarifarioService.CreateCategoriaAsync(request);
            return CreatedAtAction(nameof(GetCategoriaById), new { id = categoria.Id }, categoria);
        }

        [HttpPut("categorias/{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] UpdateCategoriaRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updated = await _tarifarioService.UpdateCategoriaAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("categorias/{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var deleted = await _tarifarioService.DeleteCategoriaAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
