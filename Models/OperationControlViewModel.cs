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
        public Dictionary<string, string> Operations { get; set; }
    }
    public class OperationControlViewModel
    {
        public List<DetailView> Details { get; set; }
    }
}
