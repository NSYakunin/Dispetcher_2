using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

using Dispetcher2.Class;

namespace Dispetcher2.DataAccess
{
    class ExcelLaborReportWriter : LaborReportWriter
    {
        bool errorFlag;
        string errorMessage;
        Excel.Application app;
        Excel.Worksheet worksheet;
        
        public override void Write(IEnumerable<string> columns, IEnumerable<LaborReportRow> rows, string H1, string H2)
        {
            Initialize();
            if (app != null)
            {
                Main(columns, rows, H1, H2);
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
            Excel.Workbooks wbs = app.Workbooks;
            Excel.Workbook workbook = wbs.Add(1);
            Excel.Sheets sheets = app.Worksheets;
            worksheet = (Excel.Worksheet)sheets.get_Item(1);
            // сборка мусора
            sheets = null;
            workbook = null;
            wbs = null;
        }
        void Finish()
        {
            app.Visible = true;
        }
        void Main(IEnumerable<string> columns, IEnumerable<LaborReportRow> rows, string H1, string H2)
        {
            bool f = true;
            int firstCol = 1;
            int lastCol = 0;
            int firstRow = 4;
            int lastRow = 0;

            int numCol = firstCol + 1;
            foreach (string c in columns)
            {

                worksheet.Columns[numCol].ColumnWidth = 12;
                worksheet.Cells[firstRow, numCol] = c;
                int numRow = firstRow + 1;
                foreach (LaborReportRow row in rows)
                {
                    if (f)
                    {
                        worksheet.Cells[numRow, firstCol] = row.Name;
                    }
                    if (row.Operations.ContainsKey(c))
                    {
                        Excel.Range r = worksheet.Cells[numRow, numCol];
                        r.Value = row.Operations[c];
                        r.HorizontalAlignment = Excel.Constants.xlLeft;
                    }
                        
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

            SetHeader(H1, 1, firstCol, lastCol, true);
            SetHeader(H2, 2, firstCol, lastCol, false);
        }

        void SetLineStyle(int firstRow, int firstCol, int lastRow, int lastCol)
        {
            Excel.Range c1 = worksheet.Cells[firstRow, firstCol];
            Excel.Range c2 = worksheet.Cells[lastRow, lastCol];
            Excel.Range r = worksheet.get_Range(c1, c2);
            r.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
        }
        void SetHeader(string text, int row, int firstCol, int lastCol, bool bold)
        {
            Excel.Range c1 = worksheet.Cells[row, firstCol];
            Excel.Range c2 = worksheet.Cells[row, lastCol];
            Excel.Range r = worksheet.get_Range(c1, c2);
            r.MergeCells = true;
            if (bold) r.Font.Bold = 1;
            r.Value = text;
            r.HorizontalAlignment = Excel.Constants.xlCenter;
        }
    }
}
