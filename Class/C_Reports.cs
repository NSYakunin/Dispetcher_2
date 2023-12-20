using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Drawing;
using SourceGrid;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Dispetcher2.Class
{
    sealed class C_Reports
    {
        const int _CmdTimeout = 60; //seconds SqlCommand cmd = new SqlCommand() { CommandTimeout = CmdTimeout};
        private System.Data.DataTable _DT;
        private int[] _OperGroupFactTime;// Только для "План-график (форма №6)"
        private int[] _FactTime; // Только для "План-график (форма №6)
        private bool _err = false;//Наличие ошибок при формировании отчёта
        IConfig config;

        public C_Reports(IConfig config)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            this.config = config;
            _DT = new DataTable();

            _OperGroupFactTime = new int[12];
            _FactTime = new int[12];
        }

        public bool RepErrors
        {
            get { return _err; }
        }

        public int[] GetGroupTimeForm6()
        {
            return _OperGroupFactTime;
        }

        public int[] GetFactTimeForm6()
        {
            return _FactTime;
        }

        float DecimalToSec(decimal time)
        {
            string[] temp = time.ToString(CultureInfo.InvariantCulture).Split('.');
            if (time.ToString().IndexOf(".") > 0) temp = time.ToString().Split('.');
            return (Convert.ToInt32(temp[0]) * 3600) + Convert.ToInt32(temp[1]) * 60;
        }

        // Только для "План-график (форма №6)"
        private int NormTimeFabrication(bool OnlyOncePay, int Tpd, int Tsh, int Amount)
        {
            int FactTime = 0;
            try
            {

                if (OnlyOncePay) FactTime = checked(Tpd + Tsh);
                else FactTime = checked(Tpd + (Tsh * Amount));
                return FactTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return FactTime;
            }
        }

        //***********************************************************************************************
        #region Отчёт-наряд по выполненным операциям
        //Переводим число во время
        private string IntToTime(int ttime)
        {
            string s = "";
            //Часы
            if (ttime > 0)
            {
                int h = (ttime / 3600);
                if (h > 0)
                    s = s + h + ":";
                else
                    s = s + "00:";
                ttime = ttime - h * 3600;

                //Минуты
                h = (ttime / 60);
                if (h > 0)
                    if (h < 10)
                        s = s + "0" + h;
                    else
                        s = s + h;
                else
                    s = s + "00";
                s = s + ":" + (ttime - h * 60);
            }
            else
                s = "0";
            return s;
        }

        private string IntToTime(ulong ttime)
        {
            string s = "";
            //Часы
            if (ttime > 0)
            {
                ulong h = (ttime / 3600);
                if (h > 0)
                    s = s + h + ":";
                else
                    s = s + "00:";
                ttime = ttime - h * 3600;

                //Минуты
                h = (ttime / 60);
                if (h > 0)
                    if (h < 10)
                        s = s + "0" + h;
                    else
                        s = s + h;
                else
                    s = s + "00";
                s = s + ":" + (ttime - h * 60);
            }
            else
                s = "0";
            return s;
        }

        /// <summary>
        /// Отчёт-наряд по выполненным операциям
        /// </summary>
        /// <param name="DateStart"></param>
        /// <param name="DateEnd"></param>
        /// <param name="IdUser">исполнитель</param>
        /// <param name="IdCeh">цех</param>
        /// <param name="FlagDays">Разбить по дням</param>
        public void rep3(DateTime DateStart, DateTime DateEnd, string loginWorker, int IdCeh, bool FlagDays, int PlanHours,int cWorkDays, bool koop, string notWorker)
        {
            try
            {
                _DT.Clear();
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    string sqlExpression;
                    if (notWorker != "")
                    {
                        sqlExpression = $"SELECT Distinct(TS1.FK_Login) AS FK_LoginWorker, " +
                            $"u.FK_IdDepartment AS PK_IdDepartment, " +
                            $"d.Department, u.DateStart, u.RateWorker, AmountWorkers, NumBrigade, OrderNum, Position, NameDetail, ShcmDetail, AmountDetails, " +
                            $" FactOper, DateFactOper, Tpd, Tsh, OnlyOncePay , sum((convert( decimal(4,1), TS1.Val_TimeFloat))) AS Val_Time, " +
                            $"u.DateEnd " +
                            $"FROM TimeSheets TS1 " +
                            $"LEFT JOIN vwFactWorkers FW ON fw.FK_LoginWorker = TS1.FK_Login AND FW.DateFactOper >= '{DateStart}' " +
                            $"AND FW.DateFactOper <= '{DateEnd}' AND FW.AmountDetails = null " +
                            $"INNER JOIN Users u ON u.PK_Login = TS1.FK_Login AND u.ITR = 0 AND Fk_IdJob<>28 AND Fk_IdJob>5 " +
                            $"AND u.PK_Login NOT IN ('Бобров С.А.','Воронцов А.А.') " +
                            $"INNER JOIN Sp_Department d ON d.PK_IdDepartment = u.FK_IdDepartment " +
                            $"WHERE TS1.PK_Date >= '{DateStart}' AND TS1.PK_Date <= '{DateEnd}' AND (u.DateEnd >= '{DateStart}' OR u.DateEnd is null) " +
                            $"AND TS1.FK_Login NOT IN ({notWorker})  " +
                            $"GROUP BY  u.FK_IdDepartment,d.Department,TS1.FK_Login,u.DateStart,u.RateWorker,AmountWorkers,NumBrigade,OrderNum,Position,NameDetail, " +
                            $"ShcmDetail,AmountDetails,FactOper,DateFactOper,Tpd,Tsh,OnlyOncePay,TS1.FK_Login,u.DateEnd " +
                            $"union all " +
                            $"SELECT FK_LoginWorker, PK_IdDepartment, Department, DateStart, RateWorker, AmountWorkers, NumBrigade, OrderNum, Position, " +
                            $"NameDetail, ShcmDetail, AmountDetails, FactOper, DateFactOper, Tpd, Tsh, OnlyOncePay, " +
                            $"sum((convert( decimal(4,1), Val_TimeFloat))) AS Val_Time, " +
                            $"DateEnd " +
                            $"FROM vwFactWorkers " +
                            $"LEFT JOIN TimeSheets TS ON TS.FK_Login = vwFactWorkers.FK_LoginWorker AND TS.PK_Date >= '{DateStart}' AND PK_Date <= '{DateEnd}' " +
                            $"WHERE DateFactOper >= '{DateStart}' AND DateFactOper <= '{DateEnd}' " +
                            $"AND FK_LoginWorker NOT IN ({notWorker})  " +
                            $"GROUP BY  PK_IdDepartment,Department,FK_LoginWorker,DateStart,RateWorker,AmountWorkers,NumBrigade,OrderNum,Position,NameDetail, " +
                            $"ShcmDetail,AmountDetails,FactOper,DateFactOper,Tpd,Tsh,OnlyOncePay, DateEnd " +
                            $"union all " +
                            $"SELECT FK_LoginWorker, PK_IdDepartment, Department, DateStart, RateWorker, AmountWorkers, NumBrigade, OrderNum, Position, " +
                            $"NameDetail, ShcmDetail, AmountDetails, FactOper, DateFactOper, Tpd, Tsh, OnlyOncePay, " +
                            $"sum((convert( decimal(4,1), Val_TimeFloat))) AS Val_Time, DateEnd " +
                            $"FROM vwFactBrigades " +
                            $"LEFT JOIN TimeSheets TS ON TS.FK_Login = vwFactBrigades.FK_LoginWorker AND TS.PK_Date >= '{DateStart}' " +
                            $"AND PK_Date <= '{DateEnd}' " +
                            $"WHERE DateFactOper >= '{DateStart}' AND DateFactOper <= '{DateEnd}' " +
                            $"AND FK_LoginWorker NOT IN ({notWorker})  " +
                            $"GROUP BY  PK_IdDepartment,Department,FK_LoginWorker,DateStart,RateWorker,AmountWorkers,NumBrigade,OrderNum,Position, " +
                            $"NameDetail, ShcmDetail,AmountDetails,FactOper,DateFactOper,Tpd,Tsh,OnlyOncePay, DateEnd " +
                            $"ORDER BY  PK_IdDepartment,FK_LoginWorker,OrderNum;";

                        SqlCommand cmd = new SqlCommand(sqlExpression) { CommandTimeout = 120 };

                        cmd.Connection = con;

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;
                        adapter.Fill(_DT);
                        adapter.Dispose();
                        _DT.Rows.Add();
                    }
                    else
                    {
                        sqlExpression = (loginWorker == "" & IdCeh == -1) ? "rep3All" : loginWorker != "" ? "rep3Worker" : "rep3Ceh";

                        SqlCommand cmd = new SqlCommand(sqlExpression) { CommandTimeout = 120 };
                        cmd.Connection = con;
                        cmd.Parameters.Clear();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@DateStart", DateStart);
                        cmd.Parameters.AddWithValue("@DateEnd", DateEnd);
                        cmd.Parameters.AddWithValue("@FK_LoginWorker", loginWorker);
                        cmd.Parameters.AddWithValue("@PK_IdDepartment", IdCeh);

                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.SelectCommand = cmd;
                        adapter.Fill(_DT);
                        adapter.Dispose();
                        _DT.Rows.Add();
                    }    
                    //Если не выбрано ничего, то процедура rep3All, если выбран логин rep3Worker, и наконец если выбран цех rep3Ceh
                }


                if (_DT.Rows.Count == 0) MessageBox.Show("Нет данных для формирования отчёта.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else//Export data to Excel
                {
                    Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                    //XlReferenceStyle RefStyle = Excel.ReferenceStyle; Excel.Visible = true;
                    ExcelApp.Workbooks.Add(1);
                    Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);
                    //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove; //Сворачиваем сверху
                    #region Настройка шапки
                    ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);
                    ////Редактирование созданного документа
                    ((Excel.Range)ExcelWorkSheet.Columns[1]).ColumnWidth = 3;
                    ((Excel.Range)ExcelWorkSheet.Columns[2]).ColumnWidth = 18;
                    ((Excel.Range)ExcelWorkSheet.Columns[2]).Font.Bold = 1;
                    ((Excel.Range)ExcelWorkSheet.Columns[3]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[4]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[5]).ColumnWidth = 25;
                    ((Excel.Range)ExcelWorkSheet.Columns[6]).ColumnWidth = 15;
                    ((Excel.Range)ExcelWorkSheet.Columns[7]).ColumnWidth = 5;
                    ((Excel.Range)ExcelWorkSheet.Columns[8]).ColumnWidth = 23;
                    ((Excel.Range)ExcelWorkSheet.Columns[9]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[10]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[11]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[12]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[13]).ColumnWidth = 8;
                    ((Excel.Range)ExcelWorkSheet.Columns[14]).ColumnWidth = 8;
                    ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Value2 = "Отчет-наряд по выполненным операциям";
                    ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).Value2 = "с " + DateStart.ToShortDateString() + " по " + DateEnd.ToShortDateString();
                    ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1:M1")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A2:M2")).Merge();
                    ExcelWorkSheet.Cells[4, 1] = "№";
                    ExcelWorkSheet.Cells[4, 2] = "ФИО исполнителя";
                    ExcelWorkSheet.Cells[4, 3] = "Заказ";
                    ExcelWorkSheet.Cells[4, 4] = "Поз.";
                    ExcelWorkSheet.Cells[4, 5] = "Наименование";
                    ExcelWorkSheet.Cells[4, 6] = "Обозначение";
                    ExcelWorkSheet.Cells[4, 7] = "Кол-во";
                    ExcelWorkSheet.Cells[4, 8] = "Операция";
                    ExcelWorkSheet.Cells[4, 9] = "Дата";
                    ((Excel.Range)ExcelWorkSheet.Columns[9]).NumberFormat = "m/d/yyyy";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 10]).Value2 = "н/ч за период (план)";
                    ((Excel.Range)ExcelWorkSheet.Columns[10]).NumberFormat = "[h]:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[10]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ExcelWorkSheet.Cells[4, 11] = "Табельное время";
                    ExcelWorkSheet.Cells[4, 12] = "Н/ч за период (факт)";
                    ExcelWorkSheet.Cells[4, 13] = "% вып. от пл. вр.";
                    ExcelWorkSheet.Cells[4, 14] = "% вып. от таб.вр.";
                    ExcelWorkSheet.Cells[4, 15] = "Н/ч за день";
                    string LastRep3Column = "";//NameColumn
                    TimeSpan tsAll = DateEnd - DateStart;
                    if (FlagDays) //Достраиваем если нужно таблицу справа
                    {
                        DateTime Day = DateStart;
                        //string LastRep3Column = "";//NameColumn
                        for (int i = 0; i <= tsAll.Days; i++) //формируем шапку
                        {
                            ((Excel.Range)ExcelWorkSheet.Cells[4, i + 16]).Value2 = Day.ToShortDateString();
                            Day = Day.AddDays(1);
                            ((Excel.Range)ExcelWorkSheet.Columns[i + 16]).ColumnWidth = 10;
                            ((Excel.Range)ExcelWorkSheet.Columns[i + 16]).HorizontalAlignment = Excel.Constants.xlCenter;
                            ((Excel.Range)ExcelWorkSheet.Columns[i + 16]).VerticalAlignment = Excel.Constants.xlCenter;
                            ((Excel.Range)ExcelWorkSheet.Columns[i + 16]).NumberFormat = "[h]:mm:ss";
                        }
                        LastRep3Column = ((Excel.Range)ExcelWorkSheet.Cells[4, tsAll.Days + 16]).Address;//NameColumn
                        LastRep3Column = LastRep3Column.Remove(0, 1);//NameColumn
                        LastRep3Column = LastRep3Column.Remove(LastRep3Column.IndexOf('$'));//NameColumn
                        (ExcelWorkSheet.get_Range("A4", LastRep3Column + "4")).Font.Bold = 1;
                        //MessageBox.Show(LastRep3Column);
                    }
                    else
                        LastRep3Column = "O";

                    #endregion
                    string Department = "";//Отдел
                    int num = 0;//Порядковый номер
                    string FIO = "";
                    Int16 Workers1 = 0;//кол-во рабочих внутри 1 отдела с полной ставкой;
                    Int16 Workers05 = 0;//кол-во рабочих внутри 1 отдела с 0,5 ставкой;
                    Int16 All_Workers1 = 0;//общее кол-во рабочих, кроме итр;
                    Int16 All_Workers05 = 0;//общее кол-во рабочих, кроме итр;
                    int NumRow = 5;
                    ulong PlanHoursDepartment = 0;//Всего по Участку
                    ulong AllPlanHours = 0; //Вообще всего
                    int FactTime = 0;//Н/ч за период (факт)
                    int FactTimeWorker = 0;//Для рабочего
                    ulong FactTimeDepartment = 0;//Всего по Участку
                    ulong AllFactTime = 0;//Вообще всего
                    int TimeSheetsSecDepartment = 0;//Общее время по табелю по Участку
                    int AllTimeSheetsSec = 0;//Вообще всего по табелю
                    int[] AllWorkerForDay = new int[tsAll.Days+1];//"Всего" по дням
                    int[] AllDepartmentForDay = new int[tsAll.Days + 1];//"Итого по участку" по дням
                    int[] AllForDay = new int[tsAll.Days + 1];//"Итого" по дням
                    ulong FactTimeDepartmentWithOutKoop = 0; //Всего по Участку без кооп
                    //int[] RowsAllDepartment = new int[10]; ;//Номера строк "Итого по участку" 10 - с запасом взял // пока есть 4 участка
                    //int RowsAllDep = 0; //Номер элемента в массиве RowsAllDepartment
                    for (int i = 0; i < _DT.Rows.Count-1; i++)
                    {
                        if (Department == "" || Department != _DT.Rows[i].ItemArray[2].ToString())//заголовок
                        {
                            (ExcelWorkSheet.get_Range("A" + NumRow.ToString(), "O" + NumRow.ToString())).Merge();
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Font.Bold = 1;
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = _DT.Rows[i].ItemArray[2].ToString();//Department
                            NumRow++;
                        }
                        //*********************************************************
                        
                        if (FIO == _DT.Rows[i].ItemArray[0].ToString())
                        {
                            (ExcelWorkSheet.get_Range("A" + (NumRow - 1).ToString(), "A" + (NumRow).ToString())).Merge();
                            (ExcelWorkSheet.get_Range("B" + (NumRow - 1).ToString(), "B" + (NumRow).ToString())).Merge();
                            (ExcelWorkSheet.get_Range((NumRow - 1) + ":" + (NumRow - 1))).Group();
                        }
                        else 
                        {
                            num++;
                            ExcelWorkSheet.Cells[NumRow, 1] = num;//№
                            ExcelWorkSheet.Cells[NumRow, 2] = _DT.Rows[i].ItemArray[0].ToString();//ФИО исполнителя - FK_LoginWorker
                        }
                        FIO = _DT.Rows[i].ItemArray[0].ToString();
                        //*********************************************************
                        ExcelWorkSheet.Cells[NumRow, 3] = _DT.Rows[i].ItemArray[7].ToString();//Заказ
                        ExcelWorkSheet.Cells[NumRow, 4] = _DT.Rows[i].ItemArray[8].ToString();//Позиция
                        ExcelWorkSheet.Cells[NumRow, 5] = _DT.Rows[i].ItemArray[9].ToString();//Наименование
                        ExcelWorkSheet.Cells[NumRow, 6] = _DT.Rows[i].ItemArray[10].ToString();//Обозначение
                        ExcelWorkSheet.Cells[NumRow, 7] = _DT.Rows[i].ItemArray[11].ToString();//Кол - во
                        ExcelWorkSheet.Cells[NumRow, 8] = _DT.Rows[i].ItemArray[12].ToString();//Операция
                        if (_DT.Rows[i].ItemArray[13].ToString() != "")//Дата
                            ExcelWorkSheet.Cells[NumRow, 9] = Convert.ToDateTime(_DT.Rows[i].ItemArray[13]);//Дата - DateFactOper
                        else ExcelWorkSheet.Cells[NumRow, 9] = "";
                        if (_DT.Rows[i].ItemArray[7].ToString() != "")//OrderNum = null, т.е. сотрудник ничего не делал и данные по нему есть только из табеля
                        {
                            //Tpd                                   //Tsh
                            if ((bool)_DT.Rows[i].ItemArray[16])//платим только раз от кол-ва не зависит
                                FactTime = (int)_DT.Rows[i].ItemArray[14] + (int)_DT.Rows[i].ItemArray[15]; //Tpd+Tsh
                            else                     //Tpd                                //Tsh                         //Amount
                                FactTime = (int)_DT.Rows[i].ItemArray[14] + ((int)_DT.Rows[i].ItemArray[15] * (int)_DT.Rows[i].ItemArray[11]);//Tpd+(Tsh/Amount)
                            FactTime = FactTime / (int)_DT.Rows[i].ItemArray[5];//Кол-во человек в бригаде
                        }
                        else FactTime = 0;
                        ExcelWorkSheet.Cells[NumRow, 12] = IntToTime(FactTime);
                        FactTimeWorker += FactTime;
                        //**************************Подсчёт по дням**************************
                        if (FlagDays & FactTime>0 & _DT.Rows[i].ItemArray[13].ToString() != "")
                        {
                            TimeSpan tsRow = Convert.ToDateTime(_DT.Rows[i].ItemArray[13]) - DateStart;
                            ExcelWorkSheet.Cells[NumRow, tsRow.Days + 16] = IntToTime(FactTime);
                            AllWorkerForDay[tsRow.Days] += FactTime;
                            //((Excel.Range)ExcelWorkSheet.Cells[NumRow, tsRow.Days + 16]).Value2 = IntToTime(AllForDay[tsRow.Days]);
                        }
                        //**************************Подсчёт по дням(END)**************************
                        NumRow++;
                        //Всего*********************************************************
                        if (FIO != "" & FIO != _DT.Rows[i + 1].ItemArray[0].ToString())
                        {
                            int _PlanHours = 0;
                            decimal TimeSheets = 0;//Время по табелю за 1 деь на 1 рабочего
                            ExcelWorkSheet.Cells[NumRow, 8] = "Всего";
                            //Н/ч за период (факт)
                            if (Convert.ToInt32(_DT.Rows[i].ItemArray[1]) != 1)//if not "ИТР, специалисты, служащий персонал цеха"
                            {
                                if (_DT.Rows[i].ItemArray[4].ToString() == "1")
                                {
                                    Workers1++;//кол-во рабочих внутри 1 отдела;
                                    All_Workers1++;//общее кол-во рабочих, кроме итр;
                                }
                                else
                                {
                                    Workers05++;
                                    All_Workers05++;
                                }
                                //***********************************************************
                                Double RateWorker = Convert.ToDouble(_DT.Rows[i].ItemArray[4]);//Ставка работника (1 или 0.5)
                                DateTime _DateEnd = Convert.ToDateTime("1900-01-01");//уволился
                                if (_DT.Rows[i].ItemArray[18].ToString() != "") _DateEnd = Convert.ToDateTime(_DT.Rows[i].ItemArray[18]);//Дата увольнения
                                if (DateStart <= Convert.ToDateTime(_DT.Rows[i].ItemArray[3]) &
                                    (_DateEnd.Year > 1900 & _DateEnd <= DateEnd)  //Устроился позже - уволился раньше _DateEnd = DateEnd;
                                    )
                                {
                                    _PlanHours = NormHoursPlanFromOneWorker(Convert.ToDateTime(_DT.Rows[i].ItemArray[3]), _DateEnd);
                                    _PlanHours = Convert.ToInt32(_PlanHours * RateWorker * 1.08);//Добавляем коэффициент 1.08
                                }
                                else
                                    if (DateStart <= Convert.ToDateTime(_DT.Rows[i].ItemArray[3]))//Устроился позже даты начала отчёта
                                {
                                    
                                    //Для сотрудника который устроился в середине отчётного месяца
                                    _DateEnd = DateEnd.AddDays(1);
                                    _PlanHours = NormHoursPlanFromOneWorker(Convert.ToDateTime(_DT.Rows[i].ItemArray[3]), _DateEnd);
                                    _PlanHours = Convert.ToInt32(_PlanHours * RateWorker * 1.08);//Добавляем коэффициент 1.08
                                }
                                    else
                                        if (_DateEnd.Year > 1900 & _DateEnd <= DateEnd)//Уволился раньше даты окончания отчёта
                                        {
                                            _PlanHours = NormHoursPlanFromOneWorker(DateStart, _DateEnd);
                                            _PlanHours = Convert.ToInt32(_PlanHours * RateWorker * 1.08);//Добавляем коэффициент 1.08
                                        }
                                        else
                                            _PlanHours = Convert.ToInt32(PlanHours * RateWorker * 1.08);//Добавляем коэффициент 1.08

                                ExcelWorkSheet.Cells[NumRow, 10] = IntToTime(_PlanHours);//н/ч за период (план)
                                //TimeSheets = TimeSheetsFromOneWorker(FIO,DateStart,DateEnd);
                                // Тут я пофиксил время до простой и работающей формулы
                                if (decimal.TryParse(_DT.Rows[i].ItemArray[17].ToString(), out TimeSheets))
                                {
                                    double time = Convert.ToDouble(TimeSheets) * 3600;
                                    ExcelWorkSheet.Cells[NumRow, 11] = IntToTime(Convert.ToInt32(time)); //// ТУТ!!!!
                                    TimeSheetsSecDepartment += Convert.ToInt32(time);
                                }
                                else TimeSheets = 0;
                                //**********************************************************
                            }
                            else
                            {
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 10]).NumberFormat = "";
                                ExcelWorkSheet.Cells[NumRow, 10] = 0;
                                _PlanHours = 0;
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 11]).NumberFormat = "";
                                ExcelWorkSheet.Cells[NumRow, 11] = 0;
                                TimeSheets = 0;
                            }
                            PlanHoursDepartment += (ulong)_PlanHours;
                            //Н/ч за период (факт)
                            ExcelWorkSheet.Cells[NumRow, 12] = IntToTime(FactTimeWorker);
                            FactTimeDepartment += (ulong)FactTimeWorker;
                            FactTimeDepartmentWithOutKoop += _DT.Rows[i].ItemArray[0].ToString() == "кооп" ? 0 :(ulong)FactTimeWorker;
                            //% выполнения
                            if (_PlanHours > 0 & TimeSheets>0)
                            {
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 13]).Value2 = Math.Round(((float)FactTimeWorker / _PlanHours) * 100,2);
                                if (decimal.TryParse(_DT.Rows[i].ItemArray[17].ToString(), out TimeSheets))
                                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 14]).Value2 = Math.Round((FactTimeWorker / DecimalToSec(TimeSheets)) * 100, 2);
                            }
                            else
                            {
                                ExcelWorkSheet.Cells[NumRow, 13] = 0;
                                ExcelWorkSheet.Cells[NumRow, 14] = 0;
                            }
                            FactTimeWorker = 0;
                            //Подсчёт по дням****************************************************
                            if (FlagDays)
                                for (int m = 0; m < AllWorkerForDay.Length; m++)
                            {
                                if (AllWorkerForDay[m] > 0)
                                {
                                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, m + 16]).Value2 = IntToTime(AllWorkerForDay[m]);
                                    AllDepartmentForDay[m] += AllWorkerForDay[m];
                                    AllWorkerForDay[m] = 0;
                                }
                            }
                            //Подсчёт по дням(END)***********************************************
                            (ExcelWorkSheet.get_Range("A" + (NumRow - 1).ToString(), "A" + (NumRow).ToString())).Merge();
                            (ExcelWorkSheet.get_Range("B" + (NumRow - 1).ToString(), "B" + (NumRow).ToString())).Merge();
                            (ExcelWorkSheet.get_Range((NumRow - 1) + ":" + (NumRow - 1))).Group();
                            (ExcelWorkSheet.get_Range("A" + (NumRow).ToString(), LastRep3Column + (NumRow).ToString())).Font.Bold = 1;
                            NumRow++;
                        }
                        //Всего(END)*********************************************************
                        //Итого по участку*********************************************************
                        if (Department == "" || Department != _DT.Rows[i].ItemArray[2].ToString())
                        {
                            Department = _DT.Rows[i].ItemArray[2].ToString();
                        }
                        if (Department != "" & Department != _DT.Rows[i+1].ItemArray[2].ToString())
                            {
                                if (PlanHoursDepartment == 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 10]).NumberFormat = "";
                                ExcelWorkSheet.Cells[NumRow, 2] = "Итого по участку";
                                ExcelWorkSheet.Cells[NumRow, 8] = "Итого по участку";
                                ExcelWorkSheet.Cells[NumRow, 10] = IntToTime(PlanHoursDepartment);//н/ч за период (план)
                                ExcelWorkSheet.Cells[NumRow, 11] = IntToTime(TimeSheetsSecDepartment);//Часы по табелю
                                ExcelWorkSheet.Cells[NumRow, 12] = IntToTime(FactTimeDepartment);//Н/ч за период (факт)
                                //% выполнения
                                if (PlanHoursDepartment > 0)
                                {
                                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 13]).Value2 = Math.Round(((float)FactTimeDepartment / PlanHoursDepartment) * 100, 2);
                                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 14]).Value2 = Math.Round(((float)FactTimeDepartment / (float)TimeSheetsSecDepartment) * 100, 2);
                                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow - 1, 14]).Value2 = Math.Round(((float)FactTimeDepartment / (float)TimeSheetsSecDepartment) * 100, 2);
                            }
                                else
                                {
                                    ExcelWorkSheet.Cells[NumRow, 13] = 0;
                                    ExcelWorkSheet.Cells[NumRow, 14] = 0;
                                }
                                AllPlanHours += PlanHoursDepartment;//н/ч за период (план)
                                AllFactTime += (koop == false ? FactTimeDepartment : FactTimeDepartmentWithOutKoop);//Н/ч за период (факт)
                                AllTimeSheetsSec += TimeSheetsSecDepartment;//Время по табелю
                                PlanHoursDepartment = 0;
                                FactTimeDepartment = 0;
                                TimeSheetsSecDepartment = 0;
                                FactTimeDepartmentWithOutKoop = 0;
                                //Подсчёт по дням****************************************************
                                if (FlagDays)
                                {
                                    DateTime SundayOrSaturday;

                                    for (int m = 0; m < AllDepartmentForDay.Length; m++)
                                    {
                                        if (AllDepartmentForDay[m] > 0)
                                        {
                                            ExcelWorkSheet.Cells[NumRow, m + 16] = IntToTime(AllDepartmentForDay[m]);//Итого по участку за каждый день
                                            //"Вып. по участку";
                                            SundayOrSaturday = Convert.ToDateTime(((Excel.Range)ExcelWorkSheet.Cells[4, m + 16]).Value2);
                                            if (SundayOrSaturday.DayOfWeek.ToString() != "Saturday" & SundayOrSaturday.DayOfWeek.ToString() != "Sunday")
                                                if (FlagDays & IdCeh == -1 & loginWorker == "" & cWorkDays > 0 & (ExcelWorkSheet.Cells[NumRow - 1, 10] as Excel.Range).Value2 != 0)
                                            {
                                                //string adrcell = ((Excel.Range)ExcelWorkSheet.Cells[NumRow, m + 16]).Address;
                                                //double adrcell = ((Excel.Range)ExcelWorkSheet.Cells[NumRow, m + 16]).Value;
                                                //((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, m + 16]).NumberFormat = "";
                                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, m + 16]).NumberFormat = "0.00%";
                                                //((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, m + 16]).Value2 = "=(" + adrcell + "/(" + "J" + (NumRow) + "/" + cWorkDays + "))";//где J это - н/ч за период (план)
                                                //((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, m + 16]).Value2 = "=(" + adrcell + "/(" + "L" + (NumRow) + "/" + cWorkDays + "))";//где L это - Н/ч за период (факт)
                                                //((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, m + 16]).Value2 = "=(" + adrcell + "/(" + (double)AmountWorkers * 8.64 + "))";//где L это - Н/ч за период (факт)
                                                double Proc = (double)AllDepartmentForDay[m] / ((double)Workers1 * 28800 * 1.08 + (double)Workers05 * 14400 * 1.08);
                                                ExcelWorkSheet.Cells[NumRow + 1, m + 16] = Proc;//где L это - Н/ч за период (факт)
                                            }

                                            if (_DT.Rows[i].ItemArray[0].ToString() == "кооп" & koop == true)
                                            {
                                                AllForDay[m] += 0;
                                                AllDepartmentForDay[m] = 0;
                                            }
                                            else
                                            {
                                                AllForDay[m] += AllDepartmentForDay[m];
                                                AllDepartmentForDay[m] = 0;
                                            }
                                        }
                                    }
                                    Workers1 = 0; Workers05 = 0;
                                }
                                //Подсчёт по дням(END)***********************************************
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow).ToString(), LastRep3Column + (NumRow).ToString())).Font.Bold = 1;
                                NumRow++;
                                //Строка для выработки******************************
                                //if (FlagDays && IdCeh == -1 && loginWorker == "" && cWorkDays > 0 && (ExcelWorkSheet.Cells[NumRow - 1, 10] as Excel.Range).Value2 != 0)
                                if (FlagDays && IdCeh == -1 && loginWorker == "" && cWorkDays > 0)
                                {
                                    string x = Convert.ToString((ExcelWorkSheet.Cells[NumRow - 1, 10] as Excel.Range).Value2);
                                if (x != null)
                                {
                                    //RowsAllDepartment[RowsAllDep] = NumRow;//int[] RowsAllDepartment = new int[10]; ;//Номера строк "Итого по участку" 10 - с запасом взял // пока есть 4 участка
                                    //RowsAllDep++;//int RowsAllDep = 0; //Номер элемента в массиве RowsAllDepartment
                                    ExcelWorkSheet.Cells[NumRow, 2] = "Вып. по участку";
                                    NumRow++;//String for %
                                }
                                }
                                //Строка для выработки(END)******************************
                            }
                        //Итого по участку(END)*********************************************************
                        
                    }//for(END)
                    //Сетка*************************************************
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (4).ToString(), LastRep3Column + (NumRow - 1).ToString())).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (4).ToString(), LastRep3Column + (NumRow - 1).ToString())).VerticalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (4).ToString(), LastRep3Column + (NumRow - 1).ToString())).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (4).ToString(), LastRep3Column + (NumRow - 1).ToString())).WrapText = true;
                    //Сетка(END)********************************************
                    //ИТОГО*******************************************************
                    NumRow++;
                    ExcelWorkSheet.Cells[NumRow, 2] = "Всего по цеху";
                    ExcelWorkSheet.Cells[NumRow, 8] = "Итого";
                    ExcelWorkSheet.Cells[NumRow, 10] = IntToTime(AllPlanHours);//н/ч за период (план)
                    ExcelWorkSheet.Cells[NumRow, 11] = IntToTime(AllTimeSheetsSec);//Табельное время
                    ExcelWorkSheet.Cells[NumRow, 12] = IntToTime(AllFactTime);//Н/ч за период (факт)
                    
                    
                    //% выполнения
                    if (AllPlanHours > 0)
                    {
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 13]).Value2 = Math.Round(((float)AllFactTime / AllPlanHours) * 100, 2);
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 14]).Value2 = Math.Round(((float)AllFactTime / (float)AllTimeSheetsSec) * 100, 2);
                    }
                    else
                    {
                        ExcelWorkSheet.Cells[NumRow, 13] = 0;
                        ExcelWorkSheet.Cells[NumRow, 14] = 0;
                    }
                    //Подсчёт по дням ИТОГО****************************************************
                    if (FlagDays)
                        for (int m = 0; m < AllForDay.Length; m++)
                        {
                            if (AllForDay[m] > 0)
                            {
                                ExcelWorkSheet.Cells[NumRow, m + 16] = IntToTime(AllForDay[m]);
                                //AllForDay[m] = 0;
                            }
                        }
                    //Подсчёт по дням(END)***********************************************
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow).ToString(), LastRep3Column + (NumRow).ToString())).Font.Bold = 1;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow).ToString(), LastRep3Column + (NumRow).ToString())).HorizontalAlignment = Excel.Constants.xlCenter;
                    //Строка для выработки******************************
                    //int[] RowsAllDepartment = new int[10]; ;//Номера строк "Итого по участку" 10 - с запасом взял // пока есть 4 участка
                    //int RowsAllDep = 0; //Номер элемента в массиве RowsAllDepartment
                    /*if (FlagDays & IdCeh == -1 & loginWorker == "" & cWorkDays > 0)
                    {
                        for (int rad = 0; rad < RowsAllDepartment.Length; rad++)
                        {
                            if (RowsAllDepartment[rad]>0)
                                if((ExcelWorkSheet.Cells[RowsAllDepartment[rad] - 1, 10] as Excel.Range).Value2 != 0)
                            for (int m = 0; m < AllForDay.Length; m++)
                            {
                                if (AllForDay[m] > 0 & (ExcelWorkSheet.Cells[RowsAllDepartment[rad] - 1, m + 14] as Excel.Range).Value2 != null)
                                {
                                    string adrcell = ((Excel.Range)ExcelWorkSheet.Cells[RowsAllDepartment[rad] - 1, m + 14]).Address;
                                    ((Excel.Range)ExcelWorkSheet.Cells[RowsAllDepartment[rad], m + 14]).NumberFormat = "0.00%";
                                    ((Excel.Range)ExcelWorkSheet.Cells[RowsAllDepartment[rad], m + 14]).Value2 = "=(" + adrcell + "/(" + "J" + (NumRow) + "/" + cWorkDays + "))";
                                }
                            }
                        }
                    }*/
                    //ИТОГО(END)*******************************************************
                    //Последняя строка - Сразу после Итого
                    if (FlagDays & IdCeh == -1 & loginWorker == "" & cWorkDays > 0 & (ExcelWorkSheet.Cells[NumRow - 1, 10] as Excel.Range).Value2 != 0)
                    {
                        NumRow++;
                        ExcelWorkSheet.Cells[NumRow, 2] = "Выполнение по цеху";
                        DateTime SundayOrSaturday;
                        for (int m = 0; m < AllForDay.Length; m++)
                        {
                            SundayOrSaturday = Convert.ToDateTime(((Excel.Range)ExcelWorkSheet.Cells[4, m + 16]).Value2);
                            if (AllForDay[m] > 0 & (ExcelWorkSheet.Cells[NumRow - 1, m + 16] as Excel.Range).Value2 != null 
                                & SundayOrSaturday.DayOfWeek.ToString() != "Saturday" & SundayOrSaturday.DayOfWeek.ToString() != "Sunday")
                            {
                                string adrcell = ((Excel.Range)ExcelWorkSheet.Cells[NumRow - 1, m + 16]).Address;
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, m + 16]).NumberFormat = "0.00%";
                                //((Excel.Range)ExcelWorkSheet.Cells[NumRow, m + 16]).Value2 = "=(" + adrcell + "/(" + "J" + (NumRow - 1) + "/" + cWorkDays + "))";//где J это - н/ч за период (план)
                                //((Excel.Range)ExcelWorkSheet.Cells[NumRow, m + 16]).Value2 = "=(" + adrcell + "/(" + "L" + (NumRow - 1) + "/" + cWorkDays + "))";//где L это - Н/ч за период (факт)
                                //((Excel.Range)ExcelWorkSheet.Cells[NumRow, m + 16]).Value2 = "=(" + adrcell + "/J" + (NumRow - 1) + ")";//где L это - Н/ч за период (факт)
                                double Proc = (double)AllForDay[m] / ((double)All_Workers1 * 28800 * 1.08 + (double)All_Workers05 * 14400 * 1.08);
                                ExcelWorkSheet.Cells[NumRow, m + 16] = Proc;//где L это - Н/ч за период (факт)
                            }
                            AllForDay[m] = 0;
                        }
                        All_Workers1 = 0; All_Workers05 = 0;
                    }
                    //Строка для выработки(END)******************************
                    

                    ExcelWorkSheet.Cells[NumRow + 2, 2] = "Сдал:";
                    ExcelWorkSheet.Cells[NumRow + 3, 2] = "   Оператор ИАЦ _______________________ /Тычинина Е.С./";
                    ExcelWorkSheet.Cells[NumRow + 4, 2] = "Принял:";
                    ExcelWorkSheet.Cells[NumRow + 5, 2] = "   Начальник мех. участка   _______________________ /Уткин В.В./";
                    ExcelWorkSheet.Cells[NumRow + 7, 2] = "   Начальник слес. участка _______________________ /Сотников Ю.И./";
                    ExcelWorkSheet.Cells[NumRow + 9, 2] = "   Начальник цеха              _______________________ /___________________/";
                    ExcelWorkSheet.Cells[NumRow + 11, 2] = "   Начальник производства _______________________ /Казьмин А.В./";

                    
                    MessageBox.Show("Формирование отчета завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public int NormHoursPlan(DateTime DateStart, DateTime DateEnd, ref int cWorkDays)
        {
            try
            {
                int sec = 0;
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//using System.Data.SqlClient;
                    SqlDataReader reader;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT SUM(Dsec) as AllSec,count(PK_Date) as WorkDays" + "\n" +
                        "FROM Sp_ProductionCalendar" + "\n" +
                        "Where Dsec>0 and PK_Date >= @DateStart and PK_Date <= @DateEnd";
                    cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date));
                    cmd.Parameters["@DateStart"].Value = DateStart;
                    cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date));
                    cmd.Parameters["@DateEnd"].Value = DateEnd;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0)) sec = reader.GetInt32(0); else sec = 0;
                            if (!reader.IsDBNull(1)) cWorkDays = reader.GetInt32(1); else cWorkDays = 0;
                        }
                    }
                    reader.Dispose(); reader.Close(); con.Close();
                }
                return sec;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        public int NormHoursPlanFromOneWorker(DateTime DateStart, DateTime DateEnd)
        {
            int PlanHours = 0;
            try
            {


                using (SqlConnection con = new SqlConnection())
                {

                    con.ConnectionString = config.ConnectionString;

                    string sqlExpression = "NormHoursPlanFromOneWorker";

                    SqlCommand cmd = new SqlCommand(sqlExpression) { CommandTimeout = 60 };

                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DateStart", DateStart);
                    cmd.Parameters.AddWithValue("@DateEnd", DateEnd);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(0) == false) PlanHours = reader.GetInt32(0);
                            }
                        }
                    }
                }
                return PlanHours;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        public decimal TimeSheetsFromOneWorker(string FIO, DateTime DateStart, DateTime DateEnd)
        {
            try
            {
                decimal PlanHours = 0;

                
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT sum((convert( decimal(4,1), Val_Time))) as Val_Time" + "\n" +
                                      "FROM TimeSheets" + "\n" +
                                      "Where FK_Login = @FK_Login and Val_Time != '' and Val_Time <='8' and PK_Date>=@DateStart and PK_Date<=@DateEnd";
                    cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                    cmd.Parameters["@FK_Login"].Value = FIO;
                    cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date));
                    cmd.Parameters["@DateStart"].Value = DateStart;
                    cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date));
                    cmd.Parameters["@DateEnd"].Value = DateEnd;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (reader.IsDBNull(0) == false) PlanHours = reader.GetDecimal(0);
                            }
                        }
                    }
                }
                return PlanHours;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
        

        #endregion Отчёт-наряд по выполненным операциям
        //***********************************************************************************************
        #region отчёт "Движение деталей"
        public void rep6(int PK_IdOrder, string OrderNum, string OrderName)
        {
            try
            {
                /*_DT.Clear();
                using (con)
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//using System.Data.SqlClient;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select PK_IdOrderDetail,FK_IdDetail,ShcmDetail,NameDetail,AmountDetails,Position,PositionParent" + "\n" +
                                      "From OrdersDetails" + "\n" +
                                      "left Join Sp_Details on Sp_Details.PK_IdDetail=OrdersDetails.FK_IdDetail" + "\n" +
                                      "Where FK_IdOrder = @PK_IdOrder" + "\n" +
                                      //"Order by Position";
                                      "Order by ShcmDetail";
                    cmd.Parameters.Add(new SqlParameter("@PK_IdOrder", SqlDbType.Int));
                    cmd.Parameters["@PK_IdOrder"].Value = PK_IdOrder;
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT);
                    adapter.Dispose();
                }*/
                AllDetailsInOrder(PK_IdOrder, "rep6");//Все детали заказа

                if (_DT.Rows.Count == 0) MessageBox.Show("Нет данных для формирования отчёта.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else//Export data to Excel
                {
                    Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                    //XlReferenceStyle RefStyle = Excel.ReferenceStyle; Excel.Visible = true;
                    ExcelApp.Workbooks.Add(1);
                    Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);
                    //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove; //Сворачиваем сверху
                    #region Настройка шапки
                    ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.Application.StandardFontSize = 10;
                    ExcelWorkSheet.Application.Cells.HorizontalAlignment = Excel.Constants.xlCenter;
                    ////Редактирование созданного документа
                    ((Excel.Range)ExcelWorkSheet.Columns[1]).ColumnWidth = 6;
                    ((Excel.Range)ExcelWorkSheet.Columns[2]).ColumnWidth = 20;
                    ((Excel.Range)ExcelWorkSheet.Columns[2]).HorizontalAlignment = Excel.Constants.xlLeft;
                    ((Excel.Range)ExcelWorkSheet.Columns[3]).ColumnWidth = 25;
                    ((Excel.Range)ExcelWorkSheet.Columns[3]).HorizontalAlignment = Excel.Constants.xlLeft;
                    ((Excel.Range)ExcelWorkSheet.Columns[4]).ColumnWidth = 6;
                    ((Excel.Range)ExcelWorkSheet.Columns[5]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[6]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[7]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[8]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1:H1")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A2:H2")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A3:H3")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1", "H3")).Font.Size = 11;
                    ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Value2 = "Отчет о движении деталей по операциям";                     
                    ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).Value2 = "Заказ: " + OrderName;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 = "на " + DateTime.Now;
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 1]).Value2 = "Поз. L";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 2]).Value2 = "Обозначение";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 2]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 3]).Value2 = "Наименование";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 3]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 4]).Value2 = "Кол-во";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 5]).Value2 = "Вып/Опер";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 6]).Value2 = "Норма план.";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 7]).Value2 = "Норма факт.";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 8]).Value2 = "Дата";
                    ((Excel.Range)ExcelWorkSheet.get_Range("A5", "H5")).Font.Bold = 1;
                    ExcelWorkSheet.PageSetup.PrintTitleRows = "$5:$5";
                    ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove;//группировка строк
                    //************************************
                    #endregion Настройка шапки
                    //Заполняем таблицу
                    int NumRow = 6;
                    int MaxColumn = 0;

                    


                    //int NumСolumn = 1;
                    loadTreeSubMenu(ref NumRow,1, 0, ExcelWorkSheet, ref MaxColumn, "rep6");

                    string LetterMaxColumn = ((Excel.Range)ExcelWorkSheet.Columns[MaxColumn-1]).Address;
                    LetterMaxColumn = LetterMaxColumn.Remove(0, 1).Remove(LetterMaxColumn.IndexOf(":") - 1);
                    ((Excel.Range)ExcelWorkSheet.get_Range("I1", LetterMaxColumn + 1)).ColumnWidth = 25;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1", LetterMaxColumn + (NumRow - 1))).WrapText = true;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A5", LetterMaxColumn + (NumRow - 1))).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    //((Excel.Range)ExcelWorkSheet.get_Range("A1", LetterMaxColumn + (_DT.Rows.Count - 2))).WrapText = true;
                    //((Excel.Range)ExcelWorkSheet.get_Range("A5", LetterMaxColumn + (_DT.Rows.Count - 2))).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    //((Excel.Range)ExcelWorkSheet.get_Range((NumRow - 1) + ":" + (NumRow - 1))).Group();

                    //************************************
                    MessageBox.Show("Формирование отчета завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LoadTehDetails(Int64 FK_IdOrderDetail, Int64 FK_IdDetails, int row, Excel.Worksheet exW, ref int MaxColumn)
        {
            try
            {
                
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "Select NumOper+'999' as NumOper,NameOperation,NumSpOper,Tpd,Tsh,sum(AmountDetails) as AmountDetailsFact, 'f' as FactOrSp, OnlyOncePay, max(DateFactOper) as DateFactOper " + "\n" +
                                        "From FactOperation " + "\n" +
                                        "inner join Sp_Operations on PK_IdOperation = FK_IdOperation " + "\n" +
                                        "where FK_IdOrderDetail = @FK_IdOrderDetail " + "\n" +
                                        "group by NumOper,NameOperation,NumSpOper,Tpd,Tsh,OnlyOncePay " + "\n" +
                                        "union all" + "\n" +
                                        "Select NumOper+'999' as NumOper,NameOperation,NumSpOper,Tpd,Tsh,0 as AmountDetails, 's' as FactOrSp, OnlyOncePay, '1983.04.01' as DateFactOper " + "\n" +
                                        "From Sp_TechnologyDetails " + "\n" +
                                        "inner join Sp_Operations on PK_IdOperation = FK_IdOperation " + "\n" +
                                        "Where FK_IdDetails = @FK_IdDetails " + "\n" +
                                        "union all" + "\n" +
                                        "Select NumOperation+'999' as NumOper,NameOperation,NumSpOper,Tpd,Tsh,0 as AmountDetails, 's' as FactOrSp, OnlyOncePay, '1983.04.01' as DateFactOper " + "\n" +
                                        "From Sp_OperationsType111" + "\n" +
                                        "inner join Sp_Operations on PK_IdOperation = FK_IdOperation" + "\n" +
                                        "Where FK_IdDetail = @FK_IdDetails" + "\n" +
                                        "union all" + "\n" +
                                        "Select '999','Передача детали на СГД',32,0,0,0 as AmountDetails, 's' as FactOrSp, 1 as OnlyOncePay, '1983.04.01' as DateFactOper " + "\n" +
                                        "Order by NumOper,FactOrSp";
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOrderDetail", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdOrderDetail"].Value = FK_IdOrderDetail;
                    cmd.Parameters.Add(new SqlParameter("@FK_IdDetails", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdDetails"].Value = FK_IdDetails;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            int num_column = 9;
                            string NumOper = "";
                            int PlanTime = 0;//Норма план.
                            int FactTime = 0;//Норма факт.
                            int NumSpOper = 0;
                            int Tpd = 0, Tsh = 0;
                            int AmountDetailsFact = 0;
                            string FactOrSp = "";
                            bool OnlyOncePay = true;//Платим только 1 раз, без учета деталей Tpd + Tsh
                            Int16 CountFactOper = 0;//max количество выполненных типов операций 
                            Color Clr = Color.White;
                            bool paint = false;
                            string Shcm = ((Excel.Range)exW.Cells[row, 2]).Value2;
                            int indS = 0;//Для раскрашивания
                            //Определяем базовый цвет
                            indS = Shcm.IndexOf(".");
                            if (indS > 0)
                            {
                                char num = Shcm[indS - 1];
                                if (num == '2' || num == '3') Clr = Color.Orange;//Сборка (большой узел)
                                else
                                    if (num == '4' || num == '5' || num == '6') Clr = Color.LightSkyBlue;//Сборка (маленький узел)
                                    else Clr = Color.LightPink;
                            }
                            else Clr = Color.White;
                            //*****************************
                            while (reader.Read())
                            {
                                
                                if (!reader.IsDBNull(3)) Tpd = reader.GetInt32(3); else Tpd = 0;
                                if (!reader.IsDBNull(4)) Tsh = reader.GetInt32(4); else Tsh = 0;
                                
                                if (!reader.IsDBNull(6)) FactOrSp = reader.GetString(6); else FactOrSp = "";
                                if (!reader.IsDBNull(7))
                                    if (reader.GetInt32(7) == 1) OnlyOncePay = true; else OnlyOncePay = false;
                                if (!reader.IsDBNull(0) & !reader.IsDBNull(1))
                                {
                                    NumOper = reader.GetString(0);
                                    if (NumOper != "999") NumOper = NumOper.Remove(3);
                                    else NumOper = "";
                                    if (num_column == 9 & reader.GetString(1).Trim() == "Передача детали на СГД")
                                    { }
                                    else
                                    {
                                        if (!paint)//Закрашиваем названия
                                        {
                                        if (Clr == Color.White) Clr = Color.LightPink;
                                        ((Excel.Range)exW.get_Range("A" + row, "H" + row)).Interior.Color = Clr;
                                        paint = true;
                                        }
                                        if (FactOrSp == "f")//Операции по факту
                                        {
                                            if (!reader.IsDBNull(2)) NumSpOper = reader.GetInt32(2); else NumSpOper = 0;
                                            if (!reader.IsDBNull(5)) AmountDetailsFact = reader.GetInt32(5); else AmountDetailsFact = 0;
                                            FactTime += NormTimeFabrication(OnlyOncePay, Tpd, Tsh, AmountDetailsFact);

                                            
                                            if (NumSpOper == 32 & AmountDetailsFact == (int)((Excel.Range)exW.Cells[row, 4]).Value2)// "Передача детали на СГД"
                                            {
                                                if (!reader.IsDBNull(8)) ((Excel.Range)exW.Cells[row, 8]).Value2 = reader.GetDateTime(8).ToShortDateString();
                                                if (Clr == Color.LightPink)
                                                {
                                                    ((Excel.Range)exW.get_Range("A" + row, "H" + row)).Interior.Color = Color.LightGreen;
                                                    ((Excel.Range)exW.Cells[row, num_column]).Interior.Color = Color.LightGreen;
                                                }
                                                else
                                                    ((Excel.Range)exW.Cells[row, num_column]).Interior.Color = Clr;
                                                /*if (Clr == Color.LightPink)
                                                for (int i = 1; i <= num_column; i++)
                                                {                                                           //Color.LightPink
                                                    if (((Excel.Range)exW.Cells[row, i]).Interior.Color == 12695295) ((Excel.Range)exW.Cells[row, i]).Interior.Color = Color.LightGreen;
                                                }*/
                                            }
                                            else
                                                if (AmountDetailsFact == (int)((Excel.Range)exW.Cells[row, 4]).Value2 & Clr == Color.LightPink)
                                            {
                                                ((Excel.Range)exW.Cells[row, num_column]).Interior.Color = Color.LightGreen;
                                            }
                                                else
                                                    ((Excel.Range)exW.Cells[row, num_column]).Interior.Color = Clr;
                                           
                                            CountFactOper++;
                                            
                                            
                                            
                                            //((Excel.Range)exW.Cells[row, 3]).Value2 = ((Excel.Range)exW.Cells[row, 3]).Value2 + "//" + AmountDetailsFact;
                                        }
                                        else
                                        {
                                            
                                            PlanTime += NormTimeFabrication(OnlyOncePay, Tpd, Tsh, (int)((Excel.Range)exW.Cells[row, 4]).Value2);
                                            ((Excel.Range)exW.Cells[row, num_column]).Value2 = NumOper + " " + reader.GetString(1);
                                            num_column++;
                                        }
                                    }
                                }
                                //num_column++;
                            }
                            ((Excel.Range)exW.Cells[row, 5]).Value2 = CountFactOper +";"+(num_column-9);
                            ((Excel.Range)exW.Cells[row, 6]).Value2 = IntToTime(PlanTime);
                            ((Excel.Range)exW.Cells[row, 7]).Value2 = IntToTime(FactTime);
                            if (MaxColumn < num_column) MaxColumn = num_column;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        //***********************************************************************************************
        private void AllDetailsInOrder(int PK_IdOrder, string NameRep)
        {
            try
            {
                _DT.Clear();
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//using System.Data.SqlClient;
                    cmd.Parameters.Clear();
                    if (NameRep == "rep6")
                    {
                        cmd.CommandText = "Select PK_IdOrderDetail,FK_IdDetail,ShcmDetail,NameDetail,AmountDetails,Position,PositionParent" + "\n" +
                                          "From OrdersDetails" + "\n" +
                                          "left Join Sp_Details on Sp_Details.PK_IdDetail=OrdersDetails.FK_IdDetail" + "\n" +
                                          "Where FK_IdOrder = @PK_IdOrder" + "\n" +
                            //"Order by Position";
                                          "Order by ShcmDetail";
                    }
                    if (NameRep == "Form6")
                    {
                        cmd.CommandText = "Select od.PK_IdOrderDetail,od.FK_IdDetail,ShcmDetail,NameDetail,od.AmountDetails,od.Position,od.PositionParent,od.AllPositionParent,cod.AllCountDetails" + "\n" +
                                          "From OrdersDetails od" + "\n" +
                                          "Inner Join Sp_Details ON Sp_Details.PK_IdDetail=od.FK_IdDetail" + "\n" +
                                          "Inner Join vwCountOrdersDetails cod ON cod.FK_IdOrder = od.FK_IdOrder and cod.FK_IdDetail = od.FK_IdDetail" + "\n" +
                                          "Where od.FK_IdOrder = @PK_IdOrder" + "\n" +
                                          //"Order by Position";
                                          "Order by ShcmDetail";
                    }
                    cmd.Parameters.Add(new SqlParameter("@PK_IdOrder", SqlDbType.Int));
                    cmd.Parameters["@PK_IdOrder"].Value = PK_IdOrder;
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT);
                    adapter.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadTreeSubMenu(ref int row, int StartColumn, int ParentId, Excel.Worksheet exW, ref int MaxColumn, string NameRep)
        {
            DataRow[] childs = _DT.Select("PositionParent='" + ParentId + "'");
            int RowStart = row;
            string Shcm = "";
            foreach (DataRow dRow in childs)
            {
                ((Excel.Range)exW.Cells[row, StartColumn]).Value2 = dRow["Position"].ToString();
                Shcm = dRow["ShcmDetail"].ToString();
                ((Excel.Range)exW.Cells[row, StartColumn+1]).Value2 = Shcm;
                ((Excel.Range)exW.Cells[row, StartColumn+2]).Value2 = dRow["NameDetail"].ToString();
                //((Excel.Range)exW.Cells[row, 4]).Value2 = dRow["PositionParent"].ToString();
                
                //*****************************
                if (NameRep == "rep6")//StartColumn=1
                {
                    ((Excel.Range)exW.Cells[row, StartColumn+3]).Value2 = dRow["AmountDetails"].ToString();
                    LoadTehDetails((Int64)dRow["PK_IdOrderDetail"], (Int64)dRow["FK_IdDetail"], row, exW, ref MaxColumn);
                }
                if (NameRep == "Form6")//StartColumn=2
                {
                    ((Excel.Range)exW.Cells[row, StartColumn+3]).Value2 = dRow["AmountDetails"].ToString() + "|" + dRow["AllCountDetails"].ToString();
                    ((Excel.Range)exW.Cells[row, StartColumn + 4]).Value2 = dRow["AllPositionParent"].ToString();
                    Load_Plan_TehDetails((Int64)dRow["PK_IdOrderDetail"], (Int32)dRow["AmountDetails"], row, exW);
                    //*****************************************************
                    /*Select od.PK_IdOrderDetail,od.FK_IdDetail,ShcmDetail,NameDetail,od.AmountDetails,od.Position,od.PositionParent,od.AllPositionParent,cod.AllCountDetails" + "\n" +
                                          "From OrdersDetails od" + "\n" +
                                          "Inner Join Sp_Details ON Sp_Details.PK_IdDetail=od.FK_IdDetail" + "\n" +
                                          "Inner Join vwCountOrdersDetails cod ON cod.FK_IdOrder = od.FK_IdOrder and cod.FK_IdDetail = od.FK_IdDetail" + "\n" +
                                          "Where od.FK_IdOrder = @PK_IdOrder" + "\n" +
                                          //"Order by Position";
                                          "Order by ShcmDetail";*/
                    





                    //***************************************************
                }
                //*******************************
                row++;
                //Recursion Call
                if (int.TryParse(dRow["Position"].ToString(), out ParentId))
                    loadTreeSubMenu(ref row, StartColumn, ParentId, exW, ref MaxColumn, NameRep);
            }
            if (
                ((RowStart > 6 & NameRep == "rep6") || (RowStart > 11 & NameRep == "Form6"))
                & childs.Length > 0)
                ((Excel.Range)exW.get_Range((RowStart) + ":" + (row - 1))).Group();
        }
        //***********************************************************************************************
        #region "План-график (форма №6)"

        public void PlanSheduleForm6(int PK_IdOrder, string OrderNum, string OrderName)
        {
            try
            {
                AllDetailsInOrder(PK_IdOrder, "Form6");//Все детали заказа загружаются в _DT
                if (_DT.Rows.Count == 0) MessageBox.Show("Нет данных для формирования отчёта.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else//Export data to Excel
                {
                    Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                    //XlReferenceStyle RefStyle = Excel.ReferenceStyle; Excel.Visible = true;
                    ExcelApp.SheetsInNewWorkbook = 3; //Кол-во книг
                    ExcelApp.Workbooks.Add();
                    Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);
                    //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove; //Сворачиваем сверху
                    #region Настройка шапки
                    ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);
                    //ExcelWorkSheet.Application.StandardFontSize = 10;
                    //ExcelWorkSheet.Application.Cells.HorizontalAlignment = Excel.Constants.xlCenter;
                    //Редактирование созданного документа
                    ((Excel.Range)ExcelWorkSheet.Columns[1]).ColumnWidth = 2;
                    ((Excel.Range)ExcelWorkSheet.Columns[2]).ColumnWidth = 5;
                    ((Excel.Range)ExcelWorkSheet.Columns[3]).ColumnWidth = 20;
                    ((Excel.Range)ExcelWorkSheet.Columns[4]).ColumnWidth = 30;
                    ((Excel.Range)ExcelWorkSheet.Columns[5]).ColumnWidth = 7;
                    for (int i = 6; i < 16; i++)
                    {
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).ColumnWidth = 10;
                        //BorderColumn++;
                    }
                    ((Excel.Range)ExcelWorkSheet.Columns[16]).ColumnWidth = 8;
                    ((Excel.Range)ExcelWorkSheet.Columns[17]).ColumnWidth = 8;
                    ((Excel.Range)ExcelWorkSheet.Columns[18]).ColumnWidth = 12;
                    //Формат времени
                    /*((Excel.Range)ExcelWorkSheet.Columns[8]).NumberFormat = "hh:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[9]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[10]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[11]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[12]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[13]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[14]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[15]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[18]).NumberFormat = "h:mm:ss";*/
                    //Формат шапки
                    //((Excel.Range)ExcelWorkSheet.get_Range("A1:R10")).NumberFormat = "";
                        //**********************
                    ((Excel.Range)ExcelWorkSheet.Cells[1, 2]).Value2 = "Форма №6";
                    ((Excel.Range)ExcelWorkSheet.Cells[1, 2]).Font.Bold = 1;
                    ((Excel.Range)ExcelWorkSheet.Cells[1, 13]).Value2 = "Утверждаю";
                    ((Excel.Range)ExcelWorkSheet.Cells[2, 13]).Value2 = "Начальник производства 50";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 13]).Value2 = "_____________________М.М.ШАГАЛОВ";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 13]).Value2 = "<<_______>>________________20____г.";
                    ((Excel.Range)ExcelWorkSheet.get_Range("B1:D1")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("M1:P1")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("M2:P2")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("M4:P4")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("M5:P5")).Merge();
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 2]).Value2 = "План-график";
                    ((Excel.Range)ExcelWorkSheet.Cells[8, 2]).Value2 = "Заказ " + OrderNum;
                    ((Excel.Range)ExcelWorkSheet.Cells[9, 2]).Value2 = OrderName;
                    ((Excel.Range)ExcelWorkSheet.Cells[9, 2]).Font.Bold = 1;

                    ((Excel.Range)ExcelWorkSheet.get_Range("B7:P7")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("B8:P8")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("B9:P9")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A7", "P8")).Font.Size = 14;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A7", "P8")).HorizontalAlignment = Excel.Constants.xlCenter;
                    
                    //Рисуем шапку                  
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 2]).Value2 = "Поз.";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 3]).Value2 = "Обозначение";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 4]).Value2 = "Наименование";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 5]).Value2 = "Кол-во";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 6]).Value2 = "Куда входит";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 7]).Value2 = "План изготов.";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 8]).Value2 = "Загот";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 9]).Value2 = "Токар";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 10]).Value2 = "Фрезер";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 11]).Value2 = "Шлиф";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 12]).Value2 = "Фрезер.ЧПУ";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 13]).Value2 = "Расточ";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 14]).Value2 = "Свароч";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 15]).Value2 = "Слесарн и малярн";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 16]).Value2 = "Заточная (технол.)";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 17]).Value2 = "Склад";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 18]).Value2 = "ОТК";
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 19]).Value2 = "Итого:";
                    ExcelWorkSheet.PageSetup.PrintTitleRows = "$10:$10";
                    ((Excel.Range)ExcelWorkSheet.Rows[11]).Select();
                    ExcelApp.ActiveWindow.FreezePanes = true;
                    ((Excel.Range)ExcelWorkSheet.Cells[10, 1]).Select();
                    ((Excel.Range)ExcelWorkSheet.get_Range("B10", "S10" + 10)).AutoFilter(1);
                    ((Excel.Range)ExcelWorkSheet.get_Range("B10", "S10" + 10)).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B10", "S10" + 10)).VerticalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B10", "S10" + 10)).WrapText = true;
                    //ExcelWorkSheet.PageSetup.PrintTitleRows = "$5:$5";
                    //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove;//группировка строк
                    //************************************
                    #endregion Настройка шапки
                    //Заполняем таблицу
                    int NumRow = 11;
                    int MaxColumn = 0;




                    loadTreeSubMenu(ref NumRow, 2, 0, ExcelWorkSheet, ref MaxColumn, "Form6");
                    ((Excel.Range)ExcelWorkSheet.get_Range("B11", "S" + NumRow)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + NumRow + ":F" + NumRow)).Merge();
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 2 ]).Value2 = "Всего:";
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + NumRow + ":S" + NumRow)).Font.Bold = 1;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + NumRow + ":S" + NumRow)).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 2]).HorizontalAlignment = Excel.Constants.xlLeft;



                    if (_OperGroupFactTime[0] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 8]).Value2 = IntToTime(_OperGroupFactTime[0]); // Заготов
                    if (_OperGroupFactTime[1] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime(_OperGroupFactTime[1]); // Токар
                    if (_OperGroupFactTime[2] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 10]).Value2 = IntToTime(_OperGroupFactTime[2]); // Фрезер
                    if (_OperGroupFactTime[3] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 11]).Value2 = IntToTime(_OperGroupFactTime[3]); //  Шлиф
                    if (_OperGroupFactTime[4] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 12]).Value2 = IntToTime(_OperGroupFactTime[4]); // Фрезер ЧПУ
                    if (_OperGroupFactTime[5] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 13]).Value2 = IntToTime(_OperGroupFactTime[5]); // Расточ
                    if (_OperGroupFactTime[6] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 14]).Value2 = IntToTime(_OperGroupFactTime[6]); // Свароч
                    if (_OperGroupFactTime[7] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 15]).Value2 = IntToTime(_OperGroupFactTime[7]); // Слесарн
                    if (_OperGroupFactTime[8] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 18]).Value2 = IntToTime(_OperGroupFactTime[8]); // Контроль ОТК
                    if(_OperGroupFactTime[9] > 0)((Excel.Range)ExcelWorkSheet.Cells[NumRow, 16]).Value2 = IntToTime(_OperGroupFactTime[9]); // Заточная (технол.)
                    if (_OperGroupFactTime[11] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 19]).Value2 = IntToTime(_OperGroupFactTime[11]); // Итого


                    NumRow += 1;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B11", "R" + NumRow)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + NumRow + ":F" + NumRow)).Merge();
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 2]).Value2 = "Всего выполнено (факт):";
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + NumRow + ":S" + NumRow)).Font.Bold = 1;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + NumRow + ":S" + NumRow)).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 2]).HorizontalAlignment = Excel.Constants.xlLeft;

                    if (_OperGroupFactTime[0] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 8]).Value2 = IntToTime(_FactTime[0]);
                    if (_OperGroupFactTime[1] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime(_FactTime[1]);
                    if (_OperGroupFactTime[2] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 10]).Value2 = IntToTime(_FactTime[2]);
                    if (_OperGroupFactTime[3] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 11]).Value2 = IntToTime(_FactTime[3]);
                    if (_OperGroupFactTime[4] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 12]).Value2 = IntToTime(_FactTime[4]);
                    if (_OperGroupFactTime[5] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 13]).Value2 = IntToTime(_FactTime[5]);
                    if (_OperGroupFactTime[6] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 14]).Value2 = IntToTime(_FactTime[6]);
                    if (_OperGroupFactTime[7] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 15]).Value2 = IntToTime(_FactTime[7]);
                    if (_OperGroupFactTime[8] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 18]).Value2 = IntToTime(_FactTime[8]);
                    if (_OperGroupFactTime[9] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 16]).Value2 = IntToTime(_FactTime[9]); // Заточная (технол.)
                    if (_OperGroupFactTime[11] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 19]).Value2 = IntToTime(_FactTime[11]);

                    NumRow += 1;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B11", "S" + NumRow)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + NumRow + ":F" + NumRow)).Merge();
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 2]).Value2 = "Остаток к изготовлению:";
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + NumRow + ":S" + NumRow)).Font.Bold = 1;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + NumRow + ":S" + NumRow)).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 2]).HorizontalAlignment = Excel.Constants.xlLeft;

                    if (_OperGroupFactTime[0] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 8]).Value2 = IntToTime(_OperGroupFactTime[0] - _FactTime[0]);
                    if (_OperGroupFactTime[1] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime(_OperGroupFactTime[1] - _FactTime[1]);
                    if (_OperGroupFactTime[2] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 10]).Value2 = IntToTime(_OperGroupFactTime[2] - _FactTime[2]);
                    if (_OperGroupFactTime[3] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 11]).Value2 = IntToTime(_OperGroupFactTime[3] - _FactTime[3]);
                    if (_OperGroupFactTime[4] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 12]).Value2 = IntToTime(_OperGroupFactTime[4] - _FactTime[4]);
                    if (_OperGroupFactTime[5] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 13]).Value2 = IntToTime(_OperGroupFactTime[5] - _FactTime[5]);
                    if (_OperGroupFactTime[6] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 14]).Value2 = IntToTime(_OperGroupFactTime[6] - _FactTime[6]);
                    if (_OperGroupFactTime[7] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 15]).Value2 = IntToTime(_OperGroupFactTime[7] - _FactTime[7]);
                    if(_OperGroupFactTime[8] > 0)((Excel.Range)ExcelWorkSheet.Cells[NumRow, 18]).Value2 = IntToTime(_OperGroupFactTime[8] - _FactTime[8]);
                    if (_OperGroupFactTime[9] > 0)((Excel.Range)ExcelWorkSheet.Cells[NumRow, 16]).Value2 = IntToTime(_OperGroupFactTime[9] - _FactTime[9]); // Заточная (технол.)
                    if (_OperGroupFactTime[11] > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 19]).Value2 = IntToTime(_OperGroupFactTime[11] - _FactTime[11]);


                    /*((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 10]).Formula = "=SUM(J11:J" + (NumRow - 1) + ")";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).FormulaHidden = true;
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Calculate();
                    string LetterMaxColumn = ((Excel.Range)ExcelWorkSheet.Columns[MaxColumn - 1]).Address;
                    LetterMaxColumn = LetterMaxColumn.Remove(0, 1).Remove(LetterMaxColumn.IndexOf(":") - 1);
                    ((Excel.Range)ExcelWorkSheet.get_Range("I1", LetterMaxColumn + 1)).ColumnWidth = 25;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1", LetterMaxColumn + (_DT.Rows.Count - 2))).WrapText = true;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A5", LetterMaxColumn + (_DT.Rows.Count - 2))).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

                    ((Excel.Range)ExcelWorkSheet.get_Range((NumRow - 1) + ":" + (NumRow - 1))).Group();*/

                    //************************************


                    MessageBox.Show("Формирование отчета завершено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                for (int i = 0; i < 12; i++)
                {
                    _OperGroupFactTime[i] = 0;
                    _FactTime[i] = 0;
                }
            }
        }

        private void Load_Plan_TehDetails(Int64 FK_IdOrderDetail, int AmountDetails, int row, Excel.Worksheet exW)
        {
            try//string[] arrOper = { "Заготов","Токарн","Заточ","Координатн","лифоваль","Свар","ЧПУ","Фрезер","Слесарн","Комплект","Испытания","Малярная","струйная","Разметка","Промывка","Прессование","свар"};
            {
                
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    string sqlExpression = "LoadPlanTehDetails";
                    SqlCommand cmd = new SqlCommand(sqlExpression) { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.Connection = con;
                    cmd.Parameters.Clear();
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@FK_IdOrderDetail",
                        Value = FK_IdOrderDetail
                    };
                    cmd.Parameters.Add(nameParam);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)//Color.LightGreen;
                        {
                            Int16 IdOperGroup = 0;
                            int Tpd = 0, Tsh = 0, Amount = 0;//по факту
                            int OperTime = 0;//Общее время 1 операции
                            bool OnlyOncePay = true;//Платим только 1 раз, без учета деталей Tpd + Tsh
                            string TypeRow = "";
                            //int
                            int AllSptTime = 0;//Всего времени по справочнику технологий изготовления
                            int AllFactTime = 0;
                            int g_f_10 = 0;

                            int[] totalTime = new int[10]; //подсчёт общего времени по группам по данным справочника
                            int[] totalFactTime = new int[10]; //подсчёт общего времени по группам по факту
                            int[] numberRow = { 8, 9, 10, 11, 12, 13, 14, 15, 18, 16 };

                            //int 
                            while (reader.Read())//IntToTime  NormTimeFabrication(bool OnlyOncePay, int Tpd, int Tsh, int Amount)
                            {
                                if (!reader.IsDBNull(0)) IdOperGroup = reader.GetInt16(0); else IdOperGroup = 0;

                                if (IdOperGroup > 0)
                                {
                                    //Присваиваем
                                    if (!reader.IsDBNull(1)) Tpd = reader.GetInt32(1); else Tpd = 0;
                                    if (!reader.IsDBNull(2)) Tsh = reader.GetInt32(2); else Tsh = 0;
                                    if (!reader.IsDBNull(3)) Amount = reader.GetInt32(3); else Amount = 0;
                                    if (!reader.IsDBNull(4)) OnlyOncePay = reader.GetBoolean(4); else OnlyOncePay = true;
                                    if (!reader.IsDBNull(7)) TypeRow = reader.GetString(7); else TypeRow = "";
                                    OperTime = NormTimeFabrication(OnlyOncePay, Tpd, Tsh, Amount);

                                    switch (IdOperGroup)
                                    {
                                        case 1://Загот
                                            if (TypeRow == "3fact") totalFactTime[0] += OperTime; else totalTime[0] += OperTime;
                                            break;
                                        case 2://Токар
                                            if (TypeRow == "3fact") totalFactTime[1] += OperTime; else totalTime[1] += OperTime;
                                            break;
                                        case 3://Фрезер
                                            if (TypeRow == "3fact") totalFactTime[2] += OperTime; else totalTime[2] += OperTime;
                                            break;
                                        case 4://Шлиф
                                            if (TypeRow == "3fact") totalFactTime[3] += OperTime; else totalTime[3] += OperTime;
                                            break;
                                        case 5://ЧПУ
                                            if (TypeRow == "3fact") totalFactTime[4] += OperTime; else totalTime[4] += OperTime;
                                            break;
                                        case 6://Расточ
                                            if (TypeRow == "3fact") totalFactTime[5] += OperTime; else totalTime[5] += OperTime;
                                            break;
                                        case 7://Свароч
                                            if (TypeRow == "3fact") totalFactTime[6] += OperTime; else totalTime[6] += OperTime;
                                            break;
                                        case 8://Слесарн и малярная
                                            if (TypeRow == "3fact") totalFactTime[7] += OperTime; else totalTime[7] += OperTime;
                                            break;
                                        case 9://ОТК
                                            if (TypeRow == "3fact") totalFactTime[8] += OperTime; else totalTime[8] += OperTime;//т.к. Контроль ОТК может повторятся несколько раз
                                            break;
                                        case 10://Склад
                                            if (TypeRow == "3fact") g_f_10 += Amount; //т.к. Склад может повторятся несколько раз
                                            break;
                                        case 12://Заточная (технол.)
                                            if (TypeRow == "3fact") totalFactTime[9] += OperTime; else totalTime[9] += OperTime;
                                            break;
                                        default:
                                            //Console.WriteLine("Default case");
                                            break;
                                    }
                                }
                            }


                            for (int i = 0; i < 10; i++)
                            {
                                if (totalFactTime[i] >= totalTime[i]) _OperGroupFactTime[i] += totalFactTime[i];
                                else _OperGroupFactTime[i] += totalTime[i];

                                if (totalTime[i] > 0)
                                {
                                    ((Excel.Range)exW.Cells[row, numberRow[i]]).Value2 = IntToTime(totalTime[i]);
                                    AllSptTime += totalTime[i];
                                }

                                if (totalFactTime[i] > 0)
                                {
                                    ((Excel.Range)exW.Cells[row, numberRow[i]]).Value2 = IntToTime(totalFactTime[i]);
                                    AllFactTime += totalFactTime[i];
                                    AllSptTime -= totalTime[i];

                                    _FactTime[i] += totalFactTime[i]; // Добавляем фактическую заготовку
                                }

                                if (totalFactTime[i] > 0)
                                {
                                    if (totalFactTime[i] >= totalTime[i])
                                        ((Excel.Range)exW.Cells[row, numberRow[i]]).Interior.Color = Color.LightGreen;
                                    else
                                        ((Excel.Range)exW.Cells[row, numberRow[i]]).Interior.Color = Color.LightPink;
                                }
                            }
                  
                            if (AllSptTime > 0) ((Excel.Range)exW.Cells[row, 19]).Value2 = IntToTime(AllSptTime);
                            if (g_f_10 > 0) ((Excel.Range)exW.Cells[row, 17]).Value2 = g_f_10;
                            if (AllFactTime > 0)
                            {
                                ((Excel.Range)exW.Cells[row, 19]).Value2 = IntToTime(AllFactTime + AllSptTime);
                                _FactTime[11] += AllFactTime;
                            }
                            _OperGroupFactTime[11] += AllFactTime;
                            _OperGroupFactTime[11] += AllSptTime;

                            //Полностью закрашиваем строку
                            if (g_f_10 > 0)
                            {
                                ((Excel.Range)exW.Cells[row, 17]).Value2 = g_f_10 + "|" + AmountDetails;

                                if (g_f_10 == AmountDetails) ((Excel.Range)exW.get_Range("A" + row, "R" + row)).Interior.Color = Color.LightGreen;//Полностью закрашиваем строку
                                else ((Excel.Range)exW.Cells[row, 17]).Interior.Color = Color.LightPink;
                            }

                            if (reader.HasRows == false)   // Если в БД Диспетчера мы не находим ничего, то идем в Лотсман
                            {
                                con.Close();
                                con.ConnectionString = config.LoodsmanConnectionString;
                                SqlCommand cmd_2 = new SqlCommand() { CommandTimeout = 60 };
                                cmd_2.Connection = con;
                                cmd_2.Parameters.Clear();
                                cmd_2.CommandText = "Select att.value AS Oper, Tpd.value AS Tpd,Tsh.asfloat AS Tsh" + "\n" +
                                      "FROM [НИИПМ].[dbo].[rvwRelations] r" + "\n" +
                                      "INNER JOIN rvwRelations AS r2 ON r2.idparent = r.idparent AND r2.idlinktype = 32" + "\n" +
                                      "INNER JOIN rvwRelationAttributes ra ON ra.idrelation  = r2.id and ra.attrtype = 0" + "\n" +
                                      "INNER JOIN rvwAttributes AS att ON att.idversion = r2.idChild AND att.idattr = 235" + "\n" +
                                      "INNER JOIN rvwAttributes AS Tpd ON Tpd.idversion = r2.idChild AND Tpd.idattr = 321" + "\n" +
                                      "INNER JOIN rvwAttributes AS Tsh ON Tsh.idversion = r2.idChild AND Tsh.idattr = 195" + "\n" +
                                      "where  r.idchild = (SELECT IdLoodsman FROM [Dispetcher2].[dbo].[FullView] WHERE PK_IdOrderDetail = @FK_IdOrderDetail) and r.idlinktype=33" + "\n" +
                                      "ORDER BY oper";
                                cmd_2.Parameters.Add(new SqlParameter("@FK_IdOrderDetail", SqlDbType.BigInt));
                                cmd_2.Parameters["@FK_IdOrderDetail"].Value = FK_IdOrderDetail;
                                using (con)
                                {
                                    con.Open();
                                    using (SqlDataReader reader2 = cmd_2.ExecuteReader())
                                    {
                                        int slesarn = 0, zagotov = 0, tokar = 0, frezer = 0, shlif = 0, frezer_chu = 0, rastoch = 0, svaroch = 0, otk = 0;

                                        while (reader2.Read())
                                        {
                                            switch (reader2.GetValue(0))
                                            {
                                                case "Заготовительная"://Загот
                                                    zagotov += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    _OperGroupFactTime[0] += zagotov;
                                                    break;
                                                case "Токарная (технол.)"://Токар
                                                    tokar += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    _OperGroupFactTime[1] += tokar;
                                                    break;
                                                case "Токарная"://Токар
                                                    tokar += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    _OperGroupFactTime[1] += tokar;
                                                    break;
                                                case "Фрезерная"://Фрезер
                                                    frezer += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    _OperGroupFactTime[2] += frezer;
                                                    break;
                                                case "Шлифовальная":
                                                    shlif += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    _OperGroupFactTime[3] += shlif;
                                                    break;
                                                case "Фрезерная с ЧПУ"://ЧПУ
                                                    frezer_chu += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    _OperGroupFactTime[4] += frezer_chu;
                                                    break; ;
                                                case "Координатно-расточная"://Расточ
                                                    rastoch += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    _OperGroupFactTime[5] += rastoch;
                                                    break;
                                                case "Сварочная"://Свароч
                                                    svaroch += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    _OperGroupFactTime[6] += svaroch;
                                                    break;
                                                case "Слесарная"://Слесарн
                                                    slesarn += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    _OperGroupFactTime[7] += slesarn;
                                                    break;
                                                case "Слесарно-сборочная"://Слесарн
                                                    slesarn += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    break;
                                                case "Контроль ОТК"://ОТК
                                                    otk += Convert.ToInt32(reader2.GetValue(1)) + Convert.ToInt32(reader2.GetValue(2));
                                                    break;
                                                default:
                                                    //Console.WriteLine("Default case");
                                                    break;
                                            }
                                        }

                                        // Заполняем пустые столбцы если содержимое больше 0
                                        // _OperGroupFactTime[8] ячейка "итого" в массиве
                                        int AllTime = 0;
                                        AllTime = (slesarn + frezer_chu + frezer + tokar + svaroch + zagotov) * AmountDetails;
                                        if (slesarn > 0) ((Excel.Range)exW.Cells[row, 15]).Value2 = IntToTime(slesarn * AmountDetails);
                                        if (frezer_chu > 0) ((Excel.Range)exW.Cells[row, 12]).Value2 = IntToTime(frezer_chu * AmountDetails);
                                        if (frezer > 0) ((Excel.Range)exW.Cells[row, 10]).Value2 = IntToTime(frezer * AmountDetails);
                                        if (tokar > 0) ((Excel.Range)exW.Cells[row, 9]).Value2 = IntToTime(tokar * AmountDetails);
                                        if (svaroch > 0) ((Excel.Range)exW.Cells[row, 14]).Value2 = IntToTime(svaroch * AmountDetails);
                                        if (zagotov > 0) ((Excel.Range)exW.Cells[row, 8]).Value2 = IntToTime(zagotov * AmountDetails);
                                        if (AllTime > 0) ((Excel.Range)exW.Cells[row, 18]).Value2 = IntToTime(AllTime);

                                        _OperGroupFactTime[8] += AllTime;
                                    }
                                }
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




        #endregion
        //***********************************************************************************************
        #region C_Gper.NameReport == 117  Операции выполненные рабочим по заказам (форма №17)

        private void SelectForm17(DateTime DateStart, DateTime DateEnd, string loginWorker)
        {
            try
            {
                _DT.Clear();
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//using System.Data.SqlClient;
                    cmd.Parameters.Clear();
                    cmd.CommandText = "SELECT DateFactOper,OrderNum,Position,ShcmDetail,NameDetail,NameOperation,OnlyOncePay," + "\n" +
                        "Tpd,Tsh,AmountDetails,FK_LoginWorker,FK_IdBrigade,AmountWorkers,FullName,RateWorker" + "\n" +
                        "From vwForm17Workers" + "\n" +
                        "Where DateFactOper>=@DateStart and DateFactOper<=@DateEnd and FK_LoginWorker=@FK_LoginWorker" + "\n" +
                        "union all" + "\n" +
                        "SELECT DateFactOper,OrderNum,Position,ShcmDetail,NameDetail,NameOperation,OnlyOncePay," + "\n" +
                        "Tpd,Tsh,AmountDetails,FK_LoginWorker,FK_IdBrigade,AmountWorkers,FullName,RateWorker" + "\n" +
                        "From vwForm17Brigades" + "\n" +
                        "Where DateFactOper>=@DateStart and DateFactOper<=@DateEnd and FK_LoginWorker=@FK_LoginWorker" + "\n" +
                        "Order by OrderNum,Position";
                    cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date));
                    cmd.Parameters["@DateStart"].Value = DateStart;
                    cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date));
                    cmd.Parameters["@DateEnd"].Value = DateEnd;
                    cmd.Parameters.Add(new SqlParameter("@FK_LoginWorker", SqlDbType.VarChar));
                    cmd.Parameters["@FK_LoginWorker"].Value = loginWorker;
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT);
                    adapter.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Form17(DateTime DateStart, DateTime DateEnd, string loginWorker, string Department, int PlanHours)
        {
            try
            {
                SelectForm17(DateStart, DateEnd, loginWorker);
                if (_DT.Rows.Count == 0) MessageBox.Show("Нет данных для формирования отчёта.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else//Export data to Excel
                {
                    Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                    //XlReferenceStyle RefStyle = Excel.ReferenceStyle; Excel.Visible = true;
                    ExcelApp.Workbooks.Add(1);
                    Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);
                    //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove; //Сворачиваем сверху
                    #region Настройка шапки
                    ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.PageSetup.PrintTitleRows = "$5:$5";
                    //Редактирование созданного документа
                    ((Excel.Range)ExcelWorkSheet.Columns[1]).ColumnWidth = 4;
                    ((Excel.Range)ExcelWorkSheet.Columns[2]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[3]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[4]).ColumnWidth = 5;
                    ((Excel.Range)ExcelWorkSheet.Columns[5]).ColumnWidth = 20;
                    ((Excel.Range)ExcelWorkSheet.Columns[6]).ColumnWidth = 30;
                    ((Excel.Range)ExcelWorkSheet.Columns[7]).ColumnWidth = 5;
                    ((Excel.Range)ExcelWorkSheet.Columns[8]).ColumnWidth = 30;
                    ((Excel.Range)ExcelWorkSheet.Columns[9]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[10]).ColumnWidth = 15;
                    ((Excel.Range)ExcelWorkSheet.Cells[1, 9]).Value2 = "Форма №17";
                    ((Excel.Range)ExcelWorkSheet.get_Range("I1:J1")).Merge();
                    //**********************************************************************
                    int chSu = 0;
                    int chSc = 0;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 = "Изготовлено рабочим:   ";
                    chSu = ((string)((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2).Length;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 = ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 + loginWorker + "   " + Department +
                                                                        "   за период с " + DateStart.ToShortDateString() + " по " + DateEnd.ToShortDateString();
                    //"   за период с " + dS.ToShortDateString() + " по " + dE.ToShortDateString();
                    chSc = chSu + loginWorker.Length + 3;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Characters[chSu, loginWorker.Length + 1].Font.Underline = true;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Characters[chSc, Department.Length + 1].Font.Underline = true;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A3:J3")).Merge();
                    //**********************************************************************
                    double RateWorker = Convert.ToDouble(_DT.Rows[0].ItemArray[14]);//Ставка работника 1 или 0.5
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 8]).Value2 = "Плановая трудоёмкость:   ";
                    PlanHours = Convert.ToInt32(PlanHours * RateWorker * 1.08);//Добавляем коэффициент 1.08
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 9]).Value2 = IntToTime(PlanHours);
                    ((Excel.Range)ExcelWorkSheet.get_Range("H5", "I5")).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    //**********************************************************************
                    //Рисуем шапку таблицы      
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 1]).Value2 = "№ п/п";
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 2]).Value2 = "Дата";
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 3]).Value2 = "Заказ";
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 4]).Value2 = "Поз.";
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 5]).Value2 = "ЩЦМ";
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 6]).Value2 = "Наименование";
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 7]).Value2 = "Кол-во";
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 8]).Value2 = "Операция";
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 9]).Value2 = "Трудоемкость";
                    ((Excel.Range)ExcelWorkSheet.Cells[7, 10]).Value2 = "Примечание";
                    ((Excel.Range)ExcelWorkSheet.get_Range("A7:J7")).Font.Bold = 1;
                    //Пишем сами значения
                    int NumRow = 8; //Номер строки
                    int StartNumRow = NumRow;//Строка с которой начинаем объединение
                    bool OnlyOncePay = true;//Платим только 1 раз, без учета деталей Tpd + Tsh
                    int Tpd = 0, Tsh = 0;
                    int AmountDetailsFact = 0;
                    Int16 AmountWorkers = 1;
                    int FactTimeWorker = 0;//Для рабочего
                    ulong FactTimeDepartment = 0;//Всего по Участку
                    ulong AllFactTime = 0;//Вообще всего
                    string OrderNum = "";
                    for (int i = 0; i < _DT.Rows.Count; i++)
                    {
                        //ПОДВОДИМ ИТОГИ
                        if (OrderNum == "") OrderNum = _DT.Rows[i].ItemArray[1].ToString();
                        else
                            if (OrderNum != _DT.Rows[i].ItemArray[1].ToString() || i == _DT.Rows.Count - 1)//Если новый заказ или если последняя строка DataSet
                            {
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = "Итого по заказу " + OrderNum;
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime((ulong)FactTimeDepartment);
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":H" + NumRow)).Merge();
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":J" + NumRow)).Font.Bold = 1;
                                ((Excel.Range)ExcelWorkSheet.get_Range(StartNumRow + ":" + (NumRow - 1))).Group(); //Группируем строки
                                AllFactTime += FactTimeDepartment;
                                FactTimeDepartment = 0;
                                OrderNum = _DT.Rows[i].ItemArray[1].ToString();
                                NumRow++;
                                StartNumRow = NumRow;
                            }
                        //END ПОДВОДИМ ИТОГИ
                        //            0               1           2               3           4                                                       5               6
                        //SELECT fo.DateFactOper,o.OrderNum,od.Position,Sp_D.ShcmDetail,Sp_D.NameDetail,(fo.NumOper + ' ' + Sp_O.NameOperation) as NameOperation,Sp_O.OnlyOncePay,
                        //      7    8          9                   10              11              12                13
                        //fo.Tpd,fo.Tsh,fo.AmountDetails,fo.FK_LoginWorker,fo.FK_IdBrigade,1 as AmountWorkers,'' as FullName
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = NumRow - 5;
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 2]).Value2 = Convert.ToDateTime(_DT.Rows[i].ItemArray[0]).ToShortDateString();//((DateTime)_DT.Rows[i].ItemArray[0]).ToShortDateString(); //Convert.ToDateTime(_DT.Rows[i].ItemArray[0]);
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 3]).Value2 = _DT.Rows[i].ItemArray[1].ToString();
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 4]).Value2 = _DT.Rows[i].ItemArray[2].ToString();
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 5]).Value2 = _DT.Rows[i].ItemArray[3].ToString();
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 6]).Value2 = _DT.Rows[i].ItemArray[4].ToString();
                        AmountDetailsFact = Convert.ToInt32(_DT.Rows[i].ItemArray[9]);
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 7]).Value2 = AmountDetailsFact;
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 8]).Value2 = _DT.Rows[i].ItemArray[5].ToString();

                        if (Convert.ToInt32(_DT.Rows[i].ItemArray[6]) == 1) OnlyOncePay = true;
                        else OnlyOncePay = false;
                        Tpd = Convert.ToInt32(_DT.Rows[i].ItemArray[7]);
                        Tsh = Convert.ToInt32(_DT.Rows[i].ItemArray[8]);
                        AmountWorkers = Convert.ToInt16(_DT.Rows[i].ItemArray[12]);
                        FactTimeWorker = NormTimeFabrication(OnlyOncePay, Tpd, Tsh, AmountDetailsFact) / AmountWorkers;
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime(FactTimeWorker);
                        FactTimeDepartment += (ulong)FactTimeWorker;
                        if (AmountWorkers > 1) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 10]).Value2 = "(" + AmountWorkers.ToString() + ") " + _DT.Rows[i].ItemArray[13].ToString();

                        NumRow++;
                        //ПОДВОДИМ ИТОГИ, ТОЛЬКО ДЛЯ ПОСЛЕДНЕГО ЗАКАЗА 
                        if (OrderNum == "") OrderNum = _DT.Rows[i].ItemArray[1].ToString();
                        else
                            if (i == _DT.Rows.Count - 1)//Если новый заказ или если последняя строка DataSet
                            {
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = "Итого по заказу " + OrderNum;
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime((ulong)FactTimeDepartment);
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":H" + NumRow)).Merge();
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":J" + NumRow)).Font.Bold = 1;
                                ((Excel.Range)ExcelWorkSheet.get_Range(StartNumRow + ":" + (NumRow - 1))).Group(); //Группируем строки
                                AllFactTime += FactTimeDepartment;
                                FactTimeDepartment = 0;
                                //OrderNum = _DT.Rows[i].ItemArray[1].ToString();
                                NumRow++;
                                StartNumRow = NumRow;
                            }
                        //END ПОДВОДИМ ИТОГИ, ТОЛЬКО ДЛЯ ПОСЛЕДНЕГО ЗАКАЗА 
                    }
                    //Общий итог
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = "Общий итог";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime((ulong)AllFactTime);
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":H" + NumRow)).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":J" + NumRow)).Font.Bold = 1;
                    //****
                    double Proc = (double)AllFactTime / (double)PlanHours;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow + 1) + ":H" + (NumRow + 1))).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow + 1) + ":J" + (NumRow + 1))).Font.Italic = 1;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow + 1) + ":J" + (NumRow + 1))).Font.Size = 11;
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, 1]).Value2 = "Выполнение планового задания, в%";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, 9]).NumberFormat = "0.00%";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, 9]).Value2 = Proc;
                    AllFactTime = 0;
                    //Выравниваем столбцы
                    for (int i = 1; i <= 10; i++)
                    {
                        if (i == 5 || i == 6 || i == 9)
                            ((Excel.Range)ExcelWorkSheet.Columns[i]).HorizontalAlignment = Excel.Constants.xlCenter;
                        else ((Excel.Range)ExcelWorkSheet.Columns[i]).HorizontalAlignment = Excel.Constants.xlCenter;
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).VerticalAlignment = Excel.Constants.xlCenter;
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).WrapText = true;
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).Font.Size = 11;
                    }
                    //Рисуем сетку
                    ((Excel.Range)ExcelWorkSheet.get_Range("A7", "J" + (NumRow + 1))).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    //Начальник
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 3, 1]).HorizontalAlignment = Excel.Constants.xlLeft;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow + 3) + ":F" + (NumRow + 3))).Merge();
                    if (Department == "Механический участок цеха по ИСТО")
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 3, 1]).Value2 = "   Начальник мех. участка   _______________________ /Уткин В.В./";
                    if (Department == "Слесарно-сборочный участок цеха по ИСТО")
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 3, 1]).Value2 = "   Начальник слес. участка _______________________ /Сотников Ю.И./";


                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*public void Form17(DateTime DateStart, DateTime DateEnd, string loginWorker, string Department, int PlanHours)
        {
            try
            {
                SelectForm17(DateStart, DateEnd, loginWorker);
                if (_DT.Rows.Count == 0) MessageBox.Show("Нет данных для формирования отчёта.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else//Export data to Excel
                {
                    Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                    //XlReferenceStyle RefStyle = Excel.ReferenceStyle; Excel.Visible = true;
                    ExcelApp.Workbooks.Add(1);
                    Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);
                    //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove; //Сворачиваем сверху
                    #region Настройка шапки
                    ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.PageSetup.PrintTitleRows = "$5:$5";
                    //Редактирование созданного документа
                    ((Excel.Range)ExcelWorkSheet.Columns[1]).ColumnWidth = 4;
                    ((Excel.Range)ExcelWorkSheet.Columns[2]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[3]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[4]).ColumnWidth = 5;
                    ((Excel.Range)ExcelWorkSheet.Columns[5]).ColumnWidth = 20;
                    ((Excel.Range)ExcelWorkSheet.Columns[6]).ColumnWidth = 30;
                    ((Excel.Range)ExcelWorkSheet.Columns[7]).ColumnWidth = 5;
                    ((Excel.Range)ExcelWorkSheet.Columns[8]).ColumnWidth = 30;
                    ((Excel.Range)ExcelWorkSheet.Columns[9]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[10]).ColumnWidth = 15;
                    ((Excel.Range)ExcelWorkSheet.Cells[1, 9]).Value2 = "Форма №17";
                    ((Excel.Range)ExcelWorkSheet.get_Range("I1:J1")).Merge();
                    //**********************************************************************
                    int chSu = 0;
                    int chSc = 0;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 = "Изготовлено рабочим:   ";
                    chSu = ((string)((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2).Length;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 = ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 + loginWorker + "   " + Department +
                                                                        "   за период с " + DateStart.ToShortDateString() +" по " + DateEnd.ToShortDateString();
                    //"   за период с " + dS.ToShortDateString() + " по " + dE.ToShortDateString();
                    chSc = chSu + loginWorker.Length + 3;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Characters[chSu, loginWorker.Length + 1].Font.Underline = true;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Characters[chSc, Department.Length + 1].Font.Underline = true;
                    ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A3:J3")).Merge();
                    //**********************************************************************
                    //Рисуем шапку таблицы      
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 1]).Value2 = "№ п/п";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 2]).Value2 = "Дата";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 3]).Value2 = "Заказ";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 4]).Value2 = "Поз.";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 5]).Value2 = "ЩЦМ";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 6]).Value2 = "Наименование";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 7]).Value2 = "Кол-во";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 8]).Value2 = "Операция";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 9]).Value2 = "Трудоемкость";
                    ((Excel.Range)ExcelWorkSheet.Cells[5, 10]).Value2 = "Примечание";
                    ((Excel.Range)ExcelWorkSheet.get_Range("A5:J5")).Font.Bold = 1;
                    //Пишем сами значения
                    int NumRow = 6; //Номер строки
                    int StartNumRow = NumRow;//Строка с которой начинаем объединение
                    bool OnlyOncePay = true;//Платим только 1 раз, без учета деталей Tpd + Tsh
                    int Tpd = 0, Tsh = 0;
                    int AmountDetailsFact = 0;
                    Int16 AmountWorkers = 1;
                    int FactTimeWorker = 0;//Для рабочего
                    ulong FactTimeDepartment = 0;//Всего по Участку
                    ulong AllFactTime = 0;//Вообще всего
                    string OrderNum = "";
                    for (int i = 0; i < _DT.Rows.Count; i++)
                    {

                        //ПОДВОДИМ ИТОГИ
                        if (OrderNum == "") OrderNum = _DT.Rows[i].ItemArray[1].ToString();
                        else
                            if (OrderNum != _DT.Rows[i].ItemArray[1].ToString() || i == _DT.Rows.Count-1)//Если новый заказ или если последняя строка DataSet
                            {
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = "Итого по заказу " + OrderNum;
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime((ulong)FactTimeDepartment);
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":H" + NumRow)).Merge();
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":J" + NumRow)).Font.Bold = 1;
                                ((Excel.Range)ExcelWorkSheet.get_Range(StartNumRow + ":" + (NumRow-1))).Group(); //Группируем строки
                                AllFactTime += FactTimeDepartment;
                                FactTimeDepartment = 0;
                                OrderNum = _DT.Rows[i].ItemArray[1].ToString();
                                NumRow++;
                                StartNumRow = NumRow;
                            }
                        //END ПОДВОДИМ ИТОГИ
                        //            0               1           2               3           4                                                       5               6
                        //SELECT fo.DateFactOper,o.OrderNum,od.Position,Sp_D.ShcmDetail,Sp_D.NameDetail,(fo.NumOper + ' ' + Sp_O.NameOperation) as NameOperation,Sp_O.OnlyOncePay,
                        //      7    8          9                   10              11              12                13
                        //fo.Tpd,fo.Tsh,fo.AmountDetails,fo.FK_LoginWorker,fo.FK_IdBrigade,1 as AmountWorkers,'' as FullName
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = NumRow - 5;
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 2]).Value2 = Convert.ToDateTime(_DT.Rows[i].ItemArray[0]).ToShortDateString();//((DateTime)_DT.Rows[i].ItemArray[0]).ToShortDateString(); //Convert.ToDateTime(_DT.Rows[i].ItemArray[0]);
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 3]).Value2 = _DT.Rows[i].ItemArray[1].ToString();
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 4]).Value2 = _DT.Rows[i].ItemArray[2].ToString();
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 5]).Value2 = _DT.Rows[i].ItemArray[3].ToString();
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 6]).Value2 = _DT.Rows[i].ItemArray[4].ToString();
                        AmountDetailsFact = Convert.ToInt32(_DT.Rows[i].ItemArray[9]);
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 7]).Value2 = AmountDetailsFact;
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 8]).Value2 = _DT.Rows[i].ItemArray[5].ToString();

                        if (Convert.ToInt32(_DT.Rows[i].ItemArray[6]) == 1) OnlyOncePay = true;
                        else OnlyOncePay = false;
                        Tpd = Convert.ToInt32(_DT.Rows[i].ItemArray[7]);
                        Tsh = Convert.ToInt32(_DT.Rows[i].ItemArray[8]);
                        AmountWorkers = Convert.ToInt16(_DT.Rows[i].ItemArray[12]);
                        FactTimeWorker = NormTimeFabrication(OnlyOncePay, Tpd, Tsh, AmountDetailsFact) / AmountWorkers;
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime(FactTimeWorker);
                        FactTimeDepartment += (ulong)FactTimeWorker;
                        if (AmountWorkers > 1) ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 10]).Value2 = "(" + AmountWorkers.ToString() + ") " + _DT.Rows[i].ItemArray[13].ToString();

                        NumRow++;
                        //ПОДВОДИМ ИТОГИ, ТОЛЬКО ДЛЯ ПОСЛЕДНЕГО ЗАКАЗА 
                        if (OrderNum == "") OrderNum = _DT.Rows[i].ItemArray[1].ToString();
                        else
                            if (i == _DT.Rows.Count - 1)//Если новый заказ или если последняя строка DataSet
                            {
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = "Итого по заказу " + OrderNum;
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime((ulong)FactTimeDepartment);
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":H" + NumRow)).Merge();
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":J" + NumRow)).Font.Bold = 1;
                                ((Excel.Range)ExcelWorkSheet.get_Range(StartNumRow + ":" + (NumRow - 1))).Group(); //Группируем строки
                                AllFactTime += FactTimeDepartment;
                                FactTimeDepartment = 0;
                                //OrderNum = _DT.Rows[i].ItemArray[1].ToString();
                                NumRow++;
                                StartNumRow = NumRow;
                            }
                        //END ПОДВОДИМ ИТОГИ, ТОЛЬКО ДЛЯ ПОСЛЕДНЕГО ЗАКАЗА 
                    }
                    //Общий итог
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = "Общий итог";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 9]).Value2 = IntToTime((ulong)AllFactTime);
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":H" + NumRow)).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":J" + NumRow)).Font.Bold = 1;
                    AllFactTime = 0;
                    //Выравниваем столбцы
                    for (int i = 1; i <= 10; i++)
                    {
                        if (i==5 || i==6 || i==9)
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).HorizontalAlignment = Excel.Constants.xlCenter;
                        else ((Excel.Range)ExcelWorkSheet.Columns[i]).HorizontalAlignment = Excel.Constants.xlCenter;
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).VerticalAlignment = Excel.Constants.xlCenter;
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).WrapText = true;
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).Font.Size = 11;
                    }
                    //Рисуем сетку
                    ((Excel.Range)ExcelWorkSheet.get_Range("A5", "J" + NumRow)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
        #endregion

        #region C_Gper.NameReport == 7 "Отчет по выполненным операциям"

        private void SelectRep7(DateTime DateStart, DateTime DateEnd, bool AllTime, bool AllOrders, string OrderNum, string OrderName)
        {
            try
            {
                _DT.Clear();
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//using System.Data.SqlClient;
                    cmd.Parameters.Clear();
                    string Where = "";
                    if (!AllTime)
                    {
                        Where = " and DateFactOper>=@DateStart and DateFactOper<=@DateEnd";
                        cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date));
                        cmd.Parameters["@DateStart"].Value = DateStart;
                        cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date));
                        cmd.Parameters["@DateEnd"].Value = DateEnd;
                    }
                    if (!AllOrders) Where += " and OrderNum = '" + OrderNum + "'";
                    cmd.CommandText = "SELECT DISTINCT fo.FK_IdOrderDetail,o.OrderNum, od.Position, od.PositionParent, Sp_Tech.NumOper," + "\n" +
                        //"SELECT DISTINCT fo.FK_IdOrderDetail,o.OrderNum, od.Position, od.FK_IdDetail, od.PositionParent, Sp_Tech.NumOper," + "\n" +
                                      "Sp_Tech.FK_IdOperation, Sp_Tech.Tpd, Sp_Tech.Tsh,Sp_D.ShcmDetail, Sp_D.NameDetail, od.AmountDetails" + "\n" +
                                      ",Sp_O.NameOperation,Sp_O.OnlyOncePay" + "\n" +
                                      "FROM dbo.FactOperation AS fo" + "\n" +
                                      "INNER JOIN dbo.OrdersDetails AS od ON od.PK_IdOrderDetail = fo.FK_IdOrderDetail" + "\n" +
                                      "INNER JOIN dbo.Orders AS o ON o.PK_IdOrder = od.FK_IdOrder" + "\n" +
                                      "LEFT OUTER JOIN dbo.Sp_TechnologyDetails AS Sp_Tech ON Sp_Tech.FK_IdDetails = od.FK_IdDetail" + "\n" +
                                      "INNER JOIN Sp_Details AS Sp_D ON Sp_D.PK_IdDetail = od.FK_IdDetail" + "\n" +
                                      "INNER JOIN Sp_Operations AS Sp_O ON Sp_O.PK_IdOperation = Sp_Tech.FK_IdOperation" + "\n" +
                                      "Where od.Position IS NOT NULL and FK_IdStatusOrders = 2" + Where + "\n" +
                        //"Where od.Position IS NOT NULL and o.OrderNum = '20544304' and fo.DateFactOper >= @DateStart and fo.DateFactOper >= @DateEnd" + "\n" +
                                      "union" + "\n" +
                                      "SELECT DISTINCT fo111.FK_IdOrderDetail, o.OrderNum, od.Position, od.PositionParent, Sp_Tech111.NumOperation as NumOper," + "\n" +
                                      "Sp_Tech111.FK_IdOperation,Sp_Tech111.Tpd,Sp_Tech111.Tsh,Sp_D.ShcmDetail, Sp_D.NameDetail, od.AmountDetails" + "\n" +
                                      ",Sp_O.NameOperation,Sp_O.OnlyOncePay" + "\n" +
                                      "FROM dbo.FactOperation AS fo111" + "\n" +
                                      "INNER JOIN OrdersDetails AS od ON od.PK_IdOrderDetail = fo111.FK_IdOrderDetail" + "\n" +
                                      "INNER JOIN dbo.Orders AS o ON o.PK_IdOrder = od.FK_IdOrder" + "\n" +
                                      "left OUTER JOIN Sp_OperationsType111 as Sp_Tech111 ON Sp_Tech111.FK_IdDetail=od.FK_IdDetail" + "\n" +
                                      "INNER JOIN Sp_Details AS Sp_D ON Sp_D.PK_IdDetail = od.FK_IdDetail" + "\n" +
                                      "INNER JOIN Sp_Operations AS Sp_O ON Sp_O.PK_IdOperation = Sp_Tech111.FK_IdOperation" + "\n" +
                                      "Where od.Position IS NULL and FK_IdStatusOrders = 2" + Where + "\n" +
                        //"Where od.Position IS NULL and o.OrderNum = '20544304' and fo111.DateFactOper >= @DateStart and fo111.DateFactOper <= @DateEnd" + "\n" +
                                      "Order by OrderNum,Position,FK_IdOrderDetail,NumOper";
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(_DT);
                    adapter.Dispose();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _err = true;
                throw;
            }
        }


        //                            0             1            2               3                   4
                        //SELECT DISTINCT fo.FK_IdOrderDetail,o.OrderNum, od.Position, od.PositionParent, Sp_Tech.NumOper, 
                        //                5                6            7          8               9                  10
                        //Sp_Tech.FK_IdOperation, Sp_Tech.Tpd, Sp_Tech.Tsh,Sp_D.ShcmDetail, Sp_D.NameDetail, cOD.AllCountDetails
                        //            11               12
                        //,Sp_O.NameOperation,Sp_O.OnlyOncePay
        private void SelectFactOper(ref DataTable DT, Int64 FK_IdOrderDetail, string NumOper,DateTime DateStart, DateTime DateEnd, bool AllTime)
        {
            try
            {
                DT.Clear();
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//using System.Data.SqlClient;
                    cmd.Parameters.Clear();
                    string Where = "";
                    if (!AllTime)
                    {
                        Where = " and DateFactOper>=@DateStart and DateFactOper<=@DateEnd";
                        cmd.Parameters.Add(new SqlParameter("@DateStart", SqlDbType.Date));
                        cmd.Parameters["@DateStart"].Value = DateStart;
                        cmd.Parameters.Add(new SqlParameter("@DateEnd", SqlDbType.Date));
                        cmd.Parameters["@DateEnd"].Value = DateEnd;
                    }
                    cmd.CommandText = "SELECT NumOper,FK_IdOperation,Tpd,Tsh,AmountDetails,DateFactOper,FK_LoginWorker,FullName,OnlyOncePay" + "\n" +
                                      "FROM FactOperation fo " + "\n" +
                                      "left JOIN Brigade AS b ON b.PK_IdBrigade = fo.FK_IdBrigade" + "\n" +
                                      "inner join Sp_Operations AS op ON op.PK_IdOperation = fo.FK_IdOperation" + "\n" +
                                      "Where FK_IdOrderDetail = @FK_IdOrderDetail and NumOper =@NumOper " + Where + "\n" +
                                      "Order by DateFactOper";
                    cmd.Parameters.Add(new SqlParameter("@FK_IdOrderDetail", SqlDbType.BigInt));
                    cmd.Parameters["@FK_IdOrderDetail"].Value = FK_IdOrderDetail;
                    cmd.Parameters.Add(new SqlParameter("@NumOper", SqlDbType.VarChar));
                    cmd.Parameters["@NumOper"].Value = NumOper;
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _err = true;
                throw;
            }
        }

        public void Rep7(DateTime DateStart, DateTime DateEnd, bool AllTime, bool AllOrders, string OrderNum, string OrderName)
        {
            try
            {
                SelectRep7(DateStart, DateEnd, AllTime, AllOrders, OrderNum, OrderName);
                if (_DT.Rows.Count == 0)
                {
                    MessageBox.Show("Нет данных для формирования отчёта.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _err = true;
                }
                else//Export data to Excel
                {
                    Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                    //XlReferenceStyle RefStyle = Excel.ReferenceStyle; Excel.Visible = true;
                    ExcelApp.Workbooks.Add(1);
                    Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);
                    //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove; //Сворачиваем сверху
                    #region Настройка шапки
                    ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                    ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
                    ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
                    ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);
                    ExcelWorkSheet.PageSetup.PrintTitleRows = "$5:$5";
                    //Редактирование созданного документа
                    ((Excel.Range)ExcelWorkSheet.Columns[1]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[2]).ColumnWidth = 6;
                    ((Excel.Range)ExcelWorkSheet.Columns[3]).ColumnWidth = 15;
                    ((Excel.Range)ExcelWorkSheet.Columns[4]).ColumnWidth = 20;
                    ((Excel.Range)ExcelWorkSheet.Columns[5]).ColumnWidth = 6;
                    ((Excel.Range)ExcelWorkSheet.Columns[6]).ColumnWidth = 7;
                    ((Excel.Range)ExcelWorkSheet.Columns[7]).ColumnWidth = 30;
                    ((Excel.Range)ExcelWorkSheet.Columns[8]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[9]).ColumnWidth = 10;
                    ((Excel.Range)ExcelWorkSheet.Columns[10]).ColumnWidth = 9;
                    ((Excel.Range)ExcelWorkSheet.Columns[11]).ColumnWidth = 25;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1:K1")).Merge();
                    ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).HorizontalAlignment = Excel.Constants.xlCenter;
                    /*((Excel.Range)ExcelWorkSheet.Columns[8]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[9]).NumberFormat = "h:mm:ss";
                    ((Excel.Range)ExcelWorkSheet.Columns[10]).NumberFormat = "h:mm:ss";*/
                    if (AllTime)
                        ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Value2 = "Выполненные операции за всё время";
                    else ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Value2 = "Выполненные операции за период с " + DateStart.ToShortDateString() + " по " + DateEnd.ToShortDateString();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A2:K2")).Merge();
                    ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).HorizontalAlignment = Excel.Constants.xlCenter;
                    if (AllOrders) ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).Value2 = "по всем заказам.";
                    else ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).Value2 = OrderName;
                    //Рисуем шапку таблицы      
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 1]).Value2 = "Заказ";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 2]).Value2 = "Поз.";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 3]).Value2 = "ЩЦМ";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 4]).Value2 = "Наименование";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 5]).Value2 = "Кол-во";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 6]).Value2 = "Входит";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 7]).Value2 = "Операции";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 8]).Value2 = "н/ч план";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 9]).Value2 = "н/ч факт";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 10]).Value2 = "н/ч откл";
                    ((Excel.Range)ExcelWorkSheet.Cells[4, 11]).Value2 = "Дата - Исполнитель";
                    ((Excel.Range)ExcelWorkSheet.get_Range("A4:K4")).Font.Bold = 1;
                    //**********************************************************************



                    //Пишем сами значения
                    int NumRow = 5; //Номер строки
                    string shcm = "";
                    int groupRow = 0;//Для добавления Передача детали на СГД 
                    int amountOper = 0;//Cчётчик операций для воля ВСЕГО
                    //int AllRows = _DT.Rows.Count + NumRow;//Для отрисовки сетки
                    int PlanTime = 0;//NormTimeFabrication(OnlyOncePay, Tpd, Tsh, AmountDetails)
                    int AllPlanTime = 0;//Общее (н/ч план) время для 1 детали
                    bool OnlyOncePay;//Платим только 1 раз, без учета деталей Tpd + Tsh
                    int Tpd = 0, Tsh = 0;
                    int AmountDetails = 0;//Кол-во операций по технологии (пишется в строку всего - для каждой детали)
                    //*******
                    string NumOper = "";
                    //Остальное для расчёта н/ч факт
                    DataTable DT_fact = new DataTable();
                    Int64 FK_IdOrderDetail;
                    int Tpd_f = 0, Tsh_f = 0;
                    int AmountDetails_f = 0;
                    string DateFactOper_f = "";
                    string NameWorkers = "";
                    string NoteFact = "";//Полное поля для записи в столбец Дата - Исполнитель
                    int FactTime = 0;//NormTimeFabrication(OnlyOncePay_fact, Tpd_f, Tsh_f, AmountDetails_f)
                    int Deviation = 0;//Deviation = PlanTime - FactTime; - это отклонение по каждой операции
                    int AllDeviation = 0;//Общее отклонение по каждой детали
                    bool OnlyOncePay_fact;//Платим только 1 раз, без учета деталей Tpd + Tsh
                    int AllFactTime = 0;//Общее (н/ч факт) время для 1 детали
                    Int16 AmountFactOper = 0;//Кол-во операций по факту (пишется в строку всего - для каждой детали)
                    //Для раздела всего:
                    int RepPlanTime = 0;
                    int RepFactTime = 0;
                    int RepDeviation = 0;

                    for (int i = 0; i < _DT.Rows.Count; i++)
                    {
                        //                            0             1            2               3                   4
                        //SELECT DISTINCT fo.FK_IdOrderDetail,o.OrderNum, od.Position, od.PositionParent, Sp_Tech.NumOper, 
                        //                5                6            7          8               9                  10
                        //Sp_Tech.FK_IdOperation, Sp_Tech.Tpd, Sp_Tech.Tsh,Sp_D.ShcmDetail, Sp_D.NameDetail, cOD.AllCountDetails
                        //            11               12
                        //,Sp_O.NameOperation,Sp_O.OnlyOncePay
                        if (i > 0 & _DT.Rows[i].ItemArray[4].ToString() == "005")
                        {

                            //MessageBox.Show(("F" + (NumRow + i).ToString() + ":F" + (NumRow - 1 + i).ToString()));
                            ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow - 1 + i) + ":A" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow - 1 + i) + ":B" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("C" + (NumRow - 1 + i) + ":C" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("D" + (NumRow - 1 + i) + ":D" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("E" + (NumRow - 1 + i) + ":E" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("F" + (NumRow - 1 + i) + ":F" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 7]).Value2 = "Передача детали на СГД";
                            amountOper++;

                            //********************************************************************************************
                            //есть ли данная операция по факту
                            FK_IdOrderDetail = (Int64)_DT.Rows[i - 1].ItemArray[0];
                            SelectFactOper(ref DT_fact, FK_IdOrderDetail, "", DateStart, DateEnd, AllTime);
                            if (DT_fact.Rows.Count > 0)
                            {
                                DateFactOper_f = Convert.ToDateTime(DT_fact.Rows[0].ItemArray[5]).ToShortDateString();
                                NameWorkers = DT_fact.Rows[0].ItemArray[6].ToString().Trim();
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 11]).Value2 = DateFactOper_f + " - " + NameWorkers;
                                AmountFactOper++;
                            }
                            //*********************************************************************************************

                            NumRow++; //AllRows++;
                            ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow - 1 + i) + ":A" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow - 1 + i) + ":B" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("C" + (NumRow - 1 + i) + ":C" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("D" + (NumRow - 1 + i) + ":D" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("E" + (NumRow - 1 + i) + ":E" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("F" + (NumRow - 1 + i) + ":F" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range((groupRow) + ":" + (NumRow - 1 + i))).Group();
                            //groupRow = 0;
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 7]).Value2 = "Всего(" + AmountFactOper + "/" + amountOper + ")";
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 8]).Value2 = IntToTime((UInt32)AllPlanTime);
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 9]).Value2 = IntToTime((UInt32)AllFactTime);
                            if (AllDeviation > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 10]).Value2 = IntToTime((UInt32)(AllDeviation));
                            RepPlanTime += AllPlanTime;
                            RepFactTime += AllFactTime;
                            RepDeviation += AllDeviation;
                            AllPlanTime = 0;
                            AllFactTime = 0;
                            AllDeviation = 0;
                            AmountFactOper = 0;
                            ((Excel.Range)ExcelWorkSheet.get_Range("G" + (NumRow + i) + ":K" + (NumRow + i))).Font.Bold = 1;
                            NumRow++; //AllRows++;
                        }
                        NumOper = _DT.Rows[i].ItemArray[4].ToString();
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 7]).Value2 = NumOper + " " + _DT.Rows[i].ItemArray[11].ToString();
                        amountOper++;//Увеличиваем счётчик операций после добавления новой строки
                        //**************************************************************************************************
                        //н/ч план
                        //**************************************************************************************************
                        OnlyOncePay = (bool)_DT.Rows[i].ItemArray[12];//Платим только 1 раз, без учета деталей Tpd + Tsh
                        Tpd = (int)_DT.Rows[i].ItemArray[6];
                        Tsh = (int)_DT.Rows[i].ItemArray[7];
                        AmountDetails = (int)_DT.Rows[i].ItemArray[10];
                        PlanTime = NormTimeFabrication(OnlyOncePay, Tpd, Tsh, AmountDetails);
                        AllPlanTime += PlanTime;
                        //if (PlanTime > 0)
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 8]).Value2 = IntToTime((UInt32)PlanTime);
                        //else ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 8]).Value2 = 0;
                        //**************************************************************************************************
                        //н/ч факт
                        //**************************************************************************************************
                        FK_IdOrderDetail = (Int64)_DT.Rows[i].ItemArray[0];
                        DT_fact.Clear();
                        Tpd_f = 0;
                        Tsh_f = 0;
                        AmountDetails_f = 0;
                        DateFactOper_f = "";
                        NameWorkers = "";
                        NoteFact = "";
                        FactTime = 0;
                        Deviation = 0;
                        //          0           1        2   3        4             5            6           7          8
                        //SELECT NumOper,FK_IdOperation,Tpd,Tsh,AmountDetails,DateFactOper,FK_LoginWorker,FullName,OnlyOncePay
                        SelectFactOper(ref DT_fact, FK_IdOrderDetail, NumOper, DateStart, DateEnd, AllTime);
                        if (DT_fact.Rows.Count > 0)
                        {
                            for (int k = 0; k < DT_fact.Rows.Count; k++)
                            {
                                //if (k == 0)//т.к. нормативы меняются и у старых операций нормативы другие могут быть, а потому для каждой операции уточняем нормы
                                {
                                    Tpd_f = (int)DT_fact.Rows[k].ItemArray[2];
                                    Tsh_f = (int)DT_fact.Rows[k].ItemArray[3];
                                }
                                AmountDetails_f = (int)DT_fact.Rows[k].ItemArray[4];
                                DateFactOper_f = Convert.ToDateTime(DT_fact.Rows[k].ItemArray[5]).ToShortDateString();
                                NameWorkers = DT_fact.Rows[k].ItemArray[6].ToString().Trim();
                                if (NameWorkers == "") NameWorkers = DT_fact.Rows[k].ItemArray[7].ToString().Trim();



                                if (NoteFact == "") NoteFact = DateFactOper_f + " - " + NameWorkers;
                                else NoteFact = NoteFact + Environment.NewLine + DateFactOper_f + " - " + NameWorkers;


                                OnlyOncePay_fact = (bool)DT_fact.Rows[k].ItemArray[8];
                                FactTime += NormTimeFabrication(OnlyOncePay_fact, Tpd_f, Tsh_f, AmountDetails_f);
                            }

                            AmountFactOper++;
                        }
                        if (FactTime > 0)
                        {
                            if (PlanTime > FactTime)
                                Deviation = PlanTime - FactTime;
                            else
                                Deviation = FactTime - PlanTime;
                            AllDeviation += Deviation;
                            if (Deviation > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 10]).Value2 = IntToTime((UInt32)Deviation);
                        }
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 9]).Value2 = IntToTime((UInt32)FactTime);
                        AllFactTime += FactTime;
                        ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 11]).Value2 = NoteFact;
                        //**************************************************************************************************
                        shcm = _DT.Rows[i].ItemArray[8].ToString().Trim();
                        if (i > 0 && shcm == _DT.Rows[i - 1].ItemArray[8].ToString().Trim())
                        {
                            ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow + i) + ":A" + (NumRow - 1 + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow + i) + ":B" + (NumRow - 1 + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("C" + (NumRow + i) + ":C" + (NumRow - 1 + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("D" + (NumRow + i) + ":D" + (NumRow - 1 + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("E" + (NumRow + i) + ":E" + (NumRow - 1 + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("F" + (NumRow + i) + ":F" + (NumRow - 1 + i))).Merge();
                            //MessageBox.Show(("F" + (NumRow + i).ToString() + ":F" + (NumRow - 1 + i).ToString()));
                        }
                        else
                        {
                            groupRow = NumRow + i;
                            amountOper = 1;//счётчик операций =  т.к. начинается новая деталь и прошла уже первая запись


                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 1]).Value2 = _DT.Rows[i].ItemArray[1].ToString();
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 2]).Value2 = _DT.Rows[i].ItemArray[2].ToString();
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 3]).Value2 = _DT.Rows[i].ItemArray[8].ToString();
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 4]).Value2 = _DT.Rows[i].ItemArray[9].ToString();
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 5]).Value2 = AmountDetails;//_DT.Rows[i].ItemArray[10].ToString();
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 6]).Value2 = _DT.Rows[i].ItemArray[3].ToString();

                        }
                        //***************************************************************************************************
                        //Для последней записи набора DataSet
                        if (i == _DT.Rows.Count - 1)//Для последней записи набора DataSet
                        {
                            NumRow++; //AllRows++;
                            ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow - 1 + i) + ":A" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow - 1 + i) + ":B" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("C" + (NumRow - 1 + i) + ":C" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("D" + (NumRow - 1 + i) + ":D" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("E" + (NumRow - 1 + i) + ":E" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("F" + (NumRow - 1 + i) + ":F" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 7]).Value2 = "Передача детали на СГД";
                            amountOper++;
                            //********************************************************************************************
                            //есть ли данная операция по факту
                            SelectFactOper(ref DT_fact, FK_IdOrderDetail, "", DateStart, DateEnd, AllTime);
                            if (DT_fact.Rows.Count > 0)
                            {
                                DateFactOper_f = Convert.ToDateTime(DT_fact.Rows[0].ItemArray[5]).ToShortDateString();
                                NameWorkers = DT_fact.Rows[0].ItemArray[6].ToString().Trim();
                                ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 11]).Value2 = DateFactOper_f + " - " + NameWorkers;
                                AmountFactOper++;
                            }

                            //*********************************************************************************************
                            NumRow++;
                            ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow - 1 + i) + ":A" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow - 1 + i) + ":B" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("C" + (NumRow - 1 + i) + ":C" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("D" + (NumRow - 1 + i) + ":D" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("E" + (NumRow - 1 + i) + ":E" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range("F" + (NumRow - 1 + i) + ":F" + (NumRow + i))).Merge();
                            ((Excel.Range)ExcelWorkSheet.get_Range((groupRow) + ":" + (NumRow - 1 + i))).Group();
                            //groupRow = 0;
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 7]).Value2 = "Всего(" + AmountFactOper + "/" + amountOper + ")";
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 8]).Value2 = IntToTime((UInt32)AllPlanTime);
                            ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 9]).Value2 = IntToTime((UInt32)AllFactTime);
                            if (AllDeviation > 0) ((Excel.Range)ExcelWorkSheet.Cells[NumRow + i, 10]).Value2 = IntToTime((UInt32)(AllDeviation));
                            RepPlanTime += AllPlanTime;
                            RepFactTime += AllFactTime;
                            RepDeviation += AllDeviation;
                            AllPlanTime = 0;
                            AllFactTime = 0;
                            AllDeviation = 0;
                            AmountFactOper = 0;


                            ((Excel.Range)ExcelWorkSheet.get_Range("G" + (NumRow + i) + ":K" + (NumRow + i))).Font.Bold = 1;
                            //Последняя строка
                            NumRow += i;
                        }
                        //***************************************************************************************************
                    }
                    //Выравниваем столбцы
                    for (int i = 1; i < 12; i++)
                    {
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).HorizontalAlignment = Excel.Constants.xlCenter;
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).VerticalAlignment = Excel.Constants.xlCenter;
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).WrapText = true;
                        ((Excel.Range)ExcelWorkSheet.Columns[i]).Font.Size = 11;
                    }
                    //Общий итог
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 2, 2]).Value2 = "Сдал:";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 2, 7]).Value2 = "Итого:";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 2, 8]).Value2 = IntToTime((UInt32)(RepPlanTime)); RepPlanTime = 0;
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 2, 9]).Value2 = IntToTime((UInt32)(RepFactTime)); RepFactTime = 0;
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 2, 10]).Value2 = IntToTime((UInt32)(RepDeviation)); RepDeviation = 0;
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 3, 2]).Value2 = "Оператор ИАЦ __________";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 4, 2]).Value2 = "Принял:";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 5, 2]).Value2 = "Диспетчер ППО __________";
                    ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 6, 2]).Value2 = "Начальник цеха __________";
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow + 2) + ":J" + (NumRow + 6))).Font.Bold = 1;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow + 2) + ":J" + (NumRow + 6))).WrapText = false;
                    ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow + 2) + ":B" + (NumRow + 6))).HorizontalAlignment = Excel.Constants.xlLeft;
                    //Рисуем сетку
                    ((Excel.Range)ExcelWorkSheet.get_Range("A4", "K" + NumRow)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _err = true;
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        







        #endregion


        /*  public static void WriteExcel()
{
    try
    {
        //string WayFile = AppDomain.CurrentDomain.BaseDirectory;
        string WayFile = System.Windows.Forms.Application.StartupPath;
        if (Cs_Gper.Ds.Tables["Gild"].Rows.Count > 0)
        {
            #region Запись отчёта XLS
            //--------------------------------------------------------------------------------------------------------
            if (System.IO.File.Exists(WayFile + "/Reports/GildReport.xlsx")) System.IO.File.Delete(WayFile + "/Reports/GildReport.xlsx");
            //System.IO.File.Copy(WayFile + "/Templates/GildReport_temp.xlsx", WayFile + "/Reports/GildReport.xlsx", true);
            Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application();
            XlReferenceStyle RefStyle = Excel.ReferenceStyle;
            //Excel.Visible = true;
            Workbook wb = null;
            String TemplatePath = WayFile + "/Templates/GildReport_temp.xlsx";//шаблон
            try
            {
                wb = Excel.Workbooks.Add(TemplatePath); // !!! 
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("Не удалось загрузить шаблон для экспорта " + TemplatePath + "\n" + ex.Message);
            }
            Worksheet ws = wb.Worksheets.get_Item(1) as Worksheet;
            ws.Unprotect("69");

            //(ws.Cells[1, 1] as Range).Value2 = "Общая сводка принятых переводов по организациям за период с " + dTP_DateLoad.Value.ToShortDateString() + "  по " + dTP_DateLoad_Po.Value.ToShortDateString();
            //int RowsNoSum = 0;
            for (int n = 0; n < Cs_Gper.Ds.Tables["Gild"].Rows.Count; ++n)
            {
                if (Cs_Gper.Ds.Tables["Gild"].Rows.Count == 1) ws.Rows[5].Delete();
                if ((n > 0) && (n < Cs_Gper.Ds.Tables["Gild"].Rows.Count - 1)) ws.Rows[4 + n].Insert();
                //(ws.Cells[4 + n, 1] as Range).Value2 = n + 1;
                (ws.Cells[4 + n, 1] as Range).Value2 = Cs_Gper.Ds.Tables["Gild"].Rows[n]["Num"].ToString().Trim();
                (ws.Cells[4 + n, 2] as Range).Value2 = Cs_Gper.Ds.Tables["Gild"].Rows[n]["Code"].ToString().Trim();
                (ws.Cells[4 + n, 3] as Range).Value2 = Cs_Gper.Ds.Tables["Gild"].Rows[n]["NameDetail"].ToString().Trim();
                // (ws.Cells[4 + n, 5] as Range).Value2 = Convert.ToDecimal(Gper.Ds.Tables["GeneralReport"].Rows[n]["SUMI"].ToString().Trim());
            }
            //----------------------------------------------
            ws.Protect("69", Type.Missing, Type.Missing, Type.Missing, Type.Missing, true, true, true, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true, true, true);
            Excel.ReferenceStyle = RefStyle;
            Excel.Visible = true;
            //wb.SaveAs(WayFile + "/Reports/GildReport.xls", -4143);
            //wb.SaveAs(WayFile + "/Reports/GildReport.xlsx");
            //wb.Close(WayFile + "/Reports/GildReport.xlsx");
            ReleaseExcel(Excel as Object);
            //-------------------------------------------------------------------------------------------------------
            #endregion
            MessageBox.Show("Выгрузка успешно завершена. " + Cs_Gper.Ds.Tables["Gild"].Rows.Count, "Завершено!!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
            MessageBox.Show("Данные отсутствуют. ", "Завершено!!!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

    }
    catch (Exception ex)
    {
        MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

*/



        //Экспорт данных из таблицы на форме F_Kit
        public void ExpKitToExcel(string All_OrderNum, DataTable _DT, int numrows, int OrdersCount)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                //XlReferenceStyle RefStyle = Excel.ReferenceStyle; Excel.Visible = true;
                ExcelApp.Workbooks.Add(1);
                Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);
                //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove; //Сворачиваем сверху
                #region Настройка шапки
                ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
                ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
                ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
                ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
                ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
                ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);
                ExcelWorkSheet.PageSetup.PrintTitleRows = "$5:$5";
                //Редактирование созданного документа
                byte Col = 0;//Смещение остальных столбцов из-за 1 столбца Заказы (если заказов несколько)

                    if (OrdersCount > 1) Col = 1;
                    ((Excel.Range)ExcelWorkSheet.Columns[1 + Col]).ColumnWidth = 5;
                    ((Excel.Range)ExcelWorkSheet.Columns[1 + Col]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Columns[2 + Col]).ColumnWidth = 5;
                    ((Excel.Range)ExcelWorkSheet.Columns[2 + Col]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Columns[3 + Col]).ColumnWidth = 20;
                    ((Excel.Range)ExcelWorkSheet.Columns[3 + Col]).WrapText = true;
                    ((Excel.Range)ExcelWorkSheet.Columns[4 + Col]).ColumnWidth = 6;
                    ((Excel.Range)ExcelWorkSheet.Columns[4 + Col]).HorizontalAlignment = Excel.Constants.xlLeft;
                if (Col == 0)
                {
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1:G1")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1:G3")).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1:G3")).Font.Bold = 1;
                }
                else
                {
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1:H1")).Merge();
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1:H3")).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.get_Range("A1:H3")).Font.Bold = 1;
                }
                ((Excel.Range)ExcelWorkSheet.Columns[5 + Col]).ColumnWidth = 90;
                ((Excel.Range)ExcelWorkSheet.Columns[5 + Col]).WrapText = true;
                ((Excel.Range)ExcelWorkSheet.Columns[6 + Col]).ColumnWidth = 8;
                ((Excel.Range)ExcelWorkSheet.Columns[6 + Col]).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.Columns[7 + Col]).ColumnWidth = 8;
                ((Excel.Range)ExcelWorkSheet.Columns[7 + Col]).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Value2 = "Заказ № " + All_OrderNum + " на " + DateTime.Now.ToShortDateString();
                ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).WrapText = true;
                //Костыль мля
                ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).RowHeight = numrows * 12.75;
                //********************************************************************
                //((Excel.Range)ExcelWorkSheet.Columns[8]).NumberFormat = "h:mm:ss";
                //Рисуем шапку таблицы      
                if (Col == 1) ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 = "Заказ";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 1 + Col]).Value2 = "№";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 2 + Col]).Value2 = "Поз.";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 3 + Col]).Value2 = "ЩЦМ.";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 4 + Col]).Value2 = "Кол-во";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 5 + Col]).Value2 = "Наименование комплектации";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 6 + Col]).Value2 = "План";
                //**********************************************************************
                for (int i = 0; i < _DT.Rows.Count; i++)
                {
                    if (Col == 1) ((Excel.Range)ExcelWorkSheet.Cells[i + 4, 1]).Value2 = _DT.Rows[i].ItemArray[9];
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 4, 1 + Col]).Value2 = _DT.Rows[i].ItemArray[0];
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 4, 2 + Col]).Value2 = _DT.Rows[i].ItemArray[1];
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 4, 3 + Col]).Value2 = _DT.Rows[i].ItemArray[3];
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 4, 4 + Col]).Value2 = _DT.Rows[i].ItemArray[4];
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 4, 5 + Col]).Value2 = _DT.Rows[i].ItemArray[7];
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 4, 6 + Col]).Value2 = _DT.Rows[i].ItemArray[5];
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 4, 7 + Col]).Value2 = _DT.Rows[i].ItemArray[10];
                    //((Excel.Range)ExcelWorkSheet.Cells[i + 4, 8 + Col]).Value2 = _DT.Rows[i].ItemArray[8];//IdLoodsman - Комплектации
                }
                //Рисуем сетку
                if (Col == 1)
                    ((Excel.Range)ExcelWorkSheet.get_Range("A3", "G" + (_DT.Rows.Count + 3))).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                else
                    ((Excel.Range)ExcelWorkSheet.get_Range("A3", "F" + (_DT.Rows.Count + 3))).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;



                //Выравниваем столбцы
                /*for (int i = 1; i < 12; i++)
                {
                    ((Excel.Range)ExcelWorkSheet.Columns[i]).HorizontalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Columns[i]).VerticalAlignment = Excel.Constants.xlCenter;
                    ((Excel.Range)ExcelWorkSheet.Columns[i]).WrapText = true;
                    ((Excel.Range)ExcelWorkSheet.Columns[i]).Font.Size = 11;
                }
                //Общий итог
                ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow + 2) + ":J" + (NumRow + 6))).Font.Bold = 1;
                ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow + 2) + ":J" + (NumRow + 6))).WrapText = false;
                ((Excel.Range)ExcelWorkSheet.get_Range("B" + (NumRow + 2) + ":B" + (NumRow + 6))).HorizontalAlignment = Excel.Constants.xlLeft;
                */
                #endregion
            }
            catch (Exception ex)
            {
                _err = true;
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExpKitToExcelSvod(string All_OrderNum, DataTable _DT, int numrows)
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                //XlReferenceStyle RefStyle = Excel.ReferenceStyle; Excel.Visible = true;
                ExcelApp.Workbooks.Add(1);
                Excel.Worksheet ExcelWorkSheet = (Excel.Worksheet)ExcelApp.Sheets.get_Item(1);
                //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove; //Сворачиваем сверху
                #region Настройка шапки
                ExcelWorkSheet.PageSetup.Orientation = Excel.XlPageOrientation.xlLandscape;
                ExcelWorkSheet.PageSetup.LeftMargin = ExcelApp.CentimetersToPoints(0.25);
                ExcelWorkSheet.PageSetup.RightMargin = ExcelApp.CentimetersToPoints(0.25);
                ExcelWorkSheet.PageSetup.TopMargin = ExcelApp.CentimetersToPoints(0.75);
                ExcelWorkSheet.PageSetup.BottomMargin = ExcelApp.CentimetersToPoints(0.75);
                ExcelWorkSheet.PageSetup.HeaderMargin = ExcelApp.CentimetersToPoints(0.3);
                ExcelWorkSheet.PageSetup.FooterMargin = ExcelApp.CentimetersToPoints(0.3);
                ExcelWorkSheet.PageSetup.PrintTitleRows = "$5:$5";
                //Редактирование созданного документа
                ((Excel.Range)ExcelWorkSheet.Columns[1]).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.Columns[2]).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.Columns[4]).HorizontalAlignment = Excel.Constants.xlLeft;
                ((Excel.Range)ExcelWorkSheet.Columns[6]).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.Columns[7]).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.Columns[8]).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.Columns[9]).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.get_Range("A1:I1")).Merge();
                ((Excel.Range)ExcelWorkSheet.get_Range("A1:I4")).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.get_Range("A1:I4")).Font.Bold = 1;
                ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Value2 = "Заказ № " + All_OrderNum + " на " + DateTime.Now.ToShortDateString();
                ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).WrapText = true;
                ((Excel.Range)ExcelWorkSheet.Columns[1]).ColumnWidth = 4;
                ((Excel.Range)ExcelWorkSheet.Columns[2]).ColumnWidth = 7;
                ((Excel.Range)ExcelWorkSheet.Columns[3]).ColumnWidth = 80;
                ((Excel.Range)ExcelWorkSheet.Columns[4]).ColumnWidth = 8;
                ((Excel.Range)ExcelWorkSheet.Columns[5]).ColumnWidth = 10;
                ((Excel.Range)ExcelWorkSheet.Columns[6]).ColumnWidth = 10;
                ((Excel.Range)ExcelWorkSheet.Columns[7]).ColumnWidth = 10;
                ((Excel.Range)ExcelWorkSheet.Columns[8]).ColumnWidth = 10;
                ((Excel.Range)ExcelWorkSheet.Columns[9]).ColumnWidth = 16;
                ((Excel.Range)ExcelWorkSheet.Columns[3]).WrapText = true;
                ((Excel.Range)ExcelWorkSheet.Columns[4]).WrapText = true;
                
                //Костыль мля
                ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).RowHeight = numrows * 12.75;
                //********************************************************************
                //((Excel.Range)ExcelWorkSheet.Columns[8]).NumberFormat = "h:mm:ss";
                //Рисуем шапку таблицы
                ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 = "п/п";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 2]).Value2 = "Код";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 3]).Value2 = "Наименование";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 4]).Value2 = "Код ОКП";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 5]).Value2 = "Поставщик";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 6]).Value2 = "Кол-во план";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 7]).Value2 = "Кол-во Факт";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 8]).Value2 = "№к.Лоц.1С";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 9]).Value2 = "Номенклатурный №";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 1]).Value2 = "1";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 2]).Value2 = "2";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 3]).Value2 = "3";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 4]).Value2 = "4";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 5]).Value2 = "5";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 6]).Value2 = "6";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 7]).Value2 = "7";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 8]).Value2 = "8";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 9]).Value2 = "9";
                //**********************************************************************
                for (int i = 0; i < _DT.Rows.Count; i++)
                {
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 5, 1]).Value2 = _DT.Rows[i].ItemArray[0];//п/п
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 5, 2]).Value2 = _DT.Rows[i].ItemArray[8];//IdLoodsman - Комплектации
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 5, 3]).Value2 = _DT.Rows[i].ItemArray[7];//Наименование
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 5, 6]).Value2 = _DT.Rows[i].ItemArray[5];//План
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 5, 7]).Value2 = _DT.Rows[i].ItemArray[10];//Факт
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 5, 8]).Value2 = _DT.Rows[i].ItemArray[11];//№к.Лоц.1С
                    ((Excel.Range)ExcelWorkSheet.Cells[i + 5, 9]).Value2 = _DT.Rows[i].ItemArray[12];//Номенклатурный №
                }
                //Рисуем сетку
                ((Excel.Range)ExcelWorkSheet.get_Range("A3", "I" + (_DT.Rows.Count + 4))).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                #endregion
            }
            catch (Exception ex)
            {
                _err = true;
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
