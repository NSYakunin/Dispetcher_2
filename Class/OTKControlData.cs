using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispetcher2.Class
{
    public class OTKControlData : ICloneable
    {
        public CheckBoxState[] States { get; set; }
        public string Note { get; set; }
        public DateTime ChangeDate { get; set; }
        public string User { get; set; }

        public OTKControlData()
        {
            States = new CheckBoxState[] { CheckBoxState.Unchecked, CheckBoxState.Unchecked, CheckBoxState.Unchecked };
            Note = string.Empty;
            ChangeDate = DateTime.MinValue;
            User = string.Empty;
        }

        public object Clone()
        {
            return new OTKControlData
            {
                States = (CheckBoxState[])this.States.Clone(),
                Note = this.Note,
                ChangeDate = this.ChangeDate,
                User = this.User
            };
        }
    }
}
