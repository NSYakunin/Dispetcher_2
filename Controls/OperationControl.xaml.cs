using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

using Dispetcher2.Class;

namespace Dispetcher2.Controls
{

    public partial class OperationControl : UserControl, IColumnUpdate
    {
        public OperationControl()
        {
            InitializeComponent();
        }

        public void Update(IEnumerable<string> names)
        {
            mainGrid.Columns.Clear();
            foreach (var n in names)
            {
                var c = new DataGridTextColumn();
                c.Header = n;
                c.Binding = new Binding($"Operations[{n}]");
                mainGrid.Columns.Add(c);
            }
        }
    }
}
