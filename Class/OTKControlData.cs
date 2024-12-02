using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispetcher2.Class
{
    public class OTKControlData : ICloneable, INotifyPropertyChanged
    {
        private CheckBoxState[] _states;
        public CheckBoxState[] States
        {
            get => _states;
            set
            {
                _states = value;
                OnPropertyChanged(nameof(States));
            }
        }

        private string _note;
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged(nameof(Note));
            }
        }

        private DateTime _changeDate;
        public DateTime ChangeDate
        {
            get => _changeDate;
            set
            {
                _changeDate = value;
                OnPropertyChanged(nameof(ChangeDate));
            }
        }

        private string _user;
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public OTKControlData()
        {
            States = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
            Note = string.Empty;
            ChangeDate = DateTime.MinValue;
            User = string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object Clone()
        {
            return new OTKControlData
            {
                States = (CheckBoxState[])this.States.Clone(),
                Note = this.Note,
                ChangeDate = this.ChangeDate,
                User = this.User
            };
        }
    }
}
