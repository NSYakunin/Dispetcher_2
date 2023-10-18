using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Dispetcher2.Class;

using Dispetcher2.Models;

namespace Dispetcher2.Class
{
    // Служебный клас, который занимается обновлением таблицы Sp_Kit
    class KitUpdater
    {
        IConfig config;
        DataTable detailTable;
        DataTable kitTable;

        // Внешняя зависимость! Надо заменить на шаблон Repository (Хранилище)
        C_DataBase DB;

        ProgressViewModel pvm;

        Task mainTask = null;
        bool stopFlag = false;
        
        public event EventHandler FinishEvent;

        public KitUpdater(IConfig config, ProgressViewModel value)
        {
            this.config = config;
            this.pvm = value;
            DB = new C_DataBase(config);
        }

        public void Start()
        {
            if (mainTask == null)
            {
                stopFlag = false;
                mainTask = new Task(this.Main);
                mainTask.Start();
            }
        }

        public void Stop()
        {

            if (mainTask != null)
            {
                stopFlag = true;
                mainTask.Wait();
                mainTask.Dispose();
                mainTask = null;
            }
        }

        void Main()
        {
            pvm.Progress = 0;
            pvm.Status = "Начало работы...";
            LoadDataTables();

            int i = 0;
            int cnt = detailTable.Rows.Count;
            while (stopFlag == false && i < cnt)
            {
                try
                {
                    DataRow r = detailTable.Rows[i];
                    long id = r.Field<long>("IdLoodsman");
                    ProcessDetail(id);

                    if (i % 10 == 0)
                    {
                        pvm.Progress = 100.0 * i / (double)cnt;
                        pvm.Status = String.Format("Выполнено: {0}%...", pvm.Progress.ToString("0.00"));
                    }
                }
                catch (Exception ex)
                {
                    // Так нельзя делать из другого потока, будет исключение:
                    //string msg = ex.Message;
                    //pvm.ErrorCollection.Add(msg);
                    // Можно так:
                    ErrorItem ei1 = new ErrorItem(i, ex.Message);
                    pvm.AddToList(ei1);
                }
                i = i + 1;
            }
            pvm.Status = "Обновление завершено.";
            pvm.Progress = 100;
            
            ErrorItem ei2 = new ErrorItem(0, "Ошибок не произошло");
            pvm.AddToList(ei2);

            if (FinishEvent != null) FinishEvent(this, new EventArgs());
            mainTask = null;
        }

        void ProcessDetail(long id)
        {
            var edr = GetKitDataTable(id);

            if (edr.Any())
            {
                DB.DeleteKit(id);
                foreach (DataRow r in edr)
                {
                    int idk = r.Field<int>("id");
                    string name = r.Field<string>("product").Trim();
                    double minquantity = r.Field<double>("minquantity");
                    int idtype = r.Field<int>("idtype");
                    int idstate = r.Field<int>("idstate");

                    DB.InsertKit(id, idk, name, minquantity, idtype, idstate);
                }
            }
        }

        IEnumerable<DataRow> GetKitDataTable(long idl)
        {

            DataTable dt = kitTable;
            var kits = from dr in dt.AsEnumerable()
                       where dr.Field<int>("idparent") == idl
                       //orderby dr.Field<string>("product") // тут сортировка не нужна, без нее быстрее
                       select dr;
            return kits;
        }

        void LoadDataTables()
        {
            detailTable = DB.GetDetail();
            kitTable = DB.GetAllKits();
        }
    }
}
