using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Dispetcher2.Class
{
    public abstract class WorkTime
    {
        public TimeSpan Time { get; set; }
    }
    public class Operation : WorkTime
    {
        public long OrderDetailId { get; set; }
        public int Numcol { get; set; }
        public string Name { get; set; }
        
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
        public bool OnlyOncePay { get; set; }
        public string Number { get; set; }
        public string TypeRow { get; set; }
        public string FormattedTypeRow
        {
            get
            {
                // if (op.TypeRow == "1sp" || op.TypeRow == "2sp111")
                if (TypeRow == "1sp" || TypeRow == "2sp111") return "план";
                // if (op.TypeRow == "3fact" && op.Login == coopLogin)
                if (TypeRow == "3fact")
                {
                    if (Login == "кооп") return "кооп";
                    else return "факт";
                }
                return TypeRow;
            }
        }
        public string Login { get; set; }
        public DateTime FactDate { get; set; }
        public int OrderId { get; set; }
        /// <summary>
        /// true, если эту операцию нужно учитывать в рассчетах.
        /// false используется в отчете "Подробно" для клеток плана трудоемкости по операциям
        /// </summary>
        public bool CountFlag { get; set; } = true;
        public Operation Clone
        {
            get
            {
                Operation c = new Operation()
                {
                    OrderDetailId = OrderDetailId,
                    Numcol = Numcol,
                    Name = Name,
                    GroupId = GroupId,
                    Tpd = Tpd,
                    Tsh = Tsh,
                    Quantity = Quantity,
                    OnlyOncePay = OnlyOncePay,
                    Number = Number,
                    TypeRow = TypeRow,
                    Login = Login,
                    FactDate = FactDate,
                    OrderId = OrderId,
                    Time = Time,
                    CountFlag = CountFlag,
                };
                return c;
            }
        }
    }
    public abstract class OperationRepository : Repository
    {
        public abstract IEnumerable<Operation> GetOperations();
        public abstract Operation[] GetArray();
    }

    public class TestOperationRepository : OperationRepository
    {
        List<Operation> operations;


        public TestOperationRepository()
        {
            
        }
        public override IEnumerable GetList()
        {
            if (operations == null) Load();
            return operations;
        }
        public override Operation[] GetArray()
        {
            return operations.ToArray();
        }
        public override IEnumerable<Operation> GetOperations()
        {
            if (operations == null) Load();
            return operations;
        }
        public override void Load()
        {
            operations = new List<Operation>();

            var x = new Operation() { Name = "Слесарная", OrderDetailId = 1, Time = TimeSpan.FromHours(1) };
            x.TypeRow = "1sp";
            operations.Add(x);

            x = new Operation() { Name = "Созерцательная", OrderDetailId = 1, Time = TimeSpan.FromMinutes(10) };
            x.TypeRow = "1sp";
            operations.Add(x);

            x = new Operation() { Name = "Перекур", OrderDetailId = 1, Time = TimeSpan.FromMinutes(5) };
            x.TypeRow = "1sp";
            operations.Add(x);

            x = new Operation() { Name = "Дискуссионная", OrderDetailId = 1, Time = TimeSpan.FromMinutes(8) };
            x.TypeRow = "1sp";
            operations.Add(x);

            x = new Operation() { Name = "Слесарная", OrderDetailId = 2, Time = TimeSpan.FromHours(3) };
            x.TypeRow = "1sp";
            operations.Add(x);
        }
    }

    public class OperationGroup
    {
        string n = String.Empty;
        public int Id { get; set; }
        public string Name
        {
            get
            {
                if (Id > 0) return n;
                else return String.Empty;
            }
            set
            {
                n = value;
            }
        }
        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}";
        }
    }
    public abstract class OperationGroupRepository : Repository
    {
        public abstract IEnumerable<OperationGroup> GetGroups();
    }
}
