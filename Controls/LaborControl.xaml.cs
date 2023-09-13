﻿using System;
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
using System.Threading.Tasks;

using Dispetcher2.Class;
using Dispetcher2.Models;
using Dispetcher2.DataAccess;

namespace Dispetcher2.Controls
{
    /// <summary>
    /// Логика взаимодействия для LaborControl.xaml
    /// </summary>
    public partial class LaborControl : UserControl
    {
        OrderRepository orep;
        IConfig config;
        OrderControlViewModel ovm;
        WaitControl wc;
        public LaborControl(OrderFactory factory, IConfig config)
        {
            if (factory == null) throw new ArgumentException("Пожалуйста укажите параметр: factory");
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр: config");
            InitializeComponent();

            this.config = config;

            orep = factory.GetOrderRepository(config);






            HideAll();
            var t = new Task(LoadOrders);
            t.Start();
        }

        private void OnRequest(object sender, RoutedEventArgs e)
        {
            if (ovm.OrderList.Count == 1) ovm.SelectedOrder = ovm.OrderList[0];
            if (ovm.SelectedOrder == null)
            {
                MessageBox.Show("Пожалуйста выберите заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //BeforeLoad();
            //loader = new LaborLoader(m.SelectedOrder);
            //loader.Finished += Ll_Finished;
            //loader.Start();


            var tdr = new TestDetailRepository();
            var tor = new TestOperationRepository();

            var c = new OperationControl(tdr, tor);
            operationPlace.Content = c;
            operationPlace.Visibility = Visibility.Visible;
            
            var wdr = new SqlWorkDayRepository(config, DateTime.Now);
            var w = new WorkTimeControl();
            WorkTimeViewModel vm = new WorkTimeViewModel(wdr);
            w.DataContext = vm;
            workTimePlace.Content = w;
            workTimePlace.Visibility = Visibility.Visible;
        }

        void HideAll()
        {
            orderListPlace.Visibility = Visibility.Collapsed;
            requestButton.Visibility = Visibility.Collapsed;
            workTimePlace.Visibility = Visibility.Collapsed;
            operationPlace.Visibility = Visibility.Collapsed;

            wc = new WaitControl();
            wc.Message = "Загрузка...";
            loadingPlace.Content = wc;
            loadingPlace.Visibility = Visibility.Visible;
            wc.Start();
        }
        void LoadOrders()
        {
            orep.Load();
            System.Threading.Thread.Sleep(1000);
            Action a = AfterLoadOrders;
            this.Dispatcher.BeginInvoke(a);
        }
        void AfterLoadOrders()
        {
            ovm = new OrderControlViewModel(orep);
            var c = new OrderControl();
            c.DataContext = ovm;
            orderListPlace.Content = c;

            wc.Stop();
            loadingPlace.Visibility = Visibility.Collapsed;

            orderListPlace.Visibility = Visibility.Visible;
            requestButton.Visibility = Visibility.Visible;
        }
    }
}
