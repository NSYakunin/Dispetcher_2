using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispetcher2.Class
{
    public class OTKControlData
    {
        public CheckBoxState[] States { get; set; }
        public string LastUser { get; set; }
        public DateTime LastChangeDate { get; set; }

        public OTKControlData()
        {
            States = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
            LastUser = string.Empty;
            LastChangeDate = DateTime.MinValue;
        }
    }
}
