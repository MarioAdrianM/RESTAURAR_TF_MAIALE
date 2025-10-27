using BE;
using BLL_Negocio;
using System;
using System.Windows.Forms;

namespace Restaurar_v1._0
{
    public partial class FormLogin : Form
    {
        private readonly BLLUsuario bll = new BLLUsuario();

        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string usuario = txtUsuario.Text.Trim();
                string password = txtPassword.Text;

                if (usuario.Length == 0) throw new Exception("Ingrese el usuario.");
                if (password.Length == 0) throw new Exception("Ingrese el password.");

                // 1) Autenticar
                BEUsuario u = bll.Autenticar(usuario, password);

                // 2) Chequeos
                if (!u.Activo) throw new Exception("El usuario está inactivo.");
                if (u.Bloqueado) throw new Exception("El usuario está bloqueado.");

                // 3) Cambio obligatorio
                if (u.DebeCambiarPassword)
                {
                    bool omitirActual = false;
                    try { omitirActual = bll.DebeOmitirActualEnCambio(u); } catch { }

                    using (var f = new frmCambiarPassword(u, omitirActual))
                    {
                        var r = f.ShowDialog(this);
                        if (r != DialogResult.OK)
                        {
                            MessageBox.Show("Debés cambiar la contraseña para continuar.",
                                "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }

                    MessageBox.Show("Contraseña actualizada. Iniciá sesión nuevamente con tu nueva clave.",
                        "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                // 4) Guardar sesión con jerarquía
                Seguridad.Sesion.UsuarioActual = bll.ListarObjetoJerarquico(u);

                // 5) Abrir menú **pasando el usuario**
                var frm = new FormMenu(Seguridad.Sesion.UsuarioActual);
                this.Hide();

                frm.FormClosed += (s, args) =>
                {
                    if (frm.LogoutRequested)
                    {
                        Seguridad.Sesion.UsuarioActual = null;
                        txtPassword.Clear();
                        txtUsuario.Clear();
                        this.Show();
                        this.Activate();
                        txtUsuario.Focus();
                    }
                    else
                    {
                        this.Close();
                    }
                };

                frm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
