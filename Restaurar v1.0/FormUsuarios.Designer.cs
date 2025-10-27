namespace Restaurar_v1._0
{
    partial class FormUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.GroupBox grpEdicion;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.CheckBox chkActivo;
        private System.Windows.Forms.CheckBox chkBloqueado;
        private System.Windows.Forms.CheckBox chkDebeCambiar;
        private System.Windows.Forms.CheckBox chkCambiarPass;
        private System.Windows.Forms.Label lblPassNueva;
        private System.Windows.Forms.TextBox txtPassNueva;
        private System.Windows.Forms.Label lblPassRepetir;
        private System.Windows.Forms.TextBox txtPassRepetir;

        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnDesbloquear;
        private System.Windows.Forms.Button btnCerrar;

        private System.Windows.Forms.DataGridView dgvUsuarios;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpEdicion = new System.Windows.Forms.GroupBox();
            this.lblId = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblApellido = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.chkActivo = new System.Windows.Forms.CheckBox();
            this.chkBloqueado = new System.Windows.Forms.CheckBox();
            this.chkDebeCambiar = new System.Windows.Forms.CheckBox();
            this.chkCambiarPass = new System.Windows.Forms.CheckBox();
            this.lblPassNueva = new System.Windows.Forms.Label();
            this.txtPassNueva = new System.Windows.Forms.TextBox();
            this.lblPassRepetir = new System.Windows.Forms.Label();
            this.txtPassRepetir = new System.Windows.Forms.TextBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnDesbloquear = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.grpEdicion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.SuspendLayout();
            // 
            // grpEdicion
            // 
            this.grpEdicion.Controls.Add(this.lblId);
            this.grpEdicion.Controls.Add(this.txtId);
            this.grpEdicion.Controls.Add(this.lblUsuario);
            this.grpEdicion.Controls.Add(this.txtUsuario);
            this.grpEdicion.Controls.Add(this.lblNombre);
            this.grpEdicion.Controls.Add(this.txtNombre);
            this.grpEdicion.Controls.Add(this.lblApellido);
            this.grpEdicion.Controls.Add(this.txtApellido);
            this.grpEdicion.Controls.Add(this.chkActivo);
            this.grpEdicion.Controls.Add(this.chkBloqueado);
            this.grpEdicion.Controls.Add(this.chkDebeCambiar);
            this.grpEdicion.Controls.Add(this.chkCambiarPass);
            this.grpEdicion.Controls.Add(this.lblPassNueva);
            this.grpEdicion.Controls.Add(this.txtPassNueva);
            this.grpEdicion.Controls.Add(this.lblPassRepetir);
            this.grpEdicion.Controls.Add(this.txtPassRepetir);
            this.grpEdicion.Controls.Add(this.btnNuevo);
            this.grpEdicion.Controls.Add(this.btnGuardar);
            this.grpEdicion.Controls.Add(this.btnEliminar);
            this.grpEdicion.Controls.Add(this.btnDesbloquear);
            this.grpEdicion.Controls.Add(this.btnCerrar);
            this.grpEdicion.Location = new System.Drawing.Point(12, 12);
            this.grpEdicion.Name = "grpEdicion";
            this.grpEdicion.Size = new System.Drawing.Size(860, 200);
            this.grpEdicion.TabIndex = 0;
            this.grpEdicion.TabStop = false;
            this.grpEdicion.Text = "Edición";
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(15, 28);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(19, 13);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Id:";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(60, 25);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(60, 20);
            this.txtId.TabIndex = 1;
            this.txtId.Text = "0";
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(140, 28);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 2;
            this.lblUsuario.Text = "Usuario:";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(200, 25);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(180, 20);
            this.txtUsuario.TabIndex = 3;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(400, 28);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(47, 13);
            this.lblNombre.TabIndex = 4;
            this.lblNombre.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(460, 25);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(170, 20);
            this.txtNombre.TabIndex = 5;
            // 
            // lblApellido
            // 
            this.lblApellido.AutoSize = true;
            this.lblApellido.Location = new System.Drawing.Point(650, 28);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(47, 13);
            this.lblApellido.TabIndex = 6;
            this.lblApellido.Text = "Apellido:";
            // 
            // txtApellido
            // 
            this.txtApellido.Location = new System.Drawing.Point(710, 25);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(130, 20);
            this.txtApellido.TabIndex = 7;
            // 
            // chkActivo
            // 
            this.chkActivo.AutoSize = true;
            this.chkActivo.Location = new System.Drawing.Point(18, 60);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(56, 17);
            this.chkActivo.TabIndex = 8;
            this.chkActivo.Text = "Activo";
            // 
            // chkBloqueado
            // 
            this.chkBloqueado.AutoSize = true;
            this.chkBloqueado.Location = new System.Drawing.Point(90, 60);
            this.chkBloqueado.Name = "chkBloqueado";
            this.chkBloqueado.Size = new System.Drawing.Size(77, 17);
            this.chkBloqueado.TabIndex = 9;
            this.chkBloqueado.Text = "Bloqueado";
            // 
            // chkDebeCambiar
            // 
            this.chkDebeCambiar.AutoSize = true;
            this.chkDebeCambiar.Location = new System.Drawing.Point(190, 60);
            this.chkDebeCambiar.Name = "chkDebeCambiar";
            this.chkDebeCambiar.Size = new System.Drawing.Size(163, 17);
            this.chkDebeCambiar.TabIndex = 10;
            this.chkDebeCambiar.Text = "Forzar cambio de contraseña";
            // 
            // chkCambiarPass
            // 
            this.chkCambiarPass.AutoSize = true;
            this.chkCambiarPass.Location = new System.Drawing.Point(18, 95);
            this.chkCambiarPass.Name = "chkCambiarPass";
            this.chkCambiarPass.Size = new System.Drawing.Size(120, 17);
            this.chkCambiarPass.TabIndex = 11;
            this.chkCambiarPass.Text = "Cambiar contraseña";
            // 
            // lblPassNueva
            // 
            this.lblPassNueva.AutoSize = true;
            this.lblPassNueva.Location = new System.Drawing.Point(40, 125);
            this.lblPassNueva.Name = "lblPassNueva";
            this.lblPassNueva.Size = new System.Drawing.Size(42, 13);
            this.lblPassNueva.TabIndex = 12;
            this.lblPassNueva.Text = "Nueva:";
            // 
            // txtPassNueva
            // 
            this.txtPassNueva.Enabled = false;
            this.txtPassNueva.Location = new System.Drawing.Point(90, 122);
            this.txtPassNueva.Name = "txtPassNueva";
            this.txtPassNueva.Size = new System.Drawing.Size(170, 20);
            this.txtPassNueva.TabIndex = 13;
            this.txtPassNueva.UseSystemPasswordChar = true;
            // 
            // lblPassRepetir
            // 
            this.lblPassRepetir.AutoSize = true;
            this.lblPassRepetir.Location = new System.Drawing.Point(280, 125);
            this.lblPassRepetir.Name = "lblPassRepetir";
            this.lblPassRepetir.Size = new System.Drawing.Size(44, 13);
            this.lblPassRepetir.TabIndex = 14;
            this.lblPassRepetir.Text = "Repetir:";
            // 
            // txtPassRepetir
            // 
            this.txtPassRepetir.Enabled = false;
            this.txtPassRepetir.Location = new System.Drawing.Point(335, 122);
            this.txtPassRepetir.Name = "txtPassRepetir";
            this.txtPassRepetir.Size = new System.Drawing.Size(170, 20);
            this.txtPassRepetir.TabIndex = 15;
            this.txtPassRepetir.UseSystemPasswordChar = true;
            // 
            // btnNuevo
            // 
            this.btnNuevo.Location = new System.Drawing.Point(540, 115);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(80, 30);
            this.btnNuevo.TabIndex = 16;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(630, 115);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(80, 30);
            this.btnGuardar.TabIndex = 17;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(720, 115);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(80, 30);
            this.btnEliminar.TabIndex = 18;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnDesbloquear
            // 
            this.btnDesbloquear.Location = new System.Drawing.Point(540, 155);
            this.btnDesbloquear.Name = "btnDesbloquear";
            this.btnDesbloquear.Size = new System.Drawing.Size(100, 30);
            this.btnDesbloquear.TabIndex = 19;
            this.btnDesbloquear.Text = "Desbloquear";
            this.btnDesbloquear.Click += new System.EventHandler(this.btnDesbloquear_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(720, 155);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(80, 30);
            this.btnCerrar.TabIndex = 20;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // dgvUsuarios
            // 
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.AllowUserToDeleteRows = false;
            this.dgvUsuarios.Location = new System.Drawing.Point(12, 225);
            this.dgvUsuarios.MultiSelect = false;
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Size = new System.Drawing.Size(860, 300);
            this.dgvUsuarios.TabIndex = 1;
            this.dgvUsuarios.SelectionChanged += new System.EventHandler(this.dgvUsuarios_SelectionChanged);
            // 
            // FormUsuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 541);
            this.Controls.Add(this.grpEdicion);
            this.Controls.Add(this.dgvUsuarios);
            this.Name = "FormUsuarios";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABM de Usuarios";
            this.Load += new System.EventHandler(this.FormUsuarios_Load);
            this.grpEdicion.ResumeLayout(false);
            this.grpEdicion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
