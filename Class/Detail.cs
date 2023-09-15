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
        public long IdDetail { get; set; }
        public long IdLoodsman { get; set; }
        public int PositionParent { get; set; }
    }

    public abstract class DetailRepository : Repository
    {
        public abstract IEnumerable<Detail> GetDetails();
        //public abstract void Load();

        public IEnumerable<Detail> GetTree(Detail d)
        {
            var DetailList = GetDetails();

            List<Detail> result = new List<Detail>();
            result.Add(d);
            if (d.Position != d.PositionParent)
            {

                var e = from item in DetailList
                        where item.PositionParent == d.Position && item.OrderId == d.OrderId
                        select item;

                if (e.Any())
                {
                    foreach (var x in e)
                    {
                        var sr = GetTree(x);
                        if (sr != null) result.AddRange(sr);
                    }
                }
            }

            return result;
        }

        public IEnumerable<Detail> GetMainDetails()
        {
            var DetailList = GetDetails();
            List<Detail> result = new List<Detail>();
            foreach (var x in DetailList)
            {
                string s = Convert.ToString(x.PositionParent).Trim();
                int p = 0;
                Int32.TryParse(s, out p);
                if (p == 0) result.Add(x);
            }
            return result;
        }
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
        public override IEnumerable<Detail> GetDetails()
        {
            if (details == null) Load();
            return details;
        }
        public override void Load()
        {
            details = new List<Detail>();

            var d = new TestDetail() { IdDetail = 1, Name = "Ротор ЩЦМ 1.111.111", OrderDetailId = 1, OrderId = 6373 };
            details.Add(d);

            d = new TestDetail() { IdDetail = 2, Name = "Торот ЩЦМ 2.222.222", OrderDetailId = 2, OrderId = 6373 };
            details.Add(d);
        }
    }
}
