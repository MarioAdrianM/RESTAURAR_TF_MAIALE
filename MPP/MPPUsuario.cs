using Abstraccion;
using Backup;
using BE;
using BE.BEComposite;
using Seguridad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;

namespace MPP
{
    public class MPPUsuario : IGestor<BEUsuario>
    {
        readonly string ruta = GestorCarpeta.UbicacionBD("BD.xml");
        XDocument BDXML;

        // ==================== Infra ====================
        private void EnsureBD()
        {
            CrearXML();
            BDXML = XDocument.Load(ruta);
        }

        #region Métodos de soporte
        private XElement RootUsuarios() => BDXML.Root.Element("Usuarios");
        private XElement RootRoles() => BDXML.Root.Element("Roles");
        private XElement RootPermisos() => BDXML.Root.Element("Permisos");
        private XElement RootUsuarioRoles() => BDXML.Root.Element("Usuario_Roles");
        private XElement RootUsuarioPermisos() => BDXML.Root.Element("Usuario_Permisos");
        private void AseguroRoot(string name)
        {
            if (BDXML.Root.Element(name) == null)
                BDXML.Root.Add(new XElement(name));
        }
        #endregion

        #region IGestor
        public bool CrearXML()
        {
            try
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
                        ));
                    BDXML.Save(ruta);
                }
                else
                {
                    BDXML = XDocument.Load(ruta);
                    AseguroRoot("Usuarios");
                    AseguroRoot("Roles");
                    AseguroRoot("Permisos");
                    AseguroRoot("Usuario_Roles");
                    AseguroRoot("Usuario_Permisos");
                    AseguroRoot("Rol_Rol");
                    AseguroRoot("Rol_Permisos");
                    AseguroRoot("Bitacora");
                    BDXML.Save(ruta);
                }
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public long ObtenerUltimoId()
        {
            try
            {
                EnsureBD();
                var ids = RootUsuarios().Elements("usuario").Select(x => (long)x.Attribute("Id"));
                return ids.Any() ? ids.Max() : 0;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool VerificarExistenciaObjeto(BEUsuario obj)
        {
            try
            {
                EnsureBD();
                return RootUsuarios().Elements("usuario")
                    .Any(x => ((string)x.Element("Usuario")).Trim()
                    .Equals(obj.Usuario.Trim(), StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex) { throw ex; }
        }

        public bool Guardar(BEUsuario oBEUsuario)
        {
            try
            {
                EnsureBD();
                if (oBEUsuario == null) throw new Exception("Error: No se puedo obtener los datos del Usuario!");

                if (oBEUsuario.Id == 0)
                {
                    if (VerificarExistenciaObjeto(oBEUsuario))
                        throw new Exception("Error: No se puede dar el alta a un Usuario que ya existe!");

                    long nuevoId = ObtenerUltimoId() + 1;
                    oBEUsuario.Id = nuevoId;

                    string passwordEncriptado = Encriptacion.EncriptarPassword(oBEUsuario.Password ?? string.Empty);

                    RootUsuarios().Add(
                        new XElement("usuario",
                            new XAttribute("Id", oBEUsuario.Id.ToString().Trim()),
                            new XElement("Usuario", oBEUsuario.Usuario.Trim()),
                            new XElement("Password", passwordEncriptado),
                            new XElement("Activo", oBEUsuario.Activo.ToString().Trim()),
                            new XElement("Bloqueado", oBEUsuario.Bloqueado.ToString().Trim()),
                            new XElement("DebeCambiarPassword", oBEUsuario.DebeCambiarPassword),
                            new XElement("Nombre", oBEUsuario.Nombre ?? string.Empty),
                            new XElement("Apellido", oBEUsuario.Apellido ?? string.Empty)
                        ));

                    BDXML.Save(ruta);

                    // >>> AQUÍ USAMOS EL MÉTODO QUE FALTABA <<<
                    SincronizarRelaciones(oBEUsuario);
                    BDXML.Save(ruta);
                    return true;
                }
                else
                {
                    var buscarUsuario =
                        from usuario in RootUsuarios().Descendants("usuario")
                        where usuario.Attribute("Id").Value.Trim() == oBEUsuario.Id.ToString().Trim()
                        select usuario;

                    if (!buscarUsuario.Any())
                        throw new Exception("Error: No se pudo recuperar los datos del Usuario para actualizar!");

                    string passwordEncriptado = Encriptacion.EncriptarPassword(oBEUsuario.Password ?? string.Empty);

                    foreach (XElement usuarioModificado in buscarUsuario)
                    {
                        usuarioModificado.Element("Usuario").Value = oBEUsuario.Usuario.Trim();
                        usuarioModificado.Element("Password").Value = passwordEncriptado;
                        usuarioModificado.Element("Activo").Value = oBEUsuario.Activo.ToString().Trim();
                        usuarioModificado.Element("Bloqueado").Value = oBEUsuario.Bloqueado.ToString().Trim();

                        var nodoDebe = usuarioModificado.Element("DebeCambiarPassword");
                        if (nodoDebe == null) usuarioModificado.Add(new XElement("DebeCambiarPassword", oBEUsuario.DebeCambiarPassword));
                        else nodoDebe.Value = oBEUsuario.DebeCambiarPassword.ToString();

                        var nodoNombre = usuarioModificado.Element("Nombre");
                        if (nodoNombre == null) usuarioModificado.Add(new XElement("Nombre", oBEUsuario.Nombre ?? string.Empty));
                        else nodoNombre.Value = oBEUsuario.Nombre ?? string.Empty;

                        var nodoApellido = usuarioModificado.Element("Apellido");
                        if (nodoApellido == null) usuarioModificado.Add(new XElement("Apellido", oBEUsuario.Apellido ?? string.Empty));
                        else nodoApellido.Value = oBEUsuario.Apellido ?? string.Empty;
                    }

                    BDXML.Save(ruta);

                    // >>> AQUÍ TAMBIÉN <<<
                    SincronizarRelaciones(oBEUsuario);
                    BDXML.Save(ruta);
                    return true;
                }
            }
            catch (CryptographicException ex) { throw ex; }
            catch (XmlException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
        }

        public bool Eliminar(BEUsuario obj)
        {
            try
            {
                EnsureBD();

                RootUsuarios().Elements("usuario")
                    .Where(x => (long)x.Attribute("Id") == obj.Id)
                    .Remove();

                RootUsuarioRoles().Elements("usuario_rol")
                    .Where(x => (long)x.Element("Id_Usuario") == obj.Id)
                    .Remove();

                RootUsuarioPermisos().Elements("usuario_permiso")
                    .Where(x => (long)x.Element("Id_Usuario") == obj.Id)
                    .Remove();

                BDXML.Save(ruta);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public List<BEUsuario> ListarTodo()
        {
            try
            {
                EnsureBD();
                var lista = new List<BEUsuario>();
                foreach (var x in RootUsuarios().Elements("usuario"))
                {
                    var u = new BEUsuario
                    {
                        Id = (long)x.Attribute("Id"),
                        Usuario = (string)x.Element("Usuario"),
                        Password = (string)x.Element("Password"),
                        Activo = (bool)x.Element("Activo"),
                        Bloqueado = (bool)x.Element("Bloqueado"),
                        DebeCambiarPassword = (bool?)x.Element("DebeCambiarPassword") ?? false,
                        Nombre = (string)x.Element("Nombre") ?? "",
                        Apellido = (string)x.Element("Apellido") ?? ""
                    };
                    CargarRelaciones(u);
                    lista.Add(u);
                }
                return lista;
            }
            catch (Exception ex) { throw ex; }
        }

        public BEUsuario ListarObjeto(BEUsuario obj)
        {
            try
            {
                EnsureBD();
                var x = RootUsuarios().Elements("usuario")
                    .FirstOrDefault(n => (long)n.Attribute("Id") == obj.Id);
                if (x == null) return null;

                var u = new BEUsuario
                {
                    Id = (long)x.Attribute("Id"),
                    Usuario = (string)x.Element("Usuario"),
                    Password = (string)x.Element("Password"),
                    Activo = (bool)x.Element("Activo"),
                    Bloqueado = (bool)x.Element("Bloqueado"),
                    DebeCambiarPassword = (bool?)x.Element("DebeCambiarPassword") ?? false,
                    Nombre = (string)x.Element("Nombre") ?? "",
                    Apellido = (string)x.Element("Apellido") ?? ""
                };
                CargarRelaciones(u);
                return u;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool MarcarDebeCambiarPassword(long idUsuario, bool valor)
        {
            try
            {
                EnsureBD();
                var nodo = RootUsuarios().Elements("usuario")
                            .FirstOrDefault(u => (long)u.Attribute("Id") == idUsuario);
                if (nodo == null) return false;

                var dcp = nodo.Element("DebeCambiarPassword");
                if (dcp == null) nodo.Add(new XElement("DebeCambiarPassword", valor));
                else dcp.Value = valor.ToString();

                BDXML.Save(ruta);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        #region Relaciones y jerarquía
        // Carga roles y permisos DIRECTOS
        private void CargarRelaciones(BEUsuario u)
        {
            var rolesUsr = (RootUsuarioRoles()?.Elements("usuario_rol") ?? Enumerable.Empty<XElement>())
                .Where(n => (long)n.Element("Id_Usuario") == u.Id)
                .Select(n => (long)n.Element("Id_Rol"));

            foreach (var idRol in rolesUsr)
            {
                var rol = (RootRoles()?.Elements("rol") ?? Enumerable.Empty<XElement>())
                          .FirstOrDefault(r => (long)r.Attribute("Id") == idRol);
                if (rol != null)
                    u.listaRoles.Add(new BERol((long)rol.Attribute("Id"), (string)rol.Element("Nombre")));
            }

            var perUsr = (RootUsuarioPermisos()?.Elements("usuario_permiso") ?? Enumerable.Empty<XElement>())
                .Where(n => (long)n.Element("Id_Usuario") == u.Id)
                .Select(n => (long)n.Element("Id_Permiso"));

            foreach (var idPer in perUsr)
            {
                var per = (RootPermisos()?.Elements("permiso") ?? Enumerable.Empty<XElement>())
                          .FirstOrDefault(p => (long)p.Attribute("Id") == idPer);
                if (per != null)
                    u.listaPermisos.Add(new BEPermiso((long)per.Attribute("Id"), (string)per.Element("Nombre")));
            }
        }

        // >>> MÉTODO QUE FALTABA <<<
        private void SincronizarRelaciones(BEUsuario u)
        {
            // Limpio lo anterior
            RootUsuarioRoles().Elements("usuario_rol")
                .Where(x => (long)x.Element("Id_Usuario") == u.Id)
                .Remove();
            RootUsuarioPermisos().Elements("usuario_permiso")
                .Where(x => (long)x.Element("Id_Usuario") == u.Id)
                .Remove();

            // Roles
            foreach (var rol in u.listaRoles)
            {
                var r = RootRoles().Elements("rol").FirstOrDefault(x => (long)x.Attribute("Id") == rol.Id);
                if (r == null)
                {
                    RootRoles().Add(new XElement("rol",
                        new XAttribute("Id", rol.Id),
                        new XElement("Nombre", rol.Nombre)));
                }

                RootUsuarioRoles().Add(new XElement("usuario_rol",
                    new XElement("Id_Usuario", u.Id),
                    new XElement("Id_Rol", rol.Id)));
            }

            // Permisos
            foreach (var per in u.listaPermisos)
            {
                var p = RootPermisos().Elements("permiso").FirstOrDefault(x => (long)x.Attribute("Id") == per.Id);
                if (p == null)
                {
                    RootPermisos().Add(new XElement("permiso",
                        new XAttribute("Id", per.Id),
                        new XElement("Nombre", per.Nombre)));
                }

                RootUsuarioPermisos().Add(new XElement("usuario_permiso",
                    new XElement("Id_Usuario", u.Id),
                    new XElement("Id_Permiso", per.Id)));
            }
        }

        public List<BEPermiso> ListarTodosLosPermisosDelUsuario(BEUsuario u)
        {
            EnsureBD();
            var lista = new List<BEPermiso>();

            var up = from x in RootUsuarioPermisos().Elements("usuario_permiso")
                     where (string)x.Element("Id_Usuario") == u.Id.ToString()
                     select x;

            foreach (var nodo in up)
            {
                var idPerm = (string)nodo.Element("Id_Permiso");
                var p = (from x in RootPermisos().Elements("permiso")
                         where (string)x.Attribute("Id") == idPerm
                         select new BEPermiso(long.Parse(idPerm), (string)x.Element("Nombre")))
                         .FirstOrDefault();
                if (p != null) lista.Add(p);
            }
            return lista;
        }

        public BEUsuario ListarObjetoJerarquico(BEUsuario u)
        {
            EnsureBD();
            var baseUsr = ListarObjeto(u);
            if (baseUsr == null) return null;

            // Roles directos
            var idsRoles = RootUsuarioRoles().Elements("usuario_rol")
                .Where(x => (long)x.Element("Id_Usuario") == baseUsr.Id)
                .Select(x => (long)x.Element("Id_Rol")).ToList();

            baseUsr.listaRoles.Clear();
            foreach (var idRol in idsRoles)
            {
                var rol = new BE.BEComposite.BERol(idRol, ObtenerNombreRol(idRol));
                // completo jerarquía
                CargarRolJerarquico(rol);
                baseUsr.listaRoles.Add(rol);
            }

            // Permisos directos
            baseUsr.listaPermisos.Clear();
            var idsPerm = RootUsuarioPermisos().Elements("usuario_permiso")
                .Where(x => (long)x.Element("Id_Usuario") == baseUsr.Id)
                .Select(x => (long)x.Element("Id_Permiso")).ToList();

            foreach (var idp in idsPerm)
                baseUsr.listaPermisos.Add(new BE.BEComposite.BEPermiso(idp, ObtenerNombrePermiso(idp)));

            return baseUsr;
        }

        private string ObtenerNombreRol(long idRol)
        {
            EnsureBD();
            var r = RootRoles().Elements("rol").FirstOrDefault(x => (long)x.Attribute("Id") == idRol);
            return r != null ? r.Element("Nombre").Value : "";
        }

        private string ObtenerNombrePermiso(long idPer)
        {
            EnsureBD();
            var p = RootPermisos().Elements("permiso").FirstOrDefault(x => (long)x.Attribute("Id") == idPer);
            return p != null ? p.Element("Nombre").Value : "";
        }

        private void CargarRolJerarquico(BE.BEComposite.BERol rolPadre)
        {
            var hijosRol = (BDXML.Root.Element("Rol_Rol")?.Elements("rol_rol") ?? Enumerable.Empty<XElement>())
                .Where(x => (long)x.Element("Id_Padre") == rolPadre.Id)
                .Select(x => (long)x.Element("Id_Hijo"))
                .ToList();

            foreach (var idHijo in hijosRol)
            {
                var rolHijo = new BE.BEComposite.BERol(idHijo, ObtenerNombreRol(idHijo));
                rolPadre.Agregar(rolHijo);
                CargarRolJerarquico(rolHijo); // recursivo
            }

            var perms = (BDXML.Root.Element("Rol_Permisos")?.Elements("rol_permiso") ?? Enumerable.Empty<XElement>())
                .Where(x => (long)x.Element("Id_Rol") == rolPadre.Id)
                .Select(x => (long)x.Element("Id_Permiso"))
                .ToList();

            foreach (var idp in perms)
            {
                rolPadre.Agregar(new BE.BEComposite.BEPermiso(idp, ObtenerNombrePermiso(idp)));
            }
        }

        // ===== Asociaciones Usuario <-> Rol / Permiso =====
        public bool AsociarUsuarioARol(BEUsuario u, BERol rol)
        {
            try
            {
                EnsureBD();
                // asegurar rol en catálogo
                var r = RootRoles().Elements("rol").FirstOrDefault(x => (long)x.Attribute("Id") == rol.Id);
                if (r == null)
                {
                    RootRoles().Add(new XElement("rol",
                        new XAttribute("Id", rol.Id),
                        new XElement("Nombre", rol.Nombre)));
                }
                // evitar duplicado
                var existe = RootUsuarioRoles().Elements("usuario_rol")
                    .Any(x => (long)x.Element("Id_Usuario") == u.Id && (long)x.Element("Id_Rol") == rol.Id);
                if (!existe)
                {
                    RootUsuarioRoles().Add(new XElement("usuario_rol",
                        new XElement("Id_Usuario", u.Id),
                        new XElement("Id_Rol", rol.Id)));
                }
                BDXML.Save(ruta);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }
        // MPPUsuario.cs  (dentro de namespace MPP, **dentro** de la clase MPPUsuario)
        public bool AgregarRolSiNoExiste(long idUsuario, long idRol)
        {
            try
            {
                EnsureBD();
                var usr = RootUsuarios().Elements("usuario")
                           .FirstOrDefault(x => (long)x.Attribute("Id") == idUsuario);
                if (usr == null) throw new Exception("Usuario no encontrado.");

                var rol = RootRoles().Elements("rol")
                           .FirstOrDefault(x => (long)x.Attribute("Id") == idRol);
                if (rol == null) throw new Exception("Rol no encontrado.");

                var existe = RootUsuarioRoles().Elements("usuario_rol")
                                .Any(x => (long)x.Element("Id_Usuario") == idUsuario &&
                                          (long)x.Element("Id_Rol") == idRol);
                if (!existe)
                {
                    RootUsuarioRoles().Add(new XElement("usuario_rol",
                        new XElement("Id_Usuario", idUsuario),
                        new XElement("Id_Rol", idRol)));
                    BDXML.Save(ruta);
                }
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool DesasociarUsuarioARol(BEUsuario u, BERol rol)
        {
            try
            {
                EnsureBD();
                RootUsuarioRoles().Elements("usuario_rol")
                    .Where(x => (long)x.Element("Id_Usuario") == u.Id && (long)x.Element("Id_Rol") == rol.Id)
                    .Remove();
                BDXML.Save(ruta);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool AsociarPermisoAUsuario(BEUsuario u, BEPermiso p)
        {
            try
            {
                EnsureBD();
                // asegurar permiso en catálogo
                var per = RootPermisos().Elements("permiso").FirstOrDefault(x => (long)x.Attribute("Id") == p.Id);
                if (per == null)
                {
                    RootPermisos().Add(new XElement("permiso",
                        new XAttribute("Id", p.Id),
                        new XElement("Nombre", p.Nombre)));
                }
                var existe = RootUsuarioPermisos().Elements("usuario_permiso")
                    .Any(x => (long)x.Element("Id_Usuario") == u.Id && (long)x.Element("Id_Permiso") == p.Id);
                if (!existe)
                {
                    RootUsuarioPermisos().Add(new XElement("usuario_permiso",
                        new XElement("Id_Usuario", u.Id),
                        new XElement("Id_Permiso", p.Id)));
                }
                BDXML.Save(ruta);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool DesasociarPermisoAUsuario(BEUsuario u, BEPermiso p)
        {
            try
            {
                EnsureBD();
                RootUsuarioPermisos().Elements("usuario_permiso")
                    .Where(x => (long)x.Element("Id_Usuario") == u.Id && (long)x.Element("Id_Permiso") == p.Id)
                    .Remove();
                BDXML.Save(ruta);
                return true;
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion

        // ==================== Actualizar password ====================
        public bool ActualizarPassword(long idUsuario, string passwordEncriptada, bool debeCambiarPassword)
        {
            EnsureBD();
            var nodo = RootUsuarios().Elements("usuario")
                        .FirstOrDefault(u => (long)u.Attribute("Id") == idUsuario);
            if (nodo == null) return false;

            var nodoPass = nodo.Element("Password");
            if (nodoPass == null) nodo.Add(new XElement("Password", passwordEncriptada ?? ""));
            else nodoPass.Value = passwordEncriptada ?? "";

            var dcp = nodo.Element("DebeCambiarPassword");
            if (dcp == null) nodo.Add(new XElement("DebeCambiarPassword", debeCambiarPassword));
            else dcp.Value = debeCambiarPassword.ToString();

            BDXML.Save(ruta);
            return true;
        }
    }
}
