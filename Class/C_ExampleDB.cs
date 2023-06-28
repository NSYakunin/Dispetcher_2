using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Dispetcher2.Class
{
    sealed class C_ExampleDB
    {
        DataTable _DT = new DataTable();

        private void SqlDataReader_Example()//Тестовая - пока не нужна
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT [id],[position],[idZakaz] " + "\n" +
                "FROM [Dispetcher].[dbo].[Relations] " + "\n" +
                "order by idZakaz,position ";
                using (C_Gper.con)
                {
                    C_Gper.con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            int ffff = 0;
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(0) == false) ffff = reader.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectForm17(DateTime DateStart, DateTime DateEnd, string loginWorker)
        {
            try
            {
                _DT.Clear();
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//using System.Data.SqlClient;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT DateFactOper,OrderNum,Position,ShcmDetail,NameDetail,NameOperation,OnlyOncePay," + "\n" +
                        "Tpd,Tsh,AmountDetails,FK_LoginWorker,FK_IdBrigade,AmountWorkers,FullName" + "\n" +
                        "From vwForm17Workers" + "\n" +
                        "Where DateFactOper>=@DateStart and DateFactOper<=@DateEnd and FK_LoginWorker=@FK_LoginWorker" + "\n" +
                        "union all" + "\n" +
                        "SELECT DateFactOper,OrderNum,Position,ShcmDetail,NameDetail,NameOperation,OnlyOncePay," + "\n" +
                        "Tpd,Tsh,AmountDetails,FK_LoginWorker,FK_IdBrigade,AmountWorkers,FullName" + "\n" +
                        "From vwForm17Brigades" + "\n" +
                        "Where DateFactOper>=@DateStart and DateFactOper<=@DateEnd and FK_LoginWorker=@FK_LoginWorker" + "\n" +
                        "Order by OrderNum,Position";
                    cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date));
                    cmd.Parameters["@DateStart"].Value = DateStart;
                    cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date));
                    cmd.Parameters["@DateEnd"].Value = DateEnd;
                    cmd.Parameters.Add(new SqlParameter("@FK_LoginWorker", SqlDbType.VarChar));
                    cmd.Parameters["@FK_LoginWorker"].Value = loginWorker;
                    cmd.Connection = C_Gper.con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT);
                    adapter.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool Insert_Example(string SHCM, string NameDetail)
        {
            try
            {
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "insert into Sp_Details (IdLoodsman,ShcmDetail,NameDetail,FK_IdTypeDetail) " + "\n" +
                          "values (1,@ShcmDetail,@NameDetail,233)";
                    cmd.Connection = C_Gper.con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@ShcmDetail", SqlDbType.VarChar));
                    cmd.Parameters["@ShcmDetail"].Value = SHCM.Trim();
                    cmd.Parameters.Add(new SqlParameter("@NameDetail", SqlDbType.VarChar));
                    cmd.Parameters["@NameDetail"].Value = NameDetail.Trim();
                    //***********************************************************
                    C_Gper.con.Open();
                    cmd.ExecuteNonQuery();
                    C_Gper.con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        public static void DeleteTechnologyDetails(Int64 FK_IdDetails)
        {
            try
            {
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "delete from Sp_TechnologyDetails where FK_IdDetails=@FK_IdDetails";
                    cmd.Connection = C_Gper.con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_IdDetails", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdDetails"].Value = FK_IdDetails;
                    //***********************************************************
                    C_Gper.con.Open();
                    cmd.ExecuteNonQuery();
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
