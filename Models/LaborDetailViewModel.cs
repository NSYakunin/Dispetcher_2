using Dispetcher2.Class;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dispetcher2.Models
{
    public class LaborDetailViewModel
    {
        IObserver observer;
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
                observer.Update(columnsValue);
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
        public LaborDetailViewModel(IObserver observer, LaborReportWriter writer)
        {
            this.observer = observer;
            this.writer = writer;
            RowsView = new ObservableCollection<LaborReportRow>();

            var c = new LaborCommand();
            c.ExecuteAction = this.ProcessExcelCommand;
            ExcelCommand = c;
        }
        void ProcessExcelCommand()
        {
            if (Columns != null && Rows != null)
                writer.Write(Columns, Rows);
        }
    }
}
