namespace Restaurar_v1._0
{
    partial class FormRolesPermisos
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
            this.treeVwUsuarioPermisosRoles = new System.Windows.Forms.TreeView();
            this.treeVwUsuarios = new System.Windows.Forms.TreeView();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.ckBxUsuarioBloqueado = new System.Windows.Forms.CheckBox();
            this.ckBxUsuarioActivo = new System.Windows.Forms.CheckBox();
            this.ckBxUsuarioClave = new System.Windows.Forms.CheckBox();
            this.txtBxUsuarioPassword = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtBxUsuarioNombre = new System.Windows.Forms.TextBox();
            this.txtBxUsuarioId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmBxPermisoItem = new System.Windows.Forms.ComboBox();
            this.cmBxPermisoMenu = new System.Windows.Forms.ComboBox();
            this.txtBxPermisoNombre = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnLimpiarCampos = new System.Windows.Forms.Button();
            this.btnRolUsuarioAsignar = new System.Windows.Forms.Button();
            this.btnRolUsuarioQuitar = new System.Windows.Forms.Button();
            this.btnPermisoUsuarioAsociar = new System.Windows.Forms.Button();
            this.btnPermisoUsuarioQuitar = new System.Windows.Forms.Button();
            this.btnPermisoRolAsociar = new System.Windows.Forms.Button();
            this.btnPermisoRolQuitar = new System.Windows.Forms.Button();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.treeVwPermisosPorRol = new System.Windows.Forms.TreeView();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.txtBxPermisoId = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.txtBxRolNombre2 = new System.Windows.Forms.TextBox();
            this.txtBxRolId2 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbBxRoles = new System.Windows.Forms.ComboBox();
            this.btnDesasociarRolARol = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.btnAsociarRolARol = new System.Windows.Forms.Button();
            this.btnRolEliminar = new System.Windows.Forms.Button();
            this.btnRolModificar = new System.Windows.Forms.Button();
            this.btnRolAlta = new System.Windows.Forms.Button();
            this.txtBxRolNombre = new System.Windows.Forms.TextBox();
            this.txtBxRolId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.treeVwRoles = new System.Windows.Forms.TreeView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.treeVwPermisos = new System.Windows.Forms.TreeView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox7.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox11.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox12.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox13.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeVwUsuarioPermisosRoles
            // 
            this.treeVwUsuarioPermisosRoles.Location = new System.Drawing.Point(6, 19);
            this.treeVwUsuarioPermisosRoles.Name = "treeVwUsuarioPermisosRoles";
            this.treeVwUsuarioPermisosRoles.Size = new System.Drawing.Size(209, 397);
            this.treeVwUsuarioPermisosRoles.TabIndex = 0;
            // 
            // treeVwUsuarios
            // 
            this.treeVwUsuarios.Location = new System.Drawing.Point(9, 19);
            this.treeVwUsuarios.Name = "treeVwUsuarios";
            this.treeVwUsuarios.Size = new System.Drawing.Size(146, 397);
            this.treeVwUsuarios.TabIndex = 0;
            this.treeVwUsuarios.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeUsuario_BeforeCheck);
            this.treeVwUsuarios.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeVwUsuarios_AfterSelect);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.ckBxUsuarioBloqueado);
            this.groupBox7.Controls.Add(this.ckBxUsuarioActivo);
            this.groupBox7.Controls.Add(this.ckBxUsuarioClave);
            this.groupBox7.Controls.Add(this.txtBxUsuarioPassword);
            this.groupBox7.Controls.Add(this.label7);
            this.groupBox7.Controls.Add(this.txtBxUsuarioNombre);
            this.groupBox7.Controls.Add(this.txtBxUsuarioId);
            this.groupBox7.Controls.Add(this.label5);
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Location = new System.Drawing.Point(12, 12);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(345, 124);
            this.groupBox7.TabIndex = 26;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Usuario:";
            // 
            // ckBxUsuarioBloqueado
            // 
            this.ckBxUsuarioBloqueado.AutoSize = true;
            this.ckBxUsuarioBloqueado.Enabled = false;
            this.ckBxUsuarioBloqueado.Location = new System.Drawing.Point(215, 26);
            this.ckBxUsuarioBloqueado.Name = "ckBxUsuarioBloqueado";
            this.ckBxUsuarioBloqueado.Size = new System.Drawing.Size(77, 17);
            this.ckBxUsuarioBloqueado.TabIndex = 8;
            this.ckBxUsuarioBloqueado.Text = "Bloqueado";
            this.ckBxUsuarioBloqueado.UseVisualStyleBackColor = true;
            // 
            // ckBxUsuarioActivo
            // 
            this.ckBxUsuarioActivo.AutoSize = true;
            this.ckBxUsuarioActivo.Enabled = false;
            this.ckBxUsuarioActivo.Location = new System.Drawing.Point(215, 58);
            this.ckBxUsuarioActivo.Name = "ckBxUsuarioActivo";
            this.ckBxUsuarioActivo.Size = new System.Drawing.Size(56, 17);
            this.ckBxUsuarioActivo.TabIndex = 7;
            this.ckBxUsuarioActivo.Text = "Activo";
            this.ckBxUsuarioActivo.UseVisualStyleBackColor = true;
            // 
            // ckBxUsuarioClave
            // 
            this.ckBxUsuarioClave.AutoSize = true;
            this.ckBxUsuarioClave.Location = new System.Drawing.Point(215, 95);
            this.ckBxUsuarioClave.Name = "ckBxUsuarioClave";
            this.ckBxUsuarioClave.Size = new System.Drawing.Size(127, 17);
            this.ckBxUsuarioClave.TabIndex = 6;
            this.ckBxUsuarioClave.Text = "Descifrar/Cifrar Clave";
            this.ckBxUsuarioClave.UseVisualStyleBackColor = true;
            this.ckBxUsuarioClave.CheckedChanged += new System.EventHandler(this.ckBxUsuarioClave_CheckedChanged);
            // 
            // txtBxUsuarioPassword
            // 
            this.txtBxUsuarioPassword.Enabled = false;
            this.txtBxUsuarioPassword.Location = new System.Drawing.Point(68, 92);
            this.txtBxUsuarioPassword.Name = "txtBxUsuarioPassword";
            this.txtBxUsuarioPassword.Size = new System.Drawing.Size(141, 20);
            this.txtBxUsuarioPassword.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Password:";
            // 
            // txtBxUsuarioNombre
            // 
            this.txtBxUsuarioNombre.Enabled = false;
            this.txtBxUsuarioNombre.Location = new System.Drawing.Point(68, 55);
            this.txtBxUsuarioNombre.Name = "txtBxUsuarioNombre";
            this.txtBxUsuarioNombre.Size = new System.Drawing.Size(141, 20);
            this.txtBxUsuarioNombre.TabIndex = 3;
            // 
            // txtBxUsuarioId
            // 
            this.txtBxUsuarioId.Enabled = false;
            this.txtBxUsuarioId.Location = new System.Drawing.Point(68, 23);
            this.txtBxUsuarioId.Name = "txtBxUsuarioId";
            this.txtBxUsuarioId.Size = new System.Drawing.Size(48, 20);
            this.txtBxUsuarioId.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Nombre:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "ID:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Menu:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(147, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(30, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Item:";
            // 
            // cmBxPermisoItem
            // 
            this.cmBxPermisoItem.FormattingEnabled = true;
            this.cmBxPermisoItem.Location = new System.Drawing.Point(183, 45);
            this.cmBxPermisoItem.Name = "cmBxPermisoItem";
            this.cmBxPermisoItem.Size = new System.Drawing.Size(103, 21);
            this.cmBxPermisoItem.TabIndex = 8;
            this.cmBxPermisoItem.SelectedIndexChanged += new System.EventHandler(this.cmBxPermisoItem_SelectedIndexChanged);
            // 
            // cmBxPermisoMenu
            // 
            this.cmBxPermisoMenu.FormattingEnabled = true;
            this.cmBxPermisoMenu.Location = new System.Drawing.Point(49, 45);
            this.cmBxPermisoMenu.Name = "cmBxPermisoMenu";
            this.cmBxPermisoMenu.Size = new System.Drawing.Size(92, 21);
            this.cmBxPermisoMenu.TabIndex = 7;
            this.cmBxPermisoMenu.SelectedIndexChanged += new System.EventHandler(this.cmBxPermisoMenu_SelectedIndexChanged);
            // 
            // txtBxPermisoNombre
            // 
            this.txtBxPermisoNombre.Enabled = false;
            this.txtBxPermisoNombre.Location = new System.Drawing.Point(133, 19);
            this.txtBxPermisoNombre.Name = "txtBxPermisoNombre";
            this.txtBxPermisoNombre.Size = new System.Drawing.Size(100, 20);
            this.txtBxPermisoNombre.TabIndex = 3;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.treeVwUsuarios);
            this.groupBox6.Location = new System.Drawing.Point(12, 248);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(161, 422);
            this.groupBox6.TabIndex = 20;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Usuarios:";
            // 
            // btnLimpiarCampos
            // 
            this.btnLimpiarCampos.Location = new System.Drawing.Point(890, 142);
            this.btnLimpiarCampos.Name = "btnLimpiarCampos";
            this.btnLimpiarCampos.Size = new System.Drawing.Size(67, 47);
            this.btnLimpiarCampos.TabIndex = 24;
            this.btnLimpiarCampos.Text = "Limpiar Campos";
            this.btnLimpiarCampos.UseVisualStyleBackColor = true;
            this.btnLimpiarCampos.Click += new System.EventHandler(this.btnLimpiarCampos_Click);
            // 
            // btnRolUsuarioAsignar
            // 
            this.btnRolUsuarioAsignar.Location = new System.Drawing.Point(6, 19);
            this.btnRolUsuarioAsignar.Name = "btnRolUsuarioAsignar";
            this.btnRolUsuarioAsignar.Size = new System.Drawing.Size(67, 49);
            this.btnRolUsuarioAsignar.TabIndex = 9;
            this.btnRolUsuarioAsignar.Text = "Asociar Rol a Usuario";
            this.btnRolUsuarioAsignar.UseVisualStyleBackColor = true;
            this.btnRolUsuarioAsignar.Click += new System.EventHandler(this.btnRolUsuarioAsignar_Click);
            // 
            // btnRolUsuarioQuitar
            // 
            this.btnRolUsuarioQuitar.Location = new System.Drawing.Point(86, 20);
            this.btnRolUsuarioQuitar.Name = "btnRolUsuarioQuitar";
            this.btnRolUsuarioQuitar.Size = new System.Drawing.Size(66, 48);
            this.btnRolUsuarioQuitar.TabIndex = 10;
            this.btnRolUsuarioQuitar.Text = "Quitar Rol a Usuario";
            this.btnRolUsuarioQuitar.UseVisualStyleBackColor = true;
            this.btnRolUsuarioQuitar.Click += new System.EventHandler(this.btnRolUsuarioQuitar_Click);
            // 
            // btnPermisoUsuarioAsociar
            // 
            this.btnPermisoUsuarioAsociar.Location = new System.Drawing.Point(6, 20);
            this.btnPermisoUsuarioAsociar.Name = "btnPermisoUsuarioAsociar";
            this.btnPermisoUsuarioAsociar.Size = new System.Drawing.Size(67, 49);
            this.btnPermisoUsuarioAsociar.TabIndex = 11;
            this.btnPermisoUsuarioAsociar.Text = "Asociar Permiso a Usuario";
            this.btnPermisoUsuarioAsociar.UseVisualStyleBackColor = true;
            this.btnPermisoUsuarioAsociar.Click += new System.EventHandler(this.btnPermisoUsuarioAsociar_Click);
            // 
            // btnPermisoUsuarioQuitar
            // 
            this.btnPermisoUsuarioQuitar.Location = new System.Drawing.Point(82, 20);
            this.btnPermisoUsuarioQuitar.Name = "btnPermisoUsuarioQuitar";
            this.btnPermisoUsuarioQuitar.Size = new System.Drawing.Size(67, 49);
            this.btnPermisoUsuarioQuitar.TabIndex = 12;
            this.btnPermisoUsuarioQuitar.Text = "Quitar Permiso a Usuario";
            this.btnPermisoUsuarioQuitar.UseVisualStyleBackColor = true;
            this.btnPermisoUsuarioQuitar.Click += new System.EventHandler(this.btnPermisoUsuarioQuitar_Click);
            // 
            // btnPermisoRolAsociar
            // 
            this.btnPermisoRolAsociar.Location = new System.Drawing.Point(29, 29);
            this.btnPermisoRolAsociar.Name = "btnPermisoRolAsociar";
            this.btnPermisoRolAsociar.Size = new System.Drawing.Size(66, 49);
            this.btnPermisoRolAsociar.TabIndex = 13;
            this.btnPermisoRolAsociar.Text = "Asociar Permiso a Rol";
            this.btnPermisoRolAsociar.UseVisualStyleBackColor = true;
            this.btnPermisoRolAsociar.Click += new System.EventHandler(this.btnPermisoRolAsociar_Click);
            // 
            // btnPermisoRolQuitar
            // 
            this.btnPermisoRolQuitar.Location = new System.Drawing.Point(112, 29);
            this.btnPermisoRolQuitar.Name = "btnPermisoRolQuitar";
            this.btnPermisoRolQuitar.Size = new System.Drawing.Size(65, 49);
            this.btnPermisoRolQuitar.TabIndex = 14;
            this.btnPermisoRolQuitar.Text = "Quitar Permiso a Rol";
            this.btnPermisoRolQuitar.UseVisualStyleBackColor = true;
            this.btnPermisoRolQuitar.Click += new System.EventHandler(this.btnPermisoRolQuitar_Click);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.treeVwPermisosPorRol);
            this.groupBox8.Location = new System.Drawing.Point(541, 248);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(189, 422);
            this.groupBox8.TabIndex = 22;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Permisos Por Rol:";
            // 
            // treeVwPermisosPorRol
            // 
            this.treeVwPermisosPorRol.Location = new System.Drawing.Point(6, 19);
            this.treeVwPermisosPorRol.Name = "treeVwPermisosPorRol";
            this.treeVwPermisosPorRol.Size = new System.Drawing.Size(177, 397);
            this.treeVwPermisosPorRol.TabIndex = 0;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(890, 195);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(67, 47);
            this.btnExit.TabIndex = 27;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.groupBox11);
            this.groupBox9.Controls.Add(this.groupBox10);
            this.groupBox9.Location = new System.Drawing.Point(12, 142);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(342, 100);
            this.groupBox9.TabIndex = 28;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Roles/Permisos Usuario:";
            // 
            // groupBox11
            // 
            this.groupBox11.Controls.Add(this.btnPermisoUsuarioAsociar);
            this.groupBox11.Controls.Add(this.btnPermisoUsuarioQuitar);
            this.groupBox11.Location = new System.Drawing.Point(181, 19);
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.Size = new System.Drawing.Size(155, 75);
            this.groupBox11.TabIndex = 1;
            this.groupBox11.TabStop = false;
            this.groupBox11.Text = "Permisos a Usuario:";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.btnRolUsuarioAsignar);
            this.groupBox10.Controls.Add(this.btnRolUsuarioQuitar);
            this.groupBox10.Location = new System.Drawing.Point(9, 19);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(166, 75);
            this.groupBox10.TabIndex = 0;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Roles a Usuario:";
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.btnPermisoRolAsociar);
            this.groupBox12.Controls.Add(this.btnPermisoRolQuitar);
            this.groupBox12.Location = new System.Drawing.Point(619, 142);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(200, 100);
            this.groupBox12.TabIndex = 29;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = "Opciones Roles/Permisos";
            // 
            // txtBxPermisoId
            // 
            this.txtBxPermisoId.Enabled = false;
            this.txtBxPermisoId.Location = new System.Drawing.Point(39, 19);
            this.txtBxPermisoId.Name = "txtBxPermisoId";
            this.txtBxPermisoId.Size = new System.Drawing.Size(35, 20);
            this.txtBxPermisoId.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox13);
            this.groupBox1.Controls.Add(this.btnRolEliminar);
            this.groupBox1.Controls.Add(this.btnRolModificar);
            this.groupBox1.Controls.Add(this.btnRolAlta);
            this.groupBox1.Controls.Add(this.txtBxRolNombre);
            this.groupBox1.Controls.Add(this.txtBxRolId);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(363, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 230);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rol:";
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.txtBxRolNombre2);
            this.groupBox13.Controls.Add(this.txtBxRolId2);
            this.groupBox13.Controls.Add(this.label12);
            this.groupBox13.Controls.Add(this.label11);
            this.groupBox13.Controls.Add(this.cmbBxRoles);
            this.groupBox13.Controls.Add(this.btnDesasociarRolARol);
            this.groupBox13.Controls.Add(this.label10);
            this.groupBox13.Controls.Add(this.btnAsociarRolARol);
            this.groupBox13.Location = new System.Drawing.Point(6, 86);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(238, 138);
            this.groupBox13.TabIndex = 11;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = "Rol para Asociar/Desasociar a otro Rol:";
            // 
            // txtBxRolNombre2
            // 
            this.txtBxRolNombre2.Enabled = false;
            this.txtBxRolNombre2.Location = new System.Drawing.Point(59, 58);
            this.txtBxRolNombre2.Name = "txtBxRolNombre2";
            this.txtBxRolNombre2.Size = new System.Drawing.Size(173, 20);
            this.txtBxRolNombre2.TabIndex = 14;
            // 
            // txtBxRolId2
            // 
            this.txtBxRolId2.Enabled = false;
            this.txtBxRolId2.Location = new System.Drawing.Point(9, 60);
            this.txtBxRolId2.Name = "txtBxRolId2";
            this.txtBxRolId2.Size = new System.Drawing.Size(34, 20);
            this.txtBxRolId2.TabIndex = 13;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(56, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Nombre:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 44);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "ID:";
            // 
            // cmbBxRoles
            // 
            this.cmbBxRoles.FormattingEnabled = true;
            this.cmbBxRoles.Location = new System.Drawing.Point(59, 19);
            this.cmbBxRoles.Name = "cmbBxRoles";
            this.cmbBxRoles.Size = new System.Drawing.Size(173, 21);
            this.cmbBxRoles.TabIndex = 7;
            this.cmbBxRoles.SelectedIndexChanged += new System.EventHandler(this.cmbBxRoles_SelectedIndexChanged);
            // 
            // btnDesasociarRolARol
            // 
            this.btnDesasociarRolARol.Location = new System.Drawing.Point(134, 84);
            this.btnDesasociarRolARol.Name = "btnDesasociarRolARol";
            this.btnDesasociarRolARol.Size = new System.Drawing.Size(98, 48);
            this.btnDesasociarRolARol.TabIndex = 10;
            this.btnDesasociarRolARol.Text = "Desasociar Roles a Usuario";
            this.btnDesasociarRolARol.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "Roles:";
            // 
            // btnAsociarRolARol
            // 
            this.btnAsociarRolARol.Location = new System.Drawing.Point(6, 86);
            this.btnAsociarRolARol.Name = "btnAsociarRolARol";
            this.btnAsociarRolARol.Size = new System.Drawing.Size(97, 46);
            this.btnAsociarRolARol.TabIndex = 9;
            this.btnAsociarRolARol.Text = "Asociar Roles a Usuario";
            this.btnAsociarRolARol.UseVisualStyleBackColor = true;
            // 
            // btnRolEliminar
            // 
            this.btnRolEliminar.Location = new System.Drawing.Point(165, 50);
            this.btnRolEliminar.Name = "btnRolEliminar";
            this.btnRolEliminar.Size = new System.Drawing.Size(75, 30);
            this.btnRolEliminar.TabIndex = 6;
            this.btnRolEliminar.Text = "Eliminar";
            this.btnRolEliminar.UseVisualStyleBackColor = true;
            this.btnRolEliminar.Click += new System.EventHandler(this.btnRolEliminar_Click);
            // 
            // btnRolModificar
            // 
            this.btnRolModificar.Location = new System.Drawing.Point(84, 50);
            this.btnRolModificar.Name = "btnRolModificar";
            this.btnRolModificar.Size = new System.Drawing.Size(75, 30);
            this.btnRolModificar.TabIndex = 5;
            this.btnRolModificar.Text = "Modificar";
            this.btnRolModificar.UseVisualStyleBackColor = true;
            this.btnRolModificar.Click += new System.EventHandler(this.btnRolModificar_Click);
            // 
            // btnRolAlta
            // 
            this.btnRolAlta.Location = new System.Drawing.Point(6, 50);
            this.btnRolAlta.Name = "btnRolAlta";
            this.btnRolAlta.Size = new System.Drawing.Size(75, 30);
            this.btnRolAlta.TabIndex = 4;
            this.btnRolAlta.Text = "Alta";
            this.btnRolAlta.UseVisualStyleBackColor = true;
            this.btnRolAlta.Click += new System.EventHandler(this.btnRolAlta_Click);
            // 
            // txtBxRolNombre
            // 
            this.txtBxRolNombre.Location = new System.Drawing.Point(140, 23);
            this.txtBxRolNombre.Name = "txtBxRolNombre";
            this.txtBxRolNombre.Size = new System.Drawing.Size(100, 20);
            this.txtBxRolNombre.TabIndex = 3;
            // 
            // txtBxRolId
            // 
            this.txtBxRolId.Enabled = false;
            this.txtBxRolId.Location = new System.Drawing.Point(33, 23);
            this.txtBxRolId.Name = "txtBxRolId";
            this.txtBxRolId.Size = new System.Drawing.Size(48, 20);
            this.txtBxRolId.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nombre:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.treeVwRoles);
            this.groupBox2.Location = new System.Drawing.Point(179, 248);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(162, 422);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Roles:";
            // 
            // treeVwRoles
            // 
            this.treeVwRoles.Location = new System.Drawing.Point(6, 19);
            this.treeVwRoles.Name = "treeVwRoles";
            this.treeVwRoles.Size = new System.Drawing.Size(150, 397);
            this.treeVwRoles.TabIndex = 0;
            this.treeVwRoles.BeforeCheck += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeRol_BeforeCheck);
            this.treeVwRoles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeVwRoles_AfterSelect);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.treeVwPermisos);
            this.groupBox3.Location = new System.Drawing.Point(347, 248);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(188, 422);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Permisos:";
            // 
            // treeVwPermisos
            // 
            this.treeVwPermisos.Location = new System.Drawing.Point(6, 19);
            this.treeVwPermisos.Name = "treeVwPermisos";
            this.treeVwPermisos.Size = new System.Drawing.Size(176, 397);
            this.treeVwPermisos.TabIndex = 0;
            this.treeVwPermisos.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeVwPermisos_AfterSelect);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.cmBxPermisoItem);
            this.groupBox4.Controls.Add(this.cmBxPermisoMenu);
            this.groupBox4.Controls.Add(this.txtBxPermisoNombre);
            this.groupBox4.Controls.Add(this.txtBxPermisoId);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(619, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(338, 124);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Permiso:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(80, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Nombre:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "ID:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.treeVwUsuarioPermisosRoles);
            this.groupBox5.Location = new System.Drawing.Point(736, 248);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(221, 422);
            this.groupBox5.TabIndex = 23;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Roles y Permisos del Usuario:";
            // 
            // FormRolesPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(979, 683);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.btnLimpiarCampos);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.groupBox9);
            this.Controls.Add(this.groupBox12);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormRolesPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Roles y Permisos";
            this.Load += new System.EventHandler(this.FormRolesPermisos_Load);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox11.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeVwUsuarioPermisosRoles;
        private System.Windows.Forms.TreeView treeVwUsuarios;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.CheckBox ckBxUsuarioBloqueado;
        private System.Windows.Forms.CheckBox ckBxUsuarioActivo;
        private System.Windows.Forms.CheckBox ckBxUsuarioClave;
        private System.Windows.Forms.TextBox txtBxUsuarioPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtBxUsuarioNombre;
        private System.Windows.Forms.TextBox txtBxUsuarioId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmBxPermisoItem;
        private System.Windows.Forms.ComboBox cmBxPermisoMenu;
        private System.Windows.Forms.TextBox txtBxPermisoNombre;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnLimpiarCampos;
        private System.Windows.Forms.Button btnRolUsuarioAsignar;
        private System.Windows.Forms.Button btnRolUsuarioQuitar;
        private System.Windows.Forms.Button btnPermisoUsuarioAsociar;
        private System.Windows.Forms.Button btnPermisoUsuarioQuitar;
        private System.Windows.Forms.Button btnPermisoRolAsociar;
        private System.Windows.Forms.Button btnPermisoRolQuitar;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TreeView treeVwPermisosPorRol;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.TextBox txtBxPermisoId;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.TextBox txtBxRolNombre2;
        private System.Windows.Forms.TextBox txtBxRolId2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbBxRoles;
        private System.Windows.Forms.Button btnDesasociarRolARol;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnAsociarRolARol;
        private System.Windows.Forms.Button btnRolEliminar;
        private System.Windows.Forms.Button btnRolModificar;
        private System.Windows.Forms.Button btnRolAlta;
        private System.Windows.Forms.TextBox txtBxRolNombre;
        private System.Windows.Forms.TextBox txtBxRolId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView treeVwRoles;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TreeView treeVwPermisos;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox5;
    }
}