using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispetcher2.Class;
using System.Data.SqlClient;
using System.IO;
using System.Threading;



namespace Dispetcher2
{
    public partial class F_Users : Form
    {
        IConfig config;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Departments departments;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Users users;

        BindingSource BindingSource_Users = new BindingSource();
        DataTable DT_SP_Department = new DataTable();

        //***************************************
        DataSet Ds = new DataSet();//using System.Data;

        public F_Users(IConfig config)
        {
            this.config = config;
            departments = new C_Departments(config);
            users = new C_Users(config);
            AddTablesDs_Sp();

            InitializeComponent();
            
        }
        void AddTablesDs_Sp()
        {
            Ds.Tables.Add("Users");
            //Ds.Tables["Users"].Columns.Add("Login");
            //Ds.Tables["Users"].Columns.Add("FullName");
            //Ds.Tables["Users"].Columns.Add("ShortName");
            //Ds.Tables["Users"].Columns.Add("Password");
            //Ds.Tables["Users"].Columns.Add("isValid");
        }

        private void F_Users_Load(object sender, EventArgs e)
        {

            departments.Select_Departments(ref DT_SP_Department);
            //Load Users List
            SelectAllUsers();
            cB_Department.DataSource = DT_SP_Department;
            cB_Department.DisplayMember = "Department";
            cB_Department.ValueMember = "PK_IdDepartment";
            //Bindings
            dGV_Users.AutoGenerateColumns = false;
            dGV_Users.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Users.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BindingSource_Users.DataSource = Ds;
            BindingSource_Users.DataMember = "Users";
            dGV_Users.DataSource = BindingSource_Users;
            dGV_Users.Columns["Col_FullName"].DataPropertyName = Ds.Tables["Users"].Columns["FullName"].ToString();//1
            dGV_Users.Columns["Col_Login"].DataPropertyName = Ds.Tables["Users"].Columns["PK_Login"].ToString();//2
            dGV_Users.Columns["Col_FK_IdDepartment"].DataPropertyName = Ds.Tables["Users"].Columns["FK_IdDepartment"].ToString();//2

            BindAllInterfaceElevents();
            //chB_Orders_Set.DataBindings.Add("CheckState", BindingSource_Users, "F_Orders", true, DataSourceUpdateMode.Never, CheckState.Unchecked);
            /*BindingSource_Users.DataSource = C_Gper.DT;
            dGV_Users.DataSource = BindingSource_Users;
            dGV_Users.Columns["Col_FullName"].DataPropertyName = C_Gper.DT.Columns["FullName"].ToString();
            chB_Orders_Set.DataBindings.Add("CheckState", BindingSource_Users, "F_Orders", true, DataSourceUpdateMode.Never, CheckState.Unchecked);*/
        }

        private void dGV_Users_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dGV_Users.CurrentRow != null && dGV_Users.CurrentRow.Cells["Col_FK_IdDepartment"].Value != null)
            {
                cB_Department.SelectedValue = dGV_Users.CurrentRow.Cells["Col_FK_IdDepartment"].Value;
                /*CurrencyManager cmgr = (CurrencyManager)this.dGV_Users.BindingContext[this.dGV_Users.DataSource, dGV_Users.DataMember];
                DataRow row = ((DataRowView)cmgr.Current).Row;
                double RateWorker = Convert.ToDouble(row["RateWorker"]);
                if (RateWorker != 1) chB_RateWorker.Checked = true; else chB_RateWorker.Checked = false;*/ 
            }
            else cB_Department.SelectedIndex = -1;
        }


#region Binding
        private void BindTextBox(TextBox tB, string NameDataSetTables)
        {
            tB.DataBindings.Clear();
            Binding b1 = new Binding("Text", BindingSource_Users, NameDataSetTables);
            b1.ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
            b1.DataSourceUpdateMode = DataSourceUpdateMode.Never;
            tB.DataBindings.Add(b1);
        }

        private void BindCheckBox(CheckBox ChB, string NameDataSetTables)
        {
            ChB.DataBindings.Clear();
            ChB.DataBindings.Add("CheckState", BindingSource_Users, NameDataSetTables, true, DataSourceUpdateMode.Never, CheckState.Unchecked);
        }

        private void BindAllInterfaceElevents()
        {
            BindTextBox(tB_Login, "PK_Login");
            BindTextBox(tB_LastName, "LastName");
            BindTextBox(tB_Name, "Name");
            BindTextBox(tB_SecondName, "SecondName");
            BindCheckBox(chB_IsValid, "IsValid");
            //Settings user
            BindTextBox(tB_Password, "Pass");
            BindCheckBox(chB_Orders_Set, "F_Orders");
            BindCheckBox(chB_Orders_Set_View, "F_Orders_View");
            BindCheckBox(chB_Fact_Set, "F_Fact");
            BindCheckBox(chB_Fact_Set_View, "F_Fact_View");
            BindCheckBox(chB_Kit_Set, "F_Kit");
            BindCheckBox(chB_Technology_Set, "F_Technology");
            BindCheckBox(chB_Planning_Set, "F_Planning");
            BindCheckBox(chB_Reports_Set, "F_Reports");
            BindCheckBox(chB_Users_Set, "F_Users");
            BindCheckBox(chB_Settings_Set, "F_Settings");
            mTB_DateStart.DataBindings.Clear();
            mTB_DateStart.DataBindings.Add("Text", BindingSource_Users, "DateStart", true, DataSourceUpdateMode.Never);
            
        }
#endregion

#region Filters
        private void rBtn_Filters()
        {
            //rBtn_IsValidall
            if (rBtn_IsValidall.Checked) BindingSource_Users.Filter = "";
            //rBtn_IsValidTrue
            if (rBtn_IsValidTrue.Checked) BindingSource_Users.Filter = "IsValid=1";
            //rBtn_IsValidFalse
            if (rBtn_IsValidFalse.Checked) BindingSource_Users.Filter = "IsValid=0";
        }

        private void rBtn_IsValidall_CheckedChanged(object sender, EventArgs e)
        {
            rBtn_Filters();
        }

        private void rBtn_IsValidTrue_CheckedChanged(object sender, EventArgs e)
        {
            rBtn_Filters();
        }

        private void rBtn_IsValidFalse_CheckedChanged(object sender, EventArgs e)
        {
            rBtn_Filters();
        }

        private void tB_Search_TextChanged(object sender, EventArgs e)
        {
            rBtn_IsValidall.Checked = true;
            BindingSource_Users.Filter = "FullName like '%" + tB_Search.Text.ToString().Trim() + "%'";
        }
#endregion

        private void ClearAllInterfaceElements()
        {
            tB_Login.Text = "";
            tB_LastName.Text = "";
            tB_Name.Text = "";
            tB_SecondName.Text = "";
            tB_Login.Text = "";
            cB_Department.SelectedIndex = -1;
            chB_IsValid.Checked = true;
            //Settings user
            tB_Password.Text = "";
            chB_Orders_Set.Checked = false;
            chB_Orders_Set_View.Checked = false;
            chB_Fact_Set.Checked = false;
            chB_Fact_Set_View.Checked = false;
            chB_Kit_Set.Checked = false;
            chB_Technology_Set.Checked = false;
            chB_Planning_Set.Checked = false;
            chB_Reports_Set.Checked = false;
            chB_Users_Set.Checked = false;
            chB_Settings_Set.Checked = false;
            mTB_DateStart.DataBindings.Clear();
        }

        private void chB_NewUser_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_NewUser.Checked)
            {
                tB_Login.ReadOnly = false; tB_Login.BackColor = SystemColors.Window;
                ClearAllInterfaceElements();
                tB_Login.Focus();
            }
            else
            {
                BindAllInterfaceElevents();
                tB_Login.ReadOnly = true; tB_Login.BackColor = SystemColors.Info;
            }
        }

        private void btn_Create_Update_Click(object sender, EventArgs e)
        {
            DateTime DateStart = DateTime.Now;
            if (tB_Login.Text.Trim()=="") MessageBox.Show("Заполните поле \"Логин\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                if (!DateTime.TryParse(mTB_DateStart.Text, out DateStart)) MessageBox.Show("Укажите дату выхода сотрудника на работу - \"Работает с:\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    if (DateStart > DateTime.Now || DateStart.Year < DateTime.Now.Year-70) MessageBox.Show("Укажите корректную дату выхода сотрудника на работу - \"Работает с:\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
            {
                int SelectedNumRow = BindingSource_Users.Position;
                if (users.Check_PK_Login(tB_Login.Text.Trim()))
                {
                    if (chB_NewUser.Checked) MessageBox.Show("Сотрудник с таким логином уже зарегистрирован. Повторная регистрация невозможна.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    {
                        UpdateorInsertUser(true, DateStart);
                    }
                }
                else UpdateorInsertUser(false, DateStart);
                DeleteAndInsert_UsersAccess();
                //Load Users List
                SelectAllUsers();
                BindingSource_Users.Position = SelectedNumRow;
                MessageBox.Show("Данные сохранены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                if (chB_NewUser.Checked) chB_NewUser.Checked = false;
            }
                
        }

        private void SelectAllUsers()
        {
            Ds.Tables["Users"].Clear();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "Select (LastName+' '+Name+' '+ SecondName) as FullName,PK_Login,LastName,Name,SecondName,FK_IdDepartment,Department,IsValid,DateStart" + "\n" +
                    ",Pass,F_Orders,F_Orders_View,F_Fact,F_Fact_View,F_Kit,F_Technology,F_Planning,F_Reports,F_Users,F_Settings" + "\n" +
                    "From Users" + "\n" +
                    "LEFT JOIN SP_Department ON FK_IdDepartment = PK_IdDepartment" + "\n" +
                    "LEFT JOIN UsersAccess ON PK_Login=FK_Login" + "\n" +
                    "Where OnlyUser = 1" + "\n" +
                    "Order by FullName";
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(Ds, "Users");
                    //adapter.Fill(C_Gper.DT);
                    adapter.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateorInsertUser(bool Update, DateTime DateStart)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                                                      //cmd.CommandType = CommandType.StoredProcedure;
                                                      //cmd.CommandText = "Delete From Editors_now Where IDRkk=@IDRkk and IDmmpRKK=@IDmmpRKK and YearRkk=@YearRkk and IdTypeRkk_FK=@IdTypeRkk_FK";
                                                      //cmd.CommandText = "[RKKeditor_out]";
                    if (Update)
                        cmd.CommandText = "Update Users set LastName=@LastName,Name=@Name,SecondName=@SecondName,OnlyUser=@OnlyUser,FK_IdDepartment=@FK_IdDepartment,IsValid=@IsValid,DateStart=@DateStart" + "\n" +
                    "where PK_Login=@PK_Login";
                    else
                        cmd.CommandText = "insert into Users (PK_Login,LastName,Name,SecondName,OnlyUser,FK_IdDepartment,IsValid,DateStart) " + "\n" +
                        "values (@PK_Login,@LastName,@Name,@SecondName,@OnlyUser,@FK_IdDepartment,@IsValid,@DateStart)";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SqlParameter("@PK_Login", SqlDbType.VarChar));
                    cmd.Parameters["@PK_Login"].Value = tB_Login.Text.Trim();
                    cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.VarChar));
                    cmd.Parameters["@LastName"].Value = tB_LastName.Text.Trim();
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar));
                    cmd.Parameters["@Name"].Value = tB_Name.Text.Trim();
                    cmd.Parameters.Add(new SqlParameter("@SecondName", SqlDbType.VarChar));
                    cmd.Parameters["@SecondName"].Value = tB_SecondName.Text.Trim();
                    cmd.Parameters.Add(new SqlParameter("@OnlyUser", SqlDbType.Bit));
                    cmd.Parameters["@OnlyUser"].Value = true;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdDepartment", SqlDbType.Int));
                    if (cB_Department.SelectedValue == null) cmd.Parameters["@FK_IdDepartment"].Value = DBNull.Value;
                    else cmd.Parameters["@FK_IdDepartment"].Value = cB_Department.SelectedValue;
                    cmd.Parameters.Add(new SqlParameter("@IsValid", SqlDbType.Bit));
                    cmd.Parameters["@IsValid"].Value = chB_IsValid.Checked;
                    cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date));
                    cmd.Parameters["@DateStart"].Value = DateStart;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteAndInsert_UsersAccess()
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "delete from UsersAccess where FK_Login=@FK_Login" + "\n" +
                        "insert into UsersAccess (FK_Login,Pass,F_Orders,F_Orders_View,F_Fact,F_Fact_View,F_Kit,F_Technology,F_Planning,F_Reports,F_Users,F_Settings) " + "\n" +
                                  "values (@FK_Login,@Pass,@F_Orders,@F_Orders_View,@F_Fact,@F_Fact_View,@F_Kit,@F_Technology,@F_Planning,@F_Reports,@F_Users,@F_Settings)";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                    cmd.Parameters["@FK_Login"].Value = tB_Login.Text.Trim();
                    cmd.Parameters.Add(new SqlParameter("@Pass", SqlDbType.VarChar));
                    cmd.Parameters["@Pass"].Value = tB_Password.Text.Trim();
                    cmd.Parameters.Add(new SqlParameter("@F_Orders", SqlDbType.Bit));
                    cmd.Parameters["@F_Orders"].Value = chB_Orders_Set.Checked;
                    cmd.Parameters.Add(new SqlParameter("@F_Orders_View", SqlDbType.Bit));
                    cmd.Parameters["@F_Orders_View"].Value = chB_Orders_Set_View.Checked;
                    cmd.Parameters.Add(new SqlParameter("@F_Fact", SqlDbType.Bit));
                    cmd.Parameters["@F_Fact"].Value = chB_Fact_Set.Checked;
                    cmd.Parameters.Add(new SqlParameter("@F_Fact_View", SqlDbType.Bit));
                    cmd.Parameters["@F_Fact_View"].Value = chB_Fact_Set_View.Checked;
                    cmd.Parameters.Add(new SqlParameter("@F_Kit", SqlDbType.Bit));
                    cmd.Parameters["@F_Kit"].Value = chB_Kit_Set.Checked;
                    cmd.Parameters.Add(new SqlParameter("@F_Technology", SqlDbType.Bit));
                    cmd.Parameters["@F_Technology"].Value = chB_Technology_Set.Checked;
                    cmd.Parameters.Add(new SqlParameter("@F_Planning", SqlDbType.Bit));
                    cmd.Parameters["@F_Planning"].Value = chB_Planning_Set.Checked;
                    cmd.Parameters.Add(new SqlParameter("@F_Reports", SqlDbType.Bit));
                    cmd.Parameters["@F_Reports"].Value = chB_Reports_Set.Checked;
                    cmd.Parameters.Add(new SqlParameter("@F_Users", SqlDbType.Bit));
                    cmd.Parameters["@F_Users"].Value = chB_Users_Set.Checked;
                    cmd.Parameters.Add(new SqlParameter("@F_Settings", SqlDbType.Bit));
                    cmd.Parameters["@F_Settings"].Value = chB_Settings_Set.Checked;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chB_Orders_Set_View_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_Orders_Set_View.Checked) chB_Orders_Set.Checked = true;
        }

        private void chB_Fact_Set_View_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_Fact_Set_View.Checked) chB_Fact_Set.Checked = true;
        }











        



 





    }
}
