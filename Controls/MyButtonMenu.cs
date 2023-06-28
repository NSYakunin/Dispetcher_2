using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dispetcher2.Controls
{
    class MyButtonMenu:Button
    {
        public MyButtonMenu()
        {
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FlatAppearance.BorderColor = SystemColors.ActiveCaption;
            this.MouseEnter += (s, e) => { this.BackColor = SystemColors.ActiveCaption; };
            this.MouseLeave += (s, e) => { this.BackColor = SystemColors.InactiveBorder; };
        }

    }
}
