using System;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
//using System.Net;
using System.Data.SqlClient;

namespace Dispetcher2.Class
{
    sealed class C_F_Orders
    {

                //constructor
        public C_F_Orders()
        { 
        
        }

        //**********************
        #region OrdersForm
        //**********************
        /// <summary>
        /// Load orders list from Dispetcher2
        /// </summary>
        public void SelectAllOrders(ref DataTable _DT_Orders)
        {
            _DT_Orders.Clear();
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.CommandText = "Select PK_IdOrder,OrderNum,OrderName,DateCreateOrder,FK_IdStatusOrders,NameStatusOrders,ValidationOrder" + "\n" +
                                  "From Orders" + "\n" +
                                  "LEFT JOIN SP_StatusOrders ON FK_IdStatusOrders = PK_IdStatusOrders";
                cmd.Connection = C_Gper.con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(_DT_Orders);
                adapter.Dispose();
                C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        /// <summary>
        /// Load All Details list from Dispetcher2
        /// </summary>
        public static void SelectAllDetails(ref DataTable _DT_AllDetails)
        {
            _DT_AllDetails.Clear();
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
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
                C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        public bool InsertDetailInOrder(int _FK_IdOrder, long _FK_IdDetail, int _AmountDetails, int _PositionParent, out long _PK_IdOrderDetail)
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
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
                C_Gper.con.Open();
                _PK_IdOrderDetail = Convert.ToInt64(cmd.ExecuteScalar());
                C_Gper.con.Close();
                return true;
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                _PK_IdOrderDetail = 0;
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool InsertDetailInSp_Details(string _ShcmDetail, string _NameDetail, out long _PK_IdDetail)
        {
            try
            {
                //Select NameType,ShcmDetail,NameDetail,PK_IdDetail,IdLoodsman
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
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
                C_Gper.con.Open();
                _PK_IdDetail = Convert.ToInt64(cmd.ExecuteScalar());
                C_Gper.con.Close();
                return true;
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                _PK_IdDetail = 0;
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool InsertDetailInSp_OperationsType111(long _PKIdDetail, DataGridView _DGV)
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
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
                 cmd.Parameters["@Tpd"].Value = Convert.ToInt32(row.Cells[2].Value.ToString().Trim())*60;
                 cmd.Parameters["@Tsh"].Value = Convert.ToInt32(row.Cells[3].Value.ToString().Trim())*60;
                 C_Gper.con.Open();
                 cmd.ExecuteNonQuery();
                 C_Gper.con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdateAmountDetailOrFasteners(long _PK_IdOrderDetailorFasteners, double _Amount, bool _Fasteners)
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Parameters.Clear();
                if (_Fasteners)//Крепёж
                    cmd.CommandText = "Update OrdersFasteners set AmountFasteners=@AmountFasteners " + "\n" +
                "where PK_IdFasteners=@PK_Id";
                else//Сборка или деталь
                    cmd.CommandText = "Update OrdersDetails set AmountDetails=@Amount " + "\n" +
                "where PK_IdOrderDetail=@PK_Id";
                cmd.Connection = C_Gper.con;
                //Parameters**************************************************
                cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.BigInt));
                cmd.Parameters["@Amount"].Value = (int)_Amount;
                cmd.Parameters.Add(new SqlParameter("@PK_Id", SqlDbType.BigInt));
                cmd.Parameters["@PK_Id"].Value = _PK_IdOrderDetailorFasteners;
                cmd.Parameters.Add(new SqlParameter("@AmountFasteners", SqlDbType.Float));
                cmd.Parameters["@AmountFasteners"].Value = _Amount;
                //***********************************************************
                C_Gper.con.Open();
                cmd.ExecuteNonQuery();
                C_Gper.con.Close();
                return true;
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
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
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
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
                C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //************************************************************************************************

        public void Select_SpOperations(ref DataTable _DT)
        {
            _DT.Clear();
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
                cmd.CommandText = "Select PK_IdOperation,NameOperation,IsValidOperation" + "\n" +
                                  "From Sp_Operations" + "\n" +
                                  "Where IsValidOperation = 1" + "\n" +
                                  "order by NameOperation";
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(_DT);
                adapter.Dispose();
                C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OpenOrders(int _PKIdOrder, byte _FK_IdStatusOrders)
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.CommandText = "Update Orders set FK_IdStatusOrders=@FK_IdStatusOrders,ValidationOrder=@ValidationOrder " + "\n" +
                "where PK_IdOrder=@PK_IdOrder";
                cmd.Connection = C_Gper.con;
                //Parameters**************************************************
                cmd.Parameters.Add(new SqlParameter("@FK_IdStatusOrders", SqlDbType.TinyInt));
                cmd.Parameters["@FK_IdStatusOrders"].Value = _FK_IdStatusOrders;
                cmd.Parameters.Add(new SqlParameter("@PK_IdOrder", SqlDbType.Int));
                cmd.Parameters["@PK_IdOrder"].Value = _PKIdOrder;
                cmd.Parameters.Add(new SqlParameter("@ValidationOrder", SqlDbType.Bit));
                cmd.Parameters["@ValidationOrder"].Value = true;
                //***********************************************************
                C_Gper.con.Open();
                cmd.ExecuteNonQuery();
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
