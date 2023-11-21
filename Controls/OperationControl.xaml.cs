
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;


using Dispetcher2.Class;

namespace Dispetcher2.Controls
{
    public partial class OperationControl : UserControl, IColumnsObserver
    {
        public OperationControl()
        {
            InitializeComponent();
        }
        void IColumnsObserver.Update(IEnumerable<string> columns)
        {
            if (columns != null)
            {
                mainGrid.Columns.Clear();
                foreach (var n in columns)
                {
                    var c = new DataGridTextColumn();
                    c.Header = n;
                    c.Binding = new Binding($"Operations[{n}]");
                    mainGrid.Columns.Add(c);
                }
            }
        }
    }
}
