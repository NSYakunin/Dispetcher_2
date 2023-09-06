using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Dispetcher2.Class;
using Dispetcher2.Models;

namespace Dispetcher2.Controls
{
    /// <summary>
    /// Логика взаимодействия для OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        OrderRepository rep;
        OrderRepositoryViewModel model;
        List<OrderView> orderList = new List<OrderView>();

        public OrderControl(OrderRepository rep, OrderRepositoryViewModel model)
        {
            if (rep == null) throw new ArgumentException("Нужно предоставить хранилище заказов");
            this.rep = rep;
            this.model = model;
            var a = rep.GetOrders();
            foreach (var x in a) orderList.Add(OrderView.GetOrderView(x));
            InitializeComponent();
            model.Filter = String.Empty;
            this.DataContext = model;
            FilterData();
        }

        void FilterData()
        {
            string num = model.Filter.Trim();

            var l2 = from x in orderList
                     where x.Number.Contains(num)
                     select x;

            if (l2.Any())
            {
                model.OrderList.Clear();
                foreach (var x in l2) model.OrderList.Add(x);
            }
            else
            {
                model.OrderList.Clear();
            }

        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterData();
        }
    }
}
