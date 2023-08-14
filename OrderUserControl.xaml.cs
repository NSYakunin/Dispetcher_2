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

namespace Dispetcher2
{
    /// <summary>
    /// Логика взаимодействия для OrderUserControl.xaml
    /// </summary>
    public partial class OrderUserControl : UserControl
    {
        C_DataBase db;
        List<Order> orderList;
        OrderControlModel m = new OrderControlModel();

        public OrderUserControl()
        {
            try
            {
                InitializeComponent();
                this.DataContext = m;
                m.Filter = String.Empty;
                db = new C_DataBase(C_Gper.ConnStrDispetcher2);
                orderList = db.GetOrderByStatus(2); // 2 — opened
                FilterData();
            }
            catch(Exception ex)
            {
                m.Message = ex.Message;
            }
        }

        private void OnRequest(object sender, RoutedEventArgs e)
        {
            if (m.SelectedOrder == null)
            {
                m.Message = "SelectedOrder: null";
            }
            else
            {
                m.Message = $"Id: {m.SelectedOrder.Id}";
            }
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterData();
        }
    }
}
