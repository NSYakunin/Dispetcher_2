namespace Dispetcher2.DialogsForms
{
    partial class dF_Workers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dF_Workers));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dGV_Workers = new System.Windows.Forms.DataGridView();
            this.Col_PK_Login = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Choose = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tB_Workers = new System.Windows.Forms.TextBox();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Workers)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dGV_Workers);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 58);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(729, 401);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Рабочие (ENTER - выбрать)";
            // 
            // dGV_Workers
            // 
            this.dGV_Workers.AllowUserToAddRows = false;
            this.dGV_Workers.AllowUserToDeleteRows = false;
            this.dGV_Workers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Workers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Workers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Workers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_PK_Login,
            this.Col_FullName});
            this.dGV_Workers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Workers.Location = new System.Drawing.Point(4, 20);
            this.dGV_Workers.Margin = new System.Windows.Forms.Padding(5);
            this.dGV_Workers.MultiSelect = false;
            this.dGV_Workers.Name = "dGV_Workers";
            this.dGV_Workers.ReadOnly = true;
            this.dGV_Workers.RowHeadersWidth = 4;
            this.dGV_Workers.Size = new System.Drawing.Size(721, 377);
            this.dGV_Workers.TabIndex = 2;
            this.dGV_Workers.TabStop = false;
            this.dGV_Workers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_Workers_KeyDown);
            this.dGV_Workers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dGV_Workers_MouseDoubleClick);
            // 
            // Col_PK_Login
            // 
            this.Col_PK_Login.HeaderText = "Логин";
            this.Col_PK_Login.MinimumWidth = 40;
            this.Col_PK_Login.Name = "Col_PK_Login";
            this.Col_PK_Login.ReadOnly = true;
            this.Col_PK_Login.Width = 180;
            // 
            // Col_FullName
            // 
            this.Col_FullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_FullName.HeaderText = "ФИО";
            this.Col_FullName.MinimumWidth = 80;
            this.Col_FullName.Name = "Col_FullName";
            this.Col_FullName.ReadOnly = true;
            // 
            // btn_Choose
            // 
            this.btn_Choose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Choose.Location = new System.Drawing.Point(0, 459);
            this.btn_Choose.Margin = new System.Windows.Forms.Padding(5);
            this.btn_Choose.Name = "btn_Choose";
            this.btn_Choose.Size = new System.Drawing.Size(729, 50);
            this.btn_Choose.TabIndex = 8;
            this.btn_Choose.Text = "Выбрать";
            this.btn_Choose.UseVisualStyleBackColor = false;
            this.btn_Choose.Click += new System.EventHandler(this.btn_Choose_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tB_Workers);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox4.Size = new System.Drawing.Size(729, 58);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Поиск по ФИО";
            // 
            // tB_Workers
            // 
            this.tB_Workers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_Workers.Location = new System.Drawing.Point(5, 21);
            this.tB_Workers.Margin = new System.Windows.Forms.Padding(5);
            this.tB_Workers.Name = "tB_Workers";
            this.tB_Workers.Size = new System.Drawing.Size(719, 23);
            this.tB_Workers.TabIndex = 1;
            this.tB_Workers.TextChanged += new System.EventHandler(this.tB_Workers_TextChanged);
            this.tB_Workers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_Workers_KeyDown);
            // 
            // dF_Workers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(729, 509);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btn_Choose);
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dF_Workers";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Рабочие";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.dF_Workers_Load);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Workers)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dGV_Workers;
        private System.Windows.Forms.Button btn_Choose;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tB_Workers;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_PK_Login;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FullName;
    }
}