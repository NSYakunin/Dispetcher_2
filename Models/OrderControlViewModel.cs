﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Dispetcher2.Class;
using System.Collections;
using System.Windows.Input;

namespace Dispetcher2.Models
{
    public class OrderView : Order
    {
        public bool Checked { get; set; }
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
    public class OrderViewRepository : OrderRepository
    {
        IEnumerable<Order> orderIterator;
        public OrderViewRepository(IEnumerable<Order> orderIterator)
        {
            this.orderIterator = orderIterator;
        }
        public override IEnumerable GetList()
        {
            return orderIterator;
        }
        public override IEnumerable<Order> GetOrders()
        {
            return orderIterator;
        }
        public override void Load() { }
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

        public ICommand SelectAllCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }

        ObservableCollection<OrderView> orderlist = new ObservableCollection<OrderView>();

        OrderRepository rep;
        List<OrderView> allOrders;

        public OrderControlViewModel(OrderRepository rep)
        {
            if (rep == null) throw new ArgumentException("Нужно предоставить хранилище заказов");
            this.rep = rep;
                        
            SelectAllCommand = new LaborCommand()
            {
                ExecuteAction = this.ProcessSelectAllCommand
            };

            ClearAllCommand = new LaborCommand()
            {
                ExecuteAction = this.ProcessClearAllCommand
            };
        }

        public ObservableCollection<OrderView> OrderList
        {
            get { return orderlist; }
        }

        void FilterData()
        {
            OrderList.Clear();
            if (Filter == null) return;

            if (allOrders == null)
            {
                var a = rep.GetOrders();
                allOrders = new List<OrderView>();
                foreach (var x in a) allOrders.Add(OrderView.GetOrderView(x));
            }

            string num = Filter.Trim();

            var l2 = from x in allOrders
                     where x.Number.Contains(num) || x.Checked == true
                     select x;

            if (l2.Any())
            {
                foreach (var x in l2) OrderList.Add(x);
            }
        }

        public OrderRepository SelectedOrders
        {
            get
            {
                var a = new List<Order>();
                var e = allOrders.Where(o => o.Checked == true);
                if (e.Any()) a.AddRange(e);
                if (OrderList.Count == 1 && a.Count < 1)
                {
                    OrderView v = OrderList[0];
                    a.Add(v);
                }
                OrderViewRepository r = new OrderViewRepository(a);
                return r;
            }
        }
        void ProcessClearAllCommand()
        {
            foreach (var item in allOrders) item.Checked = false;
            FilterData();
        }
        void ProcessSelectAllCommand()
        {
            foreach (var item in allOrders) item.Checked = true;
            FilterData();
        }
    }

}
