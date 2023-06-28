namespace Dispetcher2
{
    partial class F_Brigade
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dGV_AllBrigade = new System.Windows.Forms.DataGridView();
            this.Col_NumBrigade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_LoginBrigade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_IsValidBrigade = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tB_SearchBrigade = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dGV_NewBrigade = new System.Windows.Forms.DataGridView();
            this.Col_FullNameNewBrigade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_LoginNewBrigade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Create = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dGV_Workers = new System.Windows.Forms.DataGridView();
            this.Col_FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Login = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gB_Search = new System.Windows.Forms.GroupBox();
            this.tB_Search = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_AllBrigade)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_NewBrigade)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Workers)).BeginInit();
            this.gB_Search.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 270F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(995, 632);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.groupBox4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(499, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(481, 348);
            this.panel2.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dGV_AllBrigade);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 47);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(481, 301);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Бригады (DEL - сделать бригаду не активной. ENTER - активной )";
            // 
            // dGV_AllBrigade
            // 
            this.dGV_AllBrigade.AllowUserToAddRows = false;
            this.dGV_AllBrigade.AllowUserToDeleteRows = false;
            this.dGV_AllBrigade.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_AllBrigade.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_AllBrigade.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_AllBrigade.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_NumBrigade,
            this.Col_LoginBrigade,
            this.Col_IsValidBrigade});
            this.dGV_AllBrigade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_AllBrigade.Location = new System.Drawing.Point(3, 19);
            this.dGV_AllBrigade.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_AllBrigade.MultiSelect = false;
            this.dGV_AllBrigade.Name = "dGV_AllBrigade";
            this.dGV_AllBrigade.ReadOnly = true;
            this.dGV_AllBrigade.RowHeadersWidth = 4;
            this.dGV_AllBrigade.Size = new System.Drawing.Size(475, 279);
            this.dGV_AllBrigade.TabIndex = 2;
            this.dGV_AllBrigade.TabStop = false;
            this.dGV_AllBrigade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_AllBrigade_KeyDown);
            // 
            // Col_NumBrigade
            // 
            this.Col_NumBrigade.HeaderText = "№";
            this.Col_NumBrigade.MinimumWidth = 40;
            this.Col_NumBrigade.Name = "Col_NumBrigade";
            this.Col_NumBrigade.ReadOnly = true;
            this.Col_NumBrigade.Width = 50;
            // 
            // Col_LoginBrigade
            // 
            this.Col_LoginBrigade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_LoginBrigade.HeaderText = "Бригада";
            this.Col_LoginBrigade.MinimumWidth = 80;
            this.Col_LoginBrigade.Name = "Col_LoginBrigade";
            this.Col_LoginBrigade.ReadOnly = true;
            // 
            // Col_IsValidBrigade
            // 
            this.Col_IsValidBrigade.HeaderText = "А";
            this.Col_IsValidBrigade.MinimumWidth = 40;
            this.Col_IsValidBrigade.Name = "Col_IsValidBrigade";
            this.Col_IsValidBrigade.ReadOnly = true;
            this.Col_IsValidBrigade.ToolTipText = "Активна";
            this.Col_IsValidBrigade.Width = 40;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tB_SearchBrigade);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(481, 47);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Поиск по ФИО (по всем бригадам)";
            // 
            // tB_SearchBrigade
            // 
            this.tB_SearchBrigade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_SearchBrigade.Location = new System.Drawing.Point(4, 20);
            this.tB_SearchBrigade.Margin = new System.Windows.Forms.Padding(4);
            this.tB_SearchBrigade.Name = "tB_SearchBrigade";
            this.tB_SearchBrigade.Size = new System.Drawing.Size(473, 23);
            this.tB_SearchBrigade.TabIndex = 1;
            this.tB_SearchBrigade.TextChanged += new System.EventHandler(this.tB_SearchBrigade_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dGV_NewBrigade);
            this.groupBox1.Controls.Add(this.btn_Create);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(499, 360);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(481, 264);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Новая бригада (DEL - убрать из бригады)";
            // 
            // dGV_NewBrigade
            // 
            this.dGV_NewBrigade.AllowUserToAddRows = false;
            this.dGV_NewBrigade.AllowUserToDeleteRows = false;
            this.dGV_NewBrigade.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_NewBrigade.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_NewBrigade.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_NewBrigade.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_FullNameNewBrigade,
            this.Col_LoginNewBrigade});
            this.dGV_NewBrigade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_NewBrigade.Location = new System.Drawing.Point(3, 19);
            this.dGV_NewBrigade.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_NewBrigade.MultiSelect = false;
            this.dGV_NewBrigade.Name = "dGV_NewBrigade";
            this.dGV_NewBrigade.ReadOnly = true;
            this.dGV_NewBrigade.RowHeadersWidth = 4;
            this.dGV_NewBrigade.Size = new System.Drawing.Size(475, 201);
            this.dGV_NewBrigade.TabIndex = 3;
            this.dGV_NewBrigade.TabStop = false;
            this.dGV_NewBrigade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_Brigade_KeyDown);
            // 
            // Col_FullNameNewBrigade
            // 
            this.Col_FullNameNewBrigade.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_FullNameNewBrigade.HeaderText = "Фамилия Имя Отчество";
            this.Col_FullNameNewBrigade.MinimumWidth = 100;
            this.Col_FullNameNewBrigade.Name = "Col_FullNameNewBrigade";
            this.Col_FullNameNewBrigade.ReadOnly = true;
            // 
            // Col_LoginNewBrigade
            // 
            this.Col_LoginNewBrigade.HeaderText = "Логин";
            this.Col_LoginNewBrigade.MinimumWidth = 80;
            this.Col_LoginNewBrigade.Name = "Col_LoginNewBrigade";
            this.Col_LoginNewBrigade.ReadOnly = true;
            this.Col_LoginNewBrigade.Width = 140;
            // 
            // btn_Create
            // 
            this.btn_Create.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Create.Location = new System.Drawing.Point(3, 220);
            this.btn_Create.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Create.Name = "btn_Create";
            this.btn_Create.Size = new System.Drawing.Size(475, 41);
            this.btn_Create.TabIndex = 4;
            this.btn_Create.Text = "Создать";
            this.btn_Create.UseVisualStyleBackColor = false;
            this.btn_Create.Click += new System.EventHandler(this.btn_Create_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.gB_Search);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(12, 6);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(481, 618);
            this.panel1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dGV_Workers);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 47);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 571);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Рабочие (ENTER - добавить в бригаду)";
            // 
            // dGV_Workers
            // 
            this.dGV_Workers.AllowUserToAddRows = false;
            this.dGV_Workers.AllowUserToDeleteRows = false;
            this.dGV_Workers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Workers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Workers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Workers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_FullName,
            this.Col_Login});
            this.dGV_Workers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Workers.Location = new System.Drawing.Point(3, 19);
            this.dGV_Workers.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Workers.MultiSelect = false;
            this.dGV_Workers.Name = "dGV_Workers";
            this.dGV_Workers.ReadOnly = true;
            this.dGV_Workers.RowHeadersWidth = 4;
            this.dGV_Workers.Size = new System.Drawing.Size(475, 549);
            this.dGV_Workers.TabIndex = 2;
            this.dGV_Workers.TabStop = false;
            this.dGV_Workers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_Workers_KeyDown);
            // 
            // Col_FullName
            // 
            this.Col_FullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_FullName.HeaderText = "Фамилия Имя Отчество";
            this.Col_FullName.MinimumWidth = 100;
            this.Col_FullName.Name = "Col_FullName";
            this.Col_FullName.ReadOnly = true;
            // 
            // Col_Login
            // 
            this.Col_Login.HeaderText = "Логин";
            this.Col_Login.MinimumWidth = 80;
            this.Col_Login.Name = "Col_Login";
            this.Col_Login.ReadOnly = true;
            this.Col_Login.Width = 140;
            // 
            // gB_Search
            // 
            this.gB_Search.Controls.Add(this.tB_Search);
            this.gB_Search.Dock = System.Windows.Forms.DockStyle.Top;
            this.gB_Search.Location = new System.Drawing.Point(0, 0);
            this.gB_Search.Margin = new System.Windows.Forms.Padding(4);
            this.gB_Search.Name = "gB_Search";
            this.gB_Search.Padding = new System.Windows.Forms.Padding(4);
            this.gB_Search.Size = new System.Drawing.Size(481, 47);
            this.gB_Search.TabIndex = 1;
            this.gB_Search.TabStop = false;
            this.gB_Search.Text = "Поиск по ФИО (по всем учётным записям)";
            // 
            // tB_Search
            // 
            this.tB_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_Search.Location = new System.Drawing.Point(4, 20);
            this.tB_Search.Margin = new System.Windows.Forms.Padding(4);
            this.tB_Search.Name = "tB_Search";
            this.tB_Search.Size = new System.Drawing.Size(473, 23);
            this.tB_Search.TabIndex = 1;
            this.tB_Search.TextChanged += new System.EventHandler(this.tB_Search_TextChanged);
            // 
            // F_Brigade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(995, 632);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_Brigade";
            this.Text = "F_Brigade";
            this.Load += new System.EventHandler(this.F_Brigade_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_AllBrigade)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_NewBrigade)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Workers)).EndInit();
            this.gB_Search.ResumeLayout(false);
            this.gB_Search.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dGV_Workers;
        private System.Windows.Forms.GroupBox gB_Search;
        private System.Windows.Forms.TextBox tB_Search;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dGV_NewBrigade;
        private System.Windows.Forms.Button btn_Create;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Login;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dGV_AllBrigade;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tB_SearchBrigade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FullNameNewBrigade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_LoginNewBrigade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_NumBrigade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_LoginBrigade;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Col_IsValidBrigade;

    }
}