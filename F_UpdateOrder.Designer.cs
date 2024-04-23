namespace Dispetcher2
{
	partial class F_UpdateOrder
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.orderDetailsDGV = new System.Windows.Forms.DataGridView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.updateBTN = new System.Windows.Forms.Button();
			this.nameOrderLBL = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.orderDetailsDGV)).BeginInit();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(731, 450);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.orderDetailsDGV);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 53);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(725, 394);
			this.panel1.TabIndex = 0;
			// 
			// orderDetailsDGV
			// 
			this.orderDetailsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.orderDetailsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
			this.orderDetailsDGV.Location = new System.Drawing.Point(0, 0);
			this.orderDetailsDGV.Name = "orderDetailsDGV";
			this.orderDetailsDGV.RowHeadersVisible = false;
			this.orderDetailsDGV.Size = new System.Drawing.Size(725, 394);
			this.orderDetailsDGV.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.updateBTN);
			this.panel2.Controls.Add(this.nameOrderLBL);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(725, 44);
			this.panel2.TabIndex = 1;
			// 
			// updateBTN
			// 
			this.updateBTN.Dock = System.Windows.Forms.DockStyle.Right;
			this.updateBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.updateBTN.Location = new System.Drawing.Point(641, 0);
			this.updateBTN.Name = "updateBTN";
			this.updateBTN.Size = new System.Drawing.Size(84, 44);
			this.updateBTN.TabIndex = 1;
			this.updateBTN.Text = "Обновить";
			this.updateBTN.UseVisualStyleBackColor = true;
			this.updateBTN.Click += new System.EventHandler(this.updateBTN_Click);
			// 
			// nameOrderLBL
			// 
			this.nameOrderLBL.AutoSize = true;
			this.nameOrderLBL.Dock = System.Windows.Forms.DockStyle.Left;
			this.nameOrderLBL.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.nameOrderLBL.Location = new System.Drawing.Point(0, 0);
			this.nameOrderLBL.Name = "nameOrderLBL";
			this.nameOrderLBL.Size = new System.Drawing.Size(0, 24);
			this.nameOrderLBL.TabIndex = 0;
			// 
			// F_UpdateOrder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(731, 450);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "F_UpdateOrder";
			this.Load += new System.EventHandler(this.F_UpdateOrder_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.orderDetailsDGV)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView orderDetailsDGV;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label nameOrderLBL;
		private System.Windows.Forms.Button updateBTN;
	}
}