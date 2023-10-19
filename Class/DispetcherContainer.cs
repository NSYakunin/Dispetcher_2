using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Threading.Tasks;

using Dispetcher2.DataAccess;
using Dispetcher2.Controls;
using Dispetcher2.Models;

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
            

            //LaborViewModel labvm;
            public DispetcherFormFactory(IConfig config, IConverter converter)
            {
                this.config = config;
                this.converter = converter;
                
                // <param name="_IdStatusOrders">1-ожидание,2-открыт,3-закрыт,4-в работе,5-выполнен</param>

            }
            public override string GetInformation()
            {
                return config.Information;
            }

            Form GetReportForm()
            {
                Form rf;

                var orders = new SqlOrderRepository(config, converter, 2);
                var details = new SqlDetailRepository(config, converter);
                var operations = new SqlOperationRepository(config, converter);
                var groups = new SqlOperationGroupRepository(config, converter);
                var workDays = new SqlWorkDayRepository(config, converter, DateTime.Now);
                var writer = new ExcelLaborReportWriter();

                var ocvm = new OrderControlViewModel(orders);
                var rep = new LaborReport(details, operations, groups, workDays, ocvm as OrderRepository);
                var labvm = new LaborViewModel(orders, ocvm, rep, writer);

                rf = new F_Reports(config, converter, labvm);

                return rf;
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
                        return GetReportForm();

                    case "Операции выполненные рабочим по заказам":
                        config.SelectedReportMode = ReportMode.ОперацииВыполненныеРабочим;
                        return GetReportForm();

                    case "Движение деталей":
                        config.SelectedReportMode = ReportMode.ДвижениеДеталей;
                        return GetReportForm();

                    case "Отчет по выполненным операциям":
                        config.SelectedReportMode = ReportMode.ОтчетВыполненным;
                        return GetReportForm();
                                            
                    case "Трудоемкость":
                        config.SelectedReportMode = ReportMode.Трудоемкость;
                        return GetReportForm();

                    case "План-график":
                        config.SelectedReportMode = ReportMode.ПланГрафик;
                        f = new F_ReportsPlan(config);
                        return f;

                    case "Акт приёма-передачи. Гальваническое покрытие":
                        config.SelectedReportMode = ReportMode.Гальваника;
                        return GetReportForm();

                    case "ПРОИЗВОДСТВО-ПЛАН":
                        f = new F_Planning(config);
                        return f;
                    
                    case "Специальности":
                        var jr = new SqlJobRepository(config, converter);
                        var gr = new SqlOperationGroupRepository(config, converter);
                        var vm = new JobViewModel(jr, gr);
                        f = new JobForm(vm);
                        return f;

                    default:
                        f = new F_IndexLogo();
                        return f;
                }
            }
        }
    }
}
