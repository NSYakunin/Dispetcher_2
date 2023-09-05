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
        
        public OrderControl(OrderRepository rep)
        {
            if (rep == null) throw new ArgumentException("Нужно предоставить хранилище заказов");
            this.rep = rep;
            InitializeComponent();
        }

        void FilterData()
        {
            string num = m.Filter.Trim();

            var l2 = from x in orderList
                     where x.Number.Contains(num)
                     select x;

            //m.ListBindingSource.DataSource = l2;
            if (l2.Any())
            {
                m.OrderList.Clear();
                foreach (var x in l2) m.OrderList.Add(x);
            }
            else
            {
                m.OrderList.Clear();
            }

        }
    }
}
