using System;

namespace Dispetcher2.Class
{
    public enum OrderType
    {
        SQL,
        Excel,
    }
    public interface IConfig
    {
        string ConnectionString { get; }
        bool Debug { get; }
    }
    public class Configuration : IConfig
    {
        public string ConnectionString 
        {
            get { return C_Gper.ConnStrDispetcher2; }
        }
        public bool Debug
        {
            get { return true; }
        }
    }
}
