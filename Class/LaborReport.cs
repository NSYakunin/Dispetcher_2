using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Dispetcher2.Class
{
    public abstract class LaborReportWriter
    {
        public abstract void Write(StringRepository colrep, LaborReportRepository labrep);
    }
    public class LaborReportRow
    {
        // имя строки
        public string Name { get; set; }
        // для вывода в интерфейс пользователя
        // ключ словаря: имя операции
        // значение словаря: текст в ячейке
        public Dictionary<string, string> Operations { get; set; }
        // для целей рассчета
        // ключ словаря: имя операции
        // значение словаря: фактическое время
        public Dictionary<string, TimeSpan> TimeDictionary { get; set; }

        public LaborReportRow()
        {
            Operations = new Dictionary<string, string>();
            TimeDictionary = new Dictionary<string, TimeSpan>();
        }
    }
    public class LaborReportRepository : Repository
    {
        IEnumerable<LaborReportRow> rows;
        public LaborReportRepository(IEnumerable<LaborReportRow> rows)
        {
            this.rows = rows;
        }
        public override void Load() { }
        public override System.Collections.IEnumerable GetList()
        {
            return rows;
        }
    }
    public class LaborReport
    {
        private const string QuantityName = "Кол-во работников";
        private const string TimeSheetName = "Табельное время";
        private const string TotalName = "Всего";
        private const string LimitName = "Лимит трудоемкости";
        private const string LimitCoopName = "Лимит трудоемкости без кооперации";
        private const string ItogoName = "Итого";
        private const string ProductivityName = "Выработка, %";
        private const string CoopName = "Кооперация";
        private const string coopLogin = "кооп";
        public OrderRepository SelectedOrders { get; set; }
        public bool ShowDetailFlag { get; set; }
        public bool ShowOperationFlag { get; set; }
        public bool FactOrdersFlag { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        List<LaborReportRow> rows;
        // Внешние зависимости
        DetailRepository details;
        OperationRepository operations;
        OperationGroupRepository groups;
        WorkDayRepository workDays;

        Dictionary<int, Order> allOrderDict;
        List<Order> factOrders;
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
        }
        string Format(TimeSpan ts)
        {
            string s = $"{(int)ts.TotalHours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            return s;
        }
        public void Load()
        {
            details.Load();
            operations.Load();
            groups.Load();
        }

        private void ProcessEmployeeTime()
        {
            // словарь операций по коду
            var opDic = new Dictionary<int, OperationGroup>();
            foreach (var item in groups.GetGroups()) opDic[item.Id] = item;

            // фильтруем рабочие дни по дате
            // Группировать данные в словарь списков по коду группы операции
            // Группировать данные в словарь операция-сотрудники
            var wdgroups = workDays.GetWorkDays(BeginDate, EndDate).GroupBy(item => item.OperationGroupId);
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

            var dv = new LaborReportRow() { Name = QuantityName };
            // Всего, количество работников
            int totalCount = 0;

            foreach (var item in opDic)
            {
                OperationGroup op = item.Value;
                if (empDic.ContainsKey(op.Id))
                {
                    dv.Operations[op.Name] = empDic[op.Id].Count().ToString();
                    totalCount = totalCount + empDic[op.Id].Count();
                }
            }
            dv.Operations[TotalName] = totalCount.ToString();
            this.rows.Add(dv);

            dv = new LaborReportRow() { Name = TimeSheetName };
            // Всего, табельное время
            TimeSpan totalTime = TimeSpan.Zero;

            foreach (var item in opDic)
            {
                OperationGroup op = item.Value;
                if (wdDic.ContainsKey(op.Id))
                {
                    TimeSpan time = TimeSpan.Zero;
                    foreach (var day in wdDic[op.Id])
                    {
                        time = time.Add(day.Time);
                    }
                    dv.Operations[op.Name] = Format(time);
                    dv.TimeDictionary[op.Name] = time;
                    totalTime = totalTime.Add(time);
                }
            }
            dv.Operations[TotalName] = Format(totalTime);
            dv.TimeDictionary[TotalName] = totalTime;
            this.rows.Add(dv);
        }
        void ProcessDetailOperations()
        {
            var oe = SelectedOrders.GetOrders();
            if (FactOrdersFlag)
            {
                CalculateFactOrders();
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
            TimeSpan totalTime = TimeSpan.Zero;
            TimeSpan totalPlanTime = TimeSpan.Zero;
            TimeSpan coopTime = TimeSpan.Zero;

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
                    
                    foreach (var op in g)
                    {
                        // плановые операции
                        if (op.TypeRow == "1sp" || op.TypeRow == "2sp111")
                        {
                            planTime = planTime.Add(op.Time);
                        }
                        // фактические операции
                        if (op.TypeRow == "3fact" && BeginDate <= op.FactDate && op.FactDate < EndDate)
                        {
                            if (op.Login == coopLogin) coopTime = coopTime.Add(op.Time);
                            else factTime = factTime.Add(op.Time);
                        }
                    }

                    if (factTime > TimeSpan.Zero)
                        dv.Operations[name] = Format(factTime);
                    totalTime = totalTime.Add(factTime);
                    totalPlanTime = totalPlanTime.Add(planTime);

                    dv.TimeDictionary[name] = factTime;
                }
            }
            else
            {
                // Операции в группах
                // Для каждой группы
                foreach (var g in groups.GetGroups())
                {
                    // Получаем опперации
                    var se = from item in dvOperations
                             where item.GroupId == g.Id
                             select item;
                    if (se.Any() == false) continue;
                    // Если есть операции, считаем план и факт
                    TimeSpan planTime = TimeSpan.Zero;
                    TimeSpan factTime = TimeSpan.Zero;
                    foreach (var op in se)
                    {
                        // плановые операции
                        if (op.TypeRow == "1sp" || op.TypeRow == "2sp111")
                        {
                            planTime = planTime.Add(op.Time);
                        }
                        // фактические операции
                        if (op.TypeRow == "3fact")
                        {
                            if (op.Login == coopLogin) coopTime = coopTime.Add(op.Time);
                            else factTime = factTime.Add(op.Time);
                        }
                    }
                    string name = g.Name;

                    if (factTime > TimeSpan.Zero)
                        dv.Operations[name] = Format(factTime);
                    totalTime = totalTime.Add(factTime);
                    totalPlanTime = totalPlanTime.Add(planTime);

                    dv.TimeDictionary[name] = factTime;
                }
            }
            if (totalTime > TimeSpan.Zero)
                dv.Operations[TotalName] = Format(totalTime);
            if (totalPlanTime > TimeSpan.Zero)
                dv.Operations[LimitName] = Format(totalPlanTime);
            if (coopTime > TimeSpan.Zero)
                dv.Operations[CoopName] = Format(coopTime);
            TimeSpan ts = totalPlanTime.Subtract(coopTime);
            if (ts > TimeSpan.Zero)
                dv.Operations[LimitCoopName] = Format(ts);

            dv.TimeDictionary[TotalName] = totalTime;
            dv.TimeDictionary[LimitName] = totalPlanTime;
            dv.TimeDictionary[CoopName] = coopTime;
            dv.TimeDictionary[LimitCoopName] = ts;
        }
        public StringRepository GetOperationRepository()
        {
            HashSet<string> NameList = new HashSet<string>();
            NameList.Add(LimitName);
            NameList.Add(CoopName);
            NameList.Add(LimitCoopName);
            NameList.Add(TotalName);
            foreach (var item in rows)
            {
                foreach (var k in item.Operations.Keys)
                    NameList.Add(k);
            }

            var rep = new StringRepository(NameList);
            return rep;
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
            LaborReportRow tsrow = null;
            // Считаем итого
            LaborReportRow itog = new LaborReportRow() { Name = ItogoName };
            foreach (var dv in rows)
            {
                if (dv.Name.Equals(QuantityName)) continue;
                if (dv.Name.Equals(TimeSheetName)) 
                { 
                    tsrow = dv; 
                    continue; 
                }
                foreach (var name in dv.TimeDictionary.Keys)
                {
                    if (itog.TimeDictionary.ContainsKey(name) == false)
                        itog.TimeDictionary[name] = TimeSpan.Zero;
                    itog.TimeDictionary[name] = itog.TimeDictionary[name].Add(dv.TimeDictionary[name]);
                }
            }
            foreach (var name in itog.TimeDictionary.Keys)
            {
                if (itog.TimeDictionary[name] > TimeSpan.Zero)
                    itog.Operations[name] = Format(itog.TimeDictionary[name]);
            }
            rows.Add(itog);
            // Считаем выработку
            LaborReportRow vrow = new LaborReportRow() { Name = ProductivityName };
            if (tsrow != null)
            {
                foreach(var name in itog.TimeDictionary.Keys)
                {
                    if (tsrow.TimeDictionary.ContainsKey(name))
                    {
                        double t1 = itog.TimeDictionary[name].TotalMinutes;
                        double t2 = tsrow.TimeDictionary[name].TotalMinutes;
                        double v = 100.0 * t1 / t2;
                        vrow.Operations[name] = v.ToString("0.00") + "%";
                    }
                }
            }
            rows.Add(vrow);
        }
        public LaborReportRepository GetLaborReportRepository()
        {
            LaborReportRepository repository = new LaborReportRepository(rows);
            return repository;
        }
        // Определяем в каких заказах есть факты для указанного диаппазона дат
        void CalculateFactOrders()
        {
            HashSet<int> factOrderIdset = new HashSet<int>();

            foreach (var op in operations.GetOperations())
            {
                // фактические операции
                if (op.TypeRow == "3fact" && BeginDate <= op.FactDate && op.FactDate < EndDate)
                {
                    factOrderIdset.Add(op.OrderId);
                }
            }

            factOrders = new List<Order>();
            foreach (var item in factOrderIdset)
            {
                if (allOrderDict.ContainsKey(item)) factOrders.Add(allOrderDict[item]);
            }
        }
    }
}
