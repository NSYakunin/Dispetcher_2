
using System.Configuration;
using System.Data.SqlClient;

namespace Dispetcher2.Class
{
    public enum OrderType
    {
        SQL,
        Excel,
    }
    public enum ReportMode
    {
        Неизвестно = 0,
        ОтчетНаряд = 3,
        ДвижениеДеталей = 6,
        ОтчетВыполненным = 7,
        ПланГрафик = 106,
        ОперацииВыполненныеРабочим = 117,
        Трудоемкость = 66,
    }
    public interface IConfig
    {
        string ConnectionString { get; }
        string LoodsmanConnectionString { get; }
        bool Debug { get; }
        ReportMode SelectedReportMode { get; set; }
        string Information { get; }
    }
    public class Configuration : IConfig
    {
        public ReportMode SelectedReportMode { get; set; }
        public string ConnectionString 
        {
            get 
            {
                var appSettings = ConfigurationManager.AppSettings;
                string SERVER = appSettings["SelectedIndex"];
                string cs = ConfigurationManager.ConnectionStrings[SERVER == "0" ? "ConnStrDispetcher2" : "TestConnStrDispetcher2"].ConnectionString;
                return cs; 
            }
        }
        public string LoodsmanConnectionString
        {
            get
            {
                var appSettings = ConfigurationManager.AppSettings;
                string SERVER = appSettings["SelectedIndex"];
                string cs = ConfigurationManager.ConnectionStrings[SERVER == "0" ? "_ConnectionStringLoodsman": "_TestConnectionStringLoodsman"].ConnectionString;
                return cs;
            }
        }
        public bool Debug
        {
            get { return true; }
        }
        
        public string Information
        {
            get
            {
                var b = new SqlConnectionStringBuilder(ConnectionString);
                return b.DataSource;
            }
        }
    }
}
