using System;
using System.Collections.Generic;
using System.Linq;

namespace Dispetcher2.Class
{
    public abstract class LaborReportWriter
    {
        public abstract void Write(IEnumerable<string> columns, IEnumerable<LaborReportRow> rows, string H1, string H2);
    }
    public class LaborReportRow
    {
        // имя строки
        public string Name { get; set; }
        // для вывода в интерфейс пользователя
        // ключ словаря: имя операции
        // значение словаря: текст в ячейке
        public Dictionary<string, string> Operations { get; set; }
        // словарь операций данной строки отчета
        // ключ словаря: имя операции
        // значение словаря: список операций или рабочих дней
        public Dictionary<string, List<WorkTime>> TimeDictionary { get; set; }

        public LaborReportRow()
        {
            Operations = new Dictionary<string, string>();
            TimeDictionary = new Dictionary<string, List<WorkTime>>();
        }
        public void AddWorkTime(string name, WorkTime value)
        {
            if (TimeDictionary.ContainsKey(name) == false) TimeDictionary[name] = new List<WorkTime>();
            TimeDictionary[name].Add(value);
        }
        public TimeSpan GetTimeSpan(string name)
        {
            TimeSpan t = TimeSpan.Zero;
            if (TimeDictionary.ContainsKey(name))
            {
                if (TimeDictionary[name] != null)
                {
                    foreach(var item in TimeDictionary[name])
                    {
                        if (item is WorkDay) t = t.Add((item as WorkDay).Time);
                        if (item is Operation) t = t.Add((item as Operation).Time);
                    }
                }
            }
            return t;
        }
    }

    public class LaborReport
    {
        const string quantityName = "Кол-во работников за год";
        const string timeSheetName = "Табельное время за год";
        const string restName = "Остаток на указанную дату";
        const string limitName = "Лимит трудоемкости";
        const string limitCoopName = "Лимит трудоемкости без кооперации";
        const string itogoName = "Итого";
        const string productivityName = "Выработка, %";
        const string coopName = "Кооперация";
        const string coopLogin = "кооп";
        const string factName = "Израсходовано";
        public OrderRepository SelectedOrders { get; set; }
        public bool ShowDetailFlag { get; set; }
        public bool ShowOperationFlag { get; set; }
        public bool AllOrdersFlag { get; set; }
        // Дата начала не нужна. В отчете учитываются все фактические операции до EndDate
        //public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        
        DateTime yearBeginDate { get { return new DateTime(EndDate.Year, 1, 1); } }
        DateTime yearEndDate { get { return new DateTime(EndDate.Year + 1, 1, 1); } }

        List<LaborReportRow> rows;
        // Внешние зависимости
        DetailRepository details;
        Dictionary<long, Detail> detailDict;
        OperationRepository operations;
        OperationGroupRepository groups;
        // словарь операций по коду
        Dictionary<int, OperationGroup> opgDict;
        WorkDayRepository workDays;

        Dictionary<int, Order> allOrderDict;
        List<Order> factOrders;
        //LaborReportRow factByOperation;
        public LaborReport(DetailRepository details, OperationRepository operations, OperationGroupRepository groups, 
            WorkDayRepository workDays, OrderRepository allOrders)
        {
            if (details == null) throw new ArgumentException("Пожалуйста укажите параметр: DetailRepository");
            if (operations == null) throw new ArgumentException("Пожалуйста укажите параметр: OperationRepository");
            if (groups == null) throw new ArgumentException("Пожалуйста укажите параметр: OperationGroupRepository");
            if (workDays == null) throw new ArgumentException("Пожалуйста укажите параметр: WorkDayRepository");
            
            if (allOrders == null) throw new ArgumentException("Пожалуйста укажите параметр: allOrders");

            this.details = details;
            this.operations = operations;
            this.groups = groups;
            this.workDays = workDays;

            allOrderDict = new Dictionary<int, Order>();
            foreach (var o in allOrders.GetOrders())
            {
                allOrderDict[o.Id] = o;
            }

            rows = new List<LaborReportRow>();

            //factByOperation = new LaborReportRow() { Name = "Фактическое время по операциям" };
        }
        string Format(TimeSpan ts)
        {
            string s = $"{(int)ts.TotalHours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            return s;
        }
        public void Load()
        {
            details.Load();
            detailDict = new Dictionary<long, Detail>();
            foreach (var item in details.GetDetails()) detailDict[item.OrderDetailId] = item;
            operations.Load();
            groups.Load();
            opgDict = new Dictionary<int, OperationGroup>();
            foreach (var item in groups.GetGroups()) opgDict[item.Id] = item;
        }

        private void ProcessEmployeeTime()
        {
            // фильтруем рабочие дни по дате
            // Группировать данные в словарь списков по коду группы операции
            // Группировать данные в словарь операция-сотрудники
            var wdgroups = workDays.GetWorkDays(yearBeginDate, yearEndDate).GroupBy(item => item.OperationGroupId);
            // Ключ: код операции, значение: список рабочих дней
            var wdDic = new Dictionary<int, List<WorkDay>>();
            // Ключ: код операции, значение: уникальный список сотрудников
            var empDic = new Dictionary<int, HashSet<Employee>>();

            foreach (var g in wdgroups)
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

            var dv = new LaborReportRow() { Name = quantityName };
            // Всего, количество работников
            int totalCount = 0;

            foreach (var item in opgDict)
            {
                OperationGroup op = item.Value;
                if (empDic.ContainsKey(op.Id))
                {
                    dv.Operations[op.Name] = empDic[op.Id].Count().ToString();
                    totalCount = totalCount + empDic[op.Id].Count();
                }
            }
            dv.Operations[restName] = totalCount.ToString();
            this.rows.Add(dv);

            dv = new LaborReportRow() { Name = timeSheetName };
            // Всего, табельное время
            TimeSpan totalTime = TimeSpan.Zero;
            
            foreach (var item in opgDict)
            {
                OperationGroup op = item.Value;
                if (wdDic.ContainsKey(op.Id))
                {
                    string name = op.Name;
                    TimeSpan time = TimeSpan.Zero;
                    
                    foreach (var day in wdDic[op.Id])
                    {
                        if (day.Time > TimeSpan.Zero)
                        {
                            time = time.Add(day.Time);
                            dv.AddWorkTime(name, day);
                            dv.AddWorkTime(restName, day);
                        }
                    }
                    dv.Operations[op.Name] = Format(time);
                    totalTime = totalTime.Add(time);
                }
            }
            dv.Operations[restName] = Format(totalTime);
            
            this.rows.Add(dv);
        }
        void ProcessDetailOperations()
        {
            var oe = SelectedOrders.GetOrders();
            if (AllOrdersFlag)
            {
                //CalculateFactOrders();
                
                factOrders = new List<Order>();
                foreach (var p in allOrderDict) factOrders.Add(p.Value);
                oe = factOrders;
            }

            if (ShowDetailFlag == true)
            {
                // Операции по деталям
                // Список деталей, относящихся к указанным заказам
                List<Detail> e = new List<Detail>();
                foreach (var o in oe)
                {
                    var od = from item in details.GetDetails()
                             where item.OrderId == o.Id
                             select item;
                    foreach (var d in od) e.Add(d);
                }
                foreach (var d in e)
                {
                    // количество "родителей" по атрибуту "входит в..."
                    int parents = e.Where(item => item.Position == d.PositionParent).Count();
                    if (d.PositionParent == 0 || parents == 0) ProcessMainDetail(d);
                }
            }
            else
            {
                // Операции по заказам
                foreach (var o in oe)
                {
                    ProcessOrder(o);
                }
            }
            
        }
        void ProcessMainDetail(Detail d)
        {
            // все операции
            var e3 = operations.GetOperations();
            // Для каждой главной детали формируется “древо” деталей по атрибуту "входит в..."
            var e4 = details.GetTree(d);

            List<Operation> allTreeOperations = new List<Operation>();
            foreach (var i in e4)
            {
                // Для каждой детали древа формируется список операций
                var e5 = operations.GetOperations().Where(op => op.OrderDetailId == i.OrderDetailId);
                foreach (var e5i in e5) allTreeOperations.Add(e5i);
            }
            // Если нет операций, то не выводим деталь?
            //if (allTreeOperations.Count == 0) return;

            var dv = new LaborReportRow() { Name = d.Name };
            this.rows.Add(dv);

            ProcessRow(dv, allTreeOperations);
        }
        void ProcessOrder(Order item)
        {
            // Список деталей этого заказа
            var od = from d in details.GetDetails()
                     where d.OrderId == item.Id
                     select d;

            // список операций этого заказа
            List<Operation> itemOperations = new List<Operation>();
            foreach (var d in od)
            {
                // Для каждой детали формируется список операций
                var e5 = operations.GetOperations().Where(op => op.OrderDetailId == d.OrderDetailId);
                foreach (var e5i in e5) itemOperations.Add(e5i);
            }

            var dv = new LaborReportRow() { Name = item.Number };
            this.rows.Add(dv);

            ProcessRow(dv, itemOperations);
        }
        void ProcessRow(LaborReportRow dv, List<Operation> dvOperations)
        {
            TimeSpan totalFactTime = TimeSpan.Zero;
            TimeSpan totalPlanTime = TimeSpan.Zero;
            TimeSpan totalCoopTime = TimeSpan.Zero;
            

            if (ShowOperationFlag == true)
            {
                // Подробно по операциям
                // Операции всех деталей в древе группируются по имени
                var dvoGroups = dvOperations.GroupBy(o => o.Name);
                foreach (var g in dvoGroups)
                {

                    // Ключ словаря: имя операции. В итоге выводится в имени столбца DataGrid
                    string name = g.Key;
                    // Перечисление всех элементов группы
                    // В группу входят все операции с одним именем
                    TimeSpan planTime = TimeSpan.Zero;
                    TimeSpan factTime = TimeSpan.Zero;
                    TimeSpan coopTime = TimeSpan.Zero;
                    
                    foreach (var op in g)
                    {
                        // фактические операции
                        if (op.TypeRow == "3fact" && op.Login != coopLogin)
                        {
                            //if (yearBeginDate <= op.FactDate && op.FactDate < yearEndDate)
                            //{
                            //    factByOperation.AddWorkTime(name, op);
                            //}
                            if (op.FactDate < EndDate)
                            {
                                factTime = factTime.Add(op.Time);
                                dv.AddWorkTime(factName, op);
                                //dv.AddWorkTime(restName, op);
                            }
                        }
                        // плановые операции
                        if (op.TypeRow == "1sp" || op.TypeRow == "2sp111")
                        {
                            planTime = planTime.Add(op.Time);
                            dv.AddWorkTime(limitName, op);
                        }
                        // кооперация, фактически, вне зависимости от даты
                        if (op.TypeRow == "3fact" && op.Login == coopLogin)
                        {
                            coopTime = coopTime.Add(op.Time);
                            dv.AddWorkTime(coopName, op);
                        }
                    }
                    // это неправильно, смотри ТЗ
                    //if (factTime > TimeSpan.Zero)
                    //dv.Operations[name] = Format(factTime);
                    //dv.TimeDictionary[name] = factTime;

                    // выводим остаток по операциям
                    var ts = planTime.Subtract(factTime).Subtract(coopTime);
                    if (ts > TimeSpan.Zero)
                    {
                        dv.Operations[name] = Format(ts);
                        // Фиктивная "операция" для name, нужна для попадания в "Итого"
                        Operation x = new Operation() { Name = name, Time = ts, };
                        dv.AddWorkTime(name, x);
                    }

                    totalFactTime = totalFactTime.Add(factTime);
                    totalPlanTime = totalPlanTime.Add(planTime);
                    totalCoopTime = totalCoopTime.Add(coopTime);
                }
            }
            else
            {
                // Операции в группах
                // Для каждой группы
                foreach (var kv in opgDict)
                {
                    var g = kv.Value;
                    string name = g.Name;
                    // Получаем операции
                    var se = from item in dvOperations
                             where item.GroupId == g.Id
                             select item;
                    if (se.Any() == false) continue;
                    // Если есть операции, считаем план и факт
                    TimeSpan planTime = TimeSpan.Zero;
                    TimeSpan factTime = TimeSpan.Zero;
                    TimeSpan coopTime = TimeSpan.Zero;

                    foreach (var op in se)
                    {
                        // фактические операции
                        if (op.TypeRow == "3fact" && op.Login != coopLogin)
                        {
                            //if (yearBeginDate <= op.FactDate && op.FactDate < yearEndDate)
                            //{
                            //    factByOperation.AddWorkTime(name, op);
                            //}
                            if (op.FactDate < EndDate)
                            {
                                factTime = factTime.Add(op.Time);
                                dv.AddWorkTime(factName, op);
                                //dv.AddWorkTime(restName, op);
                            }
                        }
                        // плановые операции
                        if (op.TypeRow == "1sp" || op.TypeRow == "2sp111")
                        {
                            planTime = planTime.Add(op.Time);
                            dv.AddWorkTime(limitName, op);
                            continue;
                        }
                        // кооперация, фактически, вне зависимости от даты
                        if (op.TypeRow == "3fact" && op.Login == coopLogin)
                        {
                            coopTime = coopTime.Add(op.Time);
                            dv.AddWorkTime(coopName, op);
                            continue;
                        }
                        

                    }

                    // это неправильно, смотри ТЗ
                    //if (factTime > TimeSpan.Zero)
                    //dv.Operations[name] = Format(factTime);
                    //dv.TimeDictionary[name] = factTime;

                    // выводим остаток по операциям
                    var ts = planTime.Subtract(factTime).Subtract(coopTime);
                    if (ts > TimeSpan.Zero)
                    {
                        dv.Operations[name] = Format(ts);
                        // Фиктивная "операция" для name, нужна для попадания в "Итого"
                        Operation x = new Operation() { Name = name, Time = ts, };
                        dv.AddWorkTime(name, x);
                    }

                    totalFactTime = totalFactTime.Add(factTime);
                    totalPlanTime = totalPlanTime.Add(planTime);
                    totalCoopTime = totalCoopTime.Add(coopTime);
                }
            }
            // цитата из ТЗ:
            // 2.7 Лимит трудоемкости, н/час
            if (totalPlanTime > TimeSpan.Zero)
            {
                dv.Operations[limitName] = Format(totalPlanTime);
            }
            // 2.8 Трудоёмкость по кооперации, н/час
            if (totalCoopTime > TimeSpan.Zero)
            {
                dv.Operations[coopName] = Format(totalCoopTime);
            }
            // 2.9 Лимит трудоёмкости с учетом кооперации (рассчитывается как разница п.2.7-п.2.8);
            var planSubtractCoop = totalPlanTime.Subtract(totalCoopTime);
            //if (planSubtractCoop > TimeSpan.Zero)
            {
                dv.Operations[limitCoopName] = Format(planSubtractCoop);
                // Фиктивная "операция" для limitCoopName, нужна для попадания в "Итого"
                Operation x = new Operation() { Name = limitCoopName, Time = planSubtractCoop };
                dv.AddWorkTime(limitCoopName, x);
            }
            // 2.10 Израсходовано на 01 число месяца (на любую выбранную дату), следующего за отчетным
            if (totalFactTime > TimeSpan.Zero)
            {
                dv.Operations[factName] = Format(totalFactTime);
            }
            // 2.11 Остаток на 01 число месяца и /или на любую выбранную дату (рассчитывается как разница п.2.9-п.2.10);
            //ts = totalPlanTime.Subtract(totalFactTime);
            var planRest = planSubtractCoop.Subtract(totalFactTime);
            if (planRest >= TimeSpan.Zero)
            {
                dv.Operations[restName] = Format(planRest);
                // Фиктивная "операция" для restName, нужна для попадания в "Итого"
                Operation x = new Operation() { Name = restName, Time = planRest };
                dv.AddWorkTime(restName, x);
            }
            
        }
        public IEnumerable<string> GetColumns()
        {
            HashSet<string> NameList = new HashSet<string>();
            // цитата из ТЗ:
            // 2.7 Лимит трудоемкости, н/час
            NameList.Add(limitName);
            // 2.8 Трудоёмкость по кооперации, н/час
            NameList.Add(coopName);
            // 2.9 Лимит трудоёмкости с учетом кооперации (рассчитывается как разница п.2.7-п.2.8);
            NameList.Add(limitCoopName);
            // 2.10 Израсходовано на 01 число месяца (на любую выбранную дату), следующего за отчетным
            NameList.Add(factName);
            // 2.11 Остаток на 01 число месяца и /или на любую выбранную дату (рассчитывается как разница п.2.9-п.2.10);
            NameList.Add(restName);
            // 2.12 Пооперационно расшифровать пункт 2.11
            foreach (var item in rows)
            {
                foreach (var k in item.Operations.Keys)
                    NameList.Add(k);
            }

            return NameList;
        }
        public void Calculate()
        {
            rows.Clear();
            if (ShowOperationFlag == false)
            {
                ProcessEmployeeTime();
            }
            ProcessDetailOperations();
            ProcessTotal();
        }
        void ProcessTotal()
        {
            double t1, t2;

            LaborReportRow tsrow = null;
            // Считаем итого
            LaborReportRow itog = new LaborReportRow() { Name = itogoName };
            foreach (var dv in rows)
            {
                if (dv.Name.Equals(quantityName)) continue;
                if (dv.Name.Equals(timeSheetName)) 
                { 
                    tsrow = dv; 
                    continue; 
                }
                foreach (var name in dv.TimeDictionary.Keys)
                {
                    //if (itog.TimeDictionary.ContainsKey(name) == false)
                    //    itog.TimeDictionary[name] = TimeSpan.Zero;
                    //itog.TimeDictionary[name] = itog.TimeDictionary[name].Add(dv.TimeDictionary[name]);

                    
                    foreach (var item in dv.TimeDictionary[name])
                    {
                        itog.AddWorkTime(name, item);
                    }
                }
            }
            foreach (var name in itog.TimeDictionary.Keys)
            {
                //if (itog.TimeDictionary[name] > TimeSpan.Zero)
                //    itog.Operations[name] = Format(itog.TimeDictionary[name]);
                var t = itog.GetTimeSpan(name);
                if (t > TimeSpan.Zero) itog.Operations[name] = Format(t);
            }
            

            rows.Add(itog);
            // Считаем выработку
            //double total1 = 0;
            //double total2 = 0;
            LaborReportRow vrow = new LaborReportRow() { Name = productivityName };
            if (tsrow != null)
            {
                foreach(var name in itog.TimeDictionary.Keys)
                {
                    if (tsrow.TimeDictionary.ContainsKey(name))
                    {
                        //double t1 = itog.GetTimeSpan(name).TotalHours;
                        //double t1 = factByOperation.GetTimeSpan(name).TotalHours;

                        //double t2 = tsrow.GetTimeSpan(name).TotalHours;
                        //double v = 100.0 * t1 / t2;
                        t1 = tsrow.GetTimeSpan(name).TotalHours;
                        t2 = itog.GetTimeSpan(name).TotalHours;

                        // ТЗ:
                        // Выработка (%) определяется по формуле:
                        // Выработка (%) = (100%) * (Годовая выработка) / (Итого)
                        // Годовая выработка – годовая норма производительности труда работников цеха
                        // Итого - фактическая производительность труда работников цеха

                        double v = 100.0 * t1 / t2;
                        vrow.Operations[name] = v.ToString("0.00") + "%";
                        //total1 += t1;
                        //total2 += t2;
                    }
                }
            }
            //double totalv = 100.0 * total1 / total2;
            //vrow.Operations[factName] = totalv.ToString("0.00") + "%";
            rows.Add(vrow);
        }
        public IEnumerable<LaborReportRow> GetRows()
        {
            return rows;
        }
        // Определяем в каких заказах есть факты для указанного диаппазона дат
        //void CalculateFactOrders()
        //{
        //    HashSet<int> factOrderIdset = new HashSet<int>();

        //    foreach (var op in operations.GetOperations())
        //    {
        //        // фактические операции
        //        if (op.TypeRow == "3fact" && op.FactDate < EndDate)
        //        {
        //            factOrderIdset.Add(op.OrderId);
        //        }
        //    }

        //    factOrders = new List<Order>();
        //    foreach (var item in factOrderIdset)
        //    {
        //        if (allOrderDict.ContainsKey(item)) factOrders.Add(allOrderDict[item]);
        //    }
        //}
        public LaborReportRow GetReportRow(WorkDay item)
        {
            LaborReportRow r = new LaborReportRow();
            r.Name = item.Date.ToShortDateString();
            r.Operations["Логин"] = item.Login;
            r.Operations["Фамилия"] = item.LastName;
            r.Operations["Имя"] = item.Name;
            r.Operations["Отчество"] = item.SecondName;
            r.Operations["Специальность"] = item.JobName;
            if (opgDict.ContainsKey(item.OperationGroupId))
                r.Operations["Операция"] = opgDict[item.OperationGroupId].Name;
            r.Operations["Время"] = item.TimeString;
            return r;
        }
        public LaborReportRow GetReportRow(Operation item)
        {
            LaborReportRow r = new LaborReportRow();
            if (allOrderDict.ContainsKey(item.OrderId))
                r.Name = allOrderDict[item.OrderId].Number;
            if (detailDict.ContainsKey(item.OrderDetailId))
                r.Operations["ЩЦМ"] = detailDict[item.OrderDetailId].Shcm;
            
            r.Operations["Операция"] = item.Name;
            if (opgDict.ContainsKey(item.GroupId))
                r.Operations["Группа"] = opgDict[item.GroupId].Name;
            if (item.FactDate > DateTime.MinValue) r.Operations["Дата"] = item.FactDate.ToShortDateString();
            if (item.Login != null) r.Operations["Логин"] = item.Login;
            r.Operations["Кол-во"] = item.Quantity.ToString();
            r.Operations["TPD"] = item.Tpd.ToString();
            r.Operations["TSH"] = item.Tsh.ToString();
            r.Operations["Время"] = Format(item.Time);
            return r;
        }
    }
}
