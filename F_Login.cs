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
        public F_Login()
        {

            InitializeComponent();
        }

        private void F_Login_Load(object sender, EventArgs e)
        {
            Hide_gB_NewLogin(true);
            DataTable DT = new DataTable();
            C_DataBase DB = new C_DataBase(C_Gper.ConnStrDispetcher2);
            string sql = "SELECT StartDate, EndDate, Note FROM Sp_Note" + "\n" +
                         "Where '" + DateTime.Now.ToShortDateString() + "' >= StartDate and '" + DateTime.Now.ToShortDateString() + "' <= EndDate \n" +
                         "order by StartDate desc";
            DB.Select_DT(ref DT, sql);
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
            C_Gper.AddTablesDs_Sp();
            //Определяем текущего пользователя
            //lbl_User.Text = Environment.UserName + "|" + Environment.MachineName;
            lbl_User.Text = "Логин: " + Environment.UserName;

            string[] devs = { "NSYakunin", "IAPotapov" };
            if (!devs.Contains(Environment.UserName))  panel1.Visible = false;
            else panel1.Visible = true;

            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            this.comboBox1.SelectedIndex = appSettings["SelectedIndex"] == "0" ? 0 : 1;
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
                    if (mychB_NewLogin.Checked) C_Gper.ActiveUserLogin = tB_NewLogin.Text.Trim();
                    else
                        C_Gper.ActiveUserLogin = Environment.UserName;
                    //*****************************************
                    string pass;
                    if (C_Gper.ActiveUserLogin.Length == 0) MessageBox.Show("Введите логин пользователя.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        if (!CheckUserPass(C_Gper.ActiveUserLogin, out pass)) MessageBox.Show("Доступ запрещён.(Логин))", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        else
                            if (pass != tB_Password.Text.Trim()) MessageBox.Show("Доступ запрещён.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            else
                            {
                                F_Index Form_Index = new F_Index();
                                this.Visible = false;
                                Form_Index.ShowDialog();
                                this.Close();
                            }
                }
            }
        }

        private bool CheckUserPass(string LoginUser,out string pass)
        {
            pass = "";
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT Pass,F_Orders,F_Orders_View,F_Fact,F_Fact_View,F_Kit,F_Technology,F_Planning,F_Reports,F_Users,F_Settings" + "\n" +
                "FROM UsersAccess" + "\n" +
                "Where FK_Login=@FK_Login";
                cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                cmd.Parameters["@FK_Login"].Value = LoginUser;
                cmd.Connection = C_Gper.con;
                C_Gper.con.Open();
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
                    reader.Dispose(); reader.Close(); C_Gper.con.Close();
                    return true;
                }
                else
                {
                    reader.Dispose(); reader.Close(); C_Gper.con.Close();
                    return false;
                }
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 1)
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                comboBox1.SelectedIndex = Convert.ToInt32(appSettings["SelectedIndex"]);
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["SelectedIndex"].Value = "1";
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
                Application.Restart();

            }
            else
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                comboBox1.SelectedIndex = Convert.ToInt32(appSettings["SelectedIndex"]);
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["SelectedIndex"].Value = "0";
                config.Save();
                ConfigurationManager.RefreshSection("appSettings");
                Application.Restart();

            }
        }
    }
}
