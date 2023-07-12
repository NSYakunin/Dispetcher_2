using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dispetcher2.Class
{
    public class ErrorItem
    {
        int idvalue;
        string textvalue;

        public ErrorItem(int id, string text)
        {
            this.idvalue = id;
            this.textvalue = text;
        }
        public int Id
        {
            get { return idvalue; }
            set { idvalue = value; }
        }

        public string Text
        {
            get { return textvalue; }
            set { textvalue = value; }
        }
    }
}
