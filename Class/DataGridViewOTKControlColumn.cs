using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dispetcher2.Class
{
    public class DataGridViewOTKControlColumn : DataGridViewColumn
    {
        public DataGridViewOTKControlColumn()
            : base(new DataGridViewOTKControlCell())
        {
            this.ValueType = typeof(CheckBoxState[]);
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewOTKControlCell)))
                {
                    throw new InvalidCastException("Ячейка должна быть типа DataGridViewOTKControlCell");
                }
                base.CellTemplate = value;
            }
        }
    }
}
