using System;
using System.Drawing;
using System.Windows.Forms;
using BLL_Negocio;

namespace Restaurar_v1._0
{
    public partial class FrmAperturaCaja : Form
    {
        private readonly BLLCaja _bll = new BLLCaja();
        public FrmAperturaCaja()
        {
            InitializeComponent();
            cboTurno.Items.AddRange(new object[] { "Mañana", "Tarde", "Noche" });
            cboTurno.SelectedIndex = 2; // Noche
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                var c = _bll.Abrir(
                    punto: txtPunto.Text?.Trim(),
                    turno: cboTurno.SelectedItem?.ToString(),
                    fondoInicial: nudFondo.Value,
                    umbralDif: nudUmbral.Value
                );

                lblResultado.Text = $"Caja abierta (Id {c.Id})  Punto:{c.Punto} Turno:{c.Turno}";
                lblResultado.ForeColor = Color.ForestGreen;
            }
            catch (Exception ex)
            {
                lblResultado.Text = ex.Message;
                lblResultado.ForeColor = Color.Firebrick;
                MessageBox.Show(ex.Message, "Apertura", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmAperturaCaja_Load(object sender, EventArgs e)
        {

        }
    }
}
