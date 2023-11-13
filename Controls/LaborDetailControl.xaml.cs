using Dispetcher2.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dispetcher2.Controls
{
    /// <summary>
    /// Логика взаимодействия для LaborDetailControl.xaml
    /// </summary>
    public partial class LaborDetailControl : UserControl, IObserver
    {
        public LaborDetailControl()
        {
            InitializeComponent();
        }

        public void Update(IEnumerable<string> columns)
        {
            opc.Update(columns);
        }
    }
}
