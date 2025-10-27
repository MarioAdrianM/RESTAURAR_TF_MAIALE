namespace Restaurar_v1._0
{
    partial class FrmFactura
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
            this.lblPV = new System.Windows.Forms.Label();
            this.nudPV = new System.Windows.Forms.NumericUpDown();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cboTipo = new System.Windows.Forms.ComboBox();
            this.lblTotal = new System.Windows.Forms.Label();
            this.nudTotal = new System.Windows.Forms.NumericUpDown();
            this.btnEmitir = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudMesaId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMesa
            // 
            this.lblMesa.AutoSize = true;
            this.lblMesa.Location = new System.Drawing.Point(20, 20);
            this.lblMesa.Name = "lblMesa";
            this.lblMesa.Size = new System.Drawing.Size(48, 13);
            this.lblMesa.TabIndex = 0;
            this.lblMesa.Text = "Mesa Id:";
            // 
            // nudMesaId
            // 
            this.nudMesaId.Location = new System.Drawing.Point(120, 16);
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
            // lblPV
            // 
            this.lblPV.AutoSize = true;
            this.lblPV.Location = new System.Drawing.Point(20, 60);
            this.lblPV.Name = "lblPV";
            this.lblPV.Size = new System.Drawing.Size(84, 13);
            this.lblPV.TabIndex = 2;
            this.lblPV.Text = "Punto de Venta:";
            // 
            // nudPV
            // 
            this.nudPV.Location = new System.Drawing.Point(120, 56);
            this.nudPV.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudPV.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPV.Name = "nudPV";
            this.nudPV.Size = new System.Drawing.Size(120, 20);
            this.nudPV.TabIndex = 3;
            this.nudPV.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudPV.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(240, 60);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(56, 13);
            this.lblTipo.TabIndex = 4;
            this.lblTipo.Text = "Tipo Cbte:";
            // 
            // cboTipo
            // 
            this.cboTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipo.FormattingEnabled = true;
            this.cboTipo.Location = new System.Drawing.Point(310, 56);
            this.cboTipo.Name = "cboTipo";
            this.cboTipo.Size = new System.Drawing.Size(70, 21);
            this.cboTipo.TabIndex = 5;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(20, 100);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(72, 13);
            this.lblTotal.TabIndex = 6;
            this.lblTotal.Text = "Importe Total:";
            // 
            // nudTotal
            // 
            this.nudTotal.DecimalPlaces = 2;
            this.nudTotal.Location = new System.Drawing.Point(120, 96);
            this.nudTotal.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudTotal.Name = "nudTotal";
            this.nudTotal.Size = new System.Drawing.Size(120, 20);
            this.nudTotal.TabIndex = 7;
            this.nudTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnEmitir
            // 
            this.btnEmitir.Location = new System.Drawing.Point(20, 140);
            this.btnEmitir.Name = "btnEmitir";
            this.btnEmitir.Size = new System.Drawing.Size(360, 32);
            this.btnEmitir.TabIndex = 8;
            this.btnEmitir.Text = "Emitir";
            this.btnEmitir.UseVisualStyleBackColor = true;
            this.btnEmitir.Click += new System.EventHandler(this.btnEmitir_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoEllipsis = true;
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(20, 185);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(13, 13);
            this.lblResultado.TabIndex = 9;
            this.lblResultado.Text = "—";
            // 
            // FrmFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 201);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.btnEmitir);
            this.Controls.Add(this.nudTotal);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.cboTipo);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.nudPV);
            this.Controls.Add(this.lblPV);
            this.Controls.Add(this.nudMesaId);
            this.Controls.Add(this.lblMesa);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmFactura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Emitir Factura";
            this.Load += new System.EventHandler(this.FrmFactura_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudMesaId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTotal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMesa;
        private System.Windows.Forms.NumericUpDown nudMesaId;
        private System.Windows.Forms.Label lblPV;
        private System.Windows.Forms.NumericUpDown nudPV;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cboTipo;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.NumericUpDown nudTotal;
        private System.Windows.Forms.Button btnEmitir;
        private System.Windows.Forms.Label lblResultado;
    }
}