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
    public class OrderView : IOrder
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string Num1С { get; set; }
        public static OrderView GetOrderView(IOrder order)
        {
            var v = new OrderView();
            v.Id = order.Id;
            v.Number = order.Number;
            v.Name = order.Name;
            v.Num1С = order.Num1С;
            return v;
        }
    }
    public class OrderRepositoryViewModel : INotifyPropertyChanged
    {
        public string Filter { set; get; }
        public OrderView SelectedOrder { set; get; }

        ObservableCollection<OrderView> orderlist = new ObservableCollection<OrderView>();
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<OrderView> OrderList
        {
            get { return orderlist; }
        }
    }

}
