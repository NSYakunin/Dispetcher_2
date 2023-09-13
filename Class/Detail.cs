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

        public IEnumerable<Detail> GetTree(Detail d)
        {
            var DetailList = GetDetails();

            List<Detail> result = new List<Detail>();
            result.Add(d);

            var e = from item in DetailList
                    where item.PositionParent == d.Position
                    select item;

            if (e.Any())
            {
                foreach (var x in e)
                {
                    var sr = GetTree(x);
                    if (sr != null) result.AddRange(sr);
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
            details = new List<Detail>();

            var d = new TestDetail();
            d.Name = "Деталь ЩЦМ 1";
            details.Add(d);

            d = new TestDetail();
            d.Name = "Деталь ЩЦМ 2";
            details.Add(d);
        }
        public override IEnumerable<Detail> GetDetails()
        {
            return details;
        }
    }
}
