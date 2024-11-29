using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Xml;
using Dispetcher2.Class;
using Dispetcher2.DialogsForms;
using Microsoft.Office.Interop.Word;


namespace Dispetcher2
{
    public partial class F_Fact : Form
    {
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Details Detail;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Orders orders;
        // Внешняя зависимость! Требуется заменить на шаблон Абстрактная Фабрика
        dF_Brigade dFBrigade;
        // Внешняя зависимость! Требуется заменить на шаблон Абстрактная Фабрика
        dF_Workers dFWorkers;
        // Внешняя зависимость! Требуется заменить на шаблон Абстрактная Фабрика
        F_SearchSHCM searchForm;
        private IConfig config;

        public F_Fact(IConfig config)
        {
            Detail = new C_Details(config);
            orders = new C_Orders(config);
            dFBrigade = new dF_Brigade(config);
            dFWorkers = new dF_Workers(config);
            searchForm = new F_SearchSHCM(config);
            this.config = config;


            InitializeComponent();
            DT_Workers.Columns.Add("PK_Login", typeof(string));
            //*************************************************
            DT_Orders.Columns.Add("PK_IdOrder", typeof(int));
            DT_Orders.Columns.Add("OrderNum", typeof(string));
            DT_Orders.Columns.Add("OrderName", typeof(string));
            //*************************************************
            DT_Details.Columns.Add("Position", typeof(int));
            DT_Details.Columns.Add("ShcmDetail", typeof(string));
            DT_Details.Columns.Add("NameDetail", typeof(string));
            DT_Details.Columns.Add("AmountDetails", typeof(int));
            DT_Details.Columns.Add("NameType", typeof(string));
            DT_Details.Columns.Add("PK_IdOrderDetail", typeof(long));
            DT_Details.Columns.Add("FK_IdDetail", typeof(long));
            DT_Details.Columns.Add("IdLoodsman", typeof(long));
            //*************************************************
            DT_Tehnology.Columns.Add("FK_IdOperation", typeof(Int16));
            DT_Tehnology.Columns.Add("Oper", typeof(string));
            DT_Tehnology.Columns.Add("Tpd", typeof(int));
            DT_Tehnology.Columns.Add("Tsh", typeof(int));
            DT_Tehnology.Columns.Add("IdLoodsman", typeof(long)); // Add this line
            DT_Tehnology.Columns.Add("OTKControl", typeof(CheckBoxState[]));
            DT_Tehnology.Columns.Add("OTKControlData", typeof(OTKControlData));
            DT_Tehnology.Columns.Add("OriginalOTKControlData", typeof(OTKControlData));
            //*************************************************
            DT_FactOper.Columns.Add("LastOper", typeof(bool));
            DT_FactOper.Columns.Add("PK_IdFactOper", typeof(long));
            DT_FactOper.Columns.Add("FactOper", typeof(string));
            DT_FactOper.Columns.Add("DateFactOper", typeof(DateTime));
            DT_FactOper.Columns.Add("FK_LoginWorker", typeof(string));
            DT_FactOper.Columns.Add("AmountDetails", typeof(int));
            DT_FactOper.Columns.Add("FactTpd", typeof(int));
            DT_FactOper.Columns.Add("FactTsh", typeof(int));
            dGV_Tehnology.CellToolTipTextNeeded += DGV_Tehnology_CellToolTipTextNeeded;
        }
        //***************************************************************
        System.Data.DataTable DT_Workers = new System.Data.DataTable();
        System.Data.DataTable DT_Orders = new System.Data.DataTable();
        BindingSource BS_Orders = new BindingSource();
        System.Data.DataTable DT_Details = new System.Data.DataTable();
        BindingSource BS_Details = new BindingSource();
        System.Data.DataTable DT_Tehnology = new System.Data.DataTable();
        System.Data.DataTable DT_FactOper = new System.Data.DataTable();
        BindingSource BS_FactOper = new BindingSource();
        //***************************************************************
        int _PK_IdBrigade = 0;//non target brigade
        string _LoginWorker = "";//non target worker
        //***************************************************************

        private void F_Fact_Load(object sender, EventArgs e)
        {
            dTimeP_Fact.Value = DateTime.Now;
            dGV_Orders.AutoGenerateColumns = false;
            dGV_Orders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Orders.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Orders.DataSource = DT_Orders;
            dGV_Orders.DataSource = BS_Orders;
            dGV_Orders.Columns["Col_OrderNum"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
            //***********************************************************************************************
            dGV_Details.AutoGenerateColumns = false;
            dGV_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Details.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Details.DataSource = DT_Details;
            dGV_Details.DataSource = BS_Details;
            dGV_Details.Columns["Col_Position"].DataPropertyName = DT_Details.Columns["Position"].ToString();
            dGV_Details.Columns["Col_ShcmDetail"].DataPropertyName = DT_Details.Columns["ShcmDetail"].ToString();
            dGV_Details.Columns["Col_NameDetail"].DataPropertyName = DT_Details.Columns["NameDetail"].ToString();
            dGV_Details.Columns["Col_Amount"].DataPropertyName = DT_Details.Columns["AmountDetails"].ToString();
            dGV_Details.Columns["Col_NameType"].DataPropertyName = DT_Details.Columns["NameType"].ToString();
            //***********************************************************************************************
            dGV_Tehnology.AutoGenerateColumns = false;
            dGV_Tehnology.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Tehnology.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            dGV_Tehnology.DataSource = DT_Tehnology;
            dGV_Tehnology.Columns["Col_Oper"].DataPropertyName = DT_Tehnology.Columns["Oper"].ToString();
            dGV_Tehnology.Columns["Col_Tpd"].DataPropertyName = DT_Tehnology.Columns["Tpd"].ToString();
            dGV_Tehnology.Columns["Col_Tsh"].DataPropertyName = DT_Tehnology.Columns["Tsh"].ToString();
            //DT_Tehnology.Columns.Add("OTKControl", typeof(CheckBoxState[]));
            //***********************************************************************************************
            dGV_FactOperation.AutoGenerateColumns = false;
            dGV_FactOperation.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_FactOperation.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_FactOper.DataSource = DT_FactOper;
            dGV_FactOperation.DataSource = BS_FactOper;
            dGV_FactOperation.Columns["Col_FactOper"].DataPropertyName = DT_FactOper.Columns["FactOper"].ToString();
            dGV_FactOperation.Columns["Col_DateFactOper"].DataPropertyName = DT_FactOper.Columns["DateFactOper"].ToString();
            dGV_FactOperation.Columns["Col_FK_LoginWorker"].DataPropertyName = DT_FactOper.Columns["FK_LoginWorker"].ToString();
            dGV_FactOperation.Columns["Col_AmountDetails"].DataPropertyName = DT_FactOper.Columns["AmountDetails"].ToString();
            dGV_FactOperation.Columns["Col_FactTpd"].DataPropertyName = DT_FactOper.Columns["FactTpd"].ToString();
            dGV_FactOperation.Columns["Col_FactTsh"].DataPropertyName = DT_FactOper.Columns["FactTsh"].ToString();
            dGV_FactOperation.Columns["Col_DateFactOper"].Visible = false;
            dGV_FactOperation.Columns["Col_FK_LoginWorker"].Visible = false;
            //Bindings
            tB_OrderName.DataBindings.Add("Text", BS_Orders, "OrderName", false, DataSourceUpdateMode.OnPropertyChanged);
            tB_OrderNumInfo.DataBindings.Add("Text", BS_Orders, "OrderNum", false, DataSourceUpdateMode.OnPropertyChanged);

            //*****************
            cB_InDetail.Checked = true;
            if (Environment.UserName == "NSYakunin" || Environment.UserName == "IAPotapov") this.btnkoop.Visible = true;
            else this.btnkoop.Visible = false;

            // При создании колонки
            DataGridViewOTKControlColumn colOTKControl = new DataGridViewOTKControlColumn();
            colOTKControl.Name = "Col_OTKControl";
            colOTKControl.HeaderText = "OTK Control";
            colOTKControl.DataPropertyName = "OTKControlData"; // Указываем правильное имя столбца
            dGV_Tehnology.Columns.Add(colOTKControl);


            dGV_Tehnology.EditMode = DataGridViewEditMode.EditOnEnter;
            dGV_Tehnology.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            DataGridViewTextBoxColumn idLoodsmanColumn = new DataGridViewTextBoxColumn();
            idLoodsmanColumn.Name = "Col_IdLoodsman";
            idLoodsmanColumn.HeaderText = "IdLoodsman";
            idLoodsmanColumn.DataPropertyName = "IdLoodsman";
            idLoodsmanColumn.Visible = false;
            dGV_Tehnology.Columns.Add(idLoodsmanColumn);
            dGV_Tehnology.CellToolTipTextNeeded += DGV_Tehnology_CellToolTipTextNeeded;
            dGV_Tehnology.ShowCellToolTips = true;
        }

        private void F_Fact_Enter(object sender, EventArgs e)
        {
            orders.SelectOrdersData(2, ref DT_Orders);//2-opened

        }

        private void tB_OrderNum_TextChanged(object sender, EventArgs e)
        {
            DT_Details.Clear(); DT_Tehnology.Clear(); DT_FactOper.Clear();
            tB_ShcmDetail.Text = "";
            BS_Orders.Filter = " OrderNum like '%" + tB_OrderNum.Text.ToString().Trim() + "%'";
        }

        private void tB_ShcmDetail_TextChanged(object sender, EventArgs e)
        {
            DT_Details.Clear(); DT_Tehnology.Clear(); DT_FactOper.Clear();
        }

        private void tB_ShcmDetail_Click(object sender, EventArgs e)
        {
            tB_ShcmDetail.Select(0, tB_ShcmDetail.Text.ToString().Length);
        }

        private void tB_ShcmDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',') e.KeyChar = '.';
        }

        private void tB_OrderNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & dGV_Orders.Rows.Count>0)
            {
                tB_ShcmDetail.Focus();
                tB_ShcmDetail.Select(0, tB_ShcmDetail.Text.ToString().Length);
            }
        }
        private void dGV_Orders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                tB_ShcmDetail.Focus();
            }

        }

        private void dGV_Orders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            tB_ShcmDetail.Focus();
        }

        private void tB_ShcmDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & tB_OrderNumInfo.Text.Trim() != "" & tB_ShcmDetail.Text.Trim().Length > 0 & dGV_Orders.CurrentRow != null)
            {
                CurrencyManager cmgr = (CurrencyManager)dGV_Orders.BindingContext[dGV_Orders.DataSource, dGV_Orders.DataMember];
                DataRow row = ((DataRowView)cmgr.Current).Row;
                int FK_IdOrder = Convert.ToInt32(row["PK_IdOrder"]);
                Detail.SelectAllDetailsLikeSHCM(FK_IdOrder, tB_ShcmDetail.Text.Trim(), ref DT_Details);//Load Details
                if (dGV_Details.Rows.Count > 0) dGV_Details.Select();
            }
        }

        private void dGV_Details_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (dGV_Details.CurrentRow != null)
                {
                    //CurrencyManager cmgr = (CurrencyManager)dGV_Details.BindingContext[dGV_Details.DataSource, dGV_Details.DataMember];
                    //DataRow row = ((DataRowView)cmgr.Current).Row;
                    DataRowView rowView = dGV_Details.CurrentRow.DataBoundItem as DataRowView;
                    if (rowView != null)
                    {
                        DataRow row = rowView.Row;
                        long PK_IdOrderDetail = Convert.ToInt64(row["PK_IdOrderDetail"]);
                        if (row["IdLoodsman"] == DBNull.Value) //inside order
                        {
                            Detail.SelectTehnologyForType111(Convert.ToInt64(row["FK_IdDetail"]), ref DT_Tehnology);
                        }

                        //********************************************************************************************************************************
                        else//Get technology from loodsman
                        {


                            long IdLoodsman = Convert.ToInt64(row["IdLoodsman"]);

                            //string rowData = string.Join(Environment.NewLine, row.Table.Columns.Cast<DataColumn>().Select(c => $"{c.ColumnName}: {row[c]}"));
                            //MessageBox.Show(rowData);

                            Detail.GetTehnologyFromLoodsmanFordGV_Tehnology(ref DT_Tehnology, ref dGV_Tehnology, IdLoodsman, PK_IdOrderDetail);
                        }

                        //*********************************************************************************************************************************
       
                        if (cB_InDetail.Checked)
                        {
                            dGV_FactOperation.Columns["Col_DateFactOper"].Visible = true;
                            dGV_FactOperation.Columns["Col_FK_LoginWorker"].Visible = true;
                            Detail.SelectFullFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
                        }
                        else
                        {
                            dGV_FactOperation.Columns["Col_DateFactOper"].Visible = false;
                            dGV_FactOperation.Columns["Col_FK_LoginWorker"].Visible = false;
                            Detail.SelectFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
                        }
                    } 
                }
            }
        }

        private void chB_cooperation_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_cooperation.Checked)
            {
                _PK_IdBrigade = 0;
                _LoginWorker = "кооп";
                tB_Workers.Text = "кооп";
                
            }
            else
            {
                _PK_IdBrigade = 0;
                _LoginWorker = "";
                tB_Workers.Text = "";
            }
        }


        private void mBtnM_Worker_Click(object sender, EventArgs e)
        {
            chB_cooperation.Checked = false;
            _PK_IdBrigade = 0;
            _LoginWorker = "";
            tB_Workers.Text = "";
            
            dFWorkers.ShowDialog();
            _LoginWorker = dFWorkers.Get_PK_Login;
            tB_Workers.Text = _LoginWorker;
        }

        private void mBtnM_Brigade_Click(object sender, EventArgs e)
        {
            chB_cooperation.Checked = false;
            _PK_IdBrigade = 0;
            _LoginWorker = "";
            tB_Workers.Text = "";
            
            dFBrigade.ShowDialog();
            _PK_IdBrigade = dFBrigade.Get_IDBrigade;
            tB_Workers.Text = dFBrigade.Get_FullNameBrigade;
        }

        private void btn_SaveTehnology_Click(object sender, EventArgs e)
        {
            if (C_Gper.F_Fact_View) MessageBox.Show("Разрешение только на просмотр.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (dGV_Tehnology.CurrentRow == null) MessageBox.Show("Не выбрана операция.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            if (_PK_IdBrigade == 0 & _LoginWorker == "")
            {
                MessageBox.Show("Не указан исполнитель операции.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dGV_Tehnology.CurrentRow.Cells[0].Value.ToString().Trim() != "Передача детали на СГД" &
                Convert.ToInt32(dGV_Tehnology.CurrentRow.Cells[2].Value == DBNull.Value ? "0" : dGV_Tehnology.CurrentRow.Cells[2].Value.ToString()) == 0)
            {
                DialogResult result = MessageBox.Show(
                            "Операция не пронормированна. Закрыть со значением '0' ?",
                            "Внимание",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No) return;
            }

            CurrencyManager cmgr = (CurrencyManager)dGV_Tehnology.BindingContext[dGV_Tehnology.DataSource, dGV_Tehnology.DataMember];
            DataRow row = ((DataRowView)cmgr.Current).Row;
            string NameOper = row["Oper"].ToString();
            string NumOper = "";
            Int16 FK_IdOperation = 0;
            if (row["Oper"].ToString() != "Передача детали на СГД")
            {
                NumOper = NameOper.Remove(3);
                NameOper = NameOper.Remove(0, NameOper.IndexOf(' ', 2) + 1);
            }
            if (row["FK_IdOperation"].ToString() != "") FK_IdOperation = Convert.ToInt16(row["FK_IdOperation"]);
            else FK_IdOperation = Detail.Find_FK_IdOperationInSp_Operations(NameOper);
            //*************************
            if (FK_IdOperation == 0) MessageBox.Show("Операция не найдена в справочнике операций ПО \"Диспетчеризация\".", "Сохранение отменено!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            else//Saving
            {
                CurrencyManager cmgrDet = (CurrencyManager)dGV_Details.BindingContext[dGV_Details.DataSource, dGV_Details.DataMember];
                DataRow rowDet = ((DataRowView)cmgrDet.Current).Row;
                long PK_IdOrderDetail = Convert.ToInt64(rowDet["PK_IdOrderDetail"]);
                //int AmountDetails = Convert.ToInt32(nUpD_Tpd.Value);
                int AmountDetails = Convert.ToInt32(rowDet["AmountDetails"]);
                //***********************cmgr - row*****************************************************************
                int Tpd = row["Tpd"] is DBNull ? 0 : Convert.ToInt32(row["Tpd"]);//seconds
                int Tsh = row["Tsh"] is DBNull ? 0 : Convert.ToInt32(row["Tsh"]);//seconds
                                                                                    //Copmare fact detail amount and order detail amount
                                                                                    //if (AmountDetails < Convert.ToInt32(nUpD_Tpd.Value) + C_Details.Select_AmountFactDetailsOper(PK_IdOrderDetail, FK_IdOperation, NumOper))  //Select Distinct
                if (AmountDetails < Convert.ToInt32(nUpD_Tpd.Value) + Detail.Select_AmountFactDetailsOper(PK_IdOrderDetail, NumOper))  //Select Distinct
                    MessageBox.Show("Превышен лимит на общее количество деталей по данной позиции.", "Сохранение отменено!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else//Insert operation
                {
                    AmountDetails = Convert.ToInt32(nUpD_Tpd.Value);
                    DateTime DateFactOper = dTimeP_Fact.Value;
                    //Пока сообщение убираем
                    //if (C_Details.InsertFactOperation(PK_IdOrderDetail, NumOper, FK_IdOperation, Tpd, Tsh, AmountDetails, DateFactOper, _LoginWorker, _PK_IdBrigade))
                    //MessageBox.Show("Операция сохранена.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Detail.InsertFactOperation(PK_IdOrderDetail, NumOper, FK_IdOperation, Tpd, Tsh, AmountDetails, DateFactOper, _LoginWorker, _PK_IdBrigade);
                    //Refresh DataGrid
                    if (cB_InDetail.Checked)
                    {
                        dGV_FactOperation.Columns["Col_DateFactOper"].Visible = true;
                        dGV_FactOperation.Columns["Col_FK_LoginWorker"].Visible = true;
                        Detail.SelectFullFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
                    }
                    else
                    {
                        dGV_FactOperation.Columns["Col_DateFactOper"].Visible = false;
                        dGV_FactOperation.Columns["Col_FK_LoginWorker"].Visible = false;
                        Detail.SelectFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
                    }
                }
            }
        }                  

        private void cB_InDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (dGV_Details.CurrentRow != null)
            {
                CurrencyManager cmgr = (CurrencyManager)dGV_Details.BindingContext[dGV_Details.DataSource, dGV_Details.DataMember];
                DataRow row = ((DataRowView)cmgr.Current).Row;
                long PK_IdOrderDetail = Convert.ToInt64(row["PK_IdOrderDetail"]);
                if (cB_InDetail.Checked) Detail.SelectFullFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
                else Detail.SelectFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
            }
            if (cB_InDetail.Checked)
            {
                dGV_FactOperation.Columns["Col_DateFactOper"].Visible = true;
                dGV_FactOperation.Columns["Col_FK_LoginWorker"].Visible = true;
            }
            else
            {
                dGV_FactOperation.Columns["Col_DateFactOper"].Visible = false;
                dGV_FactOperation.Columns["Col_FK_LoginWorker"].Visible = false;
            }
        }

        private void dGV_FactOperation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete & cB_InDetail.Checked & dGV_FactOperation.CurrentRow != null & dGV_Details.CurrentRow != null)
            {
                if (C_Gper.F_Fact_View) MessageBox.Show("Разрешение только на просмотр.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    CurrencyManager cmgr = (CurrencyManager)dGV_FactOperation.BindingContext[dGV_FactOperation.DataSource, dGV_FactOperation.DataMember];
                    DataRow row = ((DataRowView)cmgr.Current).Row;
                    if (Detail.DeleteFactOper(Convert.ToInt64(row["PK_IdFactOper"])))
                    {
                        CurrencyManager cmgrDet = (CurrencyManager)dGV_Details.BindingContext[dGV_Details.DataSource, dGV_Details.DataMember];
                        DataRow rowDet = ((DataRowView)cmgrDet.Current).Row;
                        Detail.SelectFullFactOperForDetail(Convert.ToInt64(rowDet["PK_IdOrderDetail"]), ref DT_FactOper);
                    }
                }
            }
        }

        private void dGV_Details_SelectionChanged(object sender, EventArgs e)
        {
            if (dGV_Tehnology.Rows.Count > 0) DT_Tehnology.Clear();
            if (dGV_FactOperation.Rows.Count > 0) DT_FactOper.Clear();
        }

        private void dGV_Orders_SelectionChanged(object sender, EventArgs e)
        {
            if (dGV_Details.Rows.Count > 0) DT_Details.Clear();
            if (dGV_Tehnology.Rows.Count > 0) DT_Tehnology.Clear();
            if (dGV_FactOperation.Rows.Count > 0) DT_FactOper.Clear();
        }

        private void dGV_Details_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dGV_Details.CurrentRow != null && dGV_Details.CurrentRow.Cells["Col_Amount"].Value != null)
            {
                CurrencyManager cmgr = (CurrencyManager)dGV_Details.BindingContext[dGV_Details.DataSource, dGV_Details.DataMember];
                DataRow row = ((DataRowView)cmgr.Current).Row;
                int AmountDetails = Convert.ToInt32(row["AmountDetails"]);
                nUpD_Tpd.Value = AmountDetails;
            }
            else nUpD_Tpd.Value = 1;
        }

        private void btn_CloseAllWorks_Click(object sender, EventArgs e)
        {
            //Закрыть все работы
            if (C_Gper.F_Fact_View) MessageBox.Show("Разрешение только на просмотр.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            if (dGV_Tehnology.Rows.Count > 0)//если в технологии есть операции
            {
                if (dGV_Tehnology.CurrentRow.Cells[0].Value.ToString().Trim() != "Передача детали на СГД" & int.TryParse(dGV_Tehnology.CurrentRow.Cells[2].Value.ToString(), out int num) == false)
                    MessageBox.Show("Операция не пронормированна.", "Сохранение отменено!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else
                {
                    DialogResult dR = MessageBox.Show("Закрыть все оставшиеся работы по детали " + dTimeP_Fact.Value.ToShortDateString() + " числом?", "Внимание!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dR == DialogResult.Yes)
                    {
                        for (int i = 0; i < dGV_Tehnology.Rows.Count; i++)
                        {
                            /*DT_Tehnology.Columns.Add("FK_IdOperation", typeof(Int16));
                            DT_Tehnology.Columns.Add("Oper", typeof(string));
                            DT_Tehnology.Columns.Add("Tpd", typeof(int));
                            DT_Tehnology.Columns.Add("Tsh", typeof(int));*/
                            //********************Делаем проверку на наличие 0 операций в технологии***************************************
                            string NameOper = DT_Tehnology.Rows[i].ItemArray[1].ToString().Trim();
                            string NumOper = "";
                            Int16 FK_IdOperation = 0;
                            if (NameOper != "Передача детали на СГД")
                            {
                                NumOper = NameOper.Remove(3);
                                NameOper = NameOper.Remove(0, NameOper.IndexOf(' ', 2) + 1);
                            }
                            if (DT_Tehnology.Rows[i].ItemArray[0].ToString().Trim() != "") FK_IdOperation = Convert.ToInt16(DT_Tehnology.Rows[i].ItemArray[0].ToString().Trim());
                            else FK_IdOperation = Detail.Find_FK_IdOperationInSp_Operations(NameOper);
                            //*************************
                            if (FK_IdOperation == 0) MessageBox.Show("Операция не найдена в справочнике операций ПО \"Диспетчеризация\".", "Сохранение отменено!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            else//Saving
                            {
                                //*************************************************************************
                                CurrencyManager cmgrDet = (CurrencyManager)dGV_Details.BindingContext[dGV_Details.DataSource, dGV_Details.DataMember];
                                DataRow rowDet = ((DataRowView)cmgrDet.Current).Row;
                                long PK_IdOrderDetail = Convert.ToInt64(rowDet["PK_IdOrderDetail"]);
                                //int AmountDetails = Convert.ToInt32(nUpD_Tpd.Value);
                                int AmountDetails = Convert.ToInt32(rowDet["AmountDetails"]);
                                //***********************cmgr - row*****************************************************************

                                int Tpd = Convert.ToInt32(DT_Tehnology.Rows[i].ItemArray[2] == DBNull.Value ? 0 : int.TryParse(DT_Tehnology.Rows[i].ItemArray[2].ToString(), out var number) == true ? Convert.ToInt32(DT_Tehnology.Rows[i].ItemArray[2]) : 0);
                                int Tsh = Convert.ToInt32(DT_Tehnology.Rows[i].ItemArray[3] == DBNull.Value ? 0 : int.TryParse(DT_Tehnology.Rows[i].ItemArray[3].ToString(), out var number2) == true ? Convert.ToInt32(DT_Tehnology.Rows[i].ItemArray[3]) : 0);
                                //Copmare fact detail amount and order detail amount
                                //AmountDetails -= C_Details.Select_AmountFactDetailsOper(PK_IdOrderDetail, FK_IdOperation, NumOper);
                                AmountDetails -= Detail.Select_AmountFactDetailsOper(PK_IdOrderDetail, NumOper);
                                if (AmountDetails > 0)//Insert operation
                                {
                                    DateTime DateFactOper = dTimeP_Fact.Value;
                                    //Пока сообщение убираем
                                    //if (C_Details.InsertFactOperation(PK_IdOrderDetail, NumOper, FK_IdOperation, Tpd, Tsh, AmountDetails, DateFactOper, _LoginWorker, _PK_IdBrigade))
                                    //MessageBox.Show("Операция сохранена.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    string FK_LoginWorker = Detail.Find_FK_Login_UsersInSp_Operations(FK_IdOperation);
                                    Detail.InsertFactOperation(PK_IdOrderDetail, NumOper, FK_IdOperation, Tpd, Tsh, AmountDetails, DateFactOper, FK_LoginWorker, 0);
                                    //Refresh DataGrid
                                    if (cB_InDetail.Checked)
                                    {
                                        //dGV_FactOperation.Columns["Col_DateFactOper"].Visible = true;
                                        //dGV_FactOperation.Columns["Col_FK_LoginWorker"].Visible = true;
                                        Detail.SelectFullFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
                                    }
                                    else
                                    {
                                        //dGV_FactOperation.Columns["Col_DateFactOper"].Visible = false;
                                        //dGV_FactOperation.Columns["Col_FK_LoginWorker"].Visible = false;
                                        Detail.SelectFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
                                    }
                                }

                                //*************************************************************************
                            }
                        }
                    }
                }
            }
        }

        private void btn_SearchSHCM_F_Click(object sender, EventArgs e)
        {
            searchForm.ShowDialog();
        }

        private void btnkoop_Click(object sender, EventArgs e)
        {
            //Закрыть все работы на кооп

            if (chB_cooperation.Checked)
            {
                _PK_IdBrigade = 0;
                _LoginWorker = "кооп";
                tB_Workers.Text = "кооп";

                if (C_Gper.F_Fact_View) MessageBox.Show("Разрешение только на просмотр.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    for (int i = 0; i < dGV_Tehnology.Rows.Count; i++)
                    {
                        string NameOper = DT_Tehnology.Rows[i].ItemArray[1].ToString().Trim();
                        string NumOper = "";
                        Int16 FK_IdOperation = 0;
                        if (NameOper != "Передача детали на СГД")
                        {
                            NumOper = NameOper.Remove(3);
                            NameOper = NameOper.Remove(0, NameOper.IndexOf(' ', 2) + 1);
                        }
                        if (DT_Tehnology.Rows[i].ItemArray[0].ToString().Trim() != "") FK_IdOperation = Convert.ToInt16(DT_Tehnology.Rows[i].ItemArray[0].ToString().Trim());
                        else FK_IdOperation = Detail.Find_FK_IdOperationInSp_Operations(NameOper);

                        CurrencyManager cmgrDet = (CurrencyManager)dGV_Details.BindingContext[dGV_Details.DataSource, dGV_Details.DataMember];
                        DataRow rowDet = ((DataRowView)cmgrDet.Current).Row;
                        long PK_IdOrderDetail = Convert.ToInt64(rowDet["PK_IdOrderDetail"]);
                        int AmountDetails = Convert.ToInt32(rowDet["AmountDetails"]);


                        int Tpd = Convert.ToInt32(DT_Tehnology.Rows[i].ItemArray[2] == DBNull.Value ? 0 : int.TryParse(DT_Tehnology.Rows[i].ItemArray[2].ToString(), out var number) == true ? Convert.ToInt32(DT_Tehnology.Rows[i].ItemArray[2]) : 0);
                        int Tsh = Convert.ToInt32(DT_Tehnology.Rows[i].ItemArray[3] == DBNull.Value ? 0 : int.TryParse(DT_Tehnology.Rows[i].ItemArray[3].ToString(), out var number2) == true ? Convert.ToInt32(DT_Tehnology.Rows[i].ItemArray[3]) : 0);

                        AmountDetails -= Detail.Select_AmountFactDetailsOper(PK_IdOrderDetail, NumOper);
                        if (AmountDetails > 0)//Insert operation
                        {
                            DateTime DateFactOper = dTimeP_Fact.Value;

                            Detail.InsertFactOperation(PK_IdOrderDetail, NumOper, FK_IdOperation, Tpd, Tsh, AmountDetails, DateFactOper, _LoginWorker, 0);
                            //Refresh DataGrid
                            if (cB_InDetail.Checked)
                            {
                                Detail.SelectFullFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
                            }
                            else
                            {
                                Detail.SelectFactOperForDetail(PK_IdOrderDetail, ref DT_FactOper);
                            }
                        }
                    }

            }
            else
            {
                MessageBox.Show("Галочка 'кооп' не выбрана ", "Внимание!", MessageBoxButtons.OK);
            }

        }

		private void OrdersContextMenuStrip_Click(object sender, EventArgs e)
		{

			if (dGV_Orders.GetCellCount(DataGridViewElementStates.Selected) > 0)
			{
				try
				{
					Clipboard.SetDataObject(dGV_Orders.CurrentCell.Value.ToString().Trim());
				}
				catch (System.Runtime.InteropServices.ExternalException)
				{
					MessageBox.Show("Копирование невозможно");
				}
				finally
				{
					dGV_Orders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
					dGV_Orders.CurrentCell.Selected = true;
				}
			}
		}

		private void dGV_Orders_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				if (e.Button == MouseButtons.Right)
				{
					dGV_Orders.ClearSelection();
					dGV_Orders.SelectionMode = DataGridViewSelectionMode.CellSelect;
					dGV_Orders[e.ColumnIndex, e.RowIndex].Selected = true;
				}

				if (e.Button == MouseButtons.Left)
				{
					dGV_Orders.ClearSelection();
					dGV_Orders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
					dGV_Orders[e.ColumnIndex, e.RowIndex].Selected = true;
				}
			}
		}

		private void oShCMContextMenuStrip_Click(object sender, EventArgs e)
		{
			if (dGV_Details.GetCellCount(DataGridViewElementStates.Selected) > 0)
			{
				try
				{
					Clipboard.SetDataObject(dGV_Details.CurrentCell.Value.ToString().Trim());
				}
				catch (System.Runtime.InteropServices.ExternalException)
				{
					MessageBox.Show("Копирование невозможно");
				}
				finally
				{
					dGV_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
					dGV_Details.CurrentCell.Selected = true;
				}
			}
		}

		private void dGV_Details_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.RowIndex >= 0)
			{
				if (e.Button == MouseButtons.Right)
				{
					dGV_Details.ClearSelection();
					dGV_Details.SelectionMode = DataGridViewSelectionMode.CellSelect;
					dGV_Details[e.ColumnIndex, e.RowIndex].Selected = true;
				}

				if (e.Button == MouseButtons.Left)
				{
					dGV_Details.ClearSelection();
					dGV_Details.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
					dGV_Details[e.ColumnIndex, e.RowIndex].Selected = true;
				}
			}
		}

        private void DGV_Tehnology_DefaultValuesNeeded_1(object sender, DataGridViewRowEventArgs e)
        {
            // Устанавливаем значение по умолчанию для пользовательской ячейки ОТК контроля
            e.Row.Cells["Col_OTKControl"].Value = new bool[] { false, false, false };
        }

      
        private void SaveInBD_Click(object sender, EventArgs e)
        {
            SaveOTK saveOTK = new SaveOTK(dGV_Details, dGV_Tehnology, config);
            saveOTK.SaveMethod();
            this.Refresh();
        }

        

        private void DGV_Tehnology_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCell cell = dGV_Tehnology[e.ColumnIndex, e.RowIndex];
                if (cell.OwningColumn.Name == "Col_OTKControl")
                {
                    OTKControlData otkData = cell.Value as OTKControlData;
                    if (otkData != null && !string.IsNullOrEmpty(otkData.Note))
                    {
                        e.ToolTipText = $"Примечание: {otkData.Note}\nДата: {otkData.ChangeDate}\nПользователь: {otkData.User}";
                    }
                    else
                    {
                        e.ToolTipText = null;
                    }
                }
            }
        }
    }
}
