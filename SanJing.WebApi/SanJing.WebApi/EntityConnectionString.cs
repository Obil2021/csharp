using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// EF连接字符串
    /// </summary>
    public sealed class EntityConnectionString
    {
        /// <summary>
        ///获取EF连接字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ConnectionString<T>() where T : DbContext, new()
        {
            return new T().Database.Connection.ConnectionString;
        }
    }
}
