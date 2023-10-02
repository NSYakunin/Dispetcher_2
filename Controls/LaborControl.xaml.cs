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
        IConfig config;
        IConverter converter;
        OrderRepository orep;
        DetailRepository allDetails;
        OperationRepository allOperations;
        WorkDayRepository wdr;

        OrderControlViewModel ordvm;
        WorkTimeViewModel wtvm;
        OperationViewModel oprvm;

        WaitControl wc;
        UserControl ordControl;
        OperationControl oprControl;
        //UserControl wtControl;
        OperationControl wtControl;

        public LaborControl(OrderRepository ordRep, IConfig config, IConverter converter)
        {
            if (ordRep == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderRepository");
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр: IConfig");
            if (converter == null) throw new ArgumentException("Пожалуйста укажите параметр converter");
            InitializeComponent();

            this.config = config;
            this.converter = converter;

            this.orep = ordRep;

            // Эти классы являются зависимостями
            // Надо выносить наружу, также как OrderRepository
            // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
            // Хранилища
            this.allDetails = new SqlDetailRepository(config, converter);
            this.allOperations = new SqlOperationRepository(config, converter);

            var gr = new SqlOperationGroupRepository(config, converter);
            
            wdr = new SqlWorkDayRepository(config, converter, DateTime.Now);
            
            // Модели представления
            wtvm = new WorkTimeViewModel(wdr, gr);
            ordvm = new OrderControlViewModel(orep);
            oprvm = new OperationViewModel(allDetails, allOperations, gr);

            // Элементы управления
            wc = new WaitControl();
            //ordControl = new OrderControl();
            ordControl = new CheckedOrderControl();
            //var tdr = new TestDetailRepository();
            //var tor = new TestOperationRepository();
            oprControl = new OperationControl();

            //wtControl = new WorkTimeControl();
            wtControl = new OperationControl();
            // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            HideAll();
            var t = new Task(LoadOrders);
            t.Start();
        }

        private void OnRequest(object sender, RoutedEventArgs e)
        {
            HideAll();
            var t = new Task(LoadOperations);
            t.Start();
        }

        void HideAll()
        {
            orderListPlace.Visibility = Visibility.Collapsed;
            
            workTimePlace.Visibility = Visibility.Collapsed;
            operationPlace.Visibility = Visibility.Collapsed;
            commandGrid.Visibility = Visibility.Collapsed;
            
            wc.Message = "Загрузка...";
            loadingPlace.Content = wc;
            loadingPlace.Visibility = Visibility.Visible;
            wc.Start();
        }
        void LoadOrders()
        {
            orep.Load();
            allDetails.Load();
            allOperations.Load();
            Action a = AfterLoadOrders;
            this.Dispatcher.BeginInvoke(a);
        }
        void AfterLoadOrders()
        {
            ordControl.DataContext = ordvm;
            orderListPlace.Content = ordControl;
            
            wc.Stop();

            loadingPlace.Visibility = Visibility.Collapsed;
            orderListPlace.Visibility = Visibility.Visible;
            commandGrid.Visibility = Visibility.Visible;

            ordvm.Filter = String.Empty;
            oprvm.ShowDetailFlag = true;
            oprvm.ShowOperationFlag = true;
            this.DataContext = oprvm;
            
        }

        void LoadOperations()
        {
            var so = ordvm.GetSelectedOrders();
            if (so.GetOrders().Count() < 1)
            {
                MessageBox.Show("Пожалуйста выберите заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            wdr.Load();
            wtvm.Load();
            oprvm.Update(so);

            Action a = AfterLoad;
            this.Dispatcher.BeginInvoke(a);
        }

        void AfterLoad()
        {
            wc.Stop();
            loadingPlace.Visibility = Visibility.Collapsed;

            orderListPlace.Visibility = Visibility.Visible;
            commandGrid.Visibility = Visibility.Visible;

            ShowOperations();
        }

        void ShowOperations()
        {
            oprControl.DataContext = null;
            oprControl.Update(oprvm.GetOperationRepository());
            oprControl.DataContext = oprvm;

            operationPlace.Content = oprControl;
            operationPlace.Visibility = Visibility.Visible;

            wtControl.DataContext = null;
            wtControl.Update(wtvm.GetOperationRepository());
            wtControl.DataContext = wtvm;

            workTimePlace.Content = wtControl;
            workTimePlace.Visibility = Visibility.Visible;
        }
    }
}
