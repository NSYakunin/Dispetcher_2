using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispetcher2.Class
{
    public class OperationData
    {
        public long PK_IdOrderDetail { get; set; }
        public string Oper { get; set; }
        public int? Tpd { get; set; }
        public int? Tsh { get; set; }
        public long? IdLoodsman { get; set; }
        public CheckBoxState[] OTKControlValues { get; set; }
        public DateTime ChangeDate { get; set; }
        public OTKControlData OTKControlData { get; set; }
        public bool IsChanged { get; set; }
        public bool IsStateChanged { get; set; }
    }
}
