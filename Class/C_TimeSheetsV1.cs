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
        IConfig config;

        private string _LoginUs = "";
        private string _Val_Time = "";
        private DateTime _PK_Date;
        private string[] _ValCell = {"Б","В","Г","ДО","ОЖ","ОТ","ПР","Р"};
        decimal _ValCell_d;
        bool _Err = false;

        public C_TimeSheetsV1(IConfig config)
        {
            this.config = config;


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
            {
                _Err = true;
                return false;
            }
            else return true;
        }

        void CheckDate(int month, int year)
        {
            if (month < 1) _Err = true;
            if (month > 12) _Err = true;
            if (year < 1) _Err = true;
        }

        public void InsertData()
        {
            try
            {
                if (!_Err)
                {
                using (var con = new SqlConnection())
                {
                        //if (_Val_Time == "") _Val_Time = "_";
                        con.ConnectionString = config.ConnectionString;
                        SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                        cmd.CommandText = "insert into TimeSheets (FK_Login,PK_Date,Val_Time,Val_TimeFloat) " + "\n" +
                                        "values (@FK_Login,@PK_Date,@Val_Time,@Val_TimeFloat)";
                        cmd.Connection = con;
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

        public void Insert_NoteData(int month, int year, string Note, byte Tsn_days, decimal Tsn_hours)
        {
            try
            {
                CheckDate(month, year);
                if (!_Err)
                {
                using (var con = new SqlConnection())
                {
                        con.ConnectionString = config.ConnectionString;
                        SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                        cmd.CommandText = "insert into TimeSheetsNote (FK_Login,Tsn_month,Tsn_year,Note,Tsn_days,Tsn_hours) " + "\n" +
                                        "values (@FK_Login,@Tsn_month,@Tsn_year,@Note,@Tsn_days,@Tsn_hours)";
                        cmd.Connection = con;
                        //Parameters**************************************************
                        cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                        cmd.Parameters["@FK_Login"].Value = _LoginUs;
                        cmd.Parameters.Add(new SqlParameter("@Tsn_month", SqlDbType.Int));
                        cmd.Parameters["@Tsn_month"].Value = month;
                        cmd.Parameters.Add(new SqlParameter("@Tsn_year", SqlDbType.Int));
                        cmd.Parameters["@Tsn_year"].Value = year;
                        cmd.Parameters.Add(new SqlParameter("@Note", SqlDbType.VarChar));
                        cmd.Parameters["@Note"].Value = Note;
                        cmd.Parameters.Add(new SqlParameter("@Tsn_days", SqlDbType.TinyInt));
                        cmd.Parameters["@Tsn_days"].Value = Tsn_days;
                        cmd.Parameters.Add(new SqlParameter("@Tsn_hours", SqlDbType.Decimal));
                        cmd.Parameters["@Tsn_hours"].Value = Tsn_hours;
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

        public void DeleteData(int month, int year, bool Fired)
        {
            try
            {
                CheckDate(month, year);
                string where = "";
                if (!Fired) where = " and (u.DateEnd is null or (Year(u.DateEnd)<=" + year + " and MONTH(u.DateEnd)<>" + month + "))";
                else where = " and MONTH(u.DateEnd)=" + month + " and Year(u.DateEnd)=" + year;
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    //**************************************************************************************************************************
                    // Удалил +where в запросе ниже, что бы в табеле не было ошибок при сохрании часов с уволенными сотрудниками
                    cmd.CommandText = "DELETE FROM TimeSheets" + "\n" +
                        "FROM TimeSheets" + "\n" +
                        "INNER JOIN Users AS u ON u.PK_Login = TimeSheets.FK_Login" + "\n" +
                        "Where (MONTH(TimeSheets.PK_Date) = @MONTH) AND (YEAR(TimeSheets.PK_Date) = @Year)";
                    cmd.Connection = con;

                    //**************************************************************************************************************************

                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                    cmd.Parameters["@Year"].Value = year;
                    cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.Int));
                    cmd.Parameters["@MONTH"].Value = month;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Delete_NoteDataBefore(int month, int year, bool Fired)
        {
            try
            {
                CheckDate(month, year);
                string where = "";
                if (!Fired) where = " and u.DateEnd is null";
                    else where = " and MONTH(u.DateEnd)=" + month + " and Year(u.DateEnd)=" + year;
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "DELETE FROM TimeSheetsNote" + "\n" +
                        "FROM TimeSheetsNote" + "\n" +
                        "INNER JOIN Users AS u ON u.PK_Login = TimeSheetsNote.FK_Login" + "\n" +
                        "Where (TimeSheetsNote.Tsn_month = @MONTH) AND (TimeSheetsNote.Tsn_year = @Year)" + where;
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                    cmd.Parameters["@Year"].Value = year;
                    cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                    cmd.Parameters["@MONTH"].Value = month;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Delete_NoteData(int month, int year)//Удаляем конкретную запись а потому всё ОК
        {
            try
            {
                CheckDate(month, year);
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "delete from TimeSheetsNote Where FK_Login = @FK_Login and Tsn_month=@Month and Tsn_year=@Year";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                    cmd.Parameters["@FK_Login"].Value = _LoginUs;
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                    cmd.Parameters["@Year"].Value = year;
                    cmd.Parameters.Add(new SqlParameter("@Month", SqlDbType.Int));
                    cmd.Parameters["@MONTH"].Value = month;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void DeleteDataLogin(int month, int year)//Удаляем значения в таблице TimeSheets для 1 конкретного пользователя
        {
            try
            {
                CheckDate(month, year);
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "DELETE FROM TimeSheets" + "\n" +
                        "FROM TimeSheets" + "\n" +
                        "INNER JOIN Users AS u ON u.PK_Login = TimeSheets.FK_Login" + "\n" +
                        "Where FK_Login = @FK_Login and (MONTH(TimeSheets.PK_Date) = @MONTH) AND (YEAR(TimeSheets.PK_Date) = @Year)";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@FK_Login", SqlDbType.VarChar));
                    cmd.Parameters["@FK_Login"].Value = _LoginUs;
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                    cmd.Parameters["@Year"].Value = year;
                    cmd.Parameters.Add(new SqlParameter("@MONTH", SqlDbType.Int));
                    cmd.Parameters["@MONTH"].Value = month;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void Select(string sql, DataTable DT)
        {
            try
            {
                DT.Clear();
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//using System.Data.SqlClient;
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);//adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Sp_ProductionCalendar(int month, int year, DataTable DT)
        {
            
            string sql = "SELECT Day(PK_Date) as cDay, Dsec FROM Sp_ProductionCalendar" + "\n" +
                "Where Year(PK_Date)=" + year + " and MONTH(PK_Date)=" + month + "\n" +
                "order by PK_Date";
            Select(sql, DT);
        }

        public void TimeSheetsWorkers(bool fired, int month, int year, DataTable DT)
        {
            string where, sql;

            if (fired)
            {
                where = " and MONTH(u.DateEnd)=" + month + " and Year(u.DateEnd)=" + year + "and MONTH(u.DateStart) >" + (month - 1) + "))";
            }
            else
            {
                where = $" and (u.DateEnd is null or (Year(u.DateEnd) >= {year} and MONTH(u.DateEnd) > {(month - 1)}))" +
                $" and u.DateStart < '{year}-{month + 1}-17'";
            }
            
            sql = "Select Distinct(PK_Login) as PK_Login,(LastName+' '+Name+' '+ SecondName) as FullName,NameJob,TabNum,ITR" + "\n" +
                  "From TimeSheets as ts" + "\n" +
                   "Inner join Users as u On u.PK_Login = ts.FK_Login" + "\n" +
                   "LEFT join Sp_job as j On j.Pk_IdJob = u.FK_IdJob" + "\n" +
                   "Where TabNum is not Null" + where + "\n" +
                   "Order by ITR desc,FullName";

            Select(sql, DT);
        }

        public void Users_Sp_job(bool fired, int month, int year, DataTable DT)
        {
            string where, sql;

            if (fired) where = " and MONTH(u.DateEnd)=" + month + " and Year(u.DateEnd)=" + year;
            else where = " and u.DateEnd is null";

            //Загружаем рабочих
            sql = "Select PK_Login,(LastName+' '+Name+' '+ SecondName) as FullName,NameJob,TabNum,ITR" + "\n" +
                "From Users as u" + "\n" +
                "LEFT join Sp_job as j On j.Pk_IdJob = u.FK_IdJob" + "\n" +
                "Where OnlyUser = 0 and ShowTimeSheets = 1 " + where + "\n" +
                "Order by ITR desc,FullName";

            Select(sql, DT);
        }

        public void TimeSheetsNote(string Login, int month, int year, DataTable DT)
        {
            string sql;
            
            sql = "SELECT Note,Tsn_days,Tsn_hours FROM TimeSheetsNote Where FK_Login = '" + Login + 
                "' and Tsn_month = " + month + " and Tsn_year = " + year;
            Select(sql, DT);
        }

        public void TimeSheets(bool First15days, string Login, int month, int year, DataTable DT)
        {
            string where, sql;
            if (First15days) where = "DAY(PK_Date) < 17";
            else where = "DAY(PK_Date) > 16";

            sql = "SELECT FK_Login,PK_Date,Val_Time FROM TimeSheets" + "\n" +
                        "Where FK_Login = '" + Login + "' and Year(PK_Date)=" + year + " and MONTH(PK_Date)=" + month + " and " + where + "\n" +
                        //"Where FK_Login = '" + Login + "' and Year(PK_Date)=" + (int)numUD_year.Value + " and MONTH(PK_Date)=" + _MONTH  + "\n" +
                        "Order by PK_Date";

            Select(sql, DT);
        }
    }
}
