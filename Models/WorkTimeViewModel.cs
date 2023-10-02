using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Dispetcher2.Class;

namespace Dispetcher2.Models
{
    public class WorkTimeReportRow
    {
        public string Name { get; set; }
        public Dictionary<string, string> Operations { get; set; }

        public WorkTimeReportRow()
        {
            Operations = new Dictionary<string, string>();
        }
    }
    public class WorkTimeViewModel
    {
        List<OperationReportRow> rows;
        StringRepository columns;
        WorkDayRepository rep;
        OperationGroupRepository opgroups;
        TimeSpan total = TimeSpan.Zero;
        TimeSpan past = TimeSpan.Zero;

        public IEnumerable<OperationReportRow> Rows { get { return rows; } }

        public WorkTimeViewModel(WorkDayRepository rep, OperationGroupRepository opgroups)
        {
            if (rep == null) throw new Exception("Пожалуйста предоставьте параметр WorkDayRepository");
            if (opgroups == null) throw new Exception("Пожалуйста предоставьте параметр OperationGroupRepository");
            this.rep = rep;
            this.opgroups = opgroups;
        }
        string Format(TimeSpan ts)
        {
            string s = $"{(int)ts.TotalHours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            return s;
        }
        public void Load()
        {
            this.total = rep.GetTotalTime();
            this.past = rep.GetPastTime();

            // словарь операций по коду
            var opDic = new Dictionary<int, OperationGroup>();
            foreach (var item in opgroups.GetGroups()) opDic[item.Id] = item;

            

            var allWorkDays = rep.GetWorkDays();
            
            var groups = allWorkDays.GroupBy(item => item.OperationGroupId);
            //Группировать данные в словарь списков по коду группы операции. Ключ: код операции, значение: список рабочих дней
            var wdDic = new Dictionary<int, List<WorkDay>>();
            // Собрать данные в словарь операция-сотрудники. Ключ: код операции, значение: уникальный список сотрудников
            var empDic = new Dictionary<int, HashSet<Employee>>();
            
            foreach (var g in groups)
            {
                int id = g.Key;
                if (id == 0) continue;


                wdDic[id] = new List<WorkDay>();
                empDic[id] = new HashSet<Employee>();
                foreach (var item in g)
                {
                    wdDic[id].Add(item);
                    Employee e = new Employee();
                    e.ITR = item.ITR;
                    e.LastName = item.LastName;
                    e.Login = item.Login;
                    e.Name = item.Name;
                    e.SecondName = item.SecondName;
                    e.TabNum = item.TabNum;
                    if (empDic[id].Contains(e) == false) empDic[id].Add(e);
                }
            }
            

            this.rows = new List<OperationReportRow>();

            var dv = new OperationReportRow() { Name = "Итого (за год)" };
            dv.Operations = new Dictionary<string, string>();

            HashSet<string> NameList = new HashSet<string>();
            foreach (var item in opDic)
            {
                string s1 = String.Empty;
                string s2 = String.Empty;
                OperationGroup op = item.Value;
                if (empDic.ContainsKey(op.Id))
                {
                    s1 = "Рабочие: " + empDic[op.Id].Count();
                }
                if (wdDic.ContainsKey(op.Id))
                {
                    TimeSpan time = TimeSpan.Zero;
                    foreach(var day in wdDic[op.Id])
                    {
                        time = time.Add(day.Time);
                    }
                    s2 = "Время: " + Format(time);
                }
                if (s1.Length > 0 || s2.Length > 0)
                {
                    dv.Operations[op.Name] = s1 + Environment.NewLine + s2;
                    NameList.Add(item.Value.Name);
                }
            }
            this.columns = new StringRepository(NameList);
            this.rows.Add(dv);
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

        public Repository GetOperationRepository()
        {
            return columns;
        }
    }
}
