using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispetcher2.Class
{
    public class LaborLoader
    {
        Order selectedOrder;
        C_DataBase dispDB;
        bool cancelFlag = false;
        public event Action Finished;
        Task mainTask = null;

        public LaborLoader(Order selectedOrder)
        {
            this.selectedOrder = selectedOrder;
            dispDB = new C_DataBase(C_Gper.ConnStrDispetcher2);
        }

        public void Start()
        {
            if (mainTask == null)
            {
                cancelFlag = false;
                mainTask = new Task(Main);
                mainTask.Start();
            }
        }

        public void Stop()
        {
            if (mainTask != null)
            {
                cancelFlag = true;
                mainTask.Wait();

            }
        }

        void Main()
        {
            if (cancelFlag == false)
            {
                LoadDetail();
            }
            
            if (cancelFlag == false)
            {
                LoadOperation();
            }
            
            if (Finished != null) Finished();
            mainTask = null;
        }

        void LoadDetail()
        {
            var dl = dispDB.GetOrderDetailAndFastener(selectedOrder.Id);
            selectedOrder.DetailList = dl;
            if (dl != null)
            {
                
                var dl2 = from d in dl
                          where d.PositionParent == 0
                          select d;
                selectedOrder.MainDetailList = new List<Detail>();
                
                foreach(var item in dl2)
                {
                    if (item.IdLoodsman > 0) selectedOrder.MainDetailList.Add(item);
                }
            }
        }

        void LoadOperation()
        {
            C_DataBase loodDB = new C_DataBase(C_Gper.ConStr_Loodsman);
            foreach(var d in selectedOrder.MainDetailList)
            {
                if (cancelFlag) return;
                loodDB.Call_rep_VEDOMOST_TRUDOZATRAT_NIIPM_UNITED(d);

                if (cancelFlag) return;
                var dl = selectedOrder.GetTree(d);

                if (dl == null) return;
                List<Operation> factOperations = new List<Operation>();
                foreach(var item in dl)
                {
                    if (cancelFlag) return;
                    var a = dispDB.GetFactOperation(item.OrderDetailId);
                    factOperations.AddRange(a);
                }
                if (cancelFlag) return;
                d.FactOperations = new List<Operation>();
                var e = factOperations.GroupBy(item => item.Name);
                foreach(var g in e)
                {
                    Operation f = new Operation();
                    f.Name = g.Key;
                    f.Time = TimeSpan.Zero;
                    foreach(var item in g)
                    {
                        f.AddFactTime(item);
                    }
                    d.FactOperations.Add(f);
                }
            }
        }
    }
}
