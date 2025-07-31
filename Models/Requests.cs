using System.ComponentModel.DataAnnotations;

namespace TarifarioBackend.Models
{
    // Tarifario Requests
    public class TarifaRequest
    {
        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public int TipoTarifaId { get; set; }
        [Required]
        public int CategoriaId { get; set; }
    }

    public class UpdateTarifaRequest : TarifaRequest
    {
        [Required]
        public int Id { get; set; }
        public bool Activo { get; set; }
    }

    public class TipoTarifaRequest
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class UpdateTipoTarifaRequest : TipoTarifaRequest
    {
        [Required]
        public int Id { get; set; }
        public bool Activo { get; set; }
    }

    public class CategoriaRequest
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class UpdateCategoriaRequest : CategoriaRequest
    {
        [Required]
        public int Id { get; set; }
        public bool Activo { get; set; }
    }

// Maestros Requests
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
    [StringLength(50)]
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

public class ActualizacionMasivaRequest
{
    [Required]
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    [Required]
    public double Porcentaje { get; set; }
    public string Criterio { get; set; } = string.Empty;
    public int? SeleccionId { get; set; }
    public bool IncluirCliente { get; set; }
    public int? ClienteId { get; set; }
    [Required]
    public string Usuario { get; set; } = string.Empty;
}

public class ValPrepKilosRequest
{
    [Required]
    public int ClienteId { get; set; }
    [Required]
    public double Valor { get; set; }
    public double? Minimo { get; set; }
    [Required]
    public DateTime FechaInicio { get; set; }
    [Required]
    public DateTime FechaFinal { get; set; }
}
    public class TipoDocumentoRequest
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class UpdateTipoDocumentoRequest : TipoDocumentoRequest
    {
        [Required]
        public int Id { get; set; }
        public bool Activo { get; set; }
    }

    // Novedades Requests
    public class NovedadRequest
    {
        [Required]
        public int IdNovedad { get; set; }
        [Required]
        public int TipoNovedadId { get; set; }
        [Required]
        public DateTime FechaNovedad { get; set; }
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        public string? EmailDestino { get; set; }
    }

    public class UpdateNovedadRequest : NovedadRequest
    {
        [Required]
        public int Id { get; set; }
        public bool Ignorada { get; set; }
        public string? UsuarioIgnora { get; set; }
        public bool EnviadoEmail { get; set; }
    }

    public class IgnorarNovedadRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UsuarioIgnora { get; set; } = string.Empty;
    }

    public class IgnorarMultiplesRequest
    {
        [Required]
        public List<int> Ids { get; set; } = new List<int>();
        [Required]
        public string UsuarioIgnora { get; set; } = string.Empty;
    }

    public class ReimprimirPdfRequest
    {
        [Required]
        public int Id { get; set; }
    }

    public class ReimprimirPdfMasivoRequest
    {
        [Required]
        public List<int> Ids { get; set; } = new List<int>();
    }

    public class ReenviarMailRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string EmailDestino { get; set; } = string.Empty;
    }

    public class ReenviarMailMasivoRequest
    {
        [Required]
        public List<int> Ids { get; set; } = new List<int>();
        [Required]
        [EmailAddress]
        public string EmailDestino { get; set; } = string.Empty;
    }

    public class NovedadesMultiplesRequest
    {
        [Required]
        public List<NovedadRequest> Novedades { get; set; } = new List<NovedadRequest>();
    }

    // Actualizacion Requests
    public class ActualizacionRequest
    {
        [Required]
        public string TipoActualizacion { get; set; } = string.Empty; // e.g., "Tarifas", "Maestros"
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string? Resultado { get; set; }
        public bool Exitosa { get; set; }
    }

    // Reportes Requests
    public class ReporteTarifasRequest
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? TipoTarifaId { get; set; }
        public int? CategoriaId { get; set; }
    }

    public class ReporteNovedadesRequest
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int? TipoNovedadId { get; set; }
        public bool? Ignorada { get; set; }
        public bool? EnviadoEmail { get; set; }
    }
}
