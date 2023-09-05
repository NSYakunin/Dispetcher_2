using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

using Dispetcher2.Class;

namespace Dispetcher2.Models
{
    public class OrderRepositoryViewModel : INotifyPropertyChanged
    {
        public string Filter { set; get; }
        public Order SelectedOrder { set; get; }

        ObservableCollection<Order> orderblist = new ObservableCollection<Order>();
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<Order> OrderList
        {
            get { return orderblist; }
        }
    }

}
