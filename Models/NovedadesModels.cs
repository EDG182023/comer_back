using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarifarioBackend.Models
{

    public class NovedadFilter
    {
        public string? Cliente { get; set; }
        public int? Estado { get; set; } // 0: Pendiente, 1: Atendido, 2: Ignorado
        public string? TipoObservacion { get; set; }
        public string? FechaDesde { get; set; }
        public string? FechaHasta { get; set; }
        public bool IncluirIgnorados { get; set; } = false;
        public int Limit { get; set; } = 10;
        public int Page { get; set; } = 1;
    }

    public class NovedadResponse
    {
        public IEnumerable<CompObs> Data { get; set; } = new List<CompObs>();
        public PaginationInfo Pagination { get; set; } = new PaginationInfo();
    }

    public class PaginationInfo
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalPages { get; set; }
    }

    public class NovedadContadores
    {
        public int Pendientes { get; set; }
        public int Atendidos { get; set; }
        public int Ignorados { get; set; }
        public int Total { get; set; }
    }

    public class TipoObservacion
    {
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public class NovedadExportRow
    {
        public string CodigoNovedad { get; set; } = string.Empty;
        public string NumeroComprobante { get; set; } = string.Empty;
        public string CodigoCliente { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string CodigoDestino { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string FechaNovedad { get; set; } = string.Empty;
        public string UsuarioEmisor { get; set; } = string.Empty;
        public string UsuarioResolvente { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }

}
