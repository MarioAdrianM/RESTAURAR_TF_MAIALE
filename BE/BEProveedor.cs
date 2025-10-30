using System;

namespace BE
{
    public class BEProveedor : BEEntidad
    {
        public string Nombre { get; set; } = "";
        public string CUIT { get; set; } = "";
        public bool Activo { get; set; } = true;

        // Si después querés contactos, dirección, etc., lo agregamos.
        public override string ToString() => $"{Nombre} ({CUIT})";
    }
}
