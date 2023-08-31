using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Dispetcher2.Class;

namespace Dispetcher2.DataAccess
{
    public class SqlWorkDayRepository : WorkDayRepository
    {
        string connectionString;
        DateTime point;
        List<WorkDay> dayList;
        TimeSpan total;
        TimeSpan past;
        public SqlWorkDayRepository(string connectionString, DateTime point)
        {
            this.connectionString = connectionString;
            this.point = point;
            Load();
        }

        void Load()
        {
            int year = point.Year;
            DateTime begin = new DateTime(year, 1, 1);
            DateTime end = new DateTime(year + 1, 1, 1);
            
            dayList = GetProductionCalendar(begin, end);

            // Нужно вычислить:
            // общее рабочее время в году
            total = TimeSpan.Zero;
            foreach (var wd in dayList) total = total.Add(wd.Time);
            DateTime d = point.Date;
            // прошедшее рабочее время в году
            var e = from wd in dayList
                    where wd.Date < d
                    select wd;
            past = TimeSpan.Zero;
            foreach (var wd in e) past = past.Add(wd.Time);
        }

        public List<WorkDay> GetProductionCalendar(DateTime beginDate, DateTime endDate)
        {
            List<WorkDay> result = new List<WorkDay>();
            using (var cn = new SqlConnection())
            {
                cn.ConnectionString = connectionString;
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "[dbo].[GetProductionCalendar]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BeginDate", beginDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while(r.Read())
                        {
                            WorkDay d = new WorkDay();
                            d.Date = Convert.ToDateTime(r["PK_Date"]);
                            var s = r["Dsec"];
                            if (s is DBNull) d.Time = TimeSpan.Zero;
                            else d.Time = TimeSpan.FromSeconds(Convert.ToDouble(s));
                            result.Add(d);
                        }
                        
                    }
                }
            }
            return result;
        }

        public override IEnumerable<WorkDay> GetWorkDays()
        {
            return dayList;
        }

        public override TimeSpan GetTotalTime()
        {
            return total;
        }

        public override TimeSpan GetPastTime()
        {
            return past;
        }
    }
}
