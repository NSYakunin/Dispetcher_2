﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;

using Dispetcher2.Class;
using Dispetcher2.Controls;
using System.Diagnostics;
using System.Windows.Controls;

namespace Dispetcher2.Models
{
    public class LaborViewModel : INotifyPropertyChanged
    {
        OrderRepository orders;
        LaborReport report;
        
        LaborReportWriter writer;
        IColumnsObserver observer;

        OrderControlViewModel ocvm;
        Visibility dvValue;

        string waitMessageValue;

        LaborDetailViewModel detModel;
        FormFactory factory;

        public Visibility OrderVisibility
        {
            get
            { return dvValue; }
            set
            {
                dvValue = value;
                OnPropertyChanged(nameof(OrderVisibility));
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

        public ICommand RequestCommand { get; set; }
        public ICommand ExcelCommand { get; set; }
        public ICommand DetailCommand { get; set; }
        public ICommand SelectAllCommand { get; set; }
        public ICommand ClearAllCommand { get; set; }
        public ICommand BackCommand { get; set; }

        public Visibility DetailCommandVisibility
        {
            get { return Visibility.Visible; }
        }
        public Visibility CopyCommandVisibility
        {
            get { return Visibility.Collapsed; }
        }
        // Дата начала не нужна. В отчете учитываются все фактические операции до EndDate
        //public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public ObservableCollection<LaborReportRow> RowsView { get; set; }
        
        LaborReportRow SelectedLaborReportRow = null;
        string SelectedColumnHeader = null;
        public DataGridCellInfo CellInfo
        {
            set
            {
                DataGridCellInfo info = value;
                if (info != null)
                {
                    SelectedLaborReportRow = info.Item as LaborReportRow;
                    if (info.Column != null) SelectedColumnHeader = Convert.ToString(info.Column.Header);
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public LaborViewModel(OrderRepository orders, OrderControlViewModel ocvm, LaborReport report, 
            LaborReportWriter writer, IColumnsObserver observer,
            FormFactory factory, LaborDetailViewModel detModel)
        {
            if (orders == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderRepository");
            if (ocvm == null) throw new ArgumentException("Пожалуйста укажите параметр: OrderControlViewModel");
            if (report == null) throw new ArgumentException("Пожалуйста укажите параметр: LaborReport");
            if (writer == null) throw new ArgumentException("Пожалуйста укажите параметр: LaborReportWriter");

            if (factory == null) throw new ArgumentException("Пожалуйста укажите параметр: factory");
            if (detModel == null) throw new ArgumentException("Пожалуйста укажите параметр: detModel");

            this.orders = orders;
            this.ocvm = ocvm;
            this.report = report;
            this.writer = writer;
            this.observer = observer;

            this.factory = factory;
            this.detModel = detModel;

            RequestCommand = new LaborCommand()
            {
                ExecuteAction = this.ProcessRequestCommand,
            };

            ExcelCommand = new LaborCommand()
            {
                ExecuteAction = this.ProcessExcelCommand,
            };

            DetailCommand = new LaborCommand()
            {
                ExecuteAction = this.ProcessDetailCommand,
            };

            BackCommand = new LaborCommand()
            {
                ExecuteAction = this.ProcessBackCommand,
            };

            SelectAllCommand = ocvm.SelectAllCommand;
            ClearAllCommand = ocvm.ClearAllCommand;

            WaitVisibility = Visibility.Visible;
            OrderVisibility = Visibility.Collapsed;
            CommandVisibility = Visibility.Collapsed;
            OperationVisibility = Visibility.Collapsed;
            
            RowsView = new ObservableCollection<LaborReportRow>();

            DateTime n = DateTime.Now.Date;
            //BeginDate = new DateTime(n.Year, 1, 1);
            EndDate = new DateTime(n.Year, n.Month, 1);
            EndDate = EndDate.AddMonths(1);

        }
        void OnPropertyChanged(string prop)
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
            OrderVisibility = Visibility.Collapsed;
        }
        void Load()
        {
            orders.Load();
            //report.Load();
        }
        void After()
        {
            
            OrderVisibility = Visibility.Visible;
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
                OrderVisibility = Visibility.Collapsed;
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
            //report.ShowDetailFlag = this.ShowDetailFlag;
            //report.ShowOperationFlag = this.ShowOperationFlag;
            //report.BeginDate = this.BeginDate;
            report.EndDate = this.EndDate;
            //report.AllOrdersFlag = this.AllOrdersFlag;

            Action a = report.Calculate;
            await Task.Run(a);

            AfterLoadOperations();
        }

        void AfterLoadOperations()
        {
            var columns = report.GetColumns();
            if (observer != null) observer.Update(columns);

            WaitVisibility = Visibility.Collapsed;
            CommandVisibility = Visibility.Collapsed;
            OperationVisibility = Visibility.Visible;
            OrderVisibility = Visibility.Collapsed;

            var rows = report.GetRows();
            RowsView.Clear();
            foreach (LaborReportRow r in rows) RowsView.Add(r);
        }
        void ProcessExcelCommand()
        {
            WaitVisibility = Visibility.Visible;
            OrderVisibility = Visibility.Collapsed;
            CommandVisibility = Visibility.Collapsed;
            OperationVisibility = Visibility.Collapsed;

            ExcelCommandMainAsync();
        }
        void ExcelCommandMain()
        {
            var columns = report.GetColumns();
            if (columns == null) return;
            var rows = report.GetRows();
            if (rows == null) return;

            string H1 = "Трудоемкость работ";
            //string H2 = $"с {BeginDate.ToShortDateString()} по {EndDate.ToShortDateString()}";
            string H2 = $"на {EndDate.ToShortDateString()}";
            writer.Write(columns, rows, H1, H2);
        }
        async Task ExcelCommandMainAsync()
        {
            Action a = this.ExcelCommandMain;
            await Task.Run(a);
            AfterExcelCommand();
        }
        void AfterExcelCommand()
        {

            WaitVisibility = Visibility.Collapsed;
            CommandVisibility = Visibility.Collapsed;
            OperationVisibility = Visibility.Visible;
        }
        void ProcessDetailCommand()
        {
            if (SelectedLaborReportRow == null) return;
            if (SelectedColumnHeader == null) return;
            if (SelectedLaborReportRow.TimeDictionary.ContainsKey(SelectedColumnHeader) == false) return;
            var a = SelectedLaborReportRow.TimeDictionary[SelectedColumnHeader];

            List<LaborReportRow> detRows = new List<LaborReportRow>();
            foreach (var item in a)
            {
                if (item is WorkDay) detRows.Add(report.GetReportRow(item as WorkDay));
                if (item is Operation) detRows.Add(report.GetReportRow(item as Operation));
            }
            if (detRows.Count() < 1) return;
            LaborReportRow r1 = detRows[0];

            HashSet<string> NameList = new HashSet<string>();
            foreach (var k in r1.Operations.Keys)
                NameList.Add(k);



            using(System.Windows.Forms.Form f = factory.GetForm("Подробный список операций"))
            {
                detModel.Columns = NameList;
                detModel.Rows = detRows;
                detModel.H1 = SelectedColumnHeader;
                detModel.H2 = SelectedLaborReportRow.Name;
                f.ShowDialog();
            }
        }
        void ProcessBackCommand()
        {
            WaitVisibility = Visibility.Collapsed;
            OperationVisibility = Visibility.Collapsed;

            OrderVisibility = Visibility.Visible;
            CommandVisibility = Visibility.Visible;

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
