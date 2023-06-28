namespace Dispetcher2
{
    partial class F_TimeSheets
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
            this.chB_Fired = new System.Windows.Forms.CheckBox();
            this.btn_PrintTURV = new System.Windows.Forms.Button();
            this.btn_SaveTimeSheet = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cB_Month = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numUD_year = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.myGrid_TimeSH = new Dispetcher2.Controls.MyGrid.MyGrid();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUD_year)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 98F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.Controls.Add(this.myGrid_TimeSH, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(995, 632);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chB_Fired);
            this.panel1.Controls.Add(this.btn_PrintTURV);
            this.panel1.Controls.Add(this.btn_SaveTimeSheet);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(12, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(969, 60);
            this.panel1.TabIndex = 0;
            // 
            // chB_Fired
            // 
            this.chB_Fired.AutoSize = true;
            this.chB_Fired.Location = new System.Drawing.Point(821, 21);
            this.chB_Fired.Name = "chB_Fired";
            this.chB_Fired.Size = new System.Drawing.Size(101, 20);
            this.chB_Fired.TabIndex = 25;
            this.chB_Fired.Text = "Уволенные";
            this.chB_Fired.UseVisualStyleBackColor = true;
            this.chB_Fired.CheckedChanged += new System.EventHandler(this.chB_Fired_CheckedChanged);
            // 
            // btn_PrintTURV
            // 
            this.btn_PrintTURV.Location = new System.Drawing.Point(660, 12);
            this.btn_PrintTURV.Margin = new System.Windows.Forms.Padding(4);
            this.btn_PrintTURV.Name = "btn_PrintTURV";
            this.btn_PrintTURV.Size = new System.Drawing.Size(116, 41);
            this.btn_PrintTURV.TabIndex = 24;
            this.btn_PrintTURV.Text = "Печать";
            this.btn_PrintTURV.UseVisualStyleBackColor = false;
            this.btn_PrintTURV.Click += new System.EventHandler(this.btn_PrintTURV_Click);
            // 
            // btn_SaveTimeSheet
            // 
            this.btn_SaveTimeSheet.Location = new System.Drawing.Point(541, 12);
            this.btn_SaveTimeSheet.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SaveTimeSheet.Name = "btn_SaveTimeSheet";
            this.btn_SaveTimeSheet.Size = new System.Drawing.Size(111, 41);
            this.btn_SaveTimeSheet.TabIndex = 23;
            this.btn_SaveTimeSheet.Text = "Сохранить";
            this.btn_SaveTimeSheet.UseVisualStyleBackColor = false;
            this.btn_SaveTimeSheet.Click += new System.EventHandler(this.btn_SaveTimeSheet_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cB_Month);
            this.groupBox2.Location = new System.Drawing.Point(272, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(167, 48);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Месяц";
            // 
            // cB_Month
            // 
            this.cB_Month.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cB_Month.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_Month.FormattingEnabled = true;
            this.cB_Month.Items.AddRange(new object[] {
            "Январь",
            "Февраль",
            "Март",
            "Апрель",
            "Май",
            "Июнь",
            "Июль",
            "Август",
            "Сентябрь",
            "Октябрь",
            "Ноябрь",
            "Декабрь"});
            this.cB_Month.Location = new System.Drawing.Point(3, 18);
            this.cB_Month.Name = "cB_Month";
            this.cB_Month.Size = new System.Drawing.Size(161, 24);
            this.cB_Month.TabIndex = 22;
            this.cB_Month.SelectedIndexChanged += new System.EventHandler(this.cB_Month_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numUD_year);
            this.groupBox1.Location = new System.Drawing.Point(441, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(95, 48);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Год";
            // 
            // numUD_year
            // 
            this.numUD_year.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numUD_year.Location = new System.Drawing.Point(3, 18);
            this.numUD_year.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numUD_year.Minimum = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            this.numUD_year.Name = "numUD_year";
            this.numUD_year.Size = new System.Drawing.Size(89, 22);
            this.numUD_year.TabIndex = 22;
            this.numUD_year.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numUD_year.ValueChanged += new System.EventHandler(this.numUD_year_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(265, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "ТАБЕЛЬ УЧЕТА РАБОЧЕГО ВРЕМЕНИ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Подразделение: пр-во 50";
            // 
            // myGrid_TimeSH
            // 
            this.myGrid_TimeSH.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.myGrid_TimeSH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myGrid_TimeSH.EnableSort = true;
            this.myGrid_TimeSH.Location = new System.Drawing.Point(12, 74);
            this.myGrid_TimeSH.Name = "myGrid_TimeSH";
            this.myGrid_TimeSH.OptimizeMode = SourceGrid.CellOptimizeMode.ForRows;
            this.myGrid_TimeSH.SelectionMode = SourceGrid.GridSelectionMode.Cell;
            this.myGrid_TimeSH.Size = new System.Drawing.Size(969, 548);
            this.myGrid_TimeSH.TabIndex = 22;
            this.myGrid_TimeSH.TabStop = true;
            this.myGrid_TimeSH.ToolTipText = "";
            // 
            // F_TimeSheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 632);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_TimeSheets";
            this.Text = "F_TimeSheets";
            this.Load += new System.EventHandler(this.F_TimeSheets_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numUD_year)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numUD_year;
        private System.Windows.Forms.ComboBox cB_Month;
        private Controls.MyGrid.MyGrid myGrid_TimeSH;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_SaveTimeSheet;
        private System.Windows.Forms.Button btn_PrintTURV;
        private System.Windows.Forms.CheckBox chB_Fired;
    }
}