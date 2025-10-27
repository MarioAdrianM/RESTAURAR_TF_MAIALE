using System;
using System.Windows.Forms;
using BLL_Negocio;
using BE;


namespace Restaurar_v1._0
{
    public partial class frmCambiarPassword : Form
    {
        private readonly BLLUsuario bll = new BLLUsuario();
        private readonly BEUsuario usuario;
        private readonly bool omitirActual;
        public frmCambiarPassword(BEUsuario user, bool omitirActual)
        {
            InitializeComponent();
            this.usuario = user ?? throw new ArgumentNullException(nameof(user));
            this.omitirActual = omitirActual;

            // Si es primer login admin/admin oculto campo "actual"
            lblActual.Visible = !omitirActual;
            txtActual.Visible = !omitirActual;

            this.Text = $"Cambiar contraseña — {usuario.Usuario}";
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string msg;
            string actual = omitirActual ? string.Empty : txtActual.Text;

            // ⬇️ USAR el overload que recibe "omitirActual"
            bool ok = bll.CambiarPassword(usuario, actual, txtNueva.Text, txtRepetir.Text, omitirActual, out msg);

            if (ok)
            {
                MessageBox.Show("Contraseña actualizada correctamente.", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(msg, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (!omitirActual && string.IsNullOrEmpty(actual)) txtActual.Focus();
                else txtNueva.Focus();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
