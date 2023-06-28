namespace Dispetcher2
{
    partial class F_Planning
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
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.gB_OrderName = new System.Windows.Forms.GroupBox();
            this.mTB_StartOrdDate = new System.Windows.Forms.MaskedTextBox();
            this.mTB_PlannedDate = new System.Windows.Forms.MaskedTextBox();
            this.tB_Amount = new System.Windows.Forms.TextBox();
            this.tB_OrderName = new System.Windows.Forms.TextBox();
            this.tB_OrderNumInfo = new System.Windows.Forms.TextBox();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.tB_OrderNum = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeViewOrdersDetails = new System.Windows.Forms.TreeView();
            this.gBoxSearchOrders = new System.Windows.Forms.GroupBox();
            this.tB_SHCM_S_InOrders = new System.Windows.Forms.TextBox();
            this.myGrid_Gant = new Dispetcher2.Controls.MyGrid.MyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dGV_Orders = new System.Windows.Forms.DataGridView();
            this.Col_PK_IdOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_OrderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_OrderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Gant = new System.Windows.Forms.Button();
            this.tableLayoutPanel2.SuspendLayout();
            this.gB_OrderName.SuspendLayout();
            this.groupBox17.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gBoxSearchOrders.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orders)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.gB_OrderName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox17, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.splitContainer1, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(984, 632);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // gB_OrderName
            // 
            this.gB_OrderName.Controls.Add(this.mTB_StartOrdDate);
            this.gB_OrderName.Controls.Add(this.mTB_PlannedDate);
            this.gB_OrderName.Controls.Add(this.tB_Amount);
            this.gB_OrderName.Controls.Add(this.tB_OrderName);
            this.gB_OrderName.Controls.Add(this.tB_OrderNumInfo);
            this.gB_OrderName.Location = new System.Drawing.Point(153, 3);
            this.gB_OrderName.Name = "gB_OrderName";
            this.gB_OrderName.Size = new System.Drawing.Size(808, 46);
            this.gB_OrderName.TabIndex = 1000;
            this.gB_OrderName.TabStop = false;
            this.gB_OrderName.Text = "№ заказа                   Наименование заказа                                   " +
                "                     Дата запуска  Пл. дата сдачи  Кол-во,шт";
            // 
            // mTB_StartOrdDate
            // 
            this.mTB_StartOrdDate.BackColor = System.Drawing.SystemColors.Info;
            this.mTB_StartOrdDate.Location = new System.Drawing.Point(518, 19);
            this.mTB_StartOrdDate.Mask = "00/00/0000";
            this.mTB_StartOrdDate.Name = "mTB_StartOrdDate";
            this.mTB_StartOrdDate.ReadOnly = true;
            this.mTB_StartOrdDate.Size = new System.Drawing.Size(93, 23);
            this.mTB_StartOrdDate.TabIndex = 26;
            this.mTB_StartOrdDate.ValidatingType = typeof(System.DateTime);
            // 
            // mTB_PlannedDate
            // 
            this.mTB_PlannedDate.BackColor = System.Drawing.SystemColors.Info;
            this.mTB_PlannedDate.Location = new System.Drawing.Point(617, 19);
            this.mTB_PlannedDate.Mask = "00/00/0000";
            this.mTB_PlannedDate.Name = "mTB_PlannedDate";
            this.mTB_PlannedDate.ReadOnly = true;
            this.mTB_PlannedDate.Size = new System.Drawing.Size(101, 23);
            this.mTB_PlannedDate.TabIndex = 25;
            this.mTB_PlannedDate.ValidatingType = typeof(System.DateTime);
            // 
            // tB_Amount
            // 
            this.tB_Amount.BackColor = System.Drawing.SystemColors.Info;
            this.tB_Amount.Location = new System.Drawing.Point(725, 19);
            this.tB_Amount.Margin = new System.Windows.Forms.Padding(4);
            this.tB_Amount.MaxLength = 20;
            this.tB_Amount.Name = "tB_Amount";
            this.tB_Amount.ReadOnly = true;
            this.tB_Amount.Size = new System.Drawing.Size(76, 23);
            this.tB_Amount.TabIndex = 33;
            this.tB_Amount.TabStop = false;
            // 
            // tB_OrderName
            // 
            this.tB_OrderName.BackColor = System.Drawing.SystemColors.Info;
            this.tB_OrderName.Location = new System.Drawing.Point(148, 19);
            this.tB_OrderName.Margin = new System.Windows.Forms.Padding(4);
            this.tB_OrderName.MaxLength = 20;
            this.tB_OrderName.Name = "tB_OrderName";
            this.tB_OrderName.ReadOnly = true;
            this.tB_OrderName.Size = new System.Drawing.Size(363, 23);
            this.tB_OrderName.TabIndex = 32;
            this.tB_OrderName.TabStop = false;
            // 
            // tB_OrderNumInfo
            // 
            this.tB_OrderNumInfo.BackColor = System.Drawing.SystemColors.Info;
            this.tB_OrderNumInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.tB_OrderNumInfo.Location = new System.Drawing.Point(3, 19);
            this.tB_OrderNumInfo.Margin = new System.Windows.Forms.Padding(4);
            this.tB_OrderNumInfo.MaxLength = 20;
            this.tB_OrderNumInfo.Name = "tB_OrderNumInfo";
            this.tB_OrderNumInfo.ReadOnly = true;
            this.tB_OrderNumInfo.Size = new System.Drawing.Size(137, 23);
            this.tB_OrderNumInfo.TabIndex = 3;
            this.tB_OrderNumInfo.TabStop = false;
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.tB_OrderNum);
            this.groupBox17.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox17.Location = new System.Drawing.Point(3, 3);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(144, 46);
            this.groupBox17.TabIndex = 2;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = "Поиск (№ заказа)";
            // 
            // tB_OrderNum
            // 
            this.tB_OrderNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_OrderNum.Location = new System.Drawing.Point(3, 19);
            this.tB_OrderNum.MaxLength = 50;
            this.tB_OrderNum.Name = "tB_OrderNum";
            this.tB_OrderNum.Size = new System.Drawing.Size(138, 23);
            this.tB_OrderNum.TabIndex = 1;
            this.tB_OrderNum.TextChanged += new System.EventHandler(this.tB_OrderNum_TextChanged);
            this.tB_OrderNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_OrderNum_KeyDown);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(153, 55);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeViewOrdersDetails);
            this.splitContainer1.Panel1.Controls.Add(this.gBoxSearchOrders);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.myGrid_Gant);
            this.splitContainer1.Size = new System.Drawing.Size(808, 487);
            this.splitContainer1.SplitterDistance = 240;
            this.splitContainer1.SplitterIncrement = 2;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 1001;
            // 
            // treeViewOrdersDetails
            // 
            this.treeViewOrdersDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewOrdersDetails.Location = new System.Drawing.Point(0, 46);
            this.treeViewOrdersDetails.Name = "treeViewOrdersDetails";
            this.treeViewOrdersDetails.Size = new System.Drawing.Size(240, 441);
            this.treeViewOrdersDetails.TabIndex = 5;
            this.treeViewOrdersDetails.TabStop = false;
            // 
            // gBoxSearchOrders
            // 
            this.gBoxSearchOrders.Controls.Add(this.tB_SHCM_S_InOrders);
            this.gBoxSearchOrders.Dock = System.Windows.Forms.DockStyle.Top;
            this.gBoxSearchOrders.Location = new System.Drawing.Point(0, 0);
            this.gBoxSearchOrders.Name = "gBoxSearchOrders";
            this.gBoxSearchOrders.Size = new System.Drawing.Size(240, 46);
            this.gBoxSearchOrders.TabIndex = 2;
            this.gBoxSearchOrders.TabStop = false;
            this.gBoxSearchOrders.Text = "Поиск по заказу";
            // 
            // tB_SHCM_S_InOrders
            // 
            this.tB_SHCM_S_InOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_SHCM_S_InOrders.Location = new System.Drawing.Point(3, 19);
            this.tB_SHCM_S_InOrders.Name = "tB_SHCM_S_InOrders";
            this.tB_SHCM_S_InOrders.Size = new System.Drawing.Size(234, 23);
            this.tB_SHCM_S_InOrders.TabIndex = 1;
            this.tB_SHCM_S_InOrders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_SHCM_S_InOrders_KeyDown);
            this.tB_SHCM_S_InOrders.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_SHCM_S_InOrders_KeyPress);
            // 
            // myGrid_Gant
            // 
            this.myGrid_Gant.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myGrid_Gant.EnableSort = false;
            this.myGrid_Gant.Location = new System.Drawing.Point(0, 0);
            this.myGrid_Gant.Name = "myGrid_Gant";
            this.myGrid_Gant.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.myGrid_Gant.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.myGrid_Gant.Size = new System.Drawing.Size(558, 487);
            this.myGrid_Gant.TabIndex = 0;
            this.myGrid_Gant.TabStop = true;
            this.myGrid_Gant.ToolTipText = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dGV_Orders);
            this.panel1.Controls.Add(this.btn_Gant);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(144, 487);
            this.panel1.TabIndex = 1003;
            // 
            // dGV_Orders
            // 
            this.dGV_Orders.AllowUserToAddRows = false;
            this.dGV_Orders.AllowUserToDeleteRows = false;
            this.dGV_Orders.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Orders.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Orders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Orders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_PK_IdOrder,
            this.Col_OrderNum,
            this.Col_OrderName});
            this.dGV_Orders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Orders.Location = new System.Drawing.Point(0, 0);
            this.dGV_Orders.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Orders.MultiSelect = false;
            this.dGV_Orders.Name = "dGV_Orders";
            this.dGV_Orders.ReadOnly = true;
            this.dGV_Orders.RowHeadersWidth = 4;
            this.dGV_Orders.Size = new System.Drawing.Size(144, 447);
            this.dGV_Orders.TabIndex = 1002;
            this.dGV_Orders.TabStop = false;
            this.dGV_Orders.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dGV_Orders_CellMouseDoubleClick);
            this.dGV_Orders.SelectionChanged += new System.EventHandler(this.dGV_Orders_SelectionChanged);
            this.dGV_Orders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_Orders_KeyDown);
            // 
            // Col_PK_IdOrder
            // 
            this.Col_PK_IdOrder.Frozen = true;
            this.Col_PK_IdOrder.HeaderText = "PK_IdOrder";
            this.Col_PK_IdOrder.Name = "Col_PK_IdOrder";
            this.Col_PK_IdOrder.ReadOnly = true;
            this.Col_PK_IdOrder.Visible = false;
            // 
            // Col_OrderNum
            // 
            this.Col_OrderNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Col_OrderNum.Frozen = true;
            this.Col_OrderNum.HeaderText = "№ заказа";
            this.Col_OrderNum.MinimumWidth = 100;
            this.Col_OrderNum.Name = "Col_OrderNum";
            this.Col_OrderNum.ReadOnly = true;
            this.Col_OrderNum.Width = 136;
            // 
            // Col_OrderName
            // 
            this.Col_OrderName.HeaderText = "OrderName";
            this.Col_OrderName.Name = "Col_OrderName";
            this.Col_OrderName.ReadOnly = true;
            this.Col_OrderName.Visible = false;
            // 
            // btn_Gant
            // 
            this.btn_Gant.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_Gant.Location = new System.Drawing.Point(0, 447);
            this.btn_Gant.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Gant.Name = "btn_Gant";
            this.btn_Gant.Size = new System.Drawing.Size(144, 40);
            this.btn_Gant.TabIndex = 1003;
            this.btn_Gant.Text = "Сформировать";
            this.btn_Gant.UseVisualStyleBackColor = false;
            this.btn_Gant.Click += new System.EventHandler(this.btn_Gant_Click);
            // 
            // F_Planning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(984, 632);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_Planning";
            this.Text = "ПРОИЗВОДСТВО-ПЛАН";
            this.Load += new System.EventHandler(this.F_Planning_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.gB_OrderName.ResumeLayout(false);
            this.gB_OrderName.PerformLayout();
            this.groupBox17.ResumeLayout(false);
            this.groupBox17.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gBoxSearchOrders.ResumeLayout(false);
            this.gBoxSearchOrders.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orders)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox gB_OrderName;
        private System.Windows.Forms.TextBox tB_OrderNumInfo;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.TextBox tB_OrderNum;
        private System.Windows.Forms.TextBox tB_OrderName;
        private System.Windows.Forms.TextBox tB_Amount;
        private System.Windows.Forms.MaskedTextBox mTB_PlannedDate;
        private System.Windows.Forms.MaskedTextBox mTB_StartOrdDate;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox gBoxSearchOrders;
        private System.Windows.Forms.TextBox tB_SHCM_S_InOrders;
        private System.Windows.Forms.TreeView treeViewOrdersDetails;
        private System.Windows.Forms.DataGridView dGV_Orders;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_PK_IdOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_OrderNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_OrderName;
        private Controls.MyGrid.MyGrid myGrid_Gant;
        private System.Windows.Forms.Button btn_Gant;
        private System.Windows.Forms.Panel panel1;

    }
}