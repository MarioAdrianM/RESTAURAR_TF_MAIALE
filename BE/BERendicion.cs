using System;

namespace BE
{
    public class BERendicion : BEEntidad
    {
        public long CajaId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string MozoUsuario { get; set; }

        // Esperado (según pagos registrados por ese mozo y no rendidos)
        public decimal EspEfectivo { get; set; }
        public decimal EspTarjeta { get; set; }
        public decimal EspQR { get; set; }

        // Entregado en la rendición
        public decimal EntEfectivo { get; set; }
        public decimal EntTarjeta { get; set; }
        public decimal EntQR { get; set; }

        public decimal DiferenciaTotal { get; set; } // (Ent - Esp) sumado por medios
        public string Estado { get; set; } = "OK";   // OK | ConDiferencias
        public string Observacion { get; set; }
    }
}

