using System;
using System.Linq;
using System.Windows.Forms;
using BE;
using BE.BEComposite;

namespace Restaurar_v1._0
{
    public static class SeguridadUI
    {
        public static void AplicarPermisosEnFormulario(Form form)
        {
            var usr = Seguridad.Sesion.UsuarioActual;

            // Menu principal
            if (form.MainMenuStrip != null)
                AplicarPermisosEnMenuItems(form.MainMenuStrip.Items, usr);

            // Otros controles con Tag
            AplicarPermisosEnControles(form.Controls, usr);
        }

        private static void AplicarPermisosEnControles(Control.ControlCollection controls, BEUsuario usr)
        {
            foreach (Control c in controls)
            {
                string tag = (c.Tag as string ?? "").Trim();

                if (!string.IsNullOrEmpty(tag))
                {
                    if (tag.Equals("PUBLICO", StringComparison.OrdinalIgnoreCase))
                        c.Visible = true;
                    else
                        c.Visible = TienePermiso(usr, tag);
                }
                // recursivo
                if (c.HasChildren)
                    AplicarPermisosEnControles(c.Controls, usr);
            }
        }

        private static bool AplicarPermisosEnMenuItems(ToolStripItemCollection items, BEUsuario usr)
        {
            bool algunVisible = false;

            foreach (ToolStripItem it in items)
            {
                if (it is ToolStripMenuItem mi)
                {
                    // Primero resuelvo los hijos
                    bool hijosVisibles = false;
                    if (mi.DropDownItems != null && mi.DropDownItems.Count > 0)
                        hijosVisibles = AplicarPermisosEnMenuItems(mi.DropDownItems, usr);

                    string tag = (mi.Tag as string ?? "").Trim();

                    bool visible;
                    if (tag.Equals("PUBLICO", StringComparison.OrdinalIgnoreCase))
                        visible = true;                         // siempre visible
                    else if (string.IsNullOrEmpty(tag))
                        visible = hijosVisibles;               // sin tag => visible si algún hijo visible
                    else
                        visible = TienePermiso(usr, tag) || hijosVisibles; // permiso propio o algún hijo

                    mi.Visible = visible;
                    if (visible) algunVisible = true;
                }
            }
            return algunVisible;
        }

        // ====== Permisos efectivos: directos + heredados por roles (jerárquico) ======
        private static bool TienePermiso(BEUsuario usr, string code)
        {
            if (usr != null && usr.Usuario.Equals("admin", StringComparison.OrdinalIgnoreCase))
                return true;   // admin ve todo siempre

            if (usr == null || string.IsNullOrWhiteSpace(code)) return false;

            // directos por nombre
            if (usr.listaPermisos != null &&
                usr.listaPermisos.Any(p => p?.Nombre?.Equals(code, StringComparison.OrdinalIgnoreCase) == true))
                return true;

            // directos por id
            if (long.TryParse(code, out var id) &&
                usr.listaPermisos != null &&
                usr.listaPermisos.Any(p => p?.Id == id))
                return true;

            // por roles (recursivo)
            if (usr.listaRoles != null && usr.listaRoles.Any())
                return usr.listaRoles.Any(r => RolTienePermiso(r, code, id));

            return false;
        }

        private static bool RolTienePermiso(BEComposite comp, string code, long id)
        {
            if (comp == null) return false;
            var hijos = comp.ObtenerHijos();
            if (hijos == null) return false;

            foreach (var h in hijos)
            {
                if (h is BEPermiso p)
                {
                    if (!string.IsNullOrWhiteSpace(code) &&
                        p.Nombre?.Equals(code, StringComparison.OrdinalIgnoreCase) == true)
                        return true;

                    if (id != 0 && p.Id == id)
                        return true;
                }
                else if (h is BERol r)
                {
                    if (RolTienePermiso(r, code, id)) return true;
                }
            }
            return false;
        }
    }
}
