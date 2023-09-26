using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Dispetcher2.Class;
using System.Collections.Specialized;
using System.Configuration;

namespace Dispetcher2
{
    public partial class F_Login : Form
    {
        static string ActiveUserLogin = "";

        // модель представления для списка серверов
        LoginViewModel vm;
        // Конфигурация
        IConfig config;
        // Форма, которая открывается в случае успеха
        Form successForm;

        // Нарушение правила разделения ответственности!
        // Требуется вынести работу с базой данных в шаблон Repository (Хранилище)
        public F_Login(LoginViewModel vm, IConfig config, Form successForm)
        {
            if (vm == null) throw new Exception("Пожалуйста укажите параметр vm");
            if (config == null) throw new Exception("Пожалуйста укажите параметр config");
            if (successForm == null) throw new Exception("Пожалуйста укажите параметр successForm");

            this.vm = vm;
            this.config = config;
            this.successForm = successForm;

            InitializeComponent();

            // привязка списка серверов к модели представления
            serverComboBox.DataSource = vm.ServerList;
            serverComboBox.DisplayMember = "Name";
            // OnPropertyChanged не работает, все равно привязка не обновляется до потери фокуса
            serverComboBox.DataBindings.Add("SelectedItem", vm, "SelectedServer", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void F_Login_Load(object sender, EventArgs e)
        {
            ProcessLoad();
        }

        void ProcessLoad()
        {
            Hide_gB_NewLogin(true);
            DataTable DT = new DataTable();
            Sp_Note(DT);
            if (DT.Rows.Count == 0) HideServerMessage(true);
            else
            {
                foreach (DataRow row in DT.Rows)
                {
                    if (tB_ServerMessage.Text.Trim() == "") tB_ServerMessage.Text = Convert.ToDateTime(row.ItemArray[0]).ToShortDateString() + " - " + row.ItemArray[2].ToString();
                    else tB_ServerMessage.Text += Environment.NewLine + Convert.ToDateTime(row.ItemArray[0]).ToShortDateString() + " - " + row.ItemArray[2].ToString();

                }
                //HideServerMessage(false);
            }

            //Определяем текущего пользователя
            //lbl_User.Text = Environment.UserName + "|" + Environment.MachineName;
            lbl_User.Text = "Логин: " + Environment.UserName;

            string[] devs = { "NSYakunin", "IAPotapov" };
            if (!devs.Contains(Environment.UserName)) panel1.Visible = false;
            else panel1.Visible = true;

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            this.serverComboBox.SelectedIndex = appSettings["SelectedIndex"] == "0" ? 0 : 1;
        }

        private void Hide_gB_NewLogin(bool hide)
        {
            if (hide)
            {
                gB_NewLogin.Visible = false;
                tLP_Autorization.RowStyles[6].Height = 0;
                tB_Password.Focus();
            }
            else
            {
                gB_NewLogin.Visible = true;
                tLP_Autorization.RowStyles[6].Height = 60;
                tB_NewLogin.Focus();
            }
        }

        private void HideServerMessage(bool hide)
        {
            if (hide)
            {
                lbl_ServerMessage.Visible = false;
                tB_ServerMessage.Visible = false;
                //Позиционируем
                tLP_Autorization.RowStyles[3].SizeType = SizeType.Percent;
                tLP_Autorization.RowStyles[3].Height = 30;//%
                tLP_Autorization.RowStyles[11].Height = 70;//%
            }
            else
            {
                lbl_ServerMessage.Visible = true;
                tB_ServerMessage.Visible = true;
                //Позиционируем
                tLP_Autorization.RowStyles[3].Height = 60;//%
                tLP_Autorization.RowStyles[11].Height = 40;//%
            }
        }

        private void mychB_NewLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (mychB_NewLogin.Checked) Hide_gB_NewLogin(false); else Hide_gB_NewLogin(true);
        }

        private void tB_NewLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) tB_Password.Focus();
        }

        private void tB_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (tB_Password.Text.Trim() == "") MessageBox.Show("Не указан пароль!", "Внимание!!! ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    //*****************************************
                    if (mychB_NewLogin.Checked) ActiveUserLogin = tB_NewLogin.Text.Trim();
                    else
                        ActiveUserLogin = Environment.UserName;
                    //*****************************************
                    string pass;
                    if (ActiveUserLogin.Length == 0) MessageBox.Show("Введите логин пользователя.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    {
                        if (!CheckUserPass(ActiveUserLogin, out pass)) MessageBox.Show("Доступ запрещён.(Логин))", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        else
                            if (pass != tB_Password.Text.Trim()) MessageBox.Show("Доступ запрещён.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        else
                        {
                            this.Hide();
                            successForm.ShowDialog();
                            this.Close();
                        }
                    }
                }
            }
        }

        private bool CheckUserPass(string LoginUser,out string pass)
        {
            pass = "";
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT Pass,F_Orders,F_Orders_View,F_Fact,F_Fact_View,F_Kit,F_Technology,F_Planning,F_Reports,F_Users,F_Settings" + "\n" +
                    "FROM UsersAccess" + "\n" +
                    "Where FK_Login=@FK_Login";
                    cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                    cmd.Parameters["@FK_Login"].Value = LoginUser;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0)) pass = reader.GetString(0);
                            C_Gper.Orders_Set = reader.GetBoolean(1);
                            C_Gper.F_Orders_View = reader.GetBoolean(2);
                            C_Gper.Fact_Set = reader.GetBoolean(3);
                            C_Gper.F_Fact_View = reader.GetBoolean(4);
                            C_Gper.Kit_Set = reader.GetBoolean(5);
                            C_Gper.Technology_Set = reader.GetBoolean(6);
                            C_Gper.Planning_Set = reader.GetBoolean(7);
                            C_Gper.Reports_Set = reader.GetBoolean(8);
                            C_Gper.Users_Set = reader.GetBoolean(9);
                            C_Gper.Settings_Set = reader.GetBoolean(10);
                        }
                        reader.Dispose(); reader.Close();
                        return true;
                    }
                    else
                    {
                        reader.Dispose(); reader.Close();
                        return false;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void OnSelectionChangeCommitted(object sender, EventArgs e)
        {
            // этот костыль приходится писать, чтобы принудительно обновить привязку
            // иначе в Windows Forms обновление привязки не произойдет до потери фокуса
            try
            {
                ((ComboBox)sender).DataBindings["SelectedItem"].WriteValue();
            }
            catch
            {
                // игнорируем исключения в WriteValue
                vm.SelectedServer = null;
            }
            Action a = this.ProcessLoad;
            this.BeginInvoke(a);
        }

        void Sp_Note(DataTable DT)
        {
            
            string sql = "SELECT StartDate, EndDate, Note FROM Sp_Note" + "\n" +
                         "Where '" + DateTime.Now.ToShortDateString() + "' >= StartDate and '" + DateTime.Now.ToShortDateString() + "' <= EndDate \n" +
                         "order by StartDate desc";

            DT.Clear();
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//using System.Data.SqlClient;
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);//adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void F_Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
