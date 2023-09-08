using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Input;

using Dispetcher2.Class;

namespace Dispetcher2.Models
{
    public class OrderView : Order
    {

        public static OrderView GetOrderView(Order order)
        {
            var v = new OrderView();
            v.Id = order.Id;
            v.Number = order.Number;
            v.Name = order.Name;
            v.Num1С = order.Num1С;
            return v;
        }
    }
    public class OrderControlViewModel
    {
        string filterValue;
        public string Filter
        {
            get { return filterValue; }
            set
            {
                filterValue = value;
                FilterData();
            }
        }
        public Order SelectedOrder { set; get; }

        ObservableCollection<OrderView> orderlist = new ObservableCollection<OrderView>();

        List<OrderView> allOrders = new List<OrderView>();

        public OrderControlViewModel(OrderRepository rep)
        {
            if (rep == null) throw new ArgumentException("Нужно предоставить хранилище заказов");
            var a = rep.GetOrders();
            foreach (var x in a) allOrders.Add(OrderView.GetOrderView(x));
            Filter = String.Empty;
        }

        public ObservableCollection<OrderView> OrderList
        {
            get { return orderlist; }
        }

        void FilterData()
        {
            string num = Filter.Trim();

            var l2 = from x in allOrders
                     where x.Number.Contains(num)
                     select x;

            OrderList.Clear();
            if (l2.Any())
            {
                foreach (var x in l2) OrderList.Add(x);
            }
        }
    }

}
