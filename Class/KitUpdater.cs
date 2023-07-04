using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using Dispetcher2.Class;

namespace Dispetcher2.Class
{
    // Служебный клас, который занимается обновлением таблицы Sp_Kit
    class KitUpdater
    {
        DataTable detailTable;
        DataTable kitTable;
        C_DataBase db = new C_DataBase(C_Gper.ConnStrDispetcher2);

        ProgressViewModel pvm;

        ManualResetEvent endEvent = new ManualResetEvent(false);
        bool stopFlag = false;
        bool workFlag = false;
        
        public event EventHandler FinishEvent;

        public delegate void ErrorDelegate(string text);
        public event ErrorDelegate NewError;

        public KitUpdater(ProgressViewModel value)
        {
            this.pvm = value;
        }

        public void Start()
        {
            if (workFlag) return;
            stopFlag = false;
            endEvent.Reset();
            var t = new Thread(this.Main);
            t.Start();
        }

        public void Stop()
        {
            if (workFlag == false) return;
            stopFlag = true;
            endEvent.WaitOne();
        }

        void Main()
        {
            workFlag = true;
            
            pvm.Progress = 0;
            pvm.Status = "Начало работы...";
            LoadDataTables();

            int i = 0;
            double cnt = detailTable.Rows.Count;
            while (stopFlag == false && i < cnt)
            {
                try
                {
                    DataRow r = detailTable.Rows[i];
                    long id = r.Field<long>("IdLoodsman");
                    ProcessDetail(id);

                    if (i % 10 == 0)
                    {
                        pvm.Progress = 100.0 * i / cnt;
                        pvm.Status = String.Format("Выполнено: {0}%...", pvm.Progress.ToString("0.00"));
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    // Так нельзя делать из другого потока, будет исключение:
                    //pvm.ErrorCollection.Add(msg);
                    // Можно так:
                    if (NewError != null) NewError(msg);
                }
                i = i + 1;
            }
            pvm.Status = "Обновление завершено.";
            if (pvm.ErrorCollection.Count < 1) if (NewError != null) NewError("Ошибок не произошло");
            if (FinishEvent != null) FinishEvent(this, new EventArgs());
            endEvent.Set();
            workFlag = false;
        }

        void ProcessDetail(long id)
        {
            DataTable kdt = GetKitDataTable(id);
            if (kdt.Rows.Count > 0)
            {
                db.DeleteKit(id);
                IEnumerable<DataRow> edr = kdt.AsEnumerable();
                foreach (DataRow r in edr)
                {
                    int idk = r.Field<int>("id");
                    string name = r.Field<string>("product").Trim();
                    double minquantity = r.Field<double>("minquantity");
                    int idtype = r.Field<int>("idtype");
                    int idstate = r.Field<int>("idstate");

                    db.InsertKit(id, idk, name, minquantity, idtype, idstate);
                }
            }
        }

        DataTable GetKitDataTable(long idl)
        {

            DataTable dt = kitTable;
            var kits = from dr in dt.AsEnumerable()
                       where dr.Field<int>("idparent") == idl
                       orderby dr.Field<string>("product")
                       select dr;
            int i = 0;
            foreach (var x in kits) i++;
            if (i > 0) return kits.CopyToDataTable<DataRow>();
            else return new DataTable();
        }

        void LoadDataTables()
        {
            kitTable = db.GetAllKits();
            detailTable = db.GetDetail();
        }
    }
}
