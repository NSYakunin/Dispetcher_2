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
        public DetailView(Detail d)
        {
            this.AllPositionParent = d.AllPositionParent;
            this.Amount = d.Amount;
            this.IdDetail = d.IdDetail;
            this.IdLoodsman = d.IdLoodsman;
            this.Name = d.Name;
            this.NameType = d.NameType;
            this.OrderDetailId = d.OrderDetailId;
            this.Position = d.Position;
            this.PositionParent = d.PositionParent;
            this.Shcm = d.Shcm;
        }
    }
    public class DetailViewRepository
    {
        List<DetailView> details;
        
        public DetailViewRepository(DetailRepository details, OperationRepository operations)
        {
            this.details = new List<DetailView>();

            var e = details.GetMainDetails();
            foreach (var d in e)
            {
                var dv = new DetailView(d);
                this.details.Add(dv);
                dv.Operations = new Dictionary<string, string>();

                var e2 = operations.GetOperations(d);
                foreach(var x in e2)
                {
                    string name = x.Name;
                    string time = Convert.ToString(x.Time);
                    dv.Operations[name] = time;
                }
            }
        }

        public IEnumerable<DetailView> Details { get { return details; } }

    }
}
