using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public abstract partial class BaseService<T>
        where T : class, new()
    {
        public BaseService()
        {
            SetDal();
        }
        public abstract void SetDal();
        public IBaseDAL<T> Dal { get; set; }
        public int Add(IEnumerable<T> list)
        {
            long i = Dal.Add(list);
            return (int)i;
        }
        public T Get(int id)
        {
            return Dal.Get(id);
        }
    }
}
