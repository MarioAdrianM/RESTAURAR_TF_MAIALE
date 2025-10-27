using BE;
using BE.BEComposite;
using BLL_Negocio;
using BLL_Negocio.BLLComposite;
using Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Restaurar_v1._0
{
    public partial class FormMenu : Form
    {
        private BEUsuario _usuario;
        private readonly BLLUsuario _bllUsuario = new BLLUsuario();
        private readonly BLLPermiso _bllPermiso = new BLLPermiso();

        public bool LogoutRequested { get; private set; } = false;

        public FormMenu()
        {
            InitializeComponent();
            this.MainMenuStrip = this.menuPrincipal;
            this.Load += FormMenu_Load;
        }

        public FormMenu(BEUsuario usuarioLogueado) : this()
        {
            _usuario = usuarioLogueado ?? throw new ArgumentNullException(nameof(usuarioLogueado));
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            // 1) Si la BD está nueva (o falta algún permiso), la completo desde el menú (texto exacto)
            SembrarPermisosDesdeMenuSinNormalizar();

            // 2) Oculto todo
            OcultarTodosLosItemsDelMenu();

            // 3) Tomo usuario (soporta tu FormLogin actual)
            var usr = _usuario ?? Sesion.UsuarioActual;
            if (usr == null)
            {
                MostrarSoloPublicos();
                return;
            }

            // 4) Permisos efectivos del usuario (directos + roles)
            var permisos = _bllUsuario.ListarPermisosEfectivos(usr) ?? new List<BEPermiso>();

            // Si tiene ADMIN_TODO -> ve todo (sin normalizar ni nada)
            if (permisos.Any(p => string.Equals(p.Nombre, "ADMIN_TODO", StringComparison.OrdinalIgnoreCase)))
            {
                MostrarTodosLosItemsDelMenu();
                return;
            }

            // 5) Set de permisos por NOMBRE EXACTO (camino)
            var permSet = new HashSet<string>(permisos.Select(p => p.Nombre), StringComparer.OrdinalIgnoreCase);

            // 6) Aplico visibilidad por camino exacto (padres visibles si algún hijo lo es)
            foreach (ToolStripItem root in menuPrincipal.Items)
                if (root is ToolStripMenuItem top)
                    _ = AplicarVisibilidadRec(top, permSet);
            AdminSync.EnsureAdminCompleto();
        }

        // ====== Sembrado desde el menú (sin normalizar, usa camino exacto) ======
        private void SembrarPermisosDesdeMenuSinNormalizar()
        {
            if (menuPrincipal == null) return;

            var existentes = new HashSet<string>(
                _bllPermiso.ListarTodo().Select(p => p.Nombre),
                StringComparer.OrdinalIgnoreCase
            );

            var recolectados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (ToolStripItem it in menuPrincipal.Items)
                if (it is ToolStripMenuItem top)
                    RecolectarCaminos(top, recolectados);

            // Aseguro ADMIN_TODO
            if (!existentes.Contains("ADMIN_TODO")) _bllPermiso.Guardar(new BEPermiso(0, "ADMIN_TODO"));

            // Creo faltantes (todos los ítems, públicos incluidos; que existan en catálogo no molesta)
            foreach (var nombre in recolectados)
                if (!existentes.Contains(nombre))
                    _bllPermiso.Guardar(new BEPermiso(0, nombre));
        }

        private static string Camino(ToolStripMenuItem it)
        {
            var pila = new Stack<string>();
            ToolStripItem cur = it;
            while (cur != null)
            {
                if (cur is ToolStripMenuItem mi)
                    pila.Push(mi.Text.Trim());
                cur = (cur as ToolStripMenuItem)?.OwnerItem;
            }
            return string.Join("/", pila); // Texto exacto, con acentos/espacios
        }

        private void RecolectarCaminos(ToolStripMenuItem mi, HashSet<string> acc)
        {
            acc.Add(Camino(mi));
            foreach (ToolStripItem sub in mi.DropDownItems)
                if (sub is ToolStripMenuItem sm)
                    RecolectarCaminos(sm, acc);
        }

        // ===== Público por texto exacto =====
        private static bool EsPublico(ToolStripMenuItem mi)
        {
            var t = (mi.Text ?? "").Trim();
            return t.Equals("Salir", StringComparison.OrdinalIgnoreCase)
                || t.Equals("Cerrar Sesión", StringComparison.OrdinalIgnoreCase)
                || t.Equals("Cerrar Sesion", StringComparison.OrdinalIgnoreCase)
                || t.Equals("Cambiar Contraseña", StringComparison.OrdinalIgnoreCase)
                || t.Equals("Cambiar Contrasena", StringComparison.OrdinalIgnoreCase);
        }

        // ===== Visibilidad =====
        private bool AplicarVisibilidadRec(ToolStripMenuItem mi, HashSet<string> permSet)
        {
            bool visible = false;

            // Hijos primero
            foreach (ToolStripItem sub in mi.DropDownItems)
                if (sub is ToolStripMenuItem sm)
                    visible = AplicarVisibilidadRec(sm, permSet) || visible;

            // Propio ítem
            if (!visible)
            {
                if (EsPublico(mi))
                    visible = true;
                else
                {
                    var key = Camino(mi); // camino exacto
                    visible = permSet.Contains(key) || permSet.Contains(mi.Text.Trim());
                    // soporta dos estilos: por camino o por texto simple (si te queda algún permiso viejo)
                }
            }

            mi.Visible = visible;
            return visible;
        }

        private void MostrarSoloPublicos()
        {
            foreach (ToolStripItem it in menuPrincipal.Items)
                if (it is ToolStripMenuItem mi)
                    _ = MostrarSoloPublicosRec(mi);
        }

        private bool MostrarSoloPublicosRec(ToolStripMenuItem mi)
        {
            bool visible = EsPublico(mi);
            foreach (ToolStripItem sub in mi.DropDownItems)
                if (sub is ToolStripMenuItem sm)
                    visible = MostrarSoloPublicosRec(sm) || visible;
            mi.Visible = visible;
            return visible;
        }

        private void OcultarTodosLosItemsDelMenu()
        {
            if (menuPrincipal == null) return;
            foreach (ToolStripItem item in menuPrincipal.Items)
                if (item is ToolStripMenuItem mi)
                {
                    mi.Visible = false;
                    OcultarSub(mi.DropDownItems);
                }
        }
        private void OcultarSub(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
                if (item is ToolStripMenuItem sm)
                {
                    sm.Visible = false;
                    OcultarSub(sm.DropDownItems);
                }
        }
        private void MostrarTodosLosItemsDelMenu()
        {
            if (menuPrincipal == null) return;
            foreach (ToolStripItem item in menuPrincipal.Items)
                if (item is ToolStripMenuItem mi)
                {
                    mi.Visible = true;
                    MostrarSub(mi.DropDownItems);
                }
        }
        private void MostrarSub(ToolStripItemCollection items)
        {
            foreach (ToolStripItem item in items)
                if (item is ToolStripMenuItem sm)
                {
                    sm.Visible = true;
                    MostrarSub(sm.DropDownItems);
                }
        }

        // ===== Handlers existentes =====
        private void mnuRolesPermisos_Click(object sender, EventArgs e)
        {
            var frm = new FormRolesPermisos(this.menuPrincipal) { MdiParent = this };
            frm.Show();
        }
        private void mnuSalir_Click(object sender, EventArgs e)
        {
            LogoutRequested = false; this.Close();
        }
        private void cerrarSesionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogoutRequested = true; this.Close();
        }
        private void bitacoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new FormBitacora()) f.ShowDialog(this);
        }
        private void aBMDeMesasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FrmMesasABM { MdiParent = this, StartPosition = FormStartPosition.CenterScreen };
            f.Show();
        }
        private void reservasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FrmReservas { MdiParent = this, StartPosition = FormStartPosition.CenterScreen };
            f.Show();
        }
        private void aBMDeSectoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FrmSectoresABM { MdiParent = this, StartPosition = FormStartPosition.CenterScreen };
            f.Show();
        }
        private void cambiarContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Sesion.UsuarioActual == null) { MessageBox.Show("No hay usuario en sesión."); return; }
            using (var f = new frmCambiarPassword(Sesion.UsuarioActual, false))
                if (f.ShowDialog(this) == DialogResult.OK)
                    MessageBox.Show("Contraseña actualizada.");
        }
        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new FormUsuarios { MdiParent = this, StartPosition = FormStartPosition.CenterScreen };
            f.Show();
        }
        private void facturaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new FrmFactura()) f.ShowDialog(this);
        }
        private void cobranzaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new FrmCobroMesa()) f.ShowDialog(this);
        }
        private void aperturaDeCajaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new FrmAperturaCaja()) f.ShowDialog(this);
        }
        private void rendiciónDeValoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var f = new FrmRendicionValores()) f.ShowDialog(this);
        }
    }
}
