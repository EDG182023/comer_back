using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarifarioBackend.Models
{
    [Table("Items")]
    public class Item : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public int CategoriaId { get; set; }

        [ForeignKey("CategoriaId")]
        public Categoria? Categoria { get; set; }
    }

    [Table("Categorias")]
    public class Categoria : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("Unidades")]
    public class Unidad : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("Clientes")]
    public class Cliente : BaseModel
    {
        [Required]
        [StringLength(255)]
        public string Nombre { get; set; } = string.Empty;
        [StringLength(255)]
        [EmailAddress]
        public string? Email { get; set; }
        [StringLength(500)]
        public string? Acuerdo { get; set; }
        [StringLength(1000)]
        public string? Observaciones { get; set; }
    }

    [Table("TiposServicio")]
    public class TipoServicio : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("TiposContenedor")]
    public class TipoContenedor : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("Puertos")]
    public class Puerto : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("Monedas")]
    public class Moneda : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("Conceptos")]
    public class Concepto : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("TiposTarifa")]
    public class TipoTarifa : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("TiposCosto")]
    public class TipoCosto : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("TiposMovimiento")]
    public class TipoMovimiento : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("TiposDocumento")]
    public class TipoDocumento : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;
    }

    [Table("TiposNovedad")]
    public class TipoNovedad : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
    }

    [Table("Novedades")]
    public class Novedad : BaseModel
    {
        [Key]
        public int IdNovedad { get; set; } // ID de la novedad en el sistema externo
        [Required]
        public int TipoNovedadId { get; set; }
        [ForeignKey("TipoNovedadId")]
        public TipoNovedad? TipoNovedad { get; set; }
        [Required]
        public DateTime FechaNovedad { get; set; }
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;
        public bool Ignorada { get; set; } = false;
        public DateTime? FechaIgnorada { get; set; }
        public string? UsuarioIgnora { get; set; }
        public bool EnviadoEmail { get; set; } = false;
        public DateTime? FechaEnvioEmail { get; set; }
        public string? EmailDestino { get; set; }
        public string? ArchivoPdfPath { get; set; }
    }

    [Table("Usuarios")]
    public class Usuario : BaseModel
    {
        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    [Table("Permisos")]
    public class Permiso : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Descripcion { get; set; }
    }

    [Table("Roles")]
    public class Rol : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [StringLength(255)]
        public string? Descripcion { get; set; }
    }

    [Table("RolPermisos")]
    public class RolPermiso : BaseModel
    {
        [Required]
        public int RolId { get; set; }
        [Required]
        public int PermisoId { get; set; }

        [ForeignKey("RolId")]
        public Rol? Rol { get; set; }
        [ForeignKey("PermisoId")]
        public Permiso? Permiso { get; set; }
    }

    [Table("UsuarioRoles")]
    public class UsuarioRol : BaseModel
    {
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public int RolId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }
        [ForeignKey("RolId")]
        public Rol? Rol { get; set; }
    }

    // Models for Saadis_nuevo database (direct mapping to existing tables)
    [Table("COMPOBS")]
    public class CompObs
    {
        [Key]
        public int ID { get; set; }
        public string? CBO_LETCBT { get; set; }
        public string? CBO_CENEMI { get; set; }
        public int? CBO_NROCBT { get; set; }
        public string? CBO_CODCBT { get; set; }
        public string? CBO_NROCLI { get; set; }
        public string? CBO_CODDES { get; set; }
        public string? CLT_RAZSOC { get; set; } // From CLIENTES join
        public DateTime? CBO_FECHAO { get; set; }
        public TimeSpan? CBO_HORAOB { get; set; }
        public string? USUARIO_EMISOR { get; set; } // From USUARIOS join
        public string? USUARIO_RESOL { get; set; } // From USUARIOS join
        public string? CBO_DESCRI { get; set; }
        public int? CBO_ENVIAD { get; set; }
        public int? CBO_PLREMA { get; set; } // Ignorada
        [NotMapped]
        public string? ESTADO_TEXTO { get; set; }
    }

    [Table("COMPROB")]
    public class Comprob
    {
        [Key]
        public int ID { get; set; } // Assuming there's an ID column or a composite key
        public string? CBT_LETCBT { get; set; }
        public string? CBT_CENEMI { get; set; }
        public int? CBT_NROCBT { get; set; }
        public string? CBT_CODCBT { get; set; }
        public DateTime? CBT_FECREC { get; set; }
        public DateTime? CBT_FEPOEN { get; set; }
        public string? CBT_ESTADO { get; set; }
        public string? CBT_NROCLI { get; set; }
        public string? CBT_NRORTE { get; set; }
        public DateTime? CBT_DTORTE { get; set; }
        public DateTime? CBT_FECCBT { get; set; }
        public string? CBT_DETALL { get; set; }
        public string? CBT_ADIC1 { get; set; }
        public string? CBT_FLETER { get; set; }
        public double? CBT_FLEUNI { get; set; }
        public int? CBT_BULTOS { get; set; }
        public double? CBT_KILOSN { get; set; }
        public double? CBT_KILOSA { get; set; }
        public double? CBT_LARGO { get; set; }
        public double? CBT_ANCHO { get; set; }
        public double? CBT_ALTO { get; set; }
        public double? CBT_METRCB { get; set; }
        public string? CBT_NOMRTE { get; set; }
        public double? CBT_METRCU { get; set; }
        public string? CBT_DOMRTE { get; set; }
        public string? CBT_LOCRTE { get; set; }
        public string? CBT_NOMDES { get; set; }
        public string? CBT_DOMDES { get; set; }
        public string? CBT_NRODES { get; set; }
        public DateTime? CBT_DTODES { get; set; }
        public string? CBT_LOCDES { get; set; }
        public string? LCL_CPOSTA { get; set; }
        public string? CBT_ZONADE { get; set; }
        public string? CBT_LINEDE { get; set; }
        public string? CBT_VADESE { get; set; }
        public double? CBT_FLETE { get; set; }
        public string? CBT_DESCRI { get; set; }
    }

    // Email Settings Model
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
    }

    // PDF Settings Model
    public class PdfSettings
    {
        public string LogoPath { get; set; } = string.Empty;
    }
}
