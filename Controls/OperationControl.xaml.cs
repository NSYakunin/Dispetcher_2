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

using Dispetcher2.Class;
using Dispetcher2.Models;

namespace Dispetcher2.Controls
{
    /// <summary>
    /// Логика взаимодействия для LaborControl.xaml
    /// </summary>
    public partial class OperationControl : UserControl
    {
        OperationRepository operations;
        public OperationControl(OperationRepository operations)
        {
            this.operations = operations;
            InitializeComponent();
            SetColumns();
        }

        void SetColumns()
        {
            var e = operations.GetOperations();
            foreach (var n in e)
            {
                var c = new DataGridTextColumn();
                c.Header = n.Name;
                c.Binding = new Binding($"Operations[{n.Name}]");
                mainGrid.Columns.Add(c);
            }
        }
    }
}
