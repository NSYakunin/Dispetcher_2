using Dispetcher2.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dispetcher2.Models
{
    public class LaborDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public IColumnsObserver Observer { get; set; }
        LaborReportWriter writer;
        IEnumerable<string> columnsValue;
        public ObservableCollection<LaborReportRow> RowsView { get; set; }
        public ICommand ExcelCommand { get; set; }
        public IEnumerable<string> Columns
        {
            get { return columnsValue; }
            set
            {
                columnsValue = value;
                if (Observer != null) Observer.Update(columnsValue);
            }
        }
        public IEnumerable<LaborReportRow> Rows
        {
            get { return RowsView; }
            set
            {
                RowsView.Clear();
                foreach(var item in value) RowsView.Add(item);
            }
        }
        Visibility ldVis;
        public Visibility LoadVisibility
        {
            get { return ldVis; }
            set
            {
                ldVis = value;
                OnPropertyChanged(nameof(LoadVisibility));
            }
        }
        Visibility mnVis;
        public Visibility MainVisibility
        {
            get { return mnVis; }
            set
            {
                mnVis = value;
                OnPropertyChanged(nameof(MainVisibility));
            }
        }
        string h1Value = String.Empty;
        string h2Value = String.Empty;
        public string H1
        {
            get { return h1Value; }
            set
            {
                h1Value = value;
                OnPropertyChanged(nameof(H1));
            }
        }
        public string H2
        {
            get { return h2Value; }
            set
            {
                h2Value = value;
                OnPropertyChanged(nameof(H2));
            }
        }
        public LaborDetailViewModel(LaborReportWriter writer)
        {
            this.writer = writer;
            RowsView = new ObservableCollection<LaborReportRow>();

            var c = new LaborCommand();
            c.ExecuteAction = this.ProcessExcelCommand;
            ExcelCommand = c;

            LoadVisibility = Visibility.Collapsed;
            MainVisibility = Visibility.Visible;
        }
        void ProcessExcelCommand()
        {
            LoadVisibility = Visibility.Visible;
            MainVisibility = Visibility.Collapsed;

            ExcelCommandMainAsync();
        }

        async Task ExcelCommandMainAsync()
        {
            Action a = this.ExcelCommandMain;
            await Task.Run(a);
            AfterExcelCommand();
        }

        void ExcelCommandMain()
        {
            if (Columns != null && Rows != null)
                writer.Write(Columns, Rows, H1, H2);
        }
        void AfterExcelCommand()
        {
            LoadVisibility = Visibility.Collapsed;
            MainVisibility = Visibility.Visible;
        }
    }
}
