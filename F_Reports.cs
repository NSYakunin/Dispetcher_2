﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
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
        // Надо перенести функциональность в OrderRepository
        C_Orders orders;
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_Reports reports;

        // Внешняя зависимость
        System.Windows.Controls.UserControl labControl;

        DataTable Dt_SpDepartment = new DataTable();
        DataTable Dt_SpWorkers = new DataTable();
        DataTable DT_Orders = new DataTable();
        BindingSource BS_Orders = new BindingSource();

        public F_Reports(IConfig config, IConverter converter, LaborViewModel viewModel)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр: IConfig");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            if (viewModel == null) throw new ArgumentException("Пожалуйста укажите параметр: LaborViewModel");

            this.config = config;

            departments = new C_Departments(config);
            users = new C_Users(config);
            orders = new C_Orders(config);
            reports = new C_Reports(config);

            // Внешняя зависимость!
            labControl = new LaborControl();
            //viewModel.Dispatcher = labControl.Dispatcher;
            labControl.DataContext = viewModel;
            viewModel.ColumnContainer = labControl as IColumnUpdate;

            InitializeComponent();
            if (config.SelectedReportMode == ReportMode.ОтчетНаряд
                || config.SelectedReportMode == ReportMode.ДвижениеДеталей
                || config.SelectedReportMode == ReportMode.ОтчетВыполненным
                || config.SelectedReportMode == ReportMode.Гальваника)
            {
                DT_Orders.Columns.Add("PK_IdOrder", typeof(int));
                DT_Orders.Columns.Add("OrderNum", typeof(string));
                DT_Orders.Columns.Add("OrderName", typeof(string));
            }

            if (config.SelectedReportMode == ReportMode.Трудоемкость)
            {
                LaborElementHost.Child = labControl;
                myTabC_Reports.SelectedTab = LaborTabPage;
                viewModel.Start();
            }
        }

        private void F_Reports_Load(object sender, EventArgs e)
        {
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

            if (config.SelectedReportMode == ReportMode.Гальваника)//Движение деталей
            {
                myTabC_Reports.SelectedTab = tabPageGalvan;
                loaddGVGalvan(galvanStart.Value.Date, galvanEnd.Value.Date);
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
                reports.rep3(dTP_rep3Start.Value.Date, dTP_rep3End.Value.Date, loginWorker, IdCeh, flagDays, PlanHours, cWorkDays, koop);
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
                                      $"ORDER BY  OrderNum, ShcmDetail, DateFactOper";

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

        private void exelGalvan_Click(object sender, EventArgs e)
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

            ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
            ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
            ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
            ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
            ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
            ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
            ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);

            ((Excel.Range)ExcelWorkSheet.Columns[1]).ColumnWidth = 3;
            ((Excel.Range)ExcelWorkSheet.Columns[2]).ColumnWidth = 20;
            //((Excel.Range)ExcelWorkSheet.Columns[2]).Font.Bold = 1;
            ((Excel.Range)ExcelWorkSheet.Columns[3]).ColumnWidth = 5;
            ((Excel.Range)ExcelWorkSheet.Columns[4]).ColumnWidth = 20;
            ((Excel.Range)ExcelWorkSheet.Columns[5]).ColumnWidth = 30;
            ((Excel.Range)ExcelWorkSheet.Columns[6]).ColumnWidth = 6;
            ((Excel.Range)ExcelWorkSheet.Columns[7]).ColumnWidth = 45;
            ((Excel.Range)ExcelWorkSheet.Columns[8]).ColumnWidth = 10;
            ((Excel.Range)ExcelWorkSheet.Columns[9]).ColumnWidth = 30;


            ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Value2 = $"Акт приёма-передачи №{numActPeredachi.Text} от {dataNowLabel.Text}";
            ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).HorizontalAlignment = Excel.Constants.xlCenter;
            ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Font.Bold = 1;
            ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).Value2 = "с " + DateStart.ToShortDateString() + " по " + DateEnd.ToShortDateString();
            ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).HorizontalAlignment = Excel.Constants.xlCenter;
            ((Excel.Range)ExcelWorkSheet.get_Range("A1:I1")).Merge();
            ((Excel.Range)ExcelWorkSheet.get_Range("A2:I2")).Merge();
            ExcelWorkSheet.Cells[4, 1] = "№";
            ExcelWorkSheet.Cells[4, 2] = "Заказ";
            ExcelWorkSheet.Cells[4, 3] = "Поз.";
            ExcelWorkSheet.Cells[4, 4] = "Обозначение ЩЦМ";
            ExcelWorkSheet.Cells[4, 5] = "Наименование детали";
            ExcelWorkSheet.Cells[4, 6] = "Кол-во";
            ExcelWorkSheet.Cells[4, 7] = "Покрытие";
            ExcelWorkSheet.Cells[4, 8] = "Дата Факт";
            ExcelWorkSheet.Cells[4, 9] = "Примечание";

            progressBar1.Maximum = dGVGalvan.Rows.Count * 8;

            for (int row = 0; row < dGVGalvan.Rows.Count; row++)
            {
                for (int col = 0; col < dGVGalvan.Columns.Count - 1; col++)
                {
                    ExcelWorkSheet.Cells[row + 5, col + 1] = dGVGalvan.Rows[row].Cells[col].Value.ToString().Trim();
                    if (col == 1) ((Excel.Range)ExcelWorkSheet.Cells[row + 5, col + 1]).HorizontalAlignment = Excel.Constants.xlLeft;
                    progressBar1.Value++;
                }
            }

            ExcelWorkSheet.get_Range("A4", "I" + (dGVGalvan.Rows.Count + 4)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            ExcelApp.Visible = true;
            MessageBox.Show("Формирование отчета завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
