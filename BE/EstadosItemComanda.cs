namespace BE
{
    public static class EstadosItemComanda
    {
        public const string Pendiente = "Pendiente";  // recién cargado
        public const string EnPrep = "EnPrep";     // cocina lo toma
        public const string Listo = "Listo";      // cocina lo marca listo
        public const string Anulado = "Anulado";    // anulado por mozo/encargado
    }
}
