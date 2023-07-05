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
using Dispetcher2.Class;

namespace Dispetcher2.Class
{
    sealed class C_UpdaterSP
    {

        //constructor
        public C_UpdaterSP()
        {


        }

        public void SelectNewDataSp(string NameSp, ref DataTable Dt_Sp, ref DataGridView DGV, int idloodsman)
        {
            Dt_Sp.Clear();
            if (NameSp == "Sp_Details")
            {
                //****************************
                long MaxID = 0;
                //1)делаем запрос к базе и ищем макс IdLoodsman в таблице Sp_Details и грузим из ЛОЦМАНА всё где v.id>макс IdLoodsman
                SelectMaxPk_IdFromSp_Details(ref MaxID);
                Select_SpDetails_From_Loodsman(MaxID, ref Dt_Sp, idloodsman);
                //****************************
                if (Dt_Sp.Rows.Count > 0)
                {
                    //One part settings in Load Form Methods
                    DGV.DataSource = Dt_Sp;
                    DGV.Columns["Col_id"].DataPropertyName = Dt_Sp.Columns["id"].ToString();
                    DGV.Columns["Col_Product"].DataPropertyName = Dt_Sp.Columns["Product"].ToString();
                    DGV.Columns["Col_value"].DataPropertyName = Dt_Sp.Columns["value"].ToString();
                    DGV.Columns["Col_dateofCreate"].DataPropertyName = Dt_Sp.Columns["dateofCreate"].ToString();
                    DGV.Columns["Col_idtype"].DataPropertyName = Dt_Sp.Columns["idtype"].ToString();
                    DGV.Columns["Col_idparent"].DataPropertyName = Dt_Sp.Columns["idparent"].ToString();
                    DGV.Columns["Col_idchild"].DataPropertyName = Dt_Sp.Columns["idchild"].ToString();
                }
            }
        }

        public void InsertDataInSp(string NameSp,ref DataTable Dt_Sp)
        {
            if (NameSp == "Sp_Details") InsertDataInSpDetails(ref Dt_Sp);
        }

#region UpdatesSp_Details 
        private void SelectMaxPk_IdFromSp_Details(ref long _MaxID)
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.Connection = C_Gper.con;
                //cmd.CommandTimeout = 100;
                cmd.CommandText = "Select MAX(IdLoodsman)as IdLoodsman from Sp_Details";
                C_Gper.con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0)) _MaxID = reader.GetInt64(0);
                    }
                }
                reader.Dispose(); reader.Close(); C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Select_SpDetails_From_Loodsman(long _MaxID, ref DataTable DT_SP_Details,int idloodsman)
        {
            DT_SP_Details.Clear();
            try
            {
                string Where = "";
                if (idloodsman == 0) Where = "where a.idattr=235 and v.idtype in (232,233) and  v.idstate in (36,40,30) and v.id>" + _MaxID;
                else Where = "where a.idattr=235 and v.idtype in (232,233) and  v.idstate in (36,40,30) and v.id=" + idloodsman;
                C_Gper.con.ConnectionString = C_Gper.ConStr_Loodsman;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
                cmd.CommandText = "select v.id,v.Product, a.value, v.dateofCreate,v.idtype,r.idparent,r.idchild from rvwVersions v" + "\n" +
                                  "Inner join rvwAttributes a on a.idversion=v.id" + "\n" +
                                  "Left join rvwRelations r on r.id=v.id" + "\n" +
                                  Where + "\n" +
                                  "order by v.id";
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(DT_SP_Details);
                adapter.Dispose();
                C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertDataInSpDetails(ref DataTable Dt_Sp)
        {
            int NumRow = 0;
            try
            {
            
            SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
            cmd.Connection = C_Gper.con;
            cmd.Parameters.Clear();
            long IdLoodsman, IdParent, Idchild;
            DateTime DateofCreateDetail;
            short FK_IdTypeDetail;//r.idparent,r.idchild
            cmd.Parameters.Add(new SqlParameter("@IdLoodsman", SqlDbType.BigInt));
            cmd.Parameters.Add(new SqlParameter("@ShcmDetail", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@NameDetail", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@DateofCreateDetail", SqlDbType.Date));
            cmd.Parameters.Add(new SqlParameter("@FK_IdTypeDetail", SqlDbType.SmallInt));
            cmd.Parameters.Add(new SqlParameter("@IdParent", SqlDbType.BigInt));
            cmd.Parameters.Add(new SqlParameter("@Idchild", SqlDbType.BigInt));
            //for (int i = 0; i < Dt_Sp.Rows.Count; i++)
            //Dt_Sp.Rows[i].ItemArray[4].ToString().Trim()
            foreach (DataRow row in Dt_Sp.Rows)
            {
                if (C_Orders.Check_ShcmDetail(row.ItemArray[1].ToString().Trim()))
                    MessageBox.Show(row.ItemArray[1].ToString().Trim() + " - был загружен ранее", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    cmd.CommandText = "insert into Sp_Details (IdLoodsman,ShcmDetail,NameDetail,DateofCreateDetail,FK_IdTypeDetail,IdParent,Idchild) " + "\n" +
                              "values (@IdLoodsman,@ShcmDetail,@NameDetail,@DateofCreateDetail,@FK_IdTypeDetail,@IdParent,@Idchild)";
                    //Parameters**************************************************
                    if (long.TryParse(row.ItemArray[0].ToString().Trim(), out IdLoodsman))
                        cmd.Parameters["@IdLoodsman"].Value = IdLoodsman;
                    else cmd.Parameters["@IdLoodsman"].Value = DBNull.Value;
                    cmd.Parameters["@ShcmDetail"].Value = row.ItemArray[1].ToString().Trim();
                    cmd.Parameters["@NameDetail"].Value = row.ItemArray[2].ToString().Trim();
                    if (DateTime.TryParse(row.ItemArray[3].ToString().Trim(), out DateofCreateDetail))
                        cmd.Parameters["@DateofCreateDetail"].Value = DateofCreateDetail;
                    else cmd.Parameters["@DateofCreateDetail"].Value = DBNull.Value;
                    if (short.TryParse(row.ItemArray[4].ToString().Trim(), out FK_IdTypeDetail))
                        cmd.Parameters["@FK_IdTypeDetail"].Value = FK_IdTypeDetail;
                    else cmd.Parameters["@FK_IdTypeDetail"].Value = DBNull.Value;
                    if (long.TryParse(row.ItemArray[5].ToString().Trim(), out IdParent))
                        cmd.Parameters["@IdParent"].Value = IdParent;
                    else cmd.Parameters["@IdParent"].Value = DBNull.Value;
                    if (long.TryParse(row.ItemArray[6].ToString().Trim(), out Idchild))
                        cmd.Parameters["@Idchild"].Value = Idchild;
                    else cmd.Parameters["@Idchild"].Value = DBNull.Value;
                    C_Gper.con.Open();
                    cmd.ExecuteNonQuery();
                    C_Gper.con.Close();
                    NumRow++;
                }
            }
            MessageBox.Show("Всего строк: " + Dt_Sp.Rows.Count.ToString() + "\nЗагружено: " + NumRow.ToString(), "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Всего строк: " + Dt_Sp.Rows.Count.ToString() + "\nЗагружено: " + NumRow.ToString(), "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Dt_Sp.Clear();
            }
        }

        public static void SelectDetailsInWork(ref DataTable DT)
        {
            try
            {
                DT.Clear();
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "Select IdLoodsman,PK_IdDetail,ShcmDetail From OrdersDetails" + "\n" +
                                      "Inner JOIN Sp_Details on Sp_Details.PK_IdDetail = OrdersDetails.FK_IdDetail and IdLoodsman is not null" + "\n" +
                                      "Group by IdLoodsman,PK_IdDetail,ShcmDetail";
                    cmd.Connection = C_Gper.con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public static bool InsertTechnologyDetails(long FK_IdDetails, string NumOper, Int16 FK_IdOperation, int Tpd, int Tsh)
        {
            try
            {
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "insert into Sp_TechnologyDetails (FK_IdDetails,NumOper,FK_IdOperation,Tpd,Tsh) " + "\n" +
                                  "values (@FK_IdDetails,@NumOper,@FK_IdOperation,@Tpd,@Tsh)";
                    cmd.Connection = C_Gper.con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_IdDetails", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdDetails"].Value = FK_IdDetails;
                    cmd.Parameters.Add(new SqlParameter("@NumOper", SqlDbType.VarChar));
                    cmd.Parameters["@NumOper"].Value = NumOper;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOperation", SqlDbType.SmallInt));
                    cmd.Parameters["@FK_IdOperation"].Value = FK_IdOperation;
                    cmd.Parameters.Add(new SqlParameter("@Tpd", SqlDbType.Int));
                    cmd.Parameters["@Tpd"].Value = Tpd;
                    cmd.Parameters.Add(new SqlParameter("@Tsh", SqlDbType.Int));
                    cmd.Parameters["@Tsh"].Value = Tsh;
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
#endregion

        public static bool InsertFolderInSp_Details(long IdLoodsman, string SHCM, string NameDetail)
        {
            try
            {
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "insert into Sp_Details (IdLoodsman,ShcmDetail,NameDetail,FK_IdTypeDetail) " + "\n" +
                          "values (@IdLoodsman,@ShcmDetail,@NameDetail,346)";//233
                    cmd.Connection = C_Gper.con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@IdLoodsman", SqlDbType.BigInt));
                    cmd.Parameters["@IdLoodsman"].Value = IdLoodsman;
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




    }
}
