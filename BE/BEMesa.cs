namespace BE
{
    public class BEMesa : BEEntidad
    {
        public int Numero { get; set; }
        public int Capacidad { get; set; }
        public string Sector { get; set; }           // "Salon", "Terraza", etc.
        public string Estado { get; set; }           // Libre/Ocupada/Bloqueada
        public bool Habilitada { get; set; } = true;
        public string Observaciones { get; set; }
        public override string ToString() => $"Mesa {Numero} ({Capacidad}) - {Estado}";
    }
}
