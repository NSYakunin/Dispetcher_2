using System.Collections.Generic;
using System.Windows.Controls;
using Dispetcher2.Class;
namespace Dispetcher2.Controls
{
    public partial class LaborControl : UserControl, IObserver
    {
        public LaborControl()
        {
            InitializeComponent();
        }

        public void Update(object parameters)
        {
            opc.Update(parameters);
        }
    }
}