using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Restaurar_v1._0
{
    public partial class FrmCliente : Form
    {
        private readonly string _preNombre;
        private readonly string _preTelefono;
        private readonly string _preEmail;
        public string ClienteNombre { get; private set; } = "";
        public string ClienteTelefono { get; private set; } = "";
        public string ClienteEmail { get; private set; } = "";

        public FrmCliente() : this("", "", "") { }
        public FrmCliente(string nombre, string telefono, string email)
        {
            InitializeComponent();
            _preNombre = nombre ?? "";
            _preTelefono = telefono ?? "";
            _preEmail = email ?? "";
        }

        private void FrmCliente_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancelar;
            txtNombre.Text = _preNombre;
            txtTelefono.Text = _preTelefono;
            txtEmail.Text = _preEmail;
            txtNombre.Focus();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var nombre = (txtNombre.Text ?? "").Trim();
            var tel = (txtTelefono.Text ?? "").Trim();
            var mail = (txtEmail.Text ?? "").Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre es obligatorio.", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus(); return;
            }
            if (string.IsNullOrWhiteSpace(tel))
            {
                MessageBox.Show("El teléfono es obligatorio.", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTelefono.Focus(); return;
            }
            if (!string.IsNullOrWhiteSpace(mail))
            {
                // validación simple de email (opcional)
                if (!Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    MessageBox.Show("Email inválido.", "Cliente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus(); return;
                }
            }

            ClienteNombre = nombre;
            ClienteTelefono = tel;
            ClienteEmail = mail;

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
