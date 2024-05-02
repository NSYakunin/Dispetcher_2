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
    public partial class F_Workers : Form
    {
        IConfig config;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Departments departments;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Users users;

        DataTable DT_SP_Department = new DataTable();
        DataTable _DT_SP_Job = new DataTable();
        DataTable _DT_Workers = new DataTable();
        BindingSource BindingSource_Workers = new BindingSource();


        public F_Workers(IConfig config)
        {
            this.config = config;
            departments = new C_Departments(config);
            users = new C_Users(config);
            InitializeComponent();
        }

        private void Select_Job()
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    _DT_SP_Job.Clear();
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "Select Pk_IdJob,NameJob From Sp_Job" + "\n" +
                    "Where JobIsvalid = 1" + "\n" +
                    "Order by NameJob";
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT_SP_Job);
                    adapter.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void F_Workers_Load(object sender, EventArgs e)
        {
            Select_Job();
            cB_SpJob.DataSource = _DT_SP_Job;
            cB_SpJob.DisplayMember = "NameJob";
            cB_SpJob.ValueMember = "Pk_IdJob";
            departments.Select_Departments(ref DT_SP_Department);
            //Load Users List
            SelectAllWorkers();
            cB_Department.DataSource = DT_SP_Department;
            cB_Department.DisplayMember = "Department";
            cB_Department.ValueMember = "PK_IdDepartment";
            //Bindings
            dGV_Workers.AutoGenerateColumns = false;
            dGV_Workers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Workers.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BindingSource_Workers.DataSource = _DT_Workers;
            dGV_Workers.DataSource = BindingSource_Workers;
            dGV_Workers.Columns["Col_FullName"].DataPropertyName = _DT_Workers.Columns["FullName"].ToString();//1
            dGV_Workers.Columns["Col_Login"].DataPropertyName = _DT_Workers.Columns["PK_Login"].ToString();//2
            dGV_Workers.Columns["Col_FK_IdDepartment"].DataPropertyName = _DT_Workers.Columns["FK_IdDepartment"].ToString();//2

            BindAllInterfaceElevents();
        }

        private void SelectAllWorkers()
        {
            _DT_Workers.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "Select (LastName+' '+Name+' '+ SecondName) as FullName,PK_Login,LastName,Name,SecondName,FK_IdDepartment,IsValid,RateWorker,DateStart,Fk_IdJob,ITR,TabNum,DateEnd,ShowTimeSheets" + "\n" +
                    "From Users" + "\n" +
                    "Where OnlyUser = 0" + "\n" +
                    "Order by FullName";
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT_Workers);
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

        private void dGV_Workers_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dGV_Workers.CurrentRow != null)
            {
                if (dGV_Workers.CurrentRow.Cells["Col_FK_IdDepartment"].Value != null) cB_Department.SelectedValue = dGV_Workers.CurrentRow.Cells["Col_FK_IdDepartment"].Value;
                CurrencyManager cmgr = (CurrencyManager)this.dGV_Workers.BindingContext[this.dGV_Workers.DataSource, dGV_Workers.DataMember];
                DataRow row = ((DataRowView)cmgr.Current).Row;
                double RateWorker = Convert.ToDouble(row["RateWorker"]);
                if (RateWorker != 1) chB_RateWorker.Checked = true; else chB_RateWorker.Checked = false;
                if (row["Fk_IdJob"].ToString() != "")
                {
                    cB_SpJob.DataSource = _DT_SP_Job;
                    cB_SpJob.DisplayMember = "NameJob";
                    cB_SpJob.ValueMember = "Pk_IdJob";
                    cB_SpJob.SelectedValue = row["Fk_IdJob"];
                }
                else
                {
                    cB_SpJob.DataSource = null;
                    cB_SpJob.DataSource = _DT_SP_Job;
                    cB_SpJob.DisplayMember = "NameJob";
                    cB_SpJob.ValueMember = "Pk_IdJob";
                    cB_SpJob.SelectedValue = row["Fk_IdJob"];
                    cB_SpJob.SelectedIndex = -1;
                    
                }
                
                if (Convert.ToBoolean(row["ITR"])) rBtn_ITR.Checked = true; else rBtn_non_ITR.Checked = true;

            }
            else
            {
                cB_Department.SelectedIndex = -1;
                cB_SpJob.SelectedIndex = -1;
            }
        }

        #region Binding
        private void BindTextBox(TextBox tB, string NameDataSetTables)
        {
            tB.DataBindings.Clear();
            Binding b1 = new Binding("Text", BindingSource_Workers, NameDataSetTables);
            b1.ControlUpdateMode = ControlUpdateMode.OnPropertyChanged;
            b1.DataSourceUpdateMode = DataSourceUpdateMode.Never;
            tB.DataBindings.Add(b1);
        }

        private void BindCheckBox(CheckBox ChB, string NameDataSetTables)
        {
            ChB.DataBindings.Clear();
            ChB.DataBindings.Add("CheckState", BindingSource_Workers, NameDataSetTables, true, DataSourceUpdateMode.Never, CheckState.Unchecked);
        }

        private void BindAllInterfaceElevents()
        {
            BindTextBox(tB_Login, "PK_Login");
            BindTextBox(tB_LastName, "LastName");
            BindTextBox(tB_Name, "Name");
            BindTextBox(tB_SecondName, "SecondName");
            BindTextBox(tB_TabNum, "TabNum");
            BindCheckBox(chB_IsValid, "IsValid");
            BindCheckBox(chB_TimeSheets, "ShowTimeSheets");
            //Settings user
            mTB_DateStart.DataBindings.Clear();
            mTB_DateStart.DataBindings.Add("Text", BindingSource_Workers, "DateStart", true, DataSourceUpdateMode.Never);
            mTB_DateEnd.DataBindings.Clear();
            mTB_DateEnd.DataBindings.Add("Text", BindingSource_Workers, "DateEnd", true, DataSourceUpdateMode.Never);

        }
        #endregion

        #region Filters
        private void rBtn_Filters()
        {
            //rBtn_IsValidall
            if (rBtn_IsValidall.Checked) BindingSource_Workers.Filter = "";
            //rBtn_IsValidTrue
            if (rBtn_IsValidTrue.Checked) BindingSource_Workers.Filter = "IsValid=1";
            //rBtn_IsValidFalse
            if (rBtn_IsValidFalse.Checked) BindingSource_Workers.Filter = "IsValid=0";
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
            chB_RateWorker.Checked = false;
            mTB_DateStart.DataBindings.Clear();
            mTB_DateEnd.DataBindings.Clear();
            chB_TimeSheets.Checked = false;
        }

        private void chB_NewWorker_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_NewWorker.Checked)
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
            if (tB_Login.Text.Trim() == "") MessageBox.Show("Заполните поле \"Логин\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                if (!DateTime.TryParse(mTB_DateStart.Text, out DateStart)) MessageBox.Show("Укажите дату выхода сотрудника на работу - \"Работает с:\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    if (DateStart > DateTime.Now || DateStart.Year < DateTime.Now.Year - 70) MessageBox.Show("Укажите корректную дату выхода сотрудника на работу - \"Работает с:\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        //т.к. некоторые сотрудники хз с какого отдела и их должности неизвестны
                        //if (cB_SpJob.SelectedValue == null) MessageBox.Show("Укажите должность.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //else
                    {
                        int SelectedNumRow = BindingSource_Workers.Position;
                        if (users.Check_PK_Login(tB_Login.Text.Trim()))
                        {
                            if (chB_NewWorker.Checked) MessageBox.Show("Сотрудник с таким логином уже зарегистрирован. Повторная регистрация невозможна.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            else
                            {
                                UpdateorInsertWorker(true, DateStart);
                            }
                        }
                        else UpdateorInsertWorker(false, DateStart);
                        //Load Users List
                        SelectAllWorkers();
                        BindingSource_Workers.Position = SelectedNumRow;
                        MessageBox.Show("Данные сохранены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        if (chB_NewWorker.Checked) chB_NewWorker.Checked = false;
                    }
        }


        private void UpdateorInsertWorker(bool Update, DateTime DateStart)
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
                        cmd.CommandText = "Update Users set LastName=@LastName,Name=@Name,SecondName=@SecondName,OnlyUser=@OnlyUser,FK_IdDepartment=@FK_IdDepartment,IsValid=@IsValid,RateWorker=@RateWorker,DateStart=@DateStart,DateEnd=@DateEnd,Fk_IdJob=@Fk_IdJob,ITR=@ITR,TabNum=@TabNum,ShowTimeSheets=@ShowTimeSheets" + "\n" +
                    "where PK_Login=@PK_Login";
                    else
                        cmd.CommandText = "insert into Users (PK_Login,LastName,Name,SecondName,OnlyUser,FK_IdDepartment,IsValid,RateWorker,DateStart,DateEnd,Fk_IdJob,ITR,TabNum,ShowTimeSheets) " + "\n" +
                        "values (@PK_Login,@LastName,@Name,@SecondName,@OnlyUser,@FK_IdDepartment,@IsValid,@RateWorker,@DateStart,@DateEnd,@Fk_IdJob,@ITR,@TabNum,@ShowTimeSheets)";
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
                    cmd.Parameters["@OnlyUser"].Value = false;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdDepartment", SqlDbType.Int));
                    if (cB_Department.SelectedValue == null) cmd.Parameters["@FK_IdDepartment"].Value = DBNull.Value;
                    else cmd.Parameters["@FK_IdDepartment"].Value = cB_Department.SelectedValue;
                    cmd.Parameters.Add(new SqlParameter("@IsValid", SqlDbType.Bit));
                    cmd.Parameters["@IsValid"].Value = chB_IsValid.Checked;
                    cmd.Parameters.Add(new SqlParameter("@RateWorker", SqlDbType.Float));
                    if (chB_RateWorker.Checked) cmd.Parameters["@RateWorker"].Value = 0.5;
                    else cmd.Parameters["@RateWorker"].Value = 1;
                    cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date));
                    cmd.Parameters["@DateStart"].Value = DateStart;
                    DateTime DateEnd;
                    cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date));
                    if (DateTime.TryParse(mTB_DateEnd.Text, out DateEnd))
                        cmd.Parameters["@DateEnd"].Value = DateEnd;
                    else cmd.Parameters["@DateEnd"].Value = DBNull.Value;
                    cmd.Parameters.Add(new SqlParameter("@Fk_IdJob", SqlDbType.Int));
                    if (cB_SpJob.SelectedValue == null) cmd.Parameters["@Fk_IdJob"].Value = DBNull.Value;
                    else cmd.Parameters["@Fk_IdJob"].Value = cB_SpJob.SelectedValue;
                    cmd.Parameters.Add(new SqlParameter("@ITR", SqlDbType.Bit));
                    cmd.Parameters["@ITR"].Value = rBtn_ITR.Checked;
                    cmd.Parameters.Add(new SqlParameter("@TabNum", SqlDbType.VarChar));
                    if (tB_TabNum.Text.Trim() == "") cmd.Parameters["@TabNum"].Value = DBNull.Value;
                    else cmd.Parameters["@TabNum"].Value = tB_TabNum.Text.Trim();
                    cmd.Parameters.Add(new SqlParameter("@ShowTimeSheets", SqlDbType.Bit));
                    cmd.Parameters["@ShowTimeSheets"].Value = chB_TimeSheets.Checked;
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

        private void tB_TabNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void tB_Search_TextChanged(object sender, EventArgs e)
        {
            rBtn_IsValidall.Checked = true;
            BindingSource_Workers.Filter = "FullName like '%" + tB_Search.Text.ToString().Trim() + "%'";
        }



    











    }
}
