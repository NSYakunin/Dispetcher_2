using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Dispetcher2.Class
{
    public class Order
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public bool ValidationOrder { get; set; }
        public string Num1С { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PlannedDate { get; set; }
        public int Amount { get; set; }
        public List<Detail> MainDetailList { get; set; }
        public List<Detail> DetailList { get; set; }

        public void SetId(object value)
        {
            // Не NULL
            Id = Convert.ToInt32(value);
        }

        public void SetNumber(object value)
        {
            // Не NULL
            Number = Convert.ToString(value);
        }
        public void SetName(object value)
        {
            if (value is DBNull) Name = String.Empty;
            else Name = Convert.ToString(value);
        }
        public void SetCreateDate(object value)
        {
            // Не NULL
            // тип данных SQL: DATE
            CreateDate = Convert.ToDateTime(value);
        }
        public void SetStatus(object value)
        {
            // тип данных SQL: TINYINT
            if (value is DBNull) Status = 0;
            else Status = Convert.ToInt32(value);
        }
        public void SetValidationOrder(object value)
        {
            // Не NULL
            // тип данных SQL: BIT
            int i = Convert.ToInt32(value);
            if (i == 0) ValidationOrder = false;
            else ValidationOrder = true;
        }
        public void SetNum1С(object value)
        {
            if (value is DBNull) Num1С = null;
            else Num1С = Convert.ToString(value);
        }

        public void SetStartDate(object value)
        {
            // Не NULL
            // тип данных SQL: DATE
            StartDate = Convert.ToDateTime(value);
        }

        public void SetPlannedDate(object value)
        {
            // Не NULL
            // тип данных SQL: DATE
            PlannedDate = Convert.ToDateTime(value);
        }

        public void SetAmount(object value)
        {
            // Не NULL
            // тип данных SQL: SMALLINT
            Amount = Convert.ToInt32(value);
        }

        public List<Detail> GetTree(Detail d)
        {
            if (DetailList == null) return null;
            if (DetailList.Any() == false) return null;

            List<Detail> result = new List<Detail>();
            result.Add(d);

            var e = from item in DetailList
                    where item.PositionParent == d.Position
                    select item;

            if (e.Any())
            {
                foreach(var x in e)
                {
                    var sr = GetTree(x);
                    if (sr != null) result.AddRange(sr);
                }
            }

            return result;
        }
    }
}
