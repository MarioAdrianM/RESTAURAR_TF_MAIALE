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
    public class MPPProveedor : IGestor<BEProveedor>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        private void CargarBD()
        {
            if (!File.Exists(ruta))
                new XDocument(new XElement("Root")).Save(ruta);

            BDXML = XDocument.Load(ruta);
            if (BDXML.Root.Element("Proveedores") == null) BDXML.Root.Add(new XElement("Proveedores"));
            if (BDXML.Root.Element("Proveedor_Familias") == null) BDXML.Root.Add(new XElement("Proveedor_Familias"));
            if (BDXML.Root.Element("Familias") == null) BDXML.Root.Add(new XElement("Familias"));
            if (BDXML.Root.Element("Productos") == null) BDXML.Root.Add(new XElement("Productos"));
        }
        private void GuardarBD() { BDXML.Save(ruta); }
        private XElement RootProveedores() { return BDXML.Root.Element("Proveedores"); }

        public bool CrearXML() { CargarBD(); GuardarBD(); return true; }

        public bool Guardar(BEProveedor p)
        {
            CargarBD();
            if (p.Id == 0)
            {
                long nextId = RootProveedores().Elements("Proveedor").Select(x => (long?)x.Attribute("Id") ?? 0).DefaultIfEmpty(0).Max() + 1;
                p.Id = nextId;
                var xe = new XElement("Proveedor",
                    new XAttribute("Id", p.Id),
                    new XAttribute("Nombre", p.Nombre ?? ""),
                    new XAttribute("CUIT", p.CUIT ?? ""),
                    new XAttribute("Activo", p.Activo)
                );
                RootProveedores().Add(xe);
            }
            else
            {
                var xe = RootProveedores().Elements("Proveedor").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == p.Id);
                if (xe == null) throw new Exception("Proveedor no encontrado.");
                xe.SetAttributeValue("Nombre", p.Nombre ?? "");
                xe.SetAttributeValue("CUIT", p.CUIT ?? "");
                xe.SetAttributeValue("Activo", p.Activo);
            }
            GuardarBD();
            return true;
        }

        public bool Eliminar(BEProveedor p)
        {
            CargarBD();
            var xe = RootProveedores().Elements("Proveedor").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == p.Id);
            if (xe != null) { xe.Remove(); GuardarBD(); return true; }
            return false;
        }

        public BEProveedor ListarObjeto(BEProveedor p)
        {
            CargarBD();
            var xe = RootProveedores().Elements("Proveedor").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == p.Id);
            if (xe == null) return null;
            return new BEProveedor
            {
                Id = (long)xe.Attribute("Id"),
                Nombre = (string)xe.Attribute("Nombre") ?? "",
                CUIT = (string)xe.Attribute("CUIT") ?? "",
                Activo = (bool?)xe.Attribute("Activo") ?? true
            };
        }
        public long ObtenerUltimoId()
        {
            CargarBD();
            return RootProveedores().Elements("Proveedor")
                .Select(x => (long?)x.Attribute("Id") ?? 0)
                .DefaultIfEmpty(0)
                .Max();
        }

        public bool VerificarExistenciaObjeto(BEProveedor p)
        {
            if (p == null) return false;
            CargarBD();

            if (p.Id > 0)
                return RootProveedores().Elements("Proveedor")
                    .Any(x => ((long?)x.Attribute("Id") ?? 0) == p.Id);

            var nombre = (p.Nombre ?? "").Trim();
            var cuit = (p.CUIT ?? "").Trim();
            if (nombre.Length == 0 && cuit.Length == 0) return false;

            return RootProveedores().Elements("Proveedor")
                .Any(x =>
                    string.Equals(((string)x.Attribute("Nombre") ?? "").Trim(), nombre, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(((string)x.Attribute("CUIT") ?? "").Trim(), cuit, StringComparison.OrdinalIgnoreCase));
        }

        public List<BEProveedor> ListarTodo()
        {
            CargarBD();
            return RootProveedores().Elements("Proveedor")
                .Select(x => new BEProveedor
                {
                    Id = (long)x.Attribute("Id"),
                    Nombre = (string)x.Attribute("Nombre") ?? "",
                    CUIT = (string)x.Attribute("CUIT") ?? "",
                    Activo = (bool?)x.Attribute("Activo") ?? true
                })
                .OrderBy(x => x.Nombre)
                .ToList();
        }
    }
}
