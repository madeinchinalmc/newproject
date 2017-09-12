using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Model
{
    public class Database
    {
        //private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //private static readonly string ProviderFactoryString = ConfigurationManager.AppSettings["DBProvider"].ToString();
        private static readonly string ConnectionString = "System.Data.SqlClient";
        private static readonly string ProviderFactoryString = "Data Source=LMC;Initial Catalog=SpiderDb;User ID=sa;password=nokia123;Integrated Security=false";
        private static DbProviderFactory df = null;
        /// <summary>  
        /// 创建工厂提供器
        /// </summary>  
        public static IDbConnection DbService()
        {
            if (df == null)
                df = DbProviderFactories.GetFactory(ConnectionString);
            var connection = df.CreateConnection();
            connection.ConnectionString = ProviderFactoryString;
            return connection;
        }
    }
}
