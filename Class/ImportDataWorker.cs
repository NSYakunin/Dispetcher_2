using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

using Dispetcher2.Models;

namespace Dispetcher2.Class
{
    public class ImportDataWorker
    {
        C_DataBase dispDB = new C_DataBase(C_Gper.ConnStrDispetcher2);
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
                string name = Path.GetFileName(dir);
                string s = DateTime.Now.ToString() + ": Обработка файла " + name;
                ei = new ErrorItem(i+1, s);
                ei.Tag = name;
                pvm.Status = s;
                pvm.AddToList(ei);
                ProcessFile(dir);

                pvm.Progress = (double)(i + 1) * 100.0 / p2;
            }

            pvm.Status = "Обновление завершено.";
            pvm.Progress = 100;

            // в следующий раз перечитаем таблицу Orders чтобы обновить соответствия
            orderDt.Dispose();
            orderDt = null;

            if (FinishEvent != null) FinishEvent(this, new EventArgs());
            mainTask = null;
        }

        void ProcessFile(string name)
        {
            ErrorItem ei;
            try
            {
                Receipt rec = ce.ReadExcel_1C(name);
                if (rec.ErrorList.Count > 0)
                {
                    //foreach (var e in rec.ErrorList) pvm.AddToList(e);
                    ei = new ErrorItem(666, "Файл содержит ошибки. Обработка прекращена: " + name);
                    ei.Tag = name;
                    pvm.AddToList(ei);
                    return;
                }

                var id = GetOrderId(rec.OrderNum1С);
                if (id == null)
                {
                    ei = new ErrorItem(666, "Не найдено соответствие по полю №заказа лимитки в БД ПО \"Диспетчер\": " + rec.OrderNum1С);
                    ei.Tag = rec.OrderNum1С;
                    pvm.AddToList(ei);
                    return;
                }

                if (rec.ReceiptData.Rows.Count < 1)
                {
                    ei = new ErrorItem(666, "В лимитной накладной нет строк с данными: " + name);
                    ei.Tag = name;
                    pvm.AddToList(ei);
                    return;
                }
                int year = rec.DateLimit.Year;
                string num = rec.NumLimit;

                dispDB.DeleteRelationsKit(year, num);

                foreach(DataRow r in rec.ReceiptData.Rows)
                {
                    int Position = r.Field<int>("Position");
                    int IdLoodsman = r.Field<int>("IdLoodsman");
                    long PK_1С_IdKit = r.Field<long>("PK_1С_IdKit");
                    string Name1CKit = r.Field<string>("Name1CKit").Trim();
                    double AmountKit = r.Field<double>("AmountKit");

                    dispDB.SetSpKit1C(PK_1С_IdKit, Name1CKit);
                    dispDB.InsertRelationsKit(year, num, Position, id.Value, IdLoodsman, PK_1С_IdKit, rec.DateLimit, AmountKit);
                }

                string name2 = Path.GetFileName(name);
                name2 = Path.Combine(pvm.WayToFolderArchive, name2);
                if (File.Exists(name2)) File.Delete(name2);
                File.Move(name, name2);

                // tB_Logs.Text += DateTime.Now.ToString() + ": Завершено." + Environment.NewLine;
                ei = new ErrorItem(1000, DateTime.Now.ToString() + ": Завершено.");
                pvm.AddToList(ei);
            }
            catch (Exception ex)
            {
                ei = new ErrorItem(666, ex.Message);
                pvm.AddToList(ei);
            }
        }

        //Делаем запрос к базе на наличие OrderNum1С в таблице Orders
        Nullable<int> GetOrderId(string orderNum1C)
        {
            if (orderDt == null) orderDt = dispDB.GetAllOrders();

            var ords = from dr in orderDt.AsEnumerable()
                       where dr.Field<string>("OrderNum1С") == orderNum1C
                       select dr;

            if (ords.Any())
                foreach (var r in ords) return r.Field<int>("PK_IdOrder");
            return null;
        }
        /*
                C_DataBase DB_Dispetcher = new C_DataBase(C_Gper.ConnStrDispetcher2);
                
                string sql = "Select PK_IdOrder From Orders" + "\n" +
                             "Where OrderNum1С = '" + _OrderNum + "'";
                _PK_IdOrder = DB_Dispetcher.SqlDataReader(sql);//FK_IdOrder//в таблице Orders

                if (_PK_IdOrder == 0) MessageBox.Show("Не найдено соответствие по полю №заказа лимитки в БД ПО \"Диспетчер\"", "Внимание!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        */
    }
}
