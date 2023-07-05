using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Dispetcher2.Class
{
    sealed class C_Gper
    {
        public static SqlConnection con = new SqlConnection();//using System.Data.SqlClient;
        //***********************************************************************************************************************************************************
        //Data Source=Loodsman;Initial Catalog=Dispetcher;Integrated Security=true //oldDispetcher connection string

        //string ConnectionString = "Data Source=" + NameServ + ";Initial Catalog=" + NameBase + ";Persist Security Info=True;User ID=" + LoginUser + ";Password=" + PasUser;
        //string ConnectionString = "Data Source=" + NameServ + ";Initial Catalog=" + NameBase + ";Integrated Security=true";
        private static string _NameServ_Dispetcher = "ASCON";
        private static string _NameBase_Dispetcher = "Dispetcher";
        public static string ConnStrDispetcher = "Data Source=" + _NameServ_Dispetcher + ";Initial Catalog=" + _NameBase_Dispetcher + ";Integrated Security=true";
        //***********************************************************************************************************************************************************
        //private static string _NameServ_Dispetcher2 = "ASCON";
        private static string _NameServ_Dispetcher2 = "TESTSRV";
        private static string _NameBase_Dispetcher2 = "Dispetcher2";
        //private static string _NameBase_Dispetcher2 = "Disp2_TEST";
        public static string ConnStrDispetcher2 = "Data Source=" + _NameServ_Dispetcher2 + ";Initial Catalog=" + _NameBase_Dispetcher2 + ";Integrated Security=true";
        //public static string ConnStrDispetcher2 = "Data Source=" + _NameServ_Dispetcher2 + ";Initial Catalog=" + _NameBase_Dispetcher2 + ";Persist Security Info=False;Trusted_Connection=False;User ID=test;Password=test123456789";
        //public static string ConnStrDispetcher2 = "Data Source=" + _NameServ_Dispetcher2 + ";Initial Catalog=" + _NameBase_Dispetcher2 + ";Persist Security Info=False;Trusted_Connection=False;User ID=test;Password=RPSsql12345";
        //***********************************************************************************************************************************************************
        private static string _NameServ_DispetcherASCON = "ASCON";
        private static string _NameBase_DispetcherASCON = "Dispetcher";
        public static string ConnStrDispetcherASCON = "Data Source=" + _NameServ_DispetcherASCON + ";Initial Catalog=" + _NameBase_DispetcherASCON + ";Integrated Security=true";
        //***********************************************************************************************************************************************************
        //private static string _NameServ_Loodsman = "loodsman";
        private static string _NameServ_Loodsman = "ASCON";
        private static string _NameBase_Loodsman = "НИИПМ";
        //private static string _ConnectionStringLoodsman = "Data Source=" + _NameServ_Loodsman + ";Initial Catalog=" + _NameBase_Loodsman + ";Persist Security Info=false;User ID=test;Password=RPSsql12345";
        //private static string _ConnectionStringLoodsman = "Data Source=" + _NameServ_Loodsman + ";Initial Catalog=" + _NameBase_Loodsman + ";Integrated Security=true";
        private static string _ConnectionStringLoodsman = "Data Source=" + _NameServ_Loodsman + ";Initial Catalog=" + _NameBase_Loodsman + ";Persist Security Info=false;User ID=test;Password=test123456789";

        public static string ConStr_Loodsman
        {
            get
            {
                return _ConnectionStringLoodsman;
            }
        }
        //***********************************************************************************************************************************************************
        public static System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowCurrencySymbol;
        public static System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");

        public static string DecimalToHours(decimal time)
        {
            string[] temp = time.ToString(C_Gper.culture).Split('.');
            if (time.ToString().IndexOf(".") > 0) temp = time.ToString().Split('.');
            return temp[0] + ":" + temp[1] + ":00";
        }

        public static int DecimalToSec(decimal time)
        {
            string[] temp = time.ToString(C_Gper.culture).Split('.');
            if (time.ToString().IndexOf(".") > 0) temp = time.ToString().Split('.');
            return (Convert.ToInt32(temp[0]) * 3600) + Convert.ToInt32(temp[1]) * 60;
        }
        //Настройки пользователя
        public static string ActiveUserLogin = "";
        public static string ActiveUserFullName = "";
        //SettingsUser
        public static bool Orders_Set = false;
        public static bool F_Orders_View = false;
        public static bool Fact_Set = false;
        public static bool F_Fact_View = false;
        public static bool Kit_Set = false;
        public static bool Technology_Set = false;
        public static bool Planning_Set = false;
        public static bool Reports_Set = false;
        public static bool Users_Set = false;
        public static bool Settings_Set = false;
        //***************************************
        public static DataSet Ds = new DataSet();//using System.Data;
        //ActiveReport
        public static byte NameReport = 0;
        /* 
         * 3 - Отчёт-наряд по выполненным операциям
         * 6 - Движение деталей
         * 7 - Отчет по выполненным операциям (разр.)
         * 106 - План-график (форма №6)
         * 117 - Операции выполненные рабочим по заказам (форма №17)
         * */






        //DataSet.Tables["DataSet_Tables"].Rows[n]["NameColumn"].ToString();
        //MessageBox.Show("Создание заказа успешно завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //MessageBox.Show("Не указан пароль.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //MessageBox.Show("Заказ с таким номером уже зарегестрирован. Статус заказа не указан.", "Сохранение заказа отменено!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //DialogResult dR = MessageBox.Show("Продолжить запись данных?", "Внимание!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //                        if (dR == DialogResult.Yes)
        //***************************************
        //CurrencyManager cmgr = (CurrencyManager)dGV_Orders.BindingContext[dGV_Orders.DataSource, dGV_Orders.DataMember];
        //DataRow row = ((DataRowView)cmgr.Current).Row;
        //string NameOper = row["Oper"].ToString();
        //***************************************
        public static void AddTablesDs_Sp()
        {
            Ds.Tables.Add("Users");
            //Ds.Tables["Users"].Columns.Add("Login");
            //Ds.Tables["Users"].Columns.Add("FullName");
            //Ds.Tables["Users"].Columns.Add("ShortName");
            //Ds.Tables["Users"].Columns.Add("Password");
            //Ds.Tables["Users"].Columns.Add("isValid");
        }

        public static string GetServerName()
        {
            SqlConnectionStringBuilder b = new SqlConnectionStringBuilder(ConnStrDispetcher2);
            string s = b.DataSource;
            return s;
        }

    }
}
