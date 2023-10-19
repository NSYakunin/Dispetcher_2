using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

using Dispetcher2.Class;
using System.Diagnostics;

namespace Dispetcher2.DataAccess
{
    class ExcelLaborReportWriter : LaborReportWriter
    {
        bool errorFlag;
        string errorMessage;
        Excel.Application app;
        Excel.Workbook workbook;
        Excel.Worksheet worksheet;
        public override void Write(StringRepository colrep, LaborReportRepository labrep)
        {
            Initialize();
            if (app != null)
            {
                Main(colrep, labrep);
                Finish();
            }
        }
        void Initialize()
        {
            errorFlag = false;
            errorMessage = String.Empty;

            app = new Excel.Application();
            if (app == null)
            {
                errorMessage = "Ошибка создания объекта Excel.Application";
                errorFlag = true;
                return;
            }
            app.Visible = false;
            workbook = app.Workbooks.Add(1);
            worksheet = (Excel.Worksheet)app.Sheets.get_Item(1);
        }
        void Finish()
        {
            app.Visible = true;
        }
        void Main(StringRepository colrep, LaborReportRepository labrep)
        {
            bool f = true;
            int firstCol = 1;
            int lastCol = 0;
            int firstRow = 1;
            int lastRow = 0;

            int numCol = firstCol + 1;
            foreach (string c in colrep.GetList())
            {

                worksheet.Columns[numCol].ColumnWidth = 12;
                worksheet.Cells[firstCol, numCol] = c;
                int numRow = firstRow + 1;
                foreach (LaborReportRow row in labrep.GetList())
                {
                    if (f)
                    {
                        worksheet.Cells[numRow, firstCol] = row.Name;
                        
                    }
                    if (row.Operations.ContainsKey(c)) 
                        worksheet.Cells[numRow, numCol] = row.Operations[c];
                    numRow++;
                }
                f = false;
                numCol++;
                lastRow = numRow - 1;
            }
            lastCol = numCol - 1;

            worksheet.Rows[firstRow].WrapText = true;
            worksheet.Rows[firstRow].AutoFit();

            worksheet.Columns[firstCol].ColumnWidth = 18;
            worksheet.Columns[firstCol].WrapText = true;

            SetLineStyle(firstRow, firstCol + 1, firstRow, lastCol);
            SetLineStyle(firstRow+1, firstCol, lastRow, lastCol);
        }

        void SetLineStyle(int firstRow, int firstCol, int lastRow, int lastCol)
        {
            Excel.Range c1 = worksheet.Cells[firstRow, firstCol];
            Excel.Range c2 = worksheet.Cells[lastRow, lastCol];
            Excel.Range r = worksheet.get_Range(c1, c2);
            r.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
        }
    }
}
