using System;

namespace BE
{
    public class BEPago : BEEntidad
    {
        public DateTime Fecha { get; set; } = DateTime.Now;
        public long FacturaId { get; set; }
        public long MesaId { get; set; }
        public string Medio { get; set; } // "Efectivo" | "Tarjeta" | "QR"
        public decimal Monto { get; set; }
        public string Autorizacion { get; set; } // requerido para Tarjeta/QR
        public decimal Vuelto { get; set; } // solo efectivo
        public string Estado { get; set; } = "EnCustodiaMozo"; // flujo simple
        public string Cobrador { get; set; } // Usuario (mozo) que registró el cobro

    }
}
