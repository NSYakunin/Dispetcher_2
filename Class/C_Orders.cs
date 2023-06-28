using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace Dispetcher2.Class
{
    sealed class C_Orders
    {


        /// <summary>
        /// Select PK_IdOrder,OrderNum,OrderName From Orders <para />
        /// _IdStatusOrders:1-ожидание,2-открыт,3-закрыт,4-в работе,5-выполнен
        /// </summary>
        /// <param name="_IdStatusOrders">1-ожидание,2-открыт,3-закрыт,4-в работе,5-выполнен</param>
        /// <returns></returns>
        public static void SelectOrdersData(byte _IdStatusOrders, ref DataTable DT)
        {

            try
            {
                DT.Clear();
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "Select PK_IdOrder,OrderNum,OrderName,OrderNum1С,StartDate,PlannedDate,Amount From Orders" + "\n" +
                                      "Where FK_IdStatusOrders = @FK_IdStatusOrders" + "\n" +
                                      "Order by OrderNum";
                    cmd.Connection = C_Gper.con;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdStatusOrders", SqlDbType.TinyInt));
                    cmd.Parameters["@FK_IdStatusOrders"].Value = _IdStatusOrders;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
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
        /// <summary>
        /// SELECT Top(1) PK_IdDetail FROM Sp_Details Where ShcmDetail=@ShcmDetail <para />
        /// @ShcmDetail - ЩЦМ детали
        /// </summary>
        public static bool Check_ShcmDetail(string ShcmDetail, out long _long)
        {
            _long = 0;
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT Top(1) PK_IdDetail FROM Sp_Details" + "\n" +
                                             "Where ShcmDetail=@ShcmDetail and IdLoodsman is not null";//Проверяем только по нормальным деталям, внутренние добавим вручную
                cmd.Parameters.Add(new SqlParameter("@ShcmDetail", SqlDbType.VarChar));
                cmd.Parameters["@ShcmDetail"].Value = ShcmDetail.Trim();
                cmd.Connection = C_Gper.con;
                C_Gper.con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.IsDBNull(0) == false) _long = reader.GetInt64(0);
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
        /// <summary>
        /// SELECT PK_IdDetail FROM Sp_Details<para />
        /// Where ShcmDetail=@ShcmDetail
        /// </summary>
        public static bool Check_ShcmDetail(string ShcmDetail)
        {
            try
            {
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT PK_IdDetail FROM Sp_Details" + "\n" +
                                             "Where ShcmDetail=@ShcmDetail";
                    cmd.Parameters.Add(new SqlParameter("@ShcmDetail", SqlDbType.VarChar));
                    cmd.Parameters["@ShcmDetail"].Value = ShcmDetail.Trim();
                    cmd.Connection = C_Gper.con;
                    C_Gper.con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {/*while (reader.Read()){if (reader.IsDBNull(0) == false) ffff = reader.GetInt32(0);}*/
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

        /// <summary>
        /// SELECT PK_IdDetail FROM Sp_Details<para />
        /// Where IdLoodsman=@IdLoodsman
        /// </summary>
        public static bool Check_IdLoodsman(Int64 IdLoodsman)
        {
            try
            {
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT PK_IdDetail FROM Sp_Details" + "\n" +
                                             "Where IdLoodsman=@IdLoodsman";
                    cmd.Parameters.Add(new SqlParameter("@IdLoodsman", SqlDbType.BigInt));
                    cmd.Parameters["@IdLoodsman"].Value = IdLoodsman;
                    cmd.Connection = C_Gper.con;
                    C_Gper.con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {/*while (reader.Read()){if (reader.IsDBNull(0) == false) ffff = reader.GetInt32(0);}*/
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

        public static bool UpdateOrder(int _PkIdOrder, string _OrderName, string _OrderNum1С, DateTime _StartDate, DateTime _PlannedDate, Int16 _Amount)
        {
            try
            {
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "Update Orders set OrderName=@OrderName,OrderNum1С=@OrderNum1С,StartDate=@StartDate,PlannedDate=@PlannedDate,Amount=@Amount " + "\n" +
                                      "where PK_IdOrder=@PK_IdOrder";

                    cmd.Connection = C_Gper.con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@PK_IdOrder", SqlDbType.Int));
                    cmd.Parameters["@PK_IdOrder"].Value = _PkIdOrder;
                    cmd.Parameters.Add(new SqlParameter("@OrderName", SqlDbType.VarChar));
                    cmd.Parameters["@OrderName"].Value = _OrderName;
                    cmd.Parameters.Add(new SqlParameter("@OrderNum1С", SqlDbType.VarChar));
                    cmd.Parameters["@OrderNum1С"].Value = _OrderNum1С;
                    cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.Date));
                    cmd.Parameters["@StartDate"].Value = _StartDate;
                    cmd.Parameters.Add(new SqlParameter("@PlannedDate", SqlDbType.Date));
                    cmd.Parameters["@PlannedDate"].Value = _PlannedDate;
                    cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.SmallInt));
                    cmd.Parameters["@Amount"].Value = _Amount;
                    //***********************************************************
                    C_Gper.con.Open();
                    cmd.ExecuteNonQuery();
                    C_Gper.con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }















    }
}
