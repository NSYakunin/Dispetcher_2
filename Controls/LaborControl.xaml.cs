using System.Collections.Generic;
using System.Windows.Controls;
using Dispetcher2.Class;
namespace Dispetcher2.Controls
{
    public partial class LaborControl : UserControl, IColumnsObserver
    {
        IColumnsObserver observer;
        public LaborControl()
        {
            InitializeComponent();
            observer = opc;
        }
        void IColumnsObserver.Update(IEnumerable<string> columns)
        {
            observer.Update(columns);
        }
    }
}