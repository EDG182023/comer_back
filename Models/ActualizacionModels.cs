using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarifarioBackend.Models
{
    public class Actualizacion : BaseModel
    {
        [Required]
        [StringLength(100)]
        public string TipoActualizacion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        [StringLength(500)]
        public string? Resultado { get; set; }
        public bool Exitosa { get; set; }
    }
}
