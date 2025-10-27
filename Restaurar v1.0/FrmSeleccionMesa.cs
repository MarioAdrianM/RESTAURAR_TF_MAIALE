using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BE;
using BLL_Negocio;

namespace Restaurar_v1._0
{
    public partial class FrmSeleccionMesa : Form
    {
        private readonly BLLReserva bllReserva = new BLLReserva();
        private readonly BLLMesa bllMesa = new BLLMesa();
        private readonly BLLSector bllSector = new BLLSector();

        private readonly DateTime _fecha;
        private readonly string _hora;
        private readonly int _comensales;

        public long MesaSeleccionadaId { get; private set; } = 0;
        public FrmSeleccionMesa() : this(DateTime.Today, "20:00", 2) { }
        public FrmSeleccionMesa(DateTime fecha, string hora, int comensales)
        {
            InitializeComponent();
            _fecha = fecha.Date;
            _hora = hora;
            _comensales = comensales;

            // Si no enganchaste eventos en el diseñador, descomentá estas líneas:
            // this.Load += FrmSeleccionMesa_Load;
            // cboSector.SelectedIndexChanged += cboSector_SelectedIndexChanged;
            // btnAceptar.Click += btnAceptar_Click;
            // btnCancelar.Click += btnCancelar_Click;
            // dgvDisponibles.CellDoubleClick += dgvDisponibles_CellDoubleClick;
        }
        private void FrmSeleccionMesa_Load(object sender, EventArgs e)
        {
            var activos = bllSector.ListarActivos();
            if (activos == null || activos.Count == 0)
            {
                MessageBox.Show("No hay sectores habilitados. Cree o active uno en ABM de Sectores.", "Seleccionar Mesa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            lblInfo.Text = $"Fecha: {_fecha:dd/MM/yyyy}  |  Hora: {_hora}  |  Comensales: {_comensales}";
            InicializarGrilla();
            CargarComboSectores();
            if (cboSector.Items.Count == 0) cboSector.Items.Add("Todos");
            cboSector.SelectedIndex = 0;
            CargarDisponibles("Todos");
        }
        private void InicializarGrilla()
        {
            dgvDisponibles.AutoGenerateColumns = false;
            dgvDisponibles.Columns.Clear();
            dgvDisponibles.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Id", DataPropertyName = "Id", Visible = false });
            dgvDisponibles.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNumero", HeaderText = "Mesa N°", DataPropertyName = "Numero", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvDisponibles.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCapacidad", HeaderText = "Capacidad", DataPropertyName = "Capacidad", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvDisponibles.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSector", HeaderText = "Sector", DataPropertyName = "Sector", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
            dgvDisponibles.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEstado", HeaderText = "Estado", DataPropertyName = "Estado", AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
        }

        private void CargarComboSectores()
        {
            var sectores = bllSector.ListarActivos() ?? new List<BESector>();
            cboSector.Items.Clear();
            cboSector.Items.Add("Todos");
            foreach (var s in sectores) cboSector.Items.Add(s.Nombre);
        }

        private void CargarDisponibles(string sector)
        {
            try
            {
                var disponibles = bllReserva.MesasDisponibles(_fecha, _hora, _comensales, sector) ?? new List<BEMesa>();
                dgvDisponibles.DataSource = disponibles;
                dgvDisponibles.ClearSelection();
                if (dgvDisponibles.Rows.Count > 0)
                {
                    dgvDisponibles.CurrentCell = dgvDisponibles.Rows[0].Cells["colNumero"];
                    dgvDisponibles.Rows[0].Selected = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar mesas: " + ex.Message, "Seleccionar Mesa",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvDisponibles.DataSource = new List<BEMesa>();
            }
        }

        private void cboSector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sector = cboSector.SelectedItem?.ToString() ?? "Todos";
            CargarDisponibles(sector);

        }
        private void AceptarSeleccion()
        {
            if (dgvDisponibles.CurrentRow?.DataBoundItem is BEMesa m)
            {
                MesaSeleccionadaId = m.Id;
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Seleccioná una mesa.", "Seleccionar Mesa");
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            AceptarSeleccion();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void dgvDisponibles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AceptarSeleccion();
        }

        
    }
}
