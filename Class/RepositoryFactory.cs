
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
}
