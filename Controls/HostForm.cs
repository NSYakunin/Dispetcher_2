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
    public partial class HostForm : Form
    {
        public HostForm(System.Windows.Controls.UserControl control)
        {
            InitializeComponent();
            this.MainElementHost.Child = control;
        }
    }
}
