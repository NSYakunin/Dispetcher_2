using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Dispetcher2.Controls.MyGrid
{
    class MyCell : SourceGrid.Cells.Cell
    {
        public MyCell(object value): base(value)
        {
            SourceGrid.Cells.Views.Cell Cellview = new SourceGrid.Cells.Views.Cell();
            Cellview.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;

            View = Cellview;


        }

        public MyCell(object value, Type pType): base(value, pType)
        {
            SourceGrid.Cells.Views.Cell Cellview = new SourceGrid.Cells.Views.Cell();
            Cellview.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            Cellview.WordWrap = true;
            View = Cellview;

            /*SourceGrid.Cells.Editors.TextBoxNumeric numericEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(double));
            numericEditor.KeyPress += delegate(object sender, KeyPressEventArgs e)
            {
                //if (e.KeyChar == ',') e.KeyChar = '.';

                bool isValid = char.IsNumber(e.KeyChar) ||
                    e.KeyChar == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

                e.Handled = !isValid;
            };
            this.Editor = numericEditor;*/
            SourceGrid.Cells.Editors.TextBoxNumeric stringEditor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(string));
            stringEditor.KeyPress += delegate(object sender, KeyPressEventArgs e)
            {
                if (e.KeyChar == ',') e.KeyChar = '.';


            };
            this.Editor = stringEditor;

        }





        //new SourceGrid.Cells.Cell(1.5, typeof(double));




    }



}
