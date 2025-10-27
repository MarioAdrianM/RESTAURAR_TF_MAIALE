using Abstraccion;
using Backup;
using BE.BEComposite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace MPP
{
    public class MPPPermiso : IGestor<BEPermiso>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        public bool CrearXML()
        {
            if (!File.Exists(ruta))
            {
                BDXML = new XDocument(new XElement("Root",
                    new XElement("Usuarios"),
                    new XElement("Roles"),
                    new XElement("Permisos"),
                    new XElement("Usuario_Roles"),
                    new XElement("Usuario_Permisos"),
                    new XElement("Rol_Rol"),
                    new XElement("Rol_Permisos")));
                BDXML.Save(ruta);
            }
            else BDXML = XDocument.Load(ruta);
            return true;
        }

        public long ObtenerUltimoId()
        {
            CrearXML();
            var ids = BDXML.Root.Element("Permisos").Elements("permiso").Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public bool VerificarExistenciaObjeto(BEPermiso p)
        {
            CrearXML();
            return BDXML.Root.Element("Permisos").Elements("permiso")
                .Any(x => x.Element("Nombre").Value.Trim().Equals(p.Nombre.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public bool Guardar(BEPermiso p)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            if (p == null) throw new Exception("Permiso nulo.");

            if (p.Id == 0)
            {
                if (VerificarExistenciaObjeto(p)) throw new Exception("El permiso ya existe.");
                p.Id = ObtenerUltimoId() + 1;
                BDXML.Root.Element("Permisos").Add(new XElement("permiso",
                    new XAttribute("Id", p.Id),
                    new XElement("Nombre", p.Nombre.Trim())));
            }
            else
            {
                var q = BDXML.Root.Element("Permisos").Elements("permiso")
                    .Where(x => (long)x.Attribute("Id") == p.Id);
                if (!q.Any()) throw new Exception("Permiso no encontrado.");
                foreach (var n in q) n.Element("Nombre").Value = p.Nombre.Trim();
            }
            BDXML.Save(ruta);
            return true;
        }

        public bool Eliminar(BEPermiso p)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            BDXML.Root.Element("Permisos").Elements("permiso")
                .Where(x => (long)x.Attribute("Id") == p.Id).Remove();

            // limpio relaciones
            BDXML.Root.Element("Rol_Permisos").Elements("rol_permiso")
                .Where(x => (long)x.Element("Id_Permiso") == p.Id).Remove();
            BDXML.Root.Element("Usuario_Permisos").Elements("usuario_permiso")
                .Where(x => (long)x.Element("Id_Permiso") == p.Id).Remove();

            BDXML.Save(ruta);
            return true;
        }

        public List<BEPermiso> ListarTodo()
        {
            CrearXML();
            var lista = new List<BEPermiso>();
            foreach (var x in BDXML.Root.Element("Permisos").Elements("permiso"))
                lista.Add(new BEPermiso((long)x.Attribute("Id"), x.Element("Nombre").Value.Trim()));
            return lista;
        }

        public BEPermiso ListarObjeto(BEPermiso p)
        {
            CrearXML();
            XElement x = null;
            if (p.Id > 0)
                x = BDXML.Root.Element("Permisos").Elements("permiso").FirstOrDefault(n => (long)n.Attribute("Id") == p.Id);
            else
                x = BDXML.Root.Element("Permisos").Elements("permiso").FirstOrDefault(n => n.Element("Nombre").Value.Trim().Equals(p.Nombre.Trim(), StringComparison.OrdinalIgnoreCase));

            if (x == null) return null;
            return new BEPermiso((long)x.Attribute("Id"), x.Element("Nombre").Value.Trim());
        }
    }
}
