namespace Dispetcher2.DialogsForms
{
    partial class dF_Tehnology
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dF_Tehnology));
            this.dGV_AddDetails = new System.Windows.Forms.DataGridView();
            this.Col_NameType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_ShcmDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_NameDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_PK_IdDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_AddDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // dGV_AddDetails
            // 
            this.dGV_AddDetails.AllowUserToAddRows = false;
            this.dGV_AddDetails.AllowUserToDeleteRows = false;
            this.dGV_AddDetails.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_AddDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_AddDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_AddDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_NameType,
            this.Col_ShcmDetail,
            this.Col_NameDetail,
            this.Col_PK_IdDetail});
            this.dGV_AddDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_AddDetails.Location = new System.Drawing.Point(0, 0);
            this.dGV_AddDetails.Margin = new System.Windows.Forms.Padding(5);
            this.dGV_AddDetails.MultiSelect = false;
            this.dGV_AddDetails.Name = "dGV_AddDetails";
            this.dGV_AddDetails.ReadOnly = true;
            this.dGV_AddDetails.RowHeadersWidth = 4;
            this.dGV_AddDetails.Size = new System.Drawing.Size(696, 393);
            this.dGV_AddDetails.TabIndex = 5;
            this.dGV_AddDetails.TabStop = false;
            // 
            // Col_NameType
            // 
            this.Col_NameType.HeaderText = "Тип";
            this.Col_NameType.MinimumWidth = 10;
            this.Col_NameType.Name = "Col_NameType";
            this.Col_NameType.ReadOnly = true;
            this.Col_NameType.Width = 80;
            // 
            // Col_ShcmDetail
            // 
            this.Col_ShcmDetail.HeaderText = "ЩЦМ";
            this.Col_ShcmDetail.MinimumWidth = 20;
            this.Col_ShcmDetail.Name = "Col_ShcmDetail";
            this.Col_ShcmDetail.ReadOnly = true;
            this.Col_ShcmDetail.Width = 200;
            // 
            // Col_NameDetail
            // 
            this.Col_NameDetail.HeaderText = "Наименование";
            this.Col_NameDetail.MinimumWidth = 20;
            this.Col_NameDetail.Name = "Col_NameDetail";
            this.Col_NameDetail.ReadOnly = true;
            this.Col_NameDetail.Width = 600;
            // 
            // Col_PK_IdDetail
            // 
            this.Col_PK_IdDetail.HeaderText = "ИД";
            this.Col_PK_IdDetail.MinimumWidth = 10;
            this.Col_PK_IdDetail.Name = "Col_PK_IdDetail";
            this.Col_PK_IdDetail.ReadOnly = true;
            this.Col_PK_IdDetail.Visible = false;
            // 
            // dF_Tehnology
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 393);
            this.Controls.Add(this.dGV_AddDetails);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dF_Tehnology";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Технология";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.dGV_AddDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dGV_AddDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_NameType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ShcmDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_NameDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_PK_IdDetail;
    }
}