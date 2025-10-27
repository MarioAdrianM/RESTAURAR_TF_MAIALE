namespace Restaurar_v1._0
{
    partial class FormPlanoMesas
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
            this.components = new System.ComponentModel.Container();
            this.tabSectores = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tmrRefresco = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabSectores.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabSectores
            // 
            this.tabSectores.Controls.Add(this.tabPage1);
            this.tabSectores.Controls.Add(this.tabPage2);
            this.tabSectores.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSectores.Location = new System.Drawing.Point(0, 0);
            this.tabSectores.Name = "tabSectores";
            this.tabSectores.SelectedIndex = 0;
            this.tabSectores.Size = new System.Drawing.Size(800, 450);
            this.tabSectores.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tmrRefresco
            // 
            this.tmrRefresco.Enabled = true;
            this.tmrRefresco.Interval = 30000;
            this.tmrRefresco.Tick += new System.EventHandler(this.tmrRefresco_Tick);
            // 
            // FormPlanoMesas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabSectores);
            this.Name = "FormPlanoMesas";
            this.Text = "FormPlanoMesas";
            this.Load += new System.EventHandler(this.FormPlanoMesas_Load);
            this.tabSectores.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabSectores;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Timer tmrRefresco;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}