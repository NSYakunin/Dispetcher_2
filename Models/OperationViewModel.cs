using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dispetcher2.Class;

namespace Dispetcher2.Models
{
    public class ReportRow
    {
        public string Name { get; set; }
        public Dictionary<string, string> Operations { get; set; }
        
        public ReportRow()
        {
            Operations = new Dictionary<string, string>();
        }
    }
    public class OperationViewModel
    {
        List<ReportRow> rows;

        DetailRepository allDetails;
        OperationRepository allOperations;
        OperationGroupRepository opgroups;



        public bool ShowDetailFlag { get; set; }
        public bool ShowOperationFlag { get; set; }

        private class DetailViewRepositoryStringRepository : Repository
        {
            IEnumerable<string> operations;
            public DetailViewRepositoryStringRepository(IEnumerable<string> operations)
            {
                this.operations = operations;
            }
            public override void Load()
            {
                
            }
            public override System.Collections.IEnumerable GetList()
            {
                return operations;
            }
        }

        public OperationViewModel(DetailRepository allDetails, OperationRepository allOperations, OperationGroupRepository opgroups)
        {
            this.allDetails = allDetails;
            this.allOperations = allOperations;
            this.opgroups = opgroups;
            this.rows = new List<ReportRow>();
        }

        public IEnumerable<ReportRow> Rows { get { return rows; } }

        /*

        public IEnumerable<Detail> GetMainDetails()
        {
            var DetailList = GetDetails();
            List<Detail> result = new List<Detail>();
            foreach (var x in DetailList)
            {
                string s = Convert.ToString(x.PositionParent).Trim();
                int p = 0;
                Int32.TryParse(s, out p);
                if (p == 0) result.Add(x);
            }
            return result;
        }

        */

        public void Update(OrderRepository orders)
        {
            this.rows.Clear();

            if (ShowDetailFlag == true)
            {
                // Операции по деталям
                // Список деталей, относящихся к указанным заказам
                List<Detail> e = new List<Detail>();
                foreach (var o in orders.GetOrders())
                {
                    var od = from item in allDetails.GetDetails()
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
                foreach (var o in orders.GetOrders())
                {
                    ProcessOrder(o);
                }
            }
        }

        void ProcessMainDetail(Detail d)
        {
            // все операции
            var e3 = allOperations.GetOperations();
            // Для каждой главной детали формируется “древо” деталей по атрибуту "входит в..."
            var e4 = allDetails.GetTree(d);

            List<Operation> allTreeOperations = new List<Operation>();
            foreach (var i in e4)
            {
                // Для каждой детали древа формируется список операций
                var e5 = allOperations.GetOperations().Where(op => op.OrderDetailId == i.OrderDetailId);
                foreach (var e5i in e5) allTreeOperations.Add(e5i);
            }
            // Если нет операций, то не выводим деталь?
            //if (allTreeOperations.Count == 0) return;

            var dv = new ReportRow() { Name = d.Name };
            this.rows.Add(dv);
            

            ProcessRow(dv, allTreeOperations);
        }

        void ProcessOrder(Order item)
        {
            // Список деталей этого заказа
            var od = from d in allDetails.GetDetails()
                     where d.OrderId == item.Id
                     select d;

            // список операций этого заказа
            List<Operation> itemOperations = new List<Operation>();
            foreach (var d in od)
            {
                // Для каждой детали формируется список операций
                var e5 = allOperations.GetOperations().Where(op => op.OrderDetailId == d.OrderDetailId);
                foreach (var e5i in e5) itemOperations.Add(e5i);
            }

            var dv = new ReportRow() { Name = item.Number };
            this.rows.Add(dv);

            ProcessRow(dv, itemOperations);
        }

        void ProcessRow(ReportRow dv, List<Operation> dvOperations)
        {
            if (ShowOperationFlag == true)
            {
                // Подробно по операциям
                // Операции всех деталей в древе группируются по имени
                var groups = dvOperations.GroupBy(o => o.Name);
                foreach (var g in groups)
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
                        if (op.TypeRow == "3fact")
                        {
                            factTime = factTime.Add(op.Time);
                        }
                    }
                    dv.Operations[name] = MakeReportString(plan: planTime, fact: factTime);

                }
            }
            else
            {
                // Операции в группах
                // Для каждой группы
                foreach (var g in opgroups.GetGroups())
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
                            factTime = factTime.Add(op.Time);
                        }
                    }
                    string name = g.Name;
                    dv.Operations[name] = MakeReportString(plan: planTime, fact: factTime);

                }
            }
        }

        string Format(TimeSpan ts)
        {
            string s = $"{(int)ts.TotalHours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
            return s;
        }
        string MakeReportString(TimeSpan plan, TimeSpan fact)
        {
            StringBuilder sb = new StringBuilder();
            if (plan != null)
            {
                sb.Append("План: " + Format(plan));
            }
            if (fact != null)
            {
                if (sb.Length > 0) sb.Append(Environment.NewLine);
                sb.Append("Факт: " + Format(fact));
            }
            // остаток
            TimeSpan rest = TimeSpan.Zero;
            if (plan != null)
            {
                rest = plan;
                if (fact != null) rest = rest.Subtract(fact);
                sb.Append(Environment.NewLine);
                if (rest < TimeSpan.Zero) rest = TimeSpan.Zero;
                sb.Append("Остаток: " + Format(rest));
                // Процент
                if (fact != null)
                {
                    if (plan > TimeSpan.Zero)
                    {
                        if (fact <= plan)
                        {
                            decimal dp = (decimal)plan.TotalSeconds;
                            decimal df = (decimal)fact.TotalSeconds;
                            decimal dr = df * (decimal)100.0 / dp;
                            int ir = Convert.ToInt32(dr);
                            sb.Append(Environment.NewLine).Append("Процент: " + ir + " %");
                        }
                        else
                        {
                            sb.Append(Environment.NewLine).Append("Процент: " + "100 %");
                        }
                    }
                }
            }

            return sb.ToString();
        }

        public Repository GetOperationRepository()
        {
            List<string> NameList = new List<string>();
            foreach (var item in rows)
            {
                NameList.AddRange(item.Operations.Keys);
            }
            //var e = NameList.Distinct().OrderBy(x => x);
            var e = NameList.Distinct();


            var rep = new DetailViewRepositoryStringRepository(e);
            return rep;
        }


    }
}
