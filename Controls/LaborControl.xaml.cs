using System.Collections.Generic;
using System.Windows.Controls;
using Dispetcher2.Class;
namespace Dispetcher2.Controls
{
    public partial class LaborControl : UserControl, IColumnUpdate
    {
        public LaborControl()
        {
            InitializeComponent();
        }

        public void Update(IEnumerable<string> names)
        {
            opc.Update(names);
        }
    }
}