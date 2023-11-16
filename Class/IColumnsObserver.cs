using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dispetcher2.Class
{
    public interface IColumnsObserver
    {
        void Update(IEnumerable<string> columns);
    }
}
