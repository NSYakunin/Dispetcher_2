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
        C_DataBase db;
        bool cancelFlag = false;
        public event Action Finished;
        Task mainTask = null;

        public LaborLoader(Order selectedOrder)
        {
            this.selectedOrder = selectedOrder;
            db = new C_DataBase(C_Gper.ConnStrDispetcher2);
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
            var dl = db.GetOrderDetailAndFastener(selectedOrder.Id);
            if (dl != null)
            {
                
                var dl2 = from d in dl
                          where d.PositionParent == 0
                          select d;
                selectedOrder.DetailList = new List<Detail>();
                
                foreach(var item in dl2)
                {
                    if (item.IdLoodsman > 0) selectedOrder.DetailList.Add(item);
                }
            }
        }

        void LoadOperation()
        {
            foreach(var d in selectedOrder.DetailList)
            {
                if (cancelFlag) return;
                db.Call_rep_VEDOMOST_TRUDOZATRAT_NIIPM_UNITED(d);
                
            }
        }
    }
}
