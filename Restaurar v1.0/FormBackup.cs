using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Backup;

namespace Restaurar_v1._0
{
    public partial class FormBackup : Form
    {
        private readonly GestorBD _gestor = new GestorBD();
        public FormBackup()
        {
            InitializeComponent();
        }

        private void FormBackup_Load(object sender, EventArgs e)
        {
            CargarLista();
        }
        private void CargarLista()
        {
            try
            {
                lstBackups.Items.Clear();
                var items = _gestor.ListarBackups();
                foreach (var it in items) lstBackups.Items.Add(it);

                btnRestore.Enabled = lstBackups.Items.Count > 0;
                if (lstBackups.Items.Count > 0) lstBackups.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarLista();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                _gestor.CrearBackUp();
                MessageBox.Show("Backup creado correctamente.", "Backup",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Backup", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (lstBackups.SelectedItem == null)
            {
                MessageBox.Show("Seleccioná un backup.", "Restore",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nombre = lstBackups.SelectedItem.ToString();
            if (MessageBox.Show($"¿Restaurar '{nombre}'?\nSe sobrescribirá BD\\BD.xml y la aplicación se reiniciará.",
                "Confirmar restore", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                _gestor.CrearRestore(nombre);
                MessageBox.Show("Restore completado. La aplicación se reiniciará.", "Restore",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAbrirCarpeta_Click(object sender, EventArgs e)
        {
            var ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backup");
            if (!Directory.Exists(ruta)) Directory.CreateDirectory(ruta);
            try { Process.Start("explorer.exe", ruta); }
            catch { MessageBox.Show(ruta, "Carpeta de Backups"); }
        }

        private void lstBackups_DoubleClick(object sender, EventArgs e)
        {
            if (btnRestore.Enabled) btnRestore.PerformClick();

        }
    }
}
