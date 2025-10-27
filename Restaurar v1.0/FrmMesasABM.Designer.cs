namespace Restaurar_v1._0
{
    partial class FrmMesasABM
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvMesas = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboSector = new System.Windows.Forms.ComboBox();
            this.lblEstadoValor = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnLimpiarCampos = new System.Windows.Forms.Button();
            this.btnEliminarMesa = new System.Windows.Forms.Button();
            this.btnModificarMesa = new System.Windows.Forms.Button();
            this.btnCrearMesa = new System.Windows.Forms.Button();
            this.txtObservaciones = new System.Windows.Forms.TextBox();
            this.lblObservaciones = new System.Windows.Forms.Label();
            this.chkHabilitada = new System.Windows.Forms.CheckBox();
            this.lblEstadoTitulo = new System.Windows.Forms.Label();
            this.lblSector = new System.Windows.Forms.Label();
            this.txtCapacidad = new System.Windows.Forms.TextBox();
            this.lblCapacidad = new System.Windows.Forms.Label();
            this.txtNumero = new System.Windows.Forms.TextBox();
            this.lblNumero = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.lblId = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMesas
            // 
            this.dgvMesas.AllowUserToAddRows = false;
            this.dgvMesas.AllowUserToDeleteRows = false;
            this.dgvMesas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMesas.Location = new System.Drawing.Point(32, 25);
            this.dgvMesas.MultiSelect = false;
            this.dgvMesas.Name = "dgvMesas";
            this.dgvMesas.ReadOnly = true;
            this.dgvMesas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMesas.Size = new System.Drawing.Size(527, 217);
            this.dgvMesas.TabIndex = 0;
            this.dgvMesas.SelectionChanged += new System.EventHandler(this.dgvMesas_SelectionChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboSector);
            this.panel1.Controls.Add(this.lblEstadoValor);
            this.panel1.Controls.Add(this.btnCerrar);
            this.panel1.Controls.Add(this.btnLimpiarCampos);
            this.panel1.Controls.Add(this.btnEliminarMesa);
            this.panel1.Controls.Add(this.btnModificarMesa);
            this.panel1.Controls.Add(this.btnCrearMesa);
            this.panel1.Controls.Add(this.txtObservaciones);
            this.panel1.Controls.Add(this.lblObservaciones);
            this.panel1.Controls.Add(this.chkHabilitada);
            this.panel1.Controls.Add(this.lblEstadoTitulo);
            this.panel1.Controls.Add(this.lblSector);
            this.panel1.Controls.Add(this.txtCapacidad);
            this.panel1.Controls.Add(this.lblCapacidad);
            this.panel1.Controls.Add(this.txtNumero);
            this.panel1.Controls.Add(this.lblNumero);
            this.panel1.Controls.Add(this.txtId);
            this.panel1.Controls.Add(this.lblId);
            this.panel1.Location = new System.Drawing.Point(32, 248);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(527, 175);
            this.panel1.TabIndex = 1;
            // 
            // cboSector
            // 
            this.cboSector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSector.FormattingEnabled = true;
            this.cboSector.Location = new System.Drawing.Point(234, 44);
            this.cboSector.Name = "cboSector";
            this.cboSector.Size = new System.Drawing.Size(121, 21);
            this.cboSector.TabIndex = 19;
            // 
            // lblEstadoValor
            // 
            this.lblEstadoValor.AutoSize = true;
            this.lblEstadoValor.Location = new System.Drawing.Point(60, 74);
            this.lblEstadoValor.Name = "lblEstadoValor";
            this.lblEstadoValor.Size = new System.Drawing.Size(10, 13);
            this.lblEstadoValor.TabIndex = 18;
            this.lblEstadoValor.Text = "-";
            // 
            // btnCerrar
            // 
            this.btnCerrar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCerrar.Location = new System.Drawing.Point(436, 141);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(75, 23);
            this.btnCerrar.TabIndex = 17;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnLimpiarCampos
            // 
            this.btnLimpiarCampos.Location = new System.Drawing.Point(96, 139);
            this.btnLimpiarCampos.Name = "btnLimpiarCampos";
            this.btnLimpiarCampos.Size = new System.Drawing.Size(105, 23);
            this.btnLimpiarCampos.TabIndex = 16;
            this.btnLimpiarCampos.Text = "Limpiar campos";
            this.btnLimpiarCampos.UseVisualStyleBackColor = true;
            this.btnLimpiarCampos.Click += new System.EventHandler(this.btnLimpiarCampos_Click);
            // 
            // btnEliminarMesa
            // 
            this.btnEliminarMesa.Location = new System.Drawing.Point(271, 142);
            this.btnEliminarMesa.Name = "btnEliminarMesa";
            this.btnEliminarMesa.Size = new System.Drawing.Size(107, 23);
            this.btnEliminarMesa.TabIndex = 15;
            this.btnEliminarMesa.Text = "Eliminar mesa";
            this.btnEliminarMesa.UseVisualStyleBackColor = true;
            this.btnEliminarMesa.Click += new System.EventHandler(this.btnEliminarMesa_Click);
            // 
            // btnModificarMesa
            // 
            this.btnModificarMesa.Location = new System.Drawing.Point(271, 113);
            this.btnModificarMesa.Name = "btnModificarMesa";
            this.btnModificarMesa.Size = new System.Drawing.Size(107, 23);
            this.btnModificarMesa.TabIndex = 14;
            this.btnModificarMesa.Text = "Modificar mesa";
            this.btnModificarMesa.UseVisualStyleBackColor = true;
            this.btnModificarMesa.Click += new System.EventHandler(this.btnModificarMesa_Click);
            // 
            // btnCrearMesa
            // 
            this.btnCrearMesa.Location = new System.Drawing.Point(96, 113);
            this.btnCrearMesa.Name = "btnCrearMesa";
            this.btnCrearMesa.Size = new System.Drawing.Size(105, 23);
            this.btnCrearMesa.TabIndex = 13;
            this.btnCrearMesa.Text = "Crear nueva mesa";
            this.btnCrearMesa.UseVisualStyleBackColor = true;
            this.btnCrearMesa.Click += new System.EventHandler(this.btnCrearMesa_Click);
            // 
            // txtObservaciones
            // 
            this.txtObservaciones.Location = new System.Drawing.Point(351, 74);
            this.txtObservaciones.Multiline = true;
            this.txtObservaciones.Name = "txtObservaciones";
            this.txtObservaciones.Size = new System.Drawing.Size(100, 24);
            this.txtObservaciones.TabIndex = 12;
            // 
            // lblObservaciones
            // 
            this.lblObservaciones.AutoSize = true;
            this.lblObservaciones.Location = new System.Drawing.Point(255, 74);
            this.lblObservaciones.Name = "lblObservaciones";
            this.lblObservaciones.Size = new System.Drawing.Size(78, 13);
            this.lblObservaciones.TabIndex = 11;
            this.lblObservaciones.Text = "Observaciones";
            // 
            // chkHabilitada
            // 
            this.chkHabilitada.AutoSize = true;
            this.chkHabilitada.Location = new System.Drawing.Point(400, 27);
            this.chkHabilitada.Name = "chkHabilitada";
            this.chkHabilitada.Size = new System.Drawing.Size(73, 17);
            this.chkHabilitada.TabIndex = 10;
            this.chkHabilitada.Text = "Habilitada";
            this.chkHabilitada.UseVisualStyleBackColor = true;
            // 
            // lblEstadoTitulo
            // 
            this.lblEstadoTitulo.AutoSize = true;
            this.lblEstadoTitulo.Location = new System.Drawing.Point(14, 74);
            this.lblEstadoTitulo.Name = "lblEstadoTitulo";
            this.lblEstadoTitulo.Size = new System.Drawing.Size(40, 13);
            this.lblEstadoTitulo.TabIndex = 8;
            this.lblEstadoTitulo.Text = "Estado";
            // 
            // lblSector
            // 
            this.lblSector.AutoSize = true;
            this.lblSector.Location = new System.Drawing.Point(189, 44);
            this.lblSector.Name = "lblSector";
            this.lblSector.Size = new System.Drawing.Size(38, 13);
            this.lblSector.TabIndex = 6;
            this.lblSector.Text = "Sector";
            // 
            // txtCapacidad
            // 
            this.txtCapacidad.Location = new System.Drawing.Point(60, 44);
            this.txtCapacidad.Name = "txtCapacidad";
            this.txtCapacidad.Size = new System.Drawing.Size(100, 20);
            this.txtCapacidad.TabIndex = 5;
            this.txtCapacidad.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloNumeros_KeyPress);
            // 
            // lblCapacidad
            // 
            this.lblCapacidad.AutoSize = true;
            this.lblCapacidad.Location = new System.Drawing.Point(3, 44);
            this.lblCapacidad.Name = "lblCapacidad";
            this.lblCapacidad.Size = new System.Drawing.Size(58, 13);
            this.lblCapacidad.TabIndex = 4;
            this.lblCapacidad.Text = "Capacidad";
            // 
            // txtNumero
            // 
            this.txtNumero.Location = new System.Drawing.Point(233, 12);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.Size = new System.Drawing.Size(100, 20);
            this.txtNumero.TabIndex = 3;
            this.txtNumero.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SoloNumeros_KeyPress);
            // 
            // lblNumero
            // 
            this.lblNumero.AutoSize = true;
            this.lblNumero.Location = new System.Drawing.Point(183, 15);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(44, 13);
            this.lblNumero.TabIndex = 2;
            this.lblNumero.Text = "Número";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(60, 12);
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(100, 20);
            this.txtId.TabIndex = 1;
            // 
            // lblId
            // 
            this.lblId.AutoSize = true;
            this.lblId.Location = new System.Drawing.Point(38, 15);
            this.lblId.Name = "lblId";
            this.lblId.Size = new System.Drawing.Size(16, 13);
            this.lblId.TabIndex = 0;
            this.lblId.Text = "Id";
            // 
            // FrmMesasABM
            // 
            this.AcceptButton = this.btnModificarMesa;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCerrar;
            this.ClientSize = new System.Drawing.Size(593, 424);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dgvMesas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmMesasABM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABM de Mesas";
            this.Load += new System.EventHandler(this.FrmMesasABM_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMesas)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMesas;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblNumero;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.Label lblEstadoTitulo;
        private System.Windows.Forms.Label lblSector;
        private System.Windows.Forms.TextBox txtCapacidad;
        private System.Windows.Forms.Label lblCapacidad;
        private System.Windows.Forms.TextBox txtNumero;
        private System.Windows.Forms.TextBox txtObservaciones;
        private System.Windows.Forms.Label lblObservaciones;
        private System.Windows.Forms.CheckBox chkHabilitada;
        private System.Windows.Forms.Button btnLimpiarCampos;
        private System.Windows.Forms.Button btnEliminarMesa;
        private System.Windows.Forms.Button btnModificarMesa;
        private System.Windows.Forms.Button btnCrearMesa;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Label lblEstadoValor;
        private System.Windows.Forms.ComboBox cboSector;
    }
}