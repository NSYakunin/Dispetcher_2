using Dispetcher2.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dispetcher2.Controls
{
    public partial class HostForm : Form, IObserver
    {
        public HostForm(System.Windows.Controls.UserControl control)
        {
            InitializeComponent();
            this.MainElementHost.Child = control;
        }

        public void Update(IEnumerable<string> columns)
        {
            IObserver observer = this.MainElementHost.Child as IObserver;
            if (observer != null) observer.Update(columns);
        }
    }
}
