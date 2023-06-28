namespace Dispetcher2
{
    partial class F_Workers
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
            this.tLP_Users = new System.Windows.Forms.TableLayoutPanel();
            this.gB_Search = new System.Windows.Forms.GroupBox();
            this.tB_Search = new System.Windows.Forms.TextBox();
            this.gBox_FilterIsValid = new System.Windows.Forms.GroupBox();
            this.rBtn_IsValidFalse = new System.Windows.Forms.RadioButton();
            this.rBtn_IsValidTrue = new System.Windows.Forms.RadioButton();
            this.rBtn_IsValidall = new System.Windows.Forms.RadioButton();
            this.dGV_Workers = new System.Windows.Forms.DataGridView();
            this.Col_FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Login = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_FK_IdDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Create_Update = new System.Windows.Forms.Button();
            this.gB_UsersData = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chB_TimeSheets = new System.Windows.Forms.CheckBox();
            this.tB_TabNum = new System.Windows.Forms.TextBox();
            this.cB_SpJob = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rBtn_ITR = new System.Windows.Forms.RadioButton();
            this.rBtn_non_ITR = new System.Windows.Forms.RadioButton();
            this.mTB_DateStart = new System.Windows.Forms.MaskedTextBox();
            this.lbl_DateStart = new System.Windows.Forms.Label();
            this.chB_RateWorker = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cB_Department = new System.Windows.Forms.ComboBox();
            this.lbl_SecondName = new System.Windows.Forms.Label();
            this.tB_SecondName = new System.Windows.Forms.TextBox();
            this.chB_IsValid = new System.Windows.Forms.CheckBox();
            this.tB_Name = new System.Windows.Forms.TextBox();
            this.tB_LastName = new System.Windows.Forms.TextBox();
            this.lbl_Name = new System.Windows.Forms.Label();
            this.lbl_LastName = new System.Windows.Forms.Label();
            this.lbl_Login = new System.Windows.Forms.Label();
            this.chB_NewWorker = new System.Windows.Forms.CheckBox();
            this.tB_Login = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.mTB_DateEnd = new System.Windows.Forms.MaskedTextBox();
            this.tLP_Users.SuspendLayout();
            this.gB_Search.SuspendLayout();
            this.gBox_FilterIsValid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Workers)).BeginInit();
            this.gB_UsersData.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tLP_Users
            // 
            this.tLP_Users.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tLP_Users.ColumnCount = 4;
            this.tLP_Users.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_Users.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.tLP_Users.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 503F));
            this.tLP_Users.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_Users.Controls.Add(this.gB_Search, 1, 2);
            this.tLP_Users.Controls.Add(this.gBox_FilterIsValid, 1, 1);
            this.tLP_Users.Controls.Add(this.dGV_Workers, 1, 3);
            this.tLP_Users.Controls.Add(this.btn_Create_Update, 2, 4);
            this.tLP_Users.Controls.Add(this.gB_UsersData, 2, 1);
            this.tLP_Users.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLP_Users.Location = new System.Drawing.Point(0, 0);
            this.tLP_Users.Margin = new System.Windows.Forms.Padding(4);
            this.tLP_Users.Name = "tLP_Users";
            this.tLP_Users.RowCount = 6;
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tLP_Users.Size = new System.Drawing.Size(995, 632);
            this.tLP_Users.TabIndex = 2;
            // 
            // gB_Search
            // 
            this.gB_Search.Controls.Add(this.tB_Search);
            this.gB_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gB_Search.Location = new System.Drawing.Point(25, 58);
            this.gB_Search.Margin = new System.Windows.Forms.Padding(4);
            this.gB_Search.Name = "gB_Search";
            this.gB_Search.Padding = new System.Windows.Forms.Padding(4);
            this.gB_Search.Size = new System.Drawing.Size(442, 47);
            this.gB_Search.TabIndex = 14;
            this.gB_Search.TabStop = false;
            this.gB_Search.Text = "Поиск по ФИО (по всем учётным записям)";
            // 
            // tB_Search
            // 
            this.tB_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tB_Search.Location = new System.Drawing.Point(4, 20);
            this.tB_Search.Margin = new System.Windows.Forms.Padding(4);
            this.tB_Search.Name = "tB_Search";
            this.tB_Search.Size = new System.Drawing.Size(434, 23);
            this.tB_Search.TabIndex = 0;
            this.tB_Search.TabStop = false;
            this.tB_Search.TextChanged += new System.EventHandler(this.tB_Search_TextChanged);
            // 
            // gBox_FilterIsValid
            // 
            this.gBox_FilterIsValid.Controls.Add(this.rBtn_IsValidFalse);
            this.gBox_FilterIsValid.Controls.Add(this.rBtn_IsValidTrue);
            this.gBox_FilterIsValid.Controls.Add(this.rBtn_IsValidall);
            this.gBox_FilterIsValid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBox_FilterIsValid.Location = new System.Drawing.Point(24, 7);
            this.gBox_FilterIsValid.Name = "gBox_FilterIsValid";
            this.gBox_FilterIsValid.Size = new System.Drawing.Size(444, 44);
            this.gBox_FilterIsValid.TabIndex = 21;
            this.gBox_FilterIsValid.TabStop = false;
            // 
            // rBtn_IsValidFalse
            // 
            this.rBtn_IsValidFalse.AutoSize = true;
            this.rBtn_IsValidFalse.Location = new System.Drawing.Point(163, 16);
            this.rBtn_IsValidFalse.Margin = new System.Windows.Forms.Padding(4);
            this.rBtn_IsValidFalse.Name = "rBtn_IsValidFalse";
            this.rBtn_IsValidFalse.Size = new System.Drawing.Size(147, 21);
            this.rBtn_IsValidFalse.TabIndex = 13;
            this.rBtn_IsValidFalse.Text = "Заблокированные";
            this.rBtn_IsValidFalse.UseVisualStyleBackColor = true;
            this.rBtn_IsValidFalse.CheckedChanged += new System.EventHandler(this.rBtn_IsValidFalse_CheckedChanged);
            // 
            // rBtn_IsValidTrue
            // 
            this.rBtn_IsValidTrue.AutoSize = true;
            this.rBtn_IsValidTrue.Location = new System.Drawing.Point(65, 16);
            this.rBtn_IsValidTrue.Margin = new System.Windows.Forms.Padding(4);
            this.rBtn_IsValidTrue.Name = "rBtn_IsValidTrue";
            this.rBtn_IsValidTrue.Size = new System.Drawing.Size(90, 21);
            this.rBtn_IsValidTrue.TabIndex = 12;
            this.rBtn_IsValidTrue.Text = "Активные";
            this.rBtn_IsValidTrue.UseVisualStyleBackColor = true;
            this.rBtn_IsValidTrue.CheckedChanged += new System.EventHandler(this.rBtn_IsValidTrue_CheckedChanged);
            // 
            // rBtn_IsValidall
            // 
            this.rBtn_IsValidall.AutoSize = true;
            this.rBtn_IsValidall.Checked = true;
            this.rBtn_IsValidall.Location = new System.Drawing.Point(7, 16);
            this.rBtn_IsValidall.Margin = new System.Windows.Forms.Padding(4);
            this.rBtn_IsValidall.Name = "rBtn_IsValidall";
            this.rBtn_IsValidall.Size = new System.Drawing.Size(50, 21);
            this.rBtn_IsValidall.TabIndex = 11;
            this.rBtn_IsValidall.TabStop = true;
            this.rBtn_IsValidall.Text = "Все";
            this.rBtn_IsValidall.UseVisualStyleBackColor = true;
            this.rBtn_IsValidall.CheckedChanged += new System.EventHandler(this.rBtn_IsValidall_CheckedChanged);
            // 
            // dGV_Workers
            // 
            this.dGV_Workers.AllowUserToAddRows = false;
            this.dGV_Workers.AllowUserToDeleteRows = false;
            this.dGV_Workers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Workers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Workers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Workers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_FullName,
            this.Col_Login,
            this.Col_FK_IdDepartment});
            this.dGV_Workers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Workers.Location = new System.Drawing.Point(25, 113);
            this.dGV_Workers.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Workers.MultiSelect = false;
            this.dGV_Workers.Name = "dGV_Workers";
            this.dGV_Workers.ReadOnly = true;
            this.dGV_Workers.RowHeadersWidth = 4;
            this.tLP_Users.SetRowSpan(this.dGV_Workers, 2);
            this.dGV_Workers.Size = new System.Drawing.Size(442, 509);
            this.dGV_Workers.TabIndex = 10;
            this.dGV_Workers.TabStop = false;
            this.dGV_Workers.CurrentCellChanged += new System.EventHandler(this.dGV_Workers_CurrentCellChanged);
            // 
            // Col_FullName
            // 
            this.Col_FullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Col_FullName.HeaderText = "Фамилия Имя Отчество";
            this.Col_FullName.MinimumWidth = 100;
            this.Col_FullName.Name = "Col_FullName";
            this.Col_FullName.ReadOnly = true;
            // 
            // Col_Login
            // 
            this.Col_Login.HeaderText = "Логин";
            this.Col_Login.MinimumWidth = 80;
            this.Col_Login.Name = "Col_Login";
            this.Col_Login.ReadOnly = true;
            this.Col_Login.Width = 140;
            // 
            // Col_FK_IdDepartment
            // 
            this.Col_FK_IdDepartment.HeaderText = "ИД отдела";
            this.Col_FK_IdDepartment.Name = "Col_FK_IdDepartment";
            this.Col_FK_IdDepartment.ReadOnly = true;
            this.Col_FK_IdDepartment.Visible = false;
            // 
            // btn_Create_Update
            // 
            this.btn_Create_Update.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btn_Create_Update.Location = new System.Drawing.Point(475, 581);
            this.btn_Create_Update.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Create_Update.Name = "btn_Create_Update";
            this.btn_Create_Update.Size = new System.Drawing.Size(495, 41);
            this.btn_Create_Update.TabIndex = 17;
            this.btn_Create_Update.Text = "Создать/Изменить";
            this.btn_Create_Update.UseVisualStyleBackColor = false;
            this.btn_Create_Update.Click += new System.EventHandler(this.btn_Create_Update_Click);
            // 
            // gB_UsersData
            // 
            this.gB_UsersData.Controls.Add(this.chB_TimeSheets);
            this.gB_UsersData.Controls.Add(this.mTB_DateEnd);
            this.gB_UsersData.Controls.Add(this.label4);
            this.gB_UsersData.Controls.Add(this.groupBox1);
            this.gB_UsersData.Controls.Add(this.mTB_DateStart);
            this.gB_UsersData.Controls.Add(this.lbl_DateStart);
            this.gB_UsersData.Controls.Add(this.chB_RateWorker);
            this.gB_UsersData.Controls.Add(this.label1);
            this.gB_UsersData.Controls.Add(this.cB_Department);
            this.gB_UsersData.Controls.Add(this.lbl_SecondName);
            this.gB_UsersData.Controls.Add(this.tB_SecondName);
            this.gB_UsersData.Controls.Add(this.chB_IsValid);
            this.gB_UsersData.Controls.Add(this.tB_Name);
            this.gB_UsersData.Controls.Add(this.tB_LastName);
            this.gB_UsersData.Controls.Add(this.lbl_Name);
            this.gB_UsersData.Controls.Add(this.lbl_LastName);
            this.gB_UsersData.Controls.Add(this.lbl_Login);
            this.gB_UsersData.Controls.Add(this.chB_NewWorker);
            this.gB_UsersData.Controls.Add(this.tB_Login);
            this.gB_UsersData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gB_UsersData.Location = new System.Drawing.Point(475, 8);
            this.gB_UsersData.Margin = new System.Windows.Forms.Padding(4);
            this.gB_UsersData.Name = "gB_UsersData";
            this.gB_UsersData.Padding = new System.Windows.Forms.Padding(4);
            this.tLP_Users.SetRowSpan(this.gB_UsersData, 3);
            this.gB_UsersData.Size = new System.Drawing.Size(495, 565);
            this.gB_UsersData.TabIndex = 15;
            this.gB_UsersData.TabStop = false;
            this.gB_UsersData.Text = "Данные пользователя";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tB_TabNum);
            this.groupBox1.Controls.Add(this.cB_SpJob);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.rBtn_ITR);
            this.groupBox1.Controls.Add(this.rBtn_non_ITR);
            this.groupBox1.Location = new System.Drawing.Point(4, 218);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(488, 340);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Для табеля учёта рабочего времени";
            // 
            // chB_TimeSheets
            // 
            this.chB_TimeSheets.AutoSize = true;
            this.chB_TimeSheets.Location = new System.Drawing.Point(374, 114);
            this.chB_TimeSheets.Margin = new System.Windows.Forms.Padding(4);
            this.chB_TimeSheets.Name = "chB_TimeSheets";
            this.chB_TimeSheets.Size = new System.Drawing.Size(75, 21);
            this.chB_TimeSheets.TabIndex = 28;
            this.chB_TimeSheets.TabStop = false;
            this.chB_TimeSheets.Text = "Табель";
            this.chB_TimeSheets.UseVisualStyleBackColor = true;
            // 
            // tB_TabNum
            // 
            this.tB_TabNum.Location = new System.Drawing.Point(136, 82);
            this.tB_TabNum.Margin = new System.Windows.Forms.Padding(4);
            this.tB_TabNum.MaxLength = 15;
            this.tB_TabNum.Name = "tB_TabNum";
            this.tB_TabNum.Size = new System.Drawing.Size(226, 23);
            this.tB_TabNum.TabIndex = 25;
            this.tB_TabNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tB_TabNum_KeyPress);
            // 
            // cB_SpJob
            // 
            this.cB_SpJob.Location = new System.Drawing.Point(99, 122);
            this.cB_SpJob.Name = "cB_SpJob";
            this.cB_SpJob.Size = new System.Drawing.Size(382, 24);
            this.cB_SpJob.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 125);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 17);
            this.label3.TabIndex = 28;
            this.label3.Text = "Должность:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 85);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 17);
            this.label2.TabIndex = 27;
            this.label2.Text = "Табельный номер:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // rBtn_ITR
            // 
            this.rBtn_ITR.AutoSize = true;
            this.rBtn_ITR.Location = new System.Drawing.Point(6, 22);
            this.rBtn_ITR.Name = "rBtn_ITR";
            this.rBtn_ITR.Size = new System.Drawing.Size(413, 21);
            this.rBtn_ITR.TabIndex = 23;
            this.rBtn_ITR.TabStop = true;
            this.rBtn_ITR.Text = "ИТР, специалисты и служащий персонал производства 50";
            this.rBtn_ITR.UseVisualStyleBackColor = true;
            // 
            // rBtn_non_ITR
            // 
            this.rBtn_non_ITR.AutoSize = true;
            this.rBtn_non_ITR.Location = new System.Drawing.Point(6, 49);
            this.rBtn_non_ITR.Name = "rBtn_non_ITR";
            this.rBtn_non_ITR.Size = new System.Drawing.Size(200, 21);
            this.rBtn_non_ITR.TabIndex = 24;
            this.rBtn_non_ITR.TabStop = true;
            this.rBtn_non_ITR.Text = "Цех по изготовлению СТО";
            this.rBtn_non_ITR.UseVisualStyleBackColor = true;
            // 
            // mTB_DateStart
            // 
            this.mTB_DateStart.Location = new System.Drawing.Point(124, 186);
            this.mTB_DateStart.Mask = "00/00/0000";
            this.mTB_DateStart.Name = "mTB_DateStart";
            this.mTB_DateStart.Size = new System.Drawing.Size(95, 23);
            this.mTB_DateStart.TabIndex = 21;
            this.mTB_DateStart.ValidatingType = typeof(System.DateTime);
            // 
            // lbl_DateStart
            // 
            this.lbl_DateStart.AutoSize = true;
            this.lbl_DateStart.Location = new System.Drawing.Point(34, 189);
            this.lbl_DateStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_DateStart.Name = "lbl_DateStart";
            this.lbl_DateStart.Size = new System.Drawing.Size(86, 17);
            this.lbl_DateStart.TabIndex = 22;
            this.lbl_DateStart.Text = "Работает с:";
            this.lbl_DateStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chB_RateWorker
            // 
            this.chB_RateWorker.AutoSize = true;
            this.chB_RateWorker.Location = new System.Drawing.Point(374, 86);
            this.chB_RateWorker.Margin = new System.Windows.Forms.Padding(4);
            this.chB_RateWorker.Name = "chB_RateWorker";
            this.chB_RateWorker.Size = new System.Drawing.Size(95, 21);
            this.chB_RateWorker.TabIndex = 22;
            this.chB_RateWorker.TabStop = false;
            this.chB_RateWorker.Text = "0,5 ставки";
            this.chB_RateWorker.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 148);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "Отдел:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cB_Department
            // 
            this.cB_Department.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cB_Department.FormattingEnabled = true;
            this.cB_Department.Location = new System.Drawing.Point(125, 145);
            this.cB_Department.Name = "cB_Department";
            this.cB_Department.Size = new System.Drawing.Size(360, 24);
            this.cB_Department.TabIndex = 5;
            // 
            // lbl_SecondName
            // 
            this.lbl_SecondName.AutoSize = true;
            this.lbl_SecondName.Location = new System.Drawing.Point(45, 118);
            this.lbl_SecondName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_SecondName.Name = "lbl_SecondName";
            this.lbl_SecondName.Size = new System.Drawing.Size(75, 17);
            this.lbl_SecondName.TabIndex = 11;
            this.lbl_SecondName.Text = "Отчество:";
            this.lbl_SecondName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tB_SecondName
            // 
            this.tB_SecondName.Location = new System.Drawing.Point(124, 115);
            this.tB_SecondName.Margin = new System.Windows.Forms.Padding(4);
            this.tB_SecondName.MaxLength = 40;
            this.tB_SecondName.Name = "tB_SecondName";
            this.tB_SecondName.Size = new System.Drawing.Size(242, 23);
            this.tB_SecondName.TabIndex = 4;
            // 
            // chB_IsValid
            // 
            this.chB_IsValid.AutoSize = true;
            this.chB_IsValid.Location = new System.Drawing.Point(374, 55);
            this.chB_IsValid.Margin = new System.Windows.Forms.Padding(4);
            this.chB_IsValid.Name = "chB_IsValid";
            this.chB_IsValid.Size = new System.Drawing.Size(102, 21);
            this.chB_IsValid.TabIndex = 9;
            this.chB_IsValid.TabStop = false;
            this.chB_IsValid.Text = "Разрешена";
            this.chB_IsValid.UseVisualStyleBackColor = true;
            // 
            // tB_Name
            // 
            this.tB_Name.Location = new System.Drawing.Point(124, 84);
            this.tB_Name.Margin = new System.Windows.Forms.Padding(4);
            this.tB_Name.MaxLength = 40;
            this.tB_Name.Name = "tB_Name";
            this.tB_Name.Size = new System.Drawing.Size(242, 23);
            this.tB_Name.TabIndex = 3;
            // 
            // tB_LastName
            // 
            this.tB_LastName.Location = new System.Drawing.Point(125, 53);
            this.tB_LastName.Margin = new System.Windows.Forms.Padding(4);
            this.tB_LastName.MaxLength = 40;
            this.tB_LastName.Name = "tB_LastName";
            this.tB_LastName.Size = new System.Drawing.Size(241, 23);
            this.tB_LastName.TabIndex = 2;
            // 
            // lbl_Name
            // 
            this.lbl_Name.AutoSize = true;
            this.lbl_Name.Location = new System.Drawing.Point(81, 87);
            this.lbl_Name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Name.Name = "lbl_Name";
            this.lbl_Name.Size = new System.Drawing.Size(39, 17);
            this.lbl_Name.TabIndex = 4;
            this.lbl_Name.Text = "Имя:";
            this.lbl_Name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_LastName
            // 
            this.lbl_LastName.AutoSize = true;
            this.lbl_LastName.Location = new System.Drawing.Point(47, 56);
            this.lbl_LastName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_LastName.Name = "lbl_LastName";
            this.lbl_LastName.Size = new System.Drawing.Size(74, 17);
            this.lbl_LastName.TabIndex = 3;
            this.lbl_LastName.Text = "Фамилия:";
            this.lbl_LastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_Login
            // 
            this.lbl_Login.AutoSize = true;
            this.lbl_Login.Location = new System.Drawing.Point(71, 27);
            this.lbl_Login.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Login.Name = "lbl_Login";
            this.lbl_Login.Size = new System.Drawing.Size(51, 17);
            this.lbl_Login.TabIndex = 0;
            this.lbl_Login.Text = "Логин:";
            this.lbl_Login.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chB_NewWorker
            // 
            this.chB_NewWorker.AutoSize = true;
            this.chB_NewWorker.Location = new System.Drawing.Point(374, 24);
            this.chB_NewWorker.Margin = new System.Windows.Forms.Padding(4);
            this.chB_NewWorker.Name = "chB_NewWorker";
            this.chB_NewWorker.Size = new System.Drawing.Size(70, 21);
            this.chB_NewWorker.TabIndex = 1;
            this.chB_NewWorker.TabStop = false;
            this.chB_NewWorker.Text = "Новый";
            this.chB_NewWorker.UseVisualStyleBackColor = true;
            this.chB_NewWorker.CheckedChanged += new System.EventHandler(this.chB_NewWorker_CheckedChanged);
            // 
            // tB_Login
            // 
            this.tB_Login.BackColor = System.Drawing.SystemColors.Info;
            this.tB_Login.Location = new System.Drawing.Point(125, 24);
            this.tB_Login.Margin = new System.Windows.Forms.Padding(4);
            this.tB_Login.MaxLength = 20;
            this.tB_Login.Name = "tB_Login";
            this.tB_Login.ReadOnly = true;
            this.tB_Login.Size = new System.Drawing.Size(241, 23);
            this.tB_Login.TabIndex = 1;
            this.tB_Login.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(270, 189);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 17);
            this.label4.TabIndex = 28;
            this.label4.Text = "Уволен:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mTB_DateEnd
            // 
            this.mTB_DateEnd.Location = new System.Drawing.Point(337, 186);
            this.mTB_DateEnd.Mask = "00/00/0000";
            this.mTB_DateEnd.Name = "mTB_DateEnd";
            this.mTB_DateEnd.Size = new System.Drawing.Size(95, 23);
            this.mTB_DateEnd.TabIndex = 29;
            this.mTB_DateEnd.ValidatingType = typeof(System.DateTime);
            // 
            // F_Workers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 632);
            this.Controls.Add(this.tLP_Users);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_Workers";
            this.Text = "F_Workers";
            this.Load += new System.EventHandler(this.F_Workers_Load);
            this.tLP_Users.ResumeLayout(false);
            this.gB_Search.ResumeLayout(false);
            this.gB_Search.PerformLayout();
            this.gBox_FilterIsValid.ResumeLayout(false);
            this.gBox_FilterIsValid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Workers)).EndInit();
            this.gB_UsersData.ResumeLayout(false);
            this.gB_UsersData.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tLP_Users;
        private System.Windows.Forms.GroupBox gB_Search;
        private System.Windows.Forms.TextBox tB_Search;
        private System.Windows.Forms.GroupBox gB_UsersData;
        private System.Windows.Forms.MaskedTextBox mTB_DateStart;
        private System.Windows.Forms.Label lbl_DateStart;
        private System.Windows.Forms.CheckBox chB_RateWorker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cB_Department;
        private System.Windows.Forms.Label lbl_SecondName;
        private System.Windows.Forms.TextBox tB_SecondName;
        private System.Windows.Forms.CheckBox chB_IsValid;
        private System.Windows.Forms.TextBox tB_Name;
        private System.Windows.Forms.TextBox tB_LastName;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.Label lbl_LastName;
        private System.Windows.Forms.Label lbl_Login;
        private System.Windows.Forms.CheckBox chB_NewWorker;
        private System.Windows.Forms.TextBox tB_Login;
        private System.Windows.Forms.Button btn_Create_Update;
        private System.Windows.Forms.GroupBox gBox_FilterIsValid;
        private System.Windows.Forms.RadioButton rBtn_IsValidFalse;
        private System.Windows.Forms.RadioButton rBtn_IsValidTrue;
        private System.Windows.Forms.RadioButton rBtn_IsValidall;
        private System.Windows.Forms.DataGridView dGV_Workers;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Login;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FK_IdDepartment;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rBtn_ITR;
        private System.Windows.Forms.RadioButton rBtn_non_ITR;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cB_SpJob;
        private System.Windows.Forms.TextBox tB_TabNum;
        private System.Windows.Forms.CheckBox chB_TimeSheets;
        private System.Windows.Forms.MaskedTextBox mTB_DateEnd;
        private System.Windows.Forms.Label label4;
    }
}