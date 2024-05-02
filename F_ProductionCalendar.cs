using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Dispetcher2.Class;
using System.Data.SqlClient;

namespace Dispetcher2
{
    public partial class F_ProductionCalendar : Form
    {
        IConfig config;

        public F_ProductionCalendar(IConfig config)
        {
            this.config = config;
            InitializeComponent();
        }
        const int _CmdTimeout = 60; //seconds SqlCommand cmd = new SqlCommand() { CommandTimeout = CmdTimeout};
        DataTable DT_ProdCal = new DataTable();

        private void F_ProductionCalendar_Load(object sender, EventArgs e)
        {
            numUD_yearProdCal.Value = DateTime.Now.Year;
        }

        private void numUD_yearProdCal_ValueChanged(object sender, EventArgs e)
        {
            SelectProdCal(Convert.ToInt32(numUD_yearProdCal.Value), ref DT_ProdCal);
            dGV_ProdCal.Rows.Clear();
            AddMonthTodGV_ProdCal();
            if (DT_ProdCal.Rows.Count > 0)
            {
                btn_CreateProdCal.Visible = false;
                DateTime RowDateTime;
                int NumMonth = 0;
                int NumDay = 0;
                foreach (DataRow row in DT_ProdCal.Rows)
                {
                    RowDateTime = Convert.ToDateTime(row.ItemArray[0]);
                    NumMonth = RowDateTime.Month;
                    NumDay = RowDateTime.Day;
                    dGV_ProdCal.Rows[NumMonth-1].Cells[NumDay].Value = row.ItemArray[1];
                    if (RowDateTime.DayOfWeek.ToString() == "Saturday" || RowDateTime.DayOfWeek.ToString() == "Sunday")
                    {
                        dGV_ProdCal.Rows[NumMonth-1].Cells[NumDay].Style.BackColor = System.Drawing.Color.PaleGreen;
                    }
                }
            }
            else
            {
                btn_CreateProdCal.Visible = true;
            }
        }

        /*private string GetMonthName(int NumMonth)
        {
            switch (NumMonth)
            {
                case 1:
                    return "Январь";
                case 2:
                    return "Февраль";
                case 3:
                    return "Март";
                case 4:
                    return "Апрель";
                case 5:
                    return "Май";
                case 6:
                    return "Июнь";
                case 7:
                    return "Июль";
                case 8:
                    return "Август";
                case 9:
                    return "Сентябрь";
                case 10:
                    return "Октябрь";
                case 11:
                    return "Ноябрь";
                case 12:
                    return "Декабрь";
                default:
                    return "";
            }
        }*/

        private void AddMonthTodGV_ProdCal()
        {
            if (dGV_ProdCal.Rows.Count == 0)
            {
                dGV_ProdCal.Rows.Add("Январь");
                dGV_ProdCal.Rows.Add("Февраль");
                dGV_ProdCal.Rows.Add("Март");
                dGV_ProdCal.Rows.Add("Апрель");
                dGV_ProdCal.Rows.Add("Май");
                dGV_ProdCal.Rows.Add("Июнь");
                dGV_ProdCal.Rows.Add("Июль");
                dGV_ProdCal.Rows.Add("Август");
                dGV_ProdCal.Rows.Add("Сентябрь");
                dGV_ProdCal.Rows.Add("Октябрь");
                dGV_ProdCal.Rows.Add("Ноябрь");
                dGV_ProdCal.Rows.Add("Декабрь");
            }
        }

        private void btn_CreateProdCal_Click(object sender, EventArgs e)
        {
            DateTime DateT = Convert.ToDateTime("01.01." + numUD_yearProdCal.Value.ToString());
            while (DateT<Convert.ToDateTime("01.01." + (numUD_yearProdCal.Value+1).ToString()))
            {
                //if DateT

                if (DateT.DayOfWeek.ToString() == "Saturday" || DateT.DayOfWeek.ToString() == "Sunday")
                {
                    dGV_ProdCal.Rows[DateT.Month - 1].Cells[DateT.Day].Value = 0;
                    dGV_ProdCal.Rows[DateT.Month - 1].Cells[DateT.Day].Style.BackColor = System.Drawing.Color.PaleGreen;
                }
                else
                {
                    dGV_ProdCal.Rows[DateT.Month - 1].Cells[DateT.Day].Value = 8 * 60;
                }
                DateT = DateT.AddDays(1);
            }
        }

        private void SelectProdCal(int year, ref DataTable DT)
        {
            try
            {
                DT.Clear();
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = _CmdTimeout };//using System.Data.SqlClient;
                    cmd.CommandText = "SELECT PK_Date, Dsec/60 as Dsec" + "\n" +
                                      "FROM Sp_ProductionCalendar" + "\n" +
                                      "Where Year(PK_Date)=@year" + "\n" +
                                      "order by PK_Date";
                    //params
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.Int));
                    cmd.Parameters["@year"].Value = year;
                    cmd.Connection = con;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_InsertProdCal_Click(object sender, EventArgs e)
        {
            //Delete and Insert
            DeleteProdCal(Convert.ToInt32(numUD_yearProdCal.Value));
            bool err = false;
            DateTime PK_Date;
            for (int i = 0; i < dGV_ProdCal.Rows.Count; i++)
            {
                for (int d = 1; d < dGV_ProdCal.ColumnCount; d++)
                {
                    if (dGV_ProdCal.Rows[i].Cells[d].Value != null)
                    {
                        PK_Date = Convert.ToDateTime(d.ToString() + "." + (i + 1).ToString() + "." + numUD_yearProdCal.Value.ToString());
                        if (!InsertProdCal(PK_Date, Convert.ToInt32(dGV_ProdCal.Rows[i].Cells[d].Value)*60))
                            err = true;
                    }
                }
            }
            if (!err) MessageBox.Show("Данные производственного календаря успешно сохранены.", "Успех!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteProdCal(int Del_year)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                    cmd.CommandText = "delete from Sp_ProductionCalendar where Year(PK_Date)=@Del_year";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@Del_year", SqlDbType.Int));
                    cmd.Parameters["@Del_year"].Value = Del_year;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool InsertProdCal(DateTime PK_Date, int Dsec)
        {
            try
            {
                using (var con = new SqlConnection())
                {
                    con.ConnectionString = config.ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                    cmd.CommandText = "insert into Sp_ProductionCalendar (PK_Date,Dsec) " + "\n" +
                                  "values (@PK_Date,@Dsec)";
                    cmd.Connection = con;
                    //Parameters**************************************************
                    cmd.Parameters.Add(new SqlParameter("@PK_Date", SqlDbType.Date));
                    cmd.Parameters["@PK_Date"].Value = PK_Date;
                    cmd.Parameters.Add(new SqlParameter("@Dsec", SqlDbType.Int));
                    cmd.Parameters["@Dsec"].Value = Dsec;
                    //***********************************************************
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

    }
}
