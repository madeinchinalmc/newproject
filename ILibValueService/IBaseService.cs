using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    public partial interface IBaseService<T> where T : class, new()
    {
        int Add(IEnumerable<T> list);
        T Get(int id);
    }
}
