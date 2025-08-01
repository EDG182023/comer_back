using System.ComponentModel.DataAnnotations;

namespace TarifarioBackend.Models
{
    // Request DTOs for the Maestros module
    public class ItemRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public int Categoria { get; set; }
    }

    public class UnidadRequest
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class ClienteRequest
    {
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;
        [EmailAddress]
        public string? Email { get; set; }
        public string? Acuerdo { get; set; }
        public string? Observaciones { get; set; }
    }

    public class TipoServicioRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoContenedorRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class PuertoRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class MonedaRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class ConceptoRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoCostoRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoMovimientoRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoNovedadRequest
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
    }

    public class UpdateTipoNovedadRequest : TipoNovedadRequest
    {
        [Required]
        public int Id { get; set; }
        public bool Activo { get; set; }
    }
}
