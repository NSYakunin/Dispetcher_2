﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Dispetcher2.Class
{
    /// <summary>
    /// Рабочий день
    /// </summary>
    public class WorkDay : WorkTime
    {
        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Количество рабочего времени
        /// </summary>
        public string Login { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        // может быть было бы лучше включить объект Job?
        public string JobName { get; set; }
        public int OperationGroupId { get; set; }
        public string TabNum { get; set; }
        public bool ITR { get; set; }
        public string TimeString { get; set; }

    }
    /// <summary>
    /// Хранилище рабочих дней
    /// </summary>
    public abstract class WorkDayRepository
    {
        public abstract IEnumerable<WorkDay> GetWorkDays(DateTime beginDate, DateTime endDate);
    }
}
