using Microsoft.AspNetCore.Mvc;
using TarifarioBackend.Models;
using TarifarioBackend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TarifarioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaestrosController : ControllerBase
    {
        private readonly MaestrosService _maestrosService;

        public MaestrosController(MaestrosService maestrosService)
        {
            _maestrosService = maestrosService;
        }

        // Items
        [HttpPost("items")]
        public async Task<ActionResult<Item>> AddItem([FromBody] ItemRequest request)
        {
            var item = await _maestrosService.AddItemAsync(request);
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<Item>> GetItemById(int id)
        {
            var item = await _maestrosService.GetItemByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPut("items/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] ItemRequest request)
        {
            var result = await _maestrosService.UpdateItemAsync(id, request);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var result = await _maestrosService.DeleteItemAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<Item>>> GetAllItems()
        {
            var items = await _maestrosService.GetAllItemsAsync();
            return Ok(items);
        }

        // Categorias
        [HttpPost("categorias")]
        public async Task<ActionResult<Categoria>> AddCategoria([FromBody] CategoriaRequest request)
        {
            var categoria = await _maestrosService.AddCategoriaAsync(request);
            return CreatedAtAction(nameof(GetCategoriaById), new { id = categoria.Id }, categoria);
        }

        [HttpGet("categorias/{id}")]
        public async Task<ActionResult<Categoria>> GetCategoriaById(int id)
        {
            var categoria = await _maestrosService.GetCategoriaByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPut("categorias/{id}")]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] CategoriaRequest request)
        {
            var result = await _maestrosService.UpdateCategoriaAsync(id, request);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("categorias/{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var result = await _maestrosService.DeleteCategoriaAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetAllCategorias()
        {
            var categorias = await _maestrosService.GetAllCategoriasAsync();
            return Ok(categorias);
        }

        // Unidades
        [HttpPost("unidades")]
        public async Task<ActionResult<Unidad>> AddUnidad([FromBody] UnidadRequest request)
        {
            var unidad = await _maestrosService.AddUnidadAsync(request);
            return CreatedAtAction(nameof(GetUnidadById), new { id = unidad.Id }, unidad);
        }

        [HttpGet("unidades/{id}")]
        public async Task<ActionResult<Unidad>> GetUnidadById(int id)
        {
            var unidad = await _maestrosService.GetUnidadByIdAsync(id);
            if (unidad == null)
            {
                return NotFound();
            }
            return Ok(unidad);
        }

        [HttpPut("unidades/{id}")]
        public async Task<IActionResult> UpdateUnidad(int id, [FromBody] UnidadRequest request)
        {
            var result = await _maestrosService.UpdateUnidadAsync(id, request);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("unidades/{id}")]
        public async Task<IActionResult> DeleteUnidad(int id)
        {
            var result = await _maestrosService.DeleteUnidadAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("unidades")]
        public async Task<ActionResult<IEnumerable<Unidad>>> GetAllUnidades()
        {
            var unidades = await _maestrosService.GetAllUnidadesAsync();
            return Ok(unidades);
        }

        // Clientes
        [HttpPost("clientes")]
        public async Task<ActionResult<Cliente>> AddCliente([FromBody] ClienteRequest request)
        {
            var cliente = await _maestrosService.AddClienteAsync(request);
            return CreatedAtAction(nameof(GetClienteById), new { id = cliente.Id }, cliente);
        }

        [HttpGet("clientes/{id}")]
        public async Task<ActionResult<Cliente>> GetClienteById(int id)
        {
            var cliente = await _maestrosService.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return Ok(cliente);
        }

        [HttpPut("clientes/{id}")]
        public async Task<IActionResult> UpdateCliente(int id, [FromBody] ClienteRequest request)
        {
            var result = await _maestrosService.UpdateClienteAsync(id, request);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("clientes/{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var result = await _maestrosService.DeleteClienteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("clientes")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllClientes()
        {
            var clientes = await _maestrosService.GetAllClientesAsync();
            return Ok(clientes);
        }

        // TiposServicio
        [HttpPost("tiposservicio")]
        public async Task<ActionResult<TipoServicio>> AddTipoServicio([FromBody] TipoServicioRequest request)
        {
            var tipo = await _maestrosService.AddTipoServicioAsync(request);
            return CreatedAtAction(nameof(GetTipoServicioById), new { id = tipo.Id }, tipo);
        }

        [HttpGet("tiposservicio/{id}")]
        public async Task<ActionResult<TipoServicio>> GetTipoServicioById(int id)
        {
            var tipo = await _maestrosService.GetTipoServicioByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPut("tiposservicio/{id}")]
        public async Task<IActionResult> UpdateTipoServicio(int id, [FromBody] TipoServicioRequest request)
        {
            var result = await _maestrosService.UpdateTipoServicioAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("tiposservicio/{id}")]
        public async Task<IActionResult> DeleteTipoServicio(int id)
        {
            var result = await _maestrosService.DeleteTipoServicioAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("tiposservicio")]
        public async Task<ActionResult<IEnumerable<TipoServicio>>> GetAllTiposServicio()
        {
            var tipos = await _maestrosService.GetAllTiposServicioAsync();
            return Ok(tipos);
        }

        // TiposContenedor
        [HttpPost("tiposcontenedor")]
        public async Task<ActionResult<TipoContenedor>> AddTipoContenedor([FromBody] TipoContenedorRequest request)
        {
            var tipo = await _maestrosService.AddTipoContenedorAsync(request);
            return CreatedAtAction(nameof(GetTipoContenedorById), new { id = tipo.Id }, tipo);
        }

        [HttpGet("tiposcontenedor/{id}")]
        public async Task<ActionResult<TipoContenedor>> GetTipoContenedorById(int id)
        {
            var tipo = await _maestrosService.GetTipoContenedorByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPut("tiposcontenedor/{id}")]
        public async Task<IActionResult> UpdateTipoContenedor(int id, [FromBody] TipoContenedorRequest request)
        {
            var result = await _maestrosService.UpdateTipoContenedorAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("tiposcontenedor/{id}")]
        public async Task<IActionResult> DeleteTipoContenedor(int id)
        {
            var result = await _maestrosService.DeleteTipoContenedorAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("tiposcontenedor")]
        public async Task<ActionResult<IEnumerable<TipoContenedor>>> GetAllTiposContenedor()
        {
            var tipos = await _maestrosService.GetAllTiposContenedorAsync();
            return Ok(tipos);
        }

        // Puertos
        [HttpPost("puertos")]
        public async Task<ActionResult<Puerto>> AddPuerto([FromBody] PuertoRequest request)
        {
            var puerto = await _maestrosService.AddPuertoAsync(request);
            return CreatedAtAction(nameof(GetPuertoById), new { id = puerto.Id }, puerto);
        }

        [HttpGet("puertos/{id}")]
        public async Task<ActionResult<Puerto>> GetPuertoById(int id)
        {
            var puerto = await _maestrosService.GetPuertoByIdAsync(id);
            if (puerto == null) return NotFound();
            return Ok(puerto);
        }

        [HttpPut("puertos/{id}")]
        public async Task<IActionResult> UpdatePuerto(int id, [FromBody] PuertoRequest request)
        {
            var result = await _maestrosService.UpdatePuertoAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("puertos/{id}")]
        public async Task<IActionResult> DeletePuerto(int id)
        {
            var result = await _maestrosService.DeletePuertoAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("puertos")]
        public async Task<ActionResult<IEnumerable<Puerto>>> GetAllPuertos()
        {
            var puertos = await _maestrosService.GetAllPuertosAsync();
            return Ok(puertos);
        }

        // Monedas
        [HttpPost("monedas")]
        public async Task<ActionResult<Moneda>> AddMoneda([FromBody] MonedaRequest request)
        {
            var moneda = await _maestrosService.AddMonedaAsync(request);
            return CreatedAtAction(nameof(GetMonedaById), new { id = moneda.Id }, moneda);
        }

        [HttpGet("monedas/{id}")]
        public async Task<ActionResult<Moneda>> GetMonedaById(int id)
        {
            var moneda = await _maestrosService.GetMonedaByIdAsync(id);
            if (moneda == null) return NotFound();
            return Ok(moneda);
        }

        [HttpPut("monedas/{id}")]
        public async Task<IActionResult> UpdateMoneda(int id, [FromBody] MonedaRequest request)
        {
            var result = await _maestrosService.UpdateMonedaAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("monedas/{id}")]
        public async Task<IActionResult> DeleteMoneda(int id)
        {
            var result = await _maestrosService.DeleteMonedaAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("monedas")]
        public async Task<ActionResult<IEnumerable<Moneda>>> GetAllMonedas()
        {
            var monedas = await _maestrosService.GetAllMonedasAsync();
            return Ok(monedas);
        }

        // Conceptos
        [HttpPost("conceptos")]
        public async Task<ActionResult<Concepto>> AddConcepto([FromBody] ConceptoRequest request)
        {
            var concepto = await _maestrosService.AddConceptoAsync(request);
            return CreatedAtAction(nameof(GetConceptoById), new { id = concepto.Id }, concepto);
        }

        [HttpGet("conceptos/{id}")]
        public async Task<ActionResult<Concepto>> GetConceptoById(int id)
        {
            var concepto = await _maestrosService.GetConceptoByIdAsync(id);
            if (concepto == null) return NotFound();
            return Ok(concepto);
        }

        [HttpPut("conceptos/{id}")]
        public async Task<IActionResult> UpdateConcepto(int id, [FromBody] ConceptoRequest request)
        {
            var result = await _maestrosService.UpdateConceptoAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("conceptos/{id}")]
        public async Task<IActionResult> DeleteConcepto(int id)
        {
            var result = await _maestrosService.DeleteConceptoAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("conceptos")]
        public async Task<ActionResult<IEnumerable<Concepto>>> GetAllConceptos()
        {
            var conceptos = await _maestrosService.GetAllConceptosAsync();
            return Ok(conceptos);
        }

        // TiposTarifa
        [HttpPost("tipostarifa")]
        public async Task<ActionResult<TipoTarifa>> AddTipoTarifa([FromBody] TipoTarifaRequest request)
        {
            var tipo = await _maestrosService.AddTipoTarifaAsync(request);
            return CreatedAtAction(nameof(GetTipoTarifaById), new { id = tipo.Id }, tipo);
        }

        [HttpGet("tipostarifa/{id}")]
        public async Task<ActionResult<TipoTarifa>> GetTipoTarifaById(int id)
        {
            var tipo = await _maestrosService.GetTipoTarifaByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPut("tipostarifa/{id}")]
        public async Task<IActionResult> UpdateTipoTarifa(int id, [FromBody] TipoTarifaRequest request)
        {
            var result = await _maestrosService.UpdateTipoTarifaAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("tipostarifa/{id}")]
        public async Task<IActionResult> DeleteTipoTarifa(int id)
        {
            var result = await _maestrosService.DeleteTipoTarifaAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("tipostarifa")]
        public async Task<ActionResult<IEnumerable<TipoTarifa>>> GetAllTiposTarifa()
        {
            var tipos = await _maestrosService.GetAllTiposTarifaAsync();
            return Ok(tipos);
        }

        // TiposCosto
        [HttpPost("tiposcosto")]
        public async Task<ActionResult<TipoCosto>> AddTipoCosto([FromBody] TipoCostoRequest request)
        {
            var tipo = await _maestrosService.AddTipoCostoAsync(request);
            return CreatedAtAction(nameof(GetTipoCostoById), new { id = tipo.Id }, tipo);
        }

        [HttpGet("tiposcosto/{id}")]
        public async Task<ActionResult<TipoCosto>> GetTipoCostoById(int id)
        {
            var tipo = await _maestrosService.GetTipoCostoByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPut("tiposcosto/{id}")]
        public async Task<IActionResult> UpdateTipoCosto(int id, [FromBody] TipoCostoRequest request)
        {
            var result = await _maestrosService.UpdateTipoCostoAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("tiposcosto/{id}")]
        public async Task<IActionResult> DeleteTipoCosto(int id)
        {
            var result = await _maestrosService.DeleteTipoCostoAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("tiposcosto")]
        public async Task<ActionResult<IEnumerable<TipoCosto>>> GetAllTiposCosto()
        {
            var tipos = await _maestrosService.GetAllTiposCostoAsync();
            return Ok(tipos);
        }

        // TiposMovimiento
        [HttpPost("tiposmovimiento")]
        public async Task<ActionResult<TipoMovimiento>> AddTipoMovimiento([FromBody] TipoMovimientoRequest request)
        {
            var tipo = await _maestrosService.AddTipoMovimientoAsync(request);
            return CreatedAtAction(nameof(GetTipoMovimientoById), new { id = tipo.Id }, tipo);
        }

        [HttpGet("tiposmovimiento/{id}")]
        public async Task<ActionResult<TipoMovimiento>> GetTipoMovimientoById(int id)
        {
            var tipo = await _maestrosService.GetTipoMovimientoByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPut("tiposmovimiento/{id}")]
        public async Task<IActionResult> UpdateTipoMovimiento(int id, [FromBody] TipoMovimientoRequest request)
        {
            var result = await _maestrosService.UpdateTipoMovimientoAsync(id, request);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("tiposmovimiento/{id}")]
        public async Task<IActionResult> DeleteTipoMovimiento(int id)
        {
            var result = await _maestrosService.DeleteTipoMovimientoAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("tiposmovimiento")]
        public async Task<ActionResult<IEnumerable<TipoMovimiento>>> GetAllTiposMovimiento()
        {
            var tipos = await _maestrosService.GetAllTiposMovimientoAsync();
            return Ok(tipos);
        }

        // Tipos de Documento
        [HttpGet("tipos-documento")]
        public async Task<IActionResult> GetAllTiposDocumento()
        {
            var tipos = await _maestrosService.GetAllTiposDocumentoAsync();
            return Ok(tipos);
        }

        [HttpGet("tipos-documento/{id}")]
        public async Task<IActionResult> GetTipoDocumentoById(int id)
        {
            var tipo = await _maestrosService.GetTipoDocumentoByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPost("tipos-documento")]
        public async Task<IActionResult> CreateTipoDocumento([FromBody] TipoDocumentoRequest request)
        {
            var tipo = await _maestrosService.CreateTipoDocumentoAsync(request);
            return CreatedAtAction(nameof(GetTipoDocumentoById), new { id = tipo.Id }, tipo);
        }

        [HttpPut("tipos-documento/{id}")]
        public async Task<IActionResult> UpdateTipoDocumento(int id, [FromBody] UpdateTipoDocumentoRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updated = await _maestrosService.UpdateTipoDocumentoAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("tipos-documento/{id}")]
        public async Task<IActionResult> DeleteTipoDocumento(int id)
        {
            var deleted = await _maestrosService.DeleteTipoDocumentoAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Tipos de Novedad
        [HttpGet("tipos-novedad")]
        public async Task<IActionResult> GetAllTiposNovedad()
        {
            var tipos = await _maestrosService.GetAllTiposNovedadAsync();
            return Ok(tipos);
        }

        [HttpGet("tipos-novedad/{id}")]
        public async Task<IActionResult> GetTipoNovedadById(int id)
        {
            var tipo = await _maestrosService.GetTipoNovedadByIdAsync(id);
            if (tipo == null) return NotFound();
            return Ok(tipo);
        }

        [HttpPost("tipos-novedad")]
        public async Task<IActionResult> CreateTipoNovedad([FromBody] TipoNovedadRequest request)
        {
            var tipo = await _maestrosService.CreateTipoNovedadAsync(request);
            return CreatedAtAction(nameof(GetTipoNovedadById), new { id = tipo.Id }, tipo);
        }

        [HttpPut("tipos-novedad/{id}")]
        public async Task<IActionResult> UpdateTipoNovedad(int id, [FromBody] UpdateTipoNovedadRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID mismatch");
            }
            var updated = await _maestrosService.UpdateTipoNovedadAsync(request);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("tipos-novedad/{id}")]
        public async Task<IActionResult> DeleteTipoNovedad(int id)
        {
            var deleted = await _maestrosService.DeleteTipoNovedadAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
