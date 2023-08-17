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
                //m.Message = "SelectedOrder: null";
                MessageBox.Show("Пожалуйста выберите заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // m.Message = $"Id: {m.SelectedOrder.Id}";
            var dl = db.GetOrderDetailAndFastener(m.SelectedOrder.Id);
            if (dl != null)
            {
                //m.SelectedOrder.DetailList = dl;
                var dl2 = from d in dl
                          where d.PositionParent == 0
                          select d;
                m.SelectedOrder.DetailList = new List<Detail>();
                if (dl2.Any()) m.SelectedOrder.DetailList.AddRange(dl2);
                ShowOrderDetail(m.SelectedOrder);
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

        void ShowOrderDetail(Order z)
        {
            mainDataGrid.AutoGenerateColumns = false;
            DataGridTextColumn c;

            mainDataGrid.Columns.Clear();

            c = new DataGridTextColumn();
            c.Header = "Операция";
            c.MaxWidth = 200;
            c.MinWidth = 50;
            c.Binding = new Binding("Value[0]");
            mainDataGrid.Columns.Add(c);

            m.OperationList = new List<OperationDictionary>();
            OperationDictionary op1 = new OperationDictionary();
            op1.Value[0] = "Неизвестно";
            m.OperationList.Add(op1);

            for (int i = 0; i < z.DetailList.Count; i++)
            {
                var d = z.DetailList[i];
                c = new DataGridTextColumn();
                c.Header = d.ShcmAndName;
                c.MaxWidth = 200;
                c.MinWidth = 50;
                c.Binding = new Binding($"Value[{i+1}]");
                mainDataGrid.Columns.Add(c);
                op1.Value[i + 1] = TimeSpan.FromMinutes(i * 10 + 10);
            }

            mainDataGrid.ItemsSource = m.OperationList;
        }
    }


}
