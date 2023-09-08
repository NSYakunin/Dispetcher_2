using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dispetcher2.Class
{
    public abstract class StringRepository
    {
        public abstract IEnumerable<String> GetStringList();
    }
    class AssemblyTest : StringRepository
    {
        List<string> strings;

        void Load()
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
        public override IEnumerable<String> GetStringList()
        {
            if (strings == null) Load();
            return strings;
        }
    }
}
