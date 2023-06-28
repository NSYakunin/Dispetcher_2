using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dispetcher2.Controls
{
    class MyCheckBox:CheckBox
    {
        public MyCheckBox()
        {
            this.Appearance = System.Windows.Forms.Appearance.Button;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.FlatAppearance.BorderColor = SystemColors.ActiveCaption;
            this.MouseEnter += (s, e) => { this.BackColor = SystemColors.ActiveCaption; };
            this.MouseLeave += (s, e) =>{ if (this.Checked)this.BackColor = SystemColors.ActiveCaption; else this.BackColor = SystemColors.InactiveBorder; };
            this.CheckedChanged += (s, e) => { if (this.Checked)this.BackColor = SystemColors.ActiveCaption; else this.BackColor = SystemColors.InactiveBorder; };
        }
    }
}
