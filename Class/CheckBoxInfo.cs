using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispetcher2.Class
{
    internal class CheckBoxInfo
    {
        public CheckBoxState State { get; set; }
        public DateTime Date { get; set; }
        public string User { get; set; }

        public CheckBoxInfo()
        {
            State = CheckBoxState.Unchecked;
            Date = DateTime.MinValue;
            User = string.Empty;
        }
    }
}
