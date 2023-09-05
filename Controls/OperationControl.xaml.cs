using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Dispetcher2.Controls
{
    /// <summary>
    /// Логика взаимодействия для LaborControl.xaml
    /// </summary>
    public partial class OperationControl : UserControl
    {
        public OperationControl()
        {
            InitializeComponent();
        }

        /*
            public class OperationDictionary
            {
                public Dictionary<int, object> Value { get; set; }

                public OperationDictionary()
                {
                    Value = new Dictionary<int, object>();
                }
            }
        */
        //void ShowOrderDetail(Order z)
        //{


        //    m.OperationList = new List<OperationDictionary>();

        //    for (int i = 0; i < z.MainDetailList.Count; i++)
        //    {
        //        var d = z.MainDetailList[i];

        //        c = new DataGridTextColumn();
        //        c.Header = d.ShcmAndName;
        //        c.MaxWidth = 200;
        //        c.MinWidth = 50;
        //        c.Binding = new Binding($"Value[{i + 1}]");
        //        mainDataGrid.Columns.Add(c);
        //    }

        //    List<string> NameList = new List<string>();
        //    foreach (var d in z.MainDetailList)
        //    {
        //        var e = from x in d.PlanOperations select x.Name;
        //        NameList.AddRange(e);
        //        e = from x in d.FactOperations select x.Name;
        //        NameList.AddRange(e);
        //    }

        //    var e2 = NameList.Distinct().OrderBy(x => x);
        //    NameList = new List<string>();
        //    NameList.AddRange(e2);

        //    foreach (var name in NameList)
        //    {
        //        OperationDictionary op = new OperationDictionary();
        //        op.Value[0] = name;
        //        for (int i = 0; i < z.MainDetailList.Count; i++)
        //        {
        //            var d = z.MainDetailList[i];
        //            var e3 = d.PlanOperations.Where(item => item.Name == name).SingleOrDefault();

        //            var ef = d.FactOperations.Where(item => item.Name == name).SingleOrDefault();

        //            op.Value[i + 1] = MakeReportString(plan: e3, fact: ef);
        //        }
        //        m.OperationList.Add(op);
        //    }

        //    mainDataGrid.ItemsSource = m.OperationList;
        //}

        void CreateColumns()
        {
            mainDataGrid.AutoGenerateColumns = false;
            DataGridTextColumn c;

            mainDataGrid.Columns.Clear();

            c = new DataGridTextColumn();
            c.Header = "Деталь или сборка";
            c.MaxWidth = 200;
            c.MinWidth = 50;
            c.Binding = new Binding("Value[0]");
            mainDataGrid.Columns.Add(c);
        }
    }
}
