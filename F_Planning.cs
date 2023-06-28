using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispetcher2.Class;
using Dispetcher2.Controls.MyGrid;

namespace Dispetcher2
{
    public partial class F_Planning : Form
    {
        DataTable DT_Orders = new DataTable();
        BindingSource BS_Orders = new BindingSource();
        DataTable DT_TreeView = new DataTable();
        DataTable DT_Gant = new DataTable();
        DataTable DT_Gant_Data = new DataTable();

        public F_Planning()
        {
            InitializeComponent();
            DT_Orders.Columns.Add("PK_IdOrder", typeof(int));
            DT_Orders.Columns.Add("OrderNum", typeof(string));
            DT_Orders.Columns.Add("OrderName", typeof(string));
            DT_Orders.Columns.Add("OrderNum1С", typeof(string));
            DT_Orders.Columns.Add("StartDate", typeof(DateTime));
            DT_Orders.Columns.Add("PlannedDate", typeof(DateTime));
            DT_Orders.Columns.Add("Amount", typeof(Int16));
        }

        private void F_Planning_Load(object sender, EventArgs e)
        {
            dGV_Orders.AutoGenerateColumns = false;
            dGV_Orders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Orders.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Orders.DataSource = DT_Orders;
            dGV_Orders.DataSource = BS_Orders;
            dGV_Orders.Columns["Col_PK_IdOrder"].DataPropertyName = DT_Orders.Columns["PK_IdOrder"].ToString();
            dGV_Orders.Columns["Col_OrderNum"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
            dGV_Orders.Columns["Col_OrderName"].DataPropertyName = DT_Orders.Columns["OrderName"].ToString();
            //Bindings
            tB_OrderName.DataBindings.Add("Text", BS_Orders, "OrderName", false, DataSourceUpdateMode.OnPropertyChanged);
            tB_OrderNumInfo.DataBindings.Add("Text", BS_Orders, "OrderNum", false, DataSourceUpdateMode.OnPropertyChanged);
            mTB_StartOrdDate.DataBindings.Add("Text", BS_Orders, "StartDate", false, DataSourceUpdateMode.OnPropertyChanged);
            mTB_PlannedDate.DataBindings.Add("Text", BS_Orders, "PlannedDate", false, DataSourceUpdateMode.OnPropertyChanged);
            tB_Amount.DataBindings.Add("Text", BS_Orders, "Amount", true, DataSourceUpdateMode.OnPropertyChanged);

            C_Orders.SelectOrdersData(2, ref DT_Orders);//2-opened. LoadOrders
            //C_F_Orders Orders = new C_F_Orders();
            //Orders.AddInTreeViewOrders(PK_IdOrder, OrderNum, OrderName, ref treeViewOrdersDetails);
            //if (treeViewOrdersDetails.SelectedNode == null) treeViewOrdersDetails.SelectedNode = treeViewOrdersDetails.TopNode;
            //CreateGrid();
        }

        private void LoadTreeView(int _PK_IdOrder)
        {
            if (dGV_Orders.SelectedRows.Count > 0)
            {
                /*CurrencyManager cmgr = (CurrencyManager)dGV_Orders.BindingContext[dGV_Orders.DataSource, dGV_Orders.DataMember];
                DataRow row = ((DataRowView)cmgr.Current).Row;
                string NameOper = row["Oper"].ToString();
                dGV_Orders.Columns["Col_PK_IdOrder"].DataPropertyName = DT_Orders.Columns["PK_IdOrder"].ToString();
                dGV_Orders.Columns["Col_OrderNum"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
                dGV_Orders.Columns["Col_OrderName"].DataPropertyName = DT_Orders.Columns["OrderName"].ToString();*/
                string OrderNum = dGV_Orders.SelectedRows[0].Cells[1].Value.ToString();
                string OrderName = dGV_Orders.SelectedRows[0].Cells[2].Value.ToString();
                С_TreeViewOrders C_TV_Orders = new С_TreeViewOrders();
                C_TV_Orders.AddInTreeViewOrders(_PK_IdOrder, OrderNum, OrderName, ref treeViewOrdersDetails,true);
                tnc = treeViewOrdersDetails.Nodes;
                //DT_TreeView = C_TV_Orders.Get_DT_TreeView;
            }
        }



        private void btn_Gant_Click(object sender, EventArgs e)
        {
            DateTime DateStartGant = Convert.ToDateTime("1900-01-01");
            DateTime MinDateFact = Convert.ToDateTime("1900-01-01");
            DateTime DatePlanGant = Convert.ToDateTime("1900-01-01");
            DateTime MaxDateFact = Convert.ToDateTime("1900-01-01");
            int PK_IdOrder = Convert.ToInt32(dGV_Orders.SelectedRows[0].Cells[0].Value);
            LoadTreeView(PK_IdOrder);
            DT_Gant.Reset();
            string sql = "Select o.StartDate,MIN(DateFactOper) MinDateFact,o.PlannedDate,MAX(DateFactOper) MaxDateFact" + "\n" +
                         "From Orders o" + "\n" +
                         "inner join OrdersDetails od on od.FK_IdOrder = o.PK_IdOrder" + "\n" +
                         "left join FactOperation fo on fo.FK_IdOrderDetail = od.PK_IdOrderDetail" + "\n" +
                         "Where o.PK_IdOrder in (" + PK_IdOrder + ") \n" +
                         "Group by o.StartDate,o.PlannedDate";
            C_DataBase CDB = new C_DataBase(C_Gper.ConnStrDispetcher2);
            CDB.Select_DT(ref DT_Gant, sql);
            if (DT_Gant.Rows.Count > 0)
            {

                if (DT_Gant.Rows[0][1].ToString() != "")
                {
                    DateStartGant = Convert.ToDateTime(DT_Gant.Rows[0][0]);
                    MinDateFact = Convert.ToDateTime(DT_Gant.Rows[0][1]);
                    if (DateStartGant > MinDateFact) DateStartGant = MinDateFact;
                }
                if (DT_Gant.Rows[0][3].ToString() != "")
                {
                    DatePlanGant = Convert.ToDateTime(DT_Gant.Rows[0][2]);
                    MaxDateFact = Convert.ToDateTime(DT_Gant.Rows[0][3]);
                    if (DatePlanGant < MaxDateFact) DatePlanGant = MaxDateFact;
                }
                //План кол-во столбцов**********************
                //DateStartGant = Convert.ToDateTime("2019-12-10");
                //DatePlanGant = Convert.ToDateTime("2020-12-18");
                //DateStart = Convert.ToDateTime("2020-01-10");
                //DatePlan = Convert.ToDateTime("2020-06-18");
                CreateGrid(DateStartGant, DatePlanGant);
                //Заполняем
                sql = "SELECT  o.OrderNum,od.PK_IdOrderDetail, sd.ShcmDetail,od.Position,sd.FK_IdTypeDetail,sum(Std.Tsh*od.AmountDetails + Std.Tpd) as SumStd," + "\n" +
                      "sum(fo.Tsh*fo.AmountDetails + fo.Tpd) as SumFO,MIN(fo.DateFactOper) MinDateFO,Max(fo.DateFactOper) MaxDateFO" + "\n" +
                       "FROM dbo.OrdersDetails AS od" + "\n" +
                       "join Orders o on o.PK_IdOrder = od.FK_IdOrder" + "\n" +
                       "join Sp_Details sd on sd.PK_IdDetail = od.FK_IdDetail" + "\n" +
                       "join Sp_TechnologyDetails Std on std.FK_IdDetails = od.FK_IdDetail" + "\n" +
 //--left join Sp_OperationsType111 Std2 on Std2.FK_IdDetail = od.FK_IdDetail" + "\n" +
                       "left join FactOperation fo on fo.FK_IdOrderDetail = od.PK_IdOrderDetail  and fo.NumOper = Std.NumOper" + "\n" +// --and fo.FK_IdOperation = std.FK_IdOperation--
                       "Where FK_IdOrder in (" + PK_IdOrder + ") \n" +
                       "Group by o.OrderNum,od.PK_IdOrderDetail, sd.ShcmDetail,Position,sd.FK_IdTypeDetail" + "\n" +
                       /*"union" + "\n" +
                       "SELECT  o.OrderNum,od.PK_IdOrderDetail, sd.ShcmDetail,od.Position,sd.FK_IdTypeDetail,sum(Std.Tsh*od.AmountDetails + Std.Tpd) as SumStd," + "\n" +
                       "sum(fo.Tsh*fo.AmountDetails + fo.Tpd) as SumFO,MIN(fo.DateFactOper) MinDateFO,Max(fo.DateFactOper) MaxDateFO" + "\n" +
                       "FROM dbo.OrdersDetails AS od" + "\n" +
                       "join Orders o on o.PK_IdOrder = od.FK_IdOrder" + "\n" +
                       "join Sp_Details sd on sd.PK_IdDetail = od.FK_IdDetail" + "\n" +
                       "join Sp_OperationsType111 Std on Std.FK_IdDetail = od.FK_IdDetail" + "\n" +
                       "left join FactOperation fo on fo.FK_IdOrderDetail = od.PK_IdOrderDetail  and fo.NumOper = Std.NumOperation" + "\n" +// --and fo.FK_IdOperation = std.FK_IdOperation--
                       "Where FK_IdOrder in (" + PK_IdOrder + ") \n" +
                       "Group by o.OrderNum,od.PK_IdOrderDetail, sd.ShcmDetail,Position,sd.FK_IdTypeDetail" + "\n" +*/
                       "order by o.OrderNum,Position";
                DT_Gant.Reset();
                CDB.Select_DT(ref DT_Gant, sql);
                if (DT_Gant.Rows.Count > 0)
                {
                    int StartRow = 3;
                    string Old_OrderNum = "", OrderNum = "";
                    CellClickEvent clickController = new CellClickEvent();
                    //cell
                    foreach (DataRow dRow in DT_Gant.Rows)
                    {
                        OrderNum = dRow["OrderNum"].ToString();
                        if (Old_OrderNum == "" || Old_OrderNum != OrderNum)
                        {
                            //DateStartGant = Convert.ToDateTime("2019-12-10");//НЕ МЕНЯТЬ
                            //DatePlanGant = Convert.ToDateTime("2020-12-18");//НЕ МЕНЯТЬ
                            //MinDateFact = Convert.ToDateTime("2019-12-18");
                            //MaxDateFact = Convert.ToDateTime("2020-01-10");
                            Old_OrderNum = OrderNum;
                            myGrid_Gant.Rows.Insert(StartRow);
                            myGrid_Gant[StartRow, 0] = new MyCell(OrderNum, typeof(string));//dRow["ShcmDetail"].ToString()
                            myGrid_Gant[StartRow, 0].Editor.EnableEdit = false;
                            //Подсчёт дней и закраска
                            int stColl = GetAmmountMonth(DateStartGant, MinDateFact)*2;
                            int endColl = GetAmmountMonth(DateStartGant, MaxDateFact)*2;
                            if (MinDateFact.Day < 16) stColl--;
                            if (MaxDateFact.Day < 16) endColl--;
                            while (stColl <= endColl)
                            {
                                //myGrid_Gant[StartRow, stColl] = new MyCell(MinDateFact.ToShortDateString() + "|" + MaxDateFact.ToShortDateString(), typeof(string));
                                myGrid_Gant[StartRow, stColl] = new MyCell("", typeof(string));
                                myGrid_Gant[StartRow, stColl].Editor.EnableEdit = false;
                                myGrid_Gant[StartRow, stColl].View.BackColor = Color.LightGreen;//System.Drawing.SystemColors.Info;
                                stColl++;
                            }
                            StartRow++;
                        }
                        myGrid_Gant.Rows.Insert(StartRow);
                        if (dRow["Position"].ToString() != "")
                        myGrid_Gant[StartRow, 0] = new MyCell(dRow["Position"].ToString() + " | " + dRow.ItemArray[2].ToString(), typeof(string));
                        else myGrid_Gant[StartRow, 0] = new MyCell(dRow.ItemArray[2].ToString(), typeof(string));
                        myGrid_Gant[StartRow, 0].Editor.EnableEdit = false;
                        if (dRow["MinDateFO"].ToString() != "")
                        {
                            MinDateFact = Convert.ToDateTime(dRow["MinDateFO"].ToString());
                            MaxDateFact = Convert.ToDateTime(dRow["MaxDateFO"].ToString());
                            //DateStartGant = Convert.ToDateTime("2019-12-10");//НЕ МЕНЯТЬ
                            //DatePlanGant = Convert.ToDateTime("2020-12-18");//НЕ МЕНЯТЬ
                            //MinDateFact = Convert.ToDateTime("2019-12-18");
                            //MaxDateFact = Convert.ToDateTime("2019-12-18");
                            //Подсчёт дней и закраска
                            int stColl = GetAmmountMonth(DateStartGant, MinDateFact) * 2;
                            int endColl = GetAmmountMonth(DateStartGant, MaxDateFact) * 2;
                            if (MinDateFact.Day < 16) stColl--;
                            if (MaxDateFact.Day < 16) endColl--;
                            Color TypeColor = Color.LightGreen;
                            int SumStd = 0, SumFO = 0;
                            int.TryParse(dRow["SumStd"].ToString(), out SumStd);
                            int.TryParse(dRow["SumFO"].ToString(), out SumFO);
                            if (SumStd > SumFO) TypeColor = Color.LightPink;
                            while (stColl <= endColl)
                            {
                                //myGrid_Gant[StartRow, stColl] = new MyCell(MinDateFact.ToShortDateString() + "|" + MaxDateFact.ToShortDateString(), typeof(string));
                                myGrid_Gant[StartRow, stColl] = new MyCell("", typeof(string));
                                myGrid_Gant[StartRow, stColl].Editor.EnableEdit = false;
                                myGrid_Gant[StartRow, stColl].View.BackColor = TypeColor;//System.Drawing.SystemColors.Info;
                                stColl++;
                            }
                        }
                        myGrid_Gant[StartRow, 0].AddController(clickController);
                        StartRow++;
                    }
                }
                else MessageBox.Show("Нет данных.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);


                //Вариант 2 - используем TreeView
                /*TreeNodeCollection nodes = treeViewOrdersDetails.Nodes;
                foreach (TreeNode n in nodes)
                {
                    //MessageBox.Show("Tag + " + n.Tag.ToString() + "|индекс - " + n.Index + "| Name - " + n.Name + "| Text - " + n.Text);
                    //DT_Select(n, ref StatrInd, n.Tag.ToString(), n.Name, PK_IdOrder, 0, 0);
                    treeViewOrdersDetails.SelectedNode = n;
                    PrintRecursive(n, StatrInd, PK_IdOrder, 0, 0);
                }*/
                //Вариант 3 - используем TreeView
                //TreeNodeCollection _TnCollection = treeViewOrdersDetails.Nodes;
                //PrintRecursive2(_TnCollection, StatrInd, PK_IdOrder, 0,0);
            }
        }

        private int GetAmmountMonth(DateTime _DateStart, DateTime _DateEnd)
        {
            int _ammount = 0;
            if (_DateStart.Year == _DateEnd.Year) _ammount = _DateEnd.Month - _DateStart.Month + 1;
            if (_DateStart.Year < _DateEnd.Year)
                _ammount = (_DateEnd.Year - _DateStart.Year + 1) * 12 - 11 - _DateStart.Month + _DateEnd.Month;
            if (_DateStart.Year > _DateEnd.Year) _ammount = 0;
            return _ammount;
        }

        private void DT_Select(ref int StatrInd, string Tag, string TypeDetails, int _PK_IdOrder, int _Day_AmmStart, int _Day_AmmPlan)
        {
            string sql = "";
            if (Tag == "0")
            {
                myGrid_Gant.Rows.Insert(StatrInd);
                myGrid_Gant[StatrInd, 0] = new MyCell(TypeDetails, typeof(string));
                myGrid_Gant[StatrInd, 0].Editor.EnableEdit = false;
                StatrInd++;
                while (_Day_AmmStart < _Day_AmmPlan)
                {
                    myGrid_Gant[3, _Day_AmmStart + 1] = new MyCell("", typeof(string));
                    myGrid_Gant[3, _Day_AmmStart + 1].Editor.EnableEdit = false;
                    myGrid_Gant[3, _Day_AmmStart + 1].View.BackColor = Color.LightGreen;//System.Drawing.SystemColors.Info;
                    _Day_AmmStart++;
                }
            }
            else
                if (TypeDetails != "111")
                {
                    /*sql = "SELECT  od.PK_IdOrderDetail, sd.ShcmDetail,sum(Std.Tsh*od.AmountDetails + Std.Tpd) as SumStd," + "\n" +
"sum(fo.Tsh*fo.AmountDetails + fo.Tpd) as SumFO,MIN(fo.DateFactOper) MinDateFO,Max(fo.DateFactOper) MaxDateFO" + "\n" +
"FROM dbo.OrdersDetails AS od" + "\n" +
"join Sp_Details sd on sd.PK_IdDetail = od.FK_IdDetail" + "\n" +
"join Sp_TechnologyDetails Std on std.FK_IdDetails = od.FK_IdDetail" + "\n" +
"left join FactOperation fo on fo.FK_IdOrderDetail = od.PK_IdOrderDetail and fo.NumOper = Std.NumOper" + "\n" +
"Where sd.FK_IdTypeDetail<>111 and FK_IdOrder = " + _PK_IdOrder + " and PK_IdOrderDetail = " + Convert.ToInt32(n.Tag) + "\n" +
"Group by od.PK_IdOrderDetail, sd.ShcmDetail";*/ //Чуть не то из-за гальваники.


                    //Полный Вариант
                    /*sql = "SELECT od.FK_IdOrder, od.PK_IdOrderDetail, sd.ShcmDetail, sd.NameDetail, od.Position, od.AmountDetails AS Od_AmountDetails, Std.NumOper AS Std_NumOper," + "\n" +
                      "Std.Tpd AS Std_Tpd, Std.Tsh AS Std_Tsh, so.NameOperation, fo.Tpd AS Fo_tpd, fo.Tsh AS Fo_Tsh, fo.AmountDetails AS FO_AmountDetails" + "\n" +
                      "FROM dbo.OrdersDetails AS od " + "\n" +
                      "join Sp_Details sd on sd.PK_IdDetail = od.FK_IdDetail" + "\n" +
                      "join Sp_TechnologyDetails Std on std.FK_IdDetails = od.FK_IdDetail" + "\n" +
                      "join Sp_Operations so on so.PK_IdOperation = std.FK_IdOperation" + "\n" +
                      "left join FactOperation fo on fo.FK_IdOrderDetail = od.PK_IdOrderDetail and fo.FK_IdOperation = std.FK_IdOperation" + "\n" +
                      "Where sd.FK_IdTypeDetail<>111 and FK_IdOrder = " + _PK_IdOrder + " and PK_IdOrderDetail = " + Convert.ToInt32(n.Tag);*/

                   sql = "SELECT od.FK_IdOrder,od.PK_IdOrderDetail,od.Position, sd.ShcmDetail, sd.NameDetail,Std.NumOper AS Std_NumOper," + "\n" +
                         "(Std.Tsh*od.AmountDetails + Std.Tpd) as PlanTime,(fo.Tsh*fo.AmountDetails + fo.Tpd) as FactTime,fo.DateFactOper,fo.AmountDetails,so.PK_IdOperation,so.NameOperation" + "\n" +
                         "FROM dbo.OrdersDetails AS od" + "\n" +
                         "join Sp_Details sd on sd.PK_IdDetail = od.FK_IdDetail" + "\n" +
                         "join Sp_TechnologyDetails Std on std.FK_IdDetails = od.FK_IdDetail" + "\n" +
                         "left join FactOperation fo on fo.FK_IdOrderDetail = od.PK_IdOrderDetail and fo.NumOper = Std.NumOper" + "\n" +
                         "join Sp_Operations so on so.PK_IdOperation = std.FK_IdOperation" + "\n" +
                         "Where sd.FK_IdTypeDetail<>111 and od.FK_IdOrder = " + _PK_IdOrder + " and od.PK_IdOrderDetail = " + Convert.ToInt32(Tag);
                }
                else
                    if (TypeDetails == "111")
                    {
                        sql = "SELECT od.FK_IdOrder,od.PK_IdOrderDetail,od.Position, sd.ShcmDetail, sd.NameDetail,Std.NumOperation AS Std_NumOper," + "\n" +
                              "(Std.Tsh*od.AmountDetails + Std.Tpd) as PlanTime,(fo.Tsh*fo.AmountDetails + fo.Tpd) as FactTime,fo.DateFactOper,fo.AmountDetails,so.PK_IdOperation,so.NameOperation" + "\n" +
                              "FROM dbo.OrdersDetails AS od" + "\n" +
                              "join Sp_Details sd on sd.PK_IdDetail = od.FK_IdDetail" + "\n" +
                              "join Sp_OperationsType111 Std on Std.FK_IdDetail = od.FK_IdDetail" + "\n" +
                              "left join FactOperation fo on fo.FK_IdOrderDetail = od.PK_IdOrderDetail and fo.NumOper = Std.NumOperation" + "\n" +
                              "join Sp_Operations so on so.PK_IdOperation = std.FK_IdOperation" + "\n" +
                              "Where sd.FK_IdTypeDetail = 111 and od.FK_IdOrder = " + _PK_IdOrder + " and od.PK_IdOrderDetail = " + Convert.ToInt32(Tag);
                    }
            if (sql != "")
            {
                C_DataBase CDB = new C_DataBase(C_Gper.ConnStrDispetcher2);
                CDB.Select_DT(ref DT_Gant, sql);
                if (DT_Gant.Rows.Count > 0)
                {
                    bool top1 = true;
                    DateTime MinDateFO = Convert.ToDateTime("1900-01-01");
                    DateTime MaxDateFO = Convert.ToDateTime("1900-01-01");
                    DateTime DateDt_Gant = Convert.ToDateTime("1900-01-01");
                    int PlanTime = 0, FactTime = 0;

                    foreach (DataRow dRow in DT_Gant.Rows)
                    {
                        if (dRow["DateFactOper"].ToString() != "") DateDt_Gant = Convert.ToDateTime(dRow["DateFactOper"]);
                        if (dRow["PlanTime"].ToString() != "") PlanTime += Convert.ToInt32(dRow["PlanTime"]);
                        if (dRow["FactTime"].ToString() != "") FactTime += Convert.ToInt32(dRow["FactTime"]);
                        //**********************************************************************************************************************
                        if (top1)
                        {
                            MinDateFO = DateDt_Gant;
                            MaxDateFO = DateDt_Gant;
                            myGrid_Gant.Rows.Insert(StatrInd);
                            myGrid_Gant[StatrInd, 0] = new MyCell(dRow["ShcmDetail"], typeof(string));//dRow.ItemArray[1].ToString()
                            myGrid_Gant[StatrInd, 0].Editor.EnableEdit = false;
                            top1 = false;
                            StatrInd++;
                        }
                        else
                        {
                            if (MinDateFO.Year == 1900 || MinDateFO > DateDt_Gant) MinDateFO = DateDt_Gant;
                            if (MaxDateFO.Year == 1900 || MaxDateFO < DateDt_Gant) MaxDateFO = DateDt_Gant;
                            //((Excel.Range)exW.Cells[row, 8]).Interior.Color = Color.LightGreen;
                            //((Excel.Range)exW.Cells[row, 8]).Interior.Color = Color.LightPink;
                            MinDateFO = Convert.ToDateTime("2020-01-17");
                            MaxDateFO = Convert.ToDateTime("2020-03-13");
                            //План кол-во столбцов**********************
                            /*int Year_Amm = _DatePlanGrant.Year - _DateStartGrant.Year + 1;
                            int Month_Amm = (Year_Amm * 12) - _DateStartGrant.Month + _DatePlanGrant.Month - 11;
                            int Day_Amm = Month_Amm * 2;*/



                        }
                        //Начинаем прорисовку






                    }
                }
            }
        }

        private void CreateGrid(DateTime _DateStartGant, DateTime _DatePlanGant)
        {
            myGrid_Gant.Hide();
            //if (myGrid_Gant.RowsCount>0)
            //    myGrid_Gant.Rows.RemoveRange(0, myGrid_Gant.RowsCount);
            if (myGrid_Gant.ColumnsCount>0)
                myGrid_Gant.Columns.RemoveRange(0, myGrid_Gant.ColumnsCount);
            //Общее Кол-во столбцов**********************
            int Day_Amm = GetAmmountMonth(_DateStartGant, _DatePlanGant) * 2;
            //******************************************
            int Year = _DateStartGant.Year;
            int Month_IndArr = _DateStartGant.Month - 1;
            string[] Month_Arr = new string[12] { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            myGrid_Gant.Redim(3, Day_Amm + 2);//Размеры таблицы
            myGrid_Gant.FixedRows = 3;
            myGrid_Gant.FixedColumns = 1;
            myGrid_Gant.Selection.EnableMultiSelection = false;
            myGrid_Gant.BackColor = System.Drawing.SystemColors.Info;
            myGrid_Gant.Columns.SetWidth(Day_Amm + 1, 20);//задаём ширину колонок
            //myGrid_Gant.Rows[0].AutoSizeMode = SourceGrid.AutoSizeMode.None; myGrid_Gant.AutoSizeCells(); myGrid_Gant[0, Coll_Year].Editor.EnableEdit = false;myGrid_Gant[0, Coll_Year] = null;
            //HEADER
            //1 Header Row
            myGrid_Gant[0, 0] = new MyHeader("ЩЦМ");
            myGrid_Gant[0, 0].RowSpan = 3;
            myGrid_Gant[0, 0].Column.Width = 250;
            myGrid_Gant[0, 0].Column.MinimalWidth = 200;
            //2 Header Row
            int Coll_Day = 1;
            int Coll_Year = 1;
            for (int i = 0; i < Day_Amm/2; i++)
            {
                if (Year == _DatePlanGant.Year & _DateStartGant.Year == _DatePlanGant.Year)//Для 1 года
                {
                    myGrid_Gant[0, Coll_Year] = new MyHeader(Year.ToString());
                    myGrid_Gant[0, Coll_Year].ColumnSpan = (_DatePlanGant.Month - _DateStartGant.Month + 1) * 2;
                    Coll_Year = (13 - _DateStartGant.Month) * 2 + 1;
                }
                else//Для нескольких лет
                {
                    if (Year == _DateStartGant.Year)//для 1 года выборки
                    {
                        myGrid_Gant[0, Coll_Year] = new MyHeader(Year.ToString());
                        myGrid_Gant[0, Coll_Year].ColumnSpan = (13 - _DateStartGant.Month) * 2;
                        Coll_Year = (13 - _DateStartGant.Month) * 2 + 1;
                    }
                    if (Year > _DateStartGant.Year & Year < _DatePlanGant.Year)//для полных лет
                    {
                        myGrid_Gant[0, Coll_Year] = new MyHeader(Year.ToString());
                        myGrid_Gant[0, Coll_Year].ColumnSpan = 24;
                        Coll_Year += 24;
                    }
                    //if (Year == _DatePlanGrant.Year & _DateStartGrant.Year != _DatePlanGrant.Year)//для последнего года
                    if (Year == _DatePlanGant.Year)//для последнего года
                    {
                        myGrid_Gant[0, Coll_Year] = new MyHeader(Year.ToString());
                        myGrid_Gant[0, Coll_Year].ColumnSpan = _DatePlanGant.Month * 2;
                    }
                }
                Year++;
                myGrid_Gant[1, Coll_Day] = new MyHeader(Month_Arr[Month_IndArr]);
                myGrid_Gant[1, Coll_Day].ColumnSpan = 2;
                if (Month_IndArr == 11) Month_IndArr = 0;
                else Month_IndArr++;
                myGrid_Gant[2, Coll_Day] = new MyHeader("1-15");
                Coll_Day++;
                myGrid_Gant[2, Coll_Day] = new MyHeader("16+");
                Coll_Day++;
                
            }
            
            myGrid_Gant.Show();
            //AddDataToMyGrid();//Загружаем данные
        }















        #region Настройки

        private static TreeNodeCollection tnc;
        public class CellClickEvent : SourceGrid.Cells.Controllers.ControllerBase
        {
            public override void OnDoubleClick(SourceGrid.CellContext sender, EventArgs e)
            {
                //base.OnClick(sender, e);
                //base.OnDoubleClick(sender, e);
                //MessageBox.Show(sender.Grid, sender.DisplayText);
                //MessageBox.Show(sender.DisplayText);
                С_TreeViewOrders.SearchTree(tnc, sender.DisplayText);
            }
        }


        private void dGV_Orders_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
            //myGrid_Gant[0, Coll_Year]
            //if (e.ColumnIndex == 1) 
            //MessageBox.Show(myGrid_Gant[e.RowIndex, 0].Value.ToString());
        }

        private void dGV_Orders_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                LoadTreeView();
            }*/
        }

        private void tB_OrderNum_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter) LoadTreeView();
        }

        private void tB_OrderNum_TextChanged(object sender, EventArgs e)
        {
            BS_Orders.Filter = " OrderNum like '%" + tB_OrderNum.Text.ToString().Trim() + "%'";
        }

        private void tB_SHCM_S_InOrders_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) С_TreeViewOrders.SearchTree(treeViewOrdersDetails.Nodes, tB_SHCM_S_InOrders.Text.Trim());
        }

        private void tB_SHCM_S_InOrders_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',') e.KeyChar = '.';
        }

        private void dGV_Orders_SelectionChanged(object sender, EventArgs e)
        {
            treeViewOrdersDetails.Nodes.Clear();
        }

        #endregion













    }
}
