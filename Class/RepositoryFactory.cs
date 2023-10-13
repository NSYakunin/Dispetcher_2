using System.Collections.Generic;

namespace Dispetcher2.Class
{
    public abstract class Repository
    {
        public abstract System.Collections.IEnumerable GetList();
        public abstract void Load();
    }
    public abstract class RepositoryFactory
    {
        public abstract Repository GetRepository(object context, string name);
    }
    public class StringRepository : Repository
    {
        IEnumerable<string> strings;
        public StringRepository(IEnumerable<string> strings)
        {
            this.strings = strings;
        }
        public override void Load()
        {

        }
        public override System.Collections.IEnumerable GetList()
        {
            return strings;
        }
    }
    public interface IColumnUpdate
    {
        void Update(StringRepository names);
    }
}
