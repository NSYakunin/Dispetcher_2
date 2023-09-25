using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Threading.Tasks;

using Dispetcher2.DataAccess;

namespace Dispetcher2.Class
{
    class DispetcherContainer
    {
        LoginViewModel vm;
        Configuration config;
        FormFactory factory;

        public DispetcherContainer()
        {
            vm = new LoginViewModel();
            vm.ServerChangeAction = OnServerChange;

            var converter = new Dispetcher2.Class.Converter();
            config = new Class.Configuration();
            factory = new DispetcherFormFactory(config, converter);
        }

        public Form ResolveStartForm()
        {
            var sf = factory.GetForm("Меню");
            var f = new F_Login(vm, config, sf);
            return f;
        }

        void OnServerChange()
        {
            var ss = vm.SelectedServer;
            string s = "0";
            if (ss != null) if (ss.Type == ServerType.Testing) s = "1";
            var appSettings = ConfigurationManager.AppSettings;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["SelectedIndex"].Value = s;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            //Application.Restart();
        }

        private class DispetcherFormFactory : FormFactory
        {
            IConfig config;
            IConverter converter;
            SqlOrderRepository orders;
            public DispetcherFormFactory(IConfig config, IConverter converter)
            {
                this.config = config;
                this.converter = converter;
                // <param name="_IdStatusOrders">1-ожидание,2-открыт,3-закрыт,4-в работе,5-выполнен</param>
                orders = new SqlOrderRepository(config, converter, 2);
            }
            public override string GetInformation()
            {
                return config.Information;
            }

            public override Form GetForm(string purpose)
            {
                Form f;
                switch (purpose)
                {
                    
                        
                    case "Меню":
                        f = new F_Index(this);
                        return f;
                    case "Рабочие":
                        f = new F_Workers(config);
                        return f;

                    case "Бригады":
                        f = new F_Brigade(config);
                        return f;

                    case "Производственный календарь":
                        f = new F_ProductionCalendar(config);
                        return f;

                    case "Табель учёта рабочего времени":
                        f = new F_TimeSheets(config, converter);
                        return f;

                    case "Пользователи":
                        f = new F_Users(config);
                        return f;

                    case "Настройки администратора":
                        f = new F_Settings(config, converter);
                        return f;

                    case "Заказы":
                        f = new F_Orders(config);
                        return f;

                    case "Фактически выполненные операции":
                        f = new F_Fact(config);
                        return f;

                    case "Комплектация":
                        f = new F_Kit(config);
                        return f;

                    case "Отчёт-наряд по выполненным операциям":
                        config.SelectedReportMode = ReportMode.ОтчетНаряд;
                        f = new F_Reports(config, orders, converter);
                        return f;

                    case "Операции выполненные рабочим по заказам":
                        config.SelectedReportMode = ReportMode.ОперацииВыполненныеРабочим;
                        f = new F_Reports(config, orders, converter);
                        return f;

                    case "Движение деталей":
                        config.SelectedReportMode = ReportMode.ДвижениеДеталей;
                        f = new F_Reports(config, orders, converter);
                        return f;

                    case "Отчет по выполненным операциям":
                        config.SelectedReportMode = ReportMode.ОтчетВыполненным;
                        f = new F_Reports(config, orders, converter);
                        return f;

                    case "План-график":
                        config.SelectedReportMode = ReportMode.ПланГрафик;
                        f = new F_ReportsPlan(config);
                        return f;

                    case "Трудоемкость":
                        config.SelectedReportMode = ReportMode.Трудоемкость;
                        f = new F_Reports(config, orders, converter);
                        return f;

                    case "ПРОИЗВОДСТВО-ПЛАН":
                        f = new F_Planning(config);
                        return f;

                    default:
                        f = new F_IndexLogo();
                        return f;
                }
            }
        }
    }
}
