using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.Cache
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public class Redis
    {
        private static string DB_CONNECTIONSTRING { get; set; }
        /// <summary>
        /// 初始化（请在程序启动时执行此代码）
        /// </summary>
        /// <param name="dbFilename"></param>
        /// <param name="dbPassword"></param>
        public static void Initialization(string dbConnectionString)
        {
            if (string.IsNullOrWhiteSpace(dbConnectionString))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(dbConnectionString));
            }

            DB_CONNECTIONSTRING = dbConnectionString;

            using (ConnectionMultiplexer redisClient = ConnectionMultiplexer.Connect(DB_CONNECTIONSTRING))
            {
                redisClient.GetDatabase();
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="id">标识(用于分组)</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool ReadAs(string key, out string value)
        {
            value = string.Empty;

            if (string.IsNullOrWhiteSpace(DB_CONNECTIONSTRING))
            {
                throw new NullReferenceException("未初始化，请在程序启动时执行Initialization方法");
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(key));
            }

            using (ConnectionMultiplexer redisClient = ConnectionMultiplexer.Connect(DB_CONNECTIONSTRING))
            {
                var db = redisClient.GetDatabase();
                var result = db.StringGet(key);
                if (!result.HasValue)
                    return false;
                value = result;
                return true;
            }
        }
        /// <summary>
        /// 存储（key不存在）或修改（key存在）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireSecond">有效期（秒）</param>
        public static void SaveAs(string key, string value, int expireSecond)
        {
            if (string.IsNullOrWhiteSpace(DB_CONNECTIONSTRING))
            {
                throw new NullReferenceException("未初始化，请在程序启动时执行Initialization方法");
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (expireSecond <= 0)
            {
                throw new ArgumentException("<= 0", nameof(expireSecond));
            }
            using (ConnectionMultiplexer redisClient = ConnectionMultiplexer.Connect(DB_CONNECTIONSTRING))
            {
                var db = redisClient.GetDatabase();
                db.StringSet(key, value, new TimeSpan(0, 0, expireSecond));
            }
        }
    }
}
