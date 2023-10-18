using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Windows.Threading;

using Dispetcher2.Class;

namespace Dispetcher2.Models
{
    public class LaborViewModel : INotifyPropertyChanged
    {
        OrderRepository ordRep;
        DetailRepository details;
        OperationRepository operations;
        OperationGroupRepository groups;

        OrderControlViewModel ocvm;
        Visibility dvValue;
        ICommand requestCommandValue;
        public Dispatcher Dispatcher { get; set; }
        string waitMessageValue;
        public Visibility DataVisibility
        {
            get
            {
                return dvValue;
            }
            set
            {
                dvValue = value;
                OnPropertyChanged(nameof(DataVisibility));
                OnPropertyChanged(nameof(WaitVisibility));
            }
        }
        public Visibility WaitVisibility
        {
            get
            {
                if (DataVisibility == Visibility.Visible) return Visibility.Collapsed;
                else return Visibility.Visible;
            }
        }
        public string WaitMessage
        {
            get { return waitMessageValue; }
            set
            {
                waitMessageValue = value;
                OnPropertyChanged(nameof(WaitMessage));
            }
        }
        public ObservableCollection<OrderView> OrderList
        {
            get { return ocvm.OrderList; }
        }
        public string Filter
        {
            get { return ocvm.Filter; }
            set { ocvm.Filter = value; }
        }
        public bool ShowDetailFlag { get; set; }
        public bool ShowOperationFlag { get; set; }
        public ICommand RequestCommand { get { return requestCommandValue; } }

        public event PropertyChangedEventHandler PropertyChanged;

        // Нужно переделать конструктор, использовав шаблон Фасад
        public LaborViewModel(OrderRepository ordRep, OrderControlViewModel ocvm,
            DetailRepository details, OperationRepository operations, OperationGroupRepository groups)
        {
            if (ordRep == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderRepository");
            if (ocvm == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderControlViewModel");
            
            if (details == null) throw new ArgumentException("Пожалуйста укажите параметр: DetailRepository");
            if (operations == null) throw new ArgumentException("Пожалуйста укажите параметр: OperationRepository");
            if (groups == null) throw new ArgumentException("Пожалуйста укажите параметр: OperationGroupRepository");
            this.ordRep = ordRep;
            this.ocvm = ocvm;
            this.details = details;
            this.operations = operations;
            this.groups = groups;
            var c = new LaborCommand();
            c.ExecuteAction = this.ProcessRequestCommand;
            requestCommandValue = c;
        }
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public void Start()
        {
            Before();

            var t = new Task(Load);
            t.Start();
        }
        void Before()
        {
            WaitMessage = "Загрузка...";
            DataVisibility = Visibility.Collapsed;
        }
        void Load()
        {
            ordRep.Load();
            details.Load();
            operations.Load();
            //System.Threading.Thread.Sleep(2000);

            if (Dispatcher != null)
            {
                // Выполнение делегата After в потоке Dispatcer
                Action a = After;
                Dispatcher.BeginInvoke(a);
            }
        }
        void After()
        {
            DataVisibility = Visibility.Visible;
            Filter = String.Empty;
        }
        private void ProcessRequestCommand()
        {
            System.Diagnostics.Debug.WriteLine("RequestCommand!");
        }
    }
    public class LaborCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action ExecuteAction { get; set; }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            if (ExecuteAction != null) ExecuteAction();
        }
    }
}
