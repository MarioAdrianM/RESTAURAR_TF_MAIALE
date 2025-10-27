using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL_Negocio;

namespace Restaurar_v1._0
{
    public partial class FrmMesasABM : Form
    {
        private readonly BLLMesa bll = new BLLMesa();
        private readonly BLL_Negocio.BLLSector bllSector = new BLL_Negocio.BLLSector();
        public FrmMesasABM()
        {
            InitializeComponent();
        }

        private void FrmMesasABM_Load(object sender, EventArgs e)
        {
         
            InicializarGrilla();
            CargarSectores();
            CargarGrilla();
            LimpiarCampos();
        }
        private void InicializarGrilla()
        {
            dgvMesas.AutoGenerateColumns = false;
            dgvMesas.Columns.Clear();

            dgvMesas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", Visible = false });
            dgvMesas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNumero", HeaderText = "Número", DataPropertyName = "Numero", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMesas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCapacidad", HeaderText = "Capacidad", DataPropertyName = "Capacidad", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMesas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSector", HeaderText = "Sector", DataPropertyName = "Sector", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMesas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", DataPropertyName = "Estado", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMesas.Columns.Add(new DataGridViewCheckBoxColumn { Name = "colHabilitada", HeaderText = "Habilitada", DataPropertyName = "Habilitada", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvMesas.Columns.Add(new DataGridViewTextBoxColumn { Name = "colObs", HeaderText = "Observaciones", DataPropertyName = "Observaciones", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        }

        private void CargarGrilla()
        {
            try
            {
                var lista = bll.ListarTodo() ?? new List<BEMesa>();
                lista = lista.OrderBy(x => x.Numero).ToList();
                dgvMesas.DataSource = lista;

                dgvMesas.ClearSelection();
                if (dgvMesas.Rows.Count > 0)
                {
                    dgvMesas.CurrentCell = dgvMesas.Rows[0].Cells["colNumero"];
                    dgvMesas.Rows[0].Selected = true;
                    if (dgvMesas.Rows[0].DataBoundItem is BEMesa m0) MapearAlFormulario(m0);
                }
                else LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar mesas: " + ex.Message, "Mesas",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarSectores()
        {
            var activos = bllSector.ListarActivos() ?? new List<BE.BESector>();

            cboSector.BeginUpdate();
            cboSector.Items.Clear();
            foreach (var s in activos) cboSector.Items.Add(s.Nombre);
            cboSector.EndUpdate();

            if (cboSector.Items.Count > 0)
            {
                cboSector.SelectedIndex = 0;
            }
            else
            {
                // No hay sectores activos → combo vacío y sin selección
                cboSector.SelectedIndex = -1;
                // (opcional) avisar una sola vez al abrir el form:
                // MessageBox.Show("No hay sectores habilitados. Cree o active uno en ABM de Sectores.", "Mesas");
            }
        }



        private void LimpiarCampos()
        {
            txtId.Text = "";
            txtNumero.Text = "";
            txtCapacidad.Text = "";
            if (cboSector.Items.Count > 0)
                cboSector.SelectedIndex = 0;
            else
                cboSector.SelectedIndex = -1;
            chkHabilitada.Checked = true;
            txtObservaciones.Text = "";
            lblEstadoValor.Text = BE.EstadosMesa.Libre;
            txtNumero.Focus();
        }

        private BEMesa MapearDesdeFormulario()
        {
            var m = new BEMesa();
            if (long.TryParse(txtId.Text, out var id)) m.Id = id;

            if (!int.TryParse(txtNumero.Text, out var numero)) throw new Exception("Número inválido.");
            if (!int.TryParse(txtCapacidad.Text, out var capacidad)) throw new Exception("Capacidad inválida.");

            m.Numero = numero;
            m.Capacidad = capacidad;
            m.Sector = (cboSector.SelectedItem?.ToString() ?? "").Trim();
           
            if (cboSector.SelectedItem == null)
                throw new Exception("No hay sectores habilitados. Cree o active uno en ABM de Sectores.");
            m.Sector = cboSector.SelectedItem.ToString().Trim();
            // No permitir sector inactivo al guardar
            var esActivo = (bllSector.ListarActivos() ?? new List<BESector>())
                           .Any(s => string.Equals(s.Nombre, m.Sector, StringComparison.OrdinalIgnoreCase));
            if (!esActivo)
                throw new Exception("El sector seleccionado está INACTIVO. Elegí un sector activo para la mesa.");

            m.Habilitada = chkHabilitada.Checked;
            m.Observaciones = txtObservaciones.Text?.Trim();
            m.Estado = m.Habilitada ? BE.EstadosMesa.Libre : BE.EstadosMesa.Bloqueada; // ABM no marca Ocupada
            return m;
        }

        private void MapearAlFormulario(BEMesa m)
        {
            if (m == null) return;
            txtId.Text = m.Id.ToString();
            txtNumero.Text = m.Numero.ToString();
            txtCapacidad.Text = m.Capacidad.ToString();
            var nombre = m.Sector ?? "";
            int idx = -1;
            for (int i = 0; i < cboSector.Items.Count; i++)
                if (string.Equals(cboSector.Items[i].ToString(), nombre, StringComparison.OrdinalIgnoreCase))
                { idx = i; break; }
            if (idx >= 0) cboSector.SelectedIndex = idx;
            else { cboSector.Items.Add(nombre); cboSector.SelectedIndex = cboSector.Items.Count - 1; }

            chkHabilitada.Checked = m.Habilitada;
            txtObservaciones.Text = m.Observaciones;

            lblEstadoValor.Text = m.Habilitada ? BE.EstadosMesa.Libre : BE.EstadosMesa.Bloqueada;
            if (string.Equals(m.Estado, BE.EstadosMesa.Ocupada, StringComparison.OrdinalIgnoreCase))
                lblEstadoValor.Text = BE.EstadosMesa.Ocupada;
        }
        private void dgvMesas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMesas.CurrentRow?.DataBoundItem is BEMesa m) MapearAlFormulario(m);
            else if (dgvMesas.Rows.Count == 1)
            {
                dgvMesas.CurrentCell = dgvMesas.Rows[0].Cells["colNumero"];
                dgvMesas.Rows[0].Selected = true;
                if (dgvMesas.Rows[0].DataBoundItem is BEMesa m0) MapearAlFormulario(m0);
            }
        }

        

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SoloNumeros_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void btnCrearMesa_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSector.SelectedItem == null)
                {
                    MessageBox.Show("No hay sectores habilitados. Cree o active uno en ABM de Sectores.", "Mesas");
                    return;
                }
                txtId.Text = ""; // asegurar alta

                var m = MapearDesdeFormulario();
                bll.Guardar(m);
                CargarGrilla();

                var lista = dgvMesas.DataSource as List<BEMesa>;
                var idx = lista?.FindIndex(x => x.Numero == m.Numero) ?? -1;
                if (idx >= 0)
                {
                    dgvMesas.ClearSelection();
                    dgvMesas.Rows[idx].Selected = true;
                    dgvMesas.CurrentCell = dgvMesas.Rows[idx].Cells["colNumero"];
                    MapearAlFormulario(lista[idx]);
                }
                MessageBox.Show("Mesa creada.", "Mesas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("No se pudo crear: " + ex.Message, "Mesas", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void btnModificarMesa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!long.TryParse(txtId.Text, out var id) || id <= 0)
                { MessageBox.Show("Seleccioná una mesa.", "Mesas"); return; }

                if (cboSector.SelectedItem == null)
                {
                    MessageBox.Show("No hay sectores habilitados. Cree o active uno en ABM de Sectores.", "Mesas");
                    return;
                }
                var m = MapearDesdeFormulario(); m.Id = id;
                bll.Guardar(m);
                CargarGrilla();
                MessageBox.Show("Mesa modificada.", "Mesas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("No se pudo modificar: " + ex.Message, "Mesas", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void btnEliminarMesa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!long.TryParse(txtId.Text, out var id) || id <= 0)
                { MessageBox.Show("Seleccioná una mesa.", "Mesas"); return; }

                if (bll.TieneReservasAsociadas(id))
                {
                    var eleccion = MessageBox.Show(
                        "La mesa tiene reservas asociadas.\n¿Deshabilitarla (Bloqueada) en lugar de eliminar?",
                        "Mesas", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (eleccion == DialogResult.Yes)
                    {
                        var m = bll.ListarObjeto(new BEMesa { Id = id });
                        if (m == null) throw new Exception("Mesa no encontrada.");
                        m.Habilitada = false; m.Estado = BE.EstadosMesa.Bloqueada;
                        bll.Guardar(m);
                        CargarGrilla();
                        MessageBox.Show("Mesa deshabilitada (Bloqueada).", "Mesas");
                    }
                    return;
                }

                var ok = MessageBox.Show("¿Eliminar la mesa seleccionada?", "Mesas",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ok != DialogResult.Yes) return;

                bll.Eliminar(new BEMesa { Id = id });
                CargarGrilla();
                LimpiarCampos();
                MessageBox.Show("Mesa eliminada.", "Mesas");
            }
            catch (Exception ex) { MessageBox.Show("No se pudo eliminar: " + ex.Message, "Mesas", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void btnLimpiarCampos_Click(object sender, EventArgs e)
        {
            dgvMesas.ClearSelection();
            LimpiarCampos();

        }
    }
}
