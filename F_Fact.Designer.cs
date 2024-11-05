using System;

namespace Dispetcher2
{
    partial class F_Fact
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dGV_Orders = new System.Windows.Forms.DataGridView();
            this.Col_OrderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OrdersContextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.gB_OrderName = new System.Windows.Forms.GroupBox();
            this.tB_OrderNumInfo = new System.Windows.Forms.TextBox();
            this.tB_OrderName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tB_ShcmDetail = new System.Windows.Forms.TextBox();
            this.tB_OrderNum = new System.Windows.Forms.TextBox();
            this.dGV_Details = new System.Windows.Forms.DataGridView();
            this.Col_Position = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_ShcmDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_NameDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_NameType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shCMContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.oShCMContextMenuStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.dGV_Tehnology = new System.Windows.Forms.DataGridView();
            this.Col_Oper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Tpd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Tsh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dTimeP_Fact = new System.Windows.Forms.DateTimePicker();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnkoop = new System.Windows.Forms.Button();
            this.btn_CloseAllWorks = new System.Windows.Forms.Button();
            this.cB_InDetail = new System.Windows.Forms.CheckBox();
            this.mBtnM_Brigade = new Dispetcher2.Controls.MyButtonMenu();
            this.mBtnM_Worker = new Dispetcher2.Controls.MyButtonMenu();
            this.tB_Workers = new System.Windows.Forms.TextBox();
            this.btn_SaveTehnology = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.chB_cooperation = new System.Windows.Forms.CheckBox();
            this.nUpD_Tpd = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_SearchSHCM_F = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dGV_FactOperation = new System.Windows.Forms.DataGridView();
            this.Col_FactOper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_DateFactOper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_FK_LoginWorker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_AmountDetails = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_FactTpd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_FactTsh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orders)).BeginInit();
            this.OrderContextMenuStrip.SuspendLayout();
            this.gB_OrderName.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Details)).BeginInit();
            this.shCMContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Tehnology)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUpD_Tpd)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_FactOperation)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dGV_Orders
            // 
            this.dGV_Orders.AllowUserToAddRows = false;
            this.dGV_Orders.AllowUserToDeleteRows = false;
            this.dGV_Orders.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Orders.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Orders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Orders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_OrderNum});
            this.dGV_Orders.ContextMenuStrip = this.OrderContextMenuStrip;
            this.dGV_Orders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Orders.Location = new System.Drawing.Point(0, 46);
            this.dGV_Orders.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Orders.MultiSelect = false;
            this.dGV_Orders.Name = "dGV_Orders";
            this.dGV_Orders.ReadOnly = true;
            this.dGV_Orders.RowHeadersWidth = 4;
            this.dGV_Orders.Size = new System.Drawing.Size(139, 580);
            this.dGV_Orders.TabIndex = 11;
            this.dGV_Orders.TabStop = false;
            this.dGV_Orders.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dGV_Orders_CellMouseDown);
            this.dGV_Orders.SelectionChanged += new System.EventHandler(this.dGV_Orders_SelectionChanged);
            this.dGV_Orders.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_Orders_KeyDown);
            this.dGV_Orders.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dGV_Orders_MouseDoubleClick);
            // 
            // Col_OrderNum
            // 
            this.Col_OrderNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_OrderNum.HeaderText = "№ заказа";
            this.Col_OrderNum.MinimumWidth = 100;
            this.Col_OrderNum.Name = "Col_OrderNum";
            this.Col_OrderNum.ReadOnly = true;
            // 
            // OrderContextMenuStrip
            // 
            this.OrderContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OrdersContextMenuStrip});
            this.OrderContextMenuStrip.Name = "OrderContextMenuStrip";
            this.OrderContextMenuStrip.Size = new System.Drawing.Size(140, 26);
            // 
            // OrdersContextMenuStrip
            // 
            this.OrdersContextMenuStrip.Image = global::Dispetcher2.Properties.Resources.icons8_скопировать_30;
            this.OrdersContextMenuStrip.Name = "OrdersContextMenuStrip";
            this.OrdersContextMenuStrip.Size = new System.Drawing.Size(139, 22);
            this.OrdersContextMenuStrip.Text = "Копировать";
            this.OrdersContextMenuStrip.Click += new System.EventHandler(this.OrdersContextMenuStrip_Click);
            // 
            // gB_OrderName
            // 
            this.gB_OrderName.Controls.Add(this.tB_OrderNumInfo);
            this.gB_OrderName.Controls.Add(this.tB_OrderName);
            this.gB_OrderName.Location = new System.Drawing.Point(341, 1);
            this.gB_OrderName.Name = "gB_OrderName";
            this.gB_OrderName.Size = new System.Drawing.Size(493, 48);
            this.gB_OrderName.TabIndex = 999;
            this.gB_OrderName.TabStop = false;
            this.gB_OrderName.Text = "Наименование заказа";
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
            this.tB_OrderNumInfo.Size = new System.Drawing.Size(113, 23);
            this.tB_OrderNumInfo.TabIndex = 3;
            this.tB_OrderNumInfo.TabStop = false;
            // 
            // tB_OrderName
            // 
            this.tB_OrderName.BackColor = System.Drawing.SystemColors.Info;
            this.tB_OrderName.Dock = System.Windows.Forms.DockStyle.Right;
            this.tB_OrderName.Location = new System.Drawing.Point(133, 19);
            this.tB_OrderName.Margin = new System.Windows.Forms.Padding(4);
            this.tB_OrderName.MaxLength = 20;
            this.tB_OrderName.Name = "tB_OrderName";
            this.tB_OrderName.ReadOnly = true;
            this.tB_OrderName.Size = new System.Drawing.Size(357, 23);
            this.tB_OrderName.TabIndex = 2;
            this.tB_OrderName.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tB_ShcmDetail);
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(273, 48);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ЩЦМ детали (ENTER -поиск)";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // tB_ShcmDetail
            // 
            this.tB_ShcmDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_ShcmDetail.Location = new System.Drawing.Point(3, 19);
            this.tB_ShcmDetail.MaxLength = 50;
            this.tB_ShcmDetail.Name = "tB_ShcmDetail";
            this.tB_ShcmDetail.Size = new System.Drawing.Size(267, 23);
            this.tB_ShcmDetail.TabIndex = 2;
            this.tB_ShcmDetail.Click += new System.EventHandler(this.tB_ShcmDetail_Click);
            this.tB_ShcmDetail.TextChanged += new System.EventHandler(this.tB_ShcmDetail_TextChanged);
            this.tB_ShcmDetail.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_ShcmDetail_KeyDown);
            this.tB_ShcmDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_ShcmDetail_KeyPress);
            // 
            // tB_OrderNum
            // 
            this.tB_OrderNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_OrderNum.Location = new System.Drawing.Point(3, 19);
            this.tB_OrderNum.MaxLength = 50;
            this.tB_OrderNum.Name = "tB_OrderNum";
            this.tB_OrderNum.Size = new System.Drawing.Size(133, 23);
            this.tB_OrderNum.TabIndex = 1;
            this.tB_OrderNum.TextChanged += new System.EventHandler(this.tB_OrderNum_TextChanged);
            this.tB_OrderNum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tB_OrderNum_KeyDown);
            // 
            // dGV_Details
            // 
            this.dGV_Details.AllowUserToAddRows = false;
            this.dGV_Details.AllowUserToDeleteRows = false;
            this.dGV_Details.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Details.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Details.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_Position,
            this.Col_ShcmDetail,
            this.Col_NameDetail,
            this.Col_Amount,
            this.Col_NameType});
            this.dGV_Details.ContextMenuStrip = this.shCMContextMenuStrip;
            this.dGV_Details.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Details.Location = new System.Drawing.Point(3, 19);
            this.dGV_Details.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Details.MultiSelect = false;
            this.dGV_Details.Name = "dGV_Details";
            this.dGV_Details.ReadOnly = true;
            this.dGV_Details.RowHeadersWidth = 4;
            this.dGV_Details.Size = new System.Drawing.Size(825, 123);
            this.dGV_Details.TabIndex = 14;
            this.dGV_Details.TabStop = false;
            this.dGV_Details.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dGV_Details_CellMouseDown);
            this.dGV_Details.CurrentCellChanged += new System.EventHandler(this.dGV_Details_CurrentCellChanged);
            this.dGV_Details.SelectionChanged += new System.EventHandler(this.dGV_Details_SelectionChanged);
            this.dGV_Details.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_Details_KeyDown);
            // 
            // Col_Position
            // 
            this.Col_Position.Frozen = true;
            this.Col_Position.HeaderText = "П";
            this.Col_Position.MinimumWidth = 20;
            this.Col_Position.Name = "Col_Position";
            this.Col_Position.ReadOnly = true;
            this.Col_Position.ToolTipText = "Позиция";
            this.Col_Position.Width = 50;
            // 
            // Col_ShcmDetail
            // 
            this.Col_ShcmDetail.Frozen = true;
            this.Col_ShcmDetail.HeaderText = "ЩЦМ";
            this.Col_ShcmDetail.MinimumWidth = 50;
            this.Col_ShcmDetail.Name = "Col_ShcmDetail";
            this.Col_ShcmDetail.ReadOnly = true;
            this.Col_ShcmDetail.Width = 245;
            // 
            // Col_NameDetail
            // 
            this.Col_NameDetail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Col_NameDetail.Frozen = true;
            this.Col_NameDetail.HeaderText = "Название";
            this.Col_NameDetail.MinimumWidth = 50;
            this.Col_NameDetail.Name = "Col_NameDetail";
            this.Col_NameDetail.ReadOnly = true;
            this.Col_NameDetail.Width = 335;
            // 
            // Col_Amount
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Col_Amount.DefaultCellStyle = dataGridViewCellStyle1;
            this.Col_Amount.Frozen = true;
            this.Col_Amount.HeaderText = "Кол-во";
            this.Col_Amount.MinimumWidth = 20;
            this.Col_Amount.Name = "Col_Amount";
            this.Col_Amount.ReadOnly = true;
            this.Col_Amount.Width = 70;
            // 
            // Col_NameType
            // 
            this.Col_NameType.Frozen = true;
            this.Col_NameType.HeaderText = "Тип";
            this.Col_NameType.MinimumWidth = 20;
            this.Col_NameType.Name = "Col_NameType";
            this.Col_NameType.ReadOnly = true;
            this.Col_NameType.ToolTipText = "Тип";
            this.Col_NameType.Width = 105;
            // 
            // shCMContextMenuStrip
            // 
            this.shCMContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oShCMContextMenuStrip});
            this.shCMContextMenuStrip.Name = "shCMContextMenuStrip";
            this.shCMContextMenuStrip.Size = new System.Drawing.Size(140, 26);
            // 
            // oShCMContextMenuStrip
            // 
            this.oShCMContextMenuStrip.Image = global::Dispetcher2.Properties.Resources.icons8_скопировать_30;
            this.oShCMContextMenuStrip.Name = "oShCMContextMenuStrip";
            this.oShCMContextMenuStrip.Size = new System.Drawing.Size(139, 22);
            this.oShCMContextMenuStrip.Text = "Копировать";
            this.oShCMContextMenuStrip.Click += new System.EventHandler(this.oShCMContextMenuStrip_Click);
            // 
            // dGV_Tehnology
            // 
            this.dGV_Tehnology.AllowUserToAddRows = false;
            this.dGV_Tehnology.AllowUserToDeleteRows = false;
            this.dGV_Tehnology.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Tehnology.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Tehnology.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Tehnology.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_Oper,
            this.Col_Tpd,
            this.Col_Tsh});
            this.dGV_Tehnology.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Tehnology.Location = new System.Drawing.Point(3, 19);
            this.dGV_Tehnology.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Tehnology.MultiSelect = false;
            this.dGV_Tehnology.Name = "dGV_Tehnology";
            this.dGV_Tehnology.RowHeadersWidth = 4;
            this.dGV_Tehnology.Size = new System.Drawing.Size(479, 226);
            this.dGV_Tehnology.TabIndex = 15;
            this.dGV_Tehnology.TabStop = false;
            this.dGV_Tehnology.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.DGV_Tehnology_DefaultValuesNeeded_1);
            // 
            // Col_Oper
            // 
            this.Col_Oper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_Oper.HeaderText = "Операция";
            this.Col_Oper.MinimumWidth = 20;
            this.Col_Oper.Name = "Col_Oper";
            // 
            // Col_Tpd
            // 
            this.Col_Tpd.HeaderText = "Tpd, sec";
            this.Col_Tpd.MinimumWidth = 20;
            this.Col_Tpd.Name = "Col_Tpd";
            this.Col_Tpd.Width = 65;
            // 
            // Col_Tsh
            // 
            this.Col_Tsh.HeaderText = "Tsh, sec";
            this.Col_Tsh.MinimumWidth = 20;
            this.Col_Tsh.Name = "Col_Tsh";
            this.Col_Tsh.Width = 70;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dGV_Tehnology);
            this.groupBox2.Location = new System.Drawing.Point(3, 196);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(485, 248);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Технология";
            // 
            // dTimeP_Fact
            // 
            this.dTimeP_Fact.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dTimeP_Fact.Location = new System.Drawing.Point(117, 35);
            this.dTimeP_Fact.Name = "dTimeP_Fact";
            this.dTimeP_Fact.Size = new System.Drawing.Size(214, 29);
            this.dTimeP_Fact.TabIndex = 0;
            this.dTimeP_Fact.Value = new System.DateTime(2018, 7, 18, 0, 0, 0, 0);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnkoop);
            this.groupBox3.Controls.Add(this.btn_CloseAllWorks);
            this.groupBox3.Controls.Add(this.cB_InDetail);
            this.groupBox3.Controls.Add(this.mBtnM_Brigade);
            this.groupBox3.Controls.Add(this.mBtnM_Worker);
            this.groupBox3.Controls.Add(this.tB_Workers);
            this.groupBox3.Controls.Add(this.btn_SaveTehnology);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.chB_cooperation);
            this.groupBox3.Controls.Add(this.nUpD_Tpd);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.dTimeP_Fact);
            this.groupBox3.Location = new System.Drawing.Point(494, 196);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(340, 251);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            // 
            // btnkoop
            // 
            this.btnkoop.Location = new System.Drawing.Point(145, 220);
            this.btnkoop.Name = "btnkoop";
            this.btnkoop.Size = new System.Drawing.Size(192, 24);
            this.btnkoop.TabIndex = 1013;
            this.btnkoop.Text = "Закрыть все на кооп";
            this.btnkoop.UseVisualStyleBackColor = true;
            this.btnkoop.Click += new System.EventHandler(this.btnkoop_Click);
            // 
            // btn_CloseAllWorks
            // 
            this.btn_CloseAllWorks.Location = new System.Drawing.Point(7, 143);
            this.btn_CloseAllWorks.Margin = new System.Windows.Forms.Padding(4);
            this.btn_CloseAllWorks.Name = "btn_CloseAllWorks";
            this.btn_CloseAllWorks.Size = new System.Drawing.Size(153, 28);
            this.btn_CloseAllWorks.TabIndex = 1012;
            this.btn_CloseAllWorks.Text = "Закрыть все работы";
            this.btn_CloseAllWorks.UseVisualStyleBackColor = false;
            this.btn_CloseAllWorks.Click += new System.EventHandler(this.btn_CloseAllWorks_Click);
            // 
            // cB_InDetail
            // 
            this.cB_InDetail.AutoSize = true;
            this.cB_InDetail.Location = new System.Drawing.Point(240, 148);
            this.cB_InDetail.Name = "cB_InDetail";
            this.cB_InDetail.Size = new System.Drawing.Size(93, 21);
            this.cB_InDetail.TabIndex = 1010;
            this.cB_InDetail.Text = "Подробно";
            this.cB_InDetail.UseVisualStyleBackColor = true;
            this.cB_InDetail.CheckedChanged += new System.EventHandler(this.cB_InDetail_CheckedChanged);
            // 
            // mBtnM_Brigade
            // 
            this.mBtnM_Brigade.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.mBtnM_Brigade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mBtnM_Brigade.Location = new System.Drawing.Point(92, 85);
            this.mBtnM_Brigade.Margin = new System.Windows.Forms.Padding(4);
            this.mBtnM_Brigade.Name = "mBtnM_Brigade";
            this.mBtnM_Brigade.Size = new System.Drawing.Size(81, 27);
            this.mBtnM_Brigade.TabIndex = 1009;
            this.mBtnM_Brigade.Text = "Бригада";
            this.mBtnM_Brigade.UseVisualStyleBackColor = true;
            this.mBtnM_Brigade.Click += new System.EventHandler(this.mBtnM_Brigade_Click);
            // 
            // mBtnM_Worker
            // 
            this.mBtnM_Worker.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.mBtnM_Worker.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.mBtnM_Worker.Location = new System.Drawing.Point(3, 85);
            this.mBtnM_Worker.Margin = new System.Windows.Forms.Padding(4);
            this.mBtnM_Worker.Name = "mBtnM_Worker";
            this.mBtnM_Worker.Size = new System.Drawing.Size(81, 27);
            this.mBtnM_Worker.TabIndex = 1008;
            this.mBtnM_Worker.Text = "Рабочий";
            this.mBtnM_Worker.UseVisualStyleBackColor = true;
            this.mBtnM_Worker.Click += new System.EventHandler(this.mBtnM_Worker_Click);
            // 
            // tB_Workers
            // 
            this.tB_Workers.BackColor = System.Drawing.SystemColors.Info;
            this.tB_Workers.Location = new System.Drawing.Point(1, 118);
            this.tB_Workers.Margin = new System.Windows.Forms.Padding(4);
            this.tB_Workers.MaxLength = 20;
            this.tB_Workers.Name = "tB_Workers";
            this.tB_Workers.ReadOnly = true;
            this.tB_Workers.Size = new System.Drawing.Size(332, 23);
            this.tB_Workers.TabIndex = 19;
            this.tB_Workers.TabStop = false;
            // 
            // btn_SaveTehnology
            // 
            this.btn_SaveTehnology.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.btn_SaveTehnology.Location = new System.Drawing.Point(7, 174);
            this.btn_SaveTehnology.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SaveTehnology.Name = "btn_SaveTehnology";
            this.btn_SaveTehnology.Size = new System.Drawing.Size(332, 39);
            this.btn_SaveTehnology.TabIndex = 22;
            this.btn_SaveTehnology.Text = "Сохранить";
            this.btn_SaveTehnology.UseVisualStyleBackColor = false;
            this.btn_SaveTehnology.Click += new System.EventHandler(this.btn_SaveTehnology_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "кол-во";
            // 
            // chB_cooperation
            // 
            this.chB_cooperation.AutoSize = true;
            this.chB_cooperation.Location = new System.Drawing.Point(273, 89);
            this.chB_cooperation.Margin = new System.Windows.Forms.Padding(4);
            this.chB_cooperation.Name = "chB_cooperation";
            this.chB_cooperation.Size = new System.Drawing.Size(58, 21);
            this.chB_cooperation.TabIndex = 8;
            this.chB_cooperation.Text = "кооп";
            this.chB_cooperation.UseVisualStyleBackColor = true;
            this.chB_cooperation.CheckedChanged += new System.EventHandler(this.chB_cooperation_CheckedChanged);
            // 
            // nUpD_Tpd
            // 
            this.nUpD_Tpd.BackColor = System.Drawing.SystemColors.Window;
            this.nUpD_Tpd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.nUpD_Tpd.Location = new System.Drawing.Point(3, 37);
            this.nUpD_Tpd.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nUpD_Tpd.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nUpD_Tpd.Name = "nUpD_Tpd";
            this.nUpD_Tpd.Size = new System.Drawing.Size(97, 29);
            this.nUpD_Tpd.TabIndex = 6;
            this.nUpD_Tpd.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(114, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Дата";
            // 
            // btn_SearchSHCM_F
            // 
            this.btn_SearchSHCM_F.Image = global::Dispetcher2.Properties.Resources.Search;
            this.btn_SearchSHCM_F.Location = new System.Drawing.Point(283, 8);
            this.btn_SearchSHCM_F.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SearchSHCM_F.Name = "btn_SearchSHCM_F";
            this.btn_SearchSHCM_F.Size = new System.Drawing.Size(51, 41);
            this.btn_SearchSHCM_F.TabIndex = 1013;
            this.btn_SearchSHCM_F.UseVisualStyleBackColor = false;
            this.btn_SearchSHCM_F.Click += new System.EventHandler(this.btn_SearchSHCM_F_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dGV_FactOperation);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(146, 456);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(837, 173);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Операции по факту (Del - Удалить операцию)";
            // 
            // dGV_FactOperation
            // 
            this.dGV_FactOperation.AllowUserToAddRows = false;
            this.dGV_FactOperation.AllowUserToDeleteRows = false;
            this.dGV_FactOperation.BackgroundColor = System.Drawing.SystemColors.HighlightText;
            this.dGV_FactOperation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_FactOperation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_FactOperation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_FactOper,
            this.Col_DateFactOper,
            this.Col_FK_LoginWorker,
            this.Col_AmountDetails,
            this.Col_FactTpd,
            this.Col_FactTsh});
            this.dGV_FactOperation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_FactOperation.Location = new System.Drawing.Point(3, 19);
            this.dGV_FactOperation.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_FactOperation.MultiSelect = false;
            this.dGV_FactOperation.Name = "dGV_FactOperation";
            this.dGV_FactOperation.ReadOnly = true;
            this.dGV_FactOperation.RowHeadersWidth = 4;
            this.dGV_FactOperation.Size = new System.Drawing.Size(831, 151);
            this.dGV_FactOperation.TabIndex = 15;
            this.dGV_FactOperation.TabStop = false;
            this.dGV_FactOperation.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGV_FactOperation_CellContentClick);
            this.dGV_FactOperation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGV_FactOperation_KeyDown);
            // 
            // Col_FactOper
            // 
            this.Col_FactOper.HeaderText = "Операция";
            this.Col_FactOper.MinimumWidth = 20;
            this.Col_FactOper.Name = "Col_FactOper";
            this.Col_FactOper.ReadOnly = true;
            this.Col_FactOper.Width = 245;
            // 
            // Col_DateFactOper
            // 
            this.Col_DateFactOper.HeaderText = "Дата";
            this.Col_DateFactOper.Name = "Col_DateFactOper";
            this.Col_DateFactOper.ReadOnly = true;
            this.Col_DateFactOper.Width = 90;
            // 
            // Col_FK_LoginWorker
            // 
            this.Col_FK_LoginWorker.HeaderText = "Рабочий";
            this.Col_FK_LoginWorker.Name = "Col_FK_LoginWorker";
            this.Col_FK_LoginWorker.ReadOnly = true;
            this.Col_FK_LoginWorker.Width = 300;
            // 
            // Col_AmountDetails
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Col_AmountDetails.DefaultCellStyle = dataGridViewCellStyle2;
            this.Col_AmountDetails.HeaderText = "Кол-во";
            this.Col_AmountDetails.MinimumWidth = 20;
            this.Col_AmountDetails.Name = "Col_AmountDetails";
            this.Col_AmountDetails.ReadOnly = true;
            this.Col_AmountDetails.Width = 60;
            // 
            // Col_FactTpd
            // 
            this.Col_FactTpd.HeaderText = "Tpd, sec";
            this.Col_FactTpd.Name = "Col_FactTpd";
            this.Col_FactTpd.ReadOnly = true;
            this.Col_FactTpd.Width = 50;
            // 
            // Col_FactTsh
            // 
            this.Col_FactTsh.HeaderText = "Tsh, sec";
            this.Col_FactTsh.Name = "Col_FactTsh";
            this.Col_FactTsh.ReadOnly = true;
            this.Col_FactTsh.Width = 55;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dGV_Details);
            this.groupBox5.Location = new System.Drawing.Point(3, 50);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(831, 145);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Детали сборки (ENTER - взять в работу)";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dGV_Orders);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 3);
            this.panel1.Name = "panel1";
            this.tableLayoutPanel1.SetRowSpan(this.panel1, 2);
            this.panel1.Size = new System.Drawing.Size(139, 626);
            this.panel1.TabIndex = 1;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tB_OrderNum);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(139, 46);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Поиск (№ заказа)";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_SearchSHCM_F);
            this.panel2.Controls.Add(this.groupBox3);
            this.panel2.Controls.Add(this.gB_OrderName);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.groupBox5);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Location = new System.Drawing.Point(146, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(837, 447);
            this.panel2.TabIndex = 1000;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 145F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 843F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 453F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(984, 632);
            this.tableLayoutPanel1.TabIndex = 1001;
            // 
            // F_Fact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(984, 632);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_Fact";
            this.Text = "F_Fact";
            this.Load += new System.EventHandler(this.F_Fact_Load);
            this.Enter += new System.EventHandler(this.F_Fact_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orders)).EndInit();
            this.OrderContextMenuStrip.ResumeLayout(false);
            this.gB_OrderName.ResumeLayout(false);
            this.gB_OrderName.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Details)).EndInit();
            this.shCMContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Tehnology)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUpD_Tpd)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_FactOperation)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dGV_Orders;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_OrderNum;
        private System.Windows.Forms.GroupBox gB_OrderName;
        private System.Windows.Forms.TextBox tB_OrderName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tB_ShcmDetail;
        private System.Windows.Forms.TextBox tB_OrderNum;
        private System.Windows.Forms.DataGridView dGV_Details;
        private System.Windows.Forms.DataGridView dGV_Tehnology;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dTimeP_Fact;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nUpD_Tpd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chB_cooperation;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dGV_FactOperation;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox tB_OrderNumInfo;
        private System.Windows.Forms.Button btn_SaveTehnology;
        private System.Windows.Forms.TextBox tB_Workers;
        private Controls.MyButtonMenu mBtnM_Brigade;
        private Controls.MyButtonMenu mBtnM_Worker;
        private System.Windows.Forms.CheckBox cB_InDetail;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btn_CloseAllWorks;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Position;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_ShcmDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_NameDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Amount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_NameType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FactOper;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_DateFactOper;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FK_LoginWorker;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_AmountDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FactTpd;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FactTsh;
        private System.Windows.Forms.Button btn_SearchSHCM_F;
        private System.Windows.Forms.Button btnkoop;
		private System.Windows.Forms.ContextMenuStrip shCMContextMenuStrip;
		private System.Windows.Forms.ContextMenuStrip OrderContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem OrdersContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem oShCMContextMenuStrip;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Oper;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Tpd;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Tsh;
    }
}