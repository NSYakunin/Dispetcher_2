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

    public abstract class OrderRepository
    {
        public abstract IEnumerable<Order> GetOrders();
        public abstract void Load();
    }

    public abstract class OrderFactory
    {
        public abstract OrderRepository GetOrderRepository(IConfig config);
    }
}
