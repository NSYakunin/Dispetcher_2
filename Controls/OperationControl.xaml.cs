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
        DetailViewRepository vm;

        public OperationControl(DetailRepository details, OperationRepository operations)
        {

            this.vm = new DetailViewRepository(details, operations);
            InitializeComponent();
            this.DataContext = vm;
            SetColumns();
        }

        void SetColumns()
        {
            var e = GetNames();
            foreach (var n in e)
            {
                var c = new DataGridTextColumn();
                c.Header = n;
                c.Binding = new Binding($"Operations[{n}]");
                mainGrid.Columns.Add(c);
            }
        }
        IEnumerable<string> GetNames()
        {
            List<string> NameList = new List<string>();
            foreach (var item in vm.Details)
            {
                NameList.AddRange(item.Operations.Keys);
            }
            var e = NameList.Distinct().OrderBy(x => x);
            return e;
        }

        
    }
}
