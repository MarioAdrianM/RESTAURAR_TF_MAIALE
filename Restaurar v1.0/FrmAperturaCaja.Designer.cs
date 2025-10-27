namespace Restaurar_v1._0
{
    partial class FrmAperturaCaja
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtPunto = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboTurno = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudFondo = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudUmbral = new System.Windows.Forms.NumericUpDown();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudFondo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUmbral)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Punto:";
            // 
            // txtPunto
            // 
            this.txtPunto.Location = new System.Drawing.Point(120, 16);
            this.txtPunto.Name = "txtPunto";
            this.txtPunto.Size = new System.Drawing.Size(120, 20);
            this.txtPunto.TabIndex = 1;
            this.txtPunto.Text = "Caja1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Turno:";
            // 
            // cboTurno
            // 
            this.cboTurno.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTurno.FormattingEnabled = true;
            this.cboTurno.Location = new System.Drawing.Point(120, 56);
            this.cboTurno.Name = "cboTurno";
            this.cboTurno.Size = new System.Drawing.Size(120, 21);
            this.cboTurno.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Fondo inicial:";
            // 
            // nudFondo
            // 
            this.nudFondo.DecimalPlaces = 2;
            this.nudFondo.Location = new System.Drawing.Point(120, 96);
            this.nudFondo.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudFondo.Name = "nudFondo";
            this.nudFondo.Size = new System.Drawing.Size(120, 20);
            this.nudFondo.TabIndex = 5;
            this.nudFondo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(260, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Umbral dif.:";
            // 
            // nudUmbral
            // 
            this.nudUmbral.DecimalPlaces = 2;
            this.nudUmbral.Location = new System.Drawing.Point(330, 96);
            this.nudUmbral.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudUmbral.Name = "nudUmbral";
            this.nudUmbral.Size = new System.Drawing.Size(70, 20);
            this.nudUmbral.TabIndex = 7;
            this.nudUmbral.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudUmbral.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // btnAbrir
            // 
            this.btnAbrir.Location = new System.Drawing.Point(20, 140);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(380, 20);
            this.btnAbrir.TabIndex = 8;
            this.btnAbrir.Text = "Abrir caja";
            this.btnAbrir.UseVisualStyleBackColor = true;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoEllipsis = true;
            this.lblResultado.AutoSize = true;
            this.lblResultado.Location = new System.Drawing.Point(20, 180);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(13, 13);
            this.lblResultado.TabIndex = 9;
            this.lblResultado.Text = "—";
            // 
            // FrmAperturaCaja
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 232);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.btnAbrir);
            this.Controls.Add(this.nudUmbral);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudFondo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboTurno);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPunto);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAperturaCaja";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Apertura de caja";
            this.Load += new System.EventHandler(this.FrmAperturaCaja_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudFondo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUmbral)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPunto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboTurno;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudFondo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudUmbral;
        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.Label lblResultado;
    }
}