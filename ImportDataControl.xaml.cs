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
    /// Логика взаимодействия для ImportDataControl.xaml
    /// </summary>
    public partial class ImportDataControl : UserControl
    {
        ImportDataWorker idw = null;
        public event EventHandler FinishEvent;

        ProgressViewModel pvm = new ProgressViewModel();
        public ImportDataControl()
        {
            InitializeComponent();

            pvm.SetDispatcher(this.Dispatcher);
            this.DataContext = pvm;
            errorListBox.ItemsSource = pvm.ErrorList;
        }

        private void OnFinishEvent(object sender, EventArgs e)
        {
            if (FinishEvent != null) FinishEvent(sender, e);
        }

        public void Start()
        {
            pvm.Reset();
            if (idw == null)
            {
                idw = new ImportDataWorker(pvm);
                idw.FinishEvent += OnFinishEvent;
            }

            idw.Start();
        }

        public void Stop()
        {
            if (idw != null) idw.Stop();
        }

        private void wayFolderButton_Click(object sender, RoutedEventArgs e)
        {
            using (var d = new System.Windows.Forms.FolderBrowserDialog())
            {
                var r = d.ShowDialog();
                if (r == System.Windows.Forms.DialogResult.OK) pvm.WayToFolder = d.SelectedPath;
            }
        }

        private void wayArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            using (var d = new System.Windows.Forms.FolderBrowserDialog())
            {
                var r = d.ShowDialog();
                if (r == System.Windows.Forms.DialogResult.OK) pvm.WayToFolderArchive = d.SelectedPath;
            }
        }

        private void OnStart(object sender, RoutedEventArgs e)
        {
            Start();
        }

        private void OnStop(object sender, RoutedEventArgs e)
        {
            Stop();
        }

        private void OnCopyMenu(object sender, RoutedEventArgs e)
        {
            if (pvm.SelectedItem != null)
                if (pvm.SelectedItem.Tag != null)
                    Clipboard.SetText(pvm.SelectedItem.Tag.ToString());
        }
    }
}
