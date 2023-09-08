using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using Dispetcher2.Class;

namespace Dispetcher2.DataAccess
{
    public class SqlOrder : Order
    {
        
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public bool ValidationOrder { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlannedDate { get; set; }
        public int Amount { get; set; }
    }

    public class SqlOrderRepository : OrderRepository
    {
        IConfig config;
        Nullable<int> status = null;
        List<SqlOrder> orders = null;
        public SqlOrderRepository(IConfig config)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            this.config = config;
            this.status = null;
        }
        public SqlOrderRepository(IConfig config, int status)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            this.config = config;
            this.status = status;
        }

        public override IEnumerable<Order> GetOrders()
        {
            if (orders == null) Load();
            return orders;
        }

        public override void Load()
        {
            GetOrderByStatus();
        }

        void GetOrderByStatus()
        {
            orders = new List<SqlOrder>();
            using (SqlConnection cn = new SqlConnection())
            {
                cn.ConnectionString = config.ConnectionString;
                using (SqlCommand cmd = new SqlCommand() { Connection = cn })
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "GetOrders";
                    object s = DBNull.Value;
                    if (status.HasValue) s = status.Value;
                    cmd.Parameters.AddWithValue("@Status", s);
                    cn.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            var item = new SqlOrder();
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
                            orders.Add(item);
                        }
                    }
                }
            }
        }
    }

    public class MainOrderFactory : OrderFactory
    {
        // тут конечно надо будет сделать перечисление
        // не сделал сразу, потому что надо искать волшебные числа по всему проекту...
        // 1-ожидание,2-открыт,3-закрыт,4-в работе,5-выполнен
        private const int status = 2;
        OrderType type;
        public MainOrderFactory(OrderType type)
        {
            this.type = type;
        }

        public override OrderRepository GetOrderRepository(IConfig config)
        {
            if (type == OrderType.SQL) return new SqlOrderRepository(config, status);

            throw new NotImplementedException("В разработке...");
        }
    }
}
