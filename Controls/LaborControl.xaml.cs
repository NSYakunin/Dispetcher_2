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
        OrderRepository orep;
        IConfig config;
        OrderControlViewModel ovm;
        WorkTimeViewModel wtvm;

        DetailRepository allDetails;
        OperationRepository allOperations;
        OperationViewModel dvr;
        WorkDayRepository wdr;

        WaitControl wc;
        UserControl ordControl;
        OperationControl oprControl;
        UserControl wtControl;

        public LaborControl(OrderRepository ordRep, IConfig config)
        {
            if (ordRep == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderRepository");
            if (config == null) throw new ArgumentException("Пожалуйста укажите параметр: IConfig");
            InitializeComponent();

            this.config = config;

            this.orep = ordRep;

            // Эти классы являются зависимостями
            // Надо выносить наружу, также как OrderRepository
            // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
            // Хранилища
            this.allDetails = new SqlDetailRepository(config);
            this.allOperations = new SqlOperationRepository(config);
            
            wdr = new SqlWorkDayRepository(config, DateTime.Now);
            
            // Модели представления
            wtvm = new WorkTimeViewModel();
            ovm = new OrderControlViewModel(orep);
            dvr = new OperationViewModel(allDetails, allOperations);

            // Элементы управления
            wc = new WaitControl();
            //ordControl = new OrderControl();
            ordControl = new CheckedOrderControl();
            //var tdr = new TestDetailRepository();
            //var tor = new TestOperationRepository();
            oprControl = new OperationControl();
            wtControl = new WorkTimeControl();
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
            requestButton.Visibility = Visibility.Collapsed;
            workTimePlace.Visibility = Visibility.Collapsed;
            operationPlace.Visibility = Visibility.Collapsed;
            
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
            ordControl.DataContext = ovm;
            orderListPlace.Content = ordControl;
            
            wc.Stop();

            loadingPlace.Visibility = Visibility.Collapsed;
            orderListPlace.Visibility = Visibility.Visible;
            requestButton.Visibility = Visibility.Visible;

            ovm.Filter = String.Empty;
        }


        void LoadOperations()
        {
            var so = ovm.GetSelectedOrders();
            if (so.GetOrders().Count() < 1)
            {
                MessageBox.Show("Пожалуйста выберите заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            wdr.Load();
            wtvm.Load(wdr);
            dvr.Update(so);

            Action a = AfterLoad;
            this.Dispatcher.BeginInvoke(a);
        }



        void AfterLoad()
        {
            wc.Stop();
            loadingPlace.Visibility = Visibility.Collapsed;

            orderListPlace.Visibility = Visibility.Visible;
            requestButton.Visibility = Visibility.Visible;


            ShowOperations();

            
        }

        void ShowOperations()
        {
            oprControl.DataContext = null;
            oprControl.Update(dvr.GetOperationRepository());
            oprControl.DataContext = dvr;

            operationPlace.Content = oprControl;
            operationPlace.Visibility = Visibility.Visible;

            
            

            wtControl.DataContext = wtvm;
            workTimePlace.Content = wtControl;
            workTimePlace.Visibility = Visibility.Visible;

        }
    }
}
