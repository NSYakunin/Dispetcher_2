using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Dispetcher2.Controls.MyGrid
{
    class MyHeader : SourceGrid.Cells.ColumnHeader
    {
        public MyHeader(object value): base(value)
        {
            //1 Header Row
            SourceGrid.Cells.Views.ColumnHeader view = new SourceGrid.Cells.Views.ColumnHeader();
            //view.Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold);
            //view.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);

            view.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            View = view;

            AutomaticSortEnabled = false;
        }
    }
}
