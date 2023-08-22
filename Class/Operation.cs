using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Dispetcher2.Class
{
    public class Operation
    {
        public int Numcol { get; set; }
        public string Name { get; set; }
        public TimeSpan Time { get; set; }
        public string TimeString
        {
            get
            {
                TimeSpan ts = Time;
                string s = $"{(int)ts.TotalHours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
                return s;
            }
        }

        public void SetName(object value)
        {
            if (value is DBNull) Name = String.Empty;
            else Name = Convert.ToString(value).Trim();
        }

        public void SetTime(object value)
        {
            Time = TimeSpan.Zero;
            if (value is DBNull) return;
            string s = Convert.ToString(value).Trim();
            if (s.Length < 1) return;
            if (s.Contains("?")) return;
            if (s.Contains(":") == false) return;
            string[] sa = s.Split(':');
            if (sa.Length != 3) return;
            int h = C_DataBase.GetInteger(sa[0]);
            int m = C_DataBase.GetInteger(sa[1]);
            int sec = C_DataBase.GetInteger(sa[2]);
            Time = new TimeSpan(h, m, sec);
        }
    }
}
