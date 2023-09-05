using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Dispetcher2.Class;

namespace Dispetcher2.DataAccess
{
    public class SqlOrderRepository : OrderRepository
    {
        string connectionString = null;
        Nullable<int> status = null;
        public SqlOrderRepository(string connectionString)
        {
            this.connectionString = connectionString;
            this.status = null;
        }
        public SqlOrderRepository(string connectionString, int status)
        {
            this.connectionString = connectionString;
            this.status = status;
        }

        public override IEnumerable<Order> GetOrders()
        {
            return GetOrderByStatus();
        }

        List<Order> GetOrderByStatus()
        {
            List<Order> orderList = new List<Order>();
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = connectionString;
                using (SqlCommand cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "GetOrders";
                    object s = DBNull.Value;
                    if (status.HasValue) s = status.Value;
                    cmd.Parameters.AddWithValue("@Status", s);
                    cn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            Order item = new Order();
                            orderList.Add(item);
                            item.Id = Converter.GetInt(r["PK_IdOrder"]);
                            item.Number = Converter.GetString(r["OrderNum"]);
                            item.Name = Converter.GetString(r["OrderName"]);
                            item.CreateDate = Converter.GetDateTime(r["DateCreateOrder"]);
                            item.Status = Converter.GetInt(r["FK_IdStatusOrders"]);
                            item.ValidationOrder = Converter.GetBool(r["ValidationOrder"]);
                            item.Num1С = Converter.GetString(r["OrderNum1С"]);
                            item.StartDate = Converter.GetDateTime(r["StartDate"]);
                            item.PlannedDate = Converter.GetDateTime(r["PlannedDate"]);
                            item.Amount = Converter.GetInt(r["Amount"]);
                        }
                    }
                }
            }
            return orderList;
        }
    }
}
