using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dispetcher2.Class
{
    public class Detail
    {
        public string NameType { get; set; }
        public int Position { get; set; }
        public string Shcm { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string AllPositionParent { get; set; }
        public long IdOrderDetail { get; set; }
        public long IdDetail { get; set; }
        public int PositionParent { get; set; }
        public string ShcmAndName { get { return Shcm + "\n" + Name; } }
        public void SetNameType(object value)
        {
            // Не NULL
            NameType = Convert.ToString(value);
        }
        public void SetPosition(object value)
        {
            // NULL
            // тип данных SQL: INT
            if (value is DBNull) Position = 0;
            else Position = Convert.ToInt32(value);
        }
        public void SetShcm(object value)
        {
            // Не NULL
            Shcm = Convert.ToString(value);
        }
        public void SetName(object value)
        {
            // NULL
            // тип данных SQL: VARCHAR(MAX)
            if (value is DBNull) Name = String.Empty;
            else Name = Convert.ToString(value);
        }

        public void SetAmount(object value)
        {
            // NULL
            // тип данных SQL: INT
            if (value is DBNull) Amount = 0;
            else Amount = Convert.ToInt32(value);
        }

        public void SetAllPositionParent(object value)
        {
            // NULL
            // тип данных SQL: VARCHAR(MAX)
            if (value is DBNull) AllPositionParent = String.Empty;
            else AllPositionParent = Convert.ToString(value);
        }
        public void SetIdOrderDetail(object value)
        {
            // Не NULL
            IdOrderDetail = Convert.ToInt64(value);
        }
        public void SetIdDetail(object value)
        {
            // Не NULL
            IdDetail = Convert.ToInt64(value);
        }
        public void SetPositionParent(object value)
        {
            // NULL
            // тип данных SQL: INT
            if (value is DBNull) PositionParent = 0;
            else PositionParent = Convert.ToInt32(value);
        }
    }
}
