using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispetcher2.Class;


//Полный скрипт - здесь v2 больше для наглядности (что и к чему относится)
/*select r.id,r.idparent,r.idchild,r.minquantity,v2.id,v2.idtype,v2.product,v.id,v.idtype,v.idproduct,v.product
from НИИПМ.dbo.rvwRelations r
inner join НИИПМ.dbo.rvwVersions v2 on v2.id=r.idparent
left join НИИПМ.dbo.rvwVersions v on v.id=r.idchild
where v.idtype in (343,235) and v2.product = 'ЩЦМ 2.700.072' and v2.idstate=40*/

namespace Dispetcher2
{
    public partial class F_Kit : Form
    {
        public F_Kit()
        {
            InitializeComponent();
            DT_Orders.Columns.Add("PK_IdOrder", typeof(int));
            DT_Orders.Columns.Add("OrderNum", typeof(string));
            DT_Orders.Columns.Add("OrderName", typeof(string));
            //--------------------------------------------------
            DT_Details.Columns.Add("Position", typeof(int));
            DT_Details.Columns.Add("IdLoodsman", typeof(long));
            DT_Details.Columns.Add("ShcmDetail", typeof(string));
            DT_Details.Columns.Add("AmountDetails", typeof(int));
            //--------------------------------------------------
            DT_Kit.Columns.Add("Row", typeof(long));
            DT_Kit.Columns.Add("Position", typeof(string));
            DT_Kit.Columns.Add("IdLoodsman", typeof(long));
            DT_Kit.Columns.Add("ShcmDetail", typeof(string));
            DT_Kit.Columns.Add("AmountDetails", typeof(string));
            DT_Kit.Columns.Add("minquantity", typeof(double));
            //DT_Kit.Columns.Add("minquantity", typeof(int));
            DT_Kit.Columns.Add("idtype", typeof(int));
            DT_Kit.Columns.Add("NameProduct", typeof(string));
            DT_Kit.Columns.Add("id", typeof(int));//это IdLoodsman - но только для комплектацииv, из лоцмана возвращается как int(в сводном отчёте это PK_IdKit)
            DT_Kit.Columns.Add("OrderNum", typeof(string));
            DT_Kit.Columns.Add("AmountKit", typeof(double));
            DT_Kit.Columns.Add("Idloodsman1C", typeof(int));//В сводном отчёте это IdLoodsman комплектации из 1С
            DT_Kit.Columns.Add("FK_1С_IdKit", typeof(long));

            

            //AmountKit,IdLoodsman,FK_1С_IdKit


            lbl_RowsCount.Text = "";
        }






        DataTable DT_Orders = new DataTable();
        BindingSource BS_Orders = new BindingSource();
        DataTable DT_Details = new DataTable();
        DataTable DT_Kit = new DataTable();
        BindingSource BS_Kit = new BindingSource();
        //int PK_IdOrder = 0;


        private void F_Kit_Load(object sender, EventArgs e)
        {
            dGV_Orders.AutoGenerateColumns = false;
            dGV_Orders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Orders.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Orders.DataSource = DT_Orders;
            dGV_Orders.DataSource = BS_Orders;

            dGV_Orders.Columns["Col_PK_IdOrder"].DataPropertyName = DT_Orders.Columns["PK_IdOrder"].ToString();
            dGV_Orders.Columns["Col_OrderNum"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
            //Bindings
            //tB_OrderNum.DataBindings.Add("Text", BS_Orders, "OrderName", false, DataSourceUpdateMode.OnPropertyChanged);
            C_Orders.SelectOrdersData(2, ref DT_Orders);//2-opened
            //------ВРЕМЕННО------
            dGV_Kit.AutoGenerateColumns = false;
            dGV_Kit.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV_Kit.RowsDefaultCellStyle.BackColor = SystemColors.Info;
            BS_Kit.DataSource = DT_Kit;
            dGV_Kit.DataSource = BS_Kit;
            dGV_Kit.Columns["Col_NumRow"].DataPropertyName = DT_Kit.Columns["Row"].ToString();
            dGV_Kit.Columns["Col_Position"].DataPropertyName = DT_Kit.Columns["Position"].ToString();
            dGV_Kit.Columns["Col_SHCM"].DataPropertyName = DT_Kit.Columns["ShcmDetail"].ToString();
            dGV_Kit.Columns["Col_Amount"].DataPropertyName = DT_Kit.Columns["AmountDetails"].ToString();
            dGV_Kit.Columns["Col_NameKit"].DataPropertyName = DT_Kit.Columns["NameProduct"].ToString();
            dGV_Kit.Columns["Col_PlanKit"].DataPropertyName = DT_Kit.Columns["minquantity"].ToString();
            dGV_Kit.Columns["Col_FactKit"].DataPropertyName = DT_Kit.Columns["AmountKit"].ToString();
            dGV_Kit.Columns["Col_Order"].DataPropertyName = DT_Kit.Columns["OrderNum"].ToString();
            dGV_Kit.Columns["Col_IdLoodsmanKit"].DataPropertyName = DT_Kit.Columns["id"].ToString();
            dGV_Kit.Columns["Col_1C_loodsman_IdKit"].DataPropertyName = DT_Kit.Columns["Idloodsman1C"].ToString();
            dGV_Kit.Columns["Col_1C_IdKit"].DataPropertyName = DT_Kit.Columns["FK_1С_IdKit"].ToString();
            

            //AmountKit,IdLoodsman,FK_1С_IdKit

        }



        private void tB_OrderNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter & dGV_Orders.Rows.Count > 0)
            {
                tB_ShcmDetail.Focus();
                tB_ShcmDetail.Select(0, tB_ShcmDetail.Text.ToString().Length);
            }
        }

        private void dGV_Orders_SelectionChanged(object sender, EventArgs e)
        {
            lbl_RowsCount.Text = "";
            if (DT_Kit.Rows.Count > 0) DT_Kit.Clear();
        }

        private void btn_OrderDetails_Click(object sender, EventArgs e)
        {
            tB_ShcmDetail.Text = ""; tB_NameKit.Text = ""; lbl_RowsCount.Text = "";
            //CurrencyManager cmgr = (CurrencyManager)dGV_Orders.BindingContext[dGV_Orders.DataSource, dGV_Orders.DataMember];
            //DataRow row = ((DataRowView)cmgr.Current).Row;
            //int PK_IdOrder = Convert.ToInt32(row["PK_IdOrder"]);
            DT_Kit.Clear();
            int PK_IdOrder = 0;
            string All_PK_IdOrder = "";
            string OrderNum = "";
            if (radioBtn_Loodsman.Checked & chB_Summary.Checked) MessageBox.Show("Возможно сформировать только для БД \"Диспетчер\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
//***********************************
                for (int i = dGV_Orders.SelectedRows.Count - 1; i >= 0; --i)
                {
                    DataGridViewRow dgrv = dGV_Orders.SelectedRows[i];
                    if (radioBtn_Loodsman.Checked)
                    {
                        PK_IdOrder = Convert.ToInt32(dgrv.Cells[0].Value);
                        OrderNum = dgrv.Cells[1].Value.ToString();
                        LoadDataOfLoodsman(PK_IdOrder, OrderNum);
                    }
                    else
                    {
                        if (All_PK_IdOrder == "") All_PK_IdOrder = dgrv.Cells[0].Value.ToString();
                        else All_PK_IdOrder += "," + dgrv.Cells[0].Value.ToString();
                    }
                }
                if (radioBtn_Disp.Checked & !chB_Summary.Checked) LoadDataOfDispencher(All_PK_IdOrder);
                if (radioBtn_Disp.Checked & chB_Summary.Checked) LoadDataOfDispencherSvod(All_PK_IdOrder);
                lbl_RowsCount.Text = DT_Kit.Rows.Count.ToString();
                if (DT_Kit.Rows.Count > 0) MessageBox.Show("Формирование завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    if (radioBtn_Disp.Checked) MessageBox.Show("В заказе отсутствуют позиции с комплектацией.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//***********************************
            }
        }

        private void LoadDataOfDispencher(string All_PK_IdOrder)
        {
            if (All_PK_IdOrder != "")
            {
                string str = "";
                DataTable DT = new DataTable();
                if (chB_235Kit.Checked) str = " spK.FK_IdTypeKit in (235,343) "; else str = " spK.FK_IdTypeKit = 343 ";
                C_DataBase DB = new C_DataBase(C_Gper.ConnStrDispetcher2);
                string sql = "SELECT ROW_NUMBER() OVER(Order by o.OrderNum,Position) as Row,STR(OD.Position) as Position,spD.IdLoodsman,spD.ShcmDetail,STR(OD.AmountDetails) as AmountDetails,spK.NameProduct,spK.minquantity * OD.AmountDetails as minquantity,o.OrderNum,spK.PK_IdKit as id" + "\n" +
                             "FROM [Dispetcher2].[dbo].[OrdersDetails] OD " + "\n" +
                             "Inner Join Orders o On o.PK_IdOrder =  oD.FK_IdOrder " + "\n" +
                             "Inner Join Sp_Details spD On spD.PK_IdDetail = OD.FK_IdDetail " + "\n" +
                             "Inner Join Sp_Kit spK On spK.IdLoodsmanParent = spD.IdLoodsman " + "\n" +
                             "Where OD.FK_IdOrder in (" + All_PK_IdOrder + ") and spD.FK_IdTypeDetail in (232,346) and " + str;// +"\n" +
                             //"Order by o.OrderNum, Row, Position";
                //DB.Select_DT(ref DT, sql);
                //DT_Kit.Merge(DT, true);
                DB.Select_DT(ref DT_Kit, sql);
                
            }
        }

        //Сводная
        private void LoadDataOfDispencherSvod(string All_PK_IdOrder)
        {
            if (All_PK_IdOrder != "")
            {
                string str = "";
                //if (chB_235Kit.Checked) str = " spK.FK_IdTypeKit in (235,343) "; else str = " spK.FK_IdTypeKit = 343 ";
                if (chB_235Kit.Checked) str = " FK_IdTypeKit in (235,343)"; else str = " FK_IdTypeKit = 343 ";
                C_DataBase DB = new C_DataBase(C_Gper.ConnStrDispetcher2);
                /*string sql = "SELECT ROW_NUMBER() OVER(Order by spK.NameProduct) as Row, spK.NameProduct as product,sum(spK.minquantity * OD.AmountDetails) as minquantity" + "\n" +
                             "FROM [Dispetcher2].[dbo].[OrdersDetails] OD " + "\n" +
                             "Inner Join Sp_Details spD On spD.PK_IdDetail = OD.FK_IdDetail " + "\n" +
                             "Inner Join Sp_Kit spK On spK.IdLoodsmanParent = spD.IdLoodsman " + "\n" +
                             "Where OD.FK_IdOrder in (" + All_PK_IdOrder + ") and spD.FK_IdTypeDetail in (232,346) and " + str + "\n" +
                             "Group by spK.NameProduct ";// +"\n" +*/
                             //"Order by product";
                string sql = "Select ROW_NUMBER() OVER(Order by PK_IdKit) as Row,NameProduct,Sum(Minquantity) as Minquantity,Sum(AmountKit) as AmountKit,PK_IdKit as id,IdLoodsman as Idloodsman1C,FK_1С_IdKit" + "\n" +
                             "From vwKit_1C" + "\n" +
                             "Where PK_IdOrder in (" + All_PK_IdOrder + ") and " + str + "\n" +
                             "Group by PK_IdKit,NameProduct, IdLoodsman, FK_1С_IdKit";
                DB.Select_DT(ref DT_Kit, sql);
            }
        }

        // Старая версия работает медленно
        //private void LoadDataOfLoodsman(int PK_IdOrder, string OrderNum)
        //{
        //    //Загружаем список узлов и сборок
        //    if (PK_IdOrder > 0)
        //    {


        //        C_DataBase DB = new C_DataBase(C_Gper.ConnStrDispetcher2);
        //        string sql = "SELECT OD.Position,spD.IdLoodsman,spD.ShcmDetail,OD.AmountDetails" + "\n" +
        //                     "FROM [Dispetcher2].[dbo].[OrdersDetails] OD" + "\n" +
        //                     "Inner Join Sp_Details spD On spD.PK_IdDetail = OD.FK_IdDetail" + "\n" +
        //                     "Where OD.FK_IdOrder = " + PK_IdOrder + " and spD.FK_IdTypeDetail in (232,346)";

        //        DB.Select_DT(ref DT_Details, sql);
        //    }
        //    if (DT_Details.Rows.Count == 0) MessageBox.Show("В заказе отсутствуют позиции с типом \"сборка\".", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //    else
        //    {
        //        long IdLoodsman = 0;
        //        string Position = "", ShcmDetail = "", AmountDetails = "";
        //        DataTable DT = new DataTable();
        //        string str = "";
        //        if (chB_235Kit.Checked) str = " v.idtype in (235,343) "; else str = " v.idtype = 343 ";
        //        foreach (DataRow rowDT in DT_Details.Rows)
        //        {
        //            Position = rowDT.ItemArray[0].ToString().Trim();
        //            ShcmDetail = rowDT.ItemArray[2].ToString().Trim();
        //            AmountDetails = rowDT.ItemArray[3].ToString().Trim();
        //            if (long.TryParse(rowDT.ItemArray[1].ToString().Trim(), out IdLoodsman))
        //            {
        //                C_DataBase DB = new C_DataBase(C_Gper.ConStr_Loodsman);
        //                string sql = "Select '" + Position + "' as Position,'" + ShcmDetail + "' as ShcmDetail, '" + AmountDetails + "' as AmountDetails,r.minquantity *" + AmountDetails + " as minquantity,v.idtype,v.product as NameProduct,v.id,'" + OrderNum + "' as OrderNum" + "\n" +
        //                      "from НИИПМ.dbo.rvwRelations r" + "\n" +
        //                      "left join НИИПМ.dbo.rvwVersions v on v.id=r.idchild" + "\n" +
        //                    //"Where v.idtype in (235,343) and r.idparent = " + IdLoodsman;
        //                      "Where " + str + " and r.idparent = " + IdLoodsman;
        //                //"Where v.idtype in (235,343) and r.idparent = " + 311341;//IdLoodsman;
        //                DB.Select_DT(ref DT, sql);
        //                DT_Kit.Merge(DT, true);
        //            }
        //        }
        //    }
        //}

        // Новая версия работает быстрее в 10 раз
        void LoadDataOfLoodsman(int PK_IdOrder, string OrderNum)
        {
            C_DataBase DB = new C_DataBase(C_Gper.ConnStrDispetcher2);
            DT_Details = DB.GetOrderDetail(PK_IdOrder);
            if (DT_Details.Rows.Count == 0)
            {
                MessageBox.Show("В заказе отсутствуют позиции с типом \"сборка\".", "Внимание!!!", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }
            long IdLoodsman = 0;
            string Position = "", ShcmDetail = "", AmountDetails = "";
            bool standard = chB_235Kit.Checked;

            foreach (DataRow rowDT in DT_Details.Rows)
            {
                Position = rowDT.ItemArray[0].ToString().Trim();
                ShcmDetail = rowDT.ItemArray[2].ToString().Trim();
                AmountDetails = rowDT.ItemArray[3].ToString().Trim();
                if (long.TryParse(rowDT.ItemArray[1].ToString().Trim(), out IdLoodsman))
                {
                    DataTable DT = DB.GetDetailKit(Position, ShcmDetail, AmountDetails, OrderNum,
                        IdLoodsman, standard);
                    DT_Kit.Merge(DT, true);
                }
            }
        }

        private void btn_ExpKitToExcel_Click(object sender, EventArgs e)
        {
            //Экспортируем данные в Excel
            if (DT_Kit.Rows.Count == 0) MessageBox.Show("Таблица комплектации пуста.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                if (dGV_Orders.CurrentRow != null)
                {
                    string All_OrderNum = ""; byte numrows = 1, ch = 2;
                    for (int i = dGV_Orders.SelectedRows.Count - 1; i >= 0; --i)
                    {
                        DataGridViewRow dgrv = dGV_Orders.SelectedRows[i];
                        if (All_OrderNum == "") All_OrderNum = dgrv.Cells[1].Value.ToString();
                        else
                        {
                            if (ch % 11 == 0)
                            //if (ch == 7)
                            {
                                All_OrderNum += ", " + dgrv.Cells[1].Value.ToString() + Environment.NewLine;
                                numrows++;
                                //ch = 0;
                            }
                            else All_OrderNum += ", " + dgrv.Cells[1].Value.ToString();
                            ch++;
                        }
                    }
                    C_Reports ExpKit = new C_Reports();
                    //dGV_Orders.SelectedRows.Count
                    if (chB_Summary.Checked)
                        ExpKit.ExpKitToExcelSvod(All_OrderNum, DT_Kit, numrows);
                    else
                    ExpKit.ExpKitToExcel(All_OrderNum, DT_Kit, numrows, dGV_Orders.SelectedRows.Count);
                    


                    if (!ExpKit.RepErrors) MessageBox.Show("Формирование отчета завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    

                }
        }



        #region Разное
        private void tB_ShcmDetail_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ',') e.KeyChar = '.';
        }
        #endregion

        private void tB_ShcmDetail_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.Enter & tB_OrderNumInfo.Text.Trim() != "" & tB_ShcmDetail.Text.Trim().Length > 0 & dGV_Orders.CurrentRow != null)
            {
                CurrencyManager cmgr = (CurrencyManager)dGV_Orders.BindingContext[dGV_Orders.DataSource, dGV_Orders.DataMember];
                DataRow row = ((DataRowView)cmgr.Current).Row;
                int FK_IdOrder = Convert.ToInt32(row["PK_IdOrder"]);
                C_Details.SelectAllDetailsLikeSHCM(FK_IdOrder, tB_ShcmDetail.Text.Trim(), ref DT_Details);//Load Details
                if (dGV_Details.Rows.Count > 0) dGV_Details.Select();
            }*/
        }

        private void tB_OrderNum_TextChanged(object sender, EventArgs e)
        {
            BS_Orders.Filter = "OrderNum like '%" + tB_OrderNum.Text.ToString().Trim() + "%'";
        }

        private void tB_ShcmDetail_TextChanged(object sender, EventArgs e)
        {
            FilterOrderDetails2();
        }

        private void tB_NameKit_TextChanged(object sender, EventArgs e)
        {
            FilterOrderDetails2();
        }

        private void tB_IdLoodsmanKit_TextChanged(object sender, EventArgs e)
        {
            FilterOrderDetails2();
        }

        //private void FilterOrderDetails()
        //{
        //    if (tB_ShcmDetail.Text.Trim().Length > 0) BS_Kit.Filter = " ShcmDetail like '%" + tB_ShcmDetail.Text.ToString().Trim() + "%'";
        //    else
        //        if (tB_NameKit.Text.Trim().Length > 0) BS_Kit.Filter = " NameProduct like '%" + tB_NameKit.Text.ToString().Trim() + "%'";
        //        else
        //            if (tB_IdLoodsmanKit.Text.Trim().Length > 0) BS_Kit.Filter = "id = " + tB_IdLoodsmanKit.Text.ToString().Trim();
        //            else
        //                BS_Kit.Filter = "";
        //}

        private void FilterOrderDetails2()
        {
            List<string> filterParts = new List<string>();
            if (tB_ShcmDetail.Text.Trim().Length > 0)
                filterParts.Add($" ShcmDetail like '%{tB_ShcmDetail.Text.Trim()}%'");
            if (tB_NameKit.Text.Trim().Length > 0)
                filterParts.Add($" NameProduct like '%{tB_NameKit.Text.Trim()}%'");
            if (tB_IdLoodsmanKit.Text.Trim().Length > 0)
                filterParts.Add($" Convert([id], System.String) like '%{tB_IdLoodsmanKit.Text.Trim()}%'");
            string filter = string.Join(" AND ", filterParts);
            BS_Kit.Filter = filter;

        }

        private void dGV_Orders_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btn_OrderDetails.PerformClick();
                //tB_ShcmDetail.Focus();
            }*/
        }

        private void chB_Summary_CheckedChanged(object sender, EventArgs e)
        {
            DT_Kit.Clear();
            if (chB_Summary.Checked)
            {
                dGV_Kit.Columns["Col_Position"].Visible = false;
                dGV_Kit.Columns["Col_SHCM"].Visible = false;
                dGV_Kit.Columns["Col_Amount"].Visible = false;
                dGV_Kit.Columns["Col_Order"].Visible = false;
                dGV_Kit.Columns["Col_FactKit"].Visible = true;
                dGV_Kit.Columns["Col_1C_loodsman_IdKit"].Visible = true;
                dGV_Kit.Columns["Col_1C_IdKit"].Visible = true;
                
            }
            else
            {
                dGV_Kit.Columns["Col_Position"].Visible = true;
                dGV_Kit.Columns["Col_SHCM"].Visible = true;
                dGV_Kit.Columns["Col_Amount"].Visible = true;
                dGV_Kit.Columns["Col_Order"].Visible = true;
                dGV_Kit.Columns["Col_FactKit"].Visible = false;
                dGV_Kit.Columns["Col_1C_loodsman_IdKit"].Visible = false;
                dGV_Kit.Columns["Col_1C_IdKit"].Visible = false;
            }
        }

        private void radioBtn_Loodsman_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtn_Loodsman.Checked) dGV_Kit.Columns["Col_NumRow"].Visible = false; else dGV_Kit.Columns["Col_NumRow"].Visible = true;
            DT_Kit.Clear();
        }

        private void radioBtn_Disp_CheckedChanged(object sender, EventArgs e)
        {
            DT_Kit.Clear();
        }

        private void chB_235Kit_CheckedChanged(object sender, EventArgs e)
        {
            DT_Kit.Clear();
        }









    }
}
