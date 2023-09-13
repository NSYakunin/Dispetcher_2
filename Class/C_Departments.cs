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
        IConfig config;

        public C_Departments(IConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Select PK_IdDepartment,Department  <para />
        /// From SP_Department <para />
        /// Order by Department
        /// </summary>
        public void Select_Departments(ref DataTable DT)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    DT.Clear();
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "Select PK_IdDepartment,Department" + "\n" +
                    "From SP_Department" + "\n" +
                    "Order by Department";
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }








    }
}
