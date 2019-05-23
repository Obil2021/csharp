using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// 服务器缓存
    /// </summary>
    public class NonceStringCache
    {
        /// <summary>
        /// 缓存表
        /// </summary>
        private static readonly Dictionary<string, long> keyValuePairs = new Dictionary<string, long>();
        /// <summary>
        /// 验证并添加
        /// </summary>
        /// <param name="key">KEY</param>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="second">缓存时间</param>
        /// <returns></returns>
        public static bool CheckAndAdd(string key, long timeStamp, int second)
        {
            NonceStringCacheClear(timeStamp, second);

            if (keyValuePairs.ContainsKey(key))
                return false;
            keyValuePairs.Add(key, timeStamp);

            return true;
        }
        /// <summary>
        /// 清理过期的缓存
        /// </summary>
        private static void NonceStringCacheClear(long timeStamp, int second)
        {
            var temp = keyValuePairs.Where(q => q.Value + second < timeStamp).ToList();
            foreach (var item in temp)
            {
                keyValuePairs.Remove(item.Key);
            }
        }
    }
}
