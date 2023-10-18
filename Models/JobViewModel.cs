using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dispetcher2.Class;
using System.Windows.Input;
using System.Windows.Controls;
using System.ComponentModel;

namespace Dispetcher2.Models
{
    public class JobView : Job, INotifyPropertyChanged
    {
        string opn;
        public event PropertyChangedEventHandler PropertyChanged;
        public string OperationName
        {
            get { return opn; }
            set
            {
                opn = value;
                OnPropertyChanged(nameof(OperationName));
            }
        }
        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public JobView(Job j)
        {
            this.Id = j.Id;
            this.Name = j.Name;
            this.OperationGroupId = j.OperationGroupId;
            this.Valid = j.Valid;
            this.OperationName = String.Empty;
        }
    }
    public class OperationGroupView : OperationGroup
    {
        public ICommand MenuCommand { get; set; }
        public OperationGroupView(OperationGroup group)
        {
            this.Id = group.Id;
            this.Name = group.Name;
        }
    }
    public class JobViewModel
    {
        ICommand cmd;
        JobRepository jobrep;
        OperationGroupRepository groups;
        public IEnumerable<JobView> Jobs { get; set; }
        public IEnumerable<OperationGroupView> Groups { get; set; }
        public JobView SelectedItem { get; set; }


        public JobViewModel(JobRepository jobrep, OperationGroupRepository groups)
        {
            this.jobrep = jobrep;
            this.groups = groups;

            var r = new OperationCommand();
            r.ExecuteEvent = ProcessExecuteEvent;
            cmd = r;

            Load();
        }
        void Load()
        {
            List<JobView> a = new List<JobView>();
            foreach(Job j in jobrep.JobRead())
            {
                int gid = j.OperationGroupId;
                JobView jv = new JobView(j);
                a.Add(jv);

                var e = groups.GetGroups().Where(item => item.Id == gid);
                foreach (var g in e) jv.OperationName = g.Name;
            }
            this.Jobs = a;


            //this.Groups = groups.GetGroups();
            var b = new List<OperationGroupView>();
            foreach(var g in groups.GetGroups())
            {
                OperationGroupView v = new OperationGroupView(g);
                v.MenuCommand = cmd;
                b.Add(v);
            }
            this.Groups = b;
        }

        void ProcessExecuteEvent(OperationGroup group)
        {
            if (SelectedItem != null && group != null)
            {
                SelectedItem.OperationGroupId = group.Id;
                SelectedItem.OperationName = group.Name;
                //System.Diagnostics.Debug.Write(SelectedItem.ToString() + "=");
                //System.Diagnostics.Debug.WriteLine(group);
                jobrep.JobUpdate(SelectedItem);
            }
            
        }
    }

    public class OperationCommand : ICommand
    {
        public Action<OperationGroup> ExecuteEvent { get; set; }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is MenuItem)
            {
                MenuItem item = parameter as MenuItem;
                if (item.DataContext is OperationGroup)
                {
                    OperationGroup g = item.DataContext as OperationGroup;
                    
                    if (ExecuteEvent != null) ExecuteEvent(g);
                }
            }
        }
    }
}
