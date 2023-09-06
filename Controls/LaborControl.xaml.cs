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
    /// Логика взаимодействия для LaborControl.xaml
    /// </summary>
    public partial class LaborControl : UserControl
    {
        OrderFactory factory;
        IConfig config;
        OrderRepositoryViewModel ovm;
        public LaborControl(OrderFactory factory, IConfig config)
        {
            if (factory == null) throw new ArgumentException("Пожалуйста укажите параметр: factory");
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр: config");
            InitializeComponent();

            var r = factory.GetOrderRepository(config);
            ovm = new OrderRepositoryViewModel();
            var c = new OrderControl(r, ovm);
            orderListPlace.Content = c;
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

        }
    }
}
