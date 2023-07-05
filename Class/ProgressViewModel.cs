using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Dispetcher2.Class
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        double progress = 0;
        string status = "Подготовка...";
        ObservableCollection<string> errCol = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        public double Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                OnPropertyChanged("Progress");
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public ObservableCollection<string> ErrorCollection
        {
            get { return errCol; }
        }

        public void Reset()
        {
            Progress = 0;
            Status = "Подготовка...";
            errCol.Clear();
        }
    }
}
