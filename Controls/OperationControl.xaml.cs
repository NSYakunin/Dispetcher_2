
using System.Windows.Controls;
using System.Windows.Data;


using Dispetcher2.Class;

namespace Dispetcher2.Controls
{
    public partial class OperationControl : UserControl, IObserver
    {
        public OperationControl()
        {
            InitializeComponent();
        }

        public void Update(object parameters)
        {
            StringRepository names = parameters as StringRepository;
            if (names != null)
            {
                mainGrid.Columns.Clear();
                foreach (var n in names.GetList())
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
