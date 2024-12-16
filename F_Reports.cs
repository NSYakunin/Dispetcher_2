using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using Word =  Microsoft.Office.Interop.Word;

using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dispetcher2.Class;
using Dispetcher2.Controls;
using Dispetcher2.DataAccess;
using Dispetcher2.Models;
using System.Windows.Markup;
using DataTable = System.Data.DataTable;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using Microsoft.Office.Interop.Excel;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Windows.Forms.VisualStyles;
using Task = System.Threading.Tasks.Task;
using System.Windows.Documents;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Configuration;

namespace Dispetcher2
{
    public partial class F_Reports : Form
    {
        IConfig config;

        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Departments departments;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Users users;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Users usersOut;
        // Надо перенести функциональность в OrderRepository
        C_Orders orders;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Reports reports;

        DataTable Dt_SpDepartment = new DataTable();
        DataTable Dt_SpWorkers = new DataTable();
        DataTable Dt_SpWorkersOut = new DataTable();
        DataTable DT_Orders = new DataTable();
        BindingSource BS_Orders = new BindingSource();

        private List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
        List<string> selectedItems = new List<string>();

        public class ListItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }


        public F_Reports(IConfig config, IConverter converter)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр: config");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");

            this.config = config;

            departments = new C_Departments(config);
            users = new C_Users(config);
            usersOut = new C_Users(config);
            orders = new C_Orders(config);
            reports = new C_Reports(config);

            InitializeComponent();
            if (config.SelectedReportMode == ReportMode.ОтчетНаряд
                || config.SelectedReportMode == ReportMode.ДвижениеДеталей
                || config.SelectedReportMode == ReportMode.ОтчетВыполненным
                || config.SelectedReportMode == ReportMode.Гальваника
                || config.SelectedReportMode == ReportMode.ОтчетОТК)
            {
                DT_Orders.Columns.Add("PK_IdOrder", typeof(int));
                DT_Orders.Columns.Add("OrderNum", typeof(string));
                DT_Orders.Columns.Add("OrderName", typeof(string));
            }
        }

        private void F_Reports_Load(object sender, EventArgs e)
        {
            //dGVGalvan.AutoGenerateColumns = false;
            if (config.SelectedReportMode == ReportMode.ОтчетНаряд)//Отчёт-наряд по выполненным операциям
            {
                myTabC_Reports.SelectedTab = tPageRep3;
                departments.Select_Departments(ref Dt_SpDepartment);
                cB_rep3Department.DataSource = Dt_SpDepartment;
                cB_rep3Department.DisplayMember = "Department";
                cB_rep3Department.ValueMember = "PK_IdDepartment";
                cB_rep3Department.SelectedIndex = -1;

                users.Select_PkLoginOnlyWorker(ref Dt_SpWorkers);
                cB_rep3Workers.DataSource = Dt_SpWorkers;
                cB_rep3Workers.DisplayMember = "PK_Login";
                cB_rep3Workers.ValueMember = "PK_Login";
                cB_rep3Workers.SelectedIndex = -1;


                usersOut.Select_PkLoginOnlyWorker(ref Dt_SpWorkersOut);

                for (int i = 0; i < Dt_SpWorkersOut.Rows.Count; i++)
                {
                    cLB_rep3Workers.Items.Add(Dt_SpWorkersOut.Rows[i][0]);
                }
                cLB_rep3Workers.DisplayMember = "Name";
                cLB_rep3Workers.ValueMember = "ID";
                cLB_rep3Workers.CheckOnClick = true;
            }
            if (config.SelectedReportMode == ReportMode.ДвижениеДеталей)//Движение деталей
            {

                myTabC_Reports.SelectedTab = tPageRep6;
                dGV_Orders.AutoGenerateColumns = false;
                dGV_Orders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dGV_Orders.RowsDefaultCellStyle.BackColor = SystemColors.Info;
                BS_Orders.DataSource = DT_Orders;
                dGV_Orders.DataSource = BS_Orders;
                dGV_Orders.Columns["Col_OrderNum"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
                //Bindings
                tB_OrderName.DataBindings.Add("Text", BS_Orders, "OrderName", false, DataSourceUpdateMode.OnPropertyChanged);
                tB_OrderNumInfo.DataBindings.Add("Text", BS_Orders, "OrderNum", false, DataSourceUpdateMode.OnPropertyChanged);
                orders.SelectOrdersData(2, ref DT_Orders);//2-opened
            }
            if (config.SelectedReportMode == ReportMode.ОперацииВыполненныеРабочим)//Операции выполненные рабочим по заказам (форма №17)
            {
                myTabC_Reports.SelectedTab = tPageRep117;
                users.Select_PkLoginOnlyWorker(ref Dt_SpWorkers);
                BS_Orders.DataSource = Dt_SpWorkers;
                cB_rep117Workers.DataSource = BS_Orders;
                cB_rep117Workers.DisplayMember = "PK_Login";
                cB_rep117Workers.ValueMember = "PK_Login";
                //tB_rep117Department.DataBindings.Add("Text", BS_Orders, "Department", false, DataSourceUpdateMode.Never);
                cB_rep117Workers.SelectedIndex = -1;
                //cB_rep117Workers.SelectedItem = null;
            }

            if (config.SelectedReportMode == ReportMode.ОтчетВыполненным)//Движение деталей
            {
                myTabC_Reports.SelectedTab = tPageRep7;
                dGV_OrdersRep7.AutoGenerateColumns = false;
                dGV_OrdersRep7.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dGV_OrdersRep7.RowsDefaultCellStyle.BackColor = SystemColors.Info;
                BS_Orders.DataSource = DT_Orders;
                dGV_OrdersRep7.DataSource = BS_Orders;
                dGV_OrdersRep7.Columns["Col_OrderNumRep7"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
                //Bindings
                tB_OrderNameRep7.DataBindings.Add("Text", BS_Orders, "OrderName", false, DataSourceUpdateMode.OnPropertyChanged);
                tB_OrderNumInfoRep7.DataBindings.Add("Text", BS_Orders, "OrderNum", false, DataSourceUpdateMode.OnPropertyChanged);
                orders.SelectOrdersData(2, ref DT_Orders);//2-opened
            }

            if (config.SelectedReportMode == ReportMode.Гальваника)
            {
                myTabC_Reports.SelectedTab = tabPageGalvan;
                loaddGVGalvan(galvanStart.Value.Date, galvanEnd.Value.Date);
            }

            if (config.SelectedReportMode == ReportMode.ОтчетОТК)
            {
                myTabC_Reports.SelectedTab = tabPageRepOTC;
                dGV_OrdersOTK.AutoGenerateColumns = false;
                dGV_OrdersOTK.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dGV_OrdersOTK.RowsDefaultCellStyle.BackColor = SystemColors.Info;
                BS_Orders.DataSource = DT_Orders;
                dGV_OrdersOTK.DataSource = BS_Orders;
                dGV_OrdersOTK.Columns["Col_OrderNumOTK"].DataPropertyName = DT_Orders.Columns["OrderNum"].ToString();
                //Bindings
                tB_OrderNameOTK.DataBindings.Add("Text", BS_Orders, "OrderName", false, DataSourceUpdateMode.OnPropertyChanged);
                tB_OrderNumInfoOTK.DataBindings.Add("Text", BS_Orders, "OrderNum", false, DataSourceUpdateMode.OnPropertyChanged);
                orders.SelectOrdersData(2, ref DT_Orders);//2-opened
            }

        }

        #region rep3 C_Gper.NameReport == 3 Отчёт-наряд по выполненным операциям
        private void chB_rep3Department_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_rep3Department.Checked)
            {
                cB_rep3Department.Enabled = false;
                cB_rep3Department.SelectedIndex = -1;
            }
            else cB_rep3Department.Enabled = true;
            if (!chB_rep3Department.Checked & !chB_rep3Workers.Checked) chB_rep3Workers.Checked = true;
        }

        private void chB_rep3Workers_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_rep3Workers.Checked)
            {
                cB_rep3Workers.Enabled = false;
                cB_rep3Workers.SelectedIndex = -1;
            }
            else cB_rep3Workers.Enabled = true;
            if (!chB_rep3Department.Checked & !chB_rep3Workers.Checked) chB_rep3Department.Checked = true;
        }

        private void dTP_rep3Start_ValueChanged(object sender, EventArgs e)
        {
            if (dTP_rep3Start.Value > dTP_rep3End.Value) dTP_rep3End.Value = dTP_rep3Start.Value;
        }

        private void dTP_rep3End_ValueChanged(object sender, EventArgs e)
        {
            if (dTP_rep3Start.Value > dTP_rep3End.Value) dTP_rep3End.Value = dTP_rep3Start.Value;
        }

        private void btn_rep3Create_Click(object sender, EventArgs e)//C_Gper.NameReport == 3) Отчёт-наряд по выполненным операциям
        {
            string parameterValue = string.Join(",", selectedItems);
            string loginWorker;
            int IdCeh;
            bool flagDays;
            int cWorkDays = 0;
            bool koop;
            if (cB_rep3Workers.SelectedIndex != -1) loginWorker = cB_rep3Workers.SelectedValue.ToString(); else loginWorker = "";
            if (cB_rep3Department.SelectedIndex != -1) IdCeh = Convert.ToInt32(cB_rep3Department.SelectedValue); else IdCeh = -1;
            if (chB_rep3Days.Checked) flagDays = true; else flagDays = false;
            if (cBKoop.Checked) koop = true; else koop = false;
            
            int PlanHours = reports.NormHoursPlan(dTP_rep3Start.Value.Date, dTP_rep3End.Value.Date, ref cWorkDays);
            if (PlanHours == 0)
                MessageBox.Show("Не указаны данные производственного календаря.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                reports.rep3(dTP_rep3Start.Value.Date, dTP_rep3End.Value.Date, loginWorker, IdCeh, flagDays, PlanHours, cWorkDays, koop, parameterValue);
        }
        #endregion

        #region rep6 C_Gper.NameReport == 6 отчёт "Движение деталей"

        private void tB_OrderNum_TextChanged(object sender, EventArgs e)//C_Gper.NameReport == 6) Движение деталей
        {
            BS_Orders.Filter = " OrderNum like '%" + tB_OrderNum.Text.ToString().Trim() + "%'";
        }

        private void btn_rep6Create_Click(object sender, EventArgs e)//C_Gper.NameReport == 6) Движение деталей
        {
            CurrencyManager cmgr = (CurrencyManager)this.dGV_Orders.BindingContext[this.dGV_Orders.DataSource, dGV_Orders.DataMember];
            DataRow row = ((DataRowView)cmgr.Current).Row;
            int PK_IdOrder = Convert.ToInt32(row["PK_IdOrder"]);
            
            //PK_IdOrder = 102;
            //tB_OrderNumInfo.Text = "20554801";
            reports.rep6(PK_IdOrder,tB_OrderNumInfo.Text.Trim(), tB_OrderName.Text.Trim());
        }
        #endregion

        #region rep 117 C_Gper.NameReport == 117  Операции выполненные рабочим по заказам (форма №17)

        private void dTP_rep117Start_ValueChanged(object sender, EventArgs e)
        {
            if (dTP_rep117Start.Value > dTP_rep117End.Value) dTP_rep117End.Value = dTP_rep117Start.Value;
        }

        private void dTP_rep117End_ValueChanged(object sender, EventArgs e)
        {
            if (dTP_rep117Start.Value > dTP_rep117End.Value) dTP_rep117End.Value = dTP_rep117Start.Value;
        }

        private void btn_rep117Create_Click(object sender, EventArgs e)//C_Gper.NameReport == 117  Операции выполненные рабочим по заказам (форма №17)
        {
            string loginWorker;
            int cWorkDays = 0;
            if (cB_rep117Workers.SelectedIndex != -1) loginWorker = cB_rep117Workers.SelectedValue.ToString(); else loginWorker = "";
            if (loginWorker == "")
                MessageBox.Show("Не выбран исполнитель.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                
                int PlanHours = reports.NormHoursPlan(dTP_rep117Start.Value.Date, dTP_rep117End.Value.Date, ref cWorkDays);
                if (PlanHours == 0)
                    MessageBox.Show("Не указаны данные производственного календаря.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                    reports.Form17(dTP_rep117Start.Value.Date, dTP_rep117End.Value.Date, loginWorker, tB_rep117Department.Text.Trim(), PlanHours);
            }
        }

        private void cB_rep117Workers_TextChanged(object sender, EventArgs e)
        {
            if (cB_rep117Workers.SelectedValue == null)
            {
                tB_rep117Department.DataBindings.Clear();
                tB_rep117Department.Text = "";
            }
            else
            {
                tB_rep117Department.DataBindings.Clear();
                tB_rep117Department.DataBindings.Add("Text", BS_Orders, "Department", false, DataSourceUpdateMode.Never);

            }
            //tB_rep117Department.Text = cB_rep117Workers.SelectedIndex.ToString() + "|" + cB_rep117Workers.SelectedItem.ToString() + "|" + cB_rep117Workers.SelectedValue.ToString(); 
        }

        #endregion

        #region rep7 C_Gper.NameReport == 7 "Отчет по выполненным операциям"

        private void tB_OrderNumRep7_TextChanged(object sender, EventArgs e)
        {
            BS_Orders.Filter = " OrderNum like '%" + tB_OrderNumRep7.Text.ToString().Trim() + "%'";
        }

        private void chB_rep7AllTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chB_rep7AllTime.Checked)
            {
                dTP_rep7Start.Visible = false;
                dTP_rep7End.Visible = false;
            }
            else
            {
                dTP_rep7Start.Visible = true;
                dTP_rep7End.Visible = true;
            }
        }

        private void btn_rep7Create_Click(object sender, EventArgs e)
        {
            /*CurrencyManager cmgr = (CurrencyManager)this.dGV_Orders.BindingContext[this.dGV_Orders.DataSource, dGV_Orders.DataMember];
            DataRow row = ((DataRowView)cmgr.Current).Row;
            int PK_IdOrder = Convert.ToInt32(row["PK_IdOrder"]);*/
            
            //PK_IdOrder = 138;
            //tB_OrderNumInfo.Text = "20554801";
            //Report7.Rep7(PK_IdOrder, tB_OrderNumInfo.Text.Trim(), tB_OrderName.Text.Trim());
            reports.Rep7(dTP_rep7Start.Value.Date, dTP_rep7End.Value.Date, chB_rep7AllTime.Checked, chB_rep7AllOrders.Checked, tB_OrderNumInfoRep7.Text.Trim(), tB_OrderNameRep7.Text.Trim());
            if (!reports.RepErrors) MessageBox.Show("Отчёт сформирован.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dTP_rep7Start_ValueChanged(object sender, EventArgs e)
        {
            if (dTP_rep7Start.Value > dTP_rep7End.Value) dTP_rep7End.Value = dTP_rep7Start.Value;
        }

        private void dTP_rep7End_ValueChanged(object sender, EventArgs e)
        {
            if (dTP_rep7Start.Value > dTP_rep7End.Value) dTP_rep7End.Value = dTP_rep7Start.Value;
        }

        #endregion

        private void loaddGVGalvan(DateTime DateStart, DateTime DateEnd)
        {
            dataNowLabel.Text = DateTime.Now.ToShortDateString();

            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;

                    cmd.CommandText = $"SELECT OrderNum, OD.Position, SD.ShcmDetail Обозначение_ЩЦМ, SD.NameDetail AS Наименование, " +
                                               $"FO.AmountDetails AS Колличество, SO.NameOperation AS Операция, DateFactOper " +
                                      $"FROM[Dispetcher2].[dbo].[Orders] " +
                                      $"INNER JOIN OrdersDetails OD " +
                                      $"    ON OD.FK_IdOrder = Orders.PK_IdOrder " +
                                      $"INNER JOIN FactOperation FO " +
                                      $"    ON OD.PK_IdOrderDetail = FO.FK_IdOrderDetail " +
                                      $"INNER JOIN Sp_Operations SO " +
                                      $"    ON FO.FK_IdOperation = SO.PK_IdOperation " +
                                      $"INNER JOIN Sp_Details SD " +
                                      $"    ON OD.FK_IdDetail = SD.PK_IdDetail " +
                                      $"WHERE SO.NameOperation LIKE '%Гальв%' AND DateFactOper >= @DateStart AND DateFactOper <= @DateEnd " +
                                      $"ORDER BY ShcmDetail, DateFactOper";

                    cmd.Parameters.AddWithValue("@DateStart", DateStart);
                    cmd.Parameters.AddWithValue("@DateEnd", DateEnd);

                    cmd.Connection = con;
                    cmd.Connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<string[]> data = new List<string[]>();
                    int pos = 1;

                    while (reader.Read())
                    {
                        data.Add(new string[8]);

                        data[data.Count - 1][0] = pos++.ToString();
                        data[data.Count - 1][1] = reader[0].ToString();
                        data[data.Count - 1][2] = reader[1].ToString();
                        data[data.Count - 1][3] = reader[2].ToString();
                        data[data.Count - 1][4] = reader[3].ToString();
                        data[data.Count - 1][5] = reader[4].ToString();
                        data[data.Count - 1][6] = reader[5].ToString();
                        data[data.Count - 1][7] = reader[6].ToString().Replace(" 0:00:00", "");
                    }
                    reader.Close();

                    foreach (string[] s in data)
                        dGVGalvan.Rows.Add(s);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dGVGalvan.Rows.Clear();
            loaddGVGalvan(galvanStart.Value.Date, galvanEnd.Value.Date);
        }

        private void excelGalvan_Click(object sender, EventArgs e)
        {
            if (dGVGalvan.Rows.Count < 1)
            {
                MessageBox.Show("Нет данных для выгрузки в Excel.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            DateTime DateStart = galvanStart.Value;
            DateTime DateEnd = galvanEnd.Value;
            Excel.Application ExcelApp = new Excel.Application() { Visible = false };
            ExcelApp.Workbooks.Add(1);
            Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);

            //ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
            ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
            ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
            ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
            ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
            ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
            ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);

            ExcelWorkSheet.Columns[1].ColumnWidth = 4;
            ExcelWorkSheet.Columns[2].ColumnWidth = 10;
            ExcelWorkSheet.Columns[3].ColumnWidth = 5;
            ExcelWorkSheet.Columns[4].ColumnWidth = 20;
            ExcelWorkSheet.Columns[5].ColumnWidth = 25;
            ExcelWorkSheet.Columns[6].ColumnWidth = 5;
            ExcelWorkSheet.Columns[7].ColumnWidth = 26;
            ExcelWorkSheet.Columns[8].ColumnWidth = 13;
            //ExcelWorkSheet.Columns[9].ColumnWidth = 20;


            ExcelWorkSheet.Cells[1, 1].Value2 = $"Акт приёма-передачи №{numActPeredachi.Text} от {dataNowLabel.Text}";
            ExcelWorkSheet.Cells[1, 1].HorizontalAlignment = Excel.Constants.xlCenter;
            ExcelWorkSheet.Cells[1, 1].Font.Bold = 1;
            ExcelWorkSheet.Cells[1, 1].Font.Size = 11;
            ExcelWorkSheet.Cells[2, 1].Value2 = "с " + DateStart.ToShortDateString() + " по " + DateEnd.ToShortDateString();
            ExcelWorkSheet.Cells[2, 1].HorizontalAlignment = Excel.Constants.xlCenter;
            ExcelWorkSheet.get_Range("A1:H1").Merge();
            ExcelWorkSheet.get_Range("A2:H2").Merge();
            ExcelWorkSheet.Cells[4, 1] = "№";
            ExcelWorkSheet.Cells[4, 2] = "Заказ";
            ExcelWorkSheet.Cells[4, 3] = "Поз.";
            ExcelWorkSheet.Cells[4, 4] = "Обозначение ЩЦМ";
            ExcelWorkSheet.Cells[4, 5] = "Наименование детали";
            ExcelWorkSheet.Cells[4, 6] = "Кол-во";
            ExcelWorkSheet.Cells[4, 7] = "Покрытие";
            ExcelWorkSheet.Cells[4, 8] = "Фактическая дата";
            //ExcelWorkSheet.Cells[4, 9] = "Примечание";
            ExcelWorkSheet.get_Range("A4:H4").Font.Bold = 1;
            ExcelWorkSheet.get_Range("A4:H4").WrapText = true;
            ExcelWorkSheet.get_Range("A4:H4").Font.Size = 11;
            ExcelWorkSheet.get_Range("A4:H4").HorizontalAlignment = Excel.Constants.xlCenter;
            ExcelWorkSheet.get_Range("A4:H4").VerticalAlignment = Excel.Constants.xlCenter;



            progressBar1.Maximum = dGVGalvan.Rows.Count * 8;

            for (int row = 0; row < dGVGalvan.Rows.Count; row++)
            {
                for (int col = 0; col < dGVGalvan.Columns.Count - 2; col++)
                {
                    ExcelWorkSheet.Cells[row + 5, col + 1] = dGVGalvan.Rows[row].Cells[col].Value.ToString().Trim();                    
                    ExcelWorkSheet.Cells[row + 5, col + 1].WrapText = true;
                    ExcelWorkSheet.Cells[row + 5, col + 1].Font.Size = 11;
                    if (col == 1 || col == 6)
                    {

                        ExcelWorkSheet.Cells[row + 5, col + 1].HorizontalAlignment = Excel.Constants.xlLeft;
                        ExcelWorkSheet.Cells[row + 5, col + 1].VerticalAlignment = Excel.Constants.xlCenter;
                        if (dGVGalvan.Rows[row].Cells[col].Value.ToString().Contains("/"))
                        {
                            ExcelWorkSheet.Cells[row + 5, col + 1] = dGVGalvan.Rows[row].Cells[col].Value.ToString().Replace("/", "/\n");
                        }
                    }
                    else
                    {
                        ExcelWorkSheet.Cells[row + 5, col + 1].HorizontalAlignment = Excel.Constants.xlCenter;
                        ExcelWorkSheet.Cells[row + 5, col + 1].VerticalAlignment = Excel.Constants.xlCenter;
                    }

                    progressBar1.Value++;
                    //ExcelWorkSheet.Cells[row + 5, col + 1].Font.Bold = 1;
                }
            }



            ExcelWorkSheet.get_Range("A4", "H" + (dGVGalvan.Rows.Count + 4)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            ExcelApp.Visible = true;
            progressBar1.Value = 0;
            MessageBox.Show("Формирование отчета завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ExcelApp.Quit();
        }


        private async Task excelGalvanClickAsync(object sender, EventArgs e)
        {
            if (dGVGalvan.Rows.Count < 1)
            {
                MessageBox.Show("Нет данных для выгрузки в Excel.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            DateTime DateStart = galvanStart.Value;
            DateTime DateEnd = galvanEnd.Value;
            Excel.Application ExcelApp = new Excel.Application() { Visible = false };
            ExcelApp.Workbooks.Add(1);
            Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);

            //ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
            ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
            ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
            ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
            ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
            ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
            ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);

            ExcelWorkSheet.Columns[1].ColumnWidth = 3;
            ExcelWorkSheet.Columns[2].ColumnWidth = 9;
            ExcelWorkSheet.Columns[3].ColumnWidth = 5;
            ExcelWorkSheet.Columns[4].ColumnWidth = 20;
            ExcelWorkSheet.Columns[5].ColumnWidth = 25;
            ExcelWorkSheet.Columns[6].ColumnWidth = 5;
            ExcelWorkSheet.Columns[7].ColumnWidth = 26;
            ExcelWorkSheet.Columns[8].ColumnWidth = 12;
            ExcelWorkSheet.Columns[9].ColumnWidth = 20;


            ExcelWorkSheet.Cells[1, 1].Value2 = $"Акт приёма-передачи №{numActPeredachi.Text} от {dataNowLabel.Text}";
            ExcelWorkSheet.Cells[1, 1].HorizontalAlignment = Excel.Constants.xlCenter;
            ExcelWorkSheet.Cells[1, 1].Font.Bold = 1;
            ExcelWorkSheet.Cells[1, 1].Font.Size = 11;
            ExcelWorkSheet.Cells[2, 1].Value2 = "с " + DateStart.ToShortDateString() + " по " + DateEnd.ToShortDateString();
            ExcelWorkSheet.Cells[2, 1].HorizontalAlignment = Excel.Constants.xlCenter;
            ExcelWorkSheet.get_Range("A1:I1").Merge();
            ExcelWorkSheet.get_Range("A2:I2").Merge();
            ExcelWorkSheet.Cells[4, 1] = "№";
            ExcelWorkSheet.Cells[4, 2] = "Заказ";
            ExcelWorkSheet.Cells[4, 3] = "Поз.";
            ExcelWorkSheet.Cells[4, 4] = "Обозначение ЩЦМ";
            ExcelWorkSheet.Cells[4, 5] = "Наименование детали";
            ExcelWorkSheet.Cells[4, 6] = "Кол-во";
            ExcelWorkSheet.Cells[4, 7] = "Покрытие";
            ExcelWorkSheet.Cells[4, 8] = "Фактическая дата";
            ExcelWorkSheet.Cells[4, 9] = "Примечание";
            ExcelWorkSheet.get_Range("A4:I4").Font.Bold = 1;
            ExcelWorkSheet.get_Range("A4:I4").WrapText = true;
            ExcelWorkSheet.get_Range("A4:I4").Font.Size = 11;
            ExcelWorkSheet.get_Range("A4:I4").HorizontalAlignment = Excel.Constants.xlCenter;
            ExcelWorkSheet.get_Range("A4:I4").VerticalAlignment = Excel.Constants.xlCenter;

            progressBar1.Maximum = dGVGalvan.Rows.Count * 8;
            await Task.Run(() =>
            {
                for (int row = 0; row < dGVGalvan.Rows.Count; row++)
                {
                    for (int col = 0; col < dGVGalvan.Columns.Count - 2; col++)
                    {
                        ExcelWorkSheet.Cells[row + 5, col + 1] = dGVGalvan.Rows[row].Cells[col].Value.ToString().Trim();
                        ExcelWorkSheet.Cells[row + 5, col + 1].WrapText = true;
                        ExcelWorkSheet.Cells[row + 5, col + 1].Font.Size = 11;
                        if (col == 1 || col == 6) ExcelWorkSheet.Cells[row + 5, col + 1].HorizontalAlignment = Excel.Constants.xlLeft;
                        else
                        {
                            ExcelWorkSheet.Cells[row + 5, col + 1].HorizontalAlignment = Excel.Constants.xlCenter;
                            ExcelWorkSheet.Cells[row + 5, col + 1].VerticalAlignment = Excel.Constants.xlCenter;
                        }

                        progressBar1.Invoke(new System.Action(() =>
                        {
                            progressBar1.Value += 1;
                        }));
                    }
                }
            });

            ExcelWorkSheet.get_Range("A4", "I" + (dGVGalvan.Rows.Count + 4)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            ExcelApp.Visible = true;
            progressBar1.Value = 0;
            MessageBox.Show("Формирование отчета завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ExcelApp.Quit();
        }

        private void bTNBirki_Click(object sender, EventArgs e)
        {

            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одну позицию!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Create a new Excel application
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;
            // Add a new workbook
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            // Get the active sheet
            Excel.Worksheet sheet = workbook.ActiveSheet;
            // Set the sheet orientation to landscape
            sheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
            // Set the sheet paper size to A4
            sheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
            sheet.PageSetup.LeftMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.RightMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.TopMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.BottomMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.HeaderMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.FooterMargin = excelApp.CentimetersToPoints(0);
            sheet.StandardWidth = 9;

            int startRow = 1;
            int startRowList = 0;
            int countList = (selectedRows.Count <= 16 ? 1 : selectedRows.Count <= 32 ? 2 : 3);

            while(countList > 0)
            {
                for (int i = 1; i <= 4; i++)
                {
                    int endRow = startRow + 9;
                    // Draw the top border line for the block
                    sheet.Range[sheet.Cells[startRow, 1], sheet.Cells[startRow, 4]].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[startRow, 1], sheet.Cells[startRow, 4]].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                    // Draw the bottom border line for the block
                    sheet.Range[sheet.Cells[endRow, 1], sheet.Cells[endRow, 4]].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[endRow, 1], sheet.Cells[endRow, 4]].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                    // Draw the left border line for the block
                    sheet.Range[sheet.Cells[startRow, 1], sheet.Cells[endRow, 1]].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[startRow, 1], sheet.Cells[endRow, 1]].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                    // Draw the right border line for the block
                    sheet.Range[sheet.Cells[startRow, 4], sheet.Cells[endRow, 4]].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[startRow, 4], sheet.Cells[endRow, 4]].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;



                    // Draw the top border line for the block
                    sheet.Range[sheet.Cells[startRow, 5], sheet.Cells[startRow, 8]].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[startRow, 5], sheet.Cells[startRow, 8]].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                    // Draw the bottom border line for the block
                    sheet.Range[sheet.Cells[endRow, 5], sheet.Cells[endRow, 8]].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[endRow, 5], sheet.Cells[endRow, 8]].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;

                    // Draw the right border line for the block
                    sheet.Range[sheet.Cells[startRow, 8], sheet.Cells[endRow, 8]].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[startRow, 8], sheet.Cells[endRow, 8]].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                    //**************************

                    // Draw the top border line for the block
                    sheet.Range[sheet.Cells[startRow, 9], sheet.Cells[startRow, 12]].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[startRow, 9], sheet.Cells[startRow, 12]].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                    // Draw the bottom border line for the block
                    sheet.Range[sheet.Cells[endRow, 9], sheet.Cells[endRow, 12]].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[endRow, 9], sheet.Cells[endRow, 12]].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;

                    sheet.Range[sheet.Cells[startRow, 12], sheet.Cells[endRow, 12]].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[startRow, 12], sheet.Cells[endRow, 12]].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;
                    //***************************

                    // Draw the top border line for the block
                    sheet.Range[sheet.Cells[startRow, 13], sheet.Cells[startRow, 16]].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[startRow, 13], sheet.Cells[startRow, 16]].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                    // Draw the bottom border line for the block
                    sheet.Range[sheet.Cells[endRow, 13], sheet.Cells[endRow, 16]].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[endRow, 13], sheet.Cells[endRow, 16]].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;

                    sheet.Range[sheet.Cells[startRow, 16], sheet.Cells[endRow, 16]].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                    sheet.Range[sheet.Cells[startRow, 16], sheet.Cells[endRow, 16]].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;

                    sheet.get_Range($"A{startRow + 7}:D{startRow + 9}").Merge();
                    sheet.get_Range($"E{startRow + 7}:H{startRow + 9}").Merge();
                    sheet.get_Range($"I{startRow + 7}:L{startRow + 9}").Merge();
                    sheet.get_Range($"M{startRow + 7}:P{startRow + 9}").Merge();

                    //sheet.get_Range($"A{startRow + 8}:D{startRow + 8}").Merge();
                    //sheet.get_Range($"A{startRow + 8}:D{startRow + 9}").Merge();
                    //sheet.get_Range($"A{startRow + 8}:D{startRow + 9}").Merge();



                    int startPos = 1;
                    for (int j = 0; j < 4; j++)
                    {
                        sheet.Cells[startRow, startPos].Value = "ЗАКАЗ";
                        sheet.Cells[startRow, startPos].Font.Size = 10;
                        //sheet.Cells[startRow, startPos].Font.Bold = 1;
                        sheet.Cells[startRow, startPos + 1].Value = selectedRows[startRowList].Cells[1].Value;
                        sheet.Cells[startRow, startPos + 1].Font.Size = 11;
                        sheet.Cells[startRow, startPos + 1].Font.Bold = 1;

                        sheet.Cells[startRow + 1, startPos].Value = "НАИМЕНОВАНИЕ";
                        sheet.Cells[startRow + 1, startPos].Font.Size = 10;
                        //sheet.Cells[startRow + 1, startPos].Font.Bold = 1;

                        sheet.Cells[startRow + 2, startPos + 1].Value = selectedRows[startRowList].Cells[4].Value;
                        sheet.Cells[startRow + 2, startPos + 1].Font.Size = 11;
                        sheet.Cells[startRow + 2, startPos + 1].Font.Bold = 1;

                        sheet.Cells[startRow + 3, startPos].Value = "ЩЦМ";
                        sheet.Cells[startRow + 3, startPos].Font.Size = 10;
                        //sheet.Cells[startRow + 3, startPos].Font.Bold = 1;
                        sheet.Cells[startRow + 3, startPos + 1].Value = selectedRows[startRowList].Cells[3].Value;
                        sheet.Cells[startRow + 3, startPos + 1].Font.Size = 11;
                        sheet.Cells[startRow + 3, startPos + 1].Font.Bold = 1;

                        sheet.Cells[startRow + 4, startPos].Value = "КОЛ-ВО";
                        sheet.Cells[startRow + 4, startPos].Font.Size = 10;
                        //sheet.Cells[startRow + 4, startPos].Font.Bold = 1;
                        sheet.Cells[startRow + 4, startPos + 1].Value = selectedRows[startRowList].Cells[5].Value;
                        sheet.Cells[startRow + 4, startPos + 1].Font.Size = 11;
                        sheet.Cells[startRow + 4, startPos + 1].Font.Bold = 1;
                        sheet.Cells[startRow + 4, startPos + 1].HorizontalAlignment = Excel.Constants.xlLeft;

                        sheet.Cells[startRow + 4, startPos + 2].Value = "ПОЗ.";
                        sheet.Cells[startRow + 4, startPos + 2].Font.Size = 10;
                        sheet.Cells[startRow + 4, startPos + 2].HorizontalAlignment = Excel.Constants.xlRight;
                        //sheet.Cells[startRow + 4, startPos].Font.Bold = 1;
                        sheet.Cells[startRow + 4, startPos + 3].Value = selectedRows[startRowList].Cells[2].Value;
                        sheet.Cells[startRow + 4, startPos + 3].Font.Size = 11;
                        sheet.Cells[startRow + 4, startPos + 3].Font.Bold = 1;
                        sheet.Cells[startRow + 4, startPos + 3].HorizontalAlignment = Excel.Constants.xlLeft;
                        sheet.Cells[startRow + 4, startPos + 3].HorizontalAlignment = Excel.Constants.xlCenter;


                        sheet.Cells[startRow + 5, startPos].Value = "ДАТА";
                        sheet.Cells[startRow + 5, startPos].Font.Size = 10;
                        //sheet.Cells[startRow + 5, startPos].Font.Bold = 1;
                        sheet.Cells[startRow + 5, startPos + 1].Value = selectedRows[startRowList].Cells[7].Value;
                        sheet.Cells[startRow + 5, startPos + 1].Font.Size = 11;
                        sheet.Cells[startRow + 5, startPos + 1].Font.Bold = 1;

                        sheet.Cells[startRow + 6, startPos].Value = "ПОКРЫТИЕ";
                        sheet.Cells[startRow + 6, startPos].Font.Size = 10;
                       //sheet.Cells[startRow + 6, startPos].Font.Bold = 1;
                        sheet.Cells[startRow + 7, startPos].Value = selectedRows[startRowList].Cells[6].Value;
                        sheet.Cells[startRow + 7, startPos].Font.Size = 11;
                        sheet.Cells[startRow + 7, startPos].Font.Bold = 1;
                        sheet.Cells[startRow + 7, startPos].Font.Underline = true;
                        sheet.Cells[startRow + 7, startPos].HorizontalAlignment = Excel.Constants.xlCenter;
                        sheet.Cells[startRow + 7, startPos].VerticalAlignment = Excel.Constants.xlCenter;
                        sheet.Cells[startRow + 7, startPos].IndentLevel = 6;
                        sheet.Cells[startRow + 7, startPos].WrapText = true;

                        selectedRows[startRowList].DefaultCellStyle.BackColor = Color.Aquamarine;
                        startPos += 4;

                        startRowList++;
                        if (startRowList > selectedRows.Count - 1)
                        {
                            selectedRows.Clear();
                            return;
                        }
                    }
                    //**************************
                    startRow += 10;
                    endRow += 10;
                    startPos += 4;
                }

                //startRow += 3;
                countList--;
            }
        }

        private void dGVGalvan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            // Проверяем, что изменено значение в последнем столбце
            if (e.ColumnIndex == dGVGalvan.Columns.Count - 1 && e.RowIndex >= 0)
            {
                DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)dGVGalvan.Rows[e.RowIndex].Cells[e.ColumnIndex];

                bool isChecked = (bool)checkBoxCell.EditedFormattedValue;
                // Если чекбокс отмечен, добавляем строку в список
                if (isChecked)
                {
                    selectedRows.Add(dGVGalvan.Rows[e.RowIndex]);
                }
                // Если чекбокс снят, удаляем строку из списка
                else
                {
                    selectedRows.Remove(dGVGalvan.Rows[e.RowIndex]);
                }
            }
        }

        private async void btnAsync_Click(object sender, EventArgs e)
        {
            await excelGalvanClickAsync(sender, e);
        }

        //private void dGVGalvan_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
        //    if (e.RowIndex == dGVGalvan.RowCount - 1) e.AdvancedBorderStyle.Bottom = dGVGalvan.AdvancedCellBorderStyle.Bottom;
        //    if (e.RowIndex == 0) e.AdvancedBorderStyle.Top = dGVGalvan.AdvancedCellBorderStyle.Top;
        //    if (e.RowIndex < 1 || e.ColumnIndex < 0)
        //        return;
        //    if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
        //    {
        //        e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;

        //    }
        //    else
        //    {
        //        e.AdvancedBorderStyle.Top = dGVGalvan.AdvancedCellBorderStyle.Top;
        //    }
        //}

        //bool IsTheSameCellValue(int column, int row)
        //{
        //    DataGridViewCell cell1 = dGVGalvan[column, row];
        //    DataGridViewCell cell2 = dGVGalvan[column, row - 1];
        //    if (cell1.Value == null || cell2.Value == null)
        //    {
        //        return false;
        //    }
        //    return cell1.Value.ToString() == cell2.Value.ToString();
        //}

        //private void dGVGalvan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    if (e.RowIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 5 || e.ColumnIndex == 2)
        //        return;
        //    if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
        //    {
        //        e.Value = "";
        //        e.FormattingApplied = true;
        //    }
        //}

        private void exelGalvanNew_Click(object sender, EventArgs e)
        {
            if (dGVGalvan.Rows.Count < 1)
            {
                MessageBox.Show("Нет данных для выгрузки в Excel.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DateTime DateStart = galvanStart.Value;
            DateTime DateEnd = galvanEnd.Value;
            Excel.Application ExcelApp = new Excel.Application() { Visible = false };
            ExcelApp.Workbooks.Add(1);
            Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);

            //ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
            ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlPortrait;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
            ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
            ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
            ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
            ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
            ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);

            ExcelWorkSheet.Columns[1].ColumnWidth = 4;
            ExcelWorkSheet.Columns[2].ColumnWidth = 10;
            ExcelWorkSheet.Columns[3].ColumnWidth = 5;
            ExcelWorkSheet.Columns[4].ColumnWidth = 20;
            ExcelWorkSheet.Columns[5].ColumnWidth = 25;
            ExcelWorkSheet.Columns[6].ColumnWidth = 5;
            ExcelWorkSheet.Columns[7].ColumnWidth = 26;
            ExcelWorkSheet.Columns[8].ColumnWidth = 13;
            //ExcelWorkSheet.Columns[9].ColumnWidth = 20;


            ExcelWorkSheet.Cells[1, 1].Value2 = $"Акт приёма-передачи №{numActPeredachi.Text} от {dataNowLabel.Text}";
            ExcelWorkSheet.Cells[1, 1].HorizontalAlignment = Excel.Constants.xlCenter;
            ExcelWorkSheet.Cells[1, 1].Font.Bold = 1;
            ExcelWorkSheet.Cells[1, 1].Font.Size = 11;
            ExcelWorkSheet.Cells[2, 1].Value2 = "с " + DateStart.ToShortDateString() + " по " + DateEnd.ToShortDateString();
            ExcelWorkSheet.Cells[2, 1].HorizontalAlignment = Excel.Constants.xlCenter;
            ExcelWorkSheet.get_Range("A1:H1").Merge();
            ExcelWorkSheet.get_Range("A2:H2").Merge();
            ExcelWorkSheet.Cells[4, 1] = "№";
            ExcelWorkSheet.Cells[4, 2] = "Заказ";
            ExcelWorkSheet.Cells[4, 3] = "Поз.";
            ExcelWorkSheet.Cells[4, 4] = "Обозначение ЩЦМ";
            ExcelWorkSheet.Cells[4, 5] = "Наименование детали";
            ExcelWorkSheet.Cells[4, 6] = "Кол-во";
            ExcelWorkSheet.Cells[4, 7] = "Покрытие";
            ExcelWorkSheet.Cells[4, 8] = "Фактическая дата";
            //ExcelWorkSheet.Cells[4, 9] = "Примечание";
            ExcelWorkSheet.get_Range("A4:H4").Font.Bold = 1;
            ExcelWorkSheet.get_Range("A4:H4").WrapText = true;
            ExcelWorkSheet.get_Range("A4:H4").Font.Size = 11;
            ExcelWorkSheet.get_Range("A4:H4").HorizontalAlignment = Excel.Constants.xlCenter;
            ExcelWorkSheet.get_Range("A4:H4").VerticalAlignment = Excel.Constants.xlCenter;



            progressBar1.Maximum = dGVGalvan.Rows.Count * 8;

            for (int row = 0; row < dGVGalvan.Rows.Count; row++)
            {
                for (int col = 0; col < dGVGalvan.Columns.Count - 2; col++)
                {
                    ExcelWorkSheet.Cells[row + 5, col + 1] = dGVGalvan.Rows[row].Cells[col].Value.ToString().Trim();
                    ExcelWorkSheet.Cells[row + 5, col + 1].WrapText = true;
                    ExcelWorkSheet.Cells[row + 5, col + 1].Font.Size = 11;
                    if (col == 1 || col == 6)
                    {

                        ExcelWorkSheet.Cells[row + 5, col + 1].HorizontalAlignment = Excel.Constants.xlLeft;
                        ExcelWorkSheet.Cells[row + 5, col + 1].VerticalAlignment = Excel.Constants.xlCenter;
                        if (dGVGalvan.Rows[row].Cells[col].Value.ToString().Contains("/"))
                        {
                            ExcelWorkSheet.Cells[row + 5, col + 1] = dGVGalvan.Rows[row].Cells[col].Value.ToString().Replace("/", "/\n");
                        }
                    }
                    else
                    {
                        ExcelWorkSheet.Cells[row + 5, col + 1].HorizontalAlignment = Excel.Constants.xlCenter;
                        ExcelWorkSheet.Cells[row + 5, col + 1].VerticalAlignment = Excel.Constants.xlCenter;
                    }

                    progressBar1.Value++;
                    //ExcelWorkSheet.Cells[row + 5, col + 1].Font.Bold = 1;
                }
            }
            ExcelApp.DisplayAlerts = false;

            //int countDetail = 0;
            //Console.WriteLine(dGVGalvan.Rows.Count);

            //for (int row = 0; row < dGVGalvan.Rows.Count - 1; row++)
            //{

            //    countDetail += (countDetail == 0 ? Convert.ToInt32(dGVGalvan.Rows[row].Cells[5].Value) : 0);

            //    if (dGVGalvan.Rows[row].Cells[3].Value.ToString().Trim() == dGVGalvan.Rows[row + 1].Cells[3].Value.ToString().Trim())
            //    {
            //        countDetail += Convert.ToInt32(dGVGalvan.Rows[row + 1].Cells[5].Value);
            //        if (row == dGVGalvan.Rows.Count - 2)
            //        {
            //            ExcelWorkSheet.Cells[row + 5, 5] = countDetail;
            //            Console.WriteLine(countDetail);
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine(countDetail);
            //        ExcelWorkSheet.Cells[row + 5, 5] = countDetail;
            //        countDetail = 0;
            //        continue;
            //    }

            //    //if(dGVGalvan.Rows[row].Cells[3].Value.ToString().Trim() != dGVGalvan.Rows[row + 1].Cells[3].Value.ToString().Trim())
            //    //{
            //    //    Console.WriteLine(countDetail);
            //    //    countDetail = 0;
            //    //    continue;
            //    //}
            //    //Console.WriteLine(dGVGalvan.Rows[row].Cells[5].Value);
            //    //Console.WriteLine(dGVGalvan.Rows[row].Cells[5].Value.GetType());


            //}
            int countStr = 0;
            double sum = 0;
            int summ = 0;
            for (int row = 1; row < dGVGalvan.Rows.Count; row++)
            {
                
                for (int col = 0; col < dGVGalvan.Columns.Count - 2; col++)
                {

                    if (dGVGalvan.Rows[row].Cells[col].Value.ToString().Trim() == dGVGalvan.Rows[row - 1].Cells[col].Value.ToString().Trim() && col == 3)
                    {
                        summ += (summ == 0 ? Convert.ToInt32(dGVGalvan.Rows[row - 1].Cells[5].Value) : 0);
                        //Console.WriteLine($"колонка - {col} Строка{row} == строке{row - 1}");
                        countStr += 1;
                        //ExcelWorkSheet.get_Range($"D{row + 5}:D{row - 1 + 5}").Merge();
                        //ExcelWorkSheet.get_Range($"E{row + 5}:E{row - 1 + 5}").Merge();
                        //Excel.Range range = ExcelWorkSheet.Range[$"F{row + 5}:F{row - 1 + 5}"];
                        //sum += ExcelWorkSheet.Evaluate("SUM(" + range.Address + ")");

                        summ += Convert.ToInt32(dGVGalvan.Rows[row].Cells[5].Value);
                        if (dGVGalvan.Rows.Count - 1 == row)
                        {
                            ExcelWorkSheet.Cells[row + 5 - countStr, 6] = summ;
                            ExcelWorkSheet.get_Range($"F{row + 5 - countStr}:F{row + 5}").Merge();
                        }

                    }
                    if (dGVGalvan.Rows[row].Cells[col].Value.ToString().Trim() != dGVGalvan.Rows[row - 1].Cells[col].Value.ToString().Trim() && col == 3)
                    {
                        summ = summ == 0 ? Convert.ToInt32(dGVGalvan.Rows[row - 1].Cells[5].Value) : summ;
                        ExcelWorkSheet.Cells[row - 1 + 5 - countStr, 6] = summ;
                        ExcelWorkSheet.get_Range($"F{row - 1 + 5 - countStr}:F{row - 1 + 5}").Merge();
                        
                        summ = 0;
                        sum = 0;
                        countStr = 0;
                    }
                }
            }

            for (int row = 1; row < dGVGalvan.Rows.Count; row++)
            {

                for (int col = 0; col < dGVGalvan.Columns.Count - 2; col++)
                {

                    if (dGVGalvan.Rows[row].Cells[col].Value.ToString().Trim() == dGVGalvan.Rows[row - 1].Cells[col].Value.ToString().Trim() && col == 3)
                    {
                        ExcelWorkSheet.get_Range($"D{row + 5}:D{row - 1 + 5}").Merge();
                        ExcelWorkSheet.get_Range($"E{row + 5}:E{row - 1 + 5}").Merge();
                    }
                }
            }

            ExcelApp.DisplayAlerts = true;
            ExcelWorkSheet.get_Range("A4", "H" + (dGVGalvan.Rows.Count + 4)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            ExcelApp.Visible = true;
            progressBar1.Value = 0;
            MessageBox.Show("Формирование отчета завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ExcelApp.Quit();
        }

        private void bTNBirki_Click_1(object sender, EventArgs e)
        {
            if (selectedRows.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одну позицию!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<string> listBirki = new List<string>();
            int countBirki = 0;

            for (int i = 0; i < selectedRows.Count; i++)
            {
                selectedRows[i].DefaultCellStyle.BackColor = Color.Aquamarine;
                Console.WriteLine(selectedRows[i].Cells[3].Value.ToString());
                listBirki.Add(selectedRows[i].Cells[3].Value.ToString());
            }
            var result = listBirki.Distinct();
            var resultList = result.ToList();
            var resultBirkiCount = resultList.ToList().Count;


            // Create a new Excel application
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;
            // Add a new workbook
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            // Get the active sheet
            Excel.Worksheet sheet = workbook.ActiveSheet;
            // Set the sheet orientation to landscape
            sheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
            // Set the sheet paper size to A4
            sheet.PageSetup.PaperSize = Excel.XlPaperSize.xlPaperA4;
            sheet.PageSetup.LeftMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.RightMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.TopMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.BottomMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.HeaderMargin = excelApp.CentimetersToPoints(0);
            sheet.PageSetup.FooterMargin = excelApp.CentimetersToPoints(0);
            sheet.StandardWidth = 9;

            int startRow = 1;
            int startRowList = 0;
            int countList = (resultBirkiCount <= 16 ? 1 : resultBirkiCount <= 32 ? 2 : 3);

            Dictionary<string, List<string>> mainDic = new Dictionary<string, List<string>>();
            Dictionary<string, string> nameOrderDic = new Dictionary<string, string>();
            Dictionary<string, int> countTotalDic = new Dictionary<string, int>();
            Dictionary<string, List<string>> posDic = new Dictionary<string, List<string>>();
            Dictionary<string, string> galvanDic = new Dictionary<string, string>();

            for (int i = 0; i < selectedRows.Count; i++)
            {
                
                if (mainDic.ContainsKey(selectedRows[i].Cells[3].Value.ToString()))
                {
                    mainDic[selectedRows[i].Cells[3].Value.ToString()].Add(selectedRows[i].Cells[1].Value.ToString());
                    nameOrderDic[selectedRows[i].Cells[3].Value.ToString()] = selectedRows[i].Cells[4].Value.ToString();
                    countTotalDic[selectedRows[i].Cells[3].Value.ToString()] += Convert.ToInt32(selectedRows[i].Cells[5].Value.ToString());
                    posDic[selectedRows[i].Cells[3].Value.ToString()].Add(selectedRows[i].Cells[2].Value.ToString());
                    galvanDic[selectedRows[i].Cells[3].Value.ToString()] = selectedRows[i].Cells[6].Value.ToString();
                }
                else
                {
                    mainDic[selectedRows[i].Cells[3].Value.ToString()] = new List<string>
                    {
                        selectedRows[i].Cells[1].Value.ToString()
                    };

                    nameOrderDic[selectedRows[i].Cells[3].Value.ToString()] = selectedRows[i].Cells[4].Value.ToString();

                    countTotalDic[selectedRows[i].Cells[3].Value.ToString()] = Convert.ToInt32(selectedRows[i].Cells[5].Value.ToString());

                    posDic[selectedRows[i].Cells[3].Value.ToString()] = new List<string>
                    {
                        selectedRows[i].Cells[2].Value.ToString()
                    };
                    galvanDic[selectedRows[i].Cells[3].Value.ToString()] = selectedRows[i].Cells[6].Value.ToString();
                }
            }

            int totalCount = 0;
            int z = 1, x = 4;
            int four = 1;
            int startPos = 1;

            //while (totalCount < mainDic.Count)
            foreach (var order in mainDic.Keys)
            {
                int endRow = startRow + 9;
                // Рисуем верхнюю границу ячеек
                sheet.Range[sheet.Cells[startRow, z], sheet.Cells[startRow, x]].Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                sheet.Range[sheet.Cells[startRow, z], sheet.Cells[startRow, x]].Borders[Excel.XlBordersIndex.xlEdgeTop].Weight = Excel.XlBorderWeight.xlMedium;
                // Рисуем нижнюю границу ячеек
                sheet.Range[sheet.Cells[endRow, z], sheet.Cells[endRow, x]].Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                sheet.Range[sheet.Cells[endRow, z], sheet.Cells[endRow, x]].Borders[Excel.XlBordersIndex.xlEdgeBottom].Weight = Excel.XlBorderWeight.xlMedium;
                // Рисуем левую границу ячеек
                sheet.Range[sheet.Cells[startRow, z], sheet.Cells[endRow, z]].Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                sheet.Range[sheet.Cells[startRow, z], sheet.Cells[endRow, z]].Borders[Excel.XlBordersIndex.xlEdgeLeft].Weight = Excel.XlBorderWeight.xlMedium;
                // Рисуем правую границу ячеек
                sheet.Range[sheet.Cells[startRow, x], sheet.Cells[endRow, x]].Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                sheet.Range[sheet.Cells[startRow, x], sheet.Cells[endRow, x]].Borders[Excel.XlBordersIndex.xlEdgeRight].Weight = Excel.XlBorderWeight.xlMedium;


                z += 4;
                x += 4;
                four++;

                sheet.get_Range($"A{startRow + 7}:D{startRow + 9}").Merge();
                sheet.get_Range($"E{startRow + 7}:H{startRow + 9}").Merge();
                sheet.get_Range($"I{startRow + 7}:L{startRow + 9}").Merge();
                sheet.get_Range($"M{startRow + 7}:P{startRow + 9}").Merge();
                //**********************************************************
                //Наименование
                sheet.Cells[startRow, startPos].Value = "НАИМЕНОВАНИЕ";
                sheet.Cells[startRow, startPos].Font.Size = 10;

                sheet.Cells[startRow, startPos + 2].Value = nameOrderDic[order];
                sheet.Cells[startRow, startPos + 2].Font.Size = 11;
                sheet.Cells[startRow, startPos + 2].Font.Bold = 1;

                //Заказ
                sheet.Cells[startRow + 1, startPos].Value = "ЗАКАЗ";
                sheet.Cells[startRow + 1, startPos].Font.Size = 10;

                var shiftRight = 0;
                var shiftDown = 0;
                foreach (var n in mainDic[order])
                {
                    if (shiftRight == 4)
                    {
                        shiftDown += 1;
                        shiftRight = 0;
                    }
                    sheet.Cells[startRow + 2 + shiftDown, startPos + shiftRight].Value = n;
                    sheet.Cells[startRow + 2 + shiftDown, startPos + shiftRight].Font.Size = 11;
                    sheet.Cells[startRow + 2 + shiftDown, startPos + shiftRight].Font.Bold = 1;
                    shiftRight += 1;
                }

                //ЩЦМ
                sheet.Cells[startRow + 4, startPos].Value = "ЩЦМ";
                sheet.Cells[startRow + 4, startPos].Font.Size = 10;
                //sheet.Cells[startRow + 3, startPos].Font.Bold = 1;
                sheet.Cells[startRow + 4, startPos + 1].Value = order;
                sheet.Cells[startRow + 4, startPos + 1].Font.Size = 11;
                sheet.Cells[startRow + 4, startPos + 1].Font.Bold = 1;

                //Колличество
                sheet.Cells[startRow + 5, startPos].Value = "КОЛ-ВО";
                sheet.Cells[startRow + 5, startPos].Font.Size = 10;
                //sheet.Cells[startRow + 4, startPos].Font.Bold = 1;
                sheet.Cells[startRow + 5, startPos + 1].Value = countTotalDic[order].ToString();
                sheet.Cells[startRow + 5, startPos + 1].Font.Size = 11;
                sheet.Cells[startRow + 5, startPos + 1].Font.Bold = 1;
                sheet.Cells[startRow + 5, startPos + 1].HorizontalAlignment = Excel.Constants.xlLeft;

                //Позиция
                sheet.Cells[startRow + 6, startPos].Value = "ПОЗ.";
                sheet.Cells[startRow + 6, startPos].Font.Size = 10;
                //sheet.Cells[startRow + 6, startPos].HorizontalAlignment = Excel.Constants.xlRight;
                //sheet.Cells[startRow + 4, startPos].Font.Bold = 1;

                sheet.Cells[startRow + 6, startPos + 2].NumberFormat = "@";
                sheet.Cells[startRow + 6, startPos + 2].Value = String.Join(", ", posDic[order].ToArray());

                //foreach (var pos in posDic[order])
                //{
                //    sheet.Cells[startRow + 6, startPos + 2].Value += pos.ToString();
                //    sheet.Cells[startRow + 6, startPos + 2].Value += ", ";
                //}
                sheet.Cells[startRow + 6, startPos + 2].Font.Size = 11;
                sheet.Cells[startRow + 6, startPos + 2].Font.Bold = 1;
                sheet.Cells[startRow + 6, startPos + 2].HorizontalAlignment = Excel.Constants.xlLeft;
                sheet.Cells[startRow + 6, startPos + 2].HorizontalAlignment = Excel.Constants.xlCenter;

                //Операция
                sheet.Cells[startRow + 7, startPos].Value = galvanDic[order];
                sheet.Cells[startRow + 7, startPos].Font.Size = 11;
                sheet.Cells[startRow + 7, startPos].Font.Bold = 1;
                sheet.Cells[startRow + 7, startPos].Font.Underline = true;
                sheet.Cells[startRow + 7, startPos].HorizontalAlignment = Excel.Constants.xlCenter;
                sheet.Cells[startRow + 7, startPos].VerticalAlignment = Excel.Constants.xlCenter;
                sheet.Cells[startRow + 7, startPos].IndentLevel = 6;
                sheet.Cells[startRow + 7, startPos].WrapText = true;

                startRowList++;
                totalCount++;
                if (startRowList > resultBirkiCount - 1)
                {
                    selectedRows.Clear();
                    return;
                }

                if (four > 4)
                {
                    startRow += 10;
                    endRow += 10;
                    four = 1;
                    startPos = 1;
                    z = 1;
                    x = 4;
                }
                else startPos += 4;

                //startRow += 3;
                countList--;
            }
            excelApp.Quit();
        }

        private void cLB_rep3Workers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            

            if (e.NewValue == CheckState.Checked)
            {
                string selectedItem = "'" + cLB_rep3Workers.SelectedItem.ToString() + "'";
                selectedItems.Add(selectedItem);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                string selectedItem = "'" + cLB_rep3Workers.SelectedItem.ToString() + "'";
                selectedItems.Remove(selectedItem);
            }
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void tB_OrderNumOTK_TextChanged(object sender, EventArgs e)
        {
            BS_Orders.Filter = " OrderNum like '%" + tB_OrderNumOTK.Text.ToString().Trim() + "%'";
        }

        private void btn_repOTK_Click(object sender, EventArgs e)
        {
            // Проверка ввода
            string orderNum = tB_OrderNumInfoOTK.Text.Trim();
            if (string.IsNullOrEmpty(orderNum))
            {
                MessageBox.Show("Введите номер заказа.");
                return;
            }

            int pk_IdOrder = 0;
            string orderName = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(config.ConnectionString))
                {
                    conn.Open();

                    // Получаем PK_IdOrder и OrderName по номеру заказа
                    string orderQuery = @"SELECT PK_IdOrder, OrderName 
                                      FROM [Orders] 
                                      WHERE OrderNum = @OrderNum";

                    using (SqlCommand cmd = new SqlCommand(orderQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@OrderNum", orderNum);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pk_IdOrder = reader.GetInt32(0);
                                orderName = reader.GetString(1);
                            }
                            else
                            {
                                MessageBox.Show("Заказ с таким номером не найден.");
                                return;
                            }
                        }
                    }

                    // Получаем данные о деталях (добавляем PK_IdOrderDetail и AmountDetails)
                    DataTable dtDetails = new DataTable();
                    string detailsQuery = @"
                    SELECT 
                        od.PK_IdOrderDetail,
                        od.Position,
                        od.AmountDetails,
                        sd.ShcmDetail,
                        sd.NameDetail,
                        std.NameType
                    FROM OrdersDetails od
                    INNER JOIN Sp_Details sd ON od.FK_IdDetail = sd.PK_IdDetail
                    INNER JOIN Sp_TypeDetails std ON sd.FK_IdTypeDetail = std.PK_IdTypeDetail
                    WHERE od.FK_IdOrder = @IdOrder
                    ORDER BY od.Position";

                    using (SqlCommand cmd = new SqlCommand(detailsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdOrder", pk_IdOrder);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtDetails);
                        }
                    }

                    // Получаем данные о крепежах (с AmountFasteners)
                    DataTable dtFasteners = new DataTable();
                    string fastenersQuery = @"
                    SELECT 
                        OrdersFasteners.Position,
                        OrdersFasteners.NameFasteners,
                        OrdersFasteners.AmountFasteners,
                        OrdersFasteners.MeasureUnit,
                        std.NameType AS TypeFasteners
                    FROM OrdersFasteners
                    INNER JOIN Sp_TypeDetails std ON OrdersFasteners.FK_IdTypeFasteners = std.PK_IdTypeDetail
                    WHERE OrdersFasteners.FK_IdOrder = @IdOrder
                    ORDER BY OrdersFasteners.Position";

                    using (SqlCommand cmd = new SqlCommand(fastenersQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdOrder", pk_IdOrder);
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dtFasteners);
                        }
                    }

                    // Получаем статусы ОТК для деталей
                    // Сначала соберем все PK_IdOrderDetail
                    var detailIds = dtDetails.AsEnumerable().Select(r => r.Field<long>("PK_IdOrderDetail")).ToList();
                    Dictionary<long, string> otkStatusByDetail = new Dictionary<long, string>();

                    if (detailIds.Count > 0)
                    {
                        string ids = string.Join(",", detailIds);
                        string otkQuery = @"
                        SELECT o.PK_IdOrderDetail, c.CheckBoxState
                        FROM OperationsOTK o
                        LEFT JOIN [OTKControl] c ON o.OperationID = c.OperationID
                        WHERE o.PK_IdOrderDetail IN (" + ids + @")";

                        DataTable dtOtk = new DataTable();
                        using (SqlCommand cmd = new SqlCommand(otkQuery, conn))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dtOtk);
                            }
                        }

                        // Группируем по PK_IdOrderDetail
                        var groups = dtOtk.AsEnumerable().GroupBy(r => r.Field<long>("PK_IdOrderDetail"));
                        foreach (var grp in groups)
                        {
                            var states = grp.Select(g => g["CheckBoxState"]).ToList();

                            // Если нет ни одной записи в OTKControlID (CheckBoxState - null),
                            // значит все строки null или вообще ни одной. Проверим:
                            if (!states.Any() || states.All(s => s == DBNull.Value))
                            {
                                // Нет записей - статус пустой
                                otkStatusByDetail[grp.Key] = "";
                                continue;
                            }

                            // Фильтруем null-ы
                            var intStates = states.Where(s => s != DBNull.Value).Select(s => Convert.ToInt32(s)).ToList();

                            if (intStates.Count == 0)
                            {
                                // Нет валидных статусов
                                otkStatusByDetail[grp.Key] = "";
                            }
                            else if (intStates.All(st => st == 1))
                            {
                                // Все 2 - Принято
                                otkStatusByDetail[grp.Key] = "Принято";
                            }
                            else
                            {
                                // Есть что-то кроме 2 - Частично готово
                                otkStatusByDetail[grp.Key] = "Частично готово";
                            }
                        }

                        // Проверим детали, у которых нет записей вообще в OTK
                        foreach (var id in detailIds)
                        {
                            if (!otkStatusByDetail.ContainsKey(id))
                                otkStatusByDetail[id] = ""; // Пусто, если ни одной записи
                        }
                    }

                    // Формируем Excel-файл
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        var ws = pck.Workbook.Worksheets.Add("Отчет");

                        int currentRow = 1;

                        // Заголовок отчета с именем заказа
                        ws.Cells[currentRow, 1].Value = $"Отчет по заказу: {orderNum}";
                        ws.Cells[currentRow, 1, currentRow, 6].Merge = true;
                        ws.Cells[currentRow, 1].Style.Font.Bold = true;
                        ws.Cells[currentRow, 1].Style.Font.Size = 16;
                        ws.Cells[currentRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        currentRow += 2;

                        // Раздел для деталей
                        ws.Cells[currentRow, 1].Value = "Детали заказа";
                        ws.Cells[currentRow, 1].Style.Font.Bold = true;
                        currentRow++;

                        // Заголовки таблицы деталей
                        ws.Cells[currentRow, 1].Value = "Номер позиции";
                        ws.Cells[currentRow, 2].Value = "ЩЦМ детали";
                        ws.Cells[currentRow, 3].Value = "Название детали";
                        ws.Cells[currentRow, 4].Value = "Тип детали";
                        ws.Cells[currentRow, 5].Value = "Количество";
                        ws.Cells[currentRow, 6].Value = "Статус ОТК";

                        using (var headerRange = ws.Cells[currentRow, 1, currentRow, 6])
                        {
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            headerRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }

                        currentRow++;

                        int posNumber = 1;
                        foreach (DataRow dr in dtDetails.Rows)
                        {
                            long detailId = (long)dr["PK_IdOrderDetail"];
                            string shcm = dr["ShcmDetail"]?.ToString() ?? "";
                            string nameDetail = dr["NameDetail"]?.ToString() ?? "";
                            string typeDetail = dr["NameType"]?.ToString() ?? "";
                            double amount = dr["AmountDetails"] != DBNull.Value ? Convert.ToDouble(dr["AmountDetails"]) : 0.0;

                            string status = "";
                            if (otkStatusByDetail.ContainsKey(detailId))
                                status = otkStatusByDetail[detailId];

                            ws.Cells[currentRow, 1].Value = posNumber;
                            ws.Cells[currentRow, 2].Value = shcm;
                            ws.Cells[currentRow, 3].Value = nameDetail;
                            ws.Cells[currentRow, 4].Value = typeDetail;
                            ws.Cells[currentRow, 5].Value = amount;
                            ws.Cells[currentRow, 6].Value = status;

                            // Форматирование статуса ОТК
                            if (status == "Принято")
                            {
                                ws.Cells[currentRow, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[currentRow, 6].Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                            }
                            else if (status == "Частично готово")
                            {
                                ws.Cells[currentRow, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[currentRow, 6].Style.Fill.BackgroundColor.SetColor(Color.BurlyWood);
                            }
                            // Если статус пустой - без заливки

                            currentRow++;
                            posNumber++;
                        }

                        // Ширина столбцов
                        ws.Column(1).Width = 15;
                        ws.Column(2).AutoFit();
                        ws.Column(3).AutoFit();
                        ws.Column(4).AutoFit();
                        ws.Column(5).AutoFit();
                        ws.Column(6).AutoFit();

                        currentRow += 2;

                        // Раздел для крепежей
                        ws.Cells[currentRow, 1].Value = "Крепежи заказа";
                        ws.Cells[currentRow, 1].Style.Font.Bold = true;
                        currentRow++;

                        // Заголовки таблицы крепежей
                        ws.Cells[currentRow, 1].Value = "Номер позиции";
                        ws.Cells[currentRow, 2].Value = "Название крепежа";
                        ws.Cells[currentRow, 3].Value = "Количество";
                        ws.Cells[currentRow, 4].Value = "Единица измерения";
                        ws.Cells[currentRow, 5].Value = "Тип крепежа";

                        using (var headerRange = ws.Cells[currentRow, 1, currentRow, 5])
                        {
                            headerRange.Style.Font.Bold = true;
                            headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                            headerRange.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        }

                        currentRow++;

                        posNumber = 1;
                        foreach (DataRow dr in dtFasteners.Rows)
                        {
                            string nameFastener = dr["NameFasteners"]?.ToString() ?? "";
                            double amountFastener = dr["AmountFasteners"] != DBNull.Value ? Convert.ToDouble(dr["AmountFasteners"]) : 0.0;
                            string measureUnit = dr["MeasureUnit"]?.ToString() ?? "";
                            string typeFastener = dr["TypeFasteners"]?.ToString() ?? "";

                            ws.Cells[currentRow, 1].Value = posNumber;
                            ws.Cells[currentRow, 2].Value = nameFastener;
                            ws.Cells[currentRow, 3].Value = amountFastener;
                            ws.Cells[currentRow, 4].Value = measureUnit;
                            ws.Cells[currentRow, 5].Value = typeFastener;

                            currentRow++;
                            posNumber++;
                        }

                        ws.Column(1).Width = 15;
                        ws.Column(2).AutoFit();
                        ws.Column(3).AutoFit();
                        ws.Column(4).AutoFit();
                        ws.Column(5).AutoFit();

                        // Обрамляем границы всех данных
                        int maxCol = Math.Max(6, 5); // 6 столбцов в деталях, 5 в крепежах
                        using (var fullRange = ws.Cells[1, 1, currentRow - 1, maxCol])
                        {
                            fullRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            fullRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            fullRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            fullRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        // Сохранение файла
                        using (SaveFileDialog sfd = new SaveFileDialog())
                        {
                            sfd.Filter = "Excel files (*.xlsx)|*.xlsx";
                            sfd.Title = "Сохранить отчет";
                            sfd.FileName = $"Отчет ОТК по заказу №{orderNum}.xlsx";
                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                File.WriteAllBytes(sfd.FileName, pck.GetAsByteArray());
                                MessageBox.Show("Отчет успешно сформирован и сохранен.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
    }
}
