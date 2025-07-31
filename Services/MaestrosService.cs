using Microsoft.EntityFrameworkCore;
using TarifarioBackend.Data;
using TarifarioBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace TarifarioBackend.Services
{
    public class MaestrosService
    {
        private readonly GestionTarifasDbContext _context;

        public MaestrosService(GestionTarifasDbContext context)
        {
            _context = context;
        }

        // Items
        public async Task<Item> AddItemAsync(ItemRequest request)
        {
            var item = new Item
            {
                Nombre = request.Nombre,
                CategoriaId = request.Categoria,
                FechaCreacion = DateTime.Now
            };
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _context.Items.Include(i => i.Categoria).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> UpdateItemAsync(int id, ItemRequest request)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return false;

            item.Nombre = request.Nombre;
            item.CategoriaId = request.Categoria;
            item.FechaActualizacion = DateTime.Now;
            _context.Entry(item).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item == null) return false;
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            return await _context.Items.Include(i => i.Categoria).ToListAsync();
        }

        private bool ItemExists(int id) => _context.Items.Any(e => e.Id == id);

        // Categorias
        public async Task<Categoria> AddCategoriaAsync(CategoriaRequest request)
        {
            var categoria = new Categoria { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<Categoria?> GetCategoriaByIdAsync(int id) => await _context.Categorias.FindAsync(id);

        public async Task<bool> UpdateCategoriaAsync(int id, CategoriaRequest request)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return false;
            categoria.Nombre = request.Nombre;
            categoria.FechaActualizacion = DateTime.Now;
            _context.Entry(categoria).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteCategoriaAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return false;
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Categoria>> GetAllCategoriasAsync() => await _context.Categorias.ToListAsync();

        private bool CategoriaExists(int id) => _context.Categorias.Any(e => e.Id == id);

        // Unidades
        public async Task<Unidad> AddUnidadAsync(UnidadRequest request)
        {
            var unidad = new Unidad { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.Unidades.Add(unidad);
            await _context.SaveChangesAsync();
            return unidad;
        }

        public async Task<Unidad?> GetUnidadByIdAsync(int id) => await _context.Unidades.FindAsync(id);

        public async Task<bool> UpdateUnidadAsync(int id, UnidadRequest request)
        {
            var unidad = await _context.Unidades.FindAsync(id);
            if (unidad == null) return false;
            unidad.Nombre = request.Nombre;
            unidad.FechaActualizacion = DateTime.Now;
            _context.Entry(unidad).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnidadExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteUnidadAsync(int id)
        {
            var unidad = await _context.Unidades.FindAsync(id);
            if (unidad == null) return false;
            _context.Unidades.Remove(unidad);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Unidad>> GetAllUnidadesAsync() => await _context.Unidades.ToListAsync();

        private bool UnidadExists(int id) => _context.Unidades.Any(e => e.Id == id);

        // Clientes
        public async Task<Cliente> AddClienteAsync(ClienteRequest request)
        {
            var cliente = new Cliente
            {
                Nombre = request.Nombre,
                Email = request.Email,
                Acuerdo = request.Acuerdo,
                Observaciones = request.Observaciones,
                FechaCreacion = DateTime.Now
            };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente?> GetClienteByIdAsync(int id) => await _context.Clientes.FindAsync(id);

        public async Task<bool> UpdateClienteAsync(int id, ClienteRequest request)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;
            cliente.Nombre = request.Nombre;
            cliente.Email = request.Email;
            cliente.Acuerdo = request.Acuerdo;
            cliente.Observaciones = request.Observaciones;
            cliente.FechaActualizacion = DateTime.Now;
            _context.Entry(cliente).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync() => await _context.Clientes.ToListAsync();

        private bool ClienteExists(int id) => _context.Clientes.Any(e => e.Id == id);

        // TiposServicio
        public async Task<TipoServicio> AddTipoServicioAsync(TipoServicioRequest request)
        {
            var tipo = new TipoServicio { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.TiposServicio.Add(tipo);
            await _context.SaveChangesAsync();
            return tipo;
        }

        public async Task<TipoServicio?> GetTipoServicioByIdAsync(int id) => await _context.TiposServicio.FindAsync(id);

        public async Task<bool> UpdateTipoServicioAsync(int id, TipoServicioRequest request)
        {
            var tipo = await _context.TiposServicio.FindAsync(id);
            if (tipo == null) return false;
            tipo.Nombre = request.Nombre;
            tipo.FechaActualizacion = DateTime.Now;
            _context.Entry(tipo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoServicioExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteTipoServicioAsync(int id)
        {
            var tipo = await _context.TiposServicio.FindAsync(id);
            if (tipo == null) return false;
            _context.TiposServicio.Remove(tipo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TipoServicio>> GetAllTiposServicioAsync() => await _context.TiposServicio.ToListAsync();

        private bool TipoServicioExists(int id) => _context.TiposServicio.Any(e => e.Id == id);

        // TiposContenedor
        public async Task<TipoContenedor> AddTipoContenedorAsync(TipoContenedorRequest request)
        {
            var tipo = new TipoContenedor { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.TiposContenedor.Add(tipo);
            await _context.SaveChangesAsync();
            return tipo;
        }

        public async Task<TipoContenedor?> GetTipoContenedorByIdAsync(int id) => await _context.TiposContenedor.FindAsync(id);

        public async Task<bool> UpdateTipoContenedorAsync(int id, TipoContenedorRequest request)
        {
            var tipo = await _context.TiposContenedor.FindAsync(id);
            if (tipo == null) return false;
            tipo.Nombre = request.Nombre;
            tipo.FechaActualizacion = DateTime.Now;
            _context.Entry(tipo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoContenedorExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteTipoContenedorAsync(int id)
        {
            var tipo = await _context.TiposContenedor.FindAsync(id);
            if (tipo == null) return false;
            _context.TiposContenedor.Remove(tipo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TipoContenedor>> GetAllTiposContenedorAsync() => await _context.TiposContenedor.ToListAsync();

        private bool TipoContenedorExists(int id) => _context.TiposContenedor.Any(e => e.Id == id);

        // Puertos
        public async Task<Puerto> AddPuertoAsync(PuertoRequest request)
        {
            var puerto = new Puerto { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.Puertos.Add(puerto);
            await _context.SaveChangesAsync();
            return puerto;
        }

        public async Task<Puerto?> GetPuertoByIdAsync(int id) => await _context.Puertos.FindAsync(id);

        public async Task<bool> UpdatePuertoAsync(int id, PuertoRequest request)
        {
            var puerto = await _context.Puertos.FindAsync(id);
            if (puerto == null) return false;
            puerto.Nombre = request.Nombre;
            puerto.FechaActualizacion = DateTime.Now;
            _context.Entry(puerto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PuertoExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeletePuertoAsync(int id)
        {
            var puerto = await _context.Puertos.FindAsync(id);
            if (puerto == null) return false;
            _context.Puertos.Remove(puerto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Puerto>> GetAllPuertosAsync() => await _context.Puertos.ToListAsync();

        private bool PuertoExists(int id) => _context.Puertos.Any(e => e.Id == id);

        // Monedas
        public async Task<Moneda> AddMonedaAsync(MonedaRequest request)
        {
            var moneda = new Moneda { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.Monedas.Add(moneda);
            await _context.SaveChangesAsync();
            return moneda;
        }

        public async Task<Moneda?> GetMonedaByIdAsync(int id) => await _context.Monedas.FindAsync(id);

        public async Task<bool> UpdateMonedaAsync(int id, MonedaRequest request)
        {
            var moneda = await _context.Monedas.FindAsync(id);
            if (moneda == null) return false;
            moneda.Nombre = request.Nombre;
            moneda.FechaActualizacion = DateTime.Now;
            _context.Entry(moneda).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MonedaExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteMonedaAsync(int id)
        {
            var moneda = await _context.Monedas.FindAsync(id);
            if (moneda == null) return false;
            _context.Monedas.Remove(moneda);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Moneda>> GetAllMonedasAsync() => await _context.Monedas.ToListAsync();

        private bool MonedaExists(int id) => _context.Monedas.Any(e => e.Id == id);

        // Conceptos
        public async Task<Concepto> AddConceptoAsync(ConceptoRequest request)
        {
            var concepto = new Concepto { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.Conceptos.Add(concepto);
            await _context.SaveChangesAsync();
            return concepto;
        }

        public async Task<Concepto?> GetConceptoByIdAsync(int id) => await _context.Conceptos.FindAsync(id);

        public async Task<bool> UpdateConceptoAsync(int id, ConceptoRequest request)
        {
            var concepto = await _context.Conceptos.FindAsync(id);
            if (concepto == null) return false;
            concepto.Nombre = request.Nombre;
            concepto.FechaActualizacion = DateTime.Now;
            _context.Entry(concepto).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConceptoExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteConceptoAsync(int id)
        {
            var concepto = await _context.Conceptos.FindAsync(id);
            if (concepto == null) return false;
            _context.Conceptos.Remove(concepto);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Concepto>> GetAllConceptosAsync() => await _context.Conceptos.ToListAsync();

        private bool ConceptoExists(int id) => _context.Conceptos.Any(e => e.Id == id);

        // TiposTarifa
        public async Task<TipoTarifa> AddTipoTarifaAsync(TipoTarifaRequest request)
        {
            var tipo = new TipoTarifa { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.TiposTarifa.Add(tipo);
            await _context.SaveChangesAsync();
            return tipo;
        }

        public async Task<TipoTarifa?> GetTipoTarifaByIdAsync(int id) => await _context.TiposTarifa.FindAsync(id);

        public async Task<bool> UpdateTipoTarifaAsync(int id, TipoTarifaRequest request)
        {
            var tipo = await _context.TiposTarifa.FindAsync(id);
            if (tipo == null) return false;
            tipo.Nombre = request.Nombre;
            tipo.FechaActualizacion = DateTime.Now;
            _context.Entry(tipo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoTarifaExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteTipoTarifaAsync(int id)
        {
            var tipo = await _context.TiposTarifa.FindAsync(id);
            if (tipo == null) return false;
            _context.TiposTarifa.Remove(tipo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TipoTarifa>> GetAllTiposTarifaAsync() => await _context.TiposTarifa.ToListAsync();

        private bool TipoTarifaExists(int id) => _context.TiposTarifa.Any(e => e.Id == id);

        // TiposCosto
        public async Task<TipoCosto> AddTipoCostoAsync(TipoCostoRequest request)
        {
            var tipo = new TipoCosto { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.TiposCosto.Add(tipo);
            await _context.SaveChangesAsync();
            return tipo;
        }

        public async Task<TipoCosto?> GetTipoCostoByIdAsync(int id) => await _context.TiposCosto.FindAsync(id);

        public async Task<bool> UpdateTipoCostoAsync(int id, TipoCostoRequest request)
        {
            var tipo = await _context.TiposCosto.FindAsync(id);
            if (tipo == null) return false;
            tipo.Nombre = request.Nombre;
            tipo.FechaActualizacion = DateTime.Now;
            _context.Entry(tipo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoCostoExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteTipoCostoAsync(int id)
        {
            var tipo = await _context.TiposCosto.FindAsync(id);
            if (tipo == null) return false;
            _context.TiposCosto.Remove(tipo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TipoCosto>> GetAllTiposCostoAsync() => await _context.TiposCosto.ToListAsync();

        private bool TipoCostoExists(int id) => _context.TiposCosto.Any(e => e.Id == id);

        // TiposMovimiento
        public async Task<TipoMovimiento> AddTipoMovimientoAsync(TipoMovimientoRequest request)
        {
            var tipo = new TipoMovimiento { Nombre = request.Nombre, FechaCreacion = DateTime.Now };
            _context.TiposMovimiento.Add(tipo);
            await _context.SaveChangesAsync();
            return tipo;
        }

        public async Task<TipoMovimiento?> GetTipoMovimientoByIdAsync(int id) => await _context.TiposMovimiento.FindAsync(id);

        public async Task<bool> UpdateTipoMovimientoAsync(int id, TipoMovimientoRequest request)
        {
            var tipo = await _context.TiposMovimiento.FindAsync(id);
            if (tipo == null) return false;
            tipo.Nombre = request.Nombre;
            tipo.FechaActualizacion = DateTime.Now;
            _context.Entry(tipo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoMovimientoExists(id)) return false;
                else throw;
            }
        }

        public async Task<bool> DeleteTipoMovimientoAsync(int id)
        {
            var tipo = await _context.TiposMovimiento.FindAsync(id);
            if (tipo == null) return false;
            _context.TiposMovimiento.Remove(tipo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TipoMovimiento>> GetAllTiposMovimientoAsync() => await _context.TiposMovimiento.ToListAsync();

        private bool TipoMovimientoExists(int id) => _context.TiposMovimiento.Any(e => e.Id == id);

        // TiposDocumento
        public async Task<IEnumerable<TipoDocumento>> GetAllTiposDocumentoAsync()
        {
            return await _context.TiposDocumento.Where(td => td.Activo).ToListAsync();
        }

        public async Task<TipoDocumento?> GetTipoDocumentoByIdAsync(int id)
        {
            return await _context.TiposDocumento.FirstOrDefaultAsync(td => td.Id == id && td.Activo);
        }

        public async Task<TipoDocumento> CreateTipoDocumentoAsync(TipoDocumentoRequest request)
        {
            var tipoDocumento = new TipoDocumento
            {
                Nombre = request.Nombre,
                FechaCreacion = DateTime.Now,
                Activo = true
            };
            _context.TiposDocumento.Add(tipoDocumento);
            await _context.SaveChangesAsync();
            return tipoDocumento;
        }

        public async Task<bool> UpdateTipoDocumentoAsync(UpdateTipoDocumentoRequest request)
        {
            var tipoDocumento = await _context.TiposDocumento.FindAsync(request.Id);
            if (tipoDocumento == null)
            {
                return false;
            }

            tipoDocumento.Nombre = request.Nombre;
            tipoDocumento.Activo = request.Activo;
            tipoDocumento.FechaModificacion = DateTime.Now;

            _context.TiposDocumento.Update(tipoDocumento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTipoDocumentoAsync(int id)
        {
            var tipoDocumento = await _context.TiposDocumento.FindAsync(id);
            if (tipoDocumento == null)
            {
                return false;
            }

            tipoDocumento.Activo = false; // Soft delete
            tipoDocumento.FechaModificacion = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        // TiposNovedad
        public async Task<IEnumerable<TipoNovedad>> GetAllTiposNovedadAsync()
        {
            return await _context.TiposNovedad.Where(tn => tn.Activo).ToListAsync();
        }

        public async Task<TipoNovedad?> GetTipoNovedadByIdAsync(int id)
        {
            return await _context.TiposNovedad.FirstOrDefaultAsync(tn => tn.Id == id && tn.Activo);
        }

        public async Task<TipoNovedad> CreateTipoNovedadAsync(TipoNovedadRequest request)
        {
            var tipoNovedad = new TipoNovedad
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                FechaCreacion = DateTime.Now,
                Activo = true
            };
            _context.TiposNovedad.Add(tipoNovedad);
            await _context.SaveChangesAsync();
            return tipoNovedad;
        }

        public async Task<bool> UpdateTipoNovedadAsync(UpdateTipoNovedadRequest request)
        {
            var tipoNovedad = await _context.TiposNovedad.FindAsync(request.Id);
            if (tipoNovedad == null)
            {
                return false;
            }

            tipoNovedad.Nombre = request.Nombre;
            tipoNovedad.Descripcion = request.Descripcion;
            tipoNovedad.Activo = request.Activo;
            tipoNovedad.FechaModificacion = DateTime.Now;

            _context.TiposNovedad.Update(tipoNovedad);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTipoNovedadAsync(int id)
        {
            var tipoNovedad = await _context.TiposNovedad.FindAsync(id);
            if (tipoNovedad == null)
            {
                return false;
            }

            tipoNovedad.Activo = false; // Soft delete
            tipoNovedad.FechaModificacion = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
