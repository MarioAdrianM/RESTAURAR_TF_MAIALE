using System;

namespace BE
{
    public class BEItemComanda
    {
        public long ItemId { get; set; }                    // correlativo dentro de la comanda
        public long ProductoId { get; set; }
        public string Descripcion { get; set; } = "";       // snapshot nombre
        public decimal PrecioUnitario { get; set; }         // snapshot precio
        public decimal Cantidad { get; set; }               // admite 0.5, etc.
        public string Estado { get; set; } = EstadosItemComanda.Pendiente;
        public string MotivoAnulacion { get; set; } = null;

        public decimal Subtotal
        {
            get { return Math.Round(PrecioUnitario * Cantidad, 2); }
        }
    }
}
