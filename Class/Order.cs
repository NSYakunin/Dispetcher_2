using System;
using System.Collections.Generic;

namespace Dispetcher2.Class
{   
    public interface IOrder
    {
        int Id { get; set; }
        string Number { get; set; }
        string Name { get; set; }
        string Num1С { get; set; }
    }

    public abstract class OrderRepository
    {
        public abstract IEnumerable<IOrder> GetOrders();
    }

    public abstract class OrderFactory
    {
        public abstract OrderRepository GetOrderRepository(IConfig config);
    }
}
