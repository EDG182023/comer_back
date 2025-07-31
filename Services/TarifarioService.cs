using Microsoft.EntityFrameworkCore;
using TarifarioBackend.Data;
using TarifarioBackend.Models;

namespace TarifarioBackend.Services
{
    public class TarifarioService
    {
        private readonly GestionTarifasDbContext _context;

        public TarifarioService(GestionTarifasDbContext context)
        {
            _context = context;
        }

        // Tarifas
        public async Task<IEnumerable<Tarifa>> GetAllTarifasAsync()
        {
            return await _context.Tarifas
                .Include(t => t.TipoTarifa)
                .Include(t => t.Categoria)
                .Where(t => t.Activo)
                .ToListAsync();
        }

        public async Task<Tarifa?> GetTarifaByIdAsync(int id)
        {
            return await _context.Tarifas
                .Include(t => t.TipoTarifa)
                .Include(t => t.Categoria)
                .FirstOrDefaultAsync(t => t.Id == id && t.Activo);
        }

        public async Task<Tarifa> CreateTarifaAsync(TarifaRequest request)
        {
            var tarifa = new Tarifa
            {
                Codigo = request.Codigo,
                Descripcion = request.Descripcion,
                Valor = request.Valor,
                TipoTarifaId = request.TipoTarifaId,
                CategoriaId = request.CategoriaId,
                FechaCreacion = DateTime.Now,
                Activo = true
            };
            _context.Tarifas.Add(tarifa);
            await _context.SaveChangesAsync();
            return tarifa;
        }

        public async Task<bool> UpdateTarifaAsync(UpdateTarifaRequest request)
        {
            var tarifa = await _context.Tarifas.FindAsync(request.Id);
            if (tarifa == null)
            {
                return false;
            }

            tarifa.Codigo = request.Codigo;
            tarifa.Descripcion = request.Descripcion;
            tarifa.Valor = request.Valor;
            tarifa.TipoTarifaId = request.TipoTarifaId;
            tarifa.CategoriaId = request.CategoriaId;
            tarifa.Activo = request.Activo;
            tarifa.FechaModificacion = DateTime.Now;

            _context.Tarifas.Update(tarifa);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTarifaAsync(int id)
        {
            var tarifa = await _context.Tarifas.FindAsync(id);
            if (tarifa == null)
            {
                return false;
            }

            tarifa.Activo = false; // Soft delete
            tarifa.FechaModificacion = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        // Tipos de Tarifa
        public async Task<IEnumerable<TipoTarifa>> GetAllTiposTarifaAsync()
        {
            return await _context.TiposTarifa.Where(t => t.Activo).ToListAsync();
        }

        public async Task<TipoTarifa?> GetTipoTarifaByIdAsync(int id)
        {
            return await _context.TiposTarifa.FirstOrDefaultAsync(t => t.Id == id && t.Activo);
        }

        public async Task<TipoTarifa> CreateTipoTarifaAsync(TipoTarifaRequest request)
        {
            var tipoTarifa = new TipoTarifa
            {
                Nombre = request.Nombre,
                FechaCreacion = DateTime.Now,
                Activo = true
            };
            _context.TiposTarifa.Add(tipoTarifa);
            await _context.SaveChangesAsync();
            return tipoTarifa;
        }

        public async Task<bool> UpdateTipoTarifaAsync(UpdateTipoTarifaRequest request)
        {
            var tipoTarifa = await _context.TiposTarifa.FindAsync(request.Id);
            if (tipoTarifa == null)
            {
                return false;
            }

            tipoTarifa.Nombre = request.Nombre;
            tipoTarifa.Activo = request.Activo;
            tipoTarifa.FechaModificacion = DateTime.Now;

            _context.TiposTarifa.Update(tipoTarifa);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTipoTarifaAsync(int id)
        {
            var tipoTarifa = await _context.TiposTarifa.FindAsync(id);
            if (tipoTarifa == null)
            {
                return false;
            }

            tipoTarifa.Activo = false; // Soft delete
            tipoTarifa.FechaModificacion = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        // Categorias
        public async Task<IEnumerable<Categoria>> GetAllCategoriasAsync()
        {
            return await _context.Categorias.Where(c => c.Activo).ToListAsync();
        }

        public async Task<Categoria?> GetCategoriaByIdAsync(int id)
        {
            return await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id && c.Activo);
        }

        public async Task<Categoria> CreateCategoriaAsync(CategoriaRequest request)
        {
            var categoria = new Categoria
            {
                Nombre = request.Nombre,
                FechaCreacion = DateTime.Now,
                Activo = true
            };
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<bool> UpdateCategoriaAsync(UpdateCategoriaRequest request)
        {
            var categoria = await _context.Categorias.FindAsync(request.Id);
            if (categoria == null)
            {
                return false;
            }

            categoria.Nombre = request.Nombre;
            categoria.Activo = request.Activo;
            categoria.FechaModificacion = DateTime.Now;

            _context.Categorias.Update(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoriaAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return false;
            }

            categoria.Activo = false; // Soft delete
            categoria.FechaModificacion = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
