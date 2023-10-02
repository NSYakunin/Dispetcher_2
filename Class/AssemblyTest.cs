using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dispetcher2.Class
{
    
    class AssemblyTest : Repository
    {
        List<string> strings;
        public override void Load()
        {
            Type t = typeof(AssemblyTest);
            var sql = "data";
            var references = t.Assembly.GetReferencedAssemblies();

            var e = from item in references
                    where item.Name.ToLower().Contains(sql)
                    select item.Name;

            strings = new List<string>();
            if (e.Any()) strings.AddRange(e);
        }
        public override System.Collections.IEnumerable GetList()
        {
            if (strings == null) Load();
            return strings;
        }
    }
}
