using Dapper;
using Dapper.Contrib.Extensions;
using Model;
using Model.Dto;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public partial class BaseDAL<T> : IDisposable
        where T : class, new()
    {
        private IDbConnection dbContext = Database.DbService();
        public int Add(IEnumerable<T> list)
        {
            long i = 0;
            dbContext.Open();
            IDbTransaction tran = dbContext.BeginTransaction(); 
            try
            {
                i = dbContext.Insert<IEnumerable<T>>(list, tran);
                tran.Commit();
                return (int)i;
            }
            catch(Exception ex)
            {
                tran.Rollback();
            }
            finally
            {
                Dispose();
            }
            return (int)i;
        }

        public async Task<int> AddAsAsync(IEnumerable<T> list)
        {
            int i = await dbContext.InsertAsync<IEnumerable<T>>(list);
            return i;
        }


        public T Get(int id)
        {
            return dbContext.Get<T>(id);
        }

        public void Dispose()
        {
            dbContext.Close();
        }
    }
}
