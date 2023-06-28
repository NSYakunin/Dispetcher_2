using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Dispetcher2.Class
{
    sealed class C_Departments
    {
        const int CmdTimeout = 60; //seconds SqlCommand cmd = new SqlCommand() { CommandTimeout = CmdTimeout};


        /// <summary>
        /// Select PK_IdDepartment,Department  <para />
        /// From SP_Department <para />
        /// Order by Department
        /// </summary>
        public static void Select_Departments(ref DataTable DT)
        {
            try
            {
                DT.Clear();
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                cmd.CommandText = "Select PK_IdDepartment,Department" + "\n" +
                "From SP_Department" + "\n" +
                "Order by Department";
                cmd.Connection = C_Gper.con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(DT);
                adapter.Dispose();
                C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }








    }
}
