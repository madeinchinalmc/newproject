using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public partial interface IBaseDAL<T> where T: class, new()
    {
        int Add(IEnumerable<T> list);

        T Get (int id);
    }
}
