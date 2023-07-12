using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Dispetcher2.Class
{
    public class Receipt
    {
        public DataTable ReceiptData;

        public List<ErrorItem> ErrorList = new List<ErrorItem>();

        public string OrderNum1С;

        public int OrderId;

        public string NumLimit;

        public DateTime DateLimit = new DateTime();

        public int PK_IdOrder;

        int errId = 0;

        public Receipt()
        {
            ReceiptData = new DataTable();

            ReceiptData.Columns.Add("Position", typeof(int));
            ReceiptData.Columns.Add("IdLoodsman", typeof(long));
            ReceiptData.Columns.Add("PK_1С_IdKit", typeof(long));
            ReceiptData.Columns.Add("Name1CKit", typeof(string));
            ReceiptData.Columns.Add("AmountKit", typeof(double));
        }

        public void AddError(string message)
        {
            errId = errId + 1;
            ErrorItem ei = new ErrorItem(errId, message);
            ErrorList.Add(ei);
        }
    }
}
