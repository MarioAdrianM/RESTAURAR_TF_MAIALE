using System;

namespace BE
{
    public class BEReserva : BEEntidad
    {
        public DateTime Fecha { get; set; }     // solo día
        public string Hora { get; set; }     // "HH:mm"
        public int Comensales { get; set; }
        public long MesaId { get; set; }    // 0 si aún no asignada
        public string Estado { get; set; }    // EstadosReserva.*
        public string Observaciones { get; set; }

        public string ClienteNombreCompleto { get; set; } // obligatorio
        public string ClienteTelefono { get; set; }       // obligatorio
        public string ClienteEmail { get; set; }          // opcional

    }
}
