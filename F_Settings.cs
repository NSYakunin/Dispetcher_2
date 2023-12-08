using System;
using System.Collections.Generic;
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
        IConfig config;
        // Внешняя зависимость!
        KitUpdaterControl kuc = null;
        // Внешняя зависимость!
        ImportDataControl idc = null;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Orders orders;

        DataTable Dt_Sp = new DataTable();
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_UpdaterSP updater;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_TimeSheetsV1 TSHV1;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Details Detail;
        public int SelectedOrderId { set; get; }
        public F_Settings(IConfig config, IConverter converter)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр: config");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            this.config = config;
            orders = new C_Orders(config);
            updater = new C_UpdaterSP(config);
            TSHV1 = new C_TimeSheetsV1(config, converter);
            Detail = new C_Details(config);

            InitializeComponent();
            DT_Orders.Columns.Add("PK_IdOrder", typeof(int));
            DT_Orders.Columns.Add("OrderNum", typeof(string));
            DT_Orders.Columns.Add("OrderName", typeof(string));
            DT_Orders.Columns.Add("OrderNum1С", typeof(string));
            DT_Orders.Columns.Add("StartDate", typeof(DateTime));
            DT_Orders.Columns.Add("PlannedDate", typeof(DateTime));
            DT_Orders.Columns.Add("Amount", typeof(Int16));

            
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
                    if (orders.Check_IdLoodsman(IdLoodsman)) MessageBox.Show("Папка с таким Id Loodsman уже зарегистрирована.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                if (orders.Check_ShcmDetail(tB_SHCMFolder.Text.Trim())) MessageBox.Show("Папка с таким ЩЦМ уже зарегистрирована.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    if (tB_NameFolder.Text.Trim() == "") MessageBox.Show("Не указано наименование папки.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                        if (updater.InsertFolderInSp_Details(IdLoodsman,tB_SHCMFolder.Text.Trim(), tB_NameFolder.Text.Trim()))
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
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.LoodsmanConnectionString;
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

                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "DetailInfo");
                    adapter.Dispose();
                    con.Close();
                    dGV_Loodsman.DataSource = dataSet.Tables["DetailInfo"];
                }
            }
        }
        #endregion

        #region UpdatesSp_Details (Auto load data from Loodsman in Dispetcher2)


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
            List<ErrorItem> errors = new List<ErrorItem>();
            int eid = 1;
            //1.производим поиск деталей в работе
            updater.SelectDetailsInWork(ref DT_Details);
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
                    //C_Details Detail = new C_Details(Convert.ToInt64(row["IdLoodsman"]));
                    var id = Convert.ToInt64(row["IdLoodsman"]);
                    Detail.GetTehnologyFromLoodsman(ref DT_Tehnology, id);
                    FK_IdDetails = Convert.ToInt64(row["PK_IdDetail"]);
                    IdLoodsman = Convert.ToInt64(row["IdLoodsman"]);
                    Shcm = row["ShcmDetail"].ToString();
                    //if (FK_IdDetails !=23333) //это 8.816.092 там 2 одинакове операции, из-за чего вываливается в ошибку
                    if (DT_Tehnology.Rows.Count > 0)
                    {
                        //Удаляем технологию для конкретной детали из справочника Sp_TechnologyDetails
                        updater.DeleteTechnologyDetails(FK_IdDetails);
                        //Производим запись каждой операции в справочник Sp_TechnologyDetails
                        foreach (DataRow row_teh in DT_Tehnology.Rows)
                        {
                            NameOper = row_teh["Oper"].ToString();
                            NumOper = NameOper.Remove(3);
                            NameOper = NameOper.Remove(0, NameOper.IndexOf(' ', 2) + 1);
                            FK_IdOperation = Detail.Find_FK_IdOperationInSp_Operations(NameOper);
                            if (FK_IdOperation == 0)
                            {
                                err = true;
                                string error = "ID Детали:\"" + FK_IdDetails.ToString() + "; Операция:\"" + NameOper + "\" не найдена в справочнике операций ПО \"Диспетчеризация\".";
                                //MessageBox.Show(error, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ErrorItem item = new ErrorItem(eid++, error);
                                errors.Add(item);
                            }
                            else//Saving
                            {
                                int Tpd2 = Convert.ToInt32(row_teh["Tpd"] == DBNull.Value ? 0 : int.TryParse(row_teh["Tpd"].ToString(), out var number) == true ? Convert.ToInt32(row_teh["Tpd"]) : 0);
                                int Tsh2 = Convert.ToInt32(row_teh["Tsh"] == DBNull.Value ? 0 : int.TryParse(row_teh["Tsh"].ToString(), out var number2) == true ? Convert.ToInt32(row_teh["Tsh"]) : 0);
                                if (!updater.InsertTechnologyDetails(FK_IdDetails, NumOper, FK_IdOperation, Tpd2, Tsh2))
                                {
                                    err = true;
                                    //MessageBox.Show("ID Детали:\"" + FK_IdDetails.ToString() + "\",IdLoodsman: \"" + IdLoodsman + "\", \"" + Shcm +
                                    //    "\", операция:\"" + NameOper + "\" не найдена в справочнике операций ПО \"Диспетчеризация\".", "ОШИБКА сохранения!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ErrorItem item = new ErrorItem(eid++, updater.LastError);
                                    errors.Add(item);
                                }
                            }
                        }
                    }
                }
                if (err) 
                {
                    MessageBox.Show("Обновление завершено с ошибками.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    dataGridView1.DataSource = errors;
                }
                else
                {
                    MessageBox.Show("Технологии всех деталей находящихся в работе успешно обновлены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                    
                
                    
                
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
            
            string sql = "Select PK_Login From Users Where OnlyUser = 0" + "\n" +
                "Order by PK_Login";
            

            using (var con = new SqlConnection())
            {
                con.ConnectionString = config.ConnectionString;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//using System.Data.SqlClient;
                cmd.CommandText = sql;
                cmd.Connection = con;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);//adapter.SelectCommand = cmd;
                adapter.Fill(_Dt_Worker);
                adapter.Dispose();
            }

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
                
                TSHV1.LoginUs = cB_WorkersTURV.SelectedValue.ToString();
                TSHV1.Val_Time = "8";
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
                TSHV1.LoginUs = cB_WorkersTURV2.SelectedValue.ToString();
                TSHV1.Delete_NoteData(NumMonth, NumYear);//Удаляем примечание для выбранного сотрудника
                TSHV1.DeleteDataLogin(NumMonth, NumYear);//Удаляем примечание для выбранного сотрудника
                if (!TSHV1.Err) MessageBox.Show("Данные сотрудника успешно удалены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //************************
        private void btn_DeleteData_Click(object sender, EventArgs e)//Стереть показания ТУРВ + Примечание за выбранный период для конкретного типа пользователей
        {
            if (cB_MonthTURV.SelectedIndex == -1) MessageBox.Show("Не указан месяц.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Версия 1
                int month = cB_MonthTURV.SelectedIndex + 1;
                int year = (int)numUD_year.Value;
                TSHV1.DeleteData(month, year, chB_Fired.Checked);//Удаляем данные табеля
                TSHV1.Delete_NoteDataBefore(month, year, chB_Fired.Checked);//Удаляем примечания. НЕ ТРОГАТЬ!!! Это необходимо для очистки таблицы TimeSheetsNote в случае возникновения ошибок при записи в таблицу TimeSheets
                if (!TSHV1.Err) MessageBox.Show("Данные удалены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btn_DeleteNotes_Click(object sender, EventArgs e)//Стереть примечание за выбранный период для конкретного типа пользователей
        {
            if (cB_MonthTURV.SelectedIndex == -1) MessageBox.Show("Не указан месяц.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Версия 1
                int month = cB_MonthTURV.SelectedIndex + 1;
                int year = (int)numUD_year.Value;
                TSHV1.Delete_NoteDataBefore(month, year, chB_Fired.Checked);//Удаляем примечания. НЕ ТРОГАТЬ!!! Это необходимо для очистки таблицы TimeSheetsNote в случае возникновения ошибок при записи в таблицу TimeSheets
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
            orders.SelectOrdersData(2, ref DT_Orders);//2-opened. LoadOrders
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
                if (orders.UpdateOrder(PK_IdOrder, OrderName, OrderNum1C, StartDate, PlannedDate, Amount))
                    MessageBox.Show("Изменения в заказе успешно сохранены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            if (tB_ShCM2.Text.Trim().Length == 0) MessageBox.Show("Заполните поле \"ЩЦМ детали\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.LoodsmanConnectionString;
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandTimeout = 100;
                    cmd.CommandText = "select v.Product, a.value, v.dateofCreate,v.id,v.idtype from rvwVersions v " +
                                      "inner join rvwAttributes a on a.idversion=v.id " +
                                      "where a.idattr=235 and v.idtype in (232,233) and v.product like '%" + tB_ShCM2.Text.Trim().Replace("ЩЦМ", "").Replace("щцм", "") + "%' and  v.idstate in (36,40,30)";
                    //"where a.idattr=235 and v.idtype in (232,233,278) and v.product like '" + tB_ShCM.Text.Trim() + "' and v.idstate=40 order by idstate desc"; //235 - Название ЩЦМ idstate(статус)=36-Проектирование,40-Утвержден


                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet, "DetailInfo");
                    adapter.Dispose();
                    
                    dataGridView1.DataSource = dataSet.Tables["DetailInfo"];
                }

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
                kuc = new KitUpdaterControl(config);
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
                idc = new ImportDataControl(config);
                idc.FinishEvent += OnFinishEvent;
                KitElementHost.Child = idc;
            }
            
        }

        private void btn_UpdateAllDataDetails_Click(object sender, EventArgs e)
        {

        }

        private void CheckShcMTbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    using (var con = new SqlConnection())
                    {
                        con.ConnectionString = config.ConnectionString;
                        SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                        cmd.CommandText = $"SELECT TOP 1 [IdLoodsman] FROM [Dispetcher2].[dbo].[Sp_Details] where ShcmDetail = '{CheckShcMTbox.Text}'";
                        cmd.Connection = con;
                        cmd.Connection.Open();
                        object IdLoodsman = cmd.ExecuteScalar();

                        cmd.CommandText = $"SELECT [version] FROM [НИИПМ].[dbo].[rvwVersions] WHERE id = {IdLoodsman}";
                        object actualLoodsmanVersion = cmd.ExecuteScalar();

                        cmd.CommandText = $"SELECT TOP 1 [version] FROM [НИИПМ].[dbo].[rvwVersions] where product = '{CheckShcMTbox.Text}' AND state = 'Утвержден' ORDER BY version DESC";
                        object rigthtLoodsmanVersion = cmd.ExecuteScalar();

                        lBLText.Text = $"Текущая версия {CheckShcMTbox.Text} в Диспетчере - {actualLoodsmanVersion}\nАктуальная же версия в ЛОЦМАН -  {rigthtLoodsmanVersion}";
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
