using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using Dispetcher2.DialogsForms;

namespace Dispetcher2.Class
{
    sealed class C_Details
    {
        
        IConfig config;


        public C_Details(IConfig config)
        {
            this.config = config;
        }



        public void GetTehnologyFromLoodsman(ref DataTable DT, long IdLoodsman)
        {
            try
            {
                DT.Clear();
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.LoodsmanConnectionString;
                    //Console.WriteLine(IdLoodsman);
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                                                                              //cmd.CommandText = "SELECT v.product AS shcm,LEFT(v2.product, 3) + ' ' + att.value AS oper,Tpd.value AS Tpd,Tsh.value AS Tsh" + "\n" +
                                                                              //sel = "SELECT LEFT(v2.product, 3) + ' ' + att.value AS Oper,Tpd.asfloat AS Tpd,Tsh.asfloat AS Tsh";
                                                                              //sel = "SELECT LEFT(v2.product, 3) + ' ' + att.value AS Oper,Tpd.asfloat/60 AS Tpd,Tsh.asfloat/60 AS Tsh";
                    cmd.CommandText =
                        "SELECT ra.value + ' ' + att.value AS Oper, Tpd.value AS Tpd, Tsh.asfloat AS Tsh, @IdLoodsman AS IdLoodsman" + "\n" +
                        "FROM [НИИПМ].[dbo].[rvwRelations] r" + "\n" +
                        "INNER JOIN rvwRelations AS r2 ON r2.idparent = r.idparent AND r2.idlinktype = 32" + "\n" +
                        "INNER JOIN rvwRelationAttributes ra ON ra.idrelation  = r2.id and ra.attrtype = 0" + "\n" +
                        "INNER JOIN rvwAttributes AS att ON att.idversion = r2.idChild AND att.idattr = 235" + "\n" +
                        "LEFT JOIN rvwAttributes AS Tpd ON Tpd.idversion = r2.idChild AND Tpd.idattr = 321" + "\n" +
                        "LEFT JOIN rvwAttributes AS Tsh ON Tsh.idversion = r2.idChild AND Tsh.idattr = 195" + "\n" +
                        "WHERE r.idchild = @IdLoodsman AND r.idlinktype = 33" + "\n" +
                        "ORDER BY Oper";

                    cmd.Connection = con;
                    cmd.Parameters.Add(new SqlParameter("@IdLoodsman", SqlDbType.BigInt));
                    cmd.Parameters["@IdLoodsman"].Value = IdLoodsman;

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
        /// SELECT FK_IdOperation,NumOperation +' ' + NameOperation as Operation,Tpd,Tsh FROM Sp_OperationsType111<para />
        /// INNER JOIN Sp_Operations on Sp_Operations.PK_IdOperation = Sp_OperationsType111.FK_IdOperation<para />
        /// WHERE FK_IdDetail=@FK_IdDetail
        /// </summary>
        public void SelectTehnologyForType111(long FK_IdDetail, ref DataTable DT)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    DT.Clear();
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "SELECT FK_IdOperation,LTrim(NumOperation +' ' + NameOperation) AS Oper,Tpd,Tsh" + "\n" +
                    "FROM Sp_OperationsType111 " + "\n" +
                    "INNER JOIN Sp_Operations on Sp_Operations.PK_IdOperation = Sp_OperationsType111.FK_IdOperation " + "\n" +
                    "WHERE FK_IdDetail=@FK_IdDetail" + "\n" +
                    "ORDER BY oper";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdDetail", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdDetail"].Value = FK_IdDetail;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                    con.Close();
                    if (DT.Rows.Count > 0) DT.Rows.Add(32, "Передача детали на СГД", 0, 0);
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Int16 Find_FK_IdOperationInSp_Operations(string NameOperations)
        {
            Int16 PK_IdOperation = 0;
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select PK_IdOperation From Sp_Operations" + "\n" +
                                      "Where NameOperation = '" + NameOperations.Trim() + "'";
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            if (reader.IsDBNull(0) == false) PK_IdOperation = reader.GetInt16(0); else PK_IdOperation = 0;
                    }
                    reader.Dispose(); reader.Close(); con.Close();
                    return PK_IdOperation;
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return PK_IdOperation;
            }
        }

        public string Find_FK_Login_UsersInSp_Operations(Int16 PK_IdOperation)
        {
            string FK_LoginWorker = "";
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select FK_Login_Users From Sp_Operations" + "\n" +
                                      "Where PK_IdOperation = '" + PK_IdOperation + "'";
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            if (reader.IsDBNull(0) == false) FK_LoginWorker = reader.GetString(0); else FK_LoginWorker = "";
                    }
                    reader.Dispose(); reader.Close(); con.Close();
                    return FK_LoginWorker;
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return FK_LoginWorker;
            }
        }




        /// <summary>
        /// SELECT Position,ShcmDetail,NameDetail,AmountDetails,NameType,PK_IdOrderDetail,FK_IdDetail,IdLoodsman <para />
        /// Where FK_IdOrder=@FK_IdOrder and ShcmDetail like '%" + shcm.Trim() + "%'
        /// </summary>
        public void SelectAllDetailsLikeSHCM(int FK_IdOrder, string shcm, ref DataTable DT)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    DT.Clear();
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "SELECT Position,ShcmDetail,NameDetail,AmountDetails,NameType,PK_IdOrderDetail,FK_IdDetail,IdLoodsman " + "\n" +
                        "FROM OrdersDetails " + "\n" +
                        "LEFT JOIN Sp_Details ON OrdersDetails.FK_IdDetail=Sp_Details.PK_IdDetail " + "\n" +
                        "LEFT JOIN Sp_TypeDetails ON Sp_Details.FK_IdTypeDetail=Sp_TypeDetails.PK_IdTypeDetail " + "\n" +
                        "Where FK_IdOrder=@FK_IdOrder and ShcmDetail like '%" + shcm.Trim() + "%'";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOrder", SqlDbType.Int));
                    cmd.Parameters["@FK_IdOrder"].Value = FK_IdOrder;
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
        /// insert into FactOperation (FK_IdOrderDetail,NumOper,FK_IdOperation,Tpd,Tsh,AmountDetails,FK_LoginWorker,FK_IdBrigade)<para />
        /// values (@FK_IdOrderDetail,@NumOper,@FK_IdOperation,@Tpd,@Tsh,@AmountDetails,@FK_LoginWorker,@FK_IdBrigade)
        /// </summary>
        public bool InsertFactOperation(long FK_IdOrderDetail, string NumOper, Int16 FK_IdOperation, int Tpd, int Tsh, int AmountDetails, DateTime DateFactOper, string FK_LoginWorker, int FK_IdBrigade)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "insert into FactOperation (FK_IdOrderDetail,NumOper,FK_IdOperation,Tpd,Tsh,AmountDetails,DateFactOper,FK_LoginWorker,FK_IdBrigade) " + "\n" +
                                  "values (@FK_IdOrderDetail,@NumOper,@FK_IdOperation,@Tpd,@Tsh,@AmountDetails,@DateFactOper,@FK_LoginWorker,@FK_IdBrigade)";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOrderDetail", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdOrderDetail"].Value = FK_IdOrderDetail;
                    cmd.Parameters.Add(new SqlParameter("@NumOper", SqlDbType.VarChar));
                    cmd.Parameters["@NumOper"].Value = NumOper;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOperation", SqlDbType.SmallInt));
                    cmd.Parameters["@FK_IdOperation"].Value = FK_IdOperation;
                    cmd.Parameters.Add(new SqlParameter("@Tpd", SqlDbType.Int));
                    cmd.Parameters["@Tpd"].Value = Tpd;
                    cmd.Parameters.Add(new SqlParameter("@Tsh", SqlDbType.Int));
                    cmd.Parameters["@Tsh"].Value = Tsh;
                    cmd.Parameters.Add(new SqlParameter("@AmountDetails", SqlDbType.Int));
                    cmd.Parameters["@AmountDetails"].Value = AmountDetails;
                    cmd.Parameters.Add(new SqlParameter("@DateFactOper", SqlDbType.Date));
                    cmd.Parameters["@DateFactOper"].Value = DateFactOper;
                    cmd.Parameters.Add(new SqlParameter("@FK_LoginWorker", SqlDbType.VarChar));
                    if (FK_LoginWorker == "") cmd.Parameters["@FK_LoginWorker"].Value = DBNull.Value;
                    else cmd.Parameters["@FK_LoginWorker"].Value = FK_LoginWorker;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdBrigade", SqlDbType.Int));
                    if (FK_IdBrigade == 0) cmd.Parameters["@FK_IdBrigade"].Value = DBNull.Value;
                    else cmd.Parameters["@FK_IdBrigade"].Value = FK_IdBrigade;
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

        /// <summary>
        /// Select sum(AmountDetails)as AmountDetails From FactOperation<para />
        /// "Where FK_IdOrderDetail = @FK_IdOrderDetail and FK_IdOperation = @FK_IdOperation"
        /// </summary>
        /// <returns> int AmountDetails</returns>
        /*public static int Select_AmountFactDetailsOper(long FK_IdOrderDetail, int FK_IdOperation,string NumOper)
        {
            int AmountDetails = 0;
            try
            {
                con.ConnectionString = config.ConnectionString;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.Parameters.Clear();
                cmd.CommandText = "Select sum(AmountDetails)as AmountDetails" + "\n" +
                                  "From FactOperation" + "\n" +
                                  "Where FK_IdOrderDetail = @FK_IdOrderDetail and FK_IdOperation = @FK_IdOperation and NumOper=@NumOper";
                cmd.Parameters.Add(new SqlParameter("@FK_IdOrderDetail", SqlDbType.BigInt));
                cmd.Parameters["@FK_IdOrderDetail"].Value = FK_IdOrderDetail;
                cmd.Parameters.Add(new SqlParameter("@FK_IdOperation", SqlDbType.SmallInt));
                cmd.Parameters["@FK_IdOperation"].Value = FK_IdOperation;
                cmd.Parameters.Add(new SqlParameter("@NumOper", SqlDbType.VarChar));
                cmd.Parameters["@NumOper"].Value = NumOper;
                cmd.Connection = con;
                con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                        if (reader.IsDBNull(0) == false) AmountDetails = reader.GetInt32(0); else AmountDetails = 0;
                }
                reader.Dispose(); reader.Close(); con.Close();
                return AmountDetails;
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return AmountDetails;
            }
        }*/

        public int Select_AmountFactDetailsOper(long FK_IdOrderDetail, string NumOper)
        {
            int AmountDetails = 0;
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select sum(AmountDetails)as AmountDetails" + "\n" +
                                      "From FactOperation" + "\n" +
                                      "Where FK_IdOrderDetail = @FK_IdOrderDetail and NumOper=@NumOper";
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOrderDetail", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdOrderDetail"].Value = FK_IdOrderDetail;
                    cmd.Parameters.Add(new SqlParameter("@NumOper", SqlDbType.VarChar));
                    cmd.Parameters["@NumOper"].Value = NumOper;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                            if (reader.IsDBNull(0) == false) AmountDetails = reader.GetInt32(0); else AmountDetails = 0;
                    }
                    reader.Dispose(); reader.Close(); con.Close();
                    return AmountDetails;
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return AmountDetails;
            }
        }


        /// <summary>
        /// SELECT Ltrim(NumOper + ' ' + NameOperation) as Oper,SUM(AmountDetails) as AmountDetails,Tpd,Tsh FROM FactOperation<para />
        /// Where FK_IdOrderDetail = @FK_IdOrderDetail
        /// </summary>
        public void SelectFactOperForDetail(long FK_IdOrderDetail, ref DataTable DT)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    DT.Clear();
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "SELECT Ltrim(NumOper + ' ' + NameOperation) as FactOper,SUM(AmountDetails) as AmountDetails,Tpd as FactTpd,Tsh as FactTsh " + "\n" +
                    "FROM FactOperation " + "\n" +
                    "INNER JOIN Sp_Operations on Sp_Operations.PK_IdOperation = FactOperation.FK_IdOperation " + "\n" +
                    "Where FK_IdOrderDetail = @FK_IdOrderDetail " + "\n" +
                    "Group by NumOper,NameOperation,Tpd,Tsh,LastOper " + "\n" +
                    "Order by LastOper";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOrderDetail", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdOrderDetail"].Value = FK_IdOrderDetail;
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
        /// SELECT LastOper,PK_IdFactOper,Ltrim(NumOper + ' ' + NameOperation) as Oper,DateFactOper,FK_LoginWorker,AmountDetails,Tpd,Tsh FROM FactOperation<para />
        /// "Where FK_IdOrderDetail = @FK_IdOrderDetail and FK_IdBrigade is not null
        /// </summary>
        public void SelectFullFactOperForDetail(long FK_IdOrderDetail, ref DataTable DT)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    DT.Clear();
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "SELECT LastOper,PK_IdFactOper,Ltrim(NumOper + ' ' + NameOperation) as FactOper,DateFactOper,FK_LoginWorker,AmountDetails,Tpd as FactTpd,Tsh as FactTsh  " + "\n" +
                    "FROM FactOperation " + "\n" +
                    "INNER JOIN Sp_Operations on Sp_Operations.PK_IdOperation = FactOperation.FK_IdOperation " + "\n" +
                    "Where FK_IdOrderDetail = @FK_IdOrderDetail and FK_LoginWorker is not null " + "\n" +
                    "union all" + "\n" +
                    "SELECT LastOper,PK_IdFactOper,Ltrim(NumOper + ' ' + NameOperation) as FactOper,DateFactOper,FullName,AmountDetails,Tpd as FactTpd,Tsh as FactTsh " + "\n" +
                    "FROM FactOperation " + "\n" +
                    "INNER JOIN Sp_Operations on Sp_Operations.PK_IdOperation = FactOperation.FK_IdOperation " + "\n" +
                    "INNER JOIN Brigade on Brigade.PK_IdBrigade = FactOperation.FK_IdBrigade " + "\n" +
                    "Where FK_IdOrderDetail = @FK_IdOrderDetail and FK_IdBrigade is not null " + "\n" +
                    "Order by LastOper, FactOper";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOrderDetail", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdOrderDetail"].Value = FK_IdOrderDetail;
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


        public bool DeleteFactOper(long PK_IdFactOper)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "delete from FactOperation where PK_IdFactOper=@PK_IdFactOper";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@PK_IdFactOper", SqlDbType.BigInt));
                    cmd.Parameters["@PK_IdFactOper"].Value = PK_IdFactOper;
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

        public void GetTehnologyFromLoodsman2(ref DataTable DT, ref DataGridView dgv, long IdLoodsman, long PK_IdOrderDetail)
        {
            try
            {
                DT.Clear();
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.LoodsmanConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };

                    // Original query to get technology data
                    cmd.CommandText =
                        "SELECT ra.value + ' ' + att.value AS Oper, Tpd.value AS Tpd, Tsh.asfloat AS Tsh, @IdLoodsman AS IdLoodsman" + "\n" +
                        "FROM [НИИПМ].[dbo].[rvwRelations] r" + "\n" +
                        "INNER JOIN rvwRelations AS r2 ON r2.idparent = r.idparent AND r2.idlinktype = 32" + "\n" +
                        "INNER JOIN rvwRelationAttributes ra ON ra.idrelation  = r2.id and ra.attrtype = 0" + "\n" +
                        "INNER JOIN rvwAttributes AS att ON att.idversion = r2.idChild AND att.idattr = 235" + "\n" +
                        "LEFT JOIN rvwAttributes AS Tpd ON Tpd.idversion = r2.idChild AND Tpd.idattr = 321" + "\n" +
                        "LEFT JOIN rvwAttributes AS Tsh ON Tsh.idversion = r2.idChild AND Tsh.idattr = 195" + "\n" +
                        "WHERE r.idchild = @IdLoodsman AND r.idlinktype = 33" + "\n" +
                        "ORDER BY Oper";

                    cmd.Connection = con;
                    cmd.Parameters.Add(new SqlParameter("@IdLoodsman", SqlDbType.BigInt));
                    cmd.Parameters["@IdLoodsman"].Value = IdLoodsman;

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                    con.Close();
                }

                // Now, retrieve the OTKControl data from our database and merge it with DT
                using (var con = new SqlConnection(config.ConnectionString))
                {
                    con.Open();
                    DT.Rows.Add(32, "Передача детали на СГД", 0, 0);
                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        dgv.Rows[i].Cells["Col_OTKControl"].Value = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
                    }

                    foreach (DataRow dr in DT.Rows)
                    {
                        dr["OTKControl"] = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };

                        string oper = dr["Oper"].ToString();

                        string getOperationIDQuery = "SELECT OperationID FROM OperationsOTK WHERE PK_IdOrderDetail = @PK_IdOrderDetail AND Oper = @Oper";
                        SqlCommand cmdGetOperationID = new SqlCommand(getOperationIDQuery, con);
                        cmdGetOperationID.Parameters.AddWithValue("@PK_IdOrderDetail", PK_IdOrderDetail);
                        cmdGetOperationID.Parameters.AddWithValue("@Oper", oper);

                        object result = cmdGetOperationID.ExecuteScalar();

                        CheckBoxState[] otkControlStates = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };

                        if (result != null)
                        {
                            int operationID = Convert.ToInt32(result);

                            // Get the OTKControl data
                            string getOTKControlQuery = "SELECT CheckBoxIndex, CheckBoxState FROM OTKControl WHERE OperationID = @OperationID";
                            SqlCommand cmdGetOTKControl = new SqlCommand(getOTKControlQuery, con);
                            cmdGetOTKControl.Parameters.AddWithValue("@OperationID", operationID);

                            using (SqlDataReader reader = cmdGetOTKControl.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    int index = reader.GetInt32(0);
                                    CheckBoxState state = (CheckBoxState)reader.GetInt32(1);

                                    if (index >= 0 && index < 3)
                                    {
                                        otkControlStates[index] = state;
                                    }
                                }
                            }
                        }

                    dr["OTKControl"] = otkControlStates;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




    }
}
