using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace Dispetcher2.Class
{
    sealed class C_TimeSheetsV1
    {
        private string _LoginUs = "";
            private string _Val_Time = "";
            private DateTime _PK_Date;
            private string[] _ValCell = {"Б","В","Г","ДО","ОЖ","ОТ","ПР","Р"};
            decimal _ValCell_d;
            bool _Err = false;
            int _MONTH, _YEAR;

            public C_TimeSheetsV1(int MONTH, int YEAR)
            {
                if (MONTH>0 & MONTH<13) _MONTH = MONTH; else _Err = true;
                if (YEAR > 0) _YEAR = YEAR; else _Err = true;
            }

            public string LoginUs 
            { 
                get {return _LoginUs;}
                set {
                    if (value.Length>0)
                    _LoginUs = value;
                    else _LoginUs = "";
                }
            }

            public string Val_Time
            {
                get { return _Val_Time; }
                set {
                    _Val_Time = ""; _ValCell_d = 0;
                    foreach (string vl in _ValCell)
                    {
                        if (value == vl)//проверка на соответствие буквам массива
                        {
                            _Val_Time = value;
                            break;
                        }
                    }
                    //проверка на соответствие цифре
                    if (_Val_Time == "" && decimal.TryParse(value, C_Gper.style, C_Gper.culture, out _ValCell_d) & _ValCell_d <= 24)
                    {
                        _ValCell_d = Math.Round(_ValCell_d, 2, MidpointRounding.AwayFromZero);
                        _Val_Time = _ValCell_d.ToString(C_Gper.culture);
                        /*if (_ValCell_d.ToString().IndexOf(".") > 0)
                        {
                            string[] temp = _ValCell_d.ToString().Split('.');
                            if (temp[1].Length == 1) _Val_Time = _ValCell_d.ToString();//temp[0] - число до запятой, temp[1] - число после запятой
                        }*/
                    }
                }
            }

            public DateTime PK_Date
            {
                get { return _PK_Date; }
                set {
                   // _PK_Date=Convert.ToDateTime("01.01.0001");
                    DateTime.TryParse(value.ToString(), out _PK_Date);
                }
            }

            public bool Err
            {
                get { return _Err; }
            }

            public bool CheckData()
            {
                if (_LoginUs == "" || _PK_Date.Year == 1)
                //if (_LoginUs == "" || _Val_Time == "" || _PK_Date.Year == 1)
                {
                    _Err = true;
                    return false;
                }
                else return true;
            }

            public void InsertData()
            {
                try
                {
                    if (!_Err)
                    {
                        using (C_Gper.con)
                        {
                            //if (_Val_Time == "") _Val_Time = "_";
                            C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                            SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                            cmd.CommandText = "insert into TimeSheets (FK_Login,PK_Date,Val_Time,Val_TimeFloat) " + "\n" +
                                          "values (@FK_Login,@PK_Date,@Val_Time,@Val_TimeFloat)";
                            cmd.Connection = C_Gper.con;
                            //Parameters**************************************************
                            cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                            cmd.Parameters["@FK_Login"].Value = _LoginUs;
                            cmd.Parameters.Add(new SqlParameter("@PK_Date", SqlDbType.Date));
                            cmd.Parameters["@PK_Date"].Value = _PK_Date;
                            cmd.Parameters.Add(new SqlParameter("@Val_Time", SqlDbType.VarChar));
                            cmd.Parameters["@Val_Time"].Value = _Val_Time;
                            decimal VT = 0;
                            decimal.TryParse(_Val_Time, C_Gper.style, C_Gper.culture, out VT);
                            cmd.Parameters.Add(new SqlParameter("@Val_TimeFloat", SqlDbType.Float));
                            cmd.Parameters["@Val_TimeFloat"].Value = VT;
                            //***********************************************************
                            C_Gper.con.Open();
                            cmd.ExecuteNonQuery();
                            C_Gper.con.Close();
                        }
                    }   
                }
                catch (Exception ex)
                {
                    _Err = true;
                    MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public void Insert_NoteData(string Note, byte Tsn_days, decimal Tsn_hours)
            {
                try
                {
                    if (!_Err)
                    {
                        using (C_Gper.con)
                        {
                            C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                            SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                            cmd.CommandText = "insert into TimeSheetsNote (FK_Login,Tsn_month,Tsn_year,Note,Tsn_days,Tsn_hours) " + "\n" +
                                          "values (@FK_Login,@Tsn_month,@Tsn_year,@Note,@Tsn_days,@Tsn_hours)";
                            cmd.Connection = C_Gper.con;
                            //Parameters**************************************************
                            cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                            cmd.Parameters["@FK_Login"].Value = _LoginUs;
                            cmd.Parameters.Add(new SqlParameter("@Tsn_month", SqlDbType.Int));
                            cmd.Parameters["@Tsn_month"].Value = _MONTH;
                            cmd.Parameters.Add(new SqlParameter("@Tsn_year", SqlDbType.Int));
                            cmd.Parameters["@Tsn_year"].Value = _YEAR;
                            cmd.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar));
                            cmd.Parameters["@Note"].Value = Note;
                            cmd.Parameters.Add(new SqlParameter("@Tsn_days", SqlDbType.TinyInt));
                            cmd.Parameters["@Tsn_days"].Value = Tsn_days;
                            cmd.Parameters.Add(new SqlParameter("@Tsn_hours", SqlDbType.Decimal));
                            cmd.Parameters["@Tsn_hours"].Value = Tsn_hours;
                            //***********************************************************
                            C_Gper.con.Open();
                            cmd.ExecuteNonQuery();
                            C_Gper.con.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _Err = true;
                    MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public void DeleteData(bool Fired)
            {
                try
                {
                    string where = "";
                    if (!Fired) where = " and (u.DateEnd is null or (Year(u.DateEnd)<=" + _YEAR + " and MONTH(u.DateEnd)<>" + _MONTH + "))";
                    else where = " and MONTH(u.DateEnd)=" + _MONTH + " and Year(u.DateEnd)=" + _YEAR;
                    using (C_Gper.con)
                    {
                    C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    //**************************************************************************************************************************
                    // Удалил +where в запросе ниже, что бы в табеле не было ошибок при сохрании часов с уволенными сотрудниками
                    cmd.CommandText = "DELETE FROM TimeSheets" + "\n" +
                        "FROM TimeSheets" + "\n" +
                        "INNER JOIN Users AS u ON u.PK_Login = TimeSheets.FK_Login" + "\n" +
                        "Where (MONTH(TimeSheets.PK_Date) = @MONTH) AND (YEAR(TimeSheets.PK_Date) = @Year)";
                    cmd.Connection = C_Gper.con;

                    //**************************************************************************************************************************

                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                    cmd.Parameters["@Year"].Value = _YEAR;
                    cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.Int));
                    cmd.Parameters["@MONTH"].Value = _MONTH;
                    //***********************************************************
                    C_Gper.con.Open();
                    cmd.ExecuteNonQuery();
                    C_Gper.con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public void Delete_NoteDataBefore(bool Fired)
            {
                try
                {
                    string where = "";
                    if (!Fired) where = " and u.DateEnd is null";
                    else where = " and MONTH(u.DateEnd)=" + _MONTH + " and Year(u.DateEnd)=" + _YEAR;
                    using (C_Gper.con)
                    {
                        C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                        SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                        cmd.CommandText = "DELETE FROM TimeSheetsNote" + "\n" +
                            "FROM TimeSheetsNote" + "\n" +
                            "INNER JOIN Users AS u ON u.PK_Login = TimeSheetsNote.FK_Login" + "\n" +
                            "Where (TimeSheetsNote.Tsn_month = @MONTH) AND (TimeSheetsNote.Tsn_year = @Year)" + where;
                        cmd.Connection = C_Gper.con;
                        //Parameters**************************************************
                        cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                        cmd.Parameters["@Year"].Value = _YEAR;
                        cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                        cmd.Parameters["@MONTH"].Value = _MONTH;
                        //***********************************************************
                        C_Gper.con.Open();
                        cmd.ExecuteNonQuery();
                        C_Gper.con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public void Delete_NoteData()//Удаляем конкретную запись а потому всё ОК
            {
                try
                {
                    using (C_Gper.con)
                    {
                        C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                        SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                        cmd.CommandText = "delete from TimeSheetsNote Where FK_Login = @FK_Login and Tsn_month=@Month and Tsn_year=@Year";
                        cmd.Connection = C_Gper.con;
                        //Parameters**************************************************
                        cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                        cmd.Parameters["@FK_Login"].Value = _LoginUs;
                        cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                        cmd.Parameters["@Year"].Value = _YEAR;
                        cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                        cmd.Parameters["@MONTH"].Value = _MONTH;
                        //***********************************************************
                        C_Gper.con.Open();
                        cmd.ExecuteNonQuery();
                        C_Gper.con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public void DeleteDataLogin()//Удаляем значения в таблице TimeSheets для 1 конкретного пользователя
            {
                try
                {
                    using (C_Gper.con)
                    {
                        C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                        SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                        cmd.CommandText = "DELETE FROM TimeSheets" + "\n" +
                            "FROM TimeSheets" + "\n" +
                            "INNER JOIN Users AS u ON u.PK_Login = TimeSheets.FK_Login" + "\n" +
                            "Where FK_Login = @FK_Login and (MONTH(TimeSheets.PK_Date) = @MONTH) AND (YEAR(TimeSheets.PK_Date) = @Year)";
                        cmd.Connection = C_Gper.con;
                        //Parameters**************************************************
                        cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                        cmd.Parameters["@FK_Login"].Value = _LoginUs;
                        cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                        cmd.Parameters["@Year"].Value = _YEAR;
                        cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.Int));
                        cmd.Parameters["@MONTH"].Value = _MONTH;
                        //***********************************************************
                        C_Gper.con.Open();
                        cmd.ExecuteNonQuery();
                        C_Gper.con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
    }
}
