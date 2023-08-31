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
using Dispetcher2.DataAccess;

namespace Dispetcher2.Controls
{
    /// <summary>
    /// Логика взаимодействия для OrderUserControl.xaml
    /// </summary>
    public partial class OrderUserControl : UserControl
    {
        C_DataBase db;
        List<Order> orderList;
        OrderControlModel m = new OrderControlModel();
        LaborLoader loader = null;
        WorkTimeControl wtc;

        public OrderUserControl()
        {
            try
            {
                InitializeComponent();
                mainDataGrid.Visibility = Visibility.Collapsed;
                SqlWorkDayRepository swdr = new SqlWorkDayRepository(C_Gper.ConnStrDispetcher2, DateTime.Now);
                wtc = new WorkTimeControl(swdr);
                workTimePlace.Children.Add(wtc);
                wtc.Visibility = Visibility.Collapsed;

                LoadingWait.Message = "Загрузка...";
                LoadingWait.Stop();
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
            if (m.OrderList.Count == 1) m.SelectedOrder = m.OrderList[0];
            if (m.SelectedOrder == null)
            {
                MessageBox.Show("Пожалуйста выберите заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BeforeLoad();
            loader = new LaborLoader(m.SelectedOrder);
            loader.Finished += Ll_Finished;
            loader.Start();

        }

        private void Ll_Finished()
        {
            Action a = AfterLoad;
            this.Dispatcher.BeginInvoke(a);
            loader = null;
        }

        void BeforeLoad()
        {
            filterTextBox.IsEnabled = false;
            orderListBox.IsEnabled = false;
            requestButton.IsEnabled = false;
            mainDataGrid.ItemsSource = null;
            mainDataGrid.Visibility = Visibility.Collapsed;
            wtc.Visibility = Visibility.Collapsed;
            LoadingWait.Start();
        }
        void AfterLoad()
        {
            LoadingWait.Stop();
            
            filterTextBox.IsEnabled = true;
            orderListBox.IsEnabled = true;
            requestButton.IsEnabled = true;
            mainDataGrid.Visibility = Visibility.Visible;
            wtc.Visibility = Visibility.Visible;

            ShowOrderDetail(m.SelectedOrder);
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

            for (int i = 0; i < z.MainDetailList.Count; i++)
            {
                var d = z.MainDetailList[i];

                c = new DataGridTextColumn();
                c.Header = d.ShcmAndName;
                c.MaxWidth = 200;
                c.MinWidth = 50;
                c.Binding = new Binding($"Value[{i+1}]");
                mainDataGrid.Columns.Add(c);
            }

            List<string> NameList = new List<string>();
            foreach(var d in z.MainDetailList)
            {
                var e = from x in d.PlanOperations select x.Name;
                NameList.AddRange(e);
                e = from x in d.FactOperations select x.Name;
                NameList.AddRange(e);
            }

            var e2 = NameList.Distinct().OrderBy( x => x);
            NameList = new List<string>();
            NameList.AddRange(e2);

            foreach(var name in NameList)
            {
                OperationDictionary op = new OperationDictionary();
                op.Value[0] = name;
                for(int i = 0; i < z.MainDetailList.Count; i++)
                {
                    var d = z.MainDetailList[i];
                    var e3 = d.PlanOperations.Where(item => item.Name == name).SingleOrDefault();

                    var ef = d.FactOperations.Where(item => item.Name == name).SingleOrDefault();

                    op.Value[i + 1] = MakeReportString(plan: e3, fact: ef);
                }
                m.OperationList.Add(op);
            }

            mainDataGrid.ItemsSource = m.OperationList;
        }
        public string Format(TimeSpan ts)
        {
            string s = $"{(int)ts.TotalHours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            return s;
        }
        string MakeReportString(Operation plan, Operation fact)
        {
            StringBuilder sb = new StringBuilder();
            if (plan != null)
            {
                sb.Append("План: " + Format(plan.Time));
            }
            if (fact != null)
            {
                if (sb.Length > 0) sb.Append(Environment.NewLine);
                sb.Append("Факт: " + Format(fact.Time));
            }
            // остаток
            TimeSpan rest = TimeSpan.Zero;
            if (plan != null)
            {
                rest = plan.Time;
                if (fact != null) rest = rest.Subtract(fact.Time);
                sb.Append(Environment.NewLine);
                if (rest < TimeSpan.Zero) rest = TimeSpan.Zero;
                sb.Append("Остаток: " + Format(rest));
                // Процент
                if (fact != null)
                {
                    if (fact.Time > TimeSpan.Zero)
                    {
                        if (fact.Time <= plan.Time)
                        {
                            decimal dp = (decimal)plan.Time.TotalSeconds;
                            decimal df = (decimal)fact.Time.TotalSeconds;
                            decimal dr = df * (decimal)100.0 / dp;
                            int ir = Convert.ToInt32(dr);
                            sb.Append(Environment.NewLine).Append("Процент: " + ir + " %");
                        }
                        else
                        {
                            sb.Append(Environment.NewLine).Append("Процент: " + "100 %");
                        }
                    }
                }
            }
            
            return sb.ToString();
        }

        public void Stop()
        {
            if (loader != null)
            {
                loader.Stop();
                loader = null;
            }
        }
    }


}
