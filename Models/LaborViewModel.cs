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
    public class LaborReportRow
    {
        // имя строки
        public string Name { get; set; }
        // ключ словаря: имя операции
        // значение словаря: текст в ячейке
        public Dictionary<string, string> Operations { get; set; }

        public LaborReportRow()
        {
            Operations = new Dictionary<string, string>();
        }
    }
    public class LaborViewModel : INotifyPropertyChanged
    {
        OrderRepository orders;
        DetailRepository details;
        OperationRepository operations;
        OperationGroupRepository groups;
        WorkDayRepository workDays;

        OrderControlViewModel ocvm;
        Visibility dvValue;
        ICommand requestCommandValue;
        //public Dispatcher Dispatcher { get; set; }
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
        public ICommand RequestCommand { get { return requestCommandValue; } }
        
        List<LaborReportRow> rows;
        public IEnumerable<LaborReportRow> Rows { get { return rows; } }

        public event PropertyChangedEventHandler PropertyChanged;

        // Нужно переделать конструктор, использовав шаблон Фасад
        public LaborViewModel(OrderRepository orders, OrderControlViewModel ocvm,
            DetailRepository details, OperationRepository operations, 
            OperationGroupRepository groups, WorkDayRepository workDays)
        {
            if (orders == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderRepository");
            if (ocvm == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderControlViewModel");
            
            if (details == null) throw new ArgumentException("Пожалуйста укажите параметр: DetailRepository");
            if (operations == null) throw new ArgumentException("Пожалуйста укажите параметр: OperationRepository");
            if (groups == null) throw new ArgumentException("Пожалуйста укажите параметр: OperationGroupRepository");
            if (workDays == null) throw new ArgumentException("Пожалуйста укажите параметр: WorkDayRepository");
            this.orders = orders;
            this.ocvm = ocvm;
            this.details = details;
            this.operations = operations;
            this.groups = groups;
            this.workDays = workDays;

            var c = new LaborCommand();
            c.ExecuteAction = this.ProcessRequestCommand;
            requestCommandValue = c;

            WaitVisibility = Visibility.Visible;
            DataVisibility = Visibility.Collapsed;
            CommandVisibility = Visibility.Collapsed;
            OperationVisibility = Visibility.Collapsed;

            rows = new List<LaborReportRow>();
        }
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public async Task Start()
        {
            Before();

            //var t = new Task(Load);
            //t.Start();
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
            details.Load();
            operations.Load();
            groups.Load();
            workDays.Load();
        }
        void After()
        {
            DataVisibility = Visibility.Visible;
            WaitVisibility = Visibility.Collapsed;
            Filter = String.Empty;
            CommandVisibility = Visibility.Visible;
        }
        private void ProcessRequestCommand()
        {
            // Список выбранных заказов
            var selectedOrders = ocvm.GetSelectedOrders();
            if (selectedOrders.GetOrders().Any() == false)
            {
                MessageBox.Show("Пожалуйста выберите один или несколько заказов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.rows.Clear();
            if (ShowOperationFlag == false)
            {
                ProcessEmployeeTime();
            }
        }

        private void ProcessEmployeeTime()
        {
            // словарь операций по коду
            var opDic = new Dictionary<int, OperationGroup>();
            foreach (var item in groups.GetGroups()) opDic[item.Id] = item;

            // Группировать данные в словарь списков по коду группы операции
            // Группировать данные в словарь операция-сотрудники
            var wdgroups = workDays.GetWorkDays().GroupBy(item => item.OperationGroupId);
            // Ключ: код операции, значение: список рабочих дней
            var wdDic = new Dictionary<int, List<WorkDay>>();
            // Ключ: код операции, значение: уникальный список сотрудников
            var empDic = new Dictionary<int, HashSet<Employee>>();

            foreach (var g in wdgroups)
            {
                int id = g.Key;
                if (id == 0) continue;

                wdDic[id] = new List<WorkDay>();
                empDic[id] = new HashSet<Employee>();
                foreach (var item in g)
                {
                    wdDic[id].Add(item);
                    Employee e = new Employee();
                    e.ITR = item.ITR;
                    e.LastName = item.LastName;
                    e.Login = item.Login;
                    e.Name = item.Name;
                    e.SecondName = item.SecondName;
                    e.TabNum = item.TabNum;
                    if (empDic[id].Contains(e) == false) empDic[id].Add(e);
                }
            }

            var dv = new LaborReportRow() { Name = "Кол-во работников, чел" };
            foreach (var item in opDic)
            {
                OperationGroup op = item.Value;
                if (empDic.ContainsKey(op.Id))
                {
                    dv.Operations[op.Name] = empDic[op.Id].Count().ToString();
                }
            }
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
