using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dispetcher2.Class
{
    public abstract class FormFactory
    {
        public abstract Form GetForm(string purpose);
        public abstract string GetInformation();
    }
}
