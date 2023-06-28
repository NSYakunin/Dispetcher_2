namespace Dispetcher2
{
    partial class F_ReportsPlan
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
            this.myTabC_ReportsPlan = new Dispetcher2.MyTabControl();
            this.tPageForm106 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dGV_Orders = new System.Windows.Forms.DataGridView();
            this.Col_OrderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tB_OrderNum = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_PlanForm6Create = new System.Windows.Forms.Button();
            this.gB_OrderName = new System.Windows.Forms.GroupBox();
            this.tB_OrderNumInfo = new System.Windows.Forms.TextBox();
            this.tB_OrderName = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.myTabC_ReportsPlan.SuspendLayout();
            this.tPageForm106.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orders)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.panel2.SuspendLayout();
            this.gB_OrderName.SuspendLayout();
            this.SuspendLayout();
            // 
            // myTabC_ReportsPlan
            // 
            this.myTabC_ReportsPlan.Controls.Add(this.tPageForm106);
            this.myTabC_ReportsPlan.Controls.Add(this.tabPage2);
            this.myTabC_ReportsPlan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myTabC_ReportsPlan.Location = new System.Drawing.Point(0, 0);
            this.myTabC_ReportsPlan.Name = "myTabC_ReportsPlan";
            this.myTabC_ReportsPlan.SelectedIndex = 0;
            this.myTabC_ReportsPlan.Size = new System.Drawing.Size(995, 632);
            this.myTabC_ReportsPlan.TabIndex = 0;
            // 
            // tPageForm106
            // 
            this.tPageForm106.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tPageForm106.Controls.Add(this.tableLayoutPanel1);
            this.tPageForm106.Location = new System.Drawing.Point(4, 25);
            this.tPageForm106.Name = "tPageForm106";
            this.tPageForm106.Padding = new System.Windows.Forms.Padding(3);
            this.tPageForm106.Size = new System.Drawing.Size(987, 603);
            this.tPageForm106.TabIndex = 0;
            this.tPageForm106.Text = "tPageForm106";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 146F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 469F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 403F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(981, 597);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dGV_Orders);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(186, 100);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 397);
            this.panel1.TabIndex = 2;
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
            this.dGV_Orders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Orders.Location = new System.Drawing.Point(0, 46);
            this.dGV_Orders.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Orders.MultiSelect = false;
            this.dGV_Orders.Name = "dGV_Orders";
            this.dGV_Orders.ReadOnly = true;
            this.dGV_Orders.RowHeadersWidth = 4;
            this.dGV_Orders.Size = new System.Drawing.Size(140, 351);
            this.dGV_Orders.TabIndex = 11;
            this.dGV_Orders.TabStop = false;
            // 
            // Col_OrderNum
            // 
            this.Col_OrderNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_OrderNum.HeaderText = "№ заказа";
            this.Col_OrderNum.MinimumWidth = 100;
            this.Col_OrderNum.Name = "Col_OrderNum";
            this.Col_OrderNum.ReadOnly = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tB_OrderNum);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(140, 46);
            this.groupBox6.TabIndex = 1;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Поиск (№ заказа)";
            // 
            // tB_OrderNum
            // 
            this.tB_OrderNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_OrderNum.Location = new System.Drawing.Point(3, 19);
            this.tB_OrderNum.MaxLength = 50;
            this.tB_OrderNum.Name = "tB_OrderNum";
            this.tB_OrderNum.Size = new System.Drawing.Size(134, 23);
            this.tB_OrderNum.TabIndex = 1;
            this.tB_OrderNum.TextChanged += new System.EventHandler(this.tB_OrderNum_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.btn_PlanForm6Create);
            this.panel2.Controls.Add(this.gB_OrderName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(332, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(463, 397);
            this.panel2.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(111, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(242, 20);
            this.label5.TabIndex = 1003;
            this.label5.Text = "\"План-график (форма №6)\"";
            // 
            // btn_PlanForm6Create
            // 
            this.btn_PlanForm6Create.Location = new System.Drawing.Point(2, 224);
            this.btn_PlanForm6Create.Margin = new System.Windows.Forms.Padding(4);
            this.btn_PlanForm6Create.Name = "btn_PlanForm6Create";
            this.btn_PlanForm6Create.Size = new System.Drawing.Size(455, 41);
            this.btn_PlanForm6Create.TabIndex = 1001;
            this.btn_PlanForm6Create.Text = "Сформировать";
            this.btn_PlanForm6Create.UseVisualStyleBackColor = false;
            this.btn_PlanForm6Create.Click += new System.EventHandler(this.btn_PlanForm6Create_Click);
            // 
            // gB_OrderName
            // 
            this.gB_OrderName.Controls.Add(this.tB_OrderNumInfo);
            this.gB_OrderName.Controls.Add(this.tB_OrderName);
            this.gB_OrderName.Location = new System.Drawing.Point(5, 120);
            this.gB_OrderName.Name = "gB_OrderName";
            this.gB_OrderName.Size = new System.Drawing.Size(455, 84);
            this.gB_OrderName.TabIndex = 1000;
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
            this.tB_OrderNumInfo.Size = new System.Drawing.Size(156, 23);
            this.tB_OrderNumInfo.TabIndex = 3;
            this.tB_OrderNumInfo.TabStop = false;
            // 
            // tB_OrderName
            // 
            this.tB_OrderName.BackColor = System.Drawing.SystemColors.Info;
            this.tB_OrderName.Location = new System.Drawing.Point(0, 50);
            this.tB_OrderName.Margin = new System.Windows.Forms.Padding(4);
            this.tB_OrderName.MaxLength = 20;
            this.tB_OrderName.Name = "tB_OrderName";
            this.tB_OrderName.ReadOnly = true;
            this.tB_OrderName.Size = new System.Drawing.Size(448, 23);
            this.tB_OrderName.TabIndex = 2;
            this.tB_OrderName.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(987, 603);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // F_ReportsPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(995, 632);
            this.Controls.Add(this.myTabC_ReportsPlan);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_ReportsPlan";
            this.Text = "F_ReportsPlan";
            this.Load += new System.EventHandler(this.F_ReportsPlan_Load);
            this.myTabC_ReportsPlan.ResumeLayout(false);
            this.tPageForm106.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Orders)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.gB_OrderName.ResumeLayout(false);
            this.gB_OrderName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MyTabControl myTabC_ReportsPlan;
        private System.Windows.Forms.TabPage tPageForm106;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dGV_Orders;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_OrderNum;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox tB_OrderNum;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox gB_OrderName;
        private System.Windows.Forms.TextBox tB_OrderNumInfo;
        private System.Windows.Forms.TextBox tB_OrderName;
        private System.Windows.Forms.Button btn_PlanForm6Create;
        private System.Windows.Forms.Label label5;
    }
}