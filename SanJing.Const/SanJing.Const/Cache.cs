using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SanJing
{
    /// <summary>
    /// 缓存(内存)
    /// </summary>
    public static class Cache
    {
        /// <summary>
        /// 根目录
        /// </summary>
        private static string rootPath { get; set; }
        /// <summary>
        /// 缓存路径
        /// </summary>
        private const string cachePath = "sanjing.cache";
        /// <summary>
        /// 是否启动
        /// </summary>
        private static bool isInitialization { get; set; } = false;
        /// <summary>
        /// 默认ID
        /// </summary>
        private const string defaultId = "SanJing.Cache.DefaultId";
        /// <summary>
        /// 凭据Id
        /// </summary>
        public const string ID_TOKEN = "SanJing.Cache.TokenId";
        /// <summary>
        /// 验证码Id
        /// </summary>
        public const string ID_VERCODE = "SanJing.Cache.VerCodeId";
        /// <summary>
        /// 缓存对象集合
        /// </summary>
        private static List<CacheItem> cacheItems { get; set; }
        /// <summary>
        /// 初始化（请在程序启动时执行此代码）
        /// </summary>
        /// <param name="root">缓存文件夹(已存在的)路径</param>
        public static void Initialization(string root)
        {
            if (isInitialization) throw new MethodAccessException("不能重复调用此方法！");

            isInitialization = true;
            if (string.IsNullOrWhiteSpace(root))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(root));
            }

            if (Directory.Exists(root)) rootPath = root.Trim('\\') + "\\";
            else throw new DirectoryNotFoundException(root);

            cacheItems = new List<CacheItem>();
            if (File.Exists(rootPath + cachePath))
            {
                cacheItems = JsonConvert.DeserializeObject<List<CacheItem>>(File.ReadAllText(rootPath + cachePath, Encoding.UTF8));
            }
            else
            {
                File.WriteAllText(rootPath + cachePath, JsonConvert.SerializeObject(cacheItems), Encoding.UTF8);
            }
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="id">标识(用于分组)</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool ReadAs(string key, string id, out string value)
        {
            value = string.Empty;

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(id));
            }

            var cache = cacheItems.Where(q => q.Id == id).SingleOrDefault(q => q.Key == key);

            if (cache == null) return false;

            if (cache.Expire < DateTime.Now.ToUnixTimestamp()) return false;

            value = cache.Value;

            return true;
        }

        /// <summary>
        /// 读取(默认ID)
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">标识(用于分组)</param>
        /// <returns></returns>
        public static bool ReadAsDefault(string key, out string value)
        {
            return ReadAs(key, defaultId, out value);
        }

        /// <summary>
        /// 存储（key不存在）或修改（key存在）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireMiunte">有效期（分钟）</param>
        /// <param name="id">标识(用于分组)</param>
        public static void SaveAs(string key, string value, int expireMiunte, string id)
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
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(id));
            }

            lock (cacheItems)
            {
                var cache = cacheItems.Where(q => q.Id == id).SingleOrDefault(q => q.Key == key);
                if (cache == null)
                {
                    cacheItems.Add(new CacheItem()
                    {
                        Id = id,
                        Expire = DateTime.Now.AddMinutes(expireMiunte).ToUnixTimestamp(),
                        Key = key,
                        Value = value,
                    });
                }
                else
                {
                    cache.Value = value;
                    cache.Expire = DateTime.Now.AddMinutes(expireMiunte).ToUnixTimestamp();
                }

                cacheItems.RemoveAll(q => q.Expire < DateTime.Now.ToUnixTimestamp());
                File.WriteAllText(rootPath + cachePath, JsonConvert.SerializeObject(cacheItems), Encoding.UTF8);
            }
        }
        /// <summary>
        /// 存储（默认ID）（key不存在）或修改（key存在）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireMiunte">有效期（分钟）</param>
        public static void SaveAsDefault(string key, string value, int expireMiunte)
        {
            SaveAs(key, value, expireMiunte, defaultId);
        }
    }
    /// <summary>
    /// 缓存对象
    /// </summary>
    public class CacheItem
    {
        /// <summary>
        /// 标识（用于分组）
        /// </summary>
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; } = string.Empty;
        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; } = string.Empty;
        /// <summary>
        /// 过期时间戳
        /// </summary>
        public long Expire { get; set; } = 0;
    }
}
