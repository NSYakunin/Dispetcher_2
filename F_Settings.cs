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
using Dispetcher2.Controls;

namespace Dispetcher2
{
    public partial class F_Settings : Form
    {
        KitUpdaterControl kuc = null;
        ImportDataControl idc = null;
        public int SelectedOrderId { set; get; }
        public F_Settings()
        {
            InitializeComponent();
            DT_Orders.Columns.Add("PK_IdOrder", typeof(int));
            DT_Orders.Columns.Add("OrderNum", typeof(string));
            DT_Orders.Columns.Add("OrderName", typeof(string));
            DT_Orders.Columns.Add("OrderNum1С", typeof(string));
            DT_Orders.Columns.Add("StartDate", typeof(DateTime));
            DT_Orders.Columns.Add("PlannedDate", typeof(DateTime));
            DT_Orders.Columns.Add("Amount", typeof(Int16));

            AssemblyTest at = new AssemblyTest();
            var e = at.GetStringList();
            TestResults.DataSource = e;
        }

        private void F_Settings_Load(object sender, EventArgs e)
        {
            dGV_SpDetails.AutoGenerateColumns = false;
            dGV_SpDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_SpDetails.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            
            dGV_Orders.AutoGenerateColumns = false;
            dGV_Orders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Orders.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Orders.DataSource = DT_Orders;
            dGV_Orders.DataSource = BS_Orders;
            dGV_Orders.Columns["Col_OrderNum"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
            //Bindings
            tB_OrderName.DataBindings.Add("Text", BS_Orders, "OrderName", false, DataSourceUpdateMode.OnPropertyChanged);
            tB_OrderNumInfo.DataBindings.Add("Text", BS_Orders, "OrderNum", false, DataSourceUpdateMode.OnPropertyChanged);
            tB_OrderNum1C.DataBindings.Add("Text", BS_Orders, "OrderNum1С", false, DataSourceUpdateMode.OnPropertyChanged);
            dTP_StartOrdDate.DataBindings.Clear();
            dTP_StartOrdDate.DataBindings.Add("Text", BS_Orders, "StartDate", true, DataSourceUpdateMode.OnPropertyChanged);
            dTP_PlannedDate.DataBindings.Clear();
            dTP_PlannedDate.DataBindings.Add("Text", BS_Orders, "PlannedDate", true, DataSourceUpdateMode.OnPropertyChanged);
            numUD_Amount.DataBindings.Add("Text", BS_Orders, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);

            this.DataBindings.Add("SelectedOrderId", BS_Orders, "PK_IdOrder");
        }

        private void mBtnM_Sp_Click(object sender, EventArgs e)
        {
            myTabC_Settings.SelectedTab = tabPage_Sp;
        }

        private void mBtnM_InsertFolder_Click(object sender, EventArgs e)
        {
            myTabC_Settings.SelectedTab = tabPageAdd_L_Detail;
        }

        private void mBtnM_OldDispetcher_Click(object sender, EventArgs e)
        {
            myTabC_Settings.SelectedTab = tabPage_OldDispetcher;
        }


        #region Добавить ЩЦM папки из ПО "Лоцман"
        private void btn_InsertFolder_Click(object sender, EventArgs e)
        {
            long IdLoodsman = 0;
            if (tB_IdLdsman.Text.Trim() == "") MessageBox.Show("Не указан Id Loodsman.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                if (!Int64.TryParse(tB_IdLdsman.Text.Trim(), out IdLoodsman)) MessageBox.Show("Указан некорректный Id Loodsman.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                if (tB_SHCMFolder.Text.Trim() == "") MessageBox.Show("Не указан ЩЦМ.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    if (C_Orders.Check_IdLoodsman(IdLoodsman)) MessageBox.Show("Папка с таким Id Loodsman уже зарегистрирована.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                if (C_Orders.Check_ShcmDetail(tB_SHCMFolder.Text.Trim())) MessageBox.Show("Папка с таким ЩЦМ уже зарегистрирована.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    if (tB_NameFolder.Text.Trim() == "") MessageBox.Show("Не указано наименование папки.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        if (C_UpdaterSP.InsertFolderInSp_Details(IdLoodsman,tB_SHCMFolder.Text.Trim(), tB_NameFolder.Text.Trim()))
                        {
                            MessageBox.Show("Запись прошла успешно.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            tB_SHCMFolder.Text = ""; tB_NameFolder.Text = ""; tB_IdLdsman.Text = "";
                        }
        }

        private void tB_IdLdsman_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }
        #endregion

        #region OldDispetcher

        private void btn_DetailsData_Click(object sender, EventArgs e)
        {
            if (tB_ShCM.Text.Trim().Length == 0) MessageBox.Show("Заполните поле \"ЩЦМ детали\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                C_Gper.con.ConnectionString = C_Gper.ConStr_Loodsman;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 100;
                /*cmd.CommandText = "select v.Product, a.value, v.dateofCreate,v.id,v.idtype from rvwVersions v " +
                                  "inner join rvwAttributes a on a.idversion=v.id " +
                                  "where a.idattr=235 and v.idtype in (232,233) and v.product like '" + tB_ShCM.Text.Trim() + "' and  v.idstate in (36,40)";
                //"where a.idattr=235 and v.idtype in (232,233,278) and v.product like '" + tB_ShCM.Text.Trim() + "' and v.idstate=40 order by idstate desc"; //235 - Название ЩЦМ idstate(статус)=36-Проектирование,40-Утвержден*/

                cmd.CommandText = "select v.Product, a.value, v.dateofCreate,v.id,v.idtype from rvwVersions v " +
                  "inner join rvwAttributes a on a.idversion=v.id " +
                  //"where a.idattr=235 and v.idtype in (232,233) and v.product like '" + tB_ShCM.Text.Trim() + "' and  v.idstate in (36,40)";
                  "where v.product like '" + tB_ShCM.Text.Trim() + "'"; //235 - Название ЩЦМ idstate(статус)=36-Проектирование,40-Утвержден

                cmd.Connection = C_Gper.con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "DetailInfo");
                adapter.Dispose();
                C_Gper.con.Close();
                dGV_Loodsman.DataSource = dataSet.Tables["DetailInfo"];
            }
        }

        private void btn_UpdatePosL_Click(object sender, EventArgs e)
        {
            //Обновить posL таблицы Relations
            try
            {
                DataSet Ds_oldDisp = new DataSet();
                Ds_oldDisp.Tables.Add("Relations");
                if (tB_posL_NumZakaz.Text.Trim().Length == 0) MessageBox.Show("Заполните поле \"Номер заказа:\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                int Rel_id = -1;
                int Rel_position = -1;
                int Rel_offset = 0;//Поправка в случае если position начинается не с 0
                //Загрузка данных выбранного заказа
                //***************************************************************
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher;
                //***************************************************************
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
                cmd.CommandText = "Select r.id,r.idRelParent,r.position" + "\n" +
                "FROM Relations r " + "\n" +
                "inner join Zakaz z on r.idZakaz = z.id " + "\n" +
                "where z.Dogovor = @Dogovor";
                cmd.Parameters.Add(new SqlParameter("@Dogovor", SqlDbType.VarChar));
                cmd.Parameters["@Dogovor"].Value = tB_posL_NumZakaz.Text.Trim();
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(Ds_oldDisp, "Relations");
                C_Gper.con.Close();
                //для Update в цикле For
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@Rel_id", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@Rel_position", SqlDbType.Int));
                //----------------------------------------------------------------------------
                //Делаем Update где r.id=Rel_id значение поля posL изменяем на значение r.position(т.е. на переменную Rel_position)
                //for (int NumRow = 0; NumRow <= Ds_oldDisp.Tables[0].Rows.Count - 1; NumRow++)
                for (int NumRow = 0; NumRow <= Ds_oldDisp.Tables["Relations"].Rows.Count - 1; NumRow++)
                {
                    Rel_id = (int)Ds_oldDisp.Tables["Relations"].Rows[NumRow]["id"];
                    Rel_position = (int)Ds_oldDisp.Tables["Relations"].Rows[NumRow]["position"];
                    if (NumRow == 0)
                        if ((int)Ds_oldDisp.Tables["Relations"].Rows[NumRow]["idRelParent"] == 0)
                            if (Rel_position > 0) Rel_offset = Rel_position;
                            //if (Rel_position > 1) Rel_offset = -1 * (1 - Rel_position);
                    Rel_position -= Rel_offset;
                    //MessageBox.Show(Rel_id.ToString() + " || " + C_Gper.Ds_oldDisp.Tables["Relations"].Rows[NumRow]["idRelParent"].ToString() + " || " + Rel_position.ToString());
                    //-----------------------------------------------------------------------------
                    cmd.CommandText = "Update Relations set posL=@Rel_position where id=@Rel_id";
                    cmd.Parameters["@Rel_id"].Value = Rel_id;
                    if (Rel_position == 0) cmd.Parameters["@Rel_position"].Value = DBNull.Value;
                    else cmd.Parameters["@Rel_position"].Value = Rel_position;
                    //cmd.Parameters["@Rel_position"].Value = DBNull.Value;
                    C_Gper.con.Open();
                    cmd.ExecuteNonQuery();
                    C_Gper.con.Close();
                }
                MessageBox.Show("Завершено успешно. ", "Ура!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_UpdateAllDataDetails_Click(object sender, EventArgs e) //Обновляем нашу БД (наименование деталей и их технологию)
        {
            WorkWithDatabase WoWiDb = new WorkWithDatabase(C_Gper.ConnStrDispetcher);
            //string SHCM_detail_updating = "ЩЦМ 8.367.169";
            string SHCM_detail_updating = tB_ShCM_forUpdAllData.Text.Trim();
            string str = "select  d.id, d.idLoodsman,d.Number from Details d order by d.id";
            var dt = WoWiDb.ExecuteQueryInTable(str);
            DataTable dtH = WoWiDb.ExecuteQueryInTable(str);
            try
            {
                //Обновляем информацию по деталям

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //Олег if (dt.Rows[i].ItemArray[2].ToString().Contains("8.637.419"))
                    if (dt.Rows[i].ItemArray[2].ToString().Contains(SHCM_detail_updating))
                    {
                        dtH.Rows.Clear();
                        //Получаем информацию из Лоцмана
                        str = "select v.Product, a.value, v.dateofCreate,v.id,v.idtype from [НИИПМ].[dbo].rvwVersions v " +
                                   "inner join [НИИПМ].[dbo].rvwAttributes a on a.idversion=v.id " +
                            //Олег "where a.idattr=235 and v.idtype in (232,233,278) and v.product like '" + dt.Rows[i].ItemArray[2] + "' and v.idstate=40 order by idstate desc"; //235 - Название ЩЦМ idstate -статус Утвержден
                                        "where a.idattr=235 and v.idtype in (232,233) and v.product like '" + dt.Rows[i].ItemArray[2] + "' and  (v.idstate=36 or v.idstate=40)  order by idstate desc"; //235 - Название ЩЦМ idstate(статус)=36-Проектирование,40-Утвержден
                        dtH = WoWiDb.ExecuteQueryInTable(str);
                        //Обновляем 
                        if (dtH.Rows.Count == 1)// && ((int)dt.Rows[i].ItemArray[1]==324016))
                        {
                            str = "update [Dispetcher].[dbo].Details set Number='" + dtH.Rows[0].ItemArray[0] + "',Name='" + dtH.Rows[0].ItemArray[1] +
                                     "', dateCreate='" + ((DateTime)dtH.Rows[0].ItemArray[2]).ToString("d") + "', idLoodsman=" + dtH.Rows[0].ItemArray[3] + ",idtype=" + dtH.Rows[0].ItemArray[4] + " where id=" + dt.Rows[i].ItemArray[0];
                            WoWiDb.ExecuteQuery(str);
                        }
                    }
                }

                //Обновляем информацию по тех. процессам
                string strH = "";
                //lblRefresh.Text = "Технологии";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtH.Rows.Clear();
                    if (dt.Rows[i].ItemArray[2].ToString().Contains(SHCM_detail_updating))
                    {
                        str = "select * from [Dispetcher].[dbo].FactOperation fo where idOperation in(select id from [Dispetcher].[dbo].Operations op where op.idDetail=" + dt.Rows[i].ItemArray[0] + ") " +
                                   " and dateFact is not null";
                        //Если таблицу фактов еще не начали заполнять
                        if (WoWiDb.ExecuteQueryInTable(str).Rows.Count == 0)
                        {
                            //MessageBox.Show("dt.Rows[i].ItemArray[2]");
                            //string str1 = "select top 1 rs.product from [НИИПМ].[dbo].rvwVersions rs where rs.idtype=199  and rs.product like '%" + strShcm + "%' order by dateofcreate desc";
                            strH = "select top 1 r.idParent from [НИИПМ].[dbo].rvwVersions rs " +
                                           "inner join [НИИПМ].[dbo].rvwRelations r on r.idChild=rs.id " +
                                               "where rs.idtype=199 and rs.product like '%" + dt.Rows[i].ItemArray[2] + "%' order by dateofcreate desc";
                            //strH = "select a.idversion,v.product from [НИИПМ].[dbo].rvwAttributes a " +
                            //                            "inner join [НИИПМ].[dbo].rvwVersions v on v.id=a.idversion " +
                            //                                "where a.idattr=235 " + //Наименование операции
                            //                                    "and v.id in (select r.idchild from [НИИПМ].[dbo].rvwRelations r where idlinktype=32 and " +
                            //                                        "r.idParent =(select top 1 r.idParent from [НИИПМ].[dbo].rvwVersions rs " +
                            //                                            "inner join [НИИПМ].[dbo].rvwRelations r on r.idChild=rs.id " +
                            //                                                "where rs.idtype=199 and rs.product like '%" + dt.Rows[i].ItemArray[2] + "%' order by dateofcreate desc) order by Cast(Left(v.product,3) as int)";//Техоперация
                            dtH = WoWiDb.ExecuteQueryInTable(strH);
                            if (dtH.Rows.Count > 0) //Если есть какие-то операции
                            {
                                int idCh = (int)dtH.Rows[0].ItemArray[0];
                                dtH.Rows.Clear();
                                strH = "select a.idversion,v.product from [НИИПМ].[dbo].rvwAttributes a " +
                                                    "inner join [НИИПМ].[dbo].rvwVersions v on v.id=a.idversion " +
                                                        "where a.idattr=235 " + //Наименование операции
                                                            "and v.id in (select r.idchild from [НИИПМ].[dbo].rvwRelations r where idlinktype=32 and " +
                                                                "r.idParent =" + idCh + ") order by Cast(Left(v.product,3) as int)";//Техоперация
                                dtH = WoWiDb.ExecuteQueryInTable(strH);

                                //Удаляем все из нашей БД
                                int[,] ArrOpertation = new int[100, 3];
                                str = "delete from [Dispetcher].[dbo].FactOperation where idOperation in(select id from [Dispetcher].[dbo].Operations op where op.idDetail=" + dt.Rows[i].ItemArray[0] + ") " +
                                        " delete from [Dispetcher].[dbo].Operations where idDetail=" + dt.Rows[i].ItemArray[0];
                                WoWiDb.ExecuteQuery(str);
                                var dtOp = new DataTable();

                                for (int j = 0; j < dtH.Rows.Count; j++)//Олег здесь пишем тех процесс
                                {
                                    //Из-за сортировки 0-Время на деталь (Tsh 195), 1-Наименование (Name 235), 2-Подготовительное время (Tpd 321)
                                    str = "select a.value from [НИИПМ].[dbo].rvwAttributes a where a.idversion=" + dtH.Rows[j].ItemArray[0] + " and a.idattr in (195,235,321) order by a.idattr";
                                    dtOp.Clear();
                                    dtOp = WoWiDb.ExecuteQueryInTable(str);

                                    str = "insert into [Dispetcher].[dbo].Operations values (" + dt.Rows[i].ItemArray[0] + ",'" + dtH.Rows[j].ItemArray[1].ToString().Substring(0, 3) + " " + dtOp.Rows[1].ItemArray[0] + "'," + dtOp.Rows[2].ItemArray[0] + "," + dtOp.Rows[0].ItemArray[0] + ")";
                                    WoWiDb.ExecuteQuery(str);
                                }
                                if (dtH.Rows.Count > 0)//Если есть какие-то операции все-таки
                                {
                                    str = "insert into [Dispetcher].[dbo].Operations values (" + dt.Rows[i].ItemArray[0] + ",'Передача детали на СГД',0,0)";
                                    WoWiDb.ExecuteQuery(str);
                                }

                                //Получаем все родительские связи
                                str = "select id from Dispetcher.dbo.Relations r where r.idChild=" + dt.Rows[i].ItemArray[0] +
                                        " and r.id not in (select idRelations from Dispetcher.dbo.FactOperation where idOperation in(select id from [Dispetcher].[dbo].Operations op where op.idDetail=" + dt.Rows[i].ItemArray[0] + "))";
                                var dtRel = WoWiDb.ExecuteQueryInTable(str);

                                //Операции в технологии
                                dtOp.Clear();
                                str = "select o.id from Dispetcher.dbo.Operations o where idDetail=" + dt.Rows[i].ItemArray[0];
                                dtOp = WoWiDb.ExecuteQueryInTable(str);

                                for (int k = 0; k < dtOp.Rows.Count; k++) //Перебираем все операции
                                    for (int j = 0; j < dtRel.Rows.Count; j++)
                                        WoWiDb.ExecuteQuery("insert into Dispetcher.dbo.FactOperation values (" + dtRel.Rows[j].ItemArray[0] + "," + dtOp.Rows[k].ItemArray[0] + ",null,null,null,null,null,null,null,null,null,null,null)");
                                MessageBox.Show("Обновление успешно завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void FindDoublePositionsFromRelations()//Тестовая - пока не нужна
        {
            try
            {
                int ffff = -1; int nur = 0;
                int Rel_position = -1;
                //Загрузка данных выбранной записи
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.Parameters.Clear();       //0
                cmd.CommandText = "SELECT [id],[position],[idZakaz] " + "\n" +
                "FROM [Dispetcher].[dbo].[Relations] " + "\n" +
                "order by idZakaz,position ";
                cmd.Connection = C_Gper.con;
                C_Gper.con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ++nur;
                        if (reader.IsDBNull(0) == false) ffff = reader.GetInt32(0);
                        if (reader.IsDBNull(1) == false)
                        {
                            if (Rel_position == reader.GetInt32(1))
                                MessageBox.Show(nur.ToString() + "||" + ffff.ToString() + " || " + Rel_position.ToString());
                            else Rel_position = reader.GetInt32(1);
                        }
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
        #endregion

        #region UpdatesSp_Details (Auto load data from Loodsman in Dispetcher2)
        DataTable Dt_Sp = new DataTable();
        C_UpdaterSP updater = new C_UpdaterSP();

        private void tB_IdLoodsman_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void btn_LoadSpDetails_Click(object sender, EventArgs e)
        {
            int idloodsman = 0;
            if (tB_IdLoodsman.Text.Trim() != "") int.TryParse(tB_IdLoodsman.Text.Trim(), out idloodsman);
            updater.SelectNewDataSp("Sp_Details", ref Dt_Sp, ref dGV_SpDetails, idloodsman);
            lbl_RowsCount.Visible = true;
            lbl_RowsCount.Text = "Найдено новых деталей в ПО \"ЛОЦМАН\": " + Dt_Sp.Rows.Count;
            if (Dt_Sp.Rows.Count > 0) btn_LoadDataInSp.Enabled = true; else btn_LoadDataInSp.Enabled = false;
        }

        private void btn_LoadDataInSp_Click(object sender, EventArgs e)
        {
            try
            {
                updater.InsertDataInSp("Sp_Details", ref Dt_Sp);
                lbl_RowsCount.Visible = false;
            }
            finally{btn_LoadDataInSp.Enabled = false;}
        }

        DataTable DT_Details = new DataTable();
        DataTable DT_Tehnology = new DataTable();

        private void btn_TehnologyUpdate_Click(object sender, EventArgs e)
        {
            //1.производим поиск деталей в работе
            C_UpdaterSP.SelectDetailsInWork(ref DT_Details);
            if (DT_Details.Rows.Count > 0)
            {
                bool err = false;
                string NameOper = "";
                string NumOper = "";
                Int16 FK_IdOperation = 0;
                //**************************************************************************
                Int64 FK_IdDetails = 0;
                Int64 IdLoodsman = 0;
                string Shcm = "";
                foreach (DataRow row in DT_Details.Rows)
                {
                    C_Details Detail = new C_Details(Convert.ToInt64(row["IdLoodsman"]));
                    Detail.GetTehnologyFromLoodsman(ref DT_Tehnology);
                    FK_IdDetails = Convert.ToInt64(row["PK_IdDetail"]);
                    IdLoodsman = Convert.ToInt64(row["IdLoodsman"]);
                    Shcm = row["ShcmDetail"].ToString();
                    //if (FK_IdDetails !=23333) //это 8.816.092 там 2 одинакове операции, из-за чего вываливается в ошибку
                    if (DT_Tehnology.Rows.Count > 0)
                    {
                        //Удаляем технологию для конкретной детали из справочника Sp_TechnologyDetails
                        C_UpdaterSP.DeleteTechnologyDetails(FK_IdDetails);
                        //Производим запись каждой операции в справочник Sp_TechnologyDetails
                        foreach (DataRow row_teh in DT_Tehnology.Rows)
                        {
                            NameOper = row_teh["Oper"].ToString();
                            NumOper = NameOper.Remove(3);
                            NameOper = NameOper.Remove(0, NameOper.IndexOf(' ', 2) + 1);
                            FK_IdOperation = C_Details.Find_FK_IdOperationInSp_Operations(NameOper);
                            if (FK_IdOperation == 0)
                            {
                                err = true;
                                MessageBox.Show("ID Детали:\"" + FK_IdDetails.ToString() + "; Операция:\"" + NameOper + "\" не найдена в справочнике операций ПО \"Диспетчеризация\".", "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else//Saving
                            {
                                int Tpd2 = Convert.ToInt32(row_teh["Tpd"] == DBNull.Value ? 0 : int.TryParse(row_teh["Tpd"].ToString(), out var number) == true ? Convert.ToInt32(row_teh["Tpd"]) : 0);
                                int Tsh2 = Convert.ToInt32(row_teh["Tsh"] == DBNull.Value ? 0 : int.TryParse(row_teh["Tsh"].ToString(), out var number2) == true ? Convert.ToInt32(row_teh["Tsh"]) : 0);
                                if (!C_UpdaterSP.InsertTechnologyDetails(FK_IdDetails, NumOper, FK_IdOperation, Tpd2, Tsh2))
                                {
                                    err = true;
                                    MessageBox.Show("ID Детали:\"" + FK_IdDetails.ToString() + "\",IdLoodsman: \"" + IdLoodsman + "\", \"" + Shcm +
                                        "\", операция:\"" + NameOper + "\" не найдена в справочнике операций ПО \"Диспетчеризация\".", "ОШИБКА сохранения!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                if (!err) MessageBox.Show("Технологии всех деталей находящихся в работе успешно обновлены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else MessageBox.Show("Обновление завершено с ошибками.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        #endregion

        #region Табель УРВ + Заказы

        private void mBtnM_IP_Click(object sender, EventArgs e)
        {
            myTabC_Settings.SelectedTab = tabPage_TURV;
            numUD_year.Value = DateTime.Now.Year;
            numUD_year2.Value = DateTime.Now.Year;
            numUD_year3.Value = DateTime.Now.Year;
            //******************
            _Dt_Worker.Clear();
            C_DataBase DB = new C_DataBase(C_Gper.ConnStrDispetcher2);
            string sql = "Select PK_Login From Users Where OnlyUser = 0" + "\n" +
                "Order by PK_Login";
            DB.Select_DT(ref _Dt_Worker, sql);
            //****************
            cB_WorkersTURV.DataSource = _Dt_Worker;
            cB_WorkersTURV.DisplayMember = "PK_Login";
            cB_WorkersTURV.ValueMember = "PK_Login";
            cB_WorkersTURV2.DataSource = _Dt_Worker;
            cB_WorkersTURV2.DisplayMember = "PK_Login";
            cB_WorkersTURV2.ValueMember = "PK_Login";
        }

        //***********ДОБАВИТЬ СОТРУДНИКА В ТУРВ******************
        DataTable _Dt_Worker = new DataTable();

        private void btn_AddWorker_Click(object sender, EventArgs e)
        {
            int NumMonth = cB_MonthTURV2.SelectedIndex + 1;
            int NumYear = (int)numUD_year2.Value;
            if (NumMonth == 0) MessageBox.Show("Не указан месяц.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                DateTime DateT = Convert.ToDateTime("01." + NumMonth + "." + NumYear);

                C_TimeSheetsV1 TSHV1 = new C_TimeSheetsV1(NumMonth, NumYear) { LoginUs = cB_WorkersTURV.SelectedValue.ToString(), Val_Time = "8" };
                DateTime DateLast;
                if (NumMonth == 12) DateLast = Convert.ToDateTime("31.12." + NumYear);
                else DateLast = Convert.ToDateTime("01." + (NumMonth + 1) + "." + NumYear);
                while (DateT < DateLast)
                {
                    if (DateT.DayOfWeek.ToString() == "Saturday" || DateT.DayOfWeek.ToString() == "Sunday")
                        TSHV1.Val_Time = "В";
                    else TSHV1.Val_Time = "8";
                    TSHV1.PK_Date = DateT;
                    TSHV1.InsertData();
                    DateT = DateT.AddDays(1);
                }
                if (!TSHV1.Err) MessageBox.Show("Сотрудник добавлен.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //***********УДАЛИТЬ СОТРУДНИКА ИЗ ТУРВ******************
        private void btn_DeleteWorker_Click(object sender, EventArgs e)
        {
            int NumMonth = cB_MonthTURV3.SelectedIndex + 1;
            int NumYear = (int)numUD_year3.Value;
            if (NumMonth == 0) MessageBox.Show("Не указан месяц.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //DateTime DateT = Convert.ToDateTime("01." + NumMonth + "." + NumYear);
                //DateTime DateT2 = Convert.ToDateTime("01." + (NumMonth +1) + "." + NumYear);

                C_TimeSheetsV1 TSHV = new C_TimeSheetsV1(NumMonth, NumYear) { LoginUs = cB_WorkersTURV2.SelectedValue.ToString() };
                TSHV.Delete_NoteData();//Удаляем примечание для выбранного сотрудника
                TSHV.DeleteDataLogin();//Удаляем примечание для выбранного сотрудника
                if (!TSHV.Err) MessageBox.Show("Данные сотрудника успешно удалены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //************************
        private void btn_DeleteData_Click(object sender, EventArgs e)//Стереть показания ТУРВ + Примечание за выбранный период для конкретного типа пользователей
        {
            if (cB_MonthTURV.SelectedIndex == -1) MessageBox.Show("Не указан месяц.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Версия 1
                C_TimeSheetsV1 TSHV1 = new C_TimeSheetsV1(cB_MonthTURV.SelectedIndex + 1, (int)numUD_year.Value);
                TSHV1.DeleteData(chB_Fired.Checked);//Удаляем данные табеля
                TSHV1.Delete_NoteDataBefore(chB_Fired.Checked);//Удаляем примечания. НЕ ТРОГАТЬ!!! Это необходимо для очистки таблицы TimeSheetsNote в случае возникновения ошибок при записи в таблицу TimeSheets
                if (!TSHV1.Err) MessageBox.Show("Данные удалены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_DeleteNotes_Click(object sender, EventArgs e)//Стереть примечание за выбранный период для конкретного типа пользователей
        {
            if (cB_MonthTURV.SelectedIndex == -1) MessageBox.Show("Не указан месяц.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Версия 1
                C_TimeSheetsV1 TSHV1 = new C_TimeSheetsV1(cB_MonthTURV.SelectedIndex + 1, (int)numUD_year.Value);
                TSHV1.Delete_NoteDataBefore(chB_Fired.Checked);//Удаляем примечания. НЕ ТРОГАТЬ!!! Это необходимо для очистки таблицы TimeSheetsNote в случае возникновения ошибок при записи в таблицу TimeSheets
                if (!TSHV1.Err) MessageBox.Show("Данные удалены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_MyIP_Click(object sender, EventArgs e)
        {
            string HostName = System.Net.Dns.GetHostName();
            string IP = System.Net.Dns.GetHostEntry(HostName).AddressList[1].ToString();
            //string IP = System.Net.Dns.GetHostByName(HostName).AddressList[0].ToString(); //Устарело
            tB_HostName.Text = HostName;
            tB_IP.Text = IP;
        }

        //***********РАБОТА С ЗАКАЗАМИ******************
        DataTable DT_Orders = new DataTable();
        BindingSource BS_Orders = new BindingSource();

        private void tabPage_TURV_Enter(object sender, EventArgs e)
        {
            C_Orders.SelectOrdersData(2, ref DT_Orders);//2-opened. LoadOrders
        }

        private void tB_OrderNum_TextChanged(object sender, EventArgs e)
        {
            BS_Orders.Filter = " OrderNum like '%" + tB_OrderNum.Text.ToString().Trim() + "%'";
        }

        private void btn_EditOrder_Click(object sender, EventArgs e)
        {
            //if (dGV_Orders.CurrentRow == null)
            //{
            //    MessageBox.Show("Не выбран заказ.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}
            //else
            //{
            //    CurrencyManager cmgrDet = (CurrencyManager)dGV_Orders.BindingContext[dGV_Orders.DataSource, dGV_Orders.DataMember];
            //    DataRow rowOrder = ((DataRowView)cmgrDet.Current).Row;
            //    int PK_IdOrder = Convert.ToInt32(rowOrder["PK_IdOrder"]);
            //    string OrderName = tB_OrderName.Text.Trim();
            //    string OrderNum1C = tB_OrderNum1C.Text.Trim();
            //    DateTime StartDate = dTP_StartOrdDate.Value;
            //    DateTime PlannedDate = dTP_PlannedDate.Value;
            //    Int16 Amount = Convert.ToInt16(numUD_Amount.Value);
            //    if (C_Orders.UpdateOrder(PK_IdOrder, OrderName, OrderNum1C, StartDate, PlannedDate, Amount))
            //        MessageBox.Show("Изменения в заказе успешно сохранены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}

            if (dGV_Orders.CurrentRow == null)
            {
                MessageBox.Show("Не выбран заказ.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                int PK_IdOrder = SelectedOrderId;
                string OrderName = tB_OrderName.Text.Trim();
                string OrderNum1C = tB_OrderNum1C.Text.Trim();
                DateTime StartDate = dTP_StartOrdDate.Value;
                DateTime PlannedDate = dTP_PlannedDate.Value;
                Int16 Amount = Convert.ToInt16(numUD_Amount.Value);
                if (C_Orders.UpdateOrder(PK_IdOrder, OrderName, OrderNum1C, StartDate, PlannedDate, Amount))
                    MessageBox.Show("Изменения в заказе успешно сохранены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



        }









































        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (tB_ShCM2.Text.Trim().Length == 0) MessageBox.Show("Заполните поле \"ЩЦМ детали\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                C_Gper.con.ConnectionString = C_Gper.ConStr_Loodsman;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandTimeout = 100;
                cmd.CommandText = "select v.Product, a.value, v.dateofCreate,v.id,v.idtype from rvwVersions v " +
                                  "inner join rvwAttributes a on a.idversion=v.id " +
                                  "where a.idattr=235 and v.idtype in (232,233) and v.product like '%" + tB_ShCM2.Text.Trim().Replace("ЩЦМ", "").Replace("щцм", "") + "%' and  v.idstate in (36,40,30)";
                //"where a.idattr=235 and v.idtype in (232,233,278) and v.product like '" + tB_ShCM.Text.Trim() + "' and v.idstate=40 order by idstate desc"; //235 - Название ЩЦМ idstate(статус)=36-Проектирование,40-Утвержден


                cmd.Connection = C_Gper.con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet, "DetailInfo");
                adapter.Dispose();
                C_Gper.con.Close();
                dataGridView1.DataSource = dataSet.Tables["DetailInfo"];


            }
        }

        private void dGV_Loodsman_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                tB_IdLoodsman.Text = dataGridView1[3, 0].Value.ToString();
            }
            catch
            {
                MessageBox.Show("Введите ЩЦМ", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string HostName = System.Net.Dns.GetHostName();
            string IP = System.Net.Dns.GetHostEntry(HostName).AddressList[1].ToString();
            //string IP = System.Net.Dns.GetHostByName(HostName).AddressList[0].ToString(); //Устарело
            tB_HostName.Text = HostName;
            tB_IP.Text = IP;
        }

        private void KitUpdaterButton_Click(object sender, EventArgs e)
        {
            KitUpdaterButton.Enabled = false;
            ImportData1CButton.Enabled = false;
            if (kuc == null)
            {
                kuc = new KitUpdaterControl();
                kuc.FinishEvent += OnFinishEvent;
                KitElementHost.Child = kuc;
            }
            kuc.Start();
        }

        private void OnFinishEvent(object sender, EventArgs e)
        {
            // Необработанное исключение типа "System.InvalidOperationException"
            // Недопустимая операция в нескольких потоках: попытка доступа к элементу управления 'KitUpdaterButton' не из того потока, в котором он был создан.
            // KitUpdaterButton.Enabled = true;

            this.BeginInvoke(new MethodInvoker(this.AfterFinish));
        }

        void AfterFinish()
        {
            KitUpdaterButton.Enabled = true;
            ImportData1CButton.Enabled = true;
        }

        private void F_Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (kuc != null)
            {
                kuc.Stop();
                kuc = null;
            }
        }

        private void ImportData1CButton_Click(object sender, EventArgs e)
        {
            KitUpdaterButton.Enabled = false;
            ImportData1CButton.Enabled = false;
            if (idc == null)
            {
                idc = new ImportDataControl();
                idc.FinishEvent += OnFinishEvent;
                KitElementHost.Child = idc;
            }
            
        }
    }
}
