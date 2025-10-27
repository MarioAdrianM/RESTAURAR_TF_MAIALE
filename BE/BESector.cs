namespace BE
{
    public class BESector : BEEntidad
    {
        public string Nombre { get; set; }
        public bool Activo { get; set; } = true;
        public override string ToString() => Nombre;
    }
}
