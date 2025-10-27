using System;

namespace BE
{
    public class BECaja : BEEntidad
    {
        public string Nombre => $"{Punto} {FechaApertura:dd/MM/yy} {Turno}";
        public DateTime FechaApertura { get; set; } = DateTime.Now;
        public string Punto { get; set; } = "Caja1"; // texto libre o "1"
        public string Turno { get; set; } = "Noche"; // Mañana/Tarde/Noche (o texto)
        public string Responsable { get; set; }      // usuario cajero que abrió
        public decimal FondoInicial { get; set; }
        public decimal UmbralDiferencia { get; set; } = 50m;
        public bool Abierta { get; set; } = true;
    }
}
