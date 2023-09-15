using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dispetcher2.Class;

namespace Dispetcher2.Models
{
    public class DetailView : Detail
    {
        public string ShcmAndName { get { return Shcm + "\n" + Name; } }
        // Итоговый отчет по операциям
        public Dictionary<string, string> Operations { get; set; }
        
        public DetailView(Detail d)
        {
            this.AllPositionParent = d.AllPositionParent;
            this.Amount = d.Amount;
            this.IdDetail = d.IdDetail;
            this.IdLoodsman = d.IdLoodsman;
            this.Name = d.Name;
            this.NameType = d.NameType;
            this.OrderDetailId = d.OrderDetailId;
            this.Position = d.Position;
            this.PositionParent = d.PositionParent;
            this.Shcm = d.Shcm;
        }
    }
    public class DetailViewRepository : DetailRepository
    {
        List<DetailView> details;
        DetailRepository allDetails;
        OperationRepository allOperations;
        OrderRepository selectedOrders;

        private class DetailViewRepositoryOperationRepository : OperationRepository
        {
            List<Operation> operations;
            public DetailViewRepositoryOperationRepository(List<Operation> operations)
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
            public override IEnumerable<Operation> GetOperations()
            {
                return operations;
            }
        }

        public DetailViewRepository(DetailRepository allDetails, OperationRepository allOperations, 
            OrderRepository selectedOrders)
        {
            this.allDetails = allDetails;
            this.allOperations = allOperations;
            this.selectedOrders = selectedOrders;
        }

        public IEnumerable<DetailView> Details { get { return details; } }

        public override void Load()
        {
            this.details = new List<DetailView>();
            // Формируется список уникальных “главных деталей”
            var e = allDetails.GetMainDetails();
            foreach (var d in e)
            {
                // список заказов, кторорые содержат эту деталь
                var e2 = selectedOrders.GetOrders().Where(item => item.Id == d.OrderId);
                if (e2.Any() == false) continue;

                var dv = new DetailView(d);
                this.details.Add(dv);
                dv.Operations = new Dictionary<string, string>();

                // все операции
                var e3 = allOperations.GetOperations();
                // Для каждой главной детали формируется “древо” деталей
                var e4 = allDetails.GetTree(d);
                // Для каждой детали древа формируется список операций
                List<Operation> allTreeOperations = new List<Operation>();
                foreach(var i in e4)
                {
                    var e5 = allOperations.GetOperations().Where(op => op.OrderDetailId == i.OrderDetailId);
                    foreach (var e5i in e5) allTreeOperations.Add(e5i);
                }
                // Операции всех деталей в древе группируются по имени
                var groups = allTreeOperations.GroupBy(o => o.Name);
                
                foreach(var g in groups)
                {
                    // Ключ словаря: имя операции. В итоге выводится в имени столбца DataGrid
                    string name = g.Key;
                    // Перечисление всех элементов группы
                    // В группу входят все операции с одним именем
                    TimeSpan time = TimeSpan.Zero;
                    foreach(var op in g)
                    {
                        // плановые операции
                        if (op.TypeRow == "1sp" || op.TypeRow == "2sp111")
                        {
                            time = time.Add(op.Time);
                        }
                    }
                    dv.Operations[name] = Converter.GetString(time);
                }
            }



        }
        public override IEnumerable<Detail> GetDetails()
        {
            return details;
        }

        public override System.Collections.IEnumerable GetList()
        {
            return details;
        }

        public OperationRepository GetOperationRepository()
        {
            List<string> NameList = new List<string>();
            foreach (var item in details)
            {
                NameList.AddRange(item.Operations.Keys);
            }
            var e = NameList.Distinct().OrderBy(x => x);

            List<Operation> result = new List<Operation>();
            var all = allOperations.GetOperations();
            foreach(var name in e)
            {
                var op = all.Where(s => s.Name == name).Take(1).Single();
                result.Add(op);
            }

            var rep = new DetailViewRepositoryOperationRepository(result);
            return rep;
        }

        //IEnumerable<string> GetNames()
        //{
        //    List<string> NameList = new List<string>();
        //    foreach (var item in vm.Details)
        //    {
        //        NameList.AddRange(item.Operations.Keys);
        //    }
        //    var e = NameList.Distinct().OrderBy(x => x);
        //    return e;
        //}
    }
}
