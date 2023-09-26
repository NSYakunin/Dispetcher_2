using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Dispetcher2.Class
{
    public class Operation
    {
        public long OrderDetailId { get; set; }
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
        public string TypeRow { get; set; }
    }
    public abstract class OperationRepository : Repository
    {
        public abstract IEnumerable<Operation> GetOperations();
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
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public abstract class OperationGroupRepository : Repository
    {
        public abstract IEnumerable<OperationGroup> GetGroups();
    }
}
