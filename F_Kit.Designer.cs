namespace Dispetcher2
{
    partial class F_Kit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dGV_Kit = new System.Windows.Forms.DataGridView();
            this.Col_NumRow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_SHCM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_NameKit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_PlanKit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_FactKit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Order = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_IdLoodsmanKit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_1C_loodsman_IdKit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_1C_IdKit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dGV_KitcontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.копироватьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dGV_Orders = new System.Windows.Forms.DataGridView();
            this.Col_PK_IdOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_OrderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dGV_OrderscontextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.копироватьВБуферToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tB_OrderNum = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tB_IdLoodsmanKit = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lbl_RowsCount = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tB_NameKit = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tB_ShcmDetail = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chB_Summary = new System.Windows.Forms.CheckBox();
            this.radioBtn_Loodsman = new System.Windows.Forms.RadioButton();
            this.chB_235Kit = new System.Windows.Forms.CheckBox();
            this.radioBtn_Disp = new System.Windows.Forms.RadioButton();
            this.btn_OrderDetails = new System.Windows.Forms.Button();
            this.btn_ExpKitToExcel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Kit)).BeginInit();
            this.dGV_KitcontextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orders)).BeginInit();
            this.dGV_OrderscontextMenuStrip.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox5, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.dGV_Orders, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox6, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 63F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1000, 632);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dGV_Kit);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(161, 55);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(827, 491);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            // 
            // dGV_Kit
            // 
            this.dGV_Kit.AllowUserToAddRows = false;
            this.dGV_Kit.AllowUserToDeleteRows = false;
            this.dGV_Kit.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Kit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Kit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Kit.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_NumRow,
            this.Col_Position,
            this.Col_SHCM,
            this.Col_Amount,
            this.Col_NameKit,
            this.Col_PlanKit,
            this.Col_FactKit,
            this.Col_Order,
            this.Col_IdLoodsmanKit,
            this.Col_1C_loodsman_IdKit,
            this.Col_1C_IdKit});
            this.dGV_Kit.ContextMenuStrip = this.dGV_KitcontextMenuStrip;
            this.dGV_Kit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Kit.Location = new System.Drawing.Point(3, 19);
            this.dGV_Kit.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Kit.MultiSelect = false;
            this.dGV_Kit.Name = "dGV_Kit";
            this.dGV_Kit.ReadOnly = true;
            this.dGV_Kit.RowHeadersWidth = 4;
            this.dGV_Kit.Size = new System.Drawing.Size(821, 469);
            this.dGV_Kit.TabIndex = 14;
            this.dGV_Kit.TabStop = false;
            this.dGV_Kit.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dGV_Kit_CellMouseDown);
            // 
            // Col_NumRow
            // 
            this.Col_NumRow.Frozen = true;
            this.Col_NumRow.HeaderText = "№";
            this.Col_NumRow.MinimumWidth = 20;
            this.Col_NumRow.Name = "Col_NumRow";
            this.Col_NumRow.ReadOnly = true;
            this.Col_NumRow.Width = 60;
            // 
            // Col_Position
            // 
            this.Col_Position.HeaderText = "П";
            this.Col_Position.MinimumWidth = 30;
            this.Col_Position.Name = "Col_Position";
            this.Col_Position.ReadOnly = true;
            this.Col_Position.ToolTipText = "Позиция";
            this.Col_Position.Width = 70;
            // 
            // Col_SHCM
            // 
            this.Col_SHCM.HeaderText = "ЩЦМ";
            this.Col_SHCM.MinimumWidth = 20;
            this.Col_SHCM.Name = "Col_SHCM";
            this.Col_SHCM.ReadOnly = true;
            this.Col_SHCM.Width = 180;
            // 
            // Col_Amount
            // 
            this.Col_Amount.HeaderText = "Кол-во";
            this.Col_Amount.MinimumWidth = 20;
            this.Col_Amount.Name = "Col_Amount";
            this.Col_Amount.ReadOnly = true;
            this.Col_Amount.Width = 80;
            // 
            // Col_NameKit
            // 
            this.Col_NameKit.HeaderText = "Наименование комплектации";
            this.Col_NameKit.MinimumWidth = 100;
            this.Col_NameKit.Name = "Col_NameKit";
            this.Col_NameKit.ReadOnly = true;
            this.Col_NameKit.Width = 370;
            // 
            // Col_PlanKit
            // 
            this.Col_PlanKit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Col_PlanKit.HeaderText = "План";
            this.Col_PlanKit.MinimumWidth = 50;
            this.Col_PlanKit.Name = "Col_PlanKit";
            this.Col_PlanKit.ReadOnly = true;
            this.Col_PlanKit.Width = 50;
            // 
            // Col_FactKit
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Col_FactKit.DefaultCellStyle = dataGridViewCellStyle1;
            this.Col_FactKit.HeaderText = "Факт";
            this.Col_FactKit.MinimumWidth = 50;
            this.Col_FactKit.Name = "Col_FactKit";
            this.Col_FactKit.ReadOnly = true;
            this.Col_FactKit.Visible = false;
            this.Col_FactKit.Width = 50;
            // 
            // Col_Order
            // 
            this.Col_Order.HeaderText = "Заказ";
            this.Col_Order.MinimumWidth = 50;
            this.Col_Order.Name = "Col_Order";
            this.Col_Order.ReadOnly = true;
            this.Col_Order.Width = 160;
            // 
            // Col_IdLoodsmanKit
            // 
            this.Col_IdLoodsmanKit.HeaderText = "№к.Лоц.";
            this.Col_IdLoodsmanKit.Name = "Col_IdLoodsmanKit";
            this.Col_IdLoodsmanKit.ReadOnly = true;
            // 
            // Col_1C_loodsman_IdKit
            // 
            this.Col_1C_loodsman_IdKit.HeaderText = "№к.Лоц.1С";
            this.Col_1C_loodsman_IdKit.Name = "Col_1C_loodsman_IdKit";
            this.Col_1C_loodsman_IdKit.ReadOnly = true;
            this.Col_1C_loodsman_IdKit.Visible = false;
            // 
            // Col_1C_IdKit
            // 
            this.Col_1C_IdKit.HeaderText = "№комп.1С ";
            this.Col_1C_IdKit.Name = "Col_1C_IdKit";
            this.Col_1C_IdKit.ReadOnly = true;
            this.Col_1C_IdKit.Visible = false;
            // 
            // dGV_KitcontextMenuStrip
            // 
            this.dGV_KitcontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.копироватьToolStripMenuItem});
            this.dGV_KitcontextMenuStrip.Name = "contextMenuStrip1";
            this.dGV_KitcontextMenuStrip.Size = new System.Drawing.Size(187, 26);
            // 
            // копироватьToolStripMenuItem
            // 
            this.копироватьToolStripMenuItem.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.копироватьToolStripMenuItem.Image = global::Dispetcher2.Properties.Resources.icons8_скопировать_30;
            this.копироватьToolStripMenuItem.Name = "копироватьToolStripMenuItem";
            this.копироватьToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.копироватьToolStripMenuItem.Text = "Копировать в буфер";
            this.копироватьToolStripMenuItem.Click += new System.EventHandler(this.копироватьToolStripMenuItem_Click);
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
            this.Col_OrderNum});
            this.dGV_Orders.ContextMenuStrip = this.dGV_OrderscontextMenuStrip;
            this.dGV_Orders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Orders.Location = new System.Drawing.Point(12, 56);
            this.dGV_Orders.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Orders.Name = "dGV_Orders";
            this.dGV_Orders.ReadOnly = true;
            this.dGV_Orders.RowHeadersWidth = 4;
            this.dGV_Orders.Size = new System.Drawing.Size(142, 489);
            this.dGV_Orders.TabIndex = 12;
            this.dGV_Orders.TabStop = false;
            this.dGV_Orders.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dGV_Orders_CellMouseDown);
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
            // dGV_OrderscontextMenuStrip
            // 
            this.dGV_OrderscontextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.копироватьВБуферToolStripMenuItem});
            this.dGV_OrderscontextMenuStrip.Name = "dGV_KitcontextMenuStrip";
            this.dGV_OrderscontextMenuStrip.Size = new System.Drawing.Size(187, 26);
            // 
            // копироватьВБуферToolStripMenuItem
            // 
            this.копироватьВБуферToolStripMenuItem.Image = global::Dispetcher2.Properties.Resources.icons8_скопировать_30;
            this.копироватьВБуферToolStripMenuItem.Name = "копироватьВБуферToolStripMenuItem";
            this.копироватьВБуферToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.копироватьВБуферToolStripMenuItem.Text = "Копировать в буфер";
            this.копироватьВБуферToolStripMenuItem.Click += new System.EventHandler(this.копироватьВБуферToolStripMenuItem_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tB_OrderNum);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox6.Location = new System.Drawing.Point(11, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(144, 46);
            this.groupBox6.TabIndex = 2;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Поиск (№ заказа)";
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
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox7);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(158, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(833, 52);
            this.panel1.TabIndex = 14;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tB_IdLoodsmanKit);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox7.Location = new System.Drawing.Point(417, 0);
            this.groupBox7.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(232, 52);
            this.groupBox7.TabIndex = 16;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "№к.Лоц.";
            // 
            // tB_IdLoodsmanKit
            // 
            this.tB_IdLoodsmanKit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_IdLoodsmanKit.Location = new System.Drawing.Point(3, 19);
            this.tB_IdLoodsmanKit.MaxLength = 50;
            this.tB_IdLoodsmanKit.Name = "tB_IdLoodsmanKit";
            this.tB_IdLoodsmanKit.Size = new System.Drawing.Size(226, 23);
            this.tB_IdLoodsmanKit.TabIndex = 3;
            this.tB_IdLoodsmanKit.TextChanged += new System.EventHandler(this.tB_IdLoodsmanKit_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lbl_RowsCount);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox4.Location = new System.Drawing.Point(730, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(103, 52);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Всего строк";
            // 
            // lbl_RowsCount
            // 
            this.lbl_RowsCount.AutoSize = true;
            this.lbl_RowsCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_RowsCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl_RowsCount.Location = new System.Drawing.Point(3, 19);
            this.lbl_RowsCount.Name = "lbl_RowsCount";
            this.lbl_RowsCount.Size = new System.Drawing.Size(92, 20);
            this.lbl_RowsCount.TabIndex = 0;
            this.lbl_RowsCount.Text = "RowsCount";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tB_NameKit);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(185, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(232, 52);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Наименование комплектации";
            // 
            // tB_NameKit
            // 
            this.tB_NameKit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_NameKit.Location = new System.Drawing.Point(3, 19);
            this.tB_NameKit.MaxLength = 50;
            this.tB_NameKit.Name = "tB_NameKit";
            this.tB_NameKit.Size = new System.Drawing.Size(226, 23);
            this.tB_NameKit.TabIndex = 3;
            this.tB_NameKit.TextChanged += new System.EventHandler(this.tB_NameKit_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tB_ShcmDetail);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(185, 52);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ЩЦМ детали";
            // 
            // tB_ShcmDetail
            // 
            this.tB_ShcmDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_ShcmDetail.Location = new System.Drawing.Point(3, 19);
            this.tB_ShcmDetail.MaxLength = 50;
            this.tB_ShcmDetail.Name = "tB_ShcmDetail";
            this.tB_ShcmDetail.Size = new System.Drawing.Size(179, 23);
            this.tB_ShcmDetail.TabIndex = 2;
            this.tB_ShcmDetail.TextChanged += new System.EventHandler(this.tB_ShcmDetail_TextChanged);
            this.tB_ShcmDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_ShcmDetail_KeyDown);
            this.tB_ShcmDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_ShcmDetail_KeyPress);
            // 
            // groupBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 2);
            this.groupBox2.Controls.Add(this.chB_Summary);
            this.groupBox2.Controls.Add(this.radioBtn_Loodsman);
            this.groupBox2.Controls.Add(this.chB_235Kit);
            this.groupBox2.Controls.Add(this.radioBtn_Disp);
            this.groupBox2.Controls.Add(this.btn_OrderDetails);
            this.groupBox2.Controls.Add(this.btn_ExpKitToExcel);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(10, 551);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(979, 59);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // chB_Summary
            // 
            this.chB_Summary.AutoSize = true;
            this.chB_Summary.Location = new System.Drawing.Point(482, 24);
            this.chB_Summary.Name = "chB_Summary";
            this.chB_Summary.Size = new System.Drawing.Size(83, 21);
            this.chB_Summary.TabIndex = 9;
            this.chB_Summary.Text = "Сводная";
            this.chB_Summary.UseVisualStyleBackColor = true;
            this.chB_Summary.CheckedChanged += new System.EventHandler(this.chB_Summary_CheckedChanged);
            // 
            // radioBtn_Loodsman
            // 
            this.radioBtn_Loodsman.AutoSize = true;
            this.radioBtn_Loodsman.Location = new System.Drawing.Point(154, 24);
            this.radioBtn_Loodsman.Name = "radioBtn_Loodsman";
            this.radioBtn_Loodsman.Size = new System.Drawing.Size(111, 21);
            this.radioBtn_Loodsman.TabIndex = 5;
            this.radioBtn_Loodsman.Text = "БД \"Лоцман\"";
            this.radioBtn_Loodsman.UseVisualStyleBackColor = true;
            this.radioBtn_Loodsman.CheckedChanged += new System.EventHandler(this.radioBtn_Loodsman_CheckedChanged);
            // 
            // chB_235Kit
            // 
            this.chB_235Kit.AutoSize = true;
            this.chB_235Kit.Location = new System.Drawing.Point(271, 25);
            this.chB_235Kit.Name = "chB_235Kit";
            this.chB_235Kit.Size = new System.Drawing.Size(128, 21);
            this.chB_235Kit.TabIndex = 6;
            this.chB_235Kit.Text = "+ Стандартные";
            this.chB_235Kit.UseVisualStyleBackColor = true;
            this.chB_235Kit.CheckedChanged += new System.EventHandler(this.chB_235Kit_CheckedChanged);
            // 
            // radioBtn_Disp
            // 
            this.radioBtn_Disp.AutoSize = true;
            this.radioBtn_Disp.Checked = true;
            this.radioBtn_Disp.Location = new System.Drawing.Point(17, 23);
            this.radioBtn_Disp.Name = "radioBtn_Disp";
            this.radioBtn_Disp.Size = new System.Drawing.Size(133, 21);
            this.radioBtn_Disp.TabIndex = 4;
            this.radioBtn_Disp.TabStop = true;
            this.radioBtn_Disp.Text = "БД \"Диспетчер\"";
            this.radioBtn_Disp.UseVisualStyleBackColor = true;
            this.radioBtn_Disp.CheckedChanged += new System.EventHandler(this.radioBtn_Disp_CheckedChanged);
            // 
            // btn_OrderDetails
            // 
            this.btn_OrderDetails.Location = new System.Drawing.Point(570, 13);
            this.btn_OrderDetails.Margin = new System.Windows.Forms.Padding(2);
            this.btn_OrderDetails.Name = "btn_OrderDetails";
            this.btn_OrderDetails.Size = new System.Drawing.Size(160, 40);
            this.btn_OrderDetails.TabIndex = 7;
            this.btn_OrderDetails.Text = "Сформировать";
            this.btn_OrderDetails.UseVisualStyleBackColor = false;
            this.btn_OrderDetails.Click += new System.EventHandler(this.btn_OrderDetails_Click);
            // 
            // btn_ExpKitToExcel
            // 
            this.btn_ExpKitToExcel.Location = new System.Drawing.Point(813, 13);
            this.btn_ExpKitToExcel.Margin = new System.Windows.Forms.Padding(2);
            this.btn_ExpKitToExcel.Name = "btn_ExpKitToExcel";
            this.btn_ExpKitToExcel.Size = new System.Drawing.Size(160, 40);
            this.btn_ExpKitToExcel.TabIndex = 8;
            this.btn_ExpKitToExcel.Text = "Excel";
            this.btn_ExpKitToExcel.UseVisualStyleBackColor = false;
            this.btn_ExpKitToExcel.Click += new System.EventHandler(this.btn_ExpKitToExcel_Click);
            // 
            // F_Kit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(1000, 632);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_Kit";
            this.Text = "Комплектация";
            this.Load += new System.EventHandler(this.F_Kit_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Kit)).EndInit();
            this.dGV_KitcontextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orders)).EndInit();
            this.dGV_OrderscontextMenuStrip.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox tB_OrderNum;
        private System.Windows.Forms.DataGridView dGV_Orders;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tB_ShcmDetail;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DataGridView dGV_Kit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_ExpKitToExcel;
        private System.Windows.Forms.Button btn_OrderDetails;
        private System.Windows.Forms.RadioButton radioBtn_Disp;
        private System.Windows.Forms.CheckBox chB_235Kit;
        private System.Windows.Forms.RadioButton radioBtn_Loodsman;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tB_NameKit;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lbl_RowsCount;
        private System.Windows.Forms.CheckBox chB_Summary;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_PK_IdOrder;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_OrderNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_NumRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_SHCM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_NameKit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_PlanKit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FactKit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Order;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_IdLoodsmanKit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_1C_loodsman_IdKit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_1C_IdKit;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TextBox tB_IdLoodsmanKit;
        private System.Windows.Forms.ContextMenuStrip dGV_KitcontextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem копироватьToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip dGV_OrderscontextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem копироватьВБуферToolStripMenuItem;
    }
}