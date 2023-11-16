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
    public partial class HostForm : Form, IColumnsObserver
    {
        IColumnsObserver observer;
        public HostForm(System.Windows.Controls.UserControl control)
        {
            InitializeComponent();
            this.MainElementHost.Child = control;
            observer = control as IColumnsObserver;
        }

        void IColumnsObserver.Update(IEnumerable<string> columns)
        {
            if (observer != null) observer.Update(columns);
        }

        private void HostForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Controls.UserControl control = this.MainElementHost.Child as System.Windows.Controls.UserControl;
            if (control != null) control.DataContext = null;
        }
    }
}
