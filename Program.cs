using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;

using Dispetcher2.Class;

namespace Dispetcher2
{
    static class Program
    {
        static ServerViewModel vm;
        static Dispetcher2.Class.Configuration config;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            config = new Class.Configuration();
            FormFactory factory = new DispetcherFormFactory(config);
            vm = new ServerViewModel();
            vm.ServerChangeHandler = OnServerChange;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new F_Login(vm, config, factory));
        }

        static void OnServerChange()
        {
            var ss = vm.SelectedServer;
            string s = "0";
            if (ss != null) if (ss.Type == ServerType.Testing) s = "1";
            var appSettings = ConfigurationManager.AppSettings;
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["SelectedIndex"].Value = s;
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
            Application.Restart();
        }
    }

    class DispetcherFormFactory : FormFactory
    {
        IConfig config;
        public DispetcherFormFactory(IConfig config)
        {
            this.config = config;
            this.Information = config.Information;
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
                    f = new F_TimeSheets(config);
                    return f;

                case "Пользователи":
                    f = new F_Users(config);
                    return f;

                case "Настройки администратора":
                    f = new F_Settings(config);
                    return f;

                case "Заказы":
                    f = new F_Orders(config);
                    return f;

                case "Фактически выполненные операции":
                    f = new F_Fact(config);
                    return f;

                case "F_Kit":
                    f = new F_Kit(config);
                    return f;

                case "Отчёт-наряд по выполненным операциям":
                    config.SelectedReportMode = ReportMode.ОтчетНаряд;
                    f = new F_Reports(config);
                    return f;

                case "Операции выполненные рабочим по заказам":
                    config.SelectedReportMode = ReportMode.ОперацииВыполненныеРабочим;
                    f = new F_Reports(config);
                    return f;

                case "Движение деталей":
                    config.SelectedReportMode = ReportMode.ДвижениеДеталей;
                    f = new F_Reports(config);
                    return f;

                case "Отчет по выполненным операциям":
                    config.SelectedReportMode = ReportMode.ОтчетВыполненным;
                    f = new F_Reports(config);
                    return f;

                case "План-график":
                    config.SelectedReportMode = ReportMode.ПланГрафик;
                    f = new F_Reports(config);
                    return f;

                case "Трудоемкость":
                    config.SelectedReportMode = ReportMode.Трудоемкость;
                    f = new F_Reports(config);
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
