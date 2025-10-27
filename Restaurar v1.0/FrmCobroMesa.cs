using System;
using System.Drawing;
using System.Windows.Forms;
using BLL_Negocio;
using BE;


namespace Restaurar_v1._0
{
    public partial class FrmCobroMesa : Form
    {
        private readonly BLLCobranza _bll = new BLLCobranza();
        private BEFactura _facturaActual; // cache de la factura encontrada
        public FrmCobroMesa()
        {
            InitializeComponent();
        }

        private void Medio_CheckedChanged(object sender, EventArgs e)
        {
            bool esElectronico = rdoTarjeta.Checked || rdoQR.Checked;
            txtAut.Enabled = esElectronico;

            if (_facturaActual != null)
            {
                if (rdoEfectivo.Checked)
                {
                    // efectivo: importe puede ser >= total
                    if (nudImporte.Value < _facturaActual.ImporteTotal)
                        nudImporte.Value = _facturaActual.ImporteTotal;
                }
                else
                {
                    // electrónico: importe exacto
                    nudImporte.Value = _facturaActual.ImporteTotal;
                }
            }
            RecalcularVuelto();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            lblResultado.Text = "—";
            lblResultado.ForeColor = SystemColors.ControlText;
            lblVuelto.Text = "Vuelto: —";
            _facturaActual = null;

            var mesaId = (long)nudMesaId.Value;
            if (mesaId <= 0)
            {
                MessageBox.Show("Mesa inválida.", "Cobranza", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var f = _bll.UltimaFacturaEmitidaPorMesa(mesaId);
            if (f == null)
            {
                lblFactura.Text = "No hay facturas EMITIDAS para esta mesa.";
                return;
            }

            _facturaActual = f;
            lblFactura.Text = $"Factura: {f}  |  Total: {f.ImporteTotal:n2}";
            nudImporte.Value = f.ImporteTotal; // valor sugerido
            RecalcularVuelto();
        }
        private void RecalcularVuelto()
        {
            if (_facturaActual == null) { lblVuelto.Text = "Vuelto: —"; return; }

            if (rdoEfectivo.Checked)
            {
                var vuelto = nudImporte.Value - _facturaActual.ImporteTotal;
                lblVuelto.Text = $"Vuelto: {Math.Max(0, vuelto):n2}";
            }
            else
            {
                lblVuelto.Text = "Vuelto: —";
            }
        }

        private void btnCobrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_facturaActual == null)
                    throw new InvalidOperationException("Primero buscá la factura de la mesa.");

                string medio = rdoEfectivo.Checked ? "Efectivo" :
                               rdoTarjeta.Checked ? "Tarjeta" :
                               rdoQR.Checked ? "QR" : "";

                var importe = nudImporte.Value;
                var aut = txtAut.Text?.Trim();

                var pago = _bll.RegistrarPago(_facturaActual.Id, medio, importe, aut);

                lblResultado.Text = $"Pago OK ({pago.Medio})  Importe:{pago.Monto:n2}  Vuelto:{pago.Vuelto:n2}";
                lblResultado.ForeColor = Color.ForestGreen;

                // “Consumimos” la factura para evitar doble cobro
                _facturaActual = null;
                lblFactura.Text = "—";
                nudImporte.Value = 0;
                txtAut.Clear();
                lblVuelto.Text = "Vuelto: —";
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
                lblResultado.ForeColor = Color.Firebrick;
                MessageBox.Show(ex.Message, "Cobranza", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nudImporte_ValueChanged(object sender, EventArgs e)
        {
            RecalcularVuelto();
        }

        private void FrmCobroMesa_Load(object sender, EventArgs e)
        {
            CargarMesasPendientes();
        }

        private void btnRefrescarMesas_Click(object sender, EventArgs e)
        {
            CargarMesasPendientes();
        }
        private void CargarMesasPendientes()
        {
            try
            {
                var bc = new BLLCobranza();
                var mesas = bc.MesasPendientes(); // List<long>
                lstMesasPendientes.DataSource = mesas;
            }
            catch
            {
                lstMesasPendientes.DataSource = null;
            }
        }

        private void lstMesasPendientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstMesasPendientes.SelectedItem == null) return;

            long mesa;
            if (lstMesasPendientes.SelectedItem is long m) mesa = m;
            else if (!long.TryParse(lstMesasPendientes.SelectedItem.ToString(), out mesa)) return;

            nudMesaId.Value = Math.Max(nudMesaId.Minimum, Math.Min(nudMesaId.Maximum, mesa));
            btnBuscar.PerformClick(); // reutilizás tu lógica de búsqueda
        }
    }
}
