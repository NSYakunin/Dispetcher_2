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
        IConfig config;
        IConverter converter;

        List<WorkDay> dayList;

        public SqlWorkDayRepository(IConfig config, IConverter converter)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр: config");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            this.config = config;
            this.converter = converter;

            
        }

        // Архив
        /*
        List<WorkDay> GetProductionCalendar(DateTime beginDate, DateTime endDate)
        {
            List<WorkDay> result = new List<WorkDay>();
            using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
            {
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
                            if (converter.CheckConvert<DateTime>(r["PK_Date"]))
                                d.Date = converter.Convert<DateTime>(r["PK_Date"]);

                            if (converter.CheckConvert<double>(r["Dsec"]))
                            {
                                double x = converter.Convert<double>(r["Dsec"]);
                                d.Time = TimeSpan.FromSeconds(x);
                            }
                            
                            result.Add(d);
                        }
                        
                    }
                }
            }
            return result;
        }*/
        List<WorkDay> GetTimeSheet(DateTime beginDate, DateTime endDate)
        {
            List<WorkDay> result = new List<WorkDay>();
            using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "[dbo].[GetPeriodTimeSheet]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BeginDate", beginDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            WorkDay d = new WorkDay();

                            if (converter.CheckConvert<string>(r["PK_Login"]))
                                d.Login = converter.Convert<string>(r["PK_Login"]);

                            if (converter.CheckConvert<string>(r["LastName"]))
                                d.LastName = converter.Convert<string>(r["LastName"]);

                            if (converter.CheckConvert<string>(r["Name"]))
                                d.Name = converter.Convert<string>(r["Name"]);

                            if (converter.CheckConvert<string>(r["SecondName"]))
                                d.SecondName = converter.Convert<string>(r["SecondName"]);

                            if (converter.CheckConvert<string>(r["NameJob"]))
                                d.JobName = converter.Convert<string>(r["NameJob"]);

                            if (converter.CheckConvert<int>(r["OperationGroupId"]))
                                d.OperationGroupId = converter.Convert<int>(r["OperationGroupId"]);

                            if (converter.CheckConvert<string>(r["TabNum"]))
                                d.TabNum = converter.Convert<string>(r["TabNum"]);

                            if (converter.CheckConvert<bool>(r["ITR"]))
                                d.ITR = converter.Convert<bool>(r["ITR"]);

                            if (converter.CheckConvert<DateTime>(r["PK_Date"]))
                                d.Date = converter.Convert<DateTime>(r["PK_Date"]);

                            if (converter.CheckConvert<string>(r["Val_Time"]))
                                d.TimeString = converter.Convert<string>(r["Val_Time"]);

                            if (converter.CheckConvert<double>(r["Val_TimeFloat"]))
                            {
                                var t = converter.Convert<double>(r["Val_TimeFloat"]);
                                d.Time = TimeSpan.FromHours(t);
                            }

                            result.Add(d);
                        }
                    }
                }
            }
            return result;
        }

        public override IEnumerable<WorkDay> GetWorkDays(DateTime beginDate, DateTime endDate)
        {
            return GetTimeSheet(beginDate, endDate);
        }
    }
}
