using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarifarioBackend.Models
{
    [Table("TarifarioGeneral1")]
    public class TarifarioGeneral1 : BaseModel
    {
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public int UnidadId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public double Precio { get; set; }
        public double? Minimo { get; set; }
        [Required]
        public double Incremento { get; set; }
        [Required]
        public DateTime FechaVigenciaInicio { get; set; }
        public DateTime? FechaVigenciaFinal { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
        [ForeignKey("UnidadId")]
        public Unidad? Unidad { get; set; }
        [ForeignKey("ItemId")]
        public Item? Item { get; set; }
    }

    [Table("TarifarioGeneral2")]
    public class TarifarioGeneral2 : BaseModel
    {
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public int UnidadId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public double Precio { get; set; }
        [Required]
        public double Desde { get; set; }
        [Required]
        public double Hasta { get; set; }
        [Required]
        public double Incremento { get; set; }
        [Required]
        public DateTime FechaVigenciaInicio { get; set; }
        public DateTime? FechaVigenciaFinal { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
        [ForeignKey("UnidadId")]
        public Unidad? Unidad { get; set; }
        [ForeignKey("ItemId")]
        public Item? Item { get; set; }
    }

    [Table("TarifarioEcommerce")]
    public class TarifarioEcommerce : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Localidad { get; set; } = string.Empty;
        [Required]
        public int UnidadId { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [Required]
        [StringLength(100)]
        public string PlazosEntrega { get; set; } = string.Empty;
        [Required]
        public double RangoMin { get; set; }
        [Required]
        public double RangoMax { get; set; }
        [Required]
        public double Precio { get; set; }
        [Required]
        public DateTime FechaVigenciaInicio { get; set; }
        public DateTime? FechaVigenciaFinal { get; set; }
        [Required]
        public double Incremento { get; set; }

        [ForeignKey("UnidadId")]
        public Unidad? Unidad { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
    }

    [Table("TarifarioGeneralHistorico")]
    public class TarifarioGeneralHistorico : BaseModel
    {
        [Required]
        public int TarifarioGeneralId { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public int UnidadId { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public double Precio { get; set; }
        public double? Minimo { get; set; }
        [Required]
        public double Incremento { get; set; }
        [Required]
        public DateTime FechaVigenciaInicio { get; set; }
        public DateTime? FechaVigenciaFinal { get; set; }
        [Required]
        [StringLength(50)]
        public string UsuarioActualizacion { get; set; } = string.Empty;

        [ForeignKey("TarifarioGeneralId")]
        public TarifarioGeneral1? TarifarioGeneral { get; set; }
    }

    [Table("TarifarioEcommerceHistorico")]
    public class TarifarioEcommerceHistorico : BaseModel
    {
        [Required]
        public int TarifarioEcommerceId { get; set; }
        [Required]
        [StringLength(100)]
        public string Localidad { get; set; } = string.Empty;
        [Required]
        public int UnidadId { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [Required]
        [StringLength(100)]
        public string PlazosEntrega { get; set; } = string.Empty;
        [Required]
        public double RangoMin { get; set; }
        [Required]
        public double RangoMax { get; set; }
        [Required]
        public double Precio { get; set; }
        [Required]
        public DateTime FechaVigenciaInicio { get; set; }
        public DateTime? FechaVigenciaFinal { get; set; }
        [Required]
        public double Incremento { get; set; }
        [Required]
        [StringLength(50)]
        public string UsuarioActualizacion { get; set; } = string.Empty;

        [ForeignKey("TarifarioEcommerceId")]
        public TarifarioEcommerce? TarifarioEcommerce { get; set; }
    }

    [Table("Tarifas")]
    public class Tarifa : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Valor { get; set; }

        public int TipoTarifaId { get; set; }
        [ForeignKey("TipoTarifaId")]
        public TipoTarifa? TipoTarifa { get; set; }

        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }
    }

    public class TipoTarifa : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class Categoria : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

    public class Tarifario
    {
        [Key]
        public int IdTarifario { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime FechaInicioVigencia { get; set; }
        public DateTime? FechaFinVigencia { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }

    public class DetalleTarifario
    {
        [Key]
        public int IdDetalleTarifario { get; set; }
        public int IdTarifario { get; set; }
        [ForeignKey("IdTarifario")]
        public Tarifario? Tarifario { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal ValorConcepto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
}
