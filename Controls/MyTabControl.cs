using System;
using System.Windows.Forms;

namespace Dispetcher2
{
    class MyTabControl : TabControl
    {
        protected override void WndProc(ref Message m)
        {
            const int TCM_ADJUSTRECT = 0x1328;
            if (m.Msg == TCM_ADJUSTRECT && !DesignMode) m.Result = (IntPtr)1;
            else base.WndProc(ref m);
        }
    }
}