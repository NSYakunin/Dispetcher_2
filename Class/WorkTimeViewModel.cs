using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Dispetcher2.Class
{
    public class WorkTimeViewModel
    {
        TimeSpan total;
        TimeSpan past;
        
        public WorkTimeViewModel(TimeSpan total, TimeSpan past)
        {
            this.total = total;
            this.past = past;
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
    }
}
