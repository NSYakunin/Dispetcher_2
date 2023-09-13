using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dispetcher2.Class;

namespace Dispetcher2.DataAccess
{
    public class SqlDetail : Detail
    {

    }
    public class SqlDetailRepository : DetailRepository
    {
        // заглушка
        // требуется сделать реализацию и убрать этот комментарий
        List<SqlDetail> details = new List<SqlDetail>();
        public override IEnumerable<Detail> GetDetails()
        {
            return details;
        }
    }
}
