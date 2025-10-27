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
    public class MPPBitacora : IGestor<BEBitacora>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        private XElement RootBitacora() => BDXML.Root.Element("Bitacora");

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
                        new XElement("Bitacora")     
                    )
                );
                BDXML.Save(ruta);
            }
            else
            {
                BDXML = XDocument.Load(ruta);
                if (BDXML.Root.Element("Bitacora") == null)
                {
                    BDXML.Root.Add(new XElement("Bitacora"));
                    BDXML.Save(ruta);
                }
            }
            return true;
        }

        public long ObtenerUltimoId()
        {
            CrearXML();
            var ids = RootBitacora().Elements("evento").Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public bool Guardar(BEBitacora e)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            if (e.Id == 0) e.Id = ObtenerUltimoId() + 1;

            RootBitacora().Add(
                new XElement("evento",
                    new XAttribute("Id", e.Id),
                    new XElement("Fecha", e.Fecha.ToString("s")),
                    new XElement("Usuario", e.Usuario ?? ""),
                    new XElement("Accion", e.Accion ?? ""),
                    new XElement("Detalle", e.Detalle ?? "")
                )
            );
            BDXML.Save(ruta);
            return true;
        }

        public List<BEBitacora> ListarTodo()
        {
            CrearXML();
            return RootBitacora().Elements("evento")
                .Select(x => new BEBitacora
                {
                    Id = (long)x.Attribute("Id"),
                    Fecha = DateTime.Parse((string)x.Element("Fecha")),
                    Usuario = (string)x.Element("Usuario"),
                    Accion = (string)x.Element("Accion"),
                    Detalle = (string)x.Element("Detalle")
                }).ToList();
        }

        public bool VerificarExistenciaObjeto(BEBitacora obj) => false;

        public bool Eliminar(BEBitacora obj)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            var nodo = RootBitacora().Elements("evento")
                .FirstOrDefault(x => (long)x.Attribute("Id") == obj.Id);
            if (nodo != null) { nodo.Remove(); BDXML.Save(ruta); }
            return true;
        }

        public BEBitacora ListarObjeto(BEBitacora obj)
        {
            CrearXML();
            var x = RootBitacora().Elements("evento").FirstOrDefault(n => (long)n.Attribute("Id") == obj.Id);
            if (x == null) return null;
            return new BEBitacora
            {
                Id = (long)x.Attribute("Id"),
                Fecha = DateTime.Parse((string)x.Element("Fecha")),
                Usuario = (string)x.Element("Usuario"),
                Accion = (string)x.Element("Accion"),
                Detalle = (string)x.Element("Detalle")
            };
        }
    }
}
