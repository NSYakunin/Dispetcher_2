using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Dispetcher2.Class
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public bool ValidationOrder { get; set; }
        public string Num1С { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlannedDate { get; set; }
        public int Amount { get; set; }
        public DetailRepository MainDetails { get; set; }
    }

    public abstract class OrderRepository
    {
        public abstract IEnumerable<Order> GetOrders();
    }
}
