using System;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
//using System.Net;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop;
using System.Runtime.InteropServices;
using Dispetcher2.Class;
using System.Data;
using System.Data.SqlClient;

namespace Dispetcher2.Class
{
    class C_Excel
    {


#region Загрузка данных из расцеховки
        public void ReadExcelRas(string _WayFile,int _PK_IdOrder, ref System.Data.DataTable _DT_Excel, out string _ErrorsDataPos)
        {
            _ErrorsDataPos = "";
            try
            {
                _DT_Excel.Clear();
                if (_DT_Excel.Columns.Count == 0)
                {
                    _DT_Excel.Columns.Add("Position",typeof(int));
                    _DT_Excel.Columns.Add("PositionParent", typeof(int));
                    _DT_Excel.Columns.Add("AllPositionParent", typeof(string));
                    _DT_Excel.Columns.Add("FK_IdOrder", typeof(int));
                    _DT_Excel.Columns.Add("FK_IdDetail", typeof(long));
                    _DT_Excel.Columns.Add("AmountDetails", typeof(int));
                    _DT_Excel.Columns.Add("AmountFasteners", typeof(double));
                    _DT_Excel.Columns.Add("ShcmDetail", typeof(string));//or Name Fasteners
                    _DT_Excel.Columns.Add("MeasureUnit", typeof(string));
                }
                
                //string WayExcel = Cs_Gper.OnlyWayFile + "/" + Cs_Gper.NameFile;
                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application() { Visible = true };
                Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
                Microsoft.Office.Interop.Excel.Range ExcelRange;
                ExcelWorkBook = Excel.Workbooks.Open(_WayFile, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);
                ExcelRange = ExcelWorkSheet.UsedRange;
                //MessageBox.Show("Строки"+ExcelRange.Rows.Count.ToString());
                //MessageBox.Show("Столбцы" + ExcelRange.Columns.Count.ToString());
                //Делаем проверку заголовка столбцов
                bool ErrorHeader = false;
                
                if ((ExcelRange.Cells[8, 1] as Range).Value2.ToString().Trim().ToLower() != "поз." &&
                    (ExcelRange.Cells[8, 2] as Range).Value2.ToString().Trim() != "Обозначение" &&
                    (ExcelRange.Cells[8, 3] as Range).Value2.ToString().Trim() != "Наименование" &&
                    (ExcelRange.Cells[8, 4] as Range).Value2.ToString().Trim() != "Кол-во шт" &&
                    (ExcelRange.Cells[8, 5] as Range).Value2.ToString().Trim() != "Куда входит" &&
                    (ExcelRange.Cells[8, 6] as Range).Value2.ToString().Trim() != "Марка материала" &&
                    (ExcelRange.Cells[8, 7] as Range).Value2.ToString().Trim() != "Размер заготовки" &&
                    (ExcelRange.Cells[8, 8] as Range).Value2.ToString().Trim() != "Кол-во заг-вок" &&
                    (ExcelRange.Cells[8, 9] as Range).Value2.ToString().Trim() != "Маршрут изготовления" &&
                    (ExcelRange.Cells[8, 10] as Range).Value2.ToString().Trim() != "Норма расхода на ед" &&
                    (ExcelRange.Cells[8, 11] as Range).Value2.ToString().Trim() != "Норма расхода на изд." &&
                    (ExcelRange.Cells[8, 12] as Range).Value2.ToString().Trim() != "Ед изм" &&
                    (ExcelRange.Cells[8, 13] as Range).Value2.ToString().Trim() != "Примечание"
                    ) ErrorHeader = true;

                if (ErrorHeader) MessageBox.Show("Формат файла не соответствует шаблону.", "Загрузка данных отменена!!!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                else//Если нет ошибок
                {
                    object Val = null;
                    string NameDetail = "";//Надо только для крепежа
                    string Shcm = "", AllPositionParent = "";
                    int pos = 0,amount=-1, PositionParent = 0;
                    double AmountFasteners = -1;
                    string teh = "";
                    long _PK_IdDetail;
                    string Note = "";//примечание
                    string MeasureUnit = "";//единица измерения
                    bool err = false;
                    for (int NumRows = 9; NumRows <= ExcelRange.Rows.Count; NumRows++)
                    //for (int NumRows = 9; NumRows <= 10; NumRows++)
                    {
                        amount = -1; AmountFasteners = -1; err = false;
                        //Считываем данные строки
                        Val = (ExcelRange.Cells[NumRows, 1] as Range).Value2;//поз.
                        if ((Val == null)) pos = 0; 
                        else
                        if (!int.TryParse(Val.ToString(),out pos)) pos = 0;

                        Val = (ExcelRange.Cells[NumRows, 2] as Range).Value2;//Обозначение
                        if (Val == null) Shcm = ""; else Shcm = Val.ToString().Trim();

                        Val = (ExcelRange.Cells[NumRows, 3] as Range).Value2;//Наименование
                        if (Val == null) NameDetail = ""; else NameDetail = Val.ToString().Trim();
                        if (Shcm == "" && NameDetail == "") break;//Конец списка
                        

                        Val = (ExcelRange.Cells[NumRows, 4] as Range).Value2;//Кол-во шт
                        if ((Val != null))
                        {
                            if (Shcm.ToLower() != "к")//т.е. деталь
                            {
                                if (!int.TryParse(Val.ToString(), out amount)) amount =-1;
                            }
                            else
                            {
                                //if (Val is double) AmountFasteners = (double)Val;
                                if (!double.TryParse(Val.ToString(), out AmountFasteners)) AmountFasteners=-1;
                            }
                        }
                        Val = (ExcelRange.Cells[NumRows, 5] as Range).Value2;//Куда входит
                        if (Val == null)
                        {
                            PositionParent = 0;
                            AllPositionParent = "";
                        }
                        else
                        {
                            AllPositionParent = Val.ToString().Trim();
                            if (AllPositionParent.IndexOf(",") > 0)
                                AllPositionParent = AllPositionParent.Remove(AllPositionParent.IndexOf(","));
                            if (!int.TryParse(AllPositionParent, out PositionParent) || pos==PositionParent) PositionParent = 0;
                            AllPositionParent = Val.ToString().Trim();
                        }
                        Val = (ExcelRange.Cells[NumRows, 9] as Range).Value2;//Маршрут изготовления
                        if (Val == null) teh = ""; else teh = Val.ToString().Trim();

                        Val = (ExcelRange.Cells[NumRows, 12] as Range).Value2;//единица измерения
                        if (Val == null) MeasureUnit = ""; else MeasureUnit = Val.ToString().Trim();
                        Val = (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;//Примечание
                        if (Val == null) Note = ""; else Note = Val.ToString().Trim();
                        //Проверка на пустые значения
                        if (pos == 0 || Shcm == "" || NameDetail == "" || (AmountFasteners == -1 & Shcm.ToLower() == "к") || (amount == -1 & Shcm.ToLower() != "к") || (Shcm.ToLower() == "к" & teh != "") || Note != "")// || (Shcm.ToLower() == "к" & MeasureUnit==""))
                        {
                            ((Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.get_Range("A" + NumRows, "M" + NumRows)).Interior.Color = Color.LightPink;
                            if (_ErrorsDataPos == "") _ErrorsDataPos += pos.ToString(); else _ErrorsDataPos += " ," + pos.ToString();
                            err = true;
                        }
                        //Проверка значений и последующая их обработка
                        if (Note != "")
                            (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Есть Примечание" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;
                        if (pos == 0)
                            (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Отсутствует позиция" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;
                        if (Shcm == "")
                            (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Отсутствует ЩЦМ" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;
                        if (NameDetail == "")
                            (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Отсутствует наименование" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;
                        if ((AmountFasteners == -1 & Shcm.ToLower() == "к")||(amount == -1 & Shcm.ToLower() != "к"))
                            (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Отсутствует количество" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;
                        if (Shcm.ToLower() == "к" & teh != "")//Есть технология(делаем мы), значит надо проводить как обычную деталь
                           (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Есть технология!" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;
                        /*if (Shcm.ToLower() == "к" & MeasureUnit == "")
                            (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Отсутствует единица измерения!" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;*/
                        if (pos > 0 & Shcm != "" & err == false)
                        {
                            if (Shcm.ToLower() == "к")//Крепёж
                            {
                                if (Check_DetailOrFastenersInOrder(_PK_IdOrder, pos))
                                {
                                    ((Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.get_Range("A" + NumRows, "M" + NumRows)).Interior.Color = Color.LightPink;
                                    (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Крепёж с такой позицией - был загружен ранее!!!" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;
                                    if (_ErrorsDataPos == "") _ErrorsDataPos += pos.ToString(); else _ErrorsDataPos += " ," + pos.ToString();
                                }
                                else//Грузим крепёж
                                {
                                    if (Note == "")
                                        {
                                            ((Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.get_Range("A" + NumRows, "M" + NumRows)).Interior.Color = Color.LightCyan;
                                            _DT_Excel.Rows.Add(pos, PositionParent, AllPositionParent, _PK_IdOrder, 0, amount, AmountFasteners, NameDetail, MeasureUnit);
                                        }
                                }
                            }
                            else//Сборки/Детали
                            {
                                //Если есть в справочнике деталей
                                if (C_Orders.Check_ShcmDetail(Shcm, out _PK_IdDetail))//(ExcelRange.Cells[NumRows, 5] as Range).Interior.Color = Color.LightGreen;
                                {    //Если загружалась ранее (по позиции и ЩЦМ)
                                    if (Check_DetailOrFastenersInOrder(_PK_IdOrder, pos))
                                    {
                                        ((Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.get_Range("A" + NumRows, "M" + NumRows)).Interior.Color = Color.LightPink;
                                        (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Сборка/деталь с такой позицией - была загружена ранее!!!" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;
                                        if (_ErrorsDataPos == "") _ErrorsDataPos += pos.ToString(); else _ErrorsDataPos += " ," + pos.ToString();
                                    }
                                    else
                                        if (Note == "")//Грузим Сборки/Детали
                                        {
                                            ((Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.get_Range("A" + NumRows, "M" + NumRows)).Interior.Color = Color.LightGreen;
                                            _DT_Excel.Rows.Add(pos, PositionParent, AllPositionParent, _PK_IdOrder, _PK_IdDetail, amount, AmountFasteners, Shcm, MeasureUnit);
                                        }
                                }
                                else//Если нет в справочнике деталей
                                {
                                    ((Microsoft.Office.Interop.Excel.Range)ExcelWorkSheet.get_Range("A" + NumRows, "M" + NumRows)).Interior.Color = Color.LightPink;
                                    (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2 = "Сборка/деталь отсутствует в справочнике" + "\n" + (ExcelWorkSheet.Cells[NumRows, "M"] as Range).Value2;
                                    if (_ErrorsDataPos == "") _ErrorsDataPos += pos.ToString(); else _ErrorsDataPos += " ," + pos.ToString();
                                }
                            }
                        }
                    }
                }
                //Excel.Visible = true;
                //***ExcelWorkBook.Close(true, null, null);
                //ExcelWorkBook.Close(_WayFile);
                //Excel.Quit();
                //ReleaseExcel(ExcelWorkSheet);
                //ReleaseExcel(ExcelWorkBook);
                //ReleaseExcel(Excel);//*************************************************
                //ReleaseExcel(Excel as Object);
                
                //if (CritError) return false; else return true;
                //dataGridView1.DataSource = dsGal.Tables["ExcelGal"].DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public bool InsertDetailsInOrderFromExcel(System.Data.DataTable _DT)
        {
            try
            {

                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand();//using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
                
                cmd.Parameters.Add(new SqlParameter("@Position", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@PositionParent", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@AllPositionParent", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@FK_IdOrder", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@FK_IdDetail", SqlDbType.BigInt));
                cmd.Parameters.Add(new SqlParameter("@Amount", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@AmountFasteners", SqlDbType.Float));
                cmd.Parameters.Add(new SqlParameter("@NameFasteners", SqlDbType.VarChar));
                cmd.Parameters.Add(new SqlParameter("@MeasureUnit", SqlDbType.VarChar));
                foreach (DataRow row in _DT.Rows)
                {
                    if (row.ItemArray[4].ToString() == "0")//Крепёж
                        cmd.CommandText = "insert into OrdersFasteners (Position,PositionParent,AllPositionParent,FK_IdOrder,NameFasteners,AmountFasteners,MeasureUnit) " + "\n" +
                                  "values (@Position,@PositionParent,@AllPositionParent,@FK_IdOrder,@NameFasteners,@AmountFasteners,@MeasureUnit)";
                    else//Сборки/Детали
                        cmd.CommandText = "insert into OrdersDetails (Position,PositionParent,AllPositionParent,FK_IdOrder,FK_IdDetail,AmountDetails) " + "\n" +
                                  "values (@Position,@PositionParent,@AllPositionParent,@FK_IdOrder,@FK_IdDetail,@Amount)";
                    //Parameters**************************************************
                    cmd.Parameters["@Position"].Value = row.ItemArray[0];
                    cmd.Parameters["@PositionParent"].Value = row.ItemArray[1];
                    cmd.Parameters["@AllPositionParent"].Value = row.ItemArray[2];
                    cmd.Parameters["@FK_IdOrder"].Value = row.ItemArray[3];
                    cmd.Parameters["@FK_IdDetail"].Value = row.ItemArray[4];
                    cmd.Parameters["@Amount"].Value = row.ItemArray[5];
                    cmd.Parameters["@AmountFasteners"].Value = row.ItemArray[6];
                    cmd.Parameters["@NameFasteners"].Value = row.ItemArray[7];
                    cmd.Parameters["@MeasureUnit"].Value = row.ItemArray[8];
                    //***********************************************************
                    C_Gper.con.Open();
                    cmd.ExecuteNonQuery();
                    C_Gper.con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                C_Gper.con.Close();
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool Check_DetailOrFastenersInOrder(int _FK_IdOrder, int _Position)
        {
            try
            {
                C_Gper.con.ConnectionString = C_Gper.ConnStrDispetcher2;
                SqlCommand cmd = new SqlCommand() { CommandTimeout = 60 };//seconds //using System.Data.SqlClient;
                cmd.Connection = C_Gper.con;
                cmd.Parameters.Clear();
                cmd.CommandText = "Select Top(1) Position From OrdersDetails" + "\n" +
                                  "Where FK_IdOrder=@FK_IdOrder and Position=@Position" + "\n" +
                                  "union all" + "\n" +
                                  "Select Top(1) Position From OrdersFasteners" + "\n" +
                                  "Where FK_IdOrder=@FK_IdOrder and Position=@Position";
                cmd.Parameters.Add(new SqlParameter("@FK_IdOrder", SqlDbType.Int));
                cmd.Parameters["@FK_IdOrder"].Value = _FK_IdOrder;
                cmd.Parameters.Add(new SqlParameter("@Position", SqlDbType.Int));
                cmd.Parameters["@Position"].Value = _Position;
                using (C_Gper.con)
                {
                    C_Gper.con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {/*while (reader.Read()){if (reader.IsDBNull(0) == false) ffff = reader.GetInt32(0);}*/
                            reader.Dispose(); reader.Close(); C_Gper.con.Close();
                            return true;
                        }
                        else
                        {
                            reader.Dispose(); reader.Close(); C_Gper.con.Close();
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "ОШИБКА!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        #endregion

        public Receipt ReadExcel_1C(string wayFile)
        {
            Receipt rec = new Receipt();
            try
            {

                Microsoft.Office.Interop.Excel.Application Excel = new Microsoft.Office.Interop.Excel.Application() { Visible = false, DisplayAlerts = false };
                Microsoft.Office.Interop.Excel.Workbook ExcelWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet ExcelWorkSheet;
                Microsoft.Office.Interop.Excel.Range ExcelRange;
                ExcelWorkBook = Excel.Workbooks.Open(wayFile, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                ExcelWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);
                ExcelRange = ExcelWorkSheet.UsedRange;
                object _Val = null;
                string _OrderNum = null, _Name1CKit = null;
                long _IdLoodsman, _PK_1С_IdKit;
                double _AmountKit;
                int _Position = 0;
                _Val = (ExcelRange.Cells[2, 4] as Range).Value2;//_OrderNum
                if (_Val == null) _OrderNum = ""; else _OrderNum = _Val.ToString().Trim();

                _Val = (ExcelRange.Cells[4, 2] as Range).Value2;//_NumLimit
                if (_Val == null) rec.NumLimit = ""; 
                    else rec.NumLimit = _Val.ToString().Remove(0, _Val.ToString().IndexOf("№") + 1).Trim();
                _Val = (ExcelRange.Cells[4, 9] as Range).Value2;//_DateLimit
                if (_Val != null) DateTime.TryParse(_Val.ToString(), out rec.DateLimit);
                if (rec.NumLimit != "" & rec.DateLimit.Year > 1)
                    for (int NumRows = 19; NumRows <= ExcelRange.Rows.Count - 5; NumRows++)
                    {
                        //_Position
                        _Val = (ExcelRange.Cells[NumRows, 1] as Range).Value2;//_Position
                        if (_Val == null | !int.TryParse(_Val.ToString(), out _Position)) rec.AddError("_Position - err., ");
                        _Val = (ExcelRange.Cells[NumRows, 3] as Range).Value2;//_IdLoodsman
                        if (_Val == null | !long.TryParse(_Val.ToString(), out _IdLoodsman)) rec.AddError("_IdLoodsman - err., ");
                        _Val = (ExcelRange.Cells[NumRows, 4] as Range).Value2;//_PK_1С_IdKit
                        if (_Val == null | !long.TryParse(_Val.ToString(), out _PK_1С_IdKit)) rec.AddError("_PK_1С_IdKit - err., ");
                        _Val = (ExcelRange.Cells[NumRows, 5] as Range).Value2;//_Name1CKit
                        if (_Val == null) _Name1CKit = ""; else _Name1CKit = _Val.ToString().Trim();
                        _Val = (ExcelRange.Cells[NumRows, 9] as Range).Value2;//_AmountKit
                        if (_Val == null | !double.TryParse(_Val.ToString(), out _AmountKit)) rec.AddError("_AmountKit - err., ");
                        if (rec.ErrorList.Count > 0)
                        {
                            rec.ReceiptData.Clear();
                            break;
                        }
                        else
                            rec.ReceiptData.Rows.Add(_Position, _IdLoodsman, _PK_1С_IdKit, _Name1CKit, _AmountKit);
                    }//for

                Excel.Quit();
                
                Excel = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не работает. " + ex.Message, "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return rec;
        }
    }
}
