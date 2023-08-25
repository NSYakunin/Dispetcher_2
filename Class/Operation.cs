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
        public int GroupId { get; set; }
        /// <summary>
        /// Предварительно-заключительное время в секундах
        /// </summary>
        public int Tpd { get; set; }
        /// <summary>
        /// Время на деталь в секундах
        /// </summary>
        public int Tsh { get; set; }
        public int Quantity { get; set; }
        public string Number { get; set; }


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

        public void AddFactTime(Operation item)
        {
            /*
            В настоящее время, 24.08.2023 техлогическая операция в Лоцмане
            хранится так, что "время на деталь" содержит сумму основного и предварительно-заключительного.
            Причина: работа связки Лоцман-Вертикаль.
            Трудоемкость следует считать так:
            Т = К * (Tsh - Tpz) + Tpz
            */
            int t;
            if (item.Tsh > item.Tpd) t = item.Quantity * (item.Tsh - item.Tpd) + item.Tpd;
            else t = item.Quantity * item.Tsh + item.Tpd;
            TimeSpan ts = TimeSpan.FromSeconds(t);
            this.Time = this.Time.Add(ts);
        }
    }
}
