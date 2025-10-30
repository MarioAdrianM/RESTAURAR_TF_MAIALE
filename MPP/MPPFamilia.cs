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
    public class MPPFamilia : IGestor<BEFamilia>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        private void CargarBD()
        {
            if (!File.Exists(ruta))
                new XDocument(new XElement("Root")).Save(ruta);

            BDXML = XDocument.Load(ruta);
            if (BDXML.Root.Element("Familias") == null) BDXML.Root.Add(new XElement("Familias"));
            if (BDXML.Root.Element("Proveedores") == null) BDXML.Root.Add(new XElement("Proveedores"));
            if (BDXML.Root.Element("Proveedor_Familias") == null) BDXML.Root.Add(new XElement("Proveedor_Familias"));
            if (BDXML.Root.Element("Productos") == null) BDXML.Root.Add(new XElement("Productos"));
        }
        private void GuardarBD() { BDXML.Save(ruta); }
        private XElement RootFamilias() { return BDXML.Root.Element("Familias"); }

        public bool CrearXML()
        {
            CargarBD(); GuardarBD(); return true;
        }

        public bool Guardar(BEFamilia f)
        {
            CargarBD();
            if (f.Id == 0)
            {
                long nextId = RootFamilias().Elements("Familia").Select(x => (long?)x.Attribute("Id") ?? 0).DefaultIfEmpty(0).Max() + 1;
                f.Id = nextId;
                var xe = new XElement("Familia",
                    new XAttribute("Id", f.Id),
                    new XAttribute("Nombre", f.Nombre ?? ""),
                    new XAttribute("Activa", f.Activa)
                );
                RootFamilias().Add(xe);
            }
            else
            {
                var xe = RootFamilias().Elements("Familia").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == f.Id);
                if (xe == null) throw new Exception("Familia no encontrada.");
                xe.SetAttributeValue("Nombre", f.Nombre ?? "");
                xe.SetAttributeValue("Activa", f.Activa);
            }
            GuardarBD();
            return true;
        }

        public bool Eliminar(BEFamilia f)
        {
            CargarBD();
            var xe = RootFamilias().Elements("Familia").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == f.Id);
            if (xe != null) { xe.Remove(); GuardarBD(); return true; }
            return false;
        }

        public BEFamilia ListarObjeto(BEFamilia f)
        {
            CargarBD();
            var xe = RootFamilias().Elements("Familia").FirstOrDefault(x => ((long?)x.Attribute("Id") ?? 0) == f.Id);
            if (xe == null) return null;
            return new BEFamilia
            {
                Id = (long)xe.Attribute("Id"),
                Nombre = (string)xe.Attribute("Nombre") ?? "",
                Activa = (bool?)xe.Attribute("Activa") ?? true
            };
        }
        public long ObtenerUltimoId()
        {
            CargarBD();
            return RootFamilias().Elements("Familia")
                .Select(x => (long?)x.Attribute("Id") ?? 0)
                .DefaultIfEmpty(0)
                .Max();
        }

        public bool VerificarExistenciaObjeto(BEFamilia f)
        {
            if (f == null) return false;
            CargarBD();

            if (f.Id > 0)
                return RootFamilias().Elements("Familia")
                    .Any(x => ((long?)x.Attribute("Id") ?? 0) == f.Id);

            var nombre = (f.Nombre ?? "").Trim();
            if (nombre.Length == 0) return false;

            return RootFamilias().Elements("Familia")
                .Any(x => string.Equals(
                    ((string)x.Attribute("Nombre") ?? "").Trim(),
                    nombre, StringComparison.OrdinalIgnoreCase));
        }


        public List<BEFamilia> ListarTodo()
        {
            CargarBD();
            return RootFamilias().Elements("Familia")
                .Select(x => new BEFamilia
                {
                    Id = (long)x.Attribute("Id"),
                    Nombre = (string)x.Attribute("Nombre") ?? "",
                    Activa = (bool?)x.Attribute("Activa") ?? true
                })
                .OrderBy(x => x.Nombre)
                .ToList();
        }
    }
}
