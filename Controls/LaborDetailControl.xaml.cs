using Dispetcher2.Class;
using System;
using System.Collections.Generic;

using System.Windows.Controls;


namespace Dispetcher2.Controls
{
    /// <summary>
    /// Логика взаимодействия для LaborDetailControl.xaml
    /// </summary>
    public partial class LaborDetailControl : UserControl, IColumnsObserver
    {
        IColumnsObserver observer;
        public LaborDetailControl()
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
