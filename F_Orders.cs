using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Dispetcher2.Class;

namespace Dispetcher2
{
    public partial class F_Orders : Form
    {
        public F_Orders()
        {
            InitializeComponent();
        }

        BindingSource BindingSource_Orders = new BindingSource();//Заказы
        DataTable DT_Orders = new DataTable();//Заказы
        //*****************************************
        DataTable DT_OrdersOldDispetcher = new DataTable();//Для загрузки старых заказов
        //*****************************************
        BindingSource Bind_DT_OrdersDetails = new BindingSource();//Детали заказа список
        DataTable DT_OrdersDetails = new DataTable();//Детали заказа список
        string OrderNum = "";//20546002
        string OrderName = "";//Камера проходная ПКВ-1
        int PK_IdOrder = 0;//107
        //*****************************************
        BindingSource BindingS_AllDetails = new BindingSource();//Список всех внутренних деталей
        DataTable DT_AllDetails = new DataTable();//Список всех внутренних деталей
        //*****************************************
        DataTable DT_SpOperations = new DataTable();//Справочник операций для comboBox
        //Операции конкретной внутренней детали пишутся сразу в DataGridView - dGV_AddDetailsInSpOper




        private void AddColumnsInDataTables()
        {
            //C_F_Orders.SelectAllOrders
            DT_Orders.Columns.Add("PK_IdOrder", typeof(int));
            DT_Orders.Columns.Add("OrderNum", typeof(string));
            DT_Orders.Columns.Add("OrderName", typeof(string));
            DT_Orders.Columns.Add("DateCreateOrder", typeof(DateTime));
            DT_Orders.Columns.Add("FK_IdStatusOrders", typeof(byte));
            DT_Orders.Columns.Add("NameStatusOrders", typeof(string));
            DT_Orders.Columns.Add("ValidationOrder", typeof(bool));
            //C_F_Orders.Select_AllOrdersDetails
            DT_OrdersDetails.Columns.Add("NameType", typeof(string));
            DT_OrdersDetails.Columns.Add("Position", typeof(int));
            DT_OrdersDetails.Columns.Add("ShcmDetail", typeof(string));
            DT_OrdersDetails.Columns.Add("NameDetail", typeof(string));
            DT_OrdersDetails.Columns.Add("AmountDetails", typeof(double));
            DT_OrdersDetails.Columns.Add("AllPositionParent", typeof(string));
            DT_OrdersDetails.Columns.Add("PK_IdOrderDetail", typeof(long));
            DT_OrdersDetails.Columns.Add("FK_IdDetail", typeof(long));
            DT_OrdersDetails.Columns.Add("PositionParent", typeof(int));
            //C_Orders.SelectAllDetails(ref DT_AllDetails);NameType,ShcmDetail,NameDetail,PK_IdDetail
            DT_AllDetails.Columns.Add("NameType", typeof(string));
            DT_AllDetails.Columns.Add("ShcmDetail", typeof(string));
            DT_AllDetails.Columns.Add("NameDetail", typeof(string));
            DT_AllDetails.Columns.Add("PK_IdDetail", typeof(long));
        }

        private void F_Orders_Load(object sender, EventArgs e)
        {
            if (C_Gper.F_Orders_View)//Только просмотр
            {
                btn_AddNewDetailInSp.Enabled = false;
                btn_UpdateAmountDetail.Enabled = false;
                btn_Create_Update.Enabled = false;
                btn_AddNewDetail_InSp.Enabled = false;
            }

            AddColumnsInDataTables();
            C_F_Orders C_Orders = new C_F_Orders();
            C_Orders.SelectAllOrders(ref DT_Orders);
            FilterOrders();
            //Bindings
            //****************************************************************************************
            dGV_Orders.AutoGenerateColumns = false;
            dGV_Orders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Orders.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BindingSource_Orders.DataSource = DT_Orders;
            dGV_Orders.DataSource = BindingSource_Orders;
            dGV_Orders.Columns["Col_NameOrder"].DataPropertyName = DT_Orders.Columns["OrderName"].ToString();
            dGV_Orders.Columns["Col_NumOrder"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
            dGV_Orders.Columns["Col_CreateOrder"].DataPropertyName = DT_Orders.Columns["DateCreateOrder"].ToString();
            dGV_Orders.Columns["Col_CreateOrder"].DataPropertyName = DT_Orders.Columns["DateCreateOrder"].ToString();
            dGV_Orders.Columns["Col_Status"].DataPropertyName = DT_Orders.Columns["NameStatusOrders"].ToString();
            dGV_Orders.Columns["Col_Validation"].DataPropertyName = DT_Orders.Columns["ValidationOrder"].ToString();
            //****************************************************************************************
            dGV_AddDetailsFromRas.AutoGenerateColumns = false;
            dGV_AddDetailsFromRas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_AddDetailsFromRas.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            Bind_DT_OrdersDetails.DataSource = DT_OrdersDetails;
            dGV_AddDetailsFromRas.DataSource = Bind_DT_OrdersDetails;
            //dGV_AddDetailsFromRas.DataSource = DT_OrdersDetails;
            dGV_AddDetailsFromRas.Columns["С_NameType"].DataPropertyName = DT_OrdersDetails.Columns["NameType"].ToString();
            dGV_AddDetailsFromRas.Columns["С_Position"].DataPropertyName = DT_OrdersDetails.Columns["Position"].ToString();
            dGV_AddDetailsFromRas.Columns["С_ShcmDetail"].DataPropertyName = DT_OrdersDetails.Columns["ShcmDetail"].ToString();
            dGV_AddDetailsFromRas.Columns["С_NameDetail"].DataPropertyName = DT_OrdersDetails.Columns["NameDetail"].ToString();
            dGV_AddDetailsFromRas.Columns["С_AmountDetails"].DataPropertyName = DT_OrdersDetails.Columns["AmountDetails"].ToString();
            dGV_AddDetailsFromRas.Columns["С_AllPositionParent"].DataPropertyName = DT_OrdersDetails.Columns["AllPositionParent"].ToString();
            dGV_AddDetailsFromRas.Columns["С_PK_IdOrderDetail"].DataPropertyName = DT_OrdersDetails.Columns["PK_IdOrderDetail"].ToString();
            dGV_AddDetailsFromRas.Columns["С_FK_IdDetail"].DataPropertyName = DT_OrdersDetails.Columns["FK_IdDetail"].ToString();
            dGV_AddDetailsFromRas.Columns["С_PositionParent"].DataPropertyName = DT_OrdersDetails.Columns["PositionParent"].ToString();
            tB_OrderNum_Search.Focus();
            //****************************************************************************************
            dGV_AddDetails.AutoGenerateColumns = false;
            dGV_AddDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_AddDetails.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BindingS_AllDetails.DataSource = DT_AllDetails;
            dGV_AddDetails.DataSource = BindingS_AllDetails;
            dGV_AddDetails.Columns["Col_NameType"].DataPropertyName = DT_AllDetails.Columns["NameType"].ToString();
            dGV_AddDetails.Columns["Col_ShcmDetail"].DataPropertyName = DT_AllDetails.Columns["ShcmDetail"].ToString();
            dGV_AddDetails.Columns["Col_NameDetail"].DataPropertyName = DT_AllDetails.Columns["NameDetail"].ToString();
            dGV_AddDetails.Columns["Col_PK_IdDetail"].DataPropertyName = DT_AllDetails.Columns["PK_IdDetail"].ToString();
            //****************************************************************************************
            dGV_AddDetailsInSpOper.AutoGenerateColumns = false;
            dGV_AddDetailsInSpOper.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_AddDetailsInSpOper.RowsDefaultCellStyle.BackColor = SystemColors.Info;

            dGV_Tehnology.AutoGenerateColumns = false;
            dGV_Tehnology.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Tehnology.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            //dGV_AddDetailsInSpOper.DataSource = DT_SpOperationsType111;
            //dGV_AddDetailsInSpOper.Columns["Col_NumOperation"].DataPropertyName = DT_SpOperationsType111.Columns["NumOperation"].ToString();
            //dGV_AddDetailsInSpOper.Columns["Col_NameOperation"].DataPropertyName = DT_SpOperationsType111.Columns["NameOperation"].ToString();
            //dGV_AddDetailsInSpOper.Columns["Col_Tpd"].DataPropertyName = DT_SpOperationsType111.Columns["Tpd"].ToString();
            //dGV_AddDetailsInSpOper.Columns["Col_Tsh"].DataPropertyName = DT_SpOperationsType111.Columns["Tsh"].ToString();
            //dGV_AddDetailsInSpOper.Columns["Col_FKIdOperation"].DataPropertyName = DT_SpOperationsType111.Columns["FK_IdOperation"].ToString();
            //****************************************************************************************
            //if (SelectOrdersFromOldDispetcher()) InsertOldOrders(); //Only!!!!! Import old data Orders in Dispetcher2

        }


        //**********************
        #region tPageOrders
        //**********************

        //**********************
        #region Filters
        //**********************
        private void FilterOrders()
        {
            string FK_IdStatusOrders;
            if (chB_Orders_OnlyClose.Checked)
            {
                FK_IdStatusOrders = "FK_IdStatusOrders = 3";
                BindingSource_Orders.Filter = FK_IdStatusOrders;//Closed
                dGV_Orders.RowsDefaultCellStyle.BackColor = Color.Honeydew;
            }
            else
            {
                FK_IdStatusOrders = "FK_IdStatusOrders <> 3";
                BindingSource_Orders.Filter = FK_IdStatusOrders;//Not closed
                dGV_Orders.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            }
            if (tB_OrderNum_Search.Text.Trim().Length > 0 || tB_OrderName_Search.Text.Trim().Length > 0)
                FilterOrdersTextBox(FK_IdStatusOrders);
        }

        private void FilterOrdersTextBox(string FK_IdStatusOrdersOptions)
        { 
            if (tB_OrderNum_Search.Text.Trim().Length > 0) BindingSource_Orders.Filter = FK_IdStatusOrdersOptions + " and OrderNum like '%" + tB_OrderNum_Search.Text.ToString().Trim() + "%'";
            if (tB_OrderName_Search.Text.Trim().Length > 0) BindingSource_Orders.Filter = FK_IdStatusOrdersOptions + " and OrderName like '%" + tB_OrderName_Search.Text.ToString().Trim() + "%'";
        }

        private void chB_Orders_OnlyClose_CheckedChanged(object sender, EventArgs e)
        {
            FilterOrders();
        }

        private void tB_OrderNum_Search_TextChanged(object sender, EventArgs e)
        {
            if (tB_OrderNum_Search.Text.Trim().Length > 0)
            {
                FilterOrders();
                tB_NewOrderNum.Text = tB_OrderNum_Search.Text.Trim();
            }
            if (tB_OrderNum_Search.Text.Trim().Length == 0 & tB_OrderName_Search.Text.Trim().Length == 0) FilterOrders();
        }

        private void tB_OrderName_Search_TextChanged(object sender, EventArgs e)
        {

            if (tB_OrderName_Search.Text.Trim().Length > 0) FilterOrders();
            if (tB_OrderNum_Search.Text.Trim().Length == 0 & tB_OrderName_Search.Text.Trim().Length == 0) FilterOrders();
        }

        private void tB_SHCM_Search_Enter(object sender, EventArgs e)
        {
            tB_OrderNum_Search.Text = "";tB_OrderName_Search.Text = "";
        }

        private void tB_OrderNum_Search_Enter(object sender, EventArgs e)
        {
            tB_OrderName_Search.Text = "";
        }

        private void tB_OrderName_Search_Enter(object sender, EventArgs e)
        {
            tB_OrderNum_Search.Text = "";
        }

        private void tB_NewOrderNum_Enter(object sender, EventArgs e)
        {
            tB_NewOrderNum.SelectionStart = tB_NewOrderNum.Text.Length;
        }
        #endregion

        //Show or hide create new order panel - gB_NewOrder
        private void myChB_NewOrder_CheckedChanged(object sender, EventArgs e)
        {
            //ClearFilters
            tB_OrderName_Search.Text = "";
            //*********************
            if (myChB_NewOrder.Checked)
            {
                gB_NewOrder.Visible = true;
                tB_NewOrderNum.Focus();
            }
            else
            {
                tB_NewOrderNum.Text = ""; tB_NewOrderName.Text = "";
                gB_NewOrder.Visible = false;
                tB_OrderNum_Search.Focus();
            }
        }

        private void dGV_Orders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                OpenOrdersDetailsForm();
            }
        }

        private void dGV_Orders_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OpenOrdersDetailsForm();
        }

        private void OpenOrdersDetailsForm()
        {
            //Open OrdersDetailsForm
            if (dGV_Orders.CurrentRow != null) OrderNum = dGV_Orders.CurrentRow.Cells["Col_NumOrder"].Value.ToString().Trim();
            if (OrderNum != "")
            {
                OrderName = dGV_Orders.CurrentRow.Cells["Col_NameOrder"].Value.ToString().Trim();
                CurrencyManager cmgr = (CurrencyManager)this.dGV_Orders.BindingContext[this.dGV_Orders.DataSource, dGV_Orders.DataMember];
                DataRow row = ((DataRowView)cmgr.Current).Row;
                PK_IdOrder = Convert.ToInt32(row["PK_IdOrder"]);
                this.MdiParent.Text += " | " + OrderNum + " - " + OrderName;
                //************
                if (chB_Orders_OnlyClose.Checked)//если только закрытые заказы
                    btn_LoadFromExcel.Enabled = false;
                else
                    btn_LoadFromExcel.Enabled = true;
                C_F_Orders C_Orders = new C_F_Orders();//Load data in dGV_AddDetailsFromRas
                C_Orders.Select_AllOrdersDetails(PK_IdOrder, ref DT_OrdersDetails);
                if (C_Gper.F_Orders_View)//Только просмотр
                {
                    btn_LoadFromExcel.Enabled = false; btn_OpenOrders.Enabled = false;
                }
                myTabC_Orders.SelectedTab = tPageOrdersDetails;
                tB_PositionRas.Focus();
            }
        
        }

        private void tB_NewOrderNum_TextChanged(object sender, EventArgs e)
        {
            if (myChB_NewOrder.Checked)
            if (tB_OrderNum_Search.Text.Trim() != tB_NewOrderNum.Text.Trim()) tB_OrderNum_Search.Text = tB_NewOrderNum.Text.Trim();
        }

        private void btn_Create_Update_Click(object sender, EventArgs e)
        {
            if (tB_NewOrderNum.Text.Trim().Length == 0) MessageBox.Show("Введите номер заказа.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                if (tB_NewOrderName.Text.Trim().Length == 0) MessageBox.Show("Введите название заказа.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    if (CheckOrder())
                        if (InsertOrder())
                        {
                            SelectLastRowsOrders();//and add Rows in DataTable
                            MessageBox.Show("Создание заказа успешно завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
        }

        private bool CheckOrder()
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT OrderNum" + "\n" +
                "FROM Orders" + "\n" +
                "Where OrderNum=@OrderNum";
                cmd.Parameters.Add(new SqlParameter("@OrderNum", SqlDbType.VarChar));
                cmd.Parameters["@OrderNum"].Value = tB_NewOrderNum.Text.Trim();
                cmd.Connection = C_Gper.con;
                C_Gper.con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Dispose(); reader.Close(); C_Gper.con.Close();
                    MessageBox.Show("Заказ с таким номером уже зарегестрирован.", "Сохранение заказа отменено!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                else
                {
                    reader.Dispose(); reader.Close(); C_Gper.con.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool InsertOrder()
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
                cmd.CommandText = "insert into Orders (OrderNum,OrderName) " + "\n" +
                                      "values (@OrderNum,@OrderName)";
                //Parameters**************************************************
                cmd.Parameters.Add(new SqlParameter("@OrderNum", SqlDbType.VarChar));
                cmd.Parameters["@OrderNum"].Value = tB_NewOrderNum.Text.Trim();
                cmd.Parameters.Add(new SqlParameter("@OrderName", SqlDbType.VarChar));
                cmd.Parameters["@OrderName"].Value = tB_NewOrderName.Text.Trim();
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

        private void SelectLastRowsOrders()
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.CommandText = "Select PK_IdOrder,OrderNum,OrderName,DateCreateOrder,FK_IdStatusOrders,NameStatusOrders,ValidationOrder" + "\n" +
                                  "From Orders" + "\n" +
                                  "LEFT JOIN SP_StatusOrders ON FK_IdStatusOrders = PK_IdStatusOrders" + "\n" +
                                  "WHERE   PK_IdOrder = (SELECT MAX(PK_IdOrder)  FROM Orders)";
                cmd.Connection = C_Gper.con;
                C_Gper.con.Open();
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    int PK_IdOrder = 1;
                    string OrderNum = "",OrderName = "";
                    DateTime DateCreateOrder = DateTime.Now;
                    byte FK_IdStatusOrders = 1;
                    string NameStatusOrders = "";
                    bool ValidationOrder = false;
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0)) PK_IdOrder = reader.GetInt32(0); else PK_IdOrder = 1;
                        if (!reader.IsDBNull(1)) OrderNum = reader.GetString(1); else OrderNum = "";
                        if (!reader.IsDBNull(2)) OrderName = reader.GetString(2); else OrderName = "";
                        if (!reader.IsDBNull(3)) DateCreateOrder = reader.GetDateTime(3); else DateCreateOrder = DateTime.Now;
                        if (!reader.IsDBNull(4)) FK_IdStatusOrders = reader.GetByte(4); else FK_IdStatusOrders = 1;
                        if (!reader.IsDBNull(5)) NameStatusOrders = reader.GetString(5); else NameStatusOrders = "";
                        if (!reader.IsDBNull(6)) ValidationOrder = reader.GetBoolean(6); else ValidationOrder = false;
                    }
                    //Add Rows in DataTable
                    DT_Orders.Rows.Add(PK_IdOrder, OrderNum, OrderName, DateCreateOrder, FK_IdStatusOrders, NameStatusOrders, ValidationOrder);
                    dGV_Orders.CurrentCell = dGV_Orders.Rows[dGV_Orders.RowCount - 1].Cells[0];
                }
                reader.Dispose(); reader.Close(); C_Gper.con.Close();
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Import old data Orders in Dispetcher2
        private bool SelectOrdersFromOldDispetcher()
        {
            DT_OrdersOldDispetcher.Clear();
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.CommandText = "SELECT Dogovor,Name,dateSetInSystem, idStatus,CheckR" + "\n" +
                                  "FROM Zakaz" + "\n" +
                                  "LEFT JOIN Details ON idDetail = Details.id" + "\n" +
                                  "where Zakaz.id>246";//c 246 т.к. до - ручками вбил
                cmd.Connection = C_Gper.con;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(DT_OrdersOldDispetcher);
                adapter.Dispose();
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

        private void InsertOldOrders()
        {
            string OrderNum = "";
            string OrderName = "";
            DateTime DateCreateOrder = new DateTime();
            int FK_IdStatusOrders = 3;//закрытый
            bool Validation = false;
            //*************************************************************
            C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
            SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
            //cmd.Parameters.Clear();
            cmd.CommandText = "insert into Orders (OrderNum,OrderName,DateCreateOrder,FK_IdStatusOrders,ValidationOrder) " + "\n" +
                "values (@OrderNum,@OrderName,@DateCreateOrder,@FK_IdStatusOrders,@ValidationOrder)";
            cmd.Connection = C_Gper.con;
            cmd.Parameters.Add(new SqlParameter("@OrderNum", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@OrderName", SqlDbType.VarChar));
            cmd.Parameters.Add(new SqlParameter("@DateCreateOrder", SqlDbType.Date));
            cmd.Parameters.Add(new SqlParameter("@FK_IdStatusOrders", SqlDbType.TinyInt));
            cmd.Parameters.Add(new SqlParameter("@ValidationOrder", SqlDbType.Bit));
            //*************************************************************
            foreach (DataRow row in DT_OrdersOldDispetcher.Rows)
            {
                OrderNum = row.ItemArray[0].ToString().Trim();
                if (row.ItemArray[1] == DBNull.Value) OrderName = null; else OrderName = row.ItemArray[1].ToString().Trim();
                if (!DateTime.TryParse(row.ItemArray[2].ToString().Trim(), out DateCreateOrder))
                {
                    MessageBox.Show("Ошибка преобразования данных в поле \"Дата создания договора \"", "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                if (!int.TryParse(row.ItemArray[3].ToString().Trim(), out FK_IdStatusOrders))
                {
                    MessageBox.Show("Ошибка преобразования данных в поле \"Статус договора \"", "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                if (!bool.TryParse(row.ItemArray[4].ToString().Trim(), out Validation))
                {
                    MessageBox.Show("Ошибка преобразования данных в поле \"Валидация договора \"", "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                //Insert Data in Oredrs table from Dispetcher2 
                try
                    {
                        cmd.Parameters["@OrderNum"].Value = OrderNum;
                        if (OrderName == null) cmd.Parameters["@OrderName"].Value = DBNull.Value; else cmd.Parameters["@OrderName"].Value = OrderName;
                        cmd.Parameters["@DateCreateOrder"].Value = DateCreateOrder;
                        cmd.Parameters["@FK_IdStatusOrders"].Value = FK_IdStatusOrders;
                        cmd.Parameters["@ValidationOrder"].Value = Validation;
                        C_Gper.con.Open();
                        cmd.ExecuteNonQuery();
                        C_Gper.con.Close();
                    }
                    catch (Exception ex)
                    {
                        C_Gper.con.Close();
                        MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                //MessageBox.Show(OrderNum + " | " + OrderName + " | " + DateCreateOrder.ToString() + " | " + FK_IdStatusOrders.ToString());
                //break;
            }
        }
#endregion

        #endregion

        //**********************
        #region tPageOrdersDetails
        //**********************

        #region FilterOrderDetails(Sort)
        private void FilterOrderDetails()
        {
            if (tB_PositionRas.Text.Trim().Length > 0) Bind_DT_OrdersDetails.Filter = "Position = " + tB_PositionRas.Text.ToString().Trim();
            else
            if (tB_ShcmRas.Text.Trim().Length > 0) Bind_DT_OrdersDetails.Filter = "ShcmDetail like '%" + tB_ShcmRas.Text.ToString().Trim() + "%'";
            else
            if (tB_NameDetailRas.Text.Trim().Length > 0) Bind_DT_OrdersDetails.Filter = "NameDetail like '%" + tB_NameDetailRas.Text.ToString().Trim() + "%'";
            else
                Bind_DT_OrdersDetails.Filter = "";
        }

        private void tB_PositionRas_TextChanged(object sender, EventArgs e)
        {
            if (tB_PositionRas.Text.Trim().Length > 0) FilterOrderDetails();
            if (tB_PositionRas.Text.Trim().Length == 0 & tB_ShcmRas.Text.Trim().Length == 0 & tB_NameDetailRas.Text.Trim().Length == 0) FilterOrderDetails();
        }

        private void tB_ShcmRas_TextChanged(object sender, EventArgs e)
        {
            if (tB_ShcmRas.Text.Trim().Length > 0) FilterOrderDetails();
            if (tB_PositionRas.Text.Trim().Length == 0 & tB_ShcmRas.Text.Trim().Length == 0 & tB_NameDetailRas.Text.Trim().Length == 0) FilterOrderDetails();
        }

        private void tB_NameDetailRas_TextChanged(object sender, EventArgs e)
        {
            if (tB_NameDetailRas.Text.Trim().Length > 0) FilterOrderDetails();
            if (tB_PositionRas.Text.Trim().Length == 0 & tB_ShcmRas.Text.Trim().Length == 0 & tB_NameDetailRas.Text.Trim().Length == 0) FilterOrderDetails();
        }

        private void tB_PositionRas_Enter(object sender, EventArgs e)
        {
            tB_ShcmRas.Text = ""; tB_NameDetailRas.Text = "";
        }

        private void tB_ShcmRas_Enter(object sender, EventArgs e)
        {
            tB_PositionRas.Text = ""; tB_NameDetailRas.Text = "";
        }

        private void tB_NameDetailRas_Enter(object sender, EventArgs e)
        {
            tB_PositionRas.Text = ""; tB_ShcmRas.Text = "";
        }
        #endregion

        private void btn_BackInOrder_Click(object sender, EventArgs e)
        {
            this.MdiParent.Text = "ПО \"Диспетчер\"" + " - Заказы";
            myTabC_Orders.SelectedTab = tPageOrders;
        }

        private void btn_OrderTree_Click(object sender, EventArgs e)
        {
            myTabC_Orders.SelectedTab = tPageOrdersDetailsTree;
            myTabC_OrdersDetails.SelectedTab = tabPageAdd;
            if (chB_Orders_OnlyClose.Checked)//если только закрытые заказы
            {
                myTabC_OrdersDetails.Visible = false;
                добавитьToolStripMenuItem.Enabled = false;
                изменитьКолвоToolStripMenuItem.Enabled = false;
            }
            else
            {
                myTabC_OrdersDetails.Visible = true;
                добавитьToolStripMenuItem.Enabled = true;
                изменитьКолвоToolStripMenuItem.Enabled = true;
            }
            С_TreeViewOrders C_TV_Orders = new С_TreeViewOrders();
            C_TV_Orders.AddInTreeViewOrders(PK_IdOrder, OrderNum, OrderName, ref treeViewOrdersDetails, false);
            if (treeViewOrdersDetails.SelectedNode == null) treeViewOrdersDetails.SelectedNode = treeViewOrdersDetails.TopNode;
            if (DT_AllDetails.Rows.Count == 0) C_F_Orders.SelectAllDetails(ref DT_AllDetails);
        }


        //Загрузить данные из расцеховки
        private void btn_LoadFromExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string WayFile = "", OnlyWayFile = "", NameFile="";
                using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
                {
                    //openFileDialog1.InitialDirectory = "C:\\Users\\Home\\Desktop";
                    openFileDialog1.Filter = "Excel файлы (*.xls,*.xlsx)|*.xls;*.xlsx";
                    openFileDialog1.FilterIndex = 1;
                    openFileDialog1.Multiselect = false;
                    //openFileDialog1.RestoreDirectory = true;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        WayFile = openFileDialog1.FileName.ToString().Trim();
                        OnlyWayFile = WayFile.Remove(WayFile.LastIndexOf("\\"));
                        NameFile = openFileDialog1.SafeFileName.Trim();
                        //Сама загрузка
                        //Cs_Gper.ClearDS();
                        //dGridViewGild.DataSource = null;
                        //dGridViewGild.DataMember = "";
                        //btn_LoadDataXLS.Text = "Ждите";
                        DataTable DT_ExcelData = new DataTable();
                        C_Excel ExcelFile = new C_Excel();
                        string ErrorsDataPos;
                        //Считываем данные из Excel
                        ExcelFile.ReadExcelRas(WayFile, PK_IdOrder, ref DT_ExcelData, out ErrorsDataPos);
                        if (ErrorsDataPos != "")
                            MessageBox.Show("Найдены ошибки в строках с номерами \"Поз.\": " + ErrorsDataPos, "Исправьте ошибки!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        else
                        {
                            if (DT_ExcelData.Rows.Count > 0)
                            {
                                DialogResult dR = MessageBox.Show("Продолжить запись данных?", "Внимание!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                if (dR == DialogResult.Yes)
                                {
                                    if (ExcelFile.InsertDetailsInOrderFromExcel(DT_ExcelData))
                                    {
                                        MessageBox.Show("Данные загружены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //Load data in dGV_AddDetailsFromRas
                                        C_F_Orders C_Orders = new C_F_Orders();
                                        C_Orders.Select_AllOrdersDetails(PK_IdOrder, ref DT_OrdersDetails);
                                    }
                                }
                            }
                            else MessageBox.Show("Данных для записи не обнаружено.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_OpenOrders_Click(object sender, EventArgs e)
        {
            //Load data in dGV_AddDetailsFromRas
            if (PK_IdOrder > 0)
            {
                if (dGV_Orders.CurrentRow.Cells["Col_Status"].Value.ToString().Trim() != "закрыт")
                {
                    C_F_Orders C_Orders = new C_F_Orders();
                    C_Orders.OpenOrders(PK_IdOrder, 2);//2-открыт
                }
                //C_Orders.Select_AllOrdersDetails(PK_IdOrder, ref DT_OrdersDetails);
                MessageBox.Show("Заказ открыт.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.MdiParent.Text = "ПО \"Диспетчер\"" + " - Заказы";
                //myTabC_Orders.SelectedTab = tPageOrders;
                //tB_OrderNum_Search.Text = OrderNum;
            }
        }

        private void tB_PositionRas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void tB_ShcmRas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',') e.KeyChar = '.';
        }

        #endregion

        //**********************
        #region tPageOrdersDetailsTree
        //**********************
        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myTabC_OrdersDetails.SelectedTab = tabPageAdd;
            tB_SHCM_Add.Focus();
        }

        private void изменитьКолвоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeViewOrdersDetails.SelectedNode == null || treeViewOrdersDetails.SelectedNode.Tag.ToString() == "0") MessageBox.Show("Не выбрана сборка/деталь.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                tB_PK_IdDetailChange.Text = treeViewOrdersDetails.SelectedNode.Tag.ToString();
                tB_NameDetailChange.Text = treeViewOrdersDetails.SelectedNode.Text;
                myTabC_OrdersDetails.SelectedTab = tabPageChange;
                numericUpDChange.Focus();
            }
        }

        private void выйтиИзЗаказаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.MdiParent.Text = "ПО \"Диспетчер\"" + " - Заказы";
            myTabC_Orders.SelectedTab = tPageOrders;
        }

        private void tB_SHCM_Add_TextChanged(object sender, EventArgs e)
        {
            SHCM_Add_Filter(tB_SHCM_Add.Text.ToString().Trim());
            if (dGV_AddDetails.RowCount == 0)
            {
                pnl_Add_InsideDetails.Visible = true;
                gB_AddDetails.Visible = false;
                gB_Tehnology.Visible = false;
                numericUpD_SHCM_Add.Visible = false;
            }
            else
            {
                pnl_Add_InsideDetails.Visible = false;
                gB_AddDetails.Visible = true;
                gB_Tehnology.Visible = true;
                numericUpD_SHCM_Add.Visible = true;
            }
        }

        private void SHCM_Add_Filter(string ShcmDetail)
        {
            if (tB_SHCM_Add.Text.Trim().Length > 0 && DT_AllDetails.Columns.Count > 0) BindingS_AllDetails.Filter = "ShcmDetail like '%" + ShcmDetail + "%'";
            else BindingS_AllDetails.Filter = "";
        }

        private void dGV_AddDetails_KeyDown(object sender, KeyEventArgs e)
        {
            if (C_Gper.F_Orders_View) MessageBox.Show("Вашей учётной записи запрещена данная операция.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);//Только просмотр
            else
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                string ShcmDetail = "", NameDetail = "";
                long PK_IdDetail = 0;
                int AmountDetails = (int)numericUpD_SHCM_Add.Value;
                int PositionParent = 0;
                if (dGV_AddDetails.CurrentRow != null)
                {
                    PK_IdDetail = Convert.ToInt64(dGV_AddDetails.CurrentRow.Cells["Col_PK_IdDetail"].Value);
                    ShcmDetail = dGV_AddDetails.CurrentRow.Cells["Col_ShcmDetail"].Value.ToString().Trim();
                    NameDetail = dGV_AddDetails.CurrentRow.Cells["Col_NameDetail"].Value.ToString().Trim();
                }
                //Для внутренних деталей (К ним нельзя ничего прикреплять т.к. у них нет позиции)
                int StartIndexToDelete = treeViewOrdersDetails.SelectedNode.Text.IndexOf("|");
                if (treeViewOrdersDetails.SelectedNode.Tag.ToString() != "0" && (StartIndexToDelete<0 || !int.TryParse(treeViewOrdersDetails.SelectedNode.Text.Remove(StartIndexToDelete).Trim(), out PositionParent)))
                    MessageBox.Show("Нельзя прикрепить деталь к выбранному узлу (внутренняя деталь).", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    if (treeViewOrdersDetails.SelectedNode.Text.IndexOf("| К -") > 0) MessageBox.Show("Нельзя прикрепить деталь к выбранному узлу (крепёж).", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                {
                    //***********************
                    //Делаем запись в базу в OrdersDetails
                    long PK_IdOrderDetail;
                    C_F_Orders C_Orders = new C_F_Orders();
                    //***********************
                    if (C_Orders.InsertDetailInOrder(PK_IdOrder, PK_IdDetail, AmountDetails, PositionParent, out PK_IdOrderDetail))
                    {
                        TreeNode ChildNode = new TreeNode();
                        ChildNode.Text = ShcmDetail + " - " + NameDetail + " (" + numericUpD_SHCM_Add.Value.ToString() + ")";
                        ChildNode.Tag = PK_IdOrderDetail;
                        if (treeViewOrdersDetails.SelectedNode.Tag.ToString() == "0") treeViewOrdersDetails.Nodes.Add(ChildNode);
                        else
                        {
                            treeViewOrdersDetails.SelectedNode.Nodes.Add(ChildNode);
                            if (!treeViewOrdersDetails.SelectedNode.IsExpanded) treeViewOrdersDetails.SelectedNode.Expand();
                        }
                        ChildNode.BackColor = Color.DarkSeaGreen;
                        treeViewOrdersDetails.SelectedNode = ChildNode.Parent;
                        treeViewOrdersDetails.Focus();
                        MessageBox.Show("Деталь добавлена.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btn_UpdateAmountDetail_Click(object sender, EventArgs e)
        {
            //Изменяем количество деталей в заказе
            C_F_Orders C_Orders = new C_F_Orders();
            //Крепёж
            bool Fasteners;
            string amount = "";
            if (treeViewOrdersDetails.SelectedNode.Text.IndexOf("| К -") > 0)
            {
                Fasteners = true;
                amount = ((double)numericUpDChange.Value).ToString();
            }
            else
            {
                Fasteners = false;
                amount = ((int)numericUpDChange.Value).ToString();
            }
            if (C_Orders.UpdateAmountDetailOrFasteners(Convert.ToInt64(treeViewOrdersDetails.SelectedNode.Tag), (double)numericUpDChange.Value, Fasteners))
            {
                MessageBox.Show("Количество изменено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                treeViewOrdersDetails.SelectedNode.Text = treeViewOrdersDetails.SelectedNode.Text.Remove(treeViewOrdersDetails.SelectedNode.Text.LastIndexOf("(")) + "(" + amount + ")";
                numericUpDChange.Value = 1;
                myTabC_OrdersDetails.SelectedTab = tabPageAdd;
            }
        }

        private void numericUpDChange_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (treeViewOrdersDetails.SelectedNode.Text.IndexOf("| К -") < 0)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
            if (numericUpDChange.Value > 10000)
            {
                MessageBox.Show("Превышено максимальное значение.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                numericUpDChange.Value = 1;
            }
        }

        private void dGV_AddDetails_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dGV_Tehnology.Rows.Clear();
            if (dGV_AddDetails.CurrentRow != null) SelectTehnology(Convert.ToInt64(dGV_AddDetails.CurrentRow.Cells["Col_PK_IdDetail"].Value));
        }

        private void dGV_AddDetails_SelectionChanged(object sender, EventArgs e)
        {
            dGV_Tehnology.Rows.Clear();
            if (dGV_AddDetails.CurrentRow != null && dGV_AddDetails.CurrentRow.Cells["Col_PK_IdDetail"].Value != null && C_Gper.con.State == ConnectionState.Closed) 
                SelectTehnology(Convert.ToInt64(dGV_AddDetails.CurrentRow.Cells["Col_PK_IdDetail"].Value));
        }

        private void SelectTehnology(long FK_IdDetail)
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                SqlDataReader reader;
                cmd.Parameters.Clear();
                cmd.CommandText = "SELECT FK_IdOperation,NumOperation +' ' + NameOperation as Operation,Tpd,Tsh " + "\n" +
                "FROM Sp_OperationsType111 " + "\n" +
                "INNER JOIN Sp_Operations on Sp_Operations.PK_IdOperation = Sp_OperationsType111.FK_IdOperation " + "\n" +
                "WHERE FK_IdDetail=@FK_IdDetail";
                cmd.Parameters.Add(new SqlParameter("@FK_IdDetail", SqlDbType.BigInt));
                cmd.Parameters["@FK_IdDetail"].Value = FK_IdDetail;
                cmd.Connection = C_Gper.con;
                C_Gper.con.Open();
                reader = cmd.ExecuteReader();
                Int16 idoper = 0;
                string oper = "";
                int _Tpd = 0;
                int _Tsh = 0;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        if (!reader.IsDBNull(0)) idoper = reader.GetInt16(0); else idoper = 0;
                        if (!reader.IsDBNull(1)) oper = reader.GetString(1); else oper = "";
                        if (!reader.IsDBNull(2)) _Tpd = reader.GetInt32(2); else _Tpd = 0;
                        if (!reader.IsDBNull(3)) _Tsh = reader.GetInt32(3); else _Tsh = 0;
                        dGV_Tehnology.Rows.Add(idoper, oper, _Tpd, _Tsh);
                    }
                }
                reader.Dispose(); reader.Close(); C_Gper.con.Close();
                if (dGV_Tehnology.Rows.Count > 0) dGV_Tehnology.Rows.Add(32, "Передача детали на СГД", 0, 0);
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (treeViewOrdersDetails.SelectedNode == null) 
            //treeViewOrdersDetails.Nodes.Clear();
        }

        private void treeViewOrdersDetails_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(treeViewOrdersDetails.SelectedNode.Tag.ToString());
            //MessageBox.Show(treeViewOrdersDetails.SelectedNode.FullPath);//Полный путь из значений Text
            //MessageBox.Show(treeViewOrdersDetails.SelectedNode.Handle.ToString());//Дескриптор узла дерева
            //treeViewOrdersDetails.SelectedNode.Index//индекс до ближайшего родителя
            //MessageBox.Show(treeViewOrdersDetails.SelectedNode.Level.ToString());//Глубина узла дерева т.е. сколбко родителей впереди
        }

        private void tB_SHCM_S_InOrders_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',') e.KeyChar = '.';
        }

        private void tB_SHCM_S_InOrders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                С_TreeViewOrders.SearchTree(treeViewOrdersDetails.Nodes, tB_SHCM_S_InOrders.Text.Trim());
            }
        }

        private void tB_SHCM_Add_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',') e.KeyChar = '.';
        }

        private void btn_AddNewDetail_InSp_Click(object sender, EventArgs e)
        {
            if (C_Orders.Check_ShcmDetail(tB_SHCM_Add.Text.Trim()))
                MessageBox.Show("Деталь с таким ЩЦМ уже существует. Повторная регистрация невозможна.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                if (cB_SpOperations.Items.Count == 0)
                {
                    C_F_Orders CF_Orders = new C_F_Orders();
                    CF_Orders.Select_SpOperations(ref DT_SpOperations);
                    cB_SpOperations.DataSource = DT_SpOperations;
                    cB_SpOperations.DisplayMember = "NameOperation";
                    cB_SpOperations.ValueMember = "PK_IdOperation";
                }
                dGV_AddDetailsInSpOper.Rows.Clear();
                tB_SHCM_NewDetail.Text = tB_SHCM_Add.Text.Trim();
                tB_DetailName_Add.Text = "";
                cB_SpOperations.SelectedIndex = -1; cB_SpOperations.Text = "";
                tB_NumOper.Text = "005"; nUpD_Tpd.Value = 0; nUpD_Tsh.Value = 0;
                myTabC_OrdersDetails.SelectedTab = tabPageAddNewDetail;
                tB_DetailName_Add.Focus();
            }
        }
        #endregion

        #region tabPageAddNewDetail

        private void btn_AddNewDetailInSp_Click(object sender, EventArgs e)
        {
            if (tB_SHCM_Add.Text.Trim() == "") MessageBox.Show("Введите ЩЦМ сборки/детали.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                if (tB_DetailName_Add.Text.Trim() == "") MessageBox.Show("Введите наименование сборки/детали.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    if (dGV_AddDetailsInSpOper.Rows.Count == 0) MessageBox.Show("Отсутствует тех. процесс. \n Можно добавить операцию нажав клавишу \"Enter\" в поле \"Tsh,min\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    {
                        DialogResult dR = MessageBox.Show("Продолжить запись данных?", "Внимание!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dR == DialogResult.Yes)
                        {
                            long PK_IdDetail;
                            C_F_Orders CF_Orders = new C_F_Orders();
                            //Записываем деталь в справочник деталей
                            if (CF_Orders.InsertDetailInSp_Details(tB_SHCM_Add.Text.Trim(), tB_DetailName_Add.Text.Trim(), out PK_IdDetail))
                            {
                                //Записываем техпроцесс в справочник техпроцессов для ВНУТРЕННИХ деталей Sp_OperationsType111
                                if (CF_Orders.InsertDetailInSp_OperationsType111(PK_IdDetail, dGV_AddDetailsInSpOper))
                                {
                                    MessageBox.Show("Внутренняя сборка/деталь успешно добавлена в справочник.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    pnl_Add_InsideDetails.Visible = false;
                                    gB_AddDetails.Visible = true;
                                    gB_Tehnology.Visible = true;
                                    numericUpD_SHCM_Add.Visible = true;
                                    //Add Row in DataTable//Select NameType,ShcmDetail,NameDetail,PK_IdDetail,IdLoodsman
                                    DT_AllDetails.Rows.Add("Внутренние детали", tB_SHCM_Add.Text.Trim(), tB_DetailName_Add.Text.Trim(), PK_IdDetail);//111 - внутренняя деталь
                                    myTabC_OrdersDetails.SelectedTab = tabPageAdd;
                                    tB_SHCM_Add.Focus();
                                    SHCM_Add_Filter(tB_SHCM_Add.Text.ToString().Trim());
                                }
                                else
                                    MessageBox.Show("Сохранение тех. процесса завершилось неудачей.", "Обратитесь за помощью к разработчику данного ПО!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            }
                            else MessageBox.Show("Сохранение сборки/детали завершилось неудачей.", "Обратитесь за помощью к разработчику данного ПО!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                    }
                }
        }



        private void tB_NumOper_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        private void nUpD_Tpd_Click(object sender, EventArgs e)
        {
            nUpD_Tpd.Select(0, nUpD_Tpd.Value.ToString().Length);
        }

        private void nUpD_Tpd_Enter(object sender, EventArgs e)
        {
            nUpD_Tpd.Select(0, nUpD_Tpd.Value.ToString().Length);
        }

        private void nUpD_Tpd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) nUpD_Tsh.Focus();
        }

        private void nUpD_Tsh_Click(object sender, EventArgs e)
        {
            nUpD_Tsh.Select(0, nUpD_Tsh.Value.ToString().Length);
        }

        private void nUpD_Tsh_Enter(object sender, EventArgs e)
        {
            nUpD_Tsh.Select(0, nUpD_Tsh.Value.ToString().Length);
        }

        private void nUpD_Tsh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Int16 num;
                if (!Int16.TryParse(tB_NumOper.Text.Trim(), out num)) MessageBox.Show("Проверьте № тех. процесса.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                if (FindNumOperationsInDataGrid(tB_NumOper.Text, dGV_AddDetailsInSpOper)) MessageBox.Show("Операция тех. процесса с таким номером была внесена ранее! \n Проверте номер!", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                if (cB_SpOperations.SelectedValue == null) MessageBox.Show("Укажите операцию тех. процесса.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    dGV_AddDetailsInSpOper.Rows.Add(tB_NumOper.Text.Trim(), cB_SpOperations.Text.ToString(), (int)nUpD_Tpd.Value, (int)nUpD_Tsh.Value, cB_SpOperations.SelectedValue);
                    cB_SpOperations.SelectedIndex = -1; cB_SpOperations.Text = "";
                    num += 5;
                    if (num < 100) tB_NumOper.Text = "0" + num.ToString();
                    else
                    tB_NumOper.Text = num.ToString(); 
                    nUpD_Tpd.Value = 0; nUpD_Tsh.Value = 0;
                }
            }
        }

        private bool FindNumOperationsInDataGrid(string _TxtNum, DataGridView _DGV)
        {
            bool find = false;
            foreach (DataGridViewRow row in _DGV.Rows)
            {
                if (row.Cells[0].Value.ToString().Trim() == _TxtNum.Trim())
                {
                    find = true;
                    break;
                }
            }
            if (find) return true; else return false;
        }

        private void dGV_AddDetailsInSpOper_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) dGV_AddDetailsInSpOper.Rows.RemoveAt(dGV_AddDetailsInSpOper.SelectedRows[0].Index);
        }










































        #endregion

        private void копироватьВБуферToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dGV_Orders.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    Clipboard.SetDataObject(dGV_Orders.GetClipboardContent());
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

        private void копироватьВБуферToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dGV_AddDetailsFromRas.GetCellCount(DataGridViewElementStates.Selected) > 0)
            {
                try
                {
                    Clipboard.SetDataObject(dGV_AddDetailsFromRas.GetClipboardContent());
                }
                catch (System.Runtime.InteropServices.ExternalException)
                {
                    MessageBox.Show("Копирование невозможно");
                }
                finally
                {
                    dGV_AddDetailsFromRas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dGV_AddDetailsFromRas.CurrentCell.Selected = true;
                }
            }
        }

        private void dGV_AddDetailsFromRas_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dGV_AddDetailsFromRas.ClearSelection();
                    dGV_AddDetailsFromRas.SelectionMode = DataGridViewSelectionMode.CellSelect;
                    dGV_AddDetailsFromRas[e.ColumnIndex, e.RowIndex].Selected = true;
                }

                if (e.Button == MouseButtons.Left)
                {
                    dGV_AddDetailsFromRas.ClearSelection();
                    dGV_AddDetailsFromRas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dGV_AddDetailsFromRas[e.ColumnIndex, e.RowIndex].Selected = true;
                }
            }
        }
    }
}
