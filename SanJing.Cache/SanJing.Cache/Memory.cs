using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace SanJing.Cache
{
    /// <summary>
    /// 内存缓存|程序退出时释放
    /// </summary>
    public class Memory
    {
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool ReadAs(string key, out string value)
        {
            value = string.Empty;

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(key));
            }

            var result = MemoryCache.Default.Get(key);
            if (result == null)
                return false;
            value = result.ToString();
            return true;
        }
        /// <summary>
        /// 存储（key不存在）或修改（key存在）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireSecond">有效期（秒）</param>
        /// <param name="id">标识(用于分组)</param>
        public static void SaveAs(string key, string value, int expireMiunte)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(key));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (expireMiunte <= 0)
            {
                throw new ArgumentException("<= 0", nameof(expireMiunte));
            }
            MemoryCache.Default.Set(key, value, DateTime.Now.AddSeconds(expireMiunte), null);
        }
    }
}
