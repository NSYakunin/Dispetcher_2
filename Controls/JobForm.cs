using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dispetcher2.Models;

namespace Dispetcher2.Controls
{
    public partial class JobForm : Form
    {
        public JobForm(object viewmodel)
        {
            InitializeComponent();

            JobControl control = new JobControl();
            JobElementHost.Child = control;
            control.DataContext = viewmodel;

        }
    }
}
