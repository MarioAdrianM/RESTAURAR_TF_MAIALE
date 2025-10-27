using BLL_Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurar_v1._0
{
    public partial class FormBitacora : Form
    {
        private readonly BLLBitacora _bll = new BLLBitacora();
        private List<BE.BEBitacora> _cache;
        public FormBitacora()
        {
            InitializeComponent();
            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;
            cmbAccion.Items.Clear();
            cmbAccion.Items.AddRange(new object[] { "Todos", "LOGIN_OK", "LOGIN_FAIL" });
            cmbAccion.SelectedIndex = 0;
        }

        private void FormBitacora_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                _cache = _bll.Listar(); // ya viene ordenado desc por fecha
                dgvBitacora.AutoGenerateColumns = true;
                dgvBitacora.DataSource = _cache;
                dgvBitacora.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvBitacora.ReadOnly = true;
                dgvBitacora.AllowUserToAddRows = false;
                dgvBitacora.AllowUserToDeleteRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error cargando bitácora: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var desde = dtpDesde.Value.Date;
                var hasta = dtpHasta.Value.Date.AddDays(1).AddTicks(-1); // fin del día
                var usr = (txtUsuario.Text ?? "").Trim().ToLower();
                var acc = cmbAccion.SelectedItem?.ToString() ?? "Todos";

                var q = _cache.Where(b => b.Fecha >= desde && b.Fecha <= hasta);

                if (!string.IsNullOrEmpty(usr))
                    q = q.Where(b => (b.Usuario ?? "").ToLower().Contains(usr));

                if (acc != "Todos")
                    q = q.Where(b => string.Equals(b.Accion, acc, StringComparison.OrdinalIgnoreCase));

                dgvBitacora.DataSource = q.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtrando: " + ex.Message);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            dtpDesde.Value = DateTime.Today.AddDays(-7);
            dtpHasta.Value = DateTime.Today;
            txtUsuario.Text = "";
            cmbAccion.SelectedIndex = 0;
            dgvBitacora.DataSource = _cache;
        }

       


    }
}
