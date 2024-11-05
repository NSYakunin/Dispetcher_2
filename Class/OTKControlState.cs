using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispetcher2.Class
{
    public class OTKControlState
    {
        public bool Control1 { get; set; }
        public bool Control2 { get; set; }
        public bool Control3 { get; set; }

        public OTKControlState()
        {
            Control1 = false;
            Control2 = false;
            Control3 = false;
        }
    }
}
