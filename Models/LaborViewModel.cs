using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

using Dispetcher2.Class;

namespace Dispetcher2.Models
{
    public class LaborViewModel : INotifyPropertyChanged
    {
        OrderRepository orders;
        LaborReport report;
        LaborReportRepository labrep;
        StringRepository colrep;
        LaborReportWriter writer;

        OrderControlViewModel ocvm;
        Visibility dvValue;
        ICommand requestCommandValue;
        ICommand excelCommandValue;

        string waitMessageValue;
        public Visibility DataVisibility
        {
            get
            { return dvValue; }
            set
            {
                dvValue = value;
                OnPropertyChanged(nameof(DataVisibility));
            }
        }
        Visibility wtVis;
        public Visibility WaitVisibility
        {
            get { return wtVis; }
            set
            {
                wtVis = value;
                OnPropertyChanged(nameof(WaitVisibility));
            }
        }
        Visibility comVis;
        public Visibility CommandVisibility
        {
            get { return comVis; }
            set
            {
                comVis = value;
                OnPropertyChanged(nameof(CommandVisibility));
            }
        }
        Visibility opVis;
        public Visibility OperationVisibility
        {
            get { return opVis; }
            set
            {
                opVis = value;
                OnPropertyChanged(nameof(OperationVisibility));
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
        bool factOrdVal = true;
        public bool FactOrdersFlag
        {
            get { return factOrdVal; }
            set
            {
                factOrdVal = value;
                if (factOrdVal)
                {
                    DataVisibility = Visibility.Collapsed;
                }
                else
                {
                    DataVisibility = Visibility.Visible;
                }
            }
        }
        public ICommand RequestCommand { get { return requestCommandValue; } }
        public ICommand ExcelCommand { get { return excelCommandValue; } }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public ObservableCollection<LaborReportRow> RowsView { get; set; }

        public StringRepository Columns
        {
            get { return colrep; }
            set
            {
                colrep = value;
                OnPropertyChanged(nameof(Columns));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public LaborViewModel(OrderRepository orders, OrderControlViewModel ocvm, LaborReport report, LaborReportWriter writer)
        {
            if (orders == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderRepository");
            if (ocvm == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderControlViewModel");
            if (report == null) throw new ArgumentException("Пожалуйста укажите параметр: LaborReport");
            if (writer == null) throw new ArgumentException("Пожалуйста укажите параметр: LaborReportWriter");

            this.orders = orders;
            this.ocvm = ocvm;
            this.report = report;
            this.writer = writer;

            var c = new LaborCommand();
            c.ExecuteAction = this.ProcessRequestCommand;
            requestCommandValue = c;

            c = new LaborCommand();
            c.ExecuteAction = this.ProcessExcelCommand;
            excelCommandValue = c;

            WaitVisibility = Visibility.Visible;
            DataVisibility = Visibility.Collapsed;
            CommandVisibility = Visibility.Collapsed;
            OperationVisibility = Visibility.Collapsed;
            
            RowsView = new ObservableCollection<LaborReportRow>();

            DateTime n = DateTime.Now.Date;
            BeginDate = new DateTime(n.Year, 1, 1);
            EndDate = new DateTime(n.Year, n.Month, 1);
            FactOrdersFlag = true;
        }
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public async Task Start()
        {
            Before();

            Action a = this.Load;
            await Task.Run(a);

            After();
        }
        void Before()
        {
            WaitMessage = "Загрузка...";
            DataVisibility = Visibility.Collapsed;
        }
        void Load()
        {
            orders.Load();
            report.Load();
        }
        void After()
        {
            if (factOrdVal == false) DataVisibility = Visibility.Visible;
            WaitVisibility = Visibility.Collapsed;
            Filter = String.Empty;
            CommandVisibility = Visibility.Visible;
        }
        private void ProcessRequestCommand()
        {
            try
            {
                /*// Список выбранных заказов
                var selectedOrders = ocvm.GetOrders();
                if (selectedOrders.Any() == false)
                {
                    MessageBox.Show("Пожалуйста выберите один или несколько заказов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }*/

                WaitVisibility = Visibility.Visible;
                DataVisibility = Visibility.Collapsed;
                CommandVisibility = Visibility.Collapsed;
                OperationVisibility = Visibility.Collapsed;

                LoadOperationsAsync();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
        async Task LoadOperationsAsync()
        {
            report.SelectedOrders = ocvm.SelectedOrders;
            report.ShowDetailFlag = this.ShowDetailFlag;
            report.ShowOperationFlag = this.ShowOperationFlag;
            report.BeginDate = this.BeginDate;
            report.EndDate = this.EndDate;
            report.FactOrdersFlag = this.FactOrdersFlag;

            Action a = report.Calculate;
            await Task.Run(a);

            AfterLoadOperations();
        }

        void AfterLoadOperations()
        {
            Columns = report.GetOperationRepository();

            if (factOrdVal == false) DataVisibility = Visibility.Visible;
            WaitVisibility = Visibility.Collapsed;
            CommandVisibility = Visibility.Visible;
            OperationVisibility = Visibility.Visible;

            labrep = report.GetLaborReportRepository();
            RowsView.Clear();
            foreach (LaborReportRow r in labrep.GetList()) RowsView.Add(r);
        }
        void ProcessExcelCommand()
        {
            if (Columns != null && labrep != null)
                writer.Write(Columns, labrep);
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
