using System;

namespace BE
{
    public class BEProducto : BEEntidad
    {
        public string Nombre { get; set; } = "";
        public long FamiliaId { get; set; }         // 1..1
        public decimal PrecioVenta { get; set; }     // precio vigente (snapshot se toma en ItemComanda)
        public bool Activo { get; set; } = true;

        // Opcional: proveedor “preferido” (NO obligatorio para ventas)
        public long? ProveedorPrincipalId { get; set; }

        public override string ToString() => $"{Nombre} - ${PrecioVenta:n2}";
    }
}
