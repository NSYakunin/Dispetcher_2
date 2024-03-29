using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dispetcher2.Class
{
    public abstract class Detail
    {
        public int OrderId { get; set; }
        public string NameType { get; set; }
        public int Position { get; set; }
        public string Shcm { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string AllPositionParent { get; set; }
        public long OrderDetailId { get; set; }


        public int PositionParent { get; set; }
    }

    public abstract class DetailRepository : Repository
    {
        public abstract IEnumerable<Detail> GetDetails();

        public abstract Detail[] GetArray();
    }

    public class TestDetailRepository : DetailRepository
    {
        private class TestDetail : Detail
        {

        }
        List<Detail> details;
        public TestDetailRepository()
        {
            
        }
        public override System.Collections.IEnumerable GetList()
        {
            if (details == null) Load();
            return details;
        }
        public override Detail[] GetArray()
        {
            if (details == null) Load();
            return details.ToArray();
        }
        public override IEnumerable<Detail> GetDetails()
        {
            if (details == null) Load();
            return details;
        }
        public override void Load()
        {
            details = new List<Detail>();

            var d = new TestDetail() { Name = "Ротор ЩЦМ 1.111.111", OrderDetailId = 1, OrderId = 6373 };
            details.Add(d);

            d = new TestDetail() { Name = "Торот ЩЦМ 2.222.222", OrderDetailId = 2, OrderId = 6373 };
            details.Add(d);
        }
    }
}
