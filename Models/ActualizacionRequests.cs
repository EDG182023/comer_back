using System;
using System.ComponentModel.DataAnnotations;

namespace TarifarioBackend.Models
{
    public class ActualizacionMasivaRequest
    {
        [Required]
        public string Criterio { get; set; } = string.Empty; // e.g., "cliente"
        public int? SeleccionId { get; set; }
        public bool IncluirCliente { get; set; }
        public int? ClienteId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public double Porcentaje { get; set; }
        public string Usuario { get; set; } = string.Empty;
    }

    public class ValPrepKilosRequest
    {
        [Required]
        public int ClienteId { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public decimal Minimo { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFinal { get; set; }
    }
}
