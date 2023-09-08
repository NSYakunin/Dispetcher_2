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

using Dispetcher2.Class;
using Dispetcher2.Models;

namespace Dispetcher2.Controls
{
    /// <summary>
    /// Логика взаимодействия для LaborControl.xaml
    /// </summary>
    public partial class OperationControl : UserControl
    {
        OperationControlViewModel vm;
        //DetailRepository detRep;
        //OperationRepository opRep;
        //public OperationControl(DetailRepository detRep, OperationRepository opRep)
        public OperationControl(OperationControlViewModel vm)
        {
            //if (detRep == null) throw new Exception("Пожалуйста укажите параметр DetailRepository");
            //if (opRep == null) throw new Exception("Пожалуйста укажите параметр OperationRepository");
            //this.detRep = detRep;
            //this.opRep = opRep;
            this.vm = vm;
            InitializeComponent();
            SetColumns();
        }

        void SetColumns()
        {
            mainGrid.Columns.Clear();
            var e = GetNames();
            foreach (var n in e)
            {
                var c = new DataGridTextColumn();
                c.Header = n;
                c.Binding = new Binding($"Operations[{n}]");
                mainGrid.Columns.Add(c);
            }
        }
        IEnumerable<string> GetNames()
        {
            List<string> NameList = new List<string>();
            foreach (var item in vm.Details)
            {
                NameList.AddRange(item.Operations.Keys);
            }
            var e = NameList.Distinct().OrderBy(x => x);
            return e;
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

        //void CreateColumns()
        //{
        //    mainDataGrid.AutoGenerateColumns = false;
        //    DataGridTextColumn c;

        //    mainDataGrid.Columns.Clear();

        //    c = new DataGridTextColumn();
        //    c.Header = "Деталь или сборка";
        //    c.MaxWidth = 200;
        //    c.MinWidth = 50;
        //    c.Binding = new Binding("Value[0]");
        //    mainDataGrid.Columns.Add(c);
        //}
    }
}
