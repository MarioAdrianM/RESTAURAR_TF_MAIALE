using BE;
using BE.BEComposite;
using BLL_Negocio;
using BLL_Negocio.BLLComposite;
using Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Restaurar_v1._0
{
    public partial class FormRolesPermisos : Form
    {
        BEUsuario oBEUsuario;
        BERol oBERol;
        BERol oBERol2;
        BEPermiso oBEPermiso;

        BLLRol oBLLRol;
        BLLPermiso oBLLPermiso;
        BLLUsuario oBLLUsuario;

        Regex nwRegex;
        private readonly MenuStrip _menuStrip;

        public FormRolesPermisos(MenuStrip menuStripPrincipal)
        {
            InitializeComponent();
            oBEUsuario = new BEUsuario();
            oBLLUsuario = new BLLUsuario();
            oBLLRol = new BLLRol();
            oBLLPermiso = new BLLPermiso();
            _menuStrip = menuStripPrincipal;

            cmBxPermisoMenu.SelectedIndexChanged += cmBxPermisoMenu_SelectedIndexChanged;
            cmBxPermisoItem.SelectedIndexChanged += cmBxPermisoItem_SelectedIndexChanged;
        }

        private void FormRolesPermisos_Load(object sender, EventArgs e)
        {
            AdminSync.EnsureAdminCompleto();
            // Asegurá catálogo desde el menú (camino exacto)
            SincronizarPermisosDesdeMenuExacto();
            LimpiarCampos();
        }
        private void AplicarBloqueoUsuario(BEUsuario u)
        {
            bool esAdmin = u != null &&
                           !string.IsNullOrWhiteSpace(u.Usuario) &&
                           u.Usuario.Equals("admin", StringComparison.OrdinalIgnoreCase);

            // Botones que modifican roles/permisos del usuario
            btnRolUsuarioAsignar.Enabled = !esAdmin;
            btnRolUsuarioQuitar.Enabled = !esAdmin;
            btnPermisoUsuarioAsociar.Enabled = !esAdmin;
            btnPermisoUsuarioQuitar.Enabled = !esAdmin;

            // Campos “sensibles”
            txtBxUsuarioNombre.ReadOnly = esAdmin;
            txtBxUsuarioPassword.ReadOnly = esAdmin;
            ckBxUsuarioActivo.Enabled = !esAdmin;
            ckBxUsuarioBloqueado.Enabled = !esAdmin;
            ckBxUsuarioClave.Enabled = !esAdmin;
        }

        // Cancela tildado/destildado en el árbol del usuario si es admin
        private void TreeUsuario_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (oBEUsuario != null &&
                    !string.IsNullOrWhiteSpace(oBEUsuario.Usuario) &&
                    oBEUsuario.Usuario.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    e.Cancel = true;
                }
            }
            catch { /* no-op */ }
        }


        // ===== Camino exacto del ítem =====
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
            return string.Join("/", pila);
        }

        private void SincronizarPermisosDesdeMenuExacto()
        {
            if (_menuStrip == null) return;

            var existentes = new HashSet<string>(
                oBLLPermiso.ListarTodo().Select(p => p.Nombre),
                StringComparer.OrdinalIgnoreCase
            );

            var recolectados = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (ToolStripItem it in _menuStrip.Items)
                if (it is ToolStripMenuItem top)
                    Recolectar(top, recolectados);

            if (!existentes.Contains("ADMIN_TODO"))
                oBLLPermiso.Guardar(new BEPermiso(0, "ADMIN_TODO"));

            foreach (var nombre in recolectados)
                if (!existentes.Contains(nombre))
                    oBLLPermiso.Guardar(new BEPermiso(0, nombre));
        }

        private void Recolectar(ToolStripMenuItem mi, HashSet<string> acc)
        {
            acc.Add(Camino(mi));
            foreach (ToolStripItem sub in mi.DropDownItems)
                if (sub is ToolStripMenuItem sm)
                    Recolectar(sm, acc);
        }

        // =================== ABM Roles ===================
        private void btnRolAlta_Click(object sender, EventArgs e)
        {
            try
            {
                oBERol = ValidarDatosRol(txtBxRolId, txtBxRolNombre);
                oBLLRol.Guardar(oBERol);
                LimpiarCampos();
                MessageBox.Show("Se ha creado correctamente el Rol!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void btnRolModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeVwRoles.Nodes.Count == 0) throw new Exception("No hay roles.");
                oBERol = ValidarDatosRol(txtBxRolId, txtBxRolNombre);
                oBLLRol.Guardar(oBERol);
                LimpiarCampos();
                MessageBox.Show("Se ha modificado correctamente el Rol!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void btnRolEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeVwRoles.Nodes.Count == 0) throw new Exception("No hay roles.");
                oBERol = ValidarDatosRol(txtBxRolId, txtBxRolNombre);
                oBLLRol.Eliminar(oBERol);
                oBERol = null;
                LimpiarCampos();
                MessageBox.Show("Se ha eliminado correctamente el Rol!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        // =================== ABM Permisos ===================
        private void btnPermisoAlta_Click(object sender, EventArgs e)
        {
            try
            {
                oBEPermiso = ValidarDatosPermiso(); // Nombre = camino exacto
                oBLLPermiso.Guardar(oBEPermiso);
                LimpiarCampos();
                MessageBox.Show("Se ha creado correctamente el Permiso!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void btnPermisoEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeVwPermisos.Nodes.Count == 0) throw new Exception("No hay permisos.");
                oBEPermiso = ValidarDatosPermiso();
                oBLLPermiso.Eliminar(oBEPermiso);
                LimpiarCampos();
                MessageBox.Show("Se ha eliminado correctamente el Permiso!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        // =================== Rol ↔ Permiso ===================
        private void btnPermisoRolAsociar_Click(object sender, EventArgs e)
        {
            try
            {
                ChequearRolPermisoSeleccionados();
                oBLLRol.AsociarRolaPermiso(oBERol, oBEPermiso);
                LimpiarCampos();
                MessageBox.Show("Se ha asociado correctamente el Permiso al Rol!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void btnPermisoRolQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                ChequearRolPermisoSeleccionados();
                oBLLRol.DesasociarRolaPermiso(oBERol, oBEPermiso);
                LimpiarCampos();
                MessageBox.Show("Se ha desasociado correctamente el Permiso del Rol!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void ChequearRolPermisoSeleccionados()
        {
            oBERol = ValidarDatosRol(txtBxRolId, txtBxRolNombre);
            oBEPermiso = ValidarDatosPermiso();
            if (oBERol == null) throw new Exception("Seleccione un Rol.");
            if (oBEPermiso == null) throw new Exception("Seleccione un Permiso.");
        }

        // =================== Usuario ↔ Rol ===================
        private void btnRolUsuarioAsignar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBxUsuarioId.Text.Length == 0) throw new Exception("Seleccione un Usuario.");
                oBEUsuario = new BEUsuario { Id = long.Parse(txtBxUsuarioId.Text.Trim()) };
                oBEUsuario = oBLLUsuario.ListarObjeto(oBEUsuario);
                oBERol = ValidarDatosRol(txtBxRolId, txtBxRolNombre);

                oBLLUsuario.AsociarUsuarioARol(oBEUsuario, oBERol);
                LimpiarCampos();
                MessageBox.Show("Se ha asociado correctamente el Rol al Usuario!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void btnRolUsuarioQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBxUsuarioId.Text.Length == 0) throw new Exception("Seleccione un Usuario.");
                oBEUsuario = new BEUsuario { Id = long.Parse(txtBxUsuarioId.Text.Trim()) };
                oBEUsuario = oBLLUsuario.ListarObjeto(oBEUsuario);
                oBERol = ValidarDatosRol(txtBxRolId, txtBxRolNombre);

                oBLLUsuario.DesasociarUsuarioARol(oBEUsuario, oBERol);
                LimpiarCampos();
                MessageBox.Show("Se ha desasociado correctamente el Rol del Usuario!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        // =================== Usuario ↔ Permiso ===================
        private void btnPermisoUsuarioAsociar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBxUsuarioId.Text.Length == 0) throw new Exception("Seleccione un Usuario.");
                oBEUsuario = new BEUsuario { Id = long.Parse(txtBxUsuarioId.Text.Trim()) };
                oBEUsuario = oBLLUsuario.ListarObjeto(oBEUsuario);

                oBEPermiso = ValidarDatosPermiso();
                oBLLUsuario.AsociarPermisoAUsuario(oBEUsuario, oBEPermiso);
                LimpiarCampos();
                MessageBox.Show("Se ha asociado correctamente el Permiso al Usuario!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void btnPermisoUsuarioQuitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBxUsuarioId.Text.Length == 0) throw new Exception("Seleccione un Usuario.");
                oBEUsuario = new BEUsuario { Id = long.Parse(txtBxUsuarioId.Text.Trim()) };
                oBEUsuario = oBLLUsuario.ListarObjeto(oBEUsuario);

                oBEPermiso = ValidarDatosPermiso();
                oBLLUsuario.DesasociarPermisoAUsuario(oBEUsuario, oBEPermiso);
                LimpiarCampos();
                MessageBox.Show("Se ha desasociado correctamente el Permiso del Usuario!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        // =================== Árboles / combos ===================
        private void LimpiarCampos()
        {
            try
            {
                CargarComboBoxMenus();
                CargarComboBoxRoles();

                CargarTreeViewUsuarios();
                CargarTreeViewRoles();
                CargarTreeViewPermisos();
                CargarTreeViewRolPermisos();
                CargarTreeViewUsuarioRolPermisos();

                txtBxUsuarioId.Text = "";
                txtBxUsuarioNombre.Text = "";
                txtBxUsuarioPassword.Text = "";
                ckBxUsuarioActivo.Checked = false;
                ckBxUsuarioBloqueado.Checked = false;
                ckBxUsuarioClave.Checked = false;

                txtBxRolId.Text = ""; txtBxRolNombre.Text = "";
                txtBxRolId2.Text = ""; txtBxRolNombre2.Text = "";

                txtBxPermisoId.Text = ""; txtBxPermisoNombre.Text = "";
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void btnLimpiarCampos_Click(object sender, EventArgs e) => LimpiarCampos();

        private BERol ValidarDatosRol(TextBox txtId, TextBox txtNombre)
        {
            if (txtNombre.Text.Length == 0) throw new Exception("El nombre de Rol no puede ser vacío.");
            nwRegex = new Regex("^[A-Za-z0-9 áéíóúÁÉÍÓÚ_-]+$");
            if (!nwRegex.IsMatch(txtNombre.Text.Trim())) throw new Exception("Nombre de Rol inválido.");

            if (txtId.Text.Length > 0)
                return new BERol(long.Parse(txtId.Text.Trim()), txtNombre.Text.Trim());
            else
                return new BERol(0, txtNombre.Text.Trim());
        }

        private BEPermiso ValidarDatosPermiso()
        {
            if (txtBxPermisoNombre.Text.Length == 0) throw new Exception("El nombre del Permiso no puede ser vacío.");
            if (txtBxPermisoId.Text.Length > 0)
                return new BEPermiso(long.Parse(txtBxPermisoId.Text.Trim()), txtBxPermisoNombre.Text.Trim());
            else
                return new BEPermiso(0, txtBxPermisoNombre.Text.Trim());
        }

        private void CargarTreeViewUsuarios()
        {
            treeVwUsuarios.Nodes.Clear();
            TreeNode raiz = new TreeNode("Usuarios");
            treeVwUsuarios.Nodes.Add(raiz);
            List<BEUsuario> lista = oBLLUsuario.ListarTodo();
            foreach (var u in lista)
                raiz.Nodes.Add(new TreeNode(u.Id + ", " + u.Usuario));
            treeVwUsuarios.ExpandAll();
        }

        private void treeVwUsuarios_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Parent != null && e.Node.Parent.Text == "Usuarios")
                {
                    string[] parts = e.Node.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        oBEUsuario = new BEUsuario { Id = long.Parse(parts[0].Trim()), Usuario = parts[1].Trim() };
                        oBEUsuario = oBLLUsuario.ListarObjeto(oBEUsuario);
                        txtBxUsuarioId.Text = oBEUsuario.Id.ToString().Trim();
                        txtBxUsuarioNombre.Text = oBEUsuario.Usuario.Trim();
                        txtBxUsuarioPassword.Text = oBEUsuario.Password.Trim();
                        ckBxUsuarioActivo.Checked = oBEUsuario.Activo;
                        ckBxUsuarioBloqueado.Checked = oBEUsuario.Bloqueado;
                        CargarTreeViewUsuarioRolPermisos();
                        AplicarBloqueoUsuario(oBEUsuario);
                    }
                    else throw new Exception("No se pudo recuperar el usuario.");
                }
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void CargarTreeViewRoles()
        {
            treeVwRoles.Nodes.Clear();
            TreeNode raiz = new TreeNode("Roles");
            treeVwRoles.Nodes.Add(raiz);
            var lista = oBLLRol.ListarTodo();
            foreach (var r in lista)
            {
                TreeNode nodo = new TreeNode(r.Nombre);
                raiz.Nodes.Add(nodo);
                var rolFull = oBLLRol.ListarObjeto(new BERol(r.Id, r.Nombre));
                ArmarTreeViewRoles(rolFull, nodo);
            }
            treeVwRoles.ExpandAll();
        }

        private void ArmarTreeViewRoles(BEComposite comp, TreeNode nodo)
        {
            var hijos = comp.ObtenerHijos();
            if (hijos != null)
                foreach (var h in hijos)
                {
                    TreeNode n = new TreeNode(h.Nombre);
                    nodo.Nodes.Add(n);
                    if (h is BERol) ArmarTreeViewRoles(h, n);
                }
        }
        private void AplicarBloqueoRol(BERol r)
        {
            bool esAdminRol = r != null &&
                              !string.IsNullOrWhiteSpace(r.Nombre) &&
                              r.Nombre.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);

            // Botones de MANTENIMIENTO del rol
            btnRolModificar.Enabled = !esAdminRol;
            btnRolEliminar.Enabled = !esAdminRol;

            // Asociación de permisos al rol
            btnPermisoRolAsociar.Enabled = !esAdminRol;
            btnPermisoRolQuitar.Enabled = !esAdminRol;

            // Campos de edición del rol
            txtBxRolNombre.ReadOnly = esAdminRol;
        }

        // Cancela tildado/destildado en el árbol de roles si es ADMIN
        private void TreeRol_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                if (oBERol != null &&
                    !string.IsNullOrWhiteSpace(oBERol.Nombre) &&
                    oBERol.Nombre.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))
                {
                    e.Cancel = true;
                }
            }
            catch { /* no-op */ }
        }

        private void treeVwRoles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Parent != null && e.Node.Parent.Text == "Roles")
                {
                    string rolNombre = e.Node.Text;
                    oBERol = oBLLRol.ListarObjeto(new BERol(0, rolNombre));
                    txtBxRolId.Text = oBERol.Id.ToString().Trim();
                    txtBxRolNombre.Text = oBERol.Nombre.Trim();
                    AplicarBloqueoRol(oBERol);
                    CargarTreeViewRolPermisos();
                }
                else
                {
                    treeVwPermisosPorRol.Nodes.Clear();
                    txtBxRolId.Text = ""; txtBxRolNombre.Text = "";
                }
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void CargarTreeViewPermisos()
        {
            try
            {
                treeVwPermisos.Nodes.Clear();

                if (_menuStrip != null)
                {
                    foreach (ToolStripItem it in _menuStrip.Items)
                        if (it is ToolStripMenuItem top)
                        {
                            var nodoTop = new TreeNode(top.Text) { Tag = Camino(top) };
                            CargarSubPermisos(top, nodoTop);
                            treeVwPermisos.Nodes.Add(nodoTop);
                        }
                }
                treeVwPermisos.ExpandAll();
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void CargarSubPermisos(ToolStripMenuItem mi, TreeNode padre)
        {
            foreach (ToolStripItem sub in mi.DropDownItems)
                if (sub is ToolStripMenuItem sm)
                {
                    var nodo = new TreeNode(sm.Text) { Tag = Camino(sm) };
                    padre.Nodes.Add(nodo);
                    CargarSubPermisos(sm, nodo);
                }
        }

        private void treeVwPermisos_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                var n = e.Node;
                if (n == null) return;

                // El Tag guarda SIEMPRE el camino exacto
                var key = (n.Tag as string ?? "").Trim();

                if (n.Parent == null)
                {
                    SelectComboItem(cmBxPermisoMenu, n.Text);
                    txtBxPermisoNombre.Text = key;
                    txtBxPermisoId.Text = "";
                }
                else
                {
                    SelectComboItem(cmBxPermisoMenu, n.Parent.Text);
                    cmBxPermisoMenu_SelectedIndexChanged(cmBxPermisoMenu, EventArgs.Empty);
                    SelectComboItem(cmBxPermisoItem, n.Text);

                    txtBxPermisoNombre.Text = key;
                    txtBxPermisoId.Text = "";
                }

                var tmp = new BEPermiso(0, key);
                if (oBLLPermiso.VerificarExistenciaObjeto(tmp))
                {
                    tmp = oBLLPermiso.ListarObjeto(tmp);
                    txtBxPermisoId.Text = tmp.Id.ToString().Trim();
                    txtBxPermisoNombre.Text = tmp.Nombre.Trim();
                }
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void SelectComboItem(ComboBox combo, string text)
        {
            for (int i = 0; i < combo.Items.Count; i++)
                if (combo.Items[i].ToString() == text) { combo.SelectedIndex = i; return; }
        }

        private void CargarTreeViewRolPermisos()
        {
            treeVwPermisosPorRol.Nodes.Clear();
            if (oBERol == null) return;

            var rolFull = oBLLRol.ListarObjeto(new BERol(0, oBERol.Nombre));
            if (rolFull == null) return;

            TreeNode raiz = new TreeNode(rolFull.Nombre.Trim());
            treeVwPermisosPorRol.Nodes.Add(raiz);
            CargarNodosHijos(raiz, rolFull.ObtenerHijos());
            treeVwPermisosPorRol.ExpandAll();
        }

        private void CargarNodosHijos(TreeNode padre, IList<BEComposite> hijos)
        {
            if (hijos == null) return;
            foreach (var h in hijos)
            {
                if (h is BERol r)
                {
                    TreeNode n = new TreeNode(r.Nombre);
                    padre.Nodes.Add(n);
                    CargarNodosHijos(n, r.ObtenerHijos());
                }
                else if (h is BEPermiso p)
                {
                    // Muestro el texto del ítem si existe, y entre [] el camino
                    string texto = BuscarTextoPorCamino(p.Nombre, out string textoMenuPadre);
                    var nodoPadreMenu = EncontrarONuevo(padre, textoMenuPadre);
                    nodoPadreMenu.Nodes.Add(new TreeNode($"{texto} [{p.Nombre}]"));
                }
            }
        }

        private string BuscarTextoPorCamino(string camino, out string textoPadre)
        {
            textoPadre = "";
            if (_menuStrip == null) return camino;

            foreach (ToolStripItem it in _menuStrip.Items)
                if (it is ToolStripMenuItem top)
                {
                    var txt = BuscarTextoRec(top, camino, out textoPadre);
                    if (txt != null) return txt;
                }

            return camino; // fallback
        }

        private string BuscarTextoRec(ToolStripMenuItem mi, string caminoBuscado, out string textoPadre)
        {
            var k = Camino(mi);
            if (string.Equals(k, caminoBuscado, StringComparison.OrdinalIgnoreCase))
            {
                textoPadre = (mi.OwnerItem as ToolStripMenuItem)?.Text ?? "";
                return mi.Text;
            }

            foreach (ToolStripItem sub in mi.DropDownItems)
                if (sub is ToolStripMenuItem sm)
                {
                    var hallado = BuscarTextoRec(sm, caminoBuscado, out textoPadre);
                    if (hallado != null) return hallado;
                }

            textoPadre = "";
            return null;
        }

        private TreeNode EncontrarONuevo(TreeNode padre, string texto)
        {
            if (string.IsNullOrEmpty(texto)) return padre;
            foreach (TreeNode n in padre.Nodes) if (n.Text == texto) return n;
            var nuevo = new TreeNode(texto);
            padre.Nodes.Add(nuevo);
            return nuevo;
        }

        private void CargarTreeViewUsuarioRolPermisos()
        {
            treeVwUsuarioPermisosRoles.Nodes.Clear();
            if (oBEUsuario != null && oBEUsuario.Id > 0)
            {
                TreeNode raiz = new TreeNode(oBEUsuario.Usuario.Trim());
                treeVwUsuarioPermisosRoles.Nodes.Add(raiz);
                oBEUsuario = oBLLUsuario.ListarObjetoJerarquico(oBEUsuario);

                if (oBEUsuario.listaRoles != null)
                    foreach (var rol in oBEUsuario.listaRoles)
                    {
                        TreeNode nRol = new TreeNode(rol.Nombre.Trim());
                        raiz.Nodes.Add(nRol);
                        AgregarRolesYPermisosAlTreeView(rol.ObtenerHijos(), nRol);
                    }

                if (oBEUsuario.listaPermisos != null)
                    foreach (var p in oBEUsuario.listaPermisos)
                        AgregarPermisoAlTreeView(_menuStrip, p.Nombre, raiz);
            }
        }

        private void AgregarRolesYPermisosAlTreeView(IList<BEComposite> lista, TreeNode padre)
        {
            if (lista == null) return;
            foreach (var c in lista)
            {
                if (c is BERol sr)
                {
                    TreeNode nodoRol = MetodoNodos(padre, sr.Nombre);
                    AgregarRolesYPermisosAlTreeView(sr.ObtenerHijos(), nodoRol);
                }
                else if (c is BEPermiso perm)
                {
                    AgregarPermisoAlTreeView(_menuStrip, perm.Nombre, padre);
                }
            }
        }

        private void AgregarPermisoAlTreeView(MenuStrip ms, string caminoPermiso, TreeNode padre)
        {
            if (ms == null) return;

            foreach (ToolStripItem it in ms.Items)
                if (it is ToolStripMenuItem top)
                {
                    var txt = BuscarTextoRec(top, caminoPermiso, out string textoPadre);
                    if (txt != null)
                    {
                        TreeNode nodoPadreMenu = MetodoNodos(padre, textoPadre);
                        nodoPadreMenu.Nodes.Add(new TreeNode($"{txt} [{caminoPermiso}]"));
                        return;
                    }
                }
        }

        private TreeNode MetodoNodos(TreeNode padre, string texto)
        {
            foreach (TreeNode n in padre.Nodes) if (n.Text == texto) return n;
            TreeNode nuevo = new TreeNode(texto);
            padre.Nodes.Add(nuevo);
            return nuevo;
        }

        private void CargarComboBoxMenus()
        {
            var lista = new List<ToolStripMenuItem> { new ToolStripMenuItem { Text = "" } };

            if (_menuStrip != null)
                foreach (var it in _menuStrip.Items)
                    if (it is ToolStripMenuItem m && !string.IsNullOrEmpty(m.Text))
                        lista.Add(m);

            cmBxPermisoMenu.DataSource = lista;
            cmBxPermisoMenu.DisplayMember = "Text";
            cmBxPermisoItem.DataSource = null;
        }

        private void cmBxPermisoMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmBxPermisoItem.DataSource = null;
            if (cmBxPermisoMenu.SelectedItem is ToolStripMenuItem sel)
            {
                var items = new List<ToolStripMenuItem> { new ToolStripMenuItem("") };
                foreach (var sub in sel.DropDownItems)
                    if (sub is ToolStripMenuItem mi && !string.IsNullOrWhiteSpace(mi.Text))
                        items.Add(mi);

                txtBxPermisoId.Text = "";
                txtBxPermisoNombre.Text = Camino(sel); // permiso = camino exacto del MENÚ padre
                cmBxPermisoItem.DataSource = items;
                cmBxPermisoItem.DisplayMember = "Text";
            }
        }

        private void cmBxPermisoItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmBxPermisoItem.SelectedItem is ToolStripMenuItem sel)
            {
                string key = Camino(sel); // permiso = camino exacto del ÍTEM
                var tmp = new BEPermiso(0, key);
                if (oBLLPermiso.VerificarExistenciaObjeto(tmp))
                {
                    tmp = oBLLPermiso.ListarObjeto(tmp);
                    txtBxPermisoId.Text = tmp.Id.ToString().Trim();
                    txtBxPermisoNombre.Text = tmp.Nombre.Trim();
                }
                else
                {
                    txtBxPermisoId.Text = "";
                    txtBxPermisoNombre.Text = key;
                }
            }
        }

        private void CargarComboBoxRoles()
        {
            var lista = new List<BERol>();
            lista.Add(new BERol(-1, ""));
            lista.AddRange(oBLLRol.ListarTodo());
            cmbBxRoles.DataSource = lista;
            cmbBxRoles.DisplayMember = "Nombre";
        }

        private void cmbBxRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBxRoles.SelectedIndex == -1) return;
                if (cmbBxRoles.SelectedItem is BERol sel)
                {
                    if (sel.Nombre == "")
                    {
                        txtBxRolId2.Text = "";
                        txtBxRolNombre2.Text = "";
                    }
                    else
                    {
                        oBERol2 = oBLLRol.ListarObjeto(new BERol(0, sel.Nombre));
                        txtBxRolId2.Text = oBERol2.Id.ToString().Trim();
                        txtBxRolNombre2.Text = oBERol2.Nombre.Trim();
                    }
                }
            }
            catch (Exception ex) { MsgErr(ex); }
        }

        private void ckBxUsuarioClave_CheckedChanged(object sender, EventArgs e)
        {
            if (txtBxUsuarioPassword.Text.Length > 0)
            {
                if (ckBxUsuarioClave.Checked)
                    txtBxUsuarioPassword.Text = oBLLUsuario.DesencriptarPassword(txtBxUsuarioPassword.Text.Trim());
                else
                    txtBxUsuarioPassword.Text = oBLLUsuario.EncriptarPassword(txtBxUsuarioPassword.Text.Trim());
            }
        }

        private void MsgErr(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
