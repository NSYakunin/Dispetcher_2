namespace Dispetcher2.DialogsForms
{
    partial class dF_Brigade
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dF_Brigade));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tB_SearchBrigade = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dGV_Brigade = new System.Windows.Forms.DataGridView();
            this.Col_NumBrigade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_LoginBrigade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Choose = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Brigade)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tB_SearchBrigade);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox4.Size = new System.Drawing.Size(739, 58);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Поиск по ФИО (по всем бригадам)";
            // 
            // tB_SearchBrigade
            // 
            this.tB_SearchBrigade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_SearchBrigade.Location = new System.Drawing.Point(5, 21);
            this.tB_SearchBrigade.Margin = new System.Windows.Forms.Padding(5);
            this.tB_SearchBrigade.Name = "tB_SearchBrigade";
            this.tB_SearchBrigade.Size = new System.Drawing.Size(729, 23);
            this.tB_SearchBrigade.TabIndex = 1;
            this.tB_SearchBrigade.TextChanged += new System.EventHandler(this.tB_SearchBrigade_TextChanged);
            this.tB_SearchBrigade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_SearchBrigade_KeyDown);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dGV_Brigade);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 58);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(739, 411);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Бригады (ENTER - выбрать бригаду )";
            // 
            // dGV_Brigade
            // 
            this.dGV_Brigade.AllowUserToAddRows = false;
            this.dGV_Brigade.AllowUserToDeleteRows = false;
            this.dGV_Brigade.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Brigade.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Brigade.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Brigade.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_NumBrigade,
            this.Col_LoginBrigade});
            this.dGV_Brigade.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Brigade.Location = new System.Drawing.Point(4, 20);
            this.dGV_Brigade.Margin = new System.Windows.Forms.Padding(5);
            this.dGV_Brigade.MultiSelect = false;
            this.dGV_Brigade.Name = "dGV_Brigade";
            this.dGV_Brigade.ReadOnly = true;
            this.dGV_Brigade.RowHeadersWidth = 4;
            this.dGV_Brigade.Size = new System.Drawing.Size(731, 387);
            this.dGV_Brigade.TabIndex = 2;
            this.dGV_Brigade.TabStop = false;
            this.dGV_Brigade.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_Brigade_KeyDown);
            this.dGV_Brigade.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dGV_Brigade_MouseDoubleClick);
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
            // btn_Choose
            // 
            this.btn_Choose.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Choose.Location = new System.Drawing.Point(0, 469);
            this.btn_Choose.Margin = new System.Windows.Forms.Padding(5);
            this.btn_Choose.Name = "btn_Choose";
            this.btn_Choose.Size = new System.Drawing.Size(739, 50);
            this.btn_Choose.TabIndex = 5;
            this.btn_Choose.Text = "Выбрать";
            this.btn_Choose.UseVisualStyleBackColor = false;
            this.btn_Choose.Click += new System.EventHandler(this.btn_Choose_Click);
            // 
            // dF_Brigade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(739, 519);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btn_Choose);
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dF_Brigade";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Бригады";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.dF_Brigade_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Brigade)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox tB_SearchBrigade;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dGV_Brigade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_NumBrigade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_LoginBrigade;
        private System.Windows.Forms.Button btn_Choose;
    }
}