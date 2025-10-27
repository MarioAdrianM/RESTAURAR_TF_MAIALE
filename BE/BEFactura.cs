using System;

namespace BE
{
    public class BEFactura : BEEntidad
    {
        public DateTime Fecha { get; set; } = DateTime.Now;
        public long MesaId { get; set; }
        public int PuntoVenta { get; set; } = 1;        // Ej.: 1
        public string TipoCbte { get; set; } = "B";     // "B","C","X" (simple)
        public long Numero { get; set; }                // correlativo por PV+Tipo
        public decimal ImporteTotal { get; set; }
        public string Estado { get; set; } = "Emitida"; // "Emitida" | "Anulada"

        public override string ToString()
            => $"{TipoCbte}-{PuntoVenta:0000}-{Numero:00000000}";
    }
}
