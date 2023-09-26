using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Dispetcher2.Class
{
    public interface IConverter
    {
        CultureInfo ContextCulture { get; set; }
        bool CheckConvert<T>(object value);
        T Convert<T>(object value);
    }
    public class Converter : IConverter
    {
        CultureInfo c = null;
        
        public CultureInfo ContextCulture
        {
            get
            {
                if (c == null) c = CultureInfo.CurrentCulture;
                return c;
            }
            set
            {
                c = value;
            }
        }
        /// <summary>
        /// Проверяет возможность преобразования value в тип T
        /// </summary>
        /// <typeparam name="T">Целевой тип</typeparam>
        /// <param name="value">Исходная величина</param>
        /// <returns></returns>
        public bool CheckConvert<T>(object value)
        {
            if (value == null) return false;
            if (value is DBNull) return false;
            if (typeof(T) == value.GetType())
            {
                return true;
            }

            if (typeof(T) == typeof(string)) return true;

            string s = Convert<String>(value).Trim();

            if (typeof(T) == typeof(bool))
            {
                return CheckConvert<Int32>(value);
            }

            if (typeof(T) == typeof(DateTime))
            {
                DateTime i;
                return DateTime.TryParse(s, ContextCulture, DateTimeStyles.None, out i);
            }

            if (typeof(T) == typeof(Int32))
            {
                Int32 i;
                return Int32.TryParse(s, NumberStyles.Any, ContextCulture, out i);
            }

            if (typeof(T) == typeof(Int64))
            {
                Int64 i;
                return Int64.TryParse(s, NumberStyles.Any, ContextCulture, out i);
            }

            if (typeof(T) == typeof(Decimal))
            {
                Decimal i;
                return Decimal.TryParse(s, NumberStyles.Any, ContextCulture, out i);
            }

            if (typeof(T) == typeof(Double))
            {
                Double i;
                return Double.TryParse(s, NumberStyles.Any, ContextCulture, out i);
            }

            if (typeof(T) == typeof(TimeSpan))
            {
                if (s.Length < 1) return false;
                if (s.Contains("?")) return false;
                if (s.Contains(":") == false) return false;
                string[] sa = s.Split(':');
                if (sa.Length != 3) return false;
                bool b0 = CheckConvert<int>(sa[0]);
                bool b1 = CheckConvert<int>(sa[1]);
                bool b2 = CheckConvert<int>(sa[2]);
                return b0 && b1 && b2;
            }

            throw new NotImplementedException($"Работа с типом {typeof(T)} пока не реализована");
        }
        /// <summary>
        /// Преобразует value в тип T. Предполагается, что сначала был успешный вызов CheckConvert, иначе возможна генерация исключения
        /// </summary>
        /// <typeparam name="T">Целевой тип</typeparam>
        /// <param name="value">Исходная величина</param>
        /// <returns></returns>
        public T Convert<T>(object value)
        {
            if (typeof(T) == value.GetType())
            {
                return (T)value;
            }

            if (typeof(T) == typeof(String))
            {
                object i = System.Convert.ToString(value, ContextCulture);
                return (T)i;
            }

            if (typeof(T) == typeof(bool))
            {
                int i = Convert<int>(value);
                if (i == 0)
                {
                    object b = false;
                    return (T)b;
                }
                else
                {
                    object b = true;
                    return (T)b;
                }
            }

            string s = Convert<string>(value);

            if (typeof(T) == typeof(DateTime))
            {
                object i = DateTime.Parse(s, ContextCulture);
                return (T)i;
            }

            if (typeof(T) == typeof(Int32))
            {
                object i = Int32.Parse(s, NumberStyles.Any, ContextCulture);
                return (T)i;
            }

            if (typeof(T) == typeof(Int64))
            {
                object i = Int64.Parse(s, NumberStyles.Any, ContextCulture);
                return (T)i;
            }

            if (typeof(T) == typeof(Decimal))
            {
                object i = Decimal.Parse(s, NumberStyles.Any, ContextCulture);
                return (T)i;
            }

            if (typeof(T) == typeof(Double))
            {
                object i = Double.Parse(s, NumberStyles.Any, ContextCulture);
                return (T)i;
            }

            if (typeof(T) == typeof(TimeSpan))
            {
                string[] sa = s.Split(':');
                int hou = Convert<int>(sa[0]);
                int min = Convert<int>(sa[1]);
                int sec = Convert<int>(sa[2]);
                object i = new TimeSpan(hou, min, sec);
                return (T)i;
            }

            throw new NotImplementedException($"Работа с типом {typeof(T)} пока не реализована");
        }
    }
}
