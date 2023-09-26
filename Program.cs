using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Dispetcher2.Class;

namespace Dispetcher2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var d = new DispetcherContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(d.ResolveStartForm());
        }



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

                case "Комплектация":
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
                    f = new F_ReportsPlan(config);
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
