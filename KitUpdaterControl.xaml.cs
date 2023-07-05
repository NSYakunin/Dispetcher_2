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

namespace Dispetcher2
{
    /// <summary>
    /// Логика взаимодействия для KitUpdaterControl.xaml
    /// </summary>
    public partial class KitUpdaterControl : UserControl
    {
        public event EventHandler FinishEvent;
        KitUpdater ku = null;
        ProgressViewModel pvm = new ProgressViewModel();

        KitUpdater.ErrorDelegate errDel;

        public KitUpdaterControl()
        {
            InitializeComponent();
            errDel = this.ProcessNewError;
        }

        public void Start()
        {
            pvm.Reset();
            if (ku == null)
            {
                ku = new KitUpdater(pvm);
                this.DataContext = pvm;
                ku.FinishEvent += OnFinishEvent;
                ku.NewError += OnNewError;
            }
            
            ku.Start();
        }

        private void OnNewError(string text)
        {
            // errDel - делегат, хранящий адрес метода ProcessNewError
            // pa - массив параметров метода ProcessNewError
            object[] pa = new object[1];
            pa[0] = text;

            // Асинхронный вызов делегата в потоке этого элемента управления
            // BeginInvoke добавляет делегат в очередь событий элемента управления
            this.Dispatcher.BeginInvoke(errDel, pa);
        }

        void ProcessNewError(string text)
        {
            pvm.ErrorCollection.Add(text);
        }

        private void OnFinishEvent(object sender, EventArgs e)
        {
            if (FinishEvent != null) FinishEvent(sender, e);
        }

        public void Stop()
        {
            ku.Stop();
        }
    }
}
