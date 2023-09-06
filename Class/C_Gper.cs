using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Microsoft.Office.Interop.Excel;
using System.Collections.Specialized;

namespace Dispetcher2.Class
{
    sealed class C_Gper
    {
        static NameValueCollection appSettings = ConfigurationManager.AppSettings;
        static string SERVER = appSettings["SelectedIndex"];

        public static string ConnStrDispetcher2 = ConfigurationManager.ConnectionStrings[SERVER == "0"? "ConnStrDispetcher2": "TestConnStrDispetcher2"].ConnectionString;
        private static string _ConnectionStringLoodsman = ConfigurationManager.ConnectionStrings[SERVER == "0" ? "_ConnectionStringLoodsman": "_TestConnectionStringLoodsman"].ConnectionString;
      
        public static string ConStr_Loodsman
        {
            get
            {
                return _ConnectionStringLoodsman;
            }
        }

        public static SqlConnection con = new SqlConnection();//using System.Data.SqlClient;
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
        public enum ReportMode
        {
            Неизвестно = 0,
            ОтчетНаряд = 3,
            ДвижениеДеталей = 6,
            ОтчетВыполненным = 7,
            ПланГрафик = 106,
            ОперацииВыполненныеРабочим = 117,
            Трудоемкость = 66,
        }
        public static ReportMode NameReport = ReportMode.Неизвестно;
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