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
    /// Логика взаимодействия для KitUpdaterControl.xaml
    /// </summary>
    public partial class KitUpdaterControl : UserControl
    {
        IConfig config;
        public event EventHandler FinishEvent;
        KitUpdater ku = null;
        ProgressViewModel pvm = new ProgressViewModel();

        public KitUpdaterControl(IConfig config)
        {
            this.config = config;
            InitializeComponent();
            pvm.SetDispatcher(this.Dispatcher);
            this.DataContext = pvm;
            errorListBox.ItemsSource = pvm.ErrorList;
        }

        public void Start()
        {
            pvm.Reset();
            if (ku == null)
            {
                ku = new KitUpdater(config, pvm);
                ku.FinishEvent += OnFinishEvent;
            }
            
            ku.Start();
        }

        private void OnFinishEvent(object sender, EventArgs e)
        {
            if (FinishEvent != null) FinishEvent(sender, e);
        }

        public void Stop()
        {
            if (ku != null) ku.Stop();
        }
    }
}
