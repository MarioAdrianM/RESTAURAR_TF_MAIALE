using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL_Negocio;

namespace Restaurar_v1._0
{
    public partial class FrmSectoresABM : Form
    {
        private readonly BLLSector bll = new BLLSector();
        public FrmSectoresABM()
        {
            InitializeComponent();
        }

        private void FrmSectoresABM_Load(object sender, EventArgs e)
        {
            InicializarGrilla();
            CargarGrilla();
            Limpiar();
        }
        private void InicializarGrilla()
        {
            dgvSectores.AutoGenerateColumns = false;
            dgvSectores.Columns.Clear();
            dgvSectores.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", Visible = false });
            dgvSectores.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNombre", HeaderText = "Nombre", DataPropertyName = "Nombre", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvSectores.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colActivo", HeaderText = "Activo", DataPropertyName = "Activo", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
        }

        private void CargarGrilla()
        {
            try
            {
                var lista = bll.ListarTodo() ?? new List<BESector>();
                lista = lista.OrderBy(x => x.Nombre).ToList();
                dgvSectores.DataSource = lista;

                dgvSectores.ClearSelection();
                if (dgvSectores.Rows.Count > 0)
                {
                    dgvSectores.CurrentCell = dgvSectores.Rows[0].Cells["colNombre"];
                    dgvSectores.Rows[0].Selected = true;
                    if (dgvSectores.Rows[0].DataBoundItem is BESector s0) MapearAlFormulario(s0);
                }
                else Limpiar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar sectores: " + ex.Message, "Sectores",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Limpiar()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            chkActivo.Checked = true;
            txtNombre.Focus();
        }

        private BESector MapearDesdeFormulario()
        {
            var s = new BESector();
            if (long.TryParse(txtId.Text, out var id)) s.Id = id;
            s.Nombre = txtNombre.Text?.Trim();
            s.Activo = chkActivo.Checked;
            return s;
        }

        private void MapearAlFormulario(BESector s)
        {
            if (s == null) return;
            txtId.Text = s.Id.ToString();
            txtNombre.Text = s.Nombre;
            chkActivo.Checked = s.Activo;
        }

        private void dgvSectores_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSectores.CurrentRow?.DataBoundItem is BESector s) MapearAlFormulario(s);
            else if (dgvSectores.Rows.Count == 1)
            {
                dgvSectores.CurrentCell = dgvSectores.Rows[0].Cells["colNombre"];
                dgvSectores.Rows[0].Selected = true;
                if (dgvSectores.Rows[0].DataBoundItem is BESector s0) MapearAlFormulario(s0);
            }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            try { txtId.Text = ""; var s = MapearDesdeFormulario(); bll.Guardar(s); CargarGrilla(); MessageBox.Show("Sector creado.", "Sectores"); }
            catch (Exception ex) { MessageBox.Show("No se pudo crear: " + ex.Message, "Sectores"); }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!long.TryParse(txtId.Text, out var id) || id <= 0) { MessageBox.Show("Seleccioná un sector.", "Sectores"); return; }
                var s = MapearDesdeFormulario(); s.Id = id; bll.Guardar(s); CargarGrilla(); MessageBox.Show("Sector modificado.", "Sectores");
            }
            catch (Exception ex) { MessageBox.Show("No se pudo modificar: " + ex.Message, "Sectores"); }

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!long.TryParse(txtId.Text, out var id) || id <= 0) { MessageBox.Show("Seleccioná un sector.", "Sectores"); return; }
                var ok = MessageBox.Show("¿Eliminar el sector?", "Sectores", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ok != DialogResult.Yes) return;

                var s = bll.ListarObjeto(new BESector { Id = id });
                bll.Eliminar(s); CargarGrilla(); Limpiar(); MessageBox.Show("Sector eliminado.", "Sectores");
            }
            catch (Exception ex) { MessageBox.Show("No se pudo eliminar: " + ex.Message, "Sectores"); }

        }

        private void btnActivarDesactivar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!long.TryParse(txtId.Text, out var id) || id <= 0) { MessageBox.Show("Seleccioná un sector.", "Sectores"); return; }
                var s = bll.ListarObjeto(new BESector { Id = id });
                bll.Activar(id, !s.Activo); CargarGrilla();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Sectores"); }

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dgvSectores.ClearSelection();
            Limpiar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();

        }
    }
}
