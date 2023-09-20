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
using Dispetcher2.Controls.MyGrid;
using Excel = Microsoft.Office.Interop.Excel;

namespace Dispetcher2
{
    public partial class F_TimeSheets : Form
    {
        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_TimeSheetsV1 TSHV1;
        IConfig config;

        DataTable _DT_Workers = new DataTable();
        DataTable DT_Holidays = new DataTable();

        public F_TimeSheets(IConfig config)
        {
            this.config = config;
            TSHV1 = new C_TimeSheetsV1(config);
            
            
            InitializeComponent();
        }

        private void F_TimeSheets_Load(object sender, EventArgs e)
        {
            numUD_year.Value = DateTime.Now.Year;
            cB_Month.SelectedIndex = DateTime.Now.Month - 1;
            /*myGrid_TimeSH.Redim(30, 22);
            CreateHeader();
            AddDataToMyGrid();*/
        }

        #region Greed
        private void CreateGrid()
        {

            myGrid_TimeSH.Redim(_DT_Workers.Rows.Count * 2 + 2, 23);
            myGrid_TimeSH.FixedRows = 2;
            myGrid_TimeSH.FixedColumns = 3;
            myGrid_TimeSH.Selection.EnableMultiSelection = true;
            
            //SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell();
            //captionModel.BackColor = myGrid_TimeSH.BackColor;
            /*myGrid_TimeSH.Rows[0].AutoSizeMode = SourceGrid.AutoSizeMode.None;
            myGrid_TimeSH.Rows[1].AutoSizeMode = SourceGrid.AutoSizeMode.None;
            myGrid_TimeSH.Rows[2].AutoSizeMode = SourceGrid.AutoSizeMode.None;*/
            //HEADER
            //1 Header Row
            myGrid_TimeSH[0, 0] = new MyHeader("№\nп/п");
            myGrid_TimeSH[0, 0].RowSpan = 2;
            myGrid_TimeSH[0, 1] = new MyHeader("Фамилия Имя Отчество\nЗанимаемая должность");
            myGrid_TimeSH[0, 1].RowSpan = 2;
            myGrid_TimeSH[0, 1].Column.Width = 250;
            myGrid_TimeSH[0, 1].Column.MinimalWidth = 200;
            myGrid_TimeSH[0, 2] = new MyHeader("Табельный\nномер");
            myGrid_TimeSH[0, 2].RowSpan = 2;
            myGrid_TimeSH[0, 2].Column.Width = 100;
            myGrid_TimeSH[0, 2].Column.MinimalWidth = 100;
            //2 Header Row
            myGrid_TimeSH[0, 3] = new MyHeader("1");
            myGrid_TimeSH[0, 4] = new MyHeader("2");
            myGrid_TimeSH[0, 5] = new MyHeader("3");
            myGrid_TimeSH[0, 6] = new MyHeader("4");
            myGrid_TimeSH[0, 7] = new MyHeader("5");
            myGrid_TimeSH[0, 8] = new MyHeader("6");
            myGrid_TimeSH[0, 9] = new MyHeader("7");
            myGrid_TimeSH[0, 10] = new MyHeader("8");
            myGrid_TimeSH[0, 11] = new MyHeader("9");
            myGrid_TimeSH[0, 12] = new MyHeader("10");
            myGrid_TimeSH[0, 13] = new MyHeader("11");
            myGrid_TimeSH[0, 14] = new MyHeader("12");
            myGrid_TimeSH[0, 15] = new MyHeader("13");
            myGrid_TimeSH[0, 16] = new MyHeader("14");
            myGrid_TimeSH[0, 17] = new MyHeader("15");
            myGrid_TimeSH[0, 18] = new MyHeader("16");
            myGrid_TimeSH[0, 19] = new MyHeader("Дни");
            myGrid_TimeSH[0, 19].Column.Width = 60;
            myGrid_TimeSH[0, 19].Column.MinimalWidth = 60;
            myGrid_TimeSH[0, 20] = new MyHeader("Заказ");
            myGrid_TimeSH[0, 20].Column.Width = 80;
            myGrid_TimeSH[0, 20].Column.MinimalWidth = 60;
            myGrid_TimeSH[0, 20].RowSpan = 2;
            myGrid_TimeSH[0, 21] = new MyHeader("Примеч.");
            myGrid_TimeSH[0, 21].Column.Width = 200;
            myGrid_TimeSH[0, 21].Column.MinimalWidth = 100;
            myGrid_TimeSH[0, 21].RowSpan = 2;
            myGrid_TimeSH[0, 22] = new MyHeader("Login");
            myGrid_TimeSH[0, 22].Column.Width = 200;
            myGrid_TimeSH[0, 22].Column.MinimalWidth = 100;
            myGrid_TimeSH[0, 22].RowSpan = 2;
            //3 Header Row
            myGrid_TimeSH[1, 3] = new MyHeader("17");
            myGrid_TimeSH[1, 4] = new MyHeader("18");
            myGrid_TimeSH[1, 5] = new MyHeader("19");
            myGrid_TimeSH[1, 6] = new MyHeader("20");
            myGrid_TimeSH[1, 7] = new MyHeader("21");
            myGrid_TimeSH[1, 8] = new MyHeader("22");
            myGrid_TimeSH[1, 9] = new MyHeader("23");
            myGrid_TimeSH[1, 10] = new MyHeader("24");
            myGrid_TimeSH[1, 11] = new MyHeader("25");
            myGrid_TimeSH[1, 12] = new MyHeader("26");
            myGrid_TimeSH[1, 13] = new MyHeader("27");
            myGrid_TimeSH[1, 14] = new MyHeader("28");
            myGrid_TimeSH[1, 15] = new MyHeader("29");
            myGrid_TimeSH[1, 16] = new MyHeader("30");
            myGrid_TimeSH[1, 17] = new MyHeader("31");
            myGrid_TimeSH[1, 18] = null;
            // myGrid_TimeSH[1, 18] = new MyHeader("");
            myGrid_TimeSH[1, 19] = new MyHeader("Часы");
            for (int i = 3; i < 19; i++)
            {
                myGrid_TimeSH.Columns.SetWidth(i, 40);
                myGrid_TimeSH[0, i].Column.MinimalWidth = 40;
            }
            //myGrid_TimeSH.AutoSizeCells();
            AddDataToMyGrid();
        }

        private void AddDataToMyGrid()
        {
            int numRows = 0;
            int StatrInd = 2;
            int offset = 0;
            if (numRows < _DT_Workers.Rows.Count)
            {
                if (_DT_Workers.Rows[numRows].ItemArray[4].ToString() == "True")
                {
                    myGrid_TimeSH.Rows.Insert(StatrInd + offset);
                    myGrid_TimeSH[StatrInd + offset, 0] = new MyCell("ИТР, специалисты и служащий персонал производства 50", typeof(string));
                    myGrid_TimeSH[StatrInd + offset, 0].ColumnSpan = 23;
                    myGrid_TimeSH[StatrInd + offset, 0].Editor.EnableEdit = false;
                    myGrid_TimeSH[StatrInd + offset, 0].View.BackColor = System.Drawing.SystemColors.ControlLight;
                    offset++;
                }
                AddData(ref StatrInd, ref offset, ref numRows, true);
            }
            if (numRows < _DT_Workers.Rows.Count)
            {
                if (_DT_Workers.Rows[numRows].ItemArray[4].ToString() == "False")
                {
                    myGrid_TimeSH.Rows.Insert(StatrInd + offset);
                    myGrid_TimeSH[StatrInd + offset, 0] = new MyCell("Цех по изготовлению СТО", typeof(string));
                    myGrid_TimeSH[StatrInd + offset, 0].ColumnSpan = 23;
                    myGrid_TimeSH[StatrInd + offset, 0].Editor.EnableEdit = false;
                    myGrid_TimeSH[StatrInd + offset, 0].View.BackColor = System.Drawing.SystemColors.ControlLight;
                    offset++;
                }
                AddData(ref StatrInd, ref offset, ref numRows, false);
            }

            /*for (int r = 0; r < myGrid_TimeSH.RowsCount; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (myGrid_TimeSH[r, c].Editor != null)
                    {
                        myGrid_TimeSH[r, c].Editor.EnableEdit = false;
                        myGrid_TimeSH[r, c].View.BackColor = System.Drawing.SystemColors.Info;
                    }
                }
            }*/

        }

        private void AddData(ref int StatrInd, ref int _offset, ref int numRows, bool ITR)
        {

            /* SourceGrid.Cells.Editors.TextBoxNumeric numericEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(double));
             numericEditor.KeyPress += delegate(object sender, KeyPressEventArgs e)
             {
                 //if (e.KeyChar == ',') e.KeyChar = '.';

                 bool isValid = char.IsNumber(e.KeyChar) ||
                     e.KeyChar == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

                 e.Handled = !isValid;
             };*/
            PopupMenu menuController = new PopupMenu(myGrid_TimeSH);

            for (int r = StatrInd + _offset; r < myGrid_TimeSH.RowsCount; r = r + 2)
            {
                if (ITR & _DT_Workers.Rows[numRows].ItemArray[4].ToString() == "False") break;
                //myGrid_TimeSH[r, 0] = new SourceGrid.Cells.Cell(r - 2, typeof(string));
                myGrid_TimeSH[r, 0] = new MyCell(numRows + 1, typeof(string));
                myGrid_TimeSH[r, 0].RowSpan = 2;
                myGrid_TimeSH[r, 0].Editor.EnableEdit = false;
                myGrid_TimeSH[r, 0].View.BackColor = System.Drawing.SystemColors.Info;
                //myGrid_TimeSH[r, 1] = new MyCell("Акыева Оксана Николаевна", typeof(string));
                myGrid_TimeSH[r, 1] = new MyCell(_DT_Workers.Rows[numRows].ItemArray[1].ToString(), typeof(string));
                myGrid_TimeSH[r, 1].Editor.EnableEdit = false;
                myGrid_TimeSH[r, 1].View.BackColor = System.Drawing.SystemColors.ControlLightLight;
                myGrid_TimeSH[r + 1, 1] = new MyCell(_DT_Workers.Rows[numRows].ItemArray[2].ToString(), typeof(string));
                myGrid_TimeSH[r + 1, 1].Editor.EnableEdit = false;
                myGrid_TimeSH[r + 1, 1].View.BackColor = System.Drawing.SystemColors.ActiveCaption;
                myGrid_TimeSH[r, 2] = new MyCell(_DT_Workers.Rows[numRows].ItemArray[3].ToString(), typeof(string));
                myGrid_TimeSH[r, 2].RowSpan = 2;
                myGrid_TimeSH[r, 2].Editor.EnableEdit = false;
                myGrid_TimeSH[r, 2].View.BackColor = System.Drawing.SystemColors.Info;
                myGrid_TimeSH[r, 22] = new MyCell(_DT_Workers.Rows[numRows].ItemArray[0].ToString(), typeof(string));
                myGrid_TimeSH[r, 22].RowSpan = 2;
                myGrid_TimeSH[r, 22].Editor.EnableEdit = false;
                myGrid_TimeSH[r, 22].View.BackColor = System.Drawing.SystemColors.Info;
                for (int col = 3; col < 19; col++)
                {
                    myGrid_TimeSH[r, col] = new MyCell("", typeof(string));
                    myGrid_TimeSH[r, col].AddController(menuController);
                    //myGrid_TimeSH[r, col].Value = 8;
                    if (col < 18)
                    {
                        myGrid_TimeSH[r + 1, col] = new MyCell("", typeof(string));
                        myGrid_TimeSH[r + 1, col].AddController(menuController);
                        //myGrid_TimeSH[r, col].Value = 8;
                    }
                    else myGrid_TimeSH[r + 1, col] = null;

                    if (DT_Holidays.Rows[col - 3].ItemArray[1].ToString() == "0")
                    {
                        myGrid_TimeSH[r, col].Value = "В";
                        myGrid_TimeSH[r, col].View.BackColor = System.Drawing.Color.PaleGreen;
                    }
                    else myGrid_TimeSH[r, col].Value = 8;
                    if (col - 3 + 16 < DT_Holidays.Rows.Count)
                    {
                        if (DT_Holidays.Rows[col - 3 + 16].ItemArray[1].ToString() == "0")
                        {
                            myGrid_TimeSH[r + 1, col].Value = "В";
                            myGrid_TimeSH[r + 1, col].View.BackColor = System.Drawing.Color.PaleGreen;
                        }
                        else
                            myGrid_TimeSH[r + 1, col].Value = 8;
                    }

                    if (DT_Holidays.Rows.Count < 29)
                    {
                        myGrid_TimeSH[r + 1, 15] = null;
                        myGrid_TimeSH[r + 1, 16] = null;
                        myGrid_TimeSH[r + 1, 17] = null;
                    }
                    else
                    if (DT_Holidays.Rows.Count < 30)
                    {
                        myGrid_TimeSH[r + 1, 16] = null;
                        myGrid_TimeSH[r + 1, 17] = null;
                    }
                    else
                    if (DT_Holidays.Rows.Count < 31) myGrid_TimeSH[r + 1, 17] = null;




                    //myGrid_TimeSH[r, col].Editor = numericEditor;
                }
                myGrid_TimeSH[r, 19] = new MyCell("", typeof(int));//Дни
                myGrid_TimeSH[r + 1, 19] = new MyCell("", typeof(double));//Часы
                myGrid_TimeSH[r, 20] = new MyCell("", typeof(string));//Заказ
                myGrid_TimeSH[r, 20].RowSpan = 2;
                myGrid_TimeSH[r, 21] = new MyCell("", typeof(string));//Примеч.
                myGrid_TimeSH[r, 21].RowSpan = 2;
                numRows++;
                _offset = _offset + 2;
            }
        }

        private class PopupMenu : SourceGrid.Cells.Controllers.ControllerBase
        {
            ContextMenu menu = new ContextMenu();
            private MyGrid _MyGrid;
            


            public PopupMenu(MyGrid Grid)
            {
                _MyGrid = Grid;
                
                menu.MenuItems.Add("Б", new EventHandler(MenuB_Click));
                menu.MenuItems.Add("В", new EventHandler(MenuV_Click));
                menu.MenuItems.Add("Г", new EventHandler(MenuG_Click));
                menu.MenuItems.Add("ДО", new EventHandler(MenuDO_Click));
                menu.MenuItems.Add("ОЖ", new EventHandler(MenuOZH_Click));
                menu.MenuItems.Add("ОТ", new EventHandler(MenuOT_Click));
                menu.MenuItems.Add("ПР", new EventHandler(MenuPR_Click));
                menu.MenuItems.Add("Р", new EventHandler(MenuR_Click));
            }

            public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
            {
                base.OnMouseUp(sender, e);

                if (e.Button == MouseButtons.Right)
                    menu.Show(sender.Grid, new Point(e.X, e.Y));
            }

            private void MenuB_Click(object sender, EventArgs e)
            {
                
                if (_MyGrid.Selection.ActivePosition.ToString() != "-1;-1")
                {
                    try
                    {
                        string pos = _MyGrid.Selection.GetSelectionRegion().ToString().Replace(" to ", ";").Replace("RangeRegion | ", "");
                        string[] pos2 = pos.Split(';');
                        int count = Convert.ToInt32(pos2[3]) - Convert.ToInt32(pos2[1]);
                        int counter = count == 0 ? 1 : count + 1;

                        for (int i = counter, j = Convert.ToInt32(pos2[1]); i > 0; i--, j++)
                        {
                            _MyGrid[Convert.ToInt32(pos2[0]), j].Value = "Б";
                            if (pos2[0] != pos2[2]) _MyGrid[Convert.ToInt32(pos2[2]), j].Value = "Б";
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Выберите дни корректно и на одной строке", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            private void MenuV_Click(object sender, EventArgs e)
            {
                try
                {
                    string pos = _MyGrid.Selection.GetSelectionRegion().ToString().Replace(" to ", ";").Replace("RangeRegion | ", "");
                    string[] pos2 = pos.Split(';');
                    int count = Convert.ToInt32(pos2[3]) - Convert.ToInt32(pos2[1]);
                    int counter = count == 0 ? 1 : count + 1;

                    for (int i = counter, j = Convert.ToInt32(pos2[1]); i > 0; i--, j++)
                    {
                        _MyGrid[Convert.ToInt32(pos2[0]), j].Value = "В";
                        if (pos2[0] != pos2[2]) _MyGrid[Convert.ToInt32(pos2[2]), j].Value = "В";
                    }
                }
                catch
                {
                    MessageBox.Show("Выберите дни для изменения строго на одной строке", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            private void MenuG_Click(object sender, EventArgs e)
            {
                try
                {
                    string pos = _MyGrid.Selection.GetSelectionRegion().ToString().Replace(" to ", ";").Replace("RangeRegion | ", "");
                    string[] pos2 = pos.Split(';');
                    int count = Convert.ToInt32(pos2[3]) - Convert.ToInt32(pos2[1]);
                    int counter = count == 0 ? 1 : count + 1;

                    for (int i = counter, j = Convert.ToInt32(pos2[1]); i > 0; i--, j++)
                    {
                        _MyGrid[Convert.ToInt32(pos2[0]), j].Value = "Г";
                        if (pos2[0] != pos2[2]) _MyGrid[Convert.ToInt32(pos2[2]), j].Value = "Г";
                    }
                }
                catch
                {
                    MessageBox.Show("Выберите дни для изменения строго на одной строке", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            private void MenuDO_Click(object sender, EventArgs e)
            {
                try
                {
                    string pos = _MyGrid.Selection.GetSelectionRegion().ToString().Replace(" to ", ";").Replace("RangeRegion | ", "");
                    string[] pos2 = pos.Split(';');
                    int count = Convert.ToInt32(pos2[3]) - Convert.ToInt32(pos2[1]);
                    int counter = count == 0 ? 1 : count + 1;

                    for (int i = counter, j = Convert.ToInt32(pos2[1]); i > 0; i--, j++)
                    {
                        _MyGrid[Convert.ToInt32(pos2[0]), j].Value = "ДО";
                        if (pos2[0] != pos2[2]) _MyGrid[Convert.ToInt32(pos2[2]), j].Value = "ДО";
                    }
                }
                catch
                {
                    MessageBox.Show("Выберите дни для изменения строго на одной строке", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            private void MenuOZH_Click(object sender, EventArgs e)
            {
                try
                {
                    string pos = _MyGrid.Selection.GetSelectionRegion().ToString().Replace(" to ", ";").Replace("RangeRegion | ", "");
                    string[] pos2 = pos.Split(';');
                    int count = Convert.ToInt32(pos2[3]) - Convert.ToInt32(pos2[1]);
                    int counter = count == 0 ? 1 : count + 1;

                    for (int i = counter, j = Convert.ToInt32(pos2[1]); i > 0; i--, j++)
                    {
                        _MyGrid[Convert.ToInt32(pos2[0]), j].Value = "ОЖ";
                        if (pos2[0] != pos2[2]) _MyGrid[Convert.ToInt32(pos2[2]), j].Value = "ОЖ";
                    }
                }
                catch
                {
                    MessageBox.Show("Выберите дни для изменения строго на одной строке", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            private void MenuOT_Click(object sender, EventArgs e)
            {
                try
                {
                    string pos = _MyGrid.Selection.GetSelectionRegion().ToString().Replace(" to ", ";").Replace("RangeRegion | ", "");
                    string[] pos2 = pos.Split(';');
                    int count = Convert.ToInt32(pos2[3]) - Convert.ToInt32(pos2[1]);
                    int counter = count == 0 ? 1 : count + 1;

                    for (int i = counter, j = Convert.ToInt32(pos2[1]); i > 0; i--, j++)
                    {
                        _MyGrid[Convert.ToInt32(pos2[0]), j].Value = "ОТ";
                        if (pos2[0] != pos2[2]) _MyGrid[Convert.ToInt32(pos2[2]), j].Value = "ОТ";
                    }
                }
                catch
                {
                    MessageBox.Show("Выберите дни для изменения строго на одной строке", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            private void MenuPR_Click(object sender, EventArgs e)
            {
                try
                {
                    string pos = _MyGrid.Selection.GetSelectionRegion().ToString().Replace(" to ", ";").Replace("RangeRegion | ", "");
                    string[] pos2 = pos.Split(';');
                    int count = Convert.ToInt32(pos2[3]) - Convert.ToInt32(pos2[1]);
                    int counter = count == 0 ? 1 : count + 1;

                    for (int i = counter, j = Convert.ToInt32(pos2[1]); i > 0; i--, j++)
                    {
                        _MyGrid[Convert.ToInt32(pos2[0]), j].Value = "ПР";
                        if (pos2[0] != pos2[2]) _MyGrid[Convert.ToInt32(pos2[2]), j].Value = "ПР";
                    }
                }
                catch
                {
                    MessageBox.Show("Выберите дни для изменения строго на одной строке", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            private void MenuR_Click(object sender, EventArgs e)
            {
                try
                {
                string pos = _MyGrid.Selection.GetSelectionRegion().ToString().Replace(" to ", ";").Replace("RangeRegion | ", "");
                string[] pos2 = pos.Split(';');
                int count = Convert.ToInt32(pos2[3]) - Convert.ToInt32(pos2[1]);
                int counter = count == 0 ? 1 : count + 1;

                for (int i = counter, j = Convert.ToInt32(pos2[1]); i > 0; i--, j++)
                    {
                    _MyGrid[Convert.ToInt32(pos2[0]), j].Value = "Р";
                    if (pos2[0] != pos2[2]) _MyGrid[Convert.ToInt32(pos2[2]), j].Value = "Р";
                    }
                }
                catch
                {
                    MessageBox.Show("Выберите дни для изменения строго на одной строке", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }
        #endregion

        private void CheckOrInsert(bool Check, int month, int year)
        {
            try
            {
                if (myGrid_TimeSH.RowsCount > 0)
                {
                    bool First15days = true;
                    DateTime dt_date;
                    string st_date;
                    bool Note = false;
                    decimal test = 0;//необходим для подсчёта дней, а главное общего количества часов
                    byte Tsn_days = 0;//Количество выходов на работу
                    decimal Tsn_hours = 0;//Общее количество отработанных часов для каждого сотрудника
                    for (int i = 2; i < myGrid_TimeSH.RowsCount; i++)
                    {
                        Note = false;//В начале каждой строки т.к. пишем только при First15days = true;
                        if (myGrid_TimeSH[i, 22] != null)
                            if (myGrid_TimeSH[i, 22].Value != null)
                                TSHV1.LoginUs = myGrid_TimeSH[i, 22].Value.ToString();
                        if (TSHV1.LoginUs != "ИТР, специалисты и служащий персонал производства 50" &&
                            TSHV1.LoginUs != "Цех по изготовлению СТО")
                            for (int cl = 3; cl < 19; cl++)
                            {
                                if (myGrid_TimeSH[i, cl] != null)
                                {
                                    if (First15days) st_date = (cl - 2).ToString();
                                    else st_date = (cl + 14).ToString();
                                    st_date += "." + (cB_Month.SelectedIndex + 1).ToString() + "." + numUD_year.Value.ToString();
                                    DateTime.TryParse(st_date, out dt_date);
                                    TSHV1.PK_Date = dt_date;
                                    if (myGrid_TimeSH[i, cl].Value != null)
                                        TSHV1.Val_Time = myGrid_TimeSH[i, cl].Value.ToString();
                                    else TSHV1.Val_Time = "";
                                    myGrid_TimeSH[i, cl].Value = TSHV1.Val_Time;
                                    if (Check)
                                    {
                                        if (!TSHV1.CheckData())
                                        {
                                            myGrid_TimeSH[i, cl].View.BackColor = System.Drawing.Color.LightPink;
                                            myGrid_TimeSH.Refresh();
                                        }
                                        else
                                            if (TSHV1.CheckData() & myGrid_TimeSH[i, cl].View.BackColor == System.Drawing.Color.LightPink)
                                        {
                                            myGrid_TimeSH[i, cl].View.BackColor = SystemColors.InactiveBorder;
                                            myGrid_TimeSH.Refresh();
                                        }
                                        //if (decimal.TryParse(TSHV1.Val_Time, C_Gper.style, C_Gper.culture, out test))
                                        
                                        {
                                            test = Converter.GetDecimal(TSHV1.Val_Time);
                                            if (test > 0)
                                            {
                                                Tsn_days++;
                                                Tsn_hours += test;
                                            }
                                        }
                                    }


                                    if (!Check)//Значит все проверки были пройдены - пишем в БД.
                                    {
                                        TSHV1.InsertData(); //Пишем данные дня.
                                        if (!Note && First15days)//Пишем примечание 1 раз
                                        {
                                            string NoteText = "";
                                            if (myGrid_TimeSH[i, 21].Value != null) NoteText = myGrid_TimeSH[i, 21].Value.ToString().Trim();
                                            if (myGrid_TimeSH[i, 19].Value != null) Tsn_days = Convert.ToByte(myGrid_TimeSH[i, 19].Value);
                                            if (myGrid_TimeSH[i + 1, 19].Value != null) Tsn_hours = Convert.ToDecimal(myGrid_TimeSH[i + 1, 19].Value);
                                            TSHV1.Delete_NoteData(month, year);//Удаляем примечание для каждой конкретной записи
                                            if (NoteText != "" || Tsn_hours >= 0 || Tsn_days >= 0) TSHV1.Insert_NoteData(month, year, NoteText, (byte)Tsn_days, (decimal)Tsn_hours);
                                        }
                                    }
                                }
                                if (cl == 18 & First15days) First15days = false;
                                else
                                    if (cl == 18 & !First15days)
                                {
                                    myGrid_TimeSH[i - 1, 19].Value = Tsn_days.ToString();
                                    myGrid_TimeSH[i, 19].Value = String.Format("{0:0.##}", Tsn_hours);//часы
                                    First15days = true; Tsn_days = 0; Tsn_hours = 0;
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

        private void btn_SaveTimeSheet_Click(object sender, EventArgs e)
        {
            if (cB_Month.SelectedIndex == -1) MessageBox.Show("Не указан месяц.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                if (myGrid_TimeSH.Rows.Count == 0) MessageBox.Show("Нет данных для загрузки.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                //Версия 1
                //C_TimeSheetsV1 TSHV1 = new C_TimeSheetsV1(cB_Month.SelectedIndex + 1, (int)numUD_year.Value);
                int month = cB_Month.SelectedIndex + 1;
                int year = (int)numUD_year.Value;
                CheckOrInsert(true, month, year);
                if (TSHV1.Err) MessageBox.Show("Перед сохранением исправьте ошибки ввода данных.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    TSHV1.DeleteData(month, year, chB_Fired.Checked);
                    TSHV1.Delete_NoteDataBefore(month, year, chB_Fired.Checked);//НЕ ТРОГАТЬ!!! Это необходимо для очистки таблицы TimeSheetsNote в случае возникновения ошибок при записи в таблицу TimeSheets
                    CheckOrInsert(false, month, year);// - сюда входит ITSH.Delete_NoteData();

                    if (!TSHV1.Err) MessageBox.Show("Сохранено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //Версия 2
                /*C_TimeSheets TS = new C_TimeSheets(cB_Month.SelectedIndex + 1, (int)numUD_year.Value);
                TS.CeckData(ref myGrid_TimeSH);
                if (TS.Err) MessageBox.Show("Перед сохранением исправьте ошибки ввода данных.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    TS.DeleteData();
                    TS.InsertData(myGrid_TimeSH);
                    if (!TS.Err) MessageBox.Show("Сохранено.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }*/
            }
        }

        private void cB_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDataVer1();
            btn_SaveTimeSheet.Focus();
        }

        private void numUD_year_ValueChanged(object sender, EventArgs e)
        {
            LoadDataVer1();
            btn_SaveTimeSheet.Focus();
        }

        private void chB_Fired_CheckedChanged(object sender, EventArgs e)
        {
            LoadDataVer1();
        }

        private void LoadDataVer1()
        {
            try
            {
                if (cB_Month.SelectedIndex > -1) //MessageBox.Show("Не указан месяц.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //else
                {
                    int month = cB_Month.SelectedIndex + 1;
                    int year = (int)numUD_year.Value;
                    bool fired = chB_Fired.Checked;

                    TSHV1.Sp_ProductionCalendar(month, year, DT_Holidays);
                    
                    if (DT_Holidays.Rows.Count == 0) MessageBox.Show("Не заполнен производственный календарь.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    else
                    {
                        myGrid_TimeSH.Rows.Clear();
                        
                        //Если уже был сформирован табель то в него попадают только те рабочие которые значатся в табеле
                        //вне зависимости от того уволены они уже или нет
                        //Если табеля нет, то список рабочих формируется исходя из списка работающих на день формирования табеля

                        TSHV1.TimeSheetsWorkers(fired, month, year, _DT_Workers);

                        if (_DT_Workers.Rows.Count == 0)//Если нет табеля
                        {
                            TSHV1.Users_Sp_job(fired, month, year, _DT_Workers);
                            
                        }
                        if (_DT_Workers.Rows.Count > 0)
                        {
                            //Создаём грид
                            CreateGrid();
                            _DT_Workers.Clear(); _DT_Workers.Dispose();
                            DT_Holidays.Clear(); DT_Holidays.Dispose();
                            //Теперь заполняем грид своими данными
                            bool First15days = true;
                            DataTable DTtsh = new DataTable();
                            DataTable DT_note = new DataTable();
                            string Login = "";
                            DateTime NumDay;
                            for (int i = 2; i < myGrid_TimeSH.RowsCount; i++)
                            {
                                DTtsh.Clear();
                                Login = "";
                                if (myGrid_TimeSH[i, 22] != null)
                                    if (myGrid_TimeSH[i, 22].Value != null)
                                    {
                                        Login = myGrid_TimeSH[i, 22].Value.ToString();
                                        if (Login != "ИТР, специалисты и служащий персонал производства 50" &&
                                        Login != "Цех по изготовлению СТО")
                                        {
                                            if (First15days)
                                            {
                                                //Получаем примечание, дни и часы
                                                TSHV1.TimeSheetsNote(Login, month, year, DT_note);
                                            }

                                            TSHV1.TimeSheets(First15days, Login, month, year, DTtsh);
                                        }
                                    }
                                if (DTtsh.Rows.Count > 0)
                                {
                                    for (int rws = 0; rws < DTtsh.Rows.Count; rws++)
                                    {
                                        if (DT_note.Rows.Count > 0)
                                        {
                                            myGrid_TimeSH[i, 21].Value = DT_note.Rows[0].ItemArray[0].ToString();//Примечание
                                            myGrid_TimeSH[i, 19].Value = DT_note.Rows[0].ItemArray[1].ToString();//дни
                                            myGrid_TimeSH[i + 1, 19].Value = String.Format("{0:0.##}", DT_note.Rows[0].ItemArray[2]);//часы
                                            DT_note.Clear();
                                        }

                                        NumDay = (DateTime)DTtsh.Rows[rws].ItemArray[1];
                                        if (NumDay.Day < 17)
                                        {
                                            myGrid_TimeSH[i, NumDay.Day + 2].Value = DTtsh.Rows[rws].ItemArray[2].ToString();
                                        }
                                        else
                                        {
                                            myGrid_TimeSH[i, NumDay.Day - 14].Value = DTtsh.Rows[rws].ItemArray[2].ToString();
                                        }
                                        if (rws == DTtsh.Rows.Count - 1 & First15days) First15days = false;
                                        else
                                            if (rws == DTtsh.Rows.Count - 1 & !First15days) First15days = true;
                                    }
                                }
                            }
                            DTtsh.Clear(); DTtsh.Dispose();
                            DT_note.Clear(); DT_note.Dispose();
                            //MessageBox.Show(myGrid_TimeSH[1, 3].Value.ToString());//17 число
                            MessageBox.Show("Сформировано.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_PrintTURV_Click(object sender, EventArgs e)
        {
            if (cB_Month.SelectedIndex == -1) MessageBox.Show("Не указан месяц.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                if (myGrid_TimeSH.Rows.Count == 0) MessageBox.Show("Нет данных для загрузки.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {

                int month = cB_Month.SelectedIndex + 1;
                int year = (int)numUD_year.Value;
                TSHV1.Sp_ProductionCalendar(month, year, DT_Holidays);
                
                if (DT_Holidays.Rows.Count == 0) MessageBox.Show("Не заполнен производственный календарь.", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    int AllDays = 0;//Общее количество дней
                    decimal AllHours = 0;//Общее количество часов
                    foreach (DataRow dRow in DT_Holidays.Rows)
                    {
                        if (dRow.ItemArray[1].ToString() != "0")
                        {
                            AllDays++;
                            AllHours += Convert.ToDecimal(dRow.ItemArray[1]);
                        }
                    }
                    AllHours /= 3600;
                    DT_Holidays.Clear();
                    PrintTURV(AllDays, AllHours);
                }
            }

        }

        private void PrintTURV(int AllDays, decimal AllHours)
        {
            try//Вывод на печать
            {
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                //XlReferenceStyle RefStyle = Excel.ReferenceStyle; Excel.Visible = true;
                ExcelApp.SheetsInNewWorkbook = 1; //Кол-во книг
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
                ExcelWorkSheet.Application.Cells.Font.Size = 12;
                //((Excel.Range)ExcelWorkSheet.Columns[3]).Font.Size = 10;
                ExcelWorkSheet.Application.Cells.HorizontalAlignment = Excel.Constants.xlCenter;
                ExcelWorkSheet.Application.Cells.VerticalAlignment = Excel.Constants.xlCenter;
                //ExcelWorkSheet.Application.Cells.WrapText = true;
                //Редактирование созданного документа
                ((Excel.Range)ExcelWorkSheet.Columns[1]).ColumnWidth = 4;
                ((Excel.Range)ExcelWorkSheet.Columns[2]).ColumnWidth = 42;
                ((Excel.Range)ExcelWorkSheet.Columns[3]).ColumnWidth = 11;
                ((Excel.Range)ExcelWorkSheet.Columns[4]).ColumnWidth = 4;
                for (int i = 5; i < 21; i++)
                {
                    ((Excel.Range)ExcelWorkSheet.Columns[i]).ColumnWidth = 4;
                    //BorderColumn++;
                }
                ((Excel.Range)ExcelWorkSheet.Columns[21]).ColumnWidth = 7;
                ((Excel.Range)ExcelWorkSheet.Columns[22]).ColumnWidth = 5;
                ((Excel.Range)ExcelWorkSheet.Columns[23]).ColumnWidth = 17;//Примечание
                ((Excel.Range)ExcelWorkSheet.Columns[23]).WrapText = true;
                //Формат времени
                /*((Excel.Range)ExcelWorkSheet.Columns[8]).NumberFormat = "hh:mm:ss";
                ((Excel.Range)ExcelWorkSheet.Columns[18]).NumberFormat = "h:mm:ss";*/
                //Формат шапки
                //((Excel.Range)ExcelWorkSheet.get_Range("A1:R10")).NumberFormat = "";
                //**********************
                ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Value2 = "Предприятие  ОАО \"НИИПМ\"";
                ((Excel.Range)ExcelWorkSheet.Cells[1, 1]).Font.Size = 8;
                ((Excel.Range)ExcelWorkSheet.get_Range("A1:B1")).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).Value2 = "Подразделение: пр-во 50";
                ((Excel.Range)ExcelWorkSheet.Cells[2, 1]).Font.Size = 8;
                ((Excel.Range)ExcelWorkSheet.get_Range("A2:B2")).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Value2 = "ТАБЕЛЬ УЧЕТА РАБОЧЕГО ВРЕМЕНИ ЗА " + cB_Month.Text.ToUpper() + " " + numUD_year.Value.ToString() + " ГОДА";
                ((Excel.Range)ExcelWorkSheet.Cells[3, 1]).Font.Size = 9;
                ((Excel.Range)ExcelWorkSheet.get_Range("A3:W3")).Merge();
                //((Excel.Range)ExcelWorkSheet.Cells[3, 1]).HorizontalAlignment = Excel.Constants.xlCenter;
                ((Excel.Range)ExcelWorkSheet.get_Range("A1:A3")).Font.Bold = 1;
                //Рисуем шапку
                ((Excel.Range)ExcelWorkSheet.get_Range("A5:A6")).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[5, 1]).Value2 = "№.";
                ((Excel.Range)ExcelWorkSheet.Cells[7, 1]).Value2 = "п/п";
                ((Excel.Range)ExcelWorkSheet.get_Range("B5:B6")).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[5, 2]).Value2 = "Фамилия Имя Отчество";
                ((Excel.Range)ExcelWorkSheet.Cells[7, 2]).Value2 = "Занимаемая должность";
                ((Excel.Range)ExcelWorkSheet.get_Range("C5:C7")).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[5, 3]).Value2 = "Таб. номер";
                ((Excel.Range)ExcelWorkSheet.get_Range("D5:D6")).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[5, 4]).Value2 = "Ижд";
                ((Excel.Range)ExcelWorkSheet.Cells[7, 4]).Value2 = "ЧП";
                ((Excel.Range)ExcelWorkSheet.get_Range("E5:T5")).Merge();//Строка над днями
                for (int i = 1; i < 17; i++)
                {
                    ((Excel.Range)ExcelWorkSheet.Cells[6, i + 4]).Value2 = i;
                    if (i < 16) ((Excel.Range)ExcelWorkSheet.Cells[7, i + 4]).Value2 = i + 16;
                }
                ((Excel.Range)ExcelWorkSheet.get_Range("U5:U6")).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[5, 21]).Value2 = "дни";
                ((Excel.Range)ExcelWorkSheet.Cells[7, 21]).Value2 = "часы";
                ((Excel.Range)ExcelWorkSheet.get_Range("V5:V7")).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[5, 22]).Value2 = "Заказ";
                ((Excel.Range)ExcelWorkSheet.get_Range("W5:W7")).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[5, 23]).Value2 = "Примеч.";
                ((Excel.Range)ExcelWorkSheet.Cells[4, 23]).Value2 = AllDays + " день, " + AllHours + " час.";
                //((Excel.Range)ExcelWorkSheet.Rows[8]).Select();
                ((Excel.Range)ExcelWorkSheet.Cells[8, 1]).Select();
                ExcelApp.ActiveWindow.FreezePanes = true;
                //ExcelWorkSheet.PageSetup.PrintTitleRows = "10:10";
                //ExcelWorkSheet.Outline.SummaryRow = Excel.XlSummaryRow.xlSummaryAbove;//группировка строк
                //************************************
                #endregion Настройка шапки

                //ЗАГРУЗКА ДАННЫХ
                bool NumPP = false;//№ п/п
                bool NumTab = false;//Таб. номер
                bool Numord = false;//Заказ
                bool NumNote = false;//
                int flag = 0;
                for (int i = 2; i < myGrid_TimeSH.Rows.Count; i++)
                {
                    if (myGrid_TimeSH[i, 1] != null && myGrid_TimeSH[i, 1].Value != null)
                        if (myGrid_TimeSH[i, 1].Value.ToString() == "ИТР, специалисты и служащий персонал производства 50" |
                                                myGrid_TimeSH[i, 1].Value.ToString() == "Цех по изготовлению СТО")
                        {
                            ((Excel.Range)ExcelWorkSheet.get_Range("A" + (i + 6) + ":W" + (i + 6))).Merge();
                            ((Excel.Range)ExcelWorkSheet.Cells[i + 6, 1]).Value2 = myGrid_TimeSH[i, 1].Value;
                        }
                    //Пишем данные по сотрудникам
                    for (int col = 1; col < 23; col++)
                    {
                        if (myGrid_TimeSH[i, col - 1] != null && myGrid_TimeSH[i, col - 1].Value != null)
                        {
                            if (myGrid_TimeSH[i, 1] != null && myGrid_TimeSH[i, 1].Value != null)
                                if (myGrid_TimeSH[i, 1].Value.ToString() != "ИТР, специалисты и служащий персонал производства 50" &
                                                    myGrid_TimeSH[i, 1].Value.ToString() != "Цех по изготовлению СТО")
                                {
                                    if (col == 1 & NumPP == false)
                                    {
                                        ((Excel.Range)ExcelWorkSheet.get_Range("A" + (i + 6) + ":A" + (i + 7))).Merge();
                                        ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col]).Value2 = myGrid_TimeSH[i, col - 1].Value;
                                        NumPP = true;
                                    }
                                    else
                                        if (col == 1 & NumPP == true) NumPP = false;
                                    if (col == 3 & NumTab == false)
                                    {
                                        ((Excel.Range)ExcelWorkSheet.get_Range("C" + (i + 6) + ":C" + (i + 7))).Merge();
                                        ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col]).Value2 = myGrid_TimeSH[i, col - 1].Value;
                                        NumTab = true;
                                    }
                                    else
                                        if (col == 3 & NumTab == true) NumTab = false;
                                    if (col == 21 & Numord == false)
                                    {
                                        ((Excel.Range)ExcelWorkSheet.get_Range("V" + (i + 6) + ":V" + (i + 7))).Merge();
                                        ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col + 1]).Value2 = myGrid_TimeSH[i, col - 1].Value;

                                        Numord = true;
                                    }
                                    else
                                        if (col == 21 & Numord == true) Numord = false;
                                    if (col == 22 & NumNote == false)
                                    {
                                        ((Excel.Range)ExcelWorkSheet.get_Range("W" + (i + 6) + ":W" + (i + 7))).Merge();
                                        ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col + 1]).Value2 = myGrid_TimeSH[i, col - 1].Value;
                                        NumNote = true;
                                    }
                                    else
                                        if (col == 22 & NumNote == true) NumNote = false;

                                    // ФИО и занимаемая должность + стили к одному и второму
                                    if (col == 2)
                                    {
                                        ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col]).Value2 = myGrid_TimeSH[i, col - 1].Value;

                                        if (flag == 0)
                                        {
                                            ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col]).Font.Bold = true;
                                            ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col]).Font.Size = 12;
                                            flag = 1;
                                        }
                                        else
                                        {
                                            flag = 0;
                                            ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col]).Font.Size = 10;
                                            //((Excel.Range)ExcelWorkSheet.Cells[i + 6, col]).Font.Color = Color.Chocolate;
                                            ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col]).Font.Italic = true;
                                        }



                                    }



                                    if (col > 3 & col < 21)
                                    {
                                        if (myGrid_TimeSH[i, col - 1].View.BackColor == Color.PaleGreen)
                                        {
                                            ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col + 1]).Interior.Color = Color.PaleGreen;
                                        }

                                        ((Excel.Range)ExcelWorkSheet.Cells[i + 6, col + 1]).Value2 = myGrid_TimeSH[i, col - 1].Value;
                                    }
                                }

                        }
                        else
                        {
                            if (col == 1 & NumPP == false)
                            {
                                ((Excel.Range)ExcelWorkSheet.get_Range("A" + (i + 6) + ":A" + (i + 7))).Merge();
                                NumPP = true;
                            }
                            else
                                if (col == 1 & NumPP == true) NumPP = false;
                            if (col == 3 & NumTab == false)
                            {
                                ((Excel.Range)ExcelWorkSheet.get_Range("C" + (i + 6) + ":C" + (i + 7))).Merge();
                                NumTab = true;
                            }
                            else
                                if (col == 3 & NumTab == true) NumTab = false;
                            if (col == 21 & Numord == false)
                            {
                                ((Excel.Range)ExcelWorkSheet.get_Range("V" + (i + 6) + ":V" + (i + 7))).Merge();
                                Numord = true;
                            }
                            else
                                if (col == 21 & Numord == true) Numord = false;
                            if (col == 22 & NumNote == false)
                            {
                                ((Excel.Range)ExcelWorkSheet.get_Range("W" + (i + 6) + ":W" + (i + 7))).Merge();
                                NumNote = true;
                            }
                            else
                                if (col == 22 & NumNote == true) NumNote = false;
                        }
                    }//END Пишем данные по сотрудникам
                }
                int NumRow = myGrid_TimeSH.Rows.Count + 5;
                ((Excel.Range)ExcelWorkSheet.get_Range("A5", "W" + NumRow)).Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                NumRow += 2;
                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":B" + NumRow)).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = "Зам.генерального директора по производству";
                ((Excel.Range)ExcelWorkSheet.get_Range("C" + NumRow + ":E" + NumRow)).Merge();
                ((Excel.Range)ExcelWorkSheet.get_Range("C" + NumRow + ":E" + NumRow)).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)ExcelWorkSheet.get_Range("G" + NumRow + ":K" + NumRow)).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 7]).Value2 = "Пудовкин В.В.";
                ((Excel.Range)ExcelWorkSheet.get_Range("C" + (NumRow + 1) + ":E" + (NumRow + 1))).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, 3]).Value2 = "(подпись)";
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, 3]).Font.Italic = true;
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, 3]).Font.Size = 8;
                NumRow += 3;
                ((Excel.Range)ExcelWorkSheet.get_Range("A" + NumRow + ":B" + NumRow)).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 1]).Value2 = "Ответственное лицо";
                ((Excel.Range)ExcelWorkSheet.get_Range("C" + NumRow + ":E" + NumRow)).Merge();
                ((Excel.Range)ExcelWorkSheet.get_Range("C" + NumRow + ":E" + NumRow)).Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                ((Excel.Range)ExcelWorkSheet.get_Range("G" + NumRow + ":K" + NumRow)).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow, 7]).Value2 = "Кощеева О.В.";
                ((Excel.Range)ExcelWorkSheet.get_Range("C" + (NumRow + 1) + ":E" + (NumRow + 1))).Merge();
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, 3]).Value2 = "(подпись)";
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, 3]).Font.Italic = true;
                ((Excel.Range)ExcelWorkSheet.Cells[NumRow + 1, 3]).Font.Size = 8;
                //*********************************************************
                ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow - 3) + ":B" + NumRow)).HorizontalAlignment = Excel.Constants.xlRight;
                ((Excel.Range)ExcelWorkSheet.get_Range("A" + (NumRow - 3) + ":B" + NumRow)).Font.Bold = true;
                ((Excel.Range)ExcelWorkSheet.get_Range("G" + (NumRow - 3) + ":G" + NumRow)).HorizontalAlignment = Excel.Constants.xlLeft;
                ((Excel.Range)ExcelWorkSheet.get_Range("G" + (NumRow - 3) + ":G" + NumRow)).Font.Bold = true;

                MessageBox.Show("Сформировано.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }







    }
}
