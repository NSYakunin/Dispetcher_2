using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace Dispetcher2.Class
{
    public class ProgressViewModel : INotifyPropertyChanged
    {
        string wayToFolder = @"\\Ascon\DispImport";
        string wayToFolderArchive = @"\\Ascon\DispImport\Архив";
        double progress = 0;
        string status = "Подготовка...";
        BindingList<ErrorItem> errblist = new BindingList<ErrorItem>();

        Dispatcher disp = null;
        delegate void ErrorHandler(ErrorItem ei);
        ErrorHandler eh;

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

        public string WayToFolder
        {
            get { return wayToFolder; }
            set
            {
                wayToFolder = value;
                OnPropertyChanged("WayToFolder");
            }
        }

        public string WayToFolderArchive
        {
            get { return wayToFolderArchive; }
            set
            {
                wayToFolderArchive = value;
                OnPropertyChanged("WayToFolderArchive");
            }
        }

        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public BindingList<ErrorItem> ErrorList
        {
            get { return errblist; }
        }

        public void Reset()
        {
            Progress = 0;
            Status = "Подготовка...";
            errblist.Clear();
        }

        // Элемент управления ListBox нельзя обновлять из другого потока
        // Пришлось использовать делигат
        public void SetDispatcher(Dispatcher disp)
        {
            this.disp = disp;
            eh = (ErrorItem e) => errblist.Add(e);
        }

        public void AddToList(ErrorItem ei)
        {
            ErrorItem[] a = new ErrorItem[] { ei };

            if (disp != null) disp.BeginInvoke(eh, a);
            else eh(ei);
        }
    }
}
