namespace Restaurar_v1._0
{
    partial class FrmRendicionValores
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblCaja = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMozo = new System.Windows.Forms.TextBox();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblEspQR = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblEspTj = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblEspEf = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nudEntQR = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.nudEntTj = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.nudEntEf = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtObs = new System.Windows.Forms.TextBox();
            this.btnRendir = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.lblSelCaja = new System.Windows.Forms.Label();
            this.cboCajaAbierta = new System.Windows.Forms.ComboBox();
            this.btnRefrescarCajas = new System.Windows.Forms.Button();
            this.dgvMozos = new System.Windows.Forms.DataGridView();
            this.colMozo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntQR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntTj)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntEf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMozos)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCaja
            // 
            this.lblCaja.AutoEllipsis = true;
            this.lblCaja.AutoSize = true;
            this.lblCaja.Location = new System.Drawing.Point(26, 51);
            this.lblCaja.Name = "lblCaja";
            this.lblCaja.Size = new System.Drawing.Size(13, 13);
            this.lblCaja.TabIndex = 5;
            this.lblCaja.Text = "—";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Mozo (usuario):";
            // 
            // txtMozo
            // 
            this.txtMozo.Location = new System.Drawing.Point(121, 184);
            this.txtMozo.Name = "txtMozo";
            this.txtMozo.Size = new System.Drawing.Size(100, 20);
            this.txtMozo.TabIndex = 8;
            // 
            // btnCalcular
            // 
            this.btnCalcular.Location = new System.Drawing.Point(288, 180);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(120, 26);
            this.btnCalcular.TabIndex = 9;
            this.btnCalcular.Text = "Calcular esperado";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblEspQR);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblEspTj);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblEspEf);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(20, 218);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 100);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Esperado";
            // 
            // lblEspQR
            // 
            this.lblEspQR.AutoSize = true;
            this.lblEspQR.Location = new System.Drawing.Point(70, 72);
            this.lblEspQR.Name = "lblEspQR";
            this.lblEspQR.Size = new System.Drawing.Size(28, 13);
            this.lblEspQR.TabIndex = 15;
            this.lblEspQR.Text = "0,00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "QR:";
            // 
            // lblEspTj
            // 
            this.lblEspTj.AutoSize = true;
            this.lblEspTj.Location = new System.Drawing.Point(70, 49);
            this.lblEspTj.Name = "lblEspTj";
            this.lblEspTj.Size = new System.Drawing.Size(28, 13);
            this.lblEspTj.TabIndex = 13;
            this.lblEspTj.Text = "0,00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Tarjeta";
            // 
            // lblEspEf
            // 
            this.lblEspEf.AutoSize = true;
            this.lblEspEf.Location = new System.Drawing.Point(70, 26);
            this.lblEspEf.Name = "lblEspEf";
            this.lblEspEf.Size = new System.Drawing.Size(28, 13);
            this.lblEspEf.TabIndex = 11;
            this.lblEspEf.Text = "0,00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Efectivo";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudEntQR);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.nudEntTj);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.nudEntEf);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Location = new System.Drawing.Point(280, 218);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Entregado";
            // 
            // nudEntQR
            // 
            this.nudEntQR.DecimalPlaces = 2;
            this.nudEntQR.Location = new System.Drawing.Point(61, 76);
            this.nudEntQR.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudEntQR.Name = "nudEntQR";
            this.nudEntQR.Size = new System.Drawing.Size(120, 20);
            this.nudEntQR.TabIndex = 5;
            this.nudEntQR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(23, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "QR";
            // 
            // nudEntTj
            // 
            this.nudEntTj.DecimalPlaces = 2;
            this.nudEntTj.Location = new System.Drawing.Point(61, 49);
            this.nudEntTj.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudEntTj.Name = "nudEntTj";
            this.nudEntTj.Size = new System.Drawing.Size(120, 20);
            this.nudEntTj.TabIndex = 3;
            this.nudEntTj.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 49);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Tarjeta";
            // 
            // nudEntEf
            // 
            this.nudEntEf.DecimalPlaces = 2;
            this.nudEntEf.Location = new System.Drawing.Point(61, 20);
            this.nudEntEf.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudEntEf.Name = "nudEntEf";
            this.nudEntEf.Size = new System.Drawing.Size(120, 20);
            this.nudEntEf.TabIndex = 1;
            this.nudEntEf.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(46, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Efectivo";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(32, 336);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Observación";
            // 
            // txtObs
            // 
            this.txtObs.Location = new System.Drawing.Point(106, 336);
            this.txtObs.Name = "txtObs";
            this.txtObs.Size = new System.Drawing.Size(100, 20);
            this.txtObs.TabIndex = 13;
            // 
            // btnRendir
            // 
            this.btnRendir.Location = new System.Drawing.Point(38, 368);
            this.btnRendir.Name = "btnRendir";
            this.btnRendir.Size = new System.Drawing.Size(480, 30);
            this.btnRendir.TabIndex = 14;
            this.btnRendir.Text = "Registrar rendición";
            this.btnRendir.UseVisualStyleBackColor = true;
            this.btnRendir.Click += new System.EventHandler(this.btnRendir_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoEllipsis = true;
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(38, 416);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(13, 13);
            this.lblResultado.TabIndex = 15;
            this.lblResultado.Text = "—";
            // 
            // lblSelCaja
            // 
            this.lblSelCaja.AutoSize = true;
            this.lblSelCaja.Location = new System.Drawing.Point(23, 13);
            this.lblSelCaja.Name = "lblSelCaja";
            this.lblSelCaja.Size = new System.Drawing.Size(66, 13);
            this.lblSelCaja.TabIndex = 16;
            this.lblSelCaja.Text = "Caja abierta:";
            // 
            // cboCajaAbierta
            // 
            this.cboCajaAbierta.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCajaAbierta.FormattingEnabled = true;
            this.cboCajaAbierta.Location = new System.Drawing.Point(106, 13);
            this.cboCajaAbierta.Name = "cboCajaAbierta";
            this.cboCajaAbierta.Size = new System.Drawing.Size(260, 21);
            this.cboCajaAbierta.TabIndex = 17;
            this.cboCajaAbierta.SelectedIndexChanged += new System.EventHandler(this.cboCajaAbierta_SelectedIndexChanged);
            // 
            // btnRefrescarCajas
            // 
            this.btnRefrescarCajas.Location = new System.Drawing.Point(390, 13);
            this.btnRefrescarCajas.Name = "btnRefrescarCajas";
            this.btnRefrescarCajas.Size = new System.Drawing.Size(90, 26);
            this.btnRefrescarCajas.TabIndex = 18;
            this.btnRefrescarCajas.Text = "Actualizar";
            this.btnRefrescarCajas.UseVisualStyleBackColor = true;
            this.btnRefrescarCajas.Click += new System.EventHandler(this.btnRefrescarCajas_Click);
            // 
            // dgvMozos
            // 
            this.dgvMozos.AllowUserToAddRows = false;
            this.dgvMozos.AllowUserToDeleteRows = false;
            this.dgvMozos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMozos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMozos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMozo,
            this.colEf,
            this.colTj,
            this.colQr,
            this.colTot});
            this.dgvMozos.Location = new System.Drawing.Point(26, 69);
            this.dgvMozos.Name = "dgvMozos";
            this.dgvMozos.ReadOnly = true;
            this.dgvMozos.RowHeadersVisible = false;
            this.dgvMozos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMozos.Size = new System.Drawing.Size(490, 105);
            this.dgvMozos.TabIndex = 19;
            this.dgvMozos.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMozos_CellDoubleClick);
            // 
            // colMozo
            // 
            this.colMozo.HeaderText = "Mozo";
            this.colMozo.Name = "colMozo";
            this.colMozo.ReadOnly = true;
            // 
            // colEf
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle13.Format = "N2";
            dataGridViewCellStyle13.NullValue = null;
            this.colEf.DefaultCellStyle = dataGridViewCellStyle13;
            this.colEf.HeaderText = "Ef";
            this.colEf.Name = "colEf";
            this.colEf.ReadOnly = true;
            // 
            // colTj
            // 
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle14.Format = "N2";
            dataGridViewCellStyle14.NullValue = null;
            this.colTj.DefaultCellStyle = dataGridViewCellStyle14;
            this.colTj.HeaderText = "Tj";
            this.colTj.Name = "colTj";
            this.colTj.ReadOnly = true;
            // 
            // colQr
            // 
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle15.Format = "N2";
            dataGridViewCellStyle15.NullValue = null;
            this.colQr.DefaultCellStyle = dataGridViewCellStyle15;
            this.colQr.HeaderText = "QR";
            this.colQr.Name = "colQr";
            this.colQr.ReadOnly = true;
            // 
            // colTot
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle16.Format = "N2";
            dataGridViewCellStyle16.NullValue = null;
            this.colTot.DefaultCellStyle = dataGridViewCellStyle16;
            this.colTot.HeaderText = "Total";
            this.colTot.Name = "colTot";
            this.colTot.ReadOnly = true;
            // 
            // FrmRendicionValores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 432);
            this.Controls.Add(this.dgvMozos);
            this.Controls.Add(this.btnRefrescarCajas);
            this.Controls.Add(this.cboCajaAbierta);
            this.Controls.Add(this.lblSelCaja);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.btnRendir);
            this.Controls.Add(this.txtObs);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCalcular);
            this.Controls.Add(this.lblCaja);
            this.Controls.Add(this.txtMozo);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(80, 16);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRendicionValores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rendición de valores";
            this.Load += new System.EventHandler(this.FrmRendicionValores_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntQR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntTj)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntEf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMozos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblCaja;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMozo;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblEspQR;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblEspTj;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblEspEf;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nudEntQR;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nudEntTj;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudEntEf;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtObs;
        private System.Windows.Forms.Button btnRendir;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Label lblSelCaja;
        private System.Windows.Forms.ComboBox cboCajaAbierta;
        private System.Windows.Forms.Button btnRefrescarCajas;
        private System.Windows.Forms.DataGridView dgvMozos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMozo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEf;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTj;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQr;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTot;
    }
}