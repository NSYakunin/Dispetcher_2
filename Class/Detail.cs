using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dispetcher2.Class
{
    public abstract class Detail
    {
        public string NameType { get; set; }
        public int Position { get; set; }
        public string Shcm { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string AllPositionParent { get; set; }
        public long OrderDetailId { get; set; }
        public long IdDetail { get; set; }
        public long IdLoodsman { get; set; }
        public int PositionParent { get; set; }
        
    }

    public abstract class DetailRepository
    {
        public abstract IEnumerable<Detail> GetDetails();
    }
}
