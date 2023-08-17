using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Dispetcher2.Class
{
    public class OrderControlModel : INotifyPropertyChanged
    {
        string messageValue;
        public string Filter { set; get; }
        public Order SelectedOrder { set; get; }

        BindingList<Order> orderblist = new BindingList<Order>();
        public event PropertyChangedEventHandler PropertyChanged;

        public List<OperationDictionary> OperationList { get; set; }

        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public string Message
        {
            set
            {
                messageValue = value;
                OnPropertyChanged("Message");
            }
            get { return messageValue; }
        }

        public BindingList<Order> OrderList
        {
            get { return orderblist; }
        }
    }
    public class OperationDictionary
    {
        public Dictionary<int, object> Value { get; set; }

        public OperationDictionary()
        {
            Value = new Dictionary<int, object>();
        }
    }
}
