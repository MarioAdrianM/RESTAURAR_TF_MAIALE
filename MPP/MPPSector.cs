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
    public class MPPSector : IGestor<BESector>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        public MPPSector() { CrearXML(); }

        public bool CrearXML()
        {
            if (!File.Exists(ruta))
            {
                BDXML = new XDocument(
                    new XElement("Root",
                        new XElement("Usuarios"),
                        new XElement("Roles"),
                        new XElement("Permisos"),
                        new XElement("Usuario_Roles"),
                        new XElement("Usuario_Permisos"),
                        new XElement("Rol_Rol"),
                        new XElement("Rol_Permisos"),
                        new XElement("Bitacora"),
                        new XElement("Sectores"),
                        new XElement("Mesas")
                    )
                );
                BDXML.Save(ruta);
            }
            else
            {
                BDXML = XDocument.Load(ruta);
                if (BDXML.Root.Element("Sectores") == null)
                {
                    BDXML.Root.Add(new XElement("Sectores"));
                    BDXML.Save(ruta);
                }
            }

            // Semilla por defecto
            BDXML = XDocument.Load(ruta);
            var nod = BDXML.Root.Element("Sectores");
            if (!nod.Elements("sector").Any())
            {
                nod.Add(new XElement("sector",
                    new XAttribute("Id", 1),
                    new XElement("Nombre", "Salon"),
                    new XElement("Activo", "false")
                ));
                BDXML.Save(ruta);
            }
            return true;
        }

        public long ObtenerUltimoId()
        {
            CrearXML();
            var ids = BDXML.Root.Element("Sectores").Elements("sector")
                .Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public bool VerificarExistenciaObjeto(BESector s)
        {
            CrearXML();
            if (s == null || string.IsNullOrWhiteSpace(s.Nombre)) return false;
            var nombre = s.Nombre.Trim();

            if (s.Id > 0)
                return BDXML.Root.Element("Sectores").Elements("sector")
                    .Any(x => string.Equals((string)x.Element("Nombre"), nombre, StringComparison.OrdinalIgnoreCase)
                           && (long)x.Attribute("Id") != s.Id);

            return BDXML.Root.Element("Sectores").Elements("sector")
                .Any(x => string.Equals((string)x.Element("Nombre"), nombre, StringComparison.OrdinalIgnoreCase));
        }

        public bool Guardar(BESector s)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);

            if (s.Id == 0)
            {
                if (VerificarExistenciaObjeto(s)) throw new Exception("Ya existe un sector con ese nombre.");
                s.Id = ObtenerUltimoId() + 1;
                BDXML.Root.Element("Sectores").Add(Serializar(s));
            }
            else
            {
                if (VerificarExistenciaObjeto(s)) throw new Exception("Ya existe un sector con ese nombre.");

                var n = BDXML.Root.Element("Sectores").Elements("sector")
                        .FirstOrDefault(x => (long)x.Attribute("Id") == s.Id)
                        ?? throw new Exception("Sector no encontrado.");

                n.Element("Nombre").Value = s.Nombre?.Trim() ?? "";
                n.Element("Activo").Value = s.Activo ? "true" : "false";
            }
            BDXML.Save(ruta);
            return true;
        }

        public bool Eliminar(BESector s)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);

            // Evitar borrar si hay mesas con ese sector
            var nombre = s.Nombre?.Trim();
            bool enUso = BDXML.Root.Element("Mesas")?
                .Elements("mesa")
                .Any(x => string.Equals((string)x.Element("Sector"), nombre, StringComparison.OrdinalIgnoreCase))
                ?? false;

            if (enUso) throw new Exception("No se puede eliminar: hay mesas usando este sector. Desactívelo.");

            BDXML.Root.Element("Sectores").Elements("sector")
                .Where(x => (long)x.Attribute("Id") == s.Id).Remove();
            BDXML.Save(ruta);
            return true;
        }

        public List<BESector> ListarTodo()
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            return BDXML.Root.Element("Sectores").Elements("sector").Select(Deserializar).ToList();
        }

        public BESector ListarObjeto(BESector filtro)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            if (filtro.Id > 0)
            {
                var x = BDXML.Root.Element("Sectores").Elements("sector")
                    .FirstOrDefault(n => (long)n.Attribute("Id") == filtro.Id);
                return x == null ? null : Deserializar(x);
            }
            return null;
        }

        // helpers
        private static XElement Serializar(BESector s) =>
            new XElement("sector",
                new XAttribute("Id", s.Id),
                new XElement("Nombre", s.Nombre?.Trim() ?? ""),
                new XElement("Activo", s.Activo ? "true" : "false")
            );

        private static BESector Deserializar(XElement x) =>
            new BESector
            {
                Id = (long)x.Attribute("Id"),
                Nombre = (string)x.Element("Nombre"),
                Activo = (bool?)x.Element("Activo") ?? true
            };
    }
}
