using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Dispetcher2.Class
{
    public class Operation
    {
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
    }

    public abstract class OperationRepository
    {
        public abstract IEnumerable<Operation> GetOperations();
    }
}
