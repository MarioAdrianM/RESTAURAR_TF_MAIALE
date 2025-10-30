using System;
using System.Collections.Generic;

namespace BE
{
    public class BEComanda : BEEntidad
    {
        public long MesaId { get; set; }
        public string MozoUsuario { get; set; } = "";
        public DateTime FechaApertura { get; set; } = DateTime.Now;
        public DateTime? FechaCierre { get; set; } = null;
        public string Estado { get; set; } = EstadosComanda.Abierta;
        public bool FacturaSolicitada { get; set; } = false;

        public List<BEItemComanda> Items { get; set; } = new List<BEItemComanda>();
        public decimal Total { get; set; }   // persistimos el total calculado
    }
}
