using Abstraccion;
using Backup;
using BE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MPP
{
    public class MPPProducto : IGestor<BEProducto>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        private void CargarBD()
        {
            if (!File.Exists(ruta))
                new XDocument(new XElement("Root")).Save(ruta);

            BDXML = XDocument.Load(ruta);
            if (BDXML.Root.Element("Productos") == null) BDXML.Root.Add(new XElement("Productos"));
            if (BDXML.Root.Element("Familias") == null) BDXML.Root.Add(new XElement("Familias"));
            if (BDXML.Root.Element("Proveedores") == null) BDXML.Root.Add(new XElement("Proveedores"));
            if (BDXML.Root.Element("Proveedor_Familias") == null) BDXML.Root.Add(new XElement("Proveedor_Familias"));
        }
        private void GuardarBD() { BDXML.Save(ruta); }
        private XElement RootProductos() { return BDXML.Root.Element("Productos"); }

        public bool CrearXML() { CargarBD(); GuardarBD(); return true; }

        public bool Guardar(BEProducto p)
        {
            CargarBD();

            if (p.Id == 0)
            {
                long nextId = RootProductos().Elements("Producto").Select(x => (long?)x.Attribute("Id") ?? 0).DefaultIfEmpty(0).Max() + 1;
                p.Id = nextId;
                var xe = new XElement("Producto",
                    new XAttribute("Id", p.Id),
                    new XAttribute("Nombre", p.Nombre ?? ""),
                    new XAttribute("FamiliaId", p.FamiliaId),
                    new XAttribute("PrecioVenta", p.PrecioVenta),
                    new XAttribute("Activo", p.Activo)
                );
                if (p.ProveedorPrincipalId.HasValue)
                    xe.Add(new XAttribute("ProveedorPrincipalId", p.ProveedorPrincipalId.Value));

                RootProductos().Add(xe);
            }
            else
            {
                var xe = RootProductos().Elements("Producto").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == p.Id);
                if (xe == null) throw new Exception("Producto no encontrado.");
                xe.SetAttributeValue("Nombre", p.Nombre ?? "");
                xe.SetAttributeValue("FamiliaId", p.FamiliaId);
                xe.SetAttributeValue("PrecioVenta", p.PrecioVenta);
                xe.SetAttributeValue("Activo", p.Activo);
                if (p.ProveedorPrincipalId.HasValue)
                    xe.SetAttributeValue("ProveedorPrincipalId", p.ProveedorPrincipalId.Value);
                else
                    xe.Attribute("ProveedorPrincipalId")?.Remove();
            }

            GuardarBD();
            return true;
        }

        public bool Eliminar(BEProducto p)
        {
            CargarBD();
            var xe = RootProductos().Elements("Producto").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == p.Id);
            if (xe != null) { xe.Remove(); GuardarBD(); return true; }
            return false;
        }

        public BEProducto ListarObjeto(BEProducto p)
        {
            CargarBD();
            var xe = RootProductos().Elements("Producto").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == p.Id);
            if (xe == null) return null;

            var prod = new BEProducto
            {
                Id = (long)xe.Attribute("Id"),
                Nombre = (string)xe.Attribute("Nombre") ?? "",
                FamiliaId = (long)xe.Attribute("FamiliaId"),
                PrecioVenta = (decimal)xe.Attribute("PrecioVenta"),
                Activo = (bool?)xe.Attribute("Activo") ?? true
            };
            var attProv = xe.Attribute("ProveedorPrincipalId");
            if (attProv != null) prod.ProveedorPrincipalId = (long)attProv;
            return prod;
        }
        public long ObtenerUltimoId()
        {
            CargarBD();
            return RootProductos().Elements("Producto")
                .Select(x => (long?)x.Attribute("Id") ?? 0)
                .DefaultIfEmpty(0)
                .Max();
        }

        public bool VerificarExistenciaObjeto(BEProducto p)
        {
            if (p == null) return false;
            CargarBD();

            if (p.Id > 0)
                return RootProductos().Elements("Producto")
                    .Any(x => ((long?)x.Attribute("Id") ?? 0) == p.Id);

            var nombre = (p.Nombre ?? "").Trim();
            var famId = p.FamiliaId;
            if (nombre.Length == 0) return false;

            // clave natural: Nombre + FamiliaId
            return RootProductos().Elements("Producto")
                .Any(x =>
                    string.Equals(((string)x.Attribute("Nombre") ?? "").Trim(), nombre, StringComparison.OrdinalIgnoreCase)
                    && ((long?)x.Attribute("FamiliaId") ?? 0) == famId);
        }

        public List<BEProducto> ListarTodo()
        {
            CargarBD();
            return RootProductos().Elements("Producto")
                .Select(x =>
                {
                    var prod = new BEProducto
                    {
                        Id = (long)x.Attribute("Id"),
                        Nombre = (string)x.Attribute("Nombre") ?? "",
                        FamiliaId = (long)x.Attribute("FamiliaId"),
                        PrecioVenta = (decimal)x.Attribute("PrecioVenta"),
                        Activo = (bool?)x.Attribute("Activo") ?? true
                    };
                    var attProv = x.Attribute("ProveedorPrincipalId");
                    if (attProv != null) prod.ProveedorPrincipalId = (long)attProv;
                    return prod;
                })
                .OrderBy(x => x.Nombre)
                .ToList();
        }
    }
}
