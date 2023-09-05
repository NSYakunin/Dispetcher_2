using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Dispetcher2.Class
{
    public static class Converter
    {
        public static int GetInt(object value)
        {
            if (value == null) return 0;
            if (value is DBNull) return 0;
            int number;
            bool f = Int32.TryParse(value.ToString(), System.Globalization.NumberStyles.Any,
                NumberFormatInfo.InvariantInfo, out number);
            if (f == true) return number;
            else return 0;
        }
        public static long GetLong(object value)
        {
            if (value == null) return 0;
            if (value is DBNull) return 0;
            long number;
            bool f = Int64.TryParse(value.ToString(), System.Globalization.NumberStyles.Any,
                NumberFormatInfo.InvariantInfo, out number);
            if (f == true) return number;
            else return 0;
        }
        public static string GetString(object value)
        {
            if (value == null) return String.Empty;
            if (value is DBNull) return String.Empty;
            return Convert.ToString(value);
        }
        public static TimeSpan GetTime(object value)
        {
            var time = TimeSpan.Zero;
            if (value is DBNull) return time;
            string s = Convert.ToString(value).Trim();
            if (s.Length < 1) return time;
            if (s.Contains("?")) return time;
            if (s.Contains(":") == false) return time;
            string[] sa = s.Split(':');
            if (sa.Length != 3) return time;
            int h = GetInt(sa[0]);
            int m = GetInt(sa[1]);
            int sec = GetInt(sa[2]);
            time = new TimeSpan(h, m, sec);
            return time;
        }
        public static DateTime GetDateTime(object value)
        {
            if (value == null) return DateTime.MinValue;
            if (value is DBNull) return DateTime.MinValue;

            string s = Convert.ToString(value);
            DateTime d;
            bool b = DateTime.TryParse(s, out d);
            if (b == true) return d;
            else return DateTime.MinValue;
        }
        public static bool GetBool(object value)
        {
            if (value == null) return false;
            if (value is DBNull) return false;
            // тип данных SQL: BIT
            int i = Convert.ToInt32(value);
            if (i == 0) return false;
            else return true;
        }
    }
}
