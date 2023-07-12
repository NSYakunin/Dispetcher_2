using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Dispetcher2.Class
{
    public class ImportDataWorker
    {
        C_DataBase db = new C_DataBase(C_Gper.ConnStrDispetcher2);
        C_Excel ce = new C_Excel();

        DataTable orderDt = null;

        ProgressViewModel pvm;

        Task mainTask = null;
        bool stopFlag = false;

        public event EventHandler FinishEvent;

        public ImportDataWorker(ProgressViewModel value)
        {
            this.pvm = value;
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
            ErrorItem ei = null;
            pvm.Progress = 0;
            pvm.Status = "Поиск файлов Excel...";
            string WayToFolder = pvm.WayToFolder;
            string WayToArchive = pvm.WayToFolderArchive;
            if (WayToFolder.Trim().Length < 1 || !Directory.Exists(WayToFolder))
            {
                ei = new ErrorItem(1, "Не указан путь к папке с файлами Excel или она не существует");
                pvm.AddToList(ei);
                if (FinishEvent != null) FinishEvent(this, new EventArgs());
                return;
            }

            if (WayToArchive.Trim().Length < 1 || !Directory.Exists(WayToArchive))
            {
                ei = new ErrorItem(1, "Не указан путь к папке архива  или она не существует");
                pvm.AddToList(ei);
                if (FinishEvent != null) FinishEvent(this, new EventArgs());
                return;
            }

            string[] dirs = Directory.GetFiles(WayToFolder, "*.xls", SearchOption.TopDirectoryOnly);

            double p2 = dirs.Length;
            
            for(int i = 0; i < dirs.Length; i++)
            {
                if (stopFlag == true) break;
                var dir = dirs[i];
                string s = DateTime.Now.ToString() + ": Обработка файла " + Path.GetFileName(dir);
                ei = new ErrorItem(i+1, s);
                pvm.Status = s;
                pvm.ErrorList.Add(ei);
                ProcessFile(dir);

                pvm.Progress = (double)(i + 1) * 100.0 / p2;
            }
        }

        void ProcessFile(string name)
        {
            try
            {
                Receipt rec = ce.ReadExcel_1C(name);
                if (rec.ErrorList.Count > 0)
                {
                    //foreach (var e in rec.ErrorList) pvm.AddToList(e);
                    ErrorItem ei2 = new ErrorItem(666, "Файл содержит ошибки. Обработка прекращена.");
                    pvm.AddToList(ei2);
                    return;
                }

                var id = GetOrderId(rec.OrderNum1С);
                if (id == null)
                {
                    ErrorItem ei2 = new ErrorItem(666, "Не найдено соответствие по полю №заказа лимитки в БД ПО \"Диспетчер\"");
                    pvm.AddToList(ei2);
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorItem ei = new ErrorItem(666, ex.Message);
                pvm.AddToList(ei);
            }
        }

        Nullable<int> GetOrderId(string orderNum1C)
        {
            if (orderDt == null) db.GetAllOrders();

            var ords = from dr in orderDt.AsEnumerable()
                       where dr.Field<string>("OrderNum1С").Trim() == orderNum1C
                       select dr;

            if (ords.Any<DataRow>())
                foreach (var r in ords) return r.Field<int>("PK_IdOrder");
            return null;
        }
        /*
        //Делаем запрос к базе на наличие OrderNum1С в таблице Orders
                C_DataBase DB_Dispetcher = new C_DataBase(C_Gper.ConnStrDispetcher2);
                
                string sql = "Select PK_IdOrder From Orders" + "\n" +
                             "Where OrderNum1С = '" + _OrderNum + "'";
                _PK_IdOrder = DB_Dispetcher.SqlDataReader(sql);//FK_IdOrder//в таблице Orders

                if (_PK_IdOrder == 0) MessageBox.Show("Не найдено соответствие по полю №заказа лимитки в БД ПО \"Диспетчер\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        */
    }
}
