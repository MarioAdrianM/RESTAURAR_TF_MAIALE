using System;
using System.Drawing;
using System.Windows.Forms;
using BLL_Negocio;

namespace Restaurar_v1._0
{
    public partial class FrmFactura : Form
    {
        private readonly BLLFactura _bllFactura = new BLLFactura();

        public FrmFactura()
        {
            InitializeComponent();
            cboTipo.Items.AddRange(new object[] { "B", "C", "X" });
            cboTipo.SelectedIndex = 0;
        }

        private void FrmFactura_Load(object sender, EventArgs e)
        {

        }

        private void btnEmitir_Click(object sender, EventArgs e)
        {
            try
            {
                var mesaId = (long)nudMesaId.Value;
                var total = nudTotal.Value;
                var pv = (int)nudPV.Value;
                var tipo = (cboTipo.SelectedItem?.ToString() ?? "B").Trim().ToUpperInvariant();

                var fac = _bllFactura.Emitir(mesaId, total, pv, tipo);
                Restaurar_v1_0.Reportes.PdfFactura.Generar(fac);


                lblResultado.Text = $"Emitida: {fac}  |  Total: {fac.ImporteTotal:n2}";
                lblResultado.ForeColor = Color.ForestGreen;
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
                lblResultado.ForeColor = Color.Firebrick;
                MessageBox.Show(ex.Message, "Facturación", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
