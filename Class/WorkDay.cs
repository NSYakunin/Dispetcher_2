using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Dispetcher2.Class
{
    /// <summary>
    /// Рабочий день
    /// </summary>
    public class WorkDay
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Количество рабочего времени
        /// </summary>
        public TimeSpan Time { get; set; }
    }
    /// <summary>
    /// Хранилище рабочих дней
    /// </summary>
    public abstract class WorkDayRepository : Repository
    {
        public abstract IEnumerable<WorkDay> GetWorkDays();
        public abstract TimeSpan GetTotalTime();
        public abstract TimeSpan GetPastTime();
    }
}
