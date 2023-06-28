using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.ComponentModel;

namespace Dispetcher2.Class
{
    public class C_DbWorker
    {
        private SqlConnection _con;
        //Конструктор
        public C_DbWorker(string ConnectionString)
        {
            this._con = new SqlConnection();
            this._con.ConnectionString = ConnectionString;
        }


        //[DataObjectMethod(DataObjectMethodType.Select)]
        /*public void ExecuteNonQuery(in string cmd)
        {
            try
            {
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = _con;
                //Выполняем запрос
                _con.Open();
                cmd.ExecuteNonQuery();
                _con.Close();
            }
            catch (Exception ex)
            {
                _con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
        



        //public static SqlConnection con = new SqlConnection();//using System.Data.SqlClient;


        













    }
}
