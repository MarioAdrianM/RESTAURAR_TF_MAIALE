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
    public class MPPRol : IGestor<BERol>
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
        // MPPRol.cs  (dentro de namespace MPP, **dentro** de la clase MPPRol)
        public bool AgregarPermisoSiNoExiste(long idRol, long idPermiso)
        {
            try
            {
                CrearXML(); BDXML = XDocument.Load(ruta);

                var rol = BDXML.Root.Element("Roles").Elements("rol")
                          .FirstOrDefault(x => (long)x.Attribute("Id") == idRol);
                if (rol == null) throw new Exception("Rol no encontrado.");

                var perm = BDXML.Root.Element("Permisos").Elements("permiso")
                           .FirstOrDefault(x => (long)x.Attribute("Id") == idPermiso);
                if (perm == null) throw new Exception("Permiso no encontrado.");

                var existe = BDXML.Root.Element("Rol_Permisos").Elements("rol_permiso")
                              .Any(x => (long)x.Element("Id_Rol") == idRol &&
                                        (long)x.Element("Id_Permiso") == idPermiso);
                if (!existe)
                {
                    BDXML.Root.Element("Rol_Permisos").Add(new XElement("rol_permiso",
                        new XElement("Id_Rol", idRol),
                        new XElement("Id_Permiso", idPermiso)));
                    BDXML.Save(ruta);
                }
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public long ObtenerUltimoId()
        {
            CrearXML();
            var ids = BDXML.Root.Element("Roles").Elements("rol").Select(x => (long)x.Attribute("Id"));
            return ids.Any() ? ids.Max() : 0;
        }

        public bool VerificarExistenciaObjeto(BERol r)
        {
            CrearXML();
            return BDXML.Root.Element("Roles").Elements("rol")
                .Any(x => x.Element("Nombre").Value.Trim().Equals(r.Nombre.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public bool Guardar(BERol r)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            if (r == null) throw new Exception("Rol nulo.");

            if (r.Id == 0)
            {
                if (VerificarExistenciaObjeto(r)) throw new Exception("El rol ya existe.");
                r.Id = ObtenerUltimoId() + 1;
                BDXML.Root.Element("Roles").Add(new XElement("rol",
                    new XAttribute("Id", r.Id),
                    new XElement("Nombre", r.Nombre.Trim())));
            }
            else
            {
                var q = BDXML.Root.Element("Roles").Elements("rol")
                    .Where(x => (long)x.Attribute("Id") == r.Id);
                if (!q.Any()) throw new Exception("Rol no encontrado.");
                foreach (var n in q) n.Element("Nombre").Value = r.Nombre.Trim();
            }
            BDXML.Save(ruta);
            return true;
        }

        public bool Eliminar(BERol r)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            BDXML.Root.Element("Roles").Elements("rol")
                .Where(x => (long)x.Attribute("Id") == r.Id).Remove();

            // limpio relaciones
            BDXML.Root.Element("Usuario_Roles").Elements("usuario_rol")
                .Where(x => (long)x.Element("Id_Rol") == r.Id).Remove();
            BDXML.Root.Element("Rol_Rol").Elements("rol_rol")
                .Where(x => (long)x.Element("Id_Padre") == r.Id || (long)x.Element("Id_Hijo") == r.Id).Remove();
            BDXML.Root.Element("Rol_Permisos").Elements("rol_permiso")
                .Where(x => (long)x.Element("Id_Rol") == r.Id).Remove();

            BDXML.Save(ruta);
            return true;
        }

        public List<BERol> ListarTodo()
        {
            CrearXML();
            var lista = new List<BERol>();
            foreach (var x in BDXML.Root.Element("Roles").Elements("rol"))
                lista.Add(new BERol((long)x.Attribute("Id"), x.Element("Nombre").Value.Trim()));
            return lista;
        }

        public BERol ListarObjeto(BERol r)
        {
            CrearXML();
            XElement x = null;
            if (r.Id > 0)
                x = BDXML.Root.Element("Roles").Elements("rol").FirstOrDefault(n => (long)n.Attribute("Id") == r.Id);
            else
                x = BDXML.Root.Element("Roles").Elements("rol").FirstOrDefault(n => n.Element("Nombre").Value.Trim().Equals(r.Nombre.Trim(), StringComparison.OrdinalIgnoreCase));

            if (x == null) return null;
            var rol = new BERol((long)x.Attribute("Id"), x.Element("Nombre").Value.Trim());
            // completo jerarquía para visualización:
            CargarJerarquia(rol);
            return rol;
        }

        private void CargarJerarquia(BERol rolPadre)
        {
            // hijos rol
            var hijosRol = BDXML.Root.Element("Rol_Rol").Elements("rol_rol")
                .Where(x => (long)x.Element("Id_Padre") == rolPadre.Id)
                .Select(x => (long)x.Element("Id_Hijo")).ToList();

            foreach (var idHijo in hijosRol)
            {
                var hijo = new BERol(idHijo, ObtenerNombreRol(idHijo));
                rolPadre.Agregar(hijo);
                CargarJerarquia(hijo);
            }

            // permisos del rol
            var perms = BDXML.Root.Element("Rol_Permisos").Elements("rol_permiso")
                .Where(x => (long)x.Element("Id_Rol") == rolPadre.Id)
                .Select(x => (long)x.Element("Id_Permiso")).ToList();

            foreach (var idp in perms)
                rolPadre.Agregar(new BEPermiso(idp, ObtenerNombrePermiso(idp)));
        }

        private string ObtenerNombreRol(long idRol)
        {
            var r = BDXML.Root.Element("Roles").Elements("rol").FirstOrDefault(x => (long)x.Attribute("Id") == idRol);
            return r != null ? r.Element("Nombre").Value : "";
        }
        private string ObtenerNombrePermiso(long idPer)
        {
            var p = BDXML.Root.Element("Permisos").Elements("permiso").FirstOrDefault(x => (long)x.Attribute("Id") == idPer);
            return p != null ? p.Element("Nombre").Value : "";
        }

        // Relaciones Rol -Permiso / Rol - Rol
        public bool AsociarRolPermiso(BERol r, BEPermiso p)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            // asegurar existencia en catálogos
            if (!BDXML.Root.Element("Roles").Elements("rol").Any(x => (long)x.Attribute("Id") == r.Id))
                throw new Exception("Rol inexistente.");
            if (!BDXML.Root.Element("Permisos").Elements("permiso").Any(x => (long)x.Attribute("Id") == p.Id))
                throw new Exception("Permiso inexistente.");

            var existe = BDXML.Root.Element("Rol_Permisos").Elements("rol_permiso")
                .Any(x => (long)x.Element("Id_Rol") == r.Id && (long)x.Element("Id_Permiso") == p.Id);
            if (!existe)
            {
                BDXML.Root.Element("Rol_Permisos").Add(new XElement("rol_permiso",
                    new XElement("Id_Rol", r.Id),
                    new XElement("Id_Permiso", p.Id)));
                BDXML.Save(ruta);
            }
            return true;
        }

        public bool DesasociarRolPermiso(BERol r, BEPermiso p)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            BDXML.Root.Element("Rol_Permisos").Elements("rol_permiso")
                .Where(x => (long)x.Element("Id_Rol") == r.Id && (long)x.Element("Id_Permiso") == p.Id).Remove();
            BDXML.Save(ruta);
            return true;
        }

        public bool AsociarRolRol(BERol rolPadre, BERol rolHijo)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            if (rolPadre.Id == rolHijo.Id) throw new Exception("No se puede asociar un rol a sí mismo.");

            var existe = BDXML.Root.Element("Rol_Rol").Elements("rol_rol")
                .Any(x => (long)x.Element("Id_Padre") == rolPadre.Id && (long)x.Element("Id_Hijo") == rolHijo.Id);
            if (!existe)
            {
                BDXML.Root.Element("Rol_Rol").Add(new XElement("rol_rol",
                    new XElement("Id_Padre", rolPadre.Id),
                    new XElement("Id_Hijo", rolHijo.Id)));
                BDXML.Save(ruta);
            }
            return true;
        }

        public bool DesasociarRolRol(BERol rolPadre, BERol rolHijo)
        {
            CrearXML(); BDXML = XDocument.Load(ruta);
            BDXML.Root.Element("Rol_Rol").Elements("rol_rol")
                .Where(x => (long)x.Element("Id_Padre") == rolPadre.Id && (long)x.Element("Id_Hijo") == rolHijo.Id).Remove();
            BDXML.Save(ruta);
            return true;
        }
    }
}
