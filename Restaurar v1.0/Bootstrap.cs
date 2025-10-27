using BE;
using BE.BEComposite;
using BLL_Negocio;
using MPP;
using Seguridad;
using System;
using System.Linq;

namespace Restaurar_v1._0
{
    /// Siembra mínima de datos (idempotente)
    internal static class Bootstrap
    {
        public static void SembrarAdmin()
        {
            var mppU = new MPPUsuario();
            var mppR = new MPPRol();
            var mppP = new MPPPermiso();

            // 1) Aseguro archivo y nodos
            mppU.CrearXML(); // crea BD.xml si no existe y asegura <Root> con secciones

            // 2) Usuario admin (si no existe lo creo)
            var admin = mppU.ListarTodo()
                .FirstOrDefault(u => u.Usuario.Equals("admin", StringComparison.OrdinalIgnoreCase));

            if (admin == null)
            {
                admin = new BEUsuario
                {
                    Id = 0,
                    Usuario = "admin",
                    Password = "admin",              // MPPUsuario.Guardar la encripta (Base64)
                    Activo = true,
                    Bloqueado = false,
                    DebeCambiarPassword = true       // para forzar cambio en el primer login
                };
                mppU.Guardar(admin);                 // asigna Id nuevo y persiste
                admin = mppU.ListarTodo().First(u => u.Usuario.Equals("admin", StringComparison.OrdinalIgnoreCase));
            }

            // 3) Rol ADMIN (si no existe lo creo)
            var rolAdmin = mppR.ListarTodo()
                .FirstOrDefault(r => r.Nombre.Equals("ADMIN", StringComparison.OrdinalIgnoreCase));
            if (rolAdmin == null)
            {
                rolAdmin = new BERol(0, "ADMIN");    // MPPRol.Guardar asigna Id
                mppR.Guardar(rolAdmin);
                rolAdmin = mppR.ListarTodo().First(r => r.Nombre.Equals("ADMIN", StringComparison.OrdinalIgnoreCase));
            }

            // 4) Permiso ADMIN_TODO (si no existe lo creo)
            var adminTodo = mppP.ListarTodo()
                .FirstOrDefault(p => p.Nombre.Equals("ADMIN_TODO", StringComparison.OrdinalIgnoreCase));
            if (adminTodo == null)
            {
                adminTodo = new BEPermiso(0, "ADMIN_TODO"); // MPPPermiso.Guardar asigna Id
                mppP.Guardar(adminTodo);
                adminTodo = mppP.ListarTodo().First(p => p.Nombre.Equals("ADMIN_TODO", StringComparison.OrdinalIgnoreCase));
            }

            // 5) Asociaciones (idempotentes)
            mppU.AsociarUsuarioARol(admin, rolAdmin);       // evita duplicados internamente
            mppU.AsociarPermisoAUsuario(admin, adminTodo);  // idem

            // (Opcional) Si querés que ADMIN tenga literalmente todos los permisos existentes:
            foreach (var p in mppP.ListarTodo())
                mppU.AsociarPermisoAUsuario(admin, p);
        }

    }
}
