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
        IConfig config;

        public C_Orders(IConfig config)
        {
            this.config = config;
        }

        /// <summary>
        /// Load orders list from Dispetcher2
        /// </summary>
        public void SelectAllOrders(ref DataTable _DT_Orders)
        {
            _DT_Orders.Clear();
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "Select PK_IdOrder,OrderNum,OrderName,DateCreateOrder,FK_IdStatusOrders,NameStatusOrders,ValidationOrder" + "\n" +
                                      "From Orders" + "\n" +
                                      "LEFT JOIN SP_StatusOrders ON FK_IdStatusOrders = PK_IdStatusOrders";
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT_Orders);
                    adapter.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Load All Details list from Dispetcher2
        /// </summary>
        public void SelectAllDetails(ref DataTable _DT_AllDetails)
        {
            _DT_AllDetails.Clear();
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select NameType,ShcmDetail,NameDetail,PK_IdDetail" + "\n" +
                                      "from Sp_Details" + "\n" +
                                      "Left Join Sp_TypeDetails on Sp_TypeDetails.PK_IdTypeDetail=Sp_Details.FK_IdTypeDetail" + "\n" +
                                      "Where FK_IdTypeDetail = 111" + "\n" +
                                      "order by ShcmDetail";
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT_AllDetails);
                    adapter.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        public bool InsertDetailInOrder(int _FK_IdOrder, long _FK_IdDetail, int _AmountDetails, int _PositionParent, out long _PK_IdOrderDetail)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "insert into OrdersDetails (FK_IdOrder,FK_IdDetail,AmountDetails,PositionParent,AllPositionParent) " + "\n" +
                                          "values (@FK_IdOrder,@FK_IdDetail,@AmountDetails,@PositionParent,@AllPositionParent) " + "\n" +
                                           "SELECT SCOPE_IDENTITY()";
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOrder", SqlDbType.Int));
                    cmd.Parameters["@FK_IdOrder"].Value = _FK_IdOrder;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdDetail", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdDetail"].Value = _FK_IdDetail;
                    cmd.Parameters.Add(new SqlParameter("@AmountDetails", SqlDbType.Int));
                    cmd.Parameters["@AmountDetails"].Value = _AmountDetails;
                    cmd.Parameters.Add(new SqlParameter("@PositionParent", SqlDbType.Int));
                    cmd.Parameters["@PositionParent"].Value = _PositionParent;
                    cmd.Parameters.Add(new SqlParameter("@AllPositionParent", SqlDbType.Int));
                    cmd.Parameters["@AllPositionParent"].Value = _PositionParent.ToString();
                    //***********************************************************
                    con.Open();
                    _PK_IdOrderDetail = Convert.ToInt64(cmd.ExecuteScalar());
                    con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {

                _PK_IdOrderDetail = 0;
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool InsertDetailInSp_Details(string _ShcmDetail, string _NameDetail, out long _PK_IdDetail)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    //Select NameType,ShcmDetail,NameDetail,PK_IdDetail,IdLoodsman
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "insert into Sp_Details (ShcmDetail,NameDetail) " + "\n" +
                                          "values (@ShcmDetail,@NameDetail) " + "\n" +
                                        "SELECT SCOPE_IDENTITY()";
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@ShcmDetail", SqlDbType.VarChar));
                    cmd.Parameters["@ShcmDetail"].Value = _ShcmDetail;
                    cmd.Parameters.Add(new SqlParameter("@NameDetail", SqlDbType.VarChar));
                    cmd.Parameters["@NameDetail"].Value = _NameDetail;
                    //***********************************************************
                    con.Open();
                    _PK_IdDetail = Convert.ToInt64(cmd.ExecuteScalar());
                    con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {

                _PK_IdDetail = 0;
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool InsertDetailInSp_OperationsType111(long _PKIdDetail, DataGridView _DGV)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "insert into Sp_OperationsType111 (FK_IdDetail,NumOperation,FK_IdOperation,Tpd,Tsh) " + "\n" +
                                      "values (@FK_IdDetail,@NumOperation,@FK_IdOperation,@Tpd,@Tsh)";
                    cmd.Parameters.Add(new SqlParameter("@FK_IdDetail", SqlDbType.BigInt));
                    cmd.Parameters.Add(new SqlParameter("@NumOperation", SqlDbType.VarChar));
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOperation", SqlDbType.SmallInt));
                    cmd.Parameters.Add(new SqlParameter("@Tpd", SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@Tsh", SqlDbType.Int));
                    //**********************************************************************************
                    cmd.Parameters["@FK_IdDetail"].Value = _PKIdDetail;
                    foreach (DataGridViewRow row in _DGV.Rows)
                    {
                        cmd.Parameters["@NumOperation"].Value = row.Cells[0].Value.ToString().Trim();
                        cmd.Parameters["@FK_IdOperation"].Value = Convert.ToInt16(row.Cells["FK_IdOperation"].Value.ToString().Trim());
                        cmd.Parameters["@Tpd"].Value = Convert.ToInt32(row.Cells[2].Value.ToString().Trim()) * 60;
                        cmd.Parameters["@Tsh"].Value = Convert.ToInt32(row.Cells[3].Value.ToString().Trim()) * 60;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdateAmountDetailOrFasteners(long _PK_IdOrderDetailorFasteners, double _Amount, bool _Fasteners)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.Parameters.Clear();
                    if (_Fasteners)//Крепёж
                        cmd.CommandText = "Update OrdersFasteners set AmountFasteners=@AmountFasteners " + "\n" +
                    "where PK_IdFasteners=@PK_Id";
                    else//Сборка или деталь
                        cmd.CommandText = "Update OrdersDetails set AmountDetails=@Amount " + "\n" +
                    "where PK_IdOrderDetail=@PK_Id";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.BigInt));
                    cmd.Parameters["@Amount"].Value = (int)_Amount;
                    cmd.Parameters.Add(new SqlParameter("@PK_Id", SqlDbType.BigInt));
                    cmd.Parameters["@PK_Id"].Value = _PK_IdOrderDetailorFasteners;
                    cmd.Parameters.Add(new SqlParameter("@AmountFasteners", SqlDbType.Float));
                    cmd.Parameters["@AmountFasteners"].Value = _Amount;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        //************************************************************************************************
        public void Select_AllOrdersDetails(int _FK_IdOrder, ref DataTable _DT)
        {
            _DT.Clear();
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT NameType,Position,ShcmDetail,NameDetail,AmountDetails,AllPositionParent,PK_IdOrderDetail,FK_IdDetail,PositionParent" + "\n" +
                                      "FROM OrdersDetails" + "\n" +
                                      "LEFT JOIN Sp_Details ON OrdersDetails.FK_IdDetail=Sp_Details.PK_IdDetail" + "\n" +
                                      "LEFT JOIN Sp_TypeDetails ON Sp_Details.FK_IdTypeDetail=Sp_TypeDetails.PK_IdTypeDetail" + "\n" +
                                      "Where FK_IdOrder=@FK_IdOrder" + "\n" +
                                      "union all" + "\n" +
                                      "SELECT NameType,Position,'K',NameFasteners,AmountFasteners,AllPositionParent,FK_IdOrder,0,PositionParent" + "\n" +
                                      "FROM OrdersFasteners" + "\n" +
                                      "LEFT JOIN Sp_TypeDetails ON OrdersFasteners.FK_IdTypeFasteners=Sp_TypeDetails.PK_IdTypeDetail" + "\n" +
                                      "Where FK_IdOrder=@FK_IdOrder" + "\n" +
                                      "Order by Position";
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOrder", SqlDbType.Int));
                    cmd.Parameters["@FK_IdOrder"].Value = _FK_IdOrder;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT);
                    adapter.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //************************************************************************************************

        public void Select_SpOperations(ref DataTable _DT)
        {
            _DT.Clear();
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.CommandText = "Select PK_IdOperation,NameOperation,IsValidOperation" + "\n" +
                                      "From Sp_Operations" + "\n" +
                                      "Where IsValidOperation = 1" + "\n" +
                                      "order by NameOperation";
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT);
                    adapter.Dispose();
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OpenOrders(int _PKIdOrder, byte _FK_IdStatusOrders)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "Update Orders set FK_IdStatusOrders=@FK_IdStatusOrders,ValidationOrder=@ValidationOrder " + "\n" +
                    "where PK_IdOrder=@PK_IdOrder";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_IdStatusOrders", SqlDbType.TinyInt));
                    cmd.Parameters["@FK_IdStatusOrders"].Value = _FK_IdStatusOrders;
                    cmd.Parameters.Add(new SqlParameter("@PK_IdOrder", SqlDbType.Int));
                    cmd.Parameters["@PK_IdOrder"].Value = _PKIdOrder;
                    cmd.Parameters.Add(new SqlParameter("@ValidationOrder", SqlDbType.Bit));
                    cmd.Parameters["@ValidationOrder"].Value = true;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Select PK_IdOrder,OrderNum,OrderName From Orders <para />
        /// _IdStatusOrders:1-ожидание,2-открыт,3-закрыт,4-в работе,5-выполнен
        /// </summary>
        /// <param name="_IdStatusOrders">1-ожидание,2-открыт,3-закрыт,4-в работе,5-выполнен</param>
        /// <returns></returns>
        public void SelectOrdersData(byte _IdStatusOrders, ref DataTable DT)
        {

            try
            {
                DT.Clear();
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "Select PK_IdOrder,OrderNum,OrderName,OrderNum1С,StartDate,PlannedDate,Amount From Orders" + "\n" +
                                      "Where FK_IdStatusOrders = @FK_IdStatusOrders" + "\n" +
                                      "Order by OrderNum";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdStatusOrders", SqlDbType.TinyInt));
                    cmd.Parameters["@FK_IdStatusOrders"].Value = _IdStatusOrders;
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
        /// <summary>
        /// SELECT Top(1) PK_IdDetail FROM Sp_Details Where ShcmDetail=@ShcmDetail <para />
        /// @ShcmDetail - ЩЦМ детали
        /// </summary>
        public bool Check_ShcmDetail(string ShcmDetail, out long _long)
        {
            _long = 0;
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT Top(1) PK_IdDetail FROM Sp_Details" + "\n" +
                                                 "Where ShcmDetail=@ShcmDetail and IdLoodsman is not null";//Проверяем только по нормальным деталям, внутренние добавим вручную
                    cmd.Parameters.Add(new SqlParameter("@ShcmDetail", SqlDbType.VarChar));
                    cmd.Parameters["@ShcmDetail"].Value = ShcmDetail.Trim();
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(0) == false) _long = reader.GetInt64(0);
                        }
                        reader.Dispose(); reader.Close(); con.Close();
                        return true;
                    }
                    else
                    {
                        reader.Dispose(); reader.Close(); con.Close();
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
        /// Where ShcmDetail=@ShcmDetail
        /// </summary>
        public bool Check_ShcmDetail(string ShcmDetail)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT PK_IdDetail FROM Sp_Details" + "\n" +
                                             "Where ShcmDetail=@ShcmDetail";
                    cmd.Parameters.Add(new SqlParameter("@ShcmDetail", SqlDbType.VarChar));
                    cmd.Parameters["@ShcmDetail"].Value = ShcmDetail.Trim();
                    cmd.Connection = con;
                    con.Open();
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
        public bool Check_IdLoodsman(Int64 IdLoodsman)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT PK_IdDetail FROM Sp_Details" + "\n" +
                                             "Where IdLoodsman=@IdLoodsman";
                    cmd.Parameters.Add(new SqlParameter("@IdLoodsman", SqlDbType.BigInt));
                    cmd.Parameters["@IdLoodsman"].Value = IdLoodsman;
                    cmd.Connection = con;
                    con.Open();
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

        public bool UpdateOrder(int _PkIdOrder, string _OrderName, string _OrderNum1С, DateTime _StartDate, DateTime _PlannedDate, Int16 _Amount)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "Update Orders set OrderName=@OrderName,OrderNum1С=@OrderNum1С,StartDate=@StartDate,PlannedDate=@PlannedDate,Amount=@Amount " + "\n" +
                                      "where PK_IdOrder=@PK_IdOrder";

                    cmd.Connection = con;
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
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
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
