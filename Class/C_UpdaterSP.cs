﻿using System;
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
        IConfig config;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Orders orders;

        //constructor
        public C_UpdaterSP(IConfig config)
        {
            this.config = config;
            orders = new C_Orders(config);
        }
        public string LastError { get; set; }
        public void SelectNewDataSp(string NameSp, ref DataTable Dt_Sp, ref DataGridView DGV, int idloodsman)
        {
            Dt_Sp.Clear();
            if (NameSp == "Sp_Details")
            {
                //****************************
                long MaxID = 0;
                //1)делаем запрос к базе и ищем макс IdLoodsman в таблице Sp_Details и грузим из ЛОЦМАНА всё где v.id>макс IdLoodsman
                //SelectMaxPk_IdFromSp_Details(ref MaxID);
                Select_SpDetails_From_Loodsman(984417, ref Dt_Sp, idloodsman);
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
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Connection = con;
                    //cmd.CommandTimeout = 100;
                    cmd.CommandText = "Select MAX(IdLoodsman)as IdLoodsman from Sp_Details";
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0)) _MaxID = reader.GetInt64(0);
                        }
                    }
                    reader.Dispose(); reader.Close();
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Select_SpDetails_From_Loodsman(long _MaxID, ref DataTable DT_SP_Details,int idloodsman)
        {
            DT_SP_Details.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    string Where = "";
                    if (idloodsman == 0) Where = "where a.idattr=235 and v.idtype in (232,233) and  v.idstate in (36,40,30) and v.id>" + _MaxID;
                    else Where = "where a.idattr=235 and v.idtype in (232,233,234) and  v.idstate in (36,40,30) and v.id=" + idloodsman;
                    con.ConnectionString = config.LoodsmanConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.CommandText = "select v.id,v.Product, a.value, v.dateofCreate,v.idtype,r.idparent,r.idchild from rvwVersions v" + "\n" +
                                      "Inner join rvwAttributes a on a.idversion=v.id" + "\n" +
                                      "Left join rvwRelations r on r.id=v.id" + "\n" +
                                      Where + "\n" +
                                      "order by v.id";
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT_SP_Details);
                    adapter.Dispose();
                    
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertDataInSpDetails(ref DataTable Dt_Sp)
        {
            int NumRow = 0;
            int counter = 0;
            try
            {
            
            
            //for (int i = 0; i < Dt_Sp.Rows.Count; i++)
            //Dt_Sp.Rows[i].ItemArray[4].ToString().Trim()
            foreach (DataRow row in Dt_Sp.Rows)
            {
                if (orders.Check_ShcmDetail(row.ItemArray[1].ToString().Trim())) Console.WriteLine(counter++);
                    //MessageBox.Show(row.ItemArray[1].ToString().Trim() + " - был загружен ранее", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                {
                        using (SqlConnection con = new SqlConnection())
                        {
                            con.ConnectionString = config.ConnectionString;
                            SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                            cmd.Connection = con;
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
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            NumRow++;
                        }
                }
            }
            MessageBox.Show("Всего строк: " + Dt_Sp.Rows.Count.ToString() + "\nЗагружено: " + NumRow.ToString(), "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Всего строк: " + Dt_Sp.Rows.Count.ToString() + "\nЗагружено: " + NumRow.ToString(), "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                Dt_Sp.Clear();
            }
        }

        public void SelectDetailsInWork(ref DataTable DT)
        {
            try
            {
                DT.Clear();
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "Select IdLoodsman,PK_IdDetail,ShcmDetail From OrdersDetails" + "\n" +
                                      "Inner JOIN Sp_Details on Sp_Details.PK_IdDetail = OrdersDetails.FK_IdDetail and IdLoodsman is not null" + "\n" +
                                      "Group by IdLoodsman,PK_IdDetail,ShcmDetail";
                    cmd.Connection = con;
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

        public void DeleteTechnologyDetails(Int64 FK_IdDetails)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "delete from Sp_TechnologyDetails where FK_IdDetails=@FK_IdDetails";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_IdDetails", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdDetails"].Value = FK_IdDetails;
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

        public bool InsertTechnologyDetails(long FK_IdDetails, string NumOper, Int16 FK_IdOperation, int Tpd, int Tsh)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "insert into Sp_TechnologyDetails (FK_IdDetails,NumOper,FK_IdOperation,Tpd,Tsh) " + "\n" +
                                  "values (@FK_IdDetails,@NumOper,@FK_IdOperation,@Tpd,@Tsh)";
                    cmd.Connection = con;
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
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LastError = "InsertTechnologyDetails Не работает. " + ex.Message;
                return false;
            }
        }
#endregion

        public bool InsertFolderInSp_Details(long IdLoodsman, string SHCM, string NameDetail)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "insert into Sp_Details (IdLoodsman,ShcmDetail,NameDetail,FK_IdTypeDetail) " + "\n" +
                          "values (@IdLoodsman,@ShcmDetail,@NameDetail,346)";//233
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@IdLoodsman", SqlDbType.BigInt));
                    cmd.Parameters["@IdLoodsman"].Value = IdLoodsman;
                    cmd.Parameters.Add(new SqlParameter("@ShcmDetail", SqlDbType.VarChar));
                    cmd.Parameters["@ShcmDetail"].Value = SHCM.Trim();
                    cmd.Parameters.Add(new SqlParameter("@NameDetail", SqlDbType.VarChar));
                    cmd.Parameters["@NameDetail"].Value = NameDetail.Trim();
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    
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
