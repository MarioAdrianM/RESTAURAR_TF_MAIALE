using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BE.BEComposite;
using BLL_Negocio;
using Seguridad;

namespace Restaurar_v1._0
{
    public partial class FormUsuarios : Form
    {
        private readonly BLLUsuario bll = new BLLUsuario();
        private List<BEUsuario> _lista = new List<BEUsuario>();
        private BEUsuario _seleccionado; // en edición

        public FormUsuarios()
        {
            InitializeComponent();
            this.Text = "ABM de Usuarios";
        }

        private void FormUsuarios_Load(object sender, EventArgs e)
        {
            // Doble barrera: además del menú oculto, cierro si no tiene permiso
          

            ConfigurarGrilla();
            LimpiarFormulario();
            CargarGrilla();

            chkCambiarPass.CheckedChanged += (s, args) =>
            {
                txtPassNueva.Enabled = chkCambiarPass.Checked;
                txtPassRepetir.Enabled = chkCambiarPass.Checked;
                if (!chkCambiarPass.Checked)
                {
                    txtPassNueva.Clear();
                    txtPassRepetir.Clear();
                }
            };
        }

        // ======= Grilla =======
        private void ConfigurarGrilla()
        {
            dgvUsuarios.AutoGenerateColumns = false;
            dgvUsuarios.Columns.Clear();

            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "Id",
                Width = 55
            });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Usuario",
                HeaderText = "Usuario",
                Width = 140
            });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 150
            });
            dgvUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Apellido",
                HeaderText = "Apellido",
                Width = 150
            });
            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Activo",
                HeaderText = "Activo",
                Width = 60
            });
            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "Bloqueado",
                HeaderText = "Bloq.",
                Width = 60
            });
            dgvUsuarios.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "DebeCambiarPassword",
                HeaderText = "Forzar cambio",
                Width = 110
            });

            dgvUsuarios.SelectionChanged += dgvUsuarios_SelectionChanged;
        }

        private void CargarGrilla()
        {
            _lista = bll.ListarTodo();
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = _lista;
            dgvUsuarios.ClearSelection();
        }

        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0) return;

            var row = dgvUsuarios.SelectedRows[0];
            if (row?.DataBoundItem is BEUsuario u)
            {
                _seleccionado = bll.ListarObjeto(new BEUsuario { Id = u.Id });
                if (_seleccionado == null) return;

                txtId.Text = _seleccionado.Id.ToString();
                txtUsuario.Text = _seleccionado.Usuario;
                txtNombre.Text = _seleccionado.Nombre;
                txtApellido.Text = _seleccionado.Apellido;
                chkActivo.Checked = _seleccionado.Activo;
                chkBloqueado.Checked = _seleccionado.Bloqueado;
                chkDebeCambiar.Checked = _seleccionado.DebeCambiarPassword;

                chkCambiarPass.Checked = false;
                AplicarBloqueoSiAdmin();

                txtPassNueva.Clear(); txtPassRepetir.Clear();
            }
        }

        // ======= Acciones =======
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            if (dgvUsuarios.SelectedRows.Count > 0)
                dgvUsuarios.ClearSelection();
            txtUsuario.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUsuario.Text.Trim();
                string nombre = txtNombre.Text.Trim();
                string apellido = txtApellido.Text.Trim();

                if (string.IsNullOrWhiteSpace(usuario))
                    throw new Exception("El usuario no puede estar vacío.");

                bool esNuevo = string.IsNullOrWhiteSpace(txtId.Text) || txtId.Text == "0";

                // Validaciones de contraseña:
                string passPlanoAGuardar;
                if (esNuevo || chkCambiarPass.Checked)
                {
                    if (string.IsNullOrEmpty(txtPassNueva.Text))
                        throw new Exception("La contraseña no puede estar vacía.");
                    if (!string.Equals(txtPassNueva.Text, txtPassRepetir.Text))
                        throw new Exception("Las contraseñas no coinciden.");
                    if (usuario.Equals(txtPassNueva.Text, StringComparison.OrdinalIgnoreCase))
                        throw new Exception("La contraseña no puede ser igual al usuario.");
                    passPlanoAGuardar = txtPassNueva.Text;
                }
                else
                {
                    // Mantener la actual en PLANO (evitar doble encriptado)
                    if (_seleccionado == null)
                        throw new Exception("Debe seleccionar un usuario o indicar cambio de contraseña.");
                    passPlanoAGuardar = bll.DesencriptarPassword(_seleccionado.Password ?? "");
                }

                var u = new BEUsuario
                {
                    Id = esNuevo ? 0 : long.Parse(txtId.Text),
                    Usuario = usuario,
                    Password = passPlanoAGuardar,             // plano; MPP cifra
                    Nombre = nombre,
                    Apellido = apellido,
                    Activo = chkActivo.Checked,
                    Bloqueado = chkBloqueado.Checked,
                    DebeCambiarPassword = chkDebeCambiar.Checked
                };

                bll.Guardar(u);

                MessageBox.Show("Usuario guardado correctamente.", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarGrilla();
                SeleccionarEnGrillaPorUsuario(u.Usuario);

                // limpiar sección de cambio de pass
                chkCambiarPass.Checked = false;
                txtPassNueva.Clear(); txtPassRepetir.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_seleccionado == null || _seleccionado.Id <= 0)
                    throw new Exception("Seleccione un usuario.");

                if (string.Equals(_seleccionado.Usuario, "admin", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("No se puede eliminar el usuario 'admin'.");

                var r = MessageBox.Show($"¿Eliminar al usuario '{_seleccionado.Usuario}'?",
                    "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r != DialogResult.Yes) return;

                bll.Eliminar(_seleccionado);

                MessageBox.Show("Usuario eliminado.", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarFormulario();
                CargarGrilla();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            try
            {
                if (_seleccionado == null || _seleccionado.Id <= 0)
                    throw new Exception("Seleccione un usuario.");

                _seleccionado.Bloqueado = false;

                // Mantener pass actual en PLANO para evitar doble encriptado
                var passPlano = bll.DesencriptarPassword(_seleccionado.Password ?? "");
                _seleccionado.Password = passPlano;

                bll.Guardar(_seleccionado);

                MessageBox.Show("Usuario desbloqueado.", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarGrilla();
                SeleccionarEnGrillaPorUsuario(_seleccionado.Usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ======= Helpers =======
        private void LimpiarFormulario()
        {
            _seleccionado = null;
            txtId.Text = "0";
            txtUsuario.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            chkActivo.Checked = true;
            chkBloqueado.Checked = false;
            chkDebeCambiar.Checked = false;
            HabilitarParaNuevo();

            chkCambiarPass.Checked = false;
            txtPassNueva.Enabled = false; txtPassNueva.Clear();
            txtPassRepetir.Enabled = false; txtPassRepetir.Clear();
            if (dgvUsuarios.SelectedRows.Count > 0)
                dgvUsuarios.ClearSelection();
        }

        private void SeleccionarEnGrillaPorUsuario(string usuario)
        {
            foreach (DataGridViewRow r in dgvUsuarios.Rows)
            {
                if (r.DataBoundItem is BEUsuario u && u.Usuario.Equals(usuario, StringComparison.OrdinalIgnoreCase))
                {
                    r.Selected = true;
                    dgvUsuarios.FirstDisplayedScrollingRowIndex = r.Index;
                    break;
                }
            }
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
        private void AplicarBloqueoSiAdmin()
        {
            if (_seleccionado == null)
            {
                HabilitarParaNuevo();
                return;
            }
            bool esAdmin = (_seleccionado?.Usuario ?? "").Equals("admin", StringComparison.OrdinalIgnoreCase);

            // Deshabilitá los controles que NO querés tocar para admin
            txtUsuario.Enabled = !esAdmin;
            
            chkActivo.Enabled = !esAdmin;
            chkBloqueado.Enabled = !esAdmin;
            chkDebeCambiar.Enabled = !esAdmin;
            chkCambiarPass.Enabled = !esAdmin;
            txtPassNueva.Enabled = !esAdmin && chkCambiarPass.Checked;
            txtPassRepetir.Enabled = !esAdmin && chkCambiarPass.Checked;

            
            btnEliminar.Enabled = !esAdmin;
            btnDesbloquear.Enabled = !esAdmin;

            
        }
        private void HabilitarParaNuevo()
        {
            // Campos editables
            txtUsuario.Enabled = true;
            txtNombre.Enabled = true;
            txtApellido.Enabled = true;

            // Checks
            chkActivo.Enabled = true;
            chkBloqueado.Enabled = true;
            chkDebeCambiar.Enabled = true;
            chkCambiarPass.Enabled = true;

            // Sección password (inactiva hasta que tildes Cambiar)
            txtPassNueva.Enabled = false;
            txtPassRepetir.Enabled = false;

            // Botones que no tienen sentido en “nuevo”
            btnEliminar.Enabled = false;
            btnDesbloquear.Enabled = false;
        }


    }
}
