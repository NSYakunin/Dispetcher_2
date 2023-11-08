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
        IConverter converter;

        // <param name="_IdStatusOrders">1-ожидание,2-открыт,3-закрыт,4-в работе,5-выполнен</param>
        public SqlOrderRepository(IConfig config, IConverter converter, Nullable<int> status = null)
        {
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр config");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            this.config = config;
            this.converter = converter;
            this.status = status;
        }

        public override IEnumerable GetList()
        {
            if (orders == null) Load();
            return orders;
        }
        public override IEnumerable<Order> GetOrders()
        {
            if (orders == null) Load();
            return orders;
        }

        public override void Load()
        {
            try
            {
                GetOrderByStatus();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("SqlOrderRepository.Load: " + ex.Message);
            }
            
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
                            
                            if (converter.CheckConvert<int>(r["PK_IdOrder"])) 
                                item.Id = converter.Convert<int>(r["PK_IdOrder"]);

                            if (converter.CheckConvert<string>(r["OrderNum"]))
                                item.Number = converter.Convert<string>(r["OrderNum"]);

                            if (converter.CheckConvert<string>(r["OrderName"]))
                                item.Name = converter.Convert<string>(r["OrderName"]);

                            if (converter.CheckConvert<DateTime>(r["DateCreateOrder"]))
                                item.CreateDate = converter.Convert<DateTime>(r["DateCreateOrder"]);

                            if (converter.CheckConvert<int>(r["FK_IdStatusOrders"]))
                                item.Status = converter.Convert<int>(r["FK_IdStatusOrders"]);

                            if (converter.CheckConvert<bool>(r["ValidationOrder"]))
                                item.ValidationOrder = converter.Convert<bool>(r["ValidationOrder"]);

                            if (converter.CheckConvert<string>(r["OrderNum1С"]))
                                item.Num1С = converter.Convert<string>(r["OrderNum1С"]);

                            if (converter.CheckConvert<DateTime>(r["PlannedDate"]))
                                item.PlannedDate = converter.Convert<DateTime>(r["PlannedDate"]);
                            
                            if (converter.CheckConvert<int>(r["Amount"]))
                                item.Amount = converter.Convert<int>(r["Amount"]);
                            orders.Add(item);
                        }
                    }
                }
            }
        }
    }
}
