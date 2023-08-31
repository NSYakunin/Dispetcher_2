using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Dispetcher2.Class;

namespace Dispetcher2.Models
{
    public class WorkTimeViewModel
    {
        TimeSpan total;
        TimeSpan past;
        
        public WorkTimeViewModel(WorkDayRepository r)
        {
            this.total = r.GetTotalTime();
            this.past = r.GetPastTime();
        }
        public long TotalDays
        {
            get
            {
                var y = total.TotalDays;
                return Convert.ToInt64(y);
            }
        }

        public long PastDays
        {
            get
            {
                var y = past.TotalDays;
                return Convert.ToInt64(y);
            }
        }

        public long RestDays
        {
            get
            {
                TimeSpan r = total.Subtract(past);
                var y = r.TotalDays;
                return Convert.ToInt64(y);
            }
        }

        public long TotalHours
        {
            get
            {
                var y = total.TotalHours;
                return Convert.ToInt64(y);
            }
        }

        public long PastHours
        {
            get
            {
                var y = past.TotalHours;
                return Convert.ToInt64(y);
            }
        }
        public long RestHours
        {
            get
            {
                TimeSpan r = total.Subtract(past);
                var y = r.TotalHours;
                return Convert.ToInt64(y);
            }
        }
    }
}
