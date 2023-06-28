using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Dispetcher2.Class
{
    class C_DataBase
    {

        string _ConnectionString;

        public C_DataBase(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public void Select_DT(ref DataTable DT,string SQLtext)
        {
            DT.Clear();
            try
            {
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = _ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//using System.Data.SqlClient;
                    cmd.CommandText = SQLtext;
                    cmd.Connection = C_Gper.con;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);//adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                    C_Gper.con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
















    }
}
