using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dispetcher2.Class
{
    public class Job
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Valid { get; set; }
        public int OperationGroupId { get; set; }
        public override string ToString()
        {
            return $"Id:{Id}, Name:{Name}";
        }
    }

    public abstract class JobRepository : Repository
    {
        public abstract IEnumerable<Job> JobRead();
        public abstract void JobUpdate(Job item);
    }
}
