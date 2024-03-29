using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dispetcher2.Class;
using System.Collections;
using System.Globalization;

namespace Dispetcher2.DataAccess
{
    public class SqlOperation : Operation
    {
        public void CalculateTime()
        {
            int t;
            //В настоящее время, 24.08.2023 технологическая операция в Лоцмане
            //хранится так, что "время на деталь" содержит сумму основного и предварительно-заключительного.
            //Причина: работа связки Лоцман-Вертикаль.
            //Можно считать так: t = Quantity * (Tsh - Tpd) + Tpd;
            //Однако для "старых" деталей этой суммы нет
            //Пока считаем "правильно"
            if (OnlyOncePay) t = Tsh + Tpd;
            else t = Quantity * Tsh + Tpd;
            TimeSpan ts = TimeSpan.FromSeconds(t);
            this.Time = ts;
        }
    }
    public class SqlOperationRepository : OperationRepository
    {
        IConfig config;
        IConverter converter;
        List<SqlOperation> operations;

        public SqlOperationRepository(IConfig config, IConverter converter)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            this.config = config;
            this.converter = converter;
        }
        public override IEnumerable GetList()
        {
            if (operations == null) Load();
            return operations;
        }
        public override Operation[] GetArray()
        {
            return operations.ToArray();
        }
        public override IEnumerable<Operation> GetOperations()
        {
            if (operations == null) Load();
            return operations;
        }
        public override void Load()
        {
            LoadAllOperationView();
        }

        void LoadAllOperationView()
        {
            operations = new List<SqlOperation>();
            using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "SELECT * FROM [dbo].[AllOperationView]";
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 120;

                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            SqlOperation item = new SqlOperation();

                            // Пропускаем OnlyOncePay, не имеет значения в отчете трудозатрат

                            if (converter.CheckConvert<long>(r["PK_IdOrderDetail"]))
                                item.OrderDetailId = converter.Convert<long>(r["PK_IdOrderDetail"]);

                            if (converter.CheckConvert<int>(r["FK_IdOperGroup"]))
                                item.GroupId = converter.Convert<int>(r["FK_IdOperGroup"]);

                            if (converter.CheckConvert<int>(r["Tpd"]))
                                item.Tpd = converter.Convert<int>(r["Tpd"]);

                            if (converter.CheckConvert<int>(r["Tsh"]))
                                item.Tsh = converter.Convert<int>(r["Tsh"]);

                            if (converter.CheckConvert<int>(r["AmountDetails"]))
                                item.Quantity = converter.Convert<int>(r["AmountDetails"]);

                            if (converter.CheckConvert<bool>(r["OnlyOncePay"]))
                                item.OnlyOncePay = converter.Convert<bool>(r["OnlyOncePay"]);

                            if (converter.CheckConvert<string>(r["NumOper"]))
                                item.Number = converter.Convert<string>(r["NumOper"]);

                            if (converter.CheckConvert<string>(r["NameOperation"]))
                                item.Name = converter.Convert<string>(r["NameOperation"]);

                            if (converter.CheckConvert<string>(r["TypeRow"]))
                                item.TypeRow = converter.Convert<string>(r["TypeRow"]);

                            if (converter.CheckConvert<string>(r["Login"]))
                                item.Login = converter.Convert<string>(r["Login"]);

                            if (converter.CheckConvert<DateTime>(r["FactDate"]))
                                item.FactDate = converter.Convert<DateTime>(r["FactDate"]);

                            if (converter.CheckConvert<int>(r["OrderId"]))
                                item.OrderId = converter.Convert<int>(r["OrderId"]);

                            item.CalculateTime();
                            operations.Add(item);

                        }
                    }
                }
            }
        }

        // Архивная версия, может быть понадобится
        //public void Call_rep_VEDOMOST_TRUDOZATRAT_NIIPM_UNITED(Detail d)
        //{
        //    string s;
        //    d.PlanOperations = new List<Operation>();
        //    using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
        //    {
        //        using (var cmd = new SqlCommand() { Connection = cn, CommandTimeout = 120 })
        //        {
        //            cmd.CommandText = "[dbo].[rep_VEDOMOST_TRUDOZATRAT_NIIPM_UNITED]";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@objects", Convert.ToString(d.IdLoodsman));
        //            s = $"Номер заказа=;Часть=1;Количество={d.Amount}";
        //            cmd.Parameters.AddWithValue("@params", s);
        //            cn.Open();
        //            using (var r = cmd.ExecuteReader())
        //            {
        //                while (r.Read())
        //                {
        //                    // выбираем только итоговые строки
        //                    s = Convert.ToString(r["Обозначение"]).Trim();
        //                    if (s.Length > 0) continue;
        //                    s = Convert.ToString(r["Наименование"]).Trim();
        //                    if (s.Length > 0) continue;
        //                    if (Converter.GetInt(r["numpozic"]) > 0) continue;
        //                    if (Converter.GetInt(r["vsego"]) > 0) continue;

        //                    var item = new Operation();
        //                    item.Name = Converter.GetString(r["marshrut"]);
        //                    item.Numcol = Converter.GetInt(r["numcol"]);
        //                    if (item.Numcol == 0) continue;
        //                    item.Time = Converter.GetTime(r["Время на одну операцию по деталям"]);
        //                    if (item.Time > TimeSpan.Zero) d.PlanOperations.Add(item);
        //                }
        //            }
        //        }
        //    }
        //}
        //public List<Operation> GetFactOperation(long OrderDetailId)
        //{
        //    List<Operation> result = new List<Operation>();
        //    using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
        //    {
        //        using (var cmd = new SqlCommand() { Connection = cn })
        //        {
        //            cmd.CommandText = "[dbo].[GetFactOperation]";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@OrderDetailId", OrderDetailId);
        //            cn.Open();
        //            using (var r = cmd.ExecuteReader())
        //            {
        //                while (r.Read())
        //                {
        //                    var item = new Operation();
        //                    item.GroupId = Converter.GetInt(r["FK_IdOperGroup"]);
        //                    item.Tpd = Converter.GetInt(r["Tpd"]);
        //                    item.Tsh = Converter.GetInt(r["Tsh"]);
        //                    item.Quantity = Converter.GetInt(r["fo_Amount"]);
        //                    item.Number = Converter.GetString(r["NumOper"]);
        //                    item.Name = Converter.GetString(r["NameOperation"]);
        //                    result.Add(item);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
    }
}
