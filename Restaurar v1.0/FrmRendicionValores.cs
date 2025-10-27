using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BLL_Negocio;
using BE;

namespace Restaurar_v1._0
{
    public partial class FrmRendicionValores : Form
    {
        private readonly BLLCaja _bll = new BLLCaja();
        private BECaja _caja; // caja abierta seleccionada
        public FrmRendicionValores()
        {
            InitializeComponent();
            
        }
        private void CargarCajasAbiertas()
        {
            var abiertas = _bll.ListarAbiertas();
            cboCajaAbierta.DisplayMember = "Nombre";
            cboCajaAbierta.ValueMember = "Id";
            cboCajaAbierta.DataSource = abiertas;

            lblCaja.Text = abiertas.Any() ? "Seleccione una caja abierta." : "No hay cajas abiertas.";
            dgvMozos.Rows.Clear();
            LimpiarEsperadoYEntregado();
        }
        private void CargarMozosConPendientes()
        {
            dgvMozos.Rows.Clear();
            if (_caja == null) return;

            var rows = _bll.MozosConPendientes(_caja.Id);
            foreach (var x in rows)
                dgvMozos.Rows.Add(x.Mozo, x.EspEf.ToString("n2"), x.EspTj.ToString("n2"), x.EspQr.ToString("n2"), x.Total.ToString("n2"));

            dgvMozos.ClearSelection();
        }
        private void LimpiarEsperadoYEntregado()
        {
            lblEspEf.Text = "0,00"; lblEspTj.Text = "0,00"; lblEspQR.Text = "0,00";
            nudEntEf.Value = 0; nudEntTj.Value = 0; nudEntQR.Value = 0;
            txtMozo.Clear(); txtObs.Clear();
            lblResultado.Text = "—";
            lblResultado.ForeColor = SystemColors.ControlText;
        }
        private void FrmRendicionValores_Load(object sender, EventArgs e)
        {
            CargarCajasAbiertas();
        }

        

        private void btnCalcular_Click(object sender, EventArgs e)
        {

            try
            {
                if (_caja == null) throw new InvalidOperationException("Primero seleccioná una caja abierta.");

                var mozo = (txtMozo.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(mozo) && dgvMozos.CurrentRow != null)
                    mozo = dgvMozos.CurrentRow.Cells["colMozo"].Value?.ToString();

                if (string.IsNullOrWhiteSpace(mozo))
                    throw new InvalidOperationException("Indicá el usuario del mozo.");

                var (ef, tj, qr) = _bll.CalcularEsperado(mozo, _caja.FechaApertura);
                lblEspEf.Text = ef.ToString("n2");
                lblEspTj.Text = tj.ToString("n2");
                lblEspQR.Text = qr.ToString("n2");

                nudEntEf.Value = Math.Min(nudEntEf.Maximum, ef);
                nudEntTj.Value = Math.Min(nudEntTj.Maximum, tj);
                nudEntQR.Value = Math.Min(nudEntQR.Maximum, qr);

                txtMozo.Text = mozo;
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
                lblResultado.ForeColor = Color.Firebrick;
                MessageBox.Show(ex.Message, "Rendición", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRendir_Click(object sender, EventArgs e)
        {
            try
            {
                if (_caja == null) throw new InvalidOperationException("Primero seleccioná una caja abierta.");
                var mozo = (txtMozo.Text ?? "").Trim();
                if (string.IsNullOrWhiteSpace(mozo)) throw new InvalidOperationException("Indicá el usuario del mozo.");

                var r = _bll.RecibirRendicion(
                    cajaId: _caja.Id,
                    mozoUsuario: mozo,
                    entEf: nudEntEf.Value,
                    entTj: nudEntTj.Value,
                    entQr: nudEntQR.Value,
                    obs: txtObs.Text?.Trim()
                );

                lblResultado.Text = $"Rendición {r.Estado}. Dif total: {r.DiferenciaTotal:n2}";
                lblResultado.ForeColor = r.Estado == "OK" ? Color.ForestGreen : Color.OrangeRed;

                // Refrescar pendientes
                CargarMozosConPendientes();
                LimpiarEsperadoYEntregado();
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
                lblResultado.ForeColor = Color.Firebrick;
                MessageBox.Show(ex.Message, "Rendición", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboCajaAbierta_SelectedIndexChanged(object sender, EventArgs e)
        {
            _caja = cboCajaAbierta.SelectedItem as BECaja;
            if (_caja == null)
            {
                lblCaja.Text = "—";
                dgvMozos.Rows.Clear();
                LimpiarEsperadoYEntregado();
                return;
            }

            lblCaja.Text = $"Caja: {_caja.Nombre} | Resp: {_caja.Responsable} | Fondo: {_caja.FondoInicial:n2} | Umbral: {_caja.UmbralDiferencia:n2}";
            CargarMozosConPendientes();
            LimpiarEsperadoYEntregado();

        }

        private void btnRefrescarCajas_Click(object sender, EventArgs e)
        {
            CargarCajasAbiertas();
        }

        private void dgvMozos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                txtMozo.Text = dgvMozos.Rows[e.RowIndex].Cells["colMozo"].Value?.ToString();
        }
    }
}
