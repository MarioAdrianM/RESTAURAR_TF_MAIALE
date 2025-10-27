namespace Restaurar_v1._0
{
    partial class FrmCobroMesa
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
            this.lblMesa = new System.Windows.Forms.Label();
            this.nudMesaId = new System.Windows.Forms.NumericUpDown();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.lblFactura = new System.Windows.Forms.Label();
            this.grpMedio = new System.Windows.Forms.GroupBox();
            this.rdoQR = new System.Windows.Forms.RadioButton();
            this.rdoTarjeta = new System.Windows.Forms.RadioButton();
            this.rdoEfectivo = new System.Windows.Forms.RadioButton();
            this.lblImporte = new System.Windows.Forms.Label();
            this.nudImporte = new System.Windows.Forms.NumericUpDown();
            this.lblAut = new System.Windows.Forms.Label();
            this.txtAut = new System.Windows.Forms.TextBox();
            this.lblVuelto = new System.Windows.Forms.Label();
            this.btnCobrar = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.lblPend = new System.Windows.Forms.Label();
            this.lstMesasPendientes = new System.Windows.Forms.ListBox();
            this.btnRefrescarMesas = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudMesaId)).BeginInit();
            this.grpMedio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImporte)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMesa
            // 
            this.lblMesa.AutoSize = true;
            this.lblMesa.Location = new System.Drawing.Point(51, 299);
            this.lblMesa.Name = "lblMesa";
            this.lblMesa.Size = new System.Drawing.Size(48, 13);
            this.lblMesa.TabIndex = 0;
            this.lblMesa.Text = "Mesa Id:";
            // 
            // nudMesaId
            // 
            this.nudMesaId.Location = new System.Drawing.Point(151, 295);
            this.nudMesaId.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudMesaId.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMesaId.Name = "nudMesaId";
            this.nudMesaId.Size = new System.Drawing.Size(120, 20);
            this.nudMesaId.TabIndex = 1;
            this.nudMesaId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudMesaId.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnBuscar
            // 
            this.btnBuscar.Location = new System.Drawing.Point(271, 294);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(120, 26);
            this.btnBuscar.TabIndex = 2;
            this.btnBuscar.Text = "Buscar factura";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // lblFactura
            // 
            this.lblFactura.AutoEllipsis = true;
            this.lblFactura.AutoSize = true;
            this.lblFactura.Location = new System.Drawing.Point(51, 329);
            this.lblFactura.Name = "lblFactura";
            this.lblFactura.Size = new System.Drawing.Size(13, 13);
            this.lblFactura.TabIndex = 3;
            this.lblFactura.Text = "—";
            // 
            // grpMedio
            // 
            this.grpMedio.Controls.Add(this.rdoQR);
            this.grpMedio.Controls.Add(this.rdoTarjeta);
            this.grpMedio.Controls.Add(this.rdoEfectivo);
            this.grpMedio.Location = new System.Drawing.Point(51, 359);
            this.grpMedio.Name = "grpMedio";
            this.grpMedio.Size = new System.Drawing.Size(280, 80);
            this.grpMedio.TabIndex = 4;
            this.grpMedio.TabStop = false;
            this.grpMedio.Text = "Medio de pago";
            // 
            // rdoQR
            // 
            this.rdoQR.AutoSize = true;
            this.rdoQR.Location = new System.Drawing.Point(185, 30);
            this.rdoQR.Name = "rdoQR";
            this.rdoQR.Size = new System.Drawing.Size(41, 17);
            this.rdoQR.TabIndex = 2;
            this.rdoQR.TabStop = true;
            this.rdoQR.Text = "QR";
            this.rdoQR.UseVisualStyleBackColor = true;
            this.rdoQR.CheckedChanged += new System.EventHandler(this.Medio_CheckedChanged);
            // 
            // rdoTarjeta
            // 
            this.rdoTarjeta.AutoSize = true;
            this.rdoTarjeta.Location = new System.Drawing.Point(100, 30);
            this.rdoTarjeta.Name = "rdoTarjeta";
            this.rdoTarjeta.Size = new System.Drawing.Size(58, 17);
            this.rdoTarjeta.TabIndex = 1;
            this.rdoTarjeta.TabStop = true;
            this.rdoTarjeta.Text = "Tarjeta";
            this.rdoTarjeta.UseVisualStyleBackColor = true;
            this.rdoTarjeta.CheckedChanged += new System.EventHandler(this.Medio_CheckedChanged);
            // 
            // rdoEfectivo
            // 
            this.rdoEfectivo.AutoSize = true;
            this.rdoEfectivo.Checked = true;
            this.rdoEfectivo.Location = new System.Drawing.Point(15, 30);
            this.rdoEfectivo.Name = "rdoEfectivo";
            this.rdoEfectivo.Size = new System.Drawing.Size(64, 17);
            this.rdoEfectivo.TabIndex = 0;
            this.rdoEfectivo.TabStop = true;
            this.rdoEfectivo.Text = "Efectivo";
            this.rdoEfectivo.UseVisualStyleBackColor = true;
            this.rdoEfectivo.CheckedChanged += new System.EventHandler(this.Medio_CheckedChanged);
            // 
            // lblImporte
            // 
            this.lblImporte.AutoSize = true;
            this.lblImporte.Location = new System.Drawing.Point(351, 369);
            this.lblImporte.Name = "lblImporte";
            this.lblImporte.Size = new System.Drawing.Size(85, 13);
            this.lblImporte.TabIndex = 5;
            this.lblImporte.Text = "Importe recibido:";
            // 
            // nudImporte
            // 
            this.nudImporte.DecimalPlaces = 2;
            this.nudImporte.Location = new System.Drawing.Point(351, 389);
            this.nudImporte.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudImporte.Name = "nudImporte";
            this.nudImporte.Size = new System.Drawing.Size(160, 20);
            this.nudImporte.TabIndex = 6;
            this.nudImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudImporte.ValueChanged += new System.EventHandler(this.nudImporte_ValueChanged);
            // 
            // lblAut
            // 
            this.lblAut.AutoSize = true;
            this.lblAut.Location = new System.Drawing.Point(351, 424);
            this.lblAut.Name = "lblAut";
            this.lblAut.Size = new System.Drawing.Size(106, 13);
            this.lblAut.TabIndex = 7;
            this.lblAut.Text = "Autorización/Código:";
            // 
            // txtAut
            // 
            this.txtAut.Enabled = false;
            this.txtAut.Location = new System.Drawing.Point(351, 444);
            this.txtAut.Name = "txtAut";
            this.txtAut.Size = new System.Drawing.Size(160, 20);
            this.txtAut.TabIndex = 8;
            // 
            // lblVuelto
            // 
            this.lblVuelto.AutoSize = true;
            this.lblVuelto.Location = new System.Drawing.Point(351, 479);
            this.lblVuelto.Name = "lblVuelto";
            this.lblVuelto.Size = new System.Drawing.Size(49, 13);
            this.lblVuelto.TabIndex = 9;
            this.lblVuelto.Text = "Vuelto: —";
            // 
            // btnCobrar
            // 
            this.btnCobrar.Location = new System.Drawing.Point(51, 479);
            this.btnCobrar.Name = "btnCobrar";
            this.btnCobrar.Size = new System.Drawing.Size(280, 32);
            this.btnCobrar.TabIndex = 10;
            this.btnCobrar.Text = "Cobrar";
            this.btnCobrar.UseVisualStyleBackColor = true;
            this.btnCobrar.Click += new System.EventHandler(this.btnCobrar_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoEllipsis = true;
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(51, 524);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(13, 13);
            this.lblResultado.TabIndex = 11;
            this.lblResultado.Text = "—";
            // 
            // lblPend
            // 
            this.lblPend.AutoSize = true;
            this.lblPend.Location = new System.Drawing.Point(13, 13);
            this.lblPend.Name = "lblPend";
            this.lblPend.Size = new System.Drawing.Size(87, 13);
            this.lblPend.TabIndex = 12;
            this.lblPend.Text = "Mesas sin cobrar";
            // 
            // lstMesasPendientes
            // 
            this.lstMesasPendientes.FormattingEnabled = true;
            this.lstMesasPendientes.Location = new System.Drawing.Point(12, 29);
            this.lstMesasPendientes.Name = "lstMesasPendientes";
            this.lstMesasPendientes.Size = new System.Drawing.Size(150, 147);
            this.lstMesasPendientes.TabIndex = 13;
            this.lstMesasPendientes.SelectedIndexChanged += new System.EventHandler(this.lstMesasPendientes_SelectedIndexChanged);
            // 
            // btnRefrescarMesas
            // 
            this.btnRefrescarMesas.Location = new System.Drawing.Point(16, 182);
            this.btnRefrescarMesas.Name = "btnRefrescarMesas";
            this.btnRefrescarMesas.Size = new System.Drawing.Size(75, 23);
            this.btnRefrescarMesas.TabIndex = 14;
            this.btnRefrescarMesas.Text = "Actualizar";
            this.btnRefrescarMesas.UseVisualStyleBackColor = true;
            this.btnRefrescarMesas.Click += new System.EventHandler(this.btnRefrescarMesas_Click);
            // 
            // FrmCobroMesa
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 550);
            this.Controls.Add(this.btnRefrescarMesas);
            this.Controls.Add(this.lstMesasPendientes);
            this.Controls.Add(this.lblPend);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.btnCobrar);
            this.Controls.Add(this.lblVuelto);
            this.Controls.Add(this.txtAut);
            this.Controls.Add(this.lblAut);
            this.Controls.Add(this.nudImporte);
            this.Controls.Add(this.lblImporte);
            this.Controls.Add(this.grpMedio);
            this.Controls.Add(this.lblFactura);
            this.Controls.Add(this.btnBuscar);
            this.Controls.Add(this.nudMesaId);
            this.Controls.Add(this.lblMesa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCobroMesa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cobrar en mesa";
            this.Load += new System.EventHandler(this.FrmCobroMesa_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudMesaId)).EndInit();
            this.grpMedio.ResumeLayout(false);
            this.grpMedio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudImporte)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMesa;
        private System.Windows.Forms.NumericUpDown nudMesaId;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.Label lblFactura;
        private System.Windows.Forms.GroupBox grpMedio;
        private System.Windows.Forms.RadioButton rdoTarjeta;
        private System.Windows.Forms.RadioButton rdoEfectivo;
        private System.Windows.Forms.RadioButton rdoQR;
        private System.Windows.Forms.Label lblImporte;
        private System.Windows.Forms.NumericUpDown nudImporte;
        private System.Windows.Forms.Label lblAut;
        private System.Windows.Forms.TextBox txtAut;
        private System.Windows.Forms.Label lblVuelto;
        private System.Windows.Forms.Button btnCobrar;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Label lblPend;
        private System.Windows.Forms.ListBox lstMesasPendientes;
        private System.Windows.Forms.Button btnRefrescarMesas;
    }
}