namespace Dispetcher2
{
	partial class F_DeleteOrder
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(F_DeleteOrder));
			this.closeOrderBTN = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.orderListDGV = new System.Windows.Forms.DataGridView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button2 = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.gB_OrderNum_Search = new System.Windows.Forms.GroupBox();
			this.tB_OrderNum_Search = new System.Windows.Forms.TextBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.closeOrderCB = new System.Windows.Forms.CheckBox();
			this.openOrderCB = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.orderListDGV)).BeginInit();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.gB_OrderNum_Search.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// closeOrderBTN
			// 
			this.closeOrderBTN.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.closeOrderBTN.Location = new System.Drawing.Point(633, 3);
			this.closeOrderBTN.Name = "closeOrderBTN";
			this.closeOrderBTN.Size = new System.Drawing.Size(155, 32);
			this.closeOrderBTN.TabIndex = 0;
			this.closeOrderBTN.Text = "Закрыть заказы";
			this.closeOrderBTN.UseVisualStyleBackColor = true;
			this.closeOrderBTN.Click += new System.EventHandler(this.closeOrderBTN_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(9, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(391, 18);
			this.label1.TabIndex = 1;
			this.label1.Text = "Выберете заказы, статус которых вы хотите изменить";
			// 
			// orderListDGV
			// 
			this.orderListDGV.AllowUserToAddRows = false;
			this.orderListDGV.AllowUserToDeleteRows = false;
			this.orderListDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.orderListDGV.Dock = System.Windows.Forms.DockStyle.Fill;
			this.orderListDGV.Location = new System.Drawing.Point(3, 81);
			this.orderListDGV.Name = "orderListDGV";
			this.orderListDGV.RowHeadersVisible = false;
			this.orderListDGV.Size = new System.Drawing.Size(794, 366);
			this.orderListDGV.TabIndex = 2;
			this.orderListDGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.orderListDGV_CellContentClick);
			// 
			// panel1
			// 
			this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Controls.Add(this.gB_OrderNum_Search);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.closeOrderBTN);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(794, 72);
			this.panel1.TabIndex = 3;
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(633, 41);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(155, 25);
			this.button2.TabIndex = 2;
			this.button2.Text = "Заказы в режим ожидания";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.orderListDGV, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// gB_OrderNum_Search
			// 
			this.gB_OrderNum_Search.Controls.Add(this.tB_OrderNum_Search);
			this.gB_OrderNum_Search.Location = new System.Drawing.Point(9, 29);
			this.gB_OrderNum_Search.Name = "gB_OrderNum_Search";
			this.gB_OrderNum_Search.Size = new System.Drawing.Size(183, 41);
			this.gB_OrderNum_Search.TabIndex = 3;
			this.gB_OrderNum_Search.TabStop = false;
			this.gB_OrderNum_Search.Text = "Поиск по номеру заказа";
			// 
			// tB_OrderNum_Search
			// 
			this.tB_OrderNum_Search.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tB_OrderNum_Search.Location = new System.Drawing.Point(3, 16);
			this.tB_OrderNum_Search.MaxLength = 50;
			this.tB_OrderNum_Search.Name = "tB_OrderNum_Search";
			this.tB_OrderNum_Search.Size = new System.Drawing.Size(177, 20);
			this.tB_OrderNum_Search.TabIndex = 2;
			this.tB_OrderNum_Search.TextChanged += new System.EventHandler(this.tB_OrderNum_Search_TextChanged);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.openOrderCB);
			this.panel3.Controls.Add(this.closeOrderCB);
			this.panel3.Location = new System.Drawing.Point(483, 3);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(104, 62);
			this.panel3.TabIndex = 21;
			// 
			// closeOrderCB
			// 
			this.closeOrderCB.AutoSize = true;
			this.closeOrderCB.Dock = System.Windows.Forms.DockStyle.Top;
			this.closeOrderCB.Location = new System.Drawing.Point(0, 0);
			this.closeOrderCB.Margin = new System.Windows.Forms.Padding(4);
			this.closeOrderCB.Name = "closeOrderCB";
			this.closeOrderCB.Size = new System.Drawing.Size(104, 17);
			this.closeOrderCB.TabIndex = 4;
			this.closeOrderCB.Text = "Закрытые";
			this.closeOrderCB.UseVisualStyleBackColor = true;
			// 
			// openOrderCB
			// 
			this.openOrderCB.AutoSize = true;
			this.openOrderCB.Dock = System.Windows.Forms.DockStyle.Top;
			this.openOrderCB.Location = new System.Drawing.Point(0, 17);
			this.openOrderCB.Margin = new System.Windows.Forms.Padding(4);
			this.openOrderCB.Name = "openOrderCB";
			this.openOrderCB.Size = new System.Drawing.Size(104, 17);
			this.openOrderCB.TabIndex = 5;
			this.openOrderCB.Text = "Открытые";
			this.openOrderCB.UseVisualStyleBackColor = true;
			// 
			// F_DeleteOrder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "F_DeleteOrder";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Изменение статуса заказа";
			((System.ComponentModel.ISupportInitialize)(this.orderListDGV)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.gB_OrderNum_Search.ResumeLayout(false);
			this.gB_OrderNum_Search.PerformLayout();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button closeOrderBTN;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.DataGridView orderListDGV;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.GroupBox gB_OrderNum_Search;
		private System.Windows.Forms.TextBox tB_OrderNum_Search;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.CheckBox openOrderCB;
		private System.Windows.Forms.CheckBox closeOrderCB;
	}
}