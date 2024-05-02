using System;
using System.Collections.Generic;

namespace Dispetcher2.Class
{   
    public abstract class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Num1С { get; set; }
    }

    public abstract class OrderRepository : Repository
    {
        public abstract IEnumerable<Order> GetOrders();
    }
}
