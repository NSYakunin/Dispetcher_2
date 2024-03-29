using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

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
        // ключ словаря: имя операции (выводится в заголовке столбца)
        // значение словаря: текст в ячейке
        public Dictionary<string, string> Operations { get; set; }
        // словарь операций данной строки отчета
        // ключ словаря: имя операции
        // значение словаря: список операций или рабочих дней
        public Dictionary<string, List<WorkTime>> TimeDictionary { get; set; }

        public TimeSpan TotalFactTime;
        public TimeSpan TotalPlanTime;
        public TimeSpan TotalCoopTime;

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
                        if (item is Operation)
                        {
                            var oper = item as Operation;
                            if (oper.CountFlag) t = t.Add(oper.Time);
                        }
                    }
                }
            }
            return t;
        }
    }
    // Задание: вычислить одну строку отчета
    public struct LaborRowJob
    {
        // переменные для чтения:
        // массив всех деталей
        public Detail[] details;
        // массив всех операций
        public Operation[] operations;
        // деталь, для которой считается строка отчета
        public Detail ReportRowDetail;
        // заказ, для которого считается строка отчета
        public Order ReportRowOrder;
        // список операций
        //public List<Operation> RowOperations;
        // словарь групп операций
        public Dictionary<int, OperationGroup> OperationGroupDictionary;
        // показывать полный список операций
        public bool ShowOperationFlag;
        // дата, до которой учитываются фактические операции
        public DateTime EndDate;

        // переменные для записи:
        // строка отчета
        public LaborReportRow Row;
        IEnumerable<Detail> GetTree(Detail d)
        {
            List<Detail> result = new List<Detail>() { d };

            if (d.Position > 0)
            {
                // было так:
                // var DetailList = GetDetails();
                // var e = from item in DetailList
                var e = from item in details
                        where item.PositionParent == d.Position && item.OrderId == d.OrderId
                        select item;

                foreach (var x in e)
                {
                    var sr = GetTree(x);
                    result.AddRange(sr);
                }
            }

            return result;
        }
        IEnumerable<Operation> GetRowOperations()
        {
            int orderId = 0;
            if (ReportRowDetail != null) orderId = ReportRowDetail.OrderId;
            if (ReportRowOrder != null) orderId = ReportRowOrder.Id;
            if (ReportRowDetail != null)
            {
                // операции для детали
                
                // Для каждой главной детали формируется “древо” деталей по атрибуту "входит в..."
                // оформляем в массив для запроса LINQ
                var e5 = from x in GetTree(ReportRowDetail)
                         where x.OrderId == orderId
                         select x.OrderDetailId;
                long[] orderDetailIdArray = e5.ToArray();

            
                // Для каждой детали древа формируется список операций
                var odoe = from x in operations
                           where orderDetailIdArray.Contains(x.OrderDetailId) && x.OrderId == orderId
                           select x;

                //var allTreeOperations = odoe.ToList();
                return odoe;
            }
            if (ReportRowOrder != null)
            {
                
                // операции для заказа
                // Список деталей этого заказа
                // оформляем в массив для запроса LINQ
                var od = from d in details
                         where d.OrderId == orderId
                         select d.OrderDetailId;

                long[] orderDetailIdArray = od.ToArray();


                // список операций для деталей orderDetailIdArray
                var odoe = from x in operations
                           where orderDetailIdArray.Contains(x.OrderDetailId) && x.OrderId == orderId
                           select x;

                //var itemOperations = odoe.ToList();
                return odoe;
            }
            throw new ArgumentException("Нужно указать параметр ReportRowDetail или ReportRowOrder");
        }
        public void Execute()
        {
            Row.TotalFactTime = TimeSpan.Zero;
            Row.TotalPlanTime = TimeSpan.Zero;
            Row.TotalCoopTime = TimeSpan.Zero;

            var RowOperations = GetRowOperations();

            if (ShowOperationFlag == true)
            {
                // Подробно по операциям
                // Операции группируются по имени
                var dvoGroups = RowOperations.GroupBy(o => o.Name);
                foreach (var g in dvoGroups)
                {
                    // Ключ словаря: имя операции. В итоге выводится в имени столбца DataGrid
                    string name = g.Key;
                    // Перечисление всех элементов группы
                    // В группу входят все операции с одним именем: name
                    ProcessOperations(g, name);
                }
            }
            else
            {
                // Операции в группах
                // Для каждой группы
                foreach (var kv in OperationGroupDictionary)
                {
                    var g = kv.Value;
                    string name = g.Name;
                    // Получаем операции
                    var se = from item in RowOperations
                             where item.GroupId == g.Id
                             select item;

                    // Если есть операции, считаем план и факт
                    ProcessOperations(se, name);
                }
            }
            // цитата из ТЗ:
            // 2.7 Лимит трудоемкости, н/час
            if (Row.TotalPlanTime > TimeSpan.Zero)
            {
                Row.Operations[LaborReport.limitName] = LaborReport.Format(Row.TotalPlanTime);
            }
            // 2.8 Трудоёмкость по кооперации, н/час
            if (Row.TotalCoopTime > TimeSpan.Zero)
            {
                Row.Operations[LaborReport.coopName] = LaborReport.Format(Row.TotalCoopTime);
            }
            // 2.9 Лимит трудоёмкости с учетом кооперации (рассчитывается как разница п.2.7-п.2.8);
            var planSubtractCoop = Row.TotalPlanTime.Subtract(Row.TotalCoopTime);
            //if (planSubtractCoop > TimeSpan.Zero)
            {
                Row.Operations[LaborReport.limitCoopName] = LaborReport.Format(planSubtractCoop);
                // Фиктивная "операция" для limitCoopName, нужна для попадания в "Итого"
                Operation x = new Operation() { Name = LaborReport.limitCoopName, Time = planSubtractCoop };
                Row.AddWorkTime(LaborReport.limitCoopName, x);
            }
            // 2.10 Израсходовано на 01 число месяца (на любую выбранную дату), следующего за отчетным
            if (Row.TotalFactTime > TimeSpan.Zero)
            {
                Row.Operations[LaborReport.factName] = LaborReport.Format(Row.TotalFactTime);
            }
            // 2.11 Остаток на 01 число месяца и /или на любую выбранную дату (рассчитывается как разница п.2.9-п.2.10);
            //ts = totalPlanTime.Subtract(totalFactTime);
            var planRest = planSubtractCoop.Subtract(Row.TotalFactTime);
            if (planRest >= TimeSpan.Zero)
            {
                Row.Operations[LaborReport.restName] = LaborReport.Format(planRest);
                // Фиктивная "операция" для restName, нужна для попадания в "Итого"
                Operation x = new Operation() { Name = LaborReport.restName, Time = planRest };
                Row.AddWorkTime(LaborReport.restName, x);
            }
        }

        // Обработка операций, которые группируются по указанному имени: name
        void ProcessOperations(IEnumerable<Operation> opers, string name)
        {
            TimeSpan planTime = TimeSpan.Zero;
            TimeSpan factTime = TimeSpan.Zero;
            TimeSpan coopTime = TimeSpan.Zero;

            foreach (var op in opers)
            {
                // добавление операции в справочник для вывода "Подробно"
                var clone = op.Clone;
                clone.CountFlag = false;
                Row.AddWorkTime(name, clone);
                // фактические операции
                if (op.TypeRow == "3fact" && op.Login != LaborReport.coopLogin)
                {
                    //if (yearBeginDate <= op.FactDate && op.FactDate < yearEndDate)
                    //{
                    //    factByOperation.AddWorkTime(name, op);
                    //}
                    if (op.FactDate < EndDate)
                    {
                        factTime = factTime.Add(op.Time);
                        Row.AddWorkTime(LaborReport.factName, op);
                        //dv.AddWorkTime(restName, op);
                    }
                }
                // плановые операции
                if (op.TypeRow == "1sp" || op.TypeRow == "2sp111")
                {
                    planTime = planTime.Add(op.Time);
                    Row.AddWorkTime(LaborReport.limitName, op);
                }
                // кооперация, фактически, вне зависимости от даты
                if (op.TypeRow == "3fact" && op.Login == LaborReport.coopLogin)
                {
                    coopTime = coopTime.Add(op.Time);
                    Row.AddWorkTime(LaborReport.coopName, op);
                }
            }

            // выводим остаток по операциям
            var ts = planTime.Subtract(factTime).Subtract(coopTime);
            if (ts > TimeSpan.Zero)
            {
                Row.Operations[name] = LaborReport.Format(ts);
                // Фиктивная "операция" для name, нужна для попадания в "Итого"
                Operation x = new Operation() { Name = name, Time = ts, TypeRow = "ИТОГО", };
                Row.AddWorkTime(name, x);
            }

            Row.TotalFactTime = Row.TotalFactTime.Add(factTime);
            Row.TotalPlanTime = Row.TotalPlanTime.Add(planTime);
            Row.TotalCoopTime = Row.TotalCoopTime.Add(coopTime);

        }
    }

    public class LaborReport
    {
        public const string quantityName = "Кол-во работников за год";
        public const string timeSheetName = "Табельное время за год";
        public const string restName = "Остаток на указанную дату";
        public const string limitName = "Лимит трудоемкости";
        public const string limitCoopName = "Лимит трудоемкости без кооперации";
        public const string itogoName = "Итого";
        public const string productivityName = "Выработка, %";
        public const string coopName = "Кооперация";
        public const string coopLogin = "кооп";
        public const string factName = "Израсходовано";
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
        DetailRepository detrep;
        Detail[] details;
        
        OperationRepository oprep;
        Operation[] operations;

        OperationGroupRepository groups;
        // словарь операций по коду
        Dictionary<int, OperationGroup> opgDict;
        WorkDayRepository workDays;

        Dictionary<int, Order> allOrderDict;
        List<Order> factOrders;



        public LaborReport(DetailRepository detrep, OperationRepository oprep, OperationGroupRepository groups, 
            WorkDayRepository workDays, OrderRepository allOrders)
        {
            if (detrep == null) throw new ArgumentException("Пожалуйста укажите параметр: DetailRepository");
            if (oprep == null) throw new ArgumentException("Пожалуйста укажите параметр: OperationRepository");
            if (groups == null) throw new ArgumentException("Пожалуйста укажите параметр: OperationGroupRepository");
            if (workDays == null) throw new ArgumentException("Пожалуйста укажите параметр: WorkDayRepository");
            
            if (allOrders == null) throw new ArgumentException("Пожалуйста укажите параметр: allOrders");

            this.detrep = detrep;
            this.oprep = oprep;
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
        public static string Format(TimeSpan ts)
        {
            string s = $"{(int)ts.TotalHours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            return s;
        }
        public void Load()
        {
            detrep.Load();
            details = detrep.GetArray();

            oprep.Load();
            operations = oprep.GetArray();
            Debug.WriteLine("operations.Length: " + operations.Length);

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

                //factOrders = new List<Order>();
                //foreach (var p in allOrderDict) factOrders.Add(p.Value);
                //oe = factOrders;
                oe = allOrderDict.Values;
            }
            // массив идентификаторов заказов
            var orderIdArray = oe.Select(x => x.Id).ToArray();

            // список заданий формирования строк заказов
            List<LaborRowJob> jobs = new List<LaborRowJob>();

            if (ShowDetailFlag == true)
            {
                /*
                // Операции по деталям
                // Список деталей, относящихся к указанным заказам
                List<Detail> e = new List<Detail>();
                foreach (var o in oe)
                {
                    // было:
                    // from item in details.GetDetails()
                    var od = from item in detailDict.Values
                             where item.OrderId == o.Id
                             select item;
                    foreach (var d in od) e.Add(d);
                }
                */
                // новая версия алгоритма получения списка деталей заказов
                var e = from item in details
                        where orderIdArray.Contains(item.OrderId)
                        select item;

                foreach (var d in e)
                {
                    // количество "родителей" по атрибуту "входит в..."
                    int parents = e.Where(item => item.Position == d.PositionParent && item.OrderId == d.OrderId).Count();
                    if (d.PositionParent == 0 || parents == 0)
                    {
                        var job = ProcessMainDetail(d);
                        jobs.Add(job);
                    }
                }
            }
            else
            {
                // Операции по заказам
                foreach (var o in oe)
                {
                    var job = ProcessOrder(o);
                    jobs.Add(job);
                }
            }

            Stopwatch sw = Stopwatch.StartNew();
            // запускаем выполнение всех заданий:
            // параллельно:
            Parallel.ForEach(jobs, j => j.Execute());
            // последовательно:
            //foreach (var j in jobs) j.Execute();
            sw.Stop();
            var ms = sw.Elapsed;
            //System.Windows.Forms.MessageBox.Show("Время вычисления всех строк отчета: " + ms);
            Debug.WriteLine("Время вычисления всех строк отчета: " + ms);

            foreach (var j in jobs) if (j.Row.TotalPlanTime > TimeSpan.Zero) rows.Add(j.Row);
        }

        LaborRowJob ProcessMainDetail(Detail detailItem)
        {
            

            var job = new LaborRowJob()
            {
                Row = new LaborReportRow() { Name = detailItem.Name },
                operations = operations,
                details = details,
                ReportRowDetail = detailItem,
                ReportRowOrder = null,
                OperationGroupDictionary = this.opgDict,
                ShowOperationFlag = ShowOperationFlag,
                EndDate = EndDate,
            };

            return job;
        }
        LaborRowJob ProcessOrder(Order orderItem)
        {
            

            var job = new LaborRowJob()
            {
                Row = new LaborReportRow() { Name = orderItem.Number },
                operations = operations,
                details = details,
                ReportRowDetail = null,
                ReportRowOrder = orderItem,
                OperationGroupDictionary = this.opgDict,
                ShowOperationFlag = ShowOperationFlag,
                EndDate = EndDate,
            };

            return job;
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

            var d = details.Where(x => x.OrderDetailId == item.OrderDetailId && x.OrderId == item.OrderId);
            if (d.Any()) r.Operations["ЩЦМ"] = d.Single().Shcm;
            else r.Operations["ЩЦМ"] = String.Empty;

            r.Operations["Операция"] = item.Name;
            if (opgDict.ContainsKey(item.GroupId))
                r.Operations["Группа"] = opgDict[item.GroupId].Name;
            if (item.FactDate > DateTime.MinValue) r.Operations["Дата"] = item.FactDate.ToShortDateString();
            if (item.Login != null) r.Operations["Логин"] = item.Login;
            r.Operations["Кол-во"] = item.Quantity.ToString();
            r.Operations["TPD"] = item.Tpd.ToString();
            r.Operations["TSH"] = item.Tsh.ToString();
            r.Operations["Время"] = Format(item.Time);
            r.Operations["Тип"] = item.FormattedTypeRow;
            return r;
        }
    }
}
