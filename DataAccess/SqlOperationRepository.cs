using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Dispetcher2.Class;
using System.Collections;

namespace Dispetcher2.DataAccess
{
    public class SqlOperation : Operation
    {
        public void CalculateTime()
        {
            //В настоящее время, 24.08.2023 технологическая операция в Лоцмане
            //хранится так, что "время на деталь" содержит сумму основного и предварительно-заключительного.
            //Причина: работа связки Лоцман-Вертикаль.
            //Трудоемкость следует считать так:
            //Т = К * (Tsh - Tpz) + Tpz

            int t;
            if (Tsh > Tpd) t = Quantity * (Tsh - Tpd) + Tpd;
            else t = Quantity * Tsh + Tpd;
            TimeSpan ts = TimeSpan.FromSeconds(t);
            this.Time = ts;
        }
    }
    public class SqlOperationRepository : OperationRepository
    {
        IConfig config;
        List<SqlOperation> operations;
        public SqlOperationRepository(IConfig config)
        {
            this.config = config;
        }
        public override IEnumerable GetList()
        {
            if (operations == null) Load();
            return operations;
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

                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            SqlOperation item = new SqlOperation();
                            
                            item.OrderDetailId = Converter.GetLong(r["PK_IdOrderDetail"]);
                            item.GroupId = Converter.GetInt(r["FK_IdOperGroup"]);
                            item.Tpd = Converter.GetInt(r["Tpd"]);
                            item.Tsh = Converter.GetInt(r["Tsh"]);
                            item.Quantity = Converter.GetInt(r["fo_Amount"]);
                            // Пропускаем OnlyOncePay, не имеет значения в отчете трудозатрат
                            item.Number = Converter.GetString(r["NumOper"]);
                            item.Name = Converter.GetString(r["NameOperation"]);
                            item.TypeRow = Converter.GetString(r["TypeRow"]);
                            if (item.Tpd > 0 || item.Tsh > 0)
                            {
                                item.CalculateTime();
                                operations.Add(item);
                            }
                            
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
