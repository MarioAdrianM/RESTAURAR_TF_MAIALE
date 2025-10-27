using System;
using System.Collections.Generic;
using System.Linq;
using BE;
using BE.BEComposite;
using MPP;
using Seguridad;

namespace BLL_Negocio
{
    public class BLLUsuario
    {
        private readonly MPPUsuario _mpp = new MPPUsuario();
        const string ADMIN_USERNAME = "admin";

        // ===================== CRUD =====================
        public bool Guardar(BEUsuario u)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            if (string.IsNullOrWhiteSpace(u.Usuario)) throw new Exception("El nombre de usuario es obligatorio.");
            if (u.Usuario.Equals(ADMIN_USERNAME, StringComparison.OrdinalIgnoreCase))
            {
                // si llega acá, sólo permito cambiar password (si tu flujo lo exige) — opcional:
                // cualquier otro cambio (Activo, Bloqueado, Roles, Permisos) lo bloqueo:
                throw new InvalidOperationException("El usuario 'admin' no puede ser modificado.");
            }
            // Si viene Password en texto plano, MPP ya la encripta internamente.
            return _mpp.Guardar(u);
        }

        public bool Eliminar(BEUsuario u)
        {
            if (u == null || u.Id <= 0) throw new Exception("Usuario inválido.");
            if (u.Usuario.Equals(ADMIN_USERNAME, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("El usuario 'admin' no puede ser eliminado.");
            return _mpp.Eliminar(u);
        }

        public List<BEUsuario> ListarTodo() => _mpp.ListarTodo();

        public BEUsuario ListarObjeto(BEUsuario u)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            return _mpp.ListarObjeto(u);
        }

        public BEUsuario ListarObjetoJerarquico(BEUsuario u)
        {
            if (u == null) throw new ArgumentNullException(nameof(u));
            return _mpp.ListarObjetoJerarquico(u);
        }

        // ===================== Autenticación =====================
        public BEUsuario Autenticar(string usuario, string password)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrEmpty(password))
                throw new Exception("Usuario o contraseña inválidos.");

            var todos = _mpp.ListarTodo();
            var u = todos.FirstOrDefault(x => string.Equals(x.Usuario, usuario, StringComparison.OrdinalIgnoreCase));
            if (u == null) throw new Exception("Usuario o contraseña inválidos.");

            string real;
            try { real = Encriptacion.DesencriptarPassword(u.Password ?? ""); }
            catch { real = ""; }

            if (!string.Equals(real, password))
                throw new Exception("Usuario o contraseña inválidos.");

            return u;
        }

        /// <summary>Regla típica: admin/admin → permitir omitir la contraseña actual en el 1er login.</summary>
        public bool DebeOmitirActualEnCambio(BEUsuario u)
        {
            try
            {
                var real = Encriptacion.DesencriptarPassword(u.Password ?? "");
                return string.Equals(u.Usuario, "admin", StringComparison.OrdinalIgnoreCase) && real == "admin";
            }
            catch { return false; }
        }

        // ===================== Asociaciones =====================
        public void AsociarUsuarioARol(BEUsuario u, BERol rol) => _mpp.AsociarUsuarioARol(u, rol);
        public void DesasociarUsuarioARol(BEUsuario u, BERol rol) => _mpp.DesasociarUsuarioARol(u, rol);
        public void AsociarPermisoAUsuario(BEUsuario u, BEPermiso p) => _mpp.AsociarPermisoAUsuario(u, p);
        public void DesasociarPermisoAUsuario(BEUsuario u, BEPermiso p) => _mpp.DesasociarPermisoAUsuario(u, p);

        // ===================== Permisos efectivos =====================
        /// <summary>Alias práctico usado por algunos formularios.</summary>
        public List<BEPermiso> ListarPermisosEfectivos(BEUsuario u) => ListarTodosLosPermisosDelUsuario(u);

        /// <summary>Permisos directos + permisos de toda la jerarquía de roles.</summary>
        public List<BEPermiso> ListarTodosLosPermisosDelUsuario(BEUsuario u)
        {
            var lleno = ListarObjetoJerarquico(u);
            var resultado = new List<BEPermiso>();
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Directos
            if (lleno?.listaPermisos != null)
            {
                foreach (var p in lleno.listaPermisos)
                    if (set.Add(p.Nombre))
                        resultado.Add(new BEPermiso(p.Id, p.Nombre));
            }

            // Vía Roles
            void Recorrer(BEComposite comp)
            {
                var hijos = comp.ObtenerHijos();
                if (hijos == null) return;

                foreach (var h in hijos)
                {
                    if (h is BEPermiso per)
                    {
                        if (set.Add(per.Nombre))
                            resultado.Add(new BEPermiso(per.Id, per.Nombre));
                    }
                    else if (h is BERol rolHijo)
                    {
                        Recorrer(rolHijo);
                    }
                }
            }

            if (lleno?.listaRoles != null)
                foreach (var rol in lleno.listaRoles)
                    Recorrer(rol);

            return resultado;
        }

        // ===================== Password helpers =====================
        public string EncriptarPassword(string textoPlano) => Encriptacion.EncriptarPassword(textoPlano ?? "");
        public string DesencriptarPassword(string cifrada) => Encriptacion.DesencriptarPassword(cifrada ?? "");

        // ===================== Cambiar Password (sobrecargas para TU UI) =====================

        /// <summary>
        /// Firma usada por muchos formularios: (u, actual, nueva, confirmar, out error)
        /// </summary>
        public bool CambiarPassword(BEUsuario u, string actual, string nueva, string confirmar, out string error)
            => CambiarPassword(u, actual, nueva, confirmar, /*omitirActual*/ false, out error);

        /// <summary>
        /// Firma para 1er login (sin pedir actual): (u, nueva, confirmar, out error)
        /// </summary>
        public bool CambiarPassword(BEUsuario u, string nueva, string confirmar, out string error)
            => CambiarPassword(u, /*actual*/ "", nueva, confirmar, /*omitirActual*/ true, out error);

        /// <summary>
        /// Firma general y super-compat: (u, actual, nueva, confirmar, omitirActual, out error)
        /// </summary>
        public bool CambiarPassword(BEUsuario u, string actual, string nueva, string confirmar, bool omitirActual, out string error)
        {
            error = null;
            if (u == null) { error = "Usuario inválido."; return false; }

            if (string.IsNullOrWhiteSpace(nueva)) { error = "La nueva contraseña es obligatoria."; return false; }
            if (!string.Equals(nueva, confirmar)) { error = "La confirmación no coincide."; return false; }

            var stored = ListarObjeto(u);
            if (stored == null) { error = "Usuario no encontrado."; return false; }

            if (!omitirActual)
            {
                string actualReal;
                try { actualReal = DesencriptarPassword(stored.Password ?? ""); } catch { actualReal = ""; }
                if (!string.Equals(actualReal, actual))
                {
                    error = "La contraseña actual no es correcta.";
                    return false;
                }
            }

            var nuevaEnc = EncriptarPassword(nueva);
            // Al cambiarla manualmente, ya no debe forzar cambio
            return _mpp.ActualizarPassword(stored.Id, nuevaEnc, false);
        }

        /// <summary>
        /// Reset administrativo: por Id.
        /// </summary>
        public bool CambiarPassword(long idUsuario, string nueva, bool marcarDebeCambiar)
        {
            if (string.IsNullOrWhiteSpace(nueva)) throw new Exception("La nueva contraseña es obligatoria.");
            var nuevaEnc = EncriptarPassword(nueva);
            return _mpp.ActualizarPassword(idUsuario, nuevaEnc, marcarDebeCambiar);
        }
    }
}
