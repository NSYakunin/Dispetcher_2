namespace Dispetcher2
{
    partial class F_SearchSHCM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_SearchSHCM));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tB_ShcmDetail = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dGV_Search = new System.Windows.Forms.DataGridView();
            this.Col_OrderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_ShcmDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_NameDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_AmountDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_IdLoodsman = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Search)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tB_ShcmDetail);
            this.groupBox1.Location = new System.Drawing.Point(10, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 39);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ЩЦМ детали (ENTER -поиск)";
            // 
            // tB_ShcmDetail
            // 
            this.tB_ShcmDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_ShcmDetail.Location = new System.Drawing.Point(3, 16);
            this.tB_ShcmDetail.MaxLength = 50;
            this.tB_ShcmDetail.Name = "tB_ShcmDetail";
            this.tB_ShcmDetail.Size = new System.Drawing.Size(267, 20);
            this.tB_ShcmDetail.TabIndex = 2;
            this.tB_ShcmDetail.TextChanged += new System.EventHandler(this.tB_ShcmDetail_TextChanged);
            this.tB_ShcmDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_ShcmDetail_KeyDown);
            this.tB_ShcmDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_ShcmDetail_KeyPress);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.Controls.Add(this.dGV_Search, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(782, 442);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // dGV_Search
            // 
            this.dGV_Search.AllowUserToAddRows = false;
            this.dGV_Search.AllowUserToDeleteRows = false;
            this.dGV_Search.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Search.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Search.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Search.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_OrderNum,
            this.Col_Position,
            this.Col_ShcmDetail,
            this.Col_NameDetail,
            this.Col_AmountDetails,
            this.Col_IdLoodsman});
            this.dGV_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Search.Location = new System.Drawing.Point(11, 52);
            this.dGV_Search.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Search.MultiSelect = false;
            this.dGV_Search.Name = "dGV_Search";
            this.dGV_Search.ReadOnly = true;
            this.dGV_Search.RowHeadersWidth = 4;
            this.dGV_Search.Size = new System.Drawing.Size(758, 381);
            this.dGV_Search.TabIndex = 12;
            this.dGV_Search.TabStop = false;
            // 
            // Col_OrderNum
            // 
            this.Col_OrderNum.HeaderText = "№ заказа";
            this.Col_OrderNum.MinimumWidth = 20;
            this.Col_OrderNum.Name = "Col_OrderNum";
            this.Col_OrderNum.ReadOnly = true;
            // 
            // Col_Position
            // 
            this.Col_Position.HeaderText = "Позиция";
            this.Col_Position.MinimumWidth = 10;
            this.Col_Position.Name = "Col_Position";
            this.Col_Position.ReadOnly = true;
            this.Col_Position.Width = 60;
            // 
            // Col_ShcmDetail
            // 
            this.Col_ShcmDetail.HeaderText = "ШЦМ";
            this.Col_ShcmDetail.MinimumWidth = 10;
            this.Col_ShcmDetail.Name = "Col_ShcmDetail";
            this.Col_ShcmDetail.ReadOnly = true;
            this.Col_ShcmDetail.Width = 200;
            // 
            // Col_NameDetail
            // 
            this.Col_NameDetail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_NameDetail.HeaderText = "Название";
            this.Col_NameDetail.MinimumWidth = 10;
            this.Col_NameDetail.Name = "Col_NameDetail";
            this.Col_NameDetail.ReadOnly = true;
            // 
            // Col_AmountDetails
            // 
            this.Col_AmountDetails.HeaderText = "Кол-во";
            this.Col_AmountDetails.MinimumWidth = 10;
            this.Col_AmountDetails.Name = "Col_AmountDetails";
            this.Col_AmountDetails.ReadOnly = true;
            this.Col_AmountDetails.Width = 50;
            // 
            // Col_IdLoodsman
            // 
            this.Col_IdLoodsman.HeaderText = "IdLoodsman";
            this.Col_IdLoodsman.MinimumWidth = 10;
            this.Col_IdLoodsman.Name = "Col_IdLoodsman";
            this.Col_IdLoodsman.ReadOnly = true;
            // 
            // F_SearchSHCM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(782, 442);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "F_SearchSHCM";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Поиск детали/узла по всем заказам";
            this.Load += new System.EventHandler(this.F_SearchSHCM_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Search)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tB_ShcmDetail;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dGV_Search;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_OrderNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ShcmDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_NameDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_AmountDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_IdLoodsman;
    }
}