using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;

namespace Dispetcher2.Class
{
    [Obsolete("В ПО используется C_TimeSheetsV1.cs, эта версия сделана как альтернатива для проведения тестов")]
    sealed class C_TimeSheets
    {
        IConfig config;
        IConverter converter;
        int _St_Month;
        int _St_Year;
        bool _Err = false;
        private string _Val_Time = "";//Значение ячейки
        private string[] _ValCell = { "Б", "В", "Г", "ДО", "ОЖ", "ОТ", "ПР", "Р" };

        public C_TimeSheets(IConfig config, IConverter converter, int St_Month, int St_Year)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            _St_Month = St_Month;
            _St_Year = St_Year;
            this.config = config;
            this.converter = converter;
            // Эта культура записывает числа через точку: 4.5 = черыре с половиной
            converter.ContextCulture = CultureInfo.InvariantCulture;
        }

        public bool Err
        {
            get { return _Err; }
        }
        
        private string Val_Time
        {
            set
            {
                double _ValCell_d = 0;
                _Val_Time = "";
                foreach (string vl in _ValCell)
                {
                    if (value == vl)//проверка на соответствие буквам массива
                    {
                        _Val_Time = value;
                        break;
                    }
                }
                //проверка на соответствие цифре
                //if (_Val_Time == "" & double.TryParse(value, out _ValCell_d) & _ValCell_d <= 24)
                if (_Val_Time == "" & converter.CheckConvert<double>(value))
                {
                    _ValCell_d = converter.Convert<double>(value);
                    if (_ValCell_d <= 24)
                    {
                        _ValCell_d = Math.Round(_ValCell_d, 2, MidpointRounding.AwayFromZero);
                        //_Val_Time = _ValCell_d.ToString(C_Gper.culture);

                        _Val_Time = converter.Convert<string>(_ValCell_d);

                        /*if (_ValCell_d.ToString().IndexOf(",") > 0)
                        {
                            string[] temp = _ValCell_d.ToString().Split(',');
                            if (temp[1].Length < 2) _Val_Time = _ValCell_d.ToString();
                        }
                        else
                            _Val_Time = _ValCell_d.ToString();*/
                    }
                }
            }
        }

        public void CeckData(ref Controls.MyGrid.MyGrid MyGrid)
        {
            try
            {
                if (MyGrid.RowsCount > 0)
                {
                    bool First15days = true;
                    DateTime dt_date;
                    string st_date;
                    string LoginUs = "";
                    for (int i = 2; i < MyGrid.RowsCount; i++)
                    {
                        if (MyGrid[i, 22] != null)
                            if (MyGrid[i, 22].Value != null)
                                LoginUs = MyGrid[i, 22].Value.ToString();
                        if (LoginUs != "ИТР, специалисты и служащий персонал производства 50" && LoginUs != "Цех по изготовлению СТО")
                            for (int cl = 3; cl < 19; cl++)
                            {
                                if (MyGrid[i, cl] != null && MyGrid[i, cl].Value != null)
                                    {
                                        if (First15days) st_date = (cl - 2).ToString();
                                        else st_date = (cl + 14).ToString();
                                        st_date += "." + _St_Month.ToString() + "." + _St_Year.ToString();
                                        DateTime.TryParse(st_date, out dt_date);
                                        Val_Time = MyGrid[i, cl].Value.ToString();
                                        if (LoginUs == "" || _Val_Time == "" || dt_date.Year == 1)
                                        {
                                            MyGrid[i, cl].View.BackColor = System.Drawing.Color.LightPink;
                                            MyGrid.Refresh();
                                            _Err = true;
                                        }
                                        else
                                        {
                                            if (MyGrid[i, cl].View.BackColor == System.Drawing.Color.LightPink)
                                            {
                                                MyGrid[i, cl].View.BackColor = SystemColors.InactiveBorder;
                                                MyGrid.Refresh();
                                            }
                                        }

                                    }
                                if (cl == 18 & First15days) First15days = false;
                                else
                                    if (cl == 18 & !First15days) First15days = true;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteData()
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "delete from TimeSheets2 Where Ts_month=@MONTH and Ts_year=@Year";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.TinyInt));
                    cmd.Parameters["@MONTH"].Value = _St_Month;
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                    cmd.Parameters["@Year"].Value = _St_Year;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                _Err = true;
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void InsertData(Controls.MyGrid.MyGrid MyGrid)
        {
            try
            {
                if (MyGrid.RowsCount > 0)
                {
                    bool First15days = true;
                    string LoginUs = "";
                    string AllValue = "";//Значение всех дней через до 15 числа
                    string AllValue2 = "";//Значение всех дней с 16 и далее
                    for (int i = 2; i < MyGrid.RowsCount; i++)
                    {
                        if (First15days & LoginUs != "" & AllValue.Length > 0 & AllValue2.Length > 0)
                        {
                            Insert(LoginUs, AllValue, AllValue2);
                            AllValue = ""; AllValue2 = "";
                        }
                        if (MyGrid[i, 22] != null && MyGrid[i, 22].Value != null)
                            LoginUs = MyGrid[i, 22].Value.ToString();
                        if (LoginUs != "ИТР, специалисты и служащий персонал производства 50" && LoginUs != "Цех по изготовлению СТО")
                            for (int cl = 3; cl < 19; cl++)
                            {
                                if (First15days & MyGrid[i, cl] != null)
                                {
                                    if (MyGrid[i, cl].Value != null)
                                        AllValue += MyGrid[i, cl].Value.ToString() + ";";
                                    else
                                        AllValue += 0 + ";";
                                }
                                if (!First15days & MyGrid[i, cl] != null)
                                {
                                    if (MyGrid[i, cl].Value != null)
                                        AllValue2 += MyGrid[i, cl].Value.ToString() + ";";
                                    else
                                        AllValue2 += 0 + ";";
                                
                                }
                                if (cl == 18 & First15days) First15days = false;
                                else
                                    if (cl == 18 & !First15days) First15days = true;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                _Err = true;
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Insert(string LoginUs, string AllValue, string AllValue2)
        {
            try
            {
                if (!_Err)
                {
                    using (var con = new SqlConnection())
                    {
                        con.ConnectionString = config.ConnectionString;
                        SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                        cmd.CommandText = "insert into TimeSheets2 (FK_Login,Ts_month,Ts_year,Val_Time,Val_Time2,Note) " + "\n" +
                                      "values (@FK_Login,@Ts_month,@Ts_year,@Val_Time,@Val_Time2,@Note)";
                        cmd.Connection = con;
                        //Parameters**************************************************
                        cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                        cmd.Parameters["@FK_Login"].Value = LoginUs;
                        cmd.Parameters.Add(new SqlParameter("@Ts_month", SqlDbType.TinyInt));
                        cmd.Parameters["@Ts_month"].Value = _St_Month;
                        cmd.Parameters.Add(new SqlParameter("@Ts_year", SqlDbType.SmallInt));
                        cmd.Parameters["@Ts_year"].Value = _St_Year;
                        cmd.Parameters.Add(new SqlParameter("@Val_Time", SqlDbType.VarChar));
                        cmd.Parameters["@Val_Time"].Value = AllValue;
                        cmd.Parameters.Add(new SqlParameter("@Val_Time2", SqlDbType.VarChar));
                        cmd.Parameters["@Val_Time2"].Value = AllValue2;
                        cmd.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar));
                        cmd.Parameters["@Note"].Value = "none";
                        //***********************************************************
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                _Err = true;
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
