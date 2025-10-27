using System;
using System.Linq;
using System.Collections.Generic;
using BE;
using BE.BEComposite;
using MPP;

namespace Seguridad
{
    public static class AdminSync
    {
        private const string ADMIN_USERNAME = "admin";
        private const string ADMIN_ROLE_NAME = "ADMIN";

        public static void EnsureAdminCompleto()
        {
            var mppUsuario = new MPPUsuario();
            var mppRol = new MPPRol();
            var mppPerm = new MPPPermiso();

            // 1) Rol ADMIN
            var rolAdmin = mppRol.ListarTodo()
                .FirstOrDefault(r => r.Nombre.Equals("ADMIN", StringComparison.OrdinalIgnoreCase));
            if (rolAdmin == null)
            {
                rolAdmin = new BERol(0, "ADMIN");   // <-- tu ctor (id, nombre)
                mppRol.Guardar(rolAdmin);
                rolAdmin = mppRol.ListarTodo().First(r => r.Nombre.Equals("ADMIN", StringComparison.OrdinalIgnoreCase));
            }

            // 2) Usuario admin
            var admin = mppUsuario.ListarTodo()
                .FirstOrDefault(u => u.Usuario.Equals("admin", StringComparison.OrdinalIgnoreCase));
            if (admin == null)
            {
                admin = new BEUsuario
                {
                    Id = 0,
                    Usuario = "admin",
                    Password = "admin",   // MPPUsuario.Guardar la encripta
                    Activo = true,
                    Bloqueado = false
                };
                mppUsuario.Guardar(admin);
                admin = mppUsuario.ListarTodo().First(u => u.Usuario.Equals("admin", StringComparison.OrdinalIgnoreCase));
            }

            // 3) Asociar admin ↔ ADMIN (si faltara)
            mppUsuario.AgregarRolSiNoExiste(admin.Id, rolAdmin.Id);

            // 4) Dar TODOS los permisos al rol ADMIN (incluye los nuevos)
            foreach (var p in mppPerm.ListarTodo())
                mppRol.AgregarPermisoSiNoExiste(rolAdmin.Id, p.Id);
        }

    }
}
