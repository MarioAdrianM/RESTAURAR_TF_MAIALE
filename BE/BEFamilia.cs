using System;

namespace BE
{
    public class BEFamilia : BEEntidad
    {
        public string Nombre { get; set; } = "";
        public bool Activa { get; set; } = true;
        public override string ToString() => Nombre;
    }
}
