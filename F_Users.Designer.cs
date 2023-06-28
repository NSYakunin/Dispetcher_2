namespace Dispetcher2
{
    partial class F_Users
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tLP_Users = new System.Windows.Forms.TableLayoutPanel();
            this.gBoxUserAccess = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel_UserAccess = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chB_Orders_Set = new System.Windows.Forms.CheckBox();
            this.chB_Orders_Set_View = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chB_Fact_Set_View = new System.Windows.Forms.CheckBox();
            this.chB_Fact_Set = new System.Windows.Forms.CheckBox();
            this.chB_Kit_Set = new System.Windows.Forms.CheckBox();
            this.chB_Technology_Set = new System.Windows.Forms.CheckBox();
            this.chB_Planning_Set = new System.Windows.Forms.CheckBox();
            this.chB_Reports_Set = new System.Windows.Forms.CheckBox();
            this.chB_Users_Set = new System.Windows.Forms.CheckBox();
            this.chB_Settings_Set = new System.Windows.Forms.CheckBox();
            this.gB_Search = new System.Windows.Forms.GroupBox();
            this.tB_Search = new System.Windows.Forms.TextBox();
            this.gB_UsersData = new System.Windows.Forms.GroupBox();
            this.mTB_DateStart = new System.Windows.Forms.MaskedTextBox();
            this.lbl_DateStart = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cB_Department = new System.Windows.Forms.ComboBox();
            this.lbl_SecondName = new System.Windows.Forms.Label();
            this.tB_SecondName = new System.Windows.Forms.TextBox();
            this.chB_IsValid = new System.Windows.Forms.CheckBox();
            this.tB_Password = new System.Windows.Forms.TextBox();
            this.tB_Name = new System.Windows.Forms.TextBox();
            this.tB_LastName = new System.Windows.Forms.TextBox();
            this.lbl_Name = new System.Windows.Forms.Label();
            this.lbl_Password = new System.Windows.Forms.Label();
            this.lbl_LastName = new System.Windows.Forms.Label();
            this.lbl_Login = new System.Windows.Forms.Label();
            this.chB_NewUser = new System.Windows.Forms.CheckBox();
            this.tB_Login = new System.Windows.Forms.TextBox();
            this.dGV_Users = new System.Windows.Forms.DataGridView();
            this.Col_FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_Login = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col_FK_IdDepartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Create_Update = new System.Windows.Forms.Button();
            this.gBox_FilterIsValid = new System.Windows.Forms.GroupBox();
            this.rBtn_IsValidFalse = new System.Windows.Forms.RadioButton();
            this.rBtn_IsValidTrue = new System.Windows.Forms.RadioButton();
            this.rBtn_IsValidall = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.tLP_Users.SuspendLayout();
            this.gBoxUserAccess.SuspendLayout();
            this.flowLayoutPanel_UserAccess.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gB_Search.SuspendLayout();
            this.gB_UsersData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Users)).BeginInit();
            this.gBox_FilterIsValid.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.tLP_Users);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(995, 632);
            this.panel1.TabIndex = 1;
            // 
            // tLP_Users
            // 
            this.tLP_Users.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.tLP_Users.ColumnCount = 4;
            this.tLP_Users.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_Users.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 450F));
            this.tLP_Users.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 503F));
            this.tLP_Users.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tLP_Users.Controls.Add(this.gBoxUserAccess, 2, 4);
            this.tLP_Users.Controls.Add(this.gB_Search, 1, 2);
            this.tLP_Users.Controls.Add(this.gB_UsersData, 2, 1);
            this.tLP_Users.Controls.Add(this.dGV_Users, 1, 3);
            this.tLP_Users.Controls.Add(this.btn_Create_Update, 2, 5);
            this.tLP_Users.Controls.Add(this.gBox_FilterIsValid, 1, 1);
            this.tLP_Users.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tLP_Users.Location = new System.Drawing.Point(0, 0);
            this.tLP_Users.Margin = new System.Windows.Forms.Padding(4);
            this.tLP_Users.Name = "tLP_Users";
            this.tLP_Users.RowCount = 7;
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 149F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 98F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tLP_Users.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 1F));
            this.tLP_Users.Size = new System.Drawing.Size(993, 630);
            this.tLP_Users.TabIndex = 1;
            // 
            // gBoxUserAccess
            // 
            this.gBoxUserAccess.Controls.Add(this.flowLayoutPanel_UserAccess);
            this.gBoxUserAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBoxUserAccess.Location = new System.Drawing.Point(474, 261);
            this.gBoxUserAccess.Margin = new System.Windows.Forms.Padding(4);
            this.gBoxUserAccess.Name = "gBoxUserAccess";
            this.gBoxUserAccess.Padding = new System.Windows.Forms.Padding(4);
            this.gBoxUserAccess.Size = new System.Drawing.Size(495, 312);
            this.gBoxUserAccess.TabIndex = 17;
            this.gBoxUserAccess.TabStop = false;
            this.gBoxUserAccess.Text = "Настройки уровня доступа пользователя";
            // 
            // flowLayoutPanel_UserAccess
            // 
            this.flowLayoutPanel_UserAccess.AutoScroll = true;
            this.flowLayoutPanel_UserAccess.Controls.Add(this.groupBox2);
            this.flowLayoutPanel_UserAccess.Controls.Add(this.groupBox1);
            this.flowLayoutPanel_UserAccess.Controls.Add(this.chB_Kit_Set);
            this.flowLayoutPanel_UserAccess.Controls.Add(this.chB_Technology_Set);
            this.flowLayoutPanel_UserAccess.Controls.Add(this.chB_Planning_Set);
            this.flowLayoutPanel_UserAccess.Controls.Add(this.chB_Reports_Set);
            this.flowLayoutPanel_UserAccess.Controls.Add(this.chB_Users_Set);
            this.flowLayoutPanel_UserAccess.Controls.Add(this.chB_Settings_Set);
            this.flowLayoutPanel_UserAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_UserAccess.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel_UserAccess.Location = new System.Drawing.Point(4, 20);
            this.flowLayoutPanel_UserAccess.Margin = new System.Windows.Forms.Padding(4);
            this.flowLayoutPanel_UserAccess.Name = "flowLayoutPanel_UserAccess";
            this.flowLayoutPanel_UserAccess.Size = new System.Drawing.Size(487, 288);
            this.flowLayoutPanel_UserAccess.TabIndex = 0;
            this.flowLayoutPanel_UserAccess.WrapContents = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chB_Orders_Set);
            this.groupBox2.Controls.Add(this.chB_Orders_Set_View);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(245, 46);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // chB_Orders_Set
            // 
            this.chB_Orders_Set.AutoSize = true;
            this.chB_Orders_Set.Location = new System.Drawing.Point(7, 18);
            this.chB_Orders_Set.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Orders_Set.Name = "chB_Orders_Set";
            this.chB_Orders_Set.Size = new System.Drawing.Size(76, 21);
            this.chB_Orders_Set.TabIndex = 9;
            this.chB_Orders_Set.Text = "Заказы";
            this.chB_Orders_Set.UseVisualStyleBackColor = true;
            // 
            // chB_Orders_Set_View
            // 
            this.chB_Orders_Set_View.AutoSize = true;
            this.chB_Orders_Set_View.Location = new System.Drawing.Point(86, 18);
            this.chB_Orders_Set_View.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Orders_Set_View.Name = "chB_Orders_Set_View";
            this.chB_Orders_Set_View.Size = new System.Drawing.Size(141, 21);
            this.chB_Orders_Set_View.TabIndex = 11;
            this.chB_Orders_Set_View.Text = "Только просмотр";
            this.chB_Orders_Set_View.UseVisualStyleBackColor = true;
            this.chB_Orders_Set_View.CheckedChanged += new System.EventHandler(this.chB_Orders_Set_View_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chB_Fact_Set_View);
            this.groupBox1.Controls.Add(this.chB_Fact_Set);
            this.groupBox1.Location = new System.Drawing.Point(3, 55);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 46);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // chB_Fact_Set_View
            // 
            this.chB_Fact_Set_View.AutoSize = true;
            this.chB_Fact_Set_View.Location = new System.Drawing.Point(87, 18);
            this.chB_Fact_Set_View.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Fact_Set_View.Name = "chB_Fact_Set_View";
            this.chB_Fact_Set_View.Size = new System.Drawing.Size(141, 21);
            this.chB_Fact_Set_View.TabIndex = 11;
            this.chB_Fact_Set_View.Text = "Только просмотр";
            this.chB_Fact_Set_View.UseVisualStyleBackColor = true;
            this.chB_Fact_Set_View.CheckedChanged += new System.EventHandler(this.chB_Fact_Set_View_CheckedChanged);
            // 
            // chB_Fact_Set
            // 
            this.chB_Fact_Set.AutoSize = true;
            this.chB_Fact_Set.Location = new System.Drawing.Point(7, 18);
            this.chB_Fact_Set.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Fact_Set.Name = "chB_Fact_Set";
            this.chB_Fact_Set.Size = new System.Drawing.Size(72, 21);
            this.chB_Fact_Set.TabIndex = 10;
            this.chB_Fact_Set.Text = "Факты";
            this.chB_Fact_Set.UseVisualStyleBackColor = true;
            // 
            // chB_Kit_Set
            // 
            this.chB_Kit_Set.AutoSize = true;
            this.chB_Kit_Set.Location = new System.Drawing.Point(4, 108);
            this.chB_Kit_Set.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Kit_Set.Name = "chB_Kit_Set";
            this.chB_Kit_Set.Size = new System.Drawing.Size(123, 21);
            this.chB_Kit_Set.TabIndex = 11;
            this.chB_Kit_Set.Text = "Комплектация";
            this.chB_Kit_Set.UseVisualStyleBackColor = true;
            // 
            // chB_Technology_Set
            // 
            this.chB_Technology_Set.AutoSize = true;
            this.chB_Technology_Set.Location = new System.Drawing.Point(4, 137);
            this.chB_Technology_Set.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Technology_Set.Name = "chB_Technology_Set";
            this.chB_Technology_Set.Size = new System.Drawing.Size(103, 21);
            this.chB_Technology_Set.TabIndex = 12;
            this.chB_Technology_Set.Text = "Технология";
            this.chB_Technology_Set.UseVisualStyleBackColor = true;
            // 
            // chB_Planning_Set
            // 
            this.chB_Planning_Set.AutoSize = true;
            this.chB_Planning_Set.Location = new System.Drawing.Point(4, 166);
            this.chB_Planning_Set.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Planning_Set.Name = "chB_Planning_Set";
            this.chB_Planning_Set.Size = new System.Drawing.Size(124, 21);
            this.chB_Planning_Set.TabIndex = 13;
            this.chB_Planning_Set.Text = "Планирование";
            this.chB_Planning_Set.UseVisualStyleBackColor = true;
            // 
            // chB_Reports_Set
            // 
            this.chB_Reports_Set.AutoSize = true;
            this.chB_Reports_Set.Location = new System.Drawing.Point(4, 195);
            this.chB_Reports_Set.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Reports_Set.Name = "chB_Reports_Set";
            this.chB_Reports_Set.Size = new System.Drawing.Size(78, 21);
            this.chB_Reports_Set.TabIndex = 14;
            this.chB_Reports_Set.Text = "Отчёты";
            this.chB_Reports_Set.UseVisualStyleBackColor = true;
            // 
            // chB_Users_Set
            // 
            this.chB_Users_Set.AutoSize = true;
            this.chB_Users_Set.Location = new System.Drawing.Point(4, 224);
            this.chB_Users_Set.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Users_Set.Name = "chB_Users_Set";
            this.chB_Users_Set.Size = new System.Drawing.Size(121, 21);
            this.chB_Users_Set.TabIndex = 15;
            this.chB_Users_Set.Text = "Пользователи";
            this.chB_Users_Set.UseVisualStyleBackColor = true;
            // 
            // chB_Settings_Set
            // 
            this.chB_Settings_Set.AutoSize = true;
            this.chB_Settings_Set.Location = new System.Drawing.Point(4, 253);
            this.chB_Settings_Set.Margin = new System.Windows.Forms.Padding(4);
            this.chB_Settings_Set.Name = "chB_Settings_Set";
            this.chB_Settings_Set.Size = new System.Drawing.Size(98, 21);
            this.chB_Settings_Set.TabIndex = 16;
            this.chB_Settings_Set.Text = "Настройки";
            this.chB_Settings_Set.UseVisualStyleBackColor = true;
            // 
            // gB_Search
            // 
            this.gB_Search.Controls.Add(this.tB_Search);
            this.gB_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gB_Search.Location = new System.Drawing.Point(24, 57);
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
            // gB_UsersData
            // 
            this.gB_UsersData.Controls.Add(this.mTB_DateStart);
            this.gB_UsersData.Controls.Add(this.lbl_DateStart);
            this.gB_UsersData.Controls.Add(this.label1);
            this.gB_UsersData.Controls.Add(this.cB_Department);
            this.gB_UsersData.Controls.Add(this.lbl_SecondName);
            this.gB_UsersData.Controls.Add(this.tB_SecondName);
            this.gB_UsersData.Controls.Add(this.chB_IsValid);
            this.gB_UsersData.Controls.Add(this.tB_Password);
            this.gB_UsersData.Controls.Add(this.tB_Name);
            this.gB_UsersData.Controls.Add(this.tB_LastName);
            this.gB_UsersData.Controls.Add(this.lbl_Name);
            this.gB_UsersData.Controls.Add(this.lbl_Password);
            this.gB_UsersData.Controls.Add(this.lbl_LastName);
            this.gB_UsersData.Controls.Add(this.lbl_Login);
            this.gB_UsersData.Controls.Add(this.chB_NewUser);
            this.gB_UsersData.Controls.Add(this.tB_Login);
            this.gB_UsersData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gB_UsersData.Location = new System.Drawing.Point(474, 7);
            this.gB_UsersData.Margin = new System.Windows.Forms.Padding(4);
            this.gB_UsersData.Name = "gB_UsersData";
            this.gB_UsersData.Padding = new System.Windows.Forms.Padding(4);
            this.tLP_Users.SetRowSpan(this.gB_UsersData, 3);
            this.gB_UsersData.Size = new System.Drawing.Size(495, 246);
            this.gB_UsersData.TabIndex = 15;
            this.gB_UsersData.TabStop = false;
            this.gB_UsersData.Text = "Данные пользователя";
            // 
            // mTB_DateStart
            // 
            this.mTB_DateStart.Location = new System.Drawing.Point(125, 205);
            this.mTB_DateStart.Mask = "00/00/0000";
            this.mTB_DateStart.Name = "mTB_DateStart";
            this.mTB_DateStart.Size = new System.Drawing.Size(100, 23);
            this.mTB_DateStart.TabIndex = 24;
            this.mTB_DateStart.ValidatingType = typeof(System.DateTime);
            // 
            // lbl_DateStart
            // 
            this.lbl_DateStart.AutoSize = true;
            this.lbl_DateStart.Location = new System.Drawing.Point(35, 208);
            this.lbl_DateStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_DateStart.Name = "lbl_DateStart";
            this.lbl_DateStart.Size = new System.Drawing.Size(86, 17);
            this.lbl_DateStart.TabIndex = 22;
            this.lbl_DateStart.Text = "Работает с:";
            this.lbl_DateStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // tB_Password
            // 
            this.tB_Password.Location = new System.Drawing.Point(124, 176);
            this.tB_Password.Margin = new System.Windows.Forms.Padding(4);
            this.tB_Password.MaxLength = 10;
            this.tB_Password.Name = "tB_Password";
            this.tB_Password.PasswordChar = '*';
            this.tB_Password.Size = new System.Drawing.Size(242, 23);
            this.tB_Password.TabIndex = 8;
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
            // lbl_Password
            // 
            this.lbl_Password.AutoSize = true;
            this.lbl_Password.Location = new System.Drawing.Point(59, 179);
            this.lbl_Password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Password.Name = "lbl_Password";
            this.lbl_Password.Size = new System.Drawing.Size(61, 17);
            this.lbl_Password.TabIndex = 8;
            this.lbl_Password.Text = "Пароль:";
            this.lbl_Password.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
            // chB_NewUser
            // 
            this.chB_NewUser.AutoSize = true;
            this.chB_NewUser.Location = new System.Drawing.Point(374, 24);
            this.chB_NewUser.Margin = new System.Windows.Forms.Padding(4);
            this.chB_NewUser.Name = "chB_NewUser";
            this.chB_NewUser.Size = new System.Drawing.Size(70, 21);
            this.chB_NewUser.TabIndex = 1;
            this.chB_NewUser.TabStop = false;
            this.chB_NewUser.Text = "Новый";
            this.chB_NewUser.UseVisualStyleBackColor = true;
            this.chB_NewUser.CheckedChanged += new System.EventHandler(this.chB_NewUser_CheckedChanged);
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
            // dGV_Users
            // 
            this.dGV_Users.AllowUserToAddRows = false;
            this.dGV_Users.AllowUserToDeleteRows = false;
            this.dGV_Users.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dGV_Users.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGV_Users.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGV_Users.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col_FullName,
            this.Col_Login,
            this.Col_FK_IdDepartment});
            this.dGV_Users.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGV_Users.Location = new System.Drawing.Point(24, 112);
            this.dGV_Users.Margin = new System.Windows.Forms.Padding(4);
            this.dGV_Users.MultiSelect = false;
            this.dGV_Users.Name = "dGV_Users";
            this.dGV_Users.ReadOnly = true;
            this.dGV_Users.RowHeadersWidth = 4;
            this.tLP_Users.SetRowSpan(this.dGV_Users, 3);
            this.dGV_Users.Size = new System.Drawing.Size(442, 510);
            this.dGV_Users.TabIndex = 10;
            this.dGV_Users.TabStop = false;
            this.dGV_Users.CurrentCellChanged += new System.EventHandler(this.dGV_Users_CurrentCellChanged);
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
            this.btn_Create_Update.Location = new System.Drawing.Point(474, 581);
            this.btn_Create_Update.Margin = new System.Windows.Forms.Padding(4);
            this.btn_Create_Update.Name = "btn_Create_Update";
            this.btn_Create_Update.Size = new System.Drawing.Size(495, 41);
            this.btn_Create_Update.TabIndex = 17;
            this.btn_Create_Update.Text = "Создать/Изменить";
            this.btn_Create_Update.UseVisualStyleBackColor = false;
            this.btn_Create_Update.Click += new System.EventHandler(this.btn_Create_Update_Click);
            // 
            // gBox_FilterIsValid
            // 
            this.gBox_FilterIsValid.Controls.Add(this.rBtn_IsValidFalse);
            this.gBox_FilterIsValid.Controls.Add(this.rBtn_IsValidTrue);
            this.gBox_FilterIsValid.Controls.Add(this.rBtn_IsValidall);
            this.gBox_FilterIsValid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gBox_FilterIsValid.Location = new System.Drawing.Point(23, 6);
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
            // F_Users
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 632);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "F_Users";
            this.Text = "Пользователи";
            this.Load += new System.EventHandler(this.F_Users_Load);
            this.panel1.ResumeLayout(false);
            this.tLP_Users.ResumeLayout(false);
            this.gBoxUserAccess.ResumeLayout(false);
            this.flowLayoutPanel_UserAccess.ResumeLayout(false);
            this.flowLayoutPanel_UserAccess.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gB_Search.ResumeLayout(false);
            this.gB_Search.PerformLayout();
            this.gB_UsersData.ResumeLayout(false);
            this.gB_UsersData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGV_Users)).EndInit();
            this.gBox_FilterIsValid.ResumeLayout(false);
            this.gBox_FilterIsValid.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tLP_Users;
        private System.Windows.Forms.DataGridView dGV_Users;
        private System.Windows.Forms.RadioButton rBtn_IsValidFalse;
        private System.Windows.Forms.RadioButton rBtn_IsValidTrue;
        private System.Windows.Forms.RadioButton rBtn_IsValidall;
        private System.Windows.Forms.GroupBox gB_Search;
        private System.Windows.Forms.TextBox tB_Search;
        private System.Windows.Forms.GroupBox gB_UsersData;
        private System.Windows.Forms.GroupBox gBoxUserAccess;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_UserAccess;
        private System.Windows.Forms.TextBox tB_Login;
        private System.Windows.Forms.CheckBox chB_NewUser;
        private System.Windows.Forms.Label lbl_Login;
        private System.Windows.Forms.Label lbl_LastName;
        private System.Windows.Forms.TextBox tB_LastName;
        private System.Windows.Forms.Label lbl_Name;
        private System.Windows.Forms.TextBox tB_Name;
        private System.Windows.Forms.TextBox tB_Password;
        private System.Windows.Forms.Label lbl_Password;
        private System.Windows.Forms.CheckBox chB_IsValid;
        private System.Windows.Forms.CheckBox chB_Orders_Set;
        private System.Windows.Forms.CheckBox chB_Fact_Set;
        private System.Windows.Forms.CheckBox chB_Kit_Set;
        private System.Windows.Forms.CheckBox chB_Technology_Set;
        private System.Windows.Forms.CheckBox chB_Planning_Set;
        private System.Windows.Forms.CheckBox chB_Reports_Set;
        private System.Windows.Forms.CheckBox chB_Users_Set;
        private System.Windows.Forms.CheckBox chB_Settings_Set;
        private System.Windows.Forms.Label lbl_SecondName;
        private System.Windows.Forms.TextBox tB_SecondName;
        private System.Windows.Forms.Button btn_Create_Update;
        private System.Windows.Forms.ComboBox cB_Department;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gBox_FilterIsValid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_Login;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col_FK_IdDepartment;
        private System.Windows.Forms.Label lbl_DateStart;
        private System.Windows.Forms.MaskedTextBox mTB_DateStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chB_Orders_Set_View;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chB_Fact_Set_View;
    }
}