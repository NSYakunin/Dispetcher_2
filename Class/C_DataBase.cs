using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Globalization;

namespace Dispetcher2.Class
{
    class C_DataBase
    {

        string _ConnectionString;

        public C_DataBase(string ConnectionString)
        {
            _ConnectionString = ConnectionString;
        }

        public static int GetInteger(object value)
        {
            if (value == null) return 0;
            if (value is DBNull) return 0;
            int number;
            bool f = Int32.TryParse(value.ToString(), System.Globalization.NumberStyles.Any,
                NumberFormatInfo.InvariantInfo, out number);
            if (f == false) return 0;
            return Convert.ToInt32(value);
        }

        public void Select_DT(ref DataTable DT,string SQLtext)
        {
            DT.Clear();
            try
            {
                using (C_Gper.con)
                {
                    C_Gper.con.ConnectionString = _ConnectionString;
                    SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//using System.Data.SqlClient;
                    cmd.CommandText = SQLtext;
                    cmd.Connection = C_Gper.con;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);//adapter.SelectCommand = cmd;
                    adapter.Fill(DT);
                    adapter.Dispose();
                    C_Gper.con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                cn.ConnectionString = C_Gper.ConnStrDispetcher2;
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
                cn.ConnectionString = C_Gper.ConStr_Loodsman;
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
                cn.ConnectionString = C_Gper.ConStr_Loodsman;
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
                cn.ConnectionString = C_Gper.ConnStrDispetcher2;
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
                cn.ConnectionString = C_Gper.ConnStrDispetcher2;
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
                cn.ConnectionString = C_Gper.ConnStrDispetcher2;
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
                cn.ConnectionString = C_Gper.ConnStrDispetcher2;
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
                cn.ConnectionString = C_Gper.ConnStrDispetcher2;
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
                cn.ConnectionString = C_Gper.ConnStrDispetcher2;
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
                cn.ConnectionString = C_Gper.ConnStrDispetcher2;
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

        public List<Order> GetOrderByStatus(int status)
        {
            List<Order> orderList = new List<Order>();
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = C_Gper.ConnStrDispetcher2;
                using (SqlCommand cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select * From Orders Where FK_IdStatusOrders = @S";
                    cmd.Parameters.AddWithValue("@S", status);
                    cn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while(r.Read())
                        {
                            Order item = new Order();
                            orderList.Add(item);
                            item.SetId(r["PK_IdOrder"]);
                            item.SetNumber(r["OrderNum"]);
                            item.SetName(r["OrderName"]);
                            item.SetCreateDate(r["DateCreateOrder"]);
                            item.SetStatus(r["FK_IdStatusOrders"]);
                            item.SetValidationOrder(r["ValidationOrder"]);
                            item.SetNum1С(r["OrderNum1С"]);
                            item.SetStartDate(r["StartDate"]);
                            item.SetPlannedDate(r["PlannedDate"]);
                            item.SetAmount(r["Amount"]);
                        }
                    }
                }
            }
            return orderList;
        }
        public List<Detail> GetOrderDetailAndFastener(int orderId)
        {
            List<Detail> detList = new List<Detail>();
            using (var cn = new SqlConnection() { ConnectionString = C_Gper.ConnStrDispetcher2 })
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
                            Detail item = new Detail();
                            detList.Add(item);
                            item.SetNameType(r["NameType"]);
                            item.SetPosition(r["Position"]);
                            item.SetShcm(r["ShcmDetail"]);
                            item.SetName(r["NameDetail"]);
                            item.SetAmount(r["AmountDetails"]);
                            item.SetAllPositionParent(r["AllPositionParent"]);
                            item.SetIdOrderDetail(r["PK_IdOrderDetail"]);
                            item.SetIdDetail(r["FK_IdDetail"]);
                            item.SetIdLoodsman(r["IdLoodsman"]);
                            item.SetPositionParent(r["PositionParent"]);
                        }
                    }
                }
            }
            return detList;
        }

        public void Call_rep_VEDOMOST_TRUDOZATRAT_NIIPM_UNITED(Detail d)
        {
            string s;
            d.Operations = new List<Operation>();
            using (var cn = new SqlConnection() { ConnectionString = C_Gper.ConStr_Loodsman })
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
                            if (GetInteger(r["numpozic"]) > 0) continue;
                            if (GetInteger(r["vsego"]) > 0) continue;

                            var item = new Operation();
                            item.SetName(r["marshrut"]);
                            item.Numcol = GetInteger(r["numcol"]);
                            item.SetTime(r["Время на одну операцию по деталям"]);
                            if (item.Time > TimeSpan.Zero) d.Operations.Add(item);
                        }
                    }
                }
            }
        }
    }
}
