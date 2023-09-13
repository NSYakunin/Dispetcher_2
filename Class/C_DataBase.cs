using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

using Dispetcher2.DataAccess;

namespace Dispetcher2.Class
{
    class C_DataBase
    {

        IConfig config;

        public C_DataBase(IConfig config)
        {
            this.config = config;
        }



        /// <summary>
        /// Получение списка сборок заказа
        /// </summary>
        /// <param name="orderId"></param>
        /// Идентификатор заказа
        /// <returns></returns>
        public DataTable GetOrderDetail(int orderId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "[dbo].[GetOrderDetail]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    using (SqlDataAdapter ad = new SqlDataAdapter())
                    {
                        ad.SelectCommand = cmd;
                        ad.Fill(dt);
                    }
                }
            }
            return dt;
        }

        // Получение комплектации сборки из БД Лоцман
        public DataTable GetDetailKit(string position, string shcmDetail, string amountDetails,
            string orderNum, long idLoodsman, bool standard)
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.LoodsmanConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "[NIIPM].[GetDetailKit]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Position", position);
                    cmd.Parameters.AddWithValue("@ShcmDetail", shcmDetail);
                    cmd.Parameters.AddWithValue("@AmountDetails", amountDetails);
                    cmd.Parameters.AddWithValue("@OrderNum", orderNum);
                    cmd.Parameters.AddWithValue("@IdLoodsman", idLoodsman);

                    SqlParameter sp = new SqlParameter();
                    sp.ParameterName = "@Standard";
                    sp.SqlDbType = SqlDbType.Bit;
                    sp.Value = standard;
                    cmd.Parameters.Add(sp);

                    using (SqlDataAdapter ad = new SqlDataAdapter())
                    {
                        ad.SelectCommand = cmd;
                        ad.Fill(dt);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Получение полного списка комплектации из БД Лоцман
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllKits()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.LoodsmanConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "[NIIPM].[GetAllKit]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter ad = new SqlDataAdapter())
                    {
                        ad.SelectCommand = cmd;
                        ad.Fill(dt);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Получение всех сборок из БД Диспетчер
        /// </summary>
        /// <returns></returns>
        public DataTable GetDetail()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "[dbo].[GetDetail]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter ad = new SqlDataAdapter())
                    {
                        ad.SelectCommand = cmd;
                        ad.Fill(dt);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Удаление комплектации для сборки
        /// </summary>
        /// <param name="idLoodsman">Идентификатор сборки в Лоцмане</param>
        public void DeleteKit(long idLoodsman)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "[dbo].[DeleteKit]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdLoodsman", idLoodsman);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Вставка одной детали комплектации
        public void InsertKit(long idLoodsman, int idKit, string nameProduct, double minquantity,
            int idTypeKit, int idstate)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "[dbo].[InsertKit]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PK_IdKit", idKit);
                    cmd.Parameters.AddWithValue("@NameProduct", nameProduct);
                    cmd.Parameters.AddWithValue("@minquantity", minquantity);
                    cmd.Parameters.AddWithValue("@FK_IdTypeKit", idTypeKit);
                    cmd.Parameters.AddWithValue("@idstate", idstate);
                    cmd.Parameters.AddWithValue("@IdLoodsman", idLoodsman);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Сохранение записи в таблицу Sp_Kit_1C
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Наименование</param>
        public void SetSpKit1C(long id, string name)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "[dbo].[SetSpKit1C]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PK_1С_IdKit", id);
                    cmd.Parameters.AddWithValue("@Name1CKit", name);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }    
        }

        /// <summary>
        /// Удаление "Лимитки" из таблицы RelationsKit
        /// </summary>
        /// <param name="year">Год</param>
        /// <param name="num">Номер</param>
        public void DeleteRelationsKit(int year, string num)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "[dbo].[DeleteRelationsKit]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Num", num);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Вставка записи в таблицу RelationsKit
        public void InsertRelationsKit(int year, string num, int position, int FK_IdOrder,
            int IdLoodsman, long FK_1С_IdKit, DateTime DateLimit, double AmountKit)
        {
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "[dbo].[InsertRelationsKit]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@YearLimit", year);
                    cmd.Parameters.AddWithValue("@NumLimit", num);
                    cmd.Parameters.AddWithValue("@Position", position);
                    cmd.Parameters.AddWithValue("@FK_IdOrder", FK_IdOrder);
                    cmd.Parameters.AddWithValue("@IdLoodsman", IdLoodsman);
                    cmd.Parameters.AddWithValue("@FK_1С_IdKit", FK_1С_IdKit);
                    cmd.Parameters.AddWithValue("@DateLimit", DateLimit);
                    cmd.Parameters.AddWithValue("@AmountKit", AmountKit);
                    cmd.Parameters.AddWithValue("@DateIn", DateTime.Now);
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetAllOrders()
        {
            DataTable dt = new DataTable();
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "Select * From Orders";
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataAdapter ad = new SqlDataAdapter())
                    {
                        ad.SelectCommand = cmd;
                        ad.Fill(dt);
                    }
                }
            }
            int c = dt.Columns.IndexOf("OrderNum1С");
            foreach (DataRow r in dt.Rows)
                if (r[c] is string) continue;
                else r[c] = String.Empty;
            return dt;
        }

        
        public List<Detail> GetOrderDetailAndFastener(int orderId)
        {
            List<Detail> detList = new List<Detail>();
            using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "[dbo].[GetOrderDetailAndFastener]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            Detail item = new SqlDetail();
                            detList.Add(item);
                            item.NameType = Converter.GetString(r["NameType"]);
                            item.Position = Converter.GetInt(r["Position"]);
                            item.Shcm = Converter.GetString(r["ShcmDetail"]);
                            item.Name = Converter.GetString(r["NameDetail"]);
                            item.Amount = Converter.GetInt(r["AmountDetails"]);
                            item.AllPositionParent = Converter.GetString(r["AllPositionParent"]);
                            item.OrderDetailId = Converter.GetLong(r["PK_IdOrderDetail"]);
                            item.IdDetail = Converter.GetLong(r["FK_IdDetail"]);
                            item.IdLoodsman = Converter.GetLong(r["IdLoodsman"]);
                            item.PositionParent = Converter.GetInt(r["PositionParent"]);
                        }
                    }
                }
            }
            return detList;
        }

        public void Call_rep_VEDOMOST_TRUDOZATRAT_NIIPM_UNITED(Detail d)
        {
            string s;
            d.PlanOperations = new List<Operation>();
            using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn, CommandTimeout = 120 })
                {
                    cmd.CommandText = "[dbo].[rep_VEDOMOST_TRUDOZATRAT_NIIPM_UNITED]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@objects", Convert.ToString(d.IdLoodsman));
                    s = $"Номер заказа=;Часть=1;Количество={d.Amount}";
                    cmd.Parameters.AddWithValue("@params", s);
                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            // выбираем только итоговые строки
                            s = Convert.ToString(r["Обозначение"]).Trim();
                            if (s.Length > 0) continue;
                            s = Convert.ToString(r["Наименование"]).Trim();
                            if (s.Length > 0) continue;
                            if (Converter.GetInt(r["numpozic"]) > 0) continue;
                            if (Converter.GetInt(r["vsego"]) > 0) continue;

                            var item = new Operation();
                            item.Name = Converter.GetString(r["marshrut"]);
                            item.Numcol = Converter.GetInt(r["numcol"]);
                            if (item.Numcol == 0) continue;
                            item.Time = Converter.GetTime(r["Время на одну операцию по деталям"]);
                            if (item.Time > TimeSpan.Zero) d.PlanOperations.Add(item);
                        }
                    }
                }
            }
        }

        public List<Operation> GetFactOperation(long OrderDetailId)
        {
            List<Operation> result = new List<Operation>();
            using (var cn = new SqlConnection() { ConnectionString = config.ConnectionString })
            {
                using (var cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandText = "[dbo].[GetFactOperation]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderDetailId", OrderDetailId);
                    cn.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var item = new Operation();
                            item.GroupId = Converter.GetInt(r["FK_IdOperGroup"]);
                            item.Tpd = Converter.GetInt(r["Tpd"]);
                            item.Tsh = Converter.GetInt(r["Tsh"]);
                            item.Quantity = Converter.GetInt(r["fo_Amount"]);
                            item.Number = Converter.GetString(r["NumOper"]);
                            item.Name = Converter.GetString(r["NameOperation"]);
                            result.Add(item);
                        }
                    }
                }
            }
            return result;
        }


    }
}
