using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
        /// 缓存路径
        /// </summary>
        private const string cachePath = "sanjing.cache.db";
        /// <summary>
        /// 数据库密码
        /// </summary>
        private const string cachePassword = "CACHE.PASSWORD";
        /// <summary>
        /// 缓存表名
        /// </summary>
        private const string cacheTable = "SANJINGCACHE";
        /// <summary>
        /// 数据库文件地址
        /// </summary>
        private static string datasource { get; set; }
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
        /// 文件Id
        /// </summary>
        public const string ID_FILE = "SanJing.Cache.FileId";
        /// <summary>
        /// 验证码Id
        /// </summary>
        public const string ID_VERCODE = "SanJing.Cache.VerCodeId";
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
            var rootPath = root.Trim('\\') + "\\";
            Directory.CreateDirectory(rootPath);
            datasource = rootPath + cachePath;
            if (!File.Exists(datasource))
                System.Data.SQLite.SQLiteConnection.CreateFile(datasource);
            //连接数据库
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = datasource;
                connstr.Password = cachePassword;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //创建表
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS {cacheTable}(Id VARCHAR(100) NOT NULL,Key VARCHAR(500) NOT NULL,Value NVARCHAR(4000),Expire DECIMAL(18,0))";
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
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

            //连接数据库
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = datasource;
                connstr.Password = cachePassword;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //执行语句
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.Connection = conn;

                    //查询旧数据
                    cmd.CommandText = $"SELECT Value FROM {cacheTable} WHERE Id = @Id AND Key = @Key AND Expire >= @Expire";
                    cmd.Parameters.Add("@Id", DbType.String).Value = id ?? string.Empty;
                    cmd.Parameters.Add("@Key", DbType.String).Value = key ?? string.Empty;
                    cmd.Parameters.Add("@Expire", DbType.Decimal).Value = DateTime.Now.ToUnixTimestamp();
                    var obj = cmd.ExecuteScalar();
                    if (obj == null || obj == DBNull.Value) return false;
                    value = obj.ToString();
                    return true;
                }
            }
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

            //连接数据库
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = datasource;
                connstr.Password = cachePassword;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //创建表
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.Connection = conn;

                    //查询旧数据
                    cmd.CommandText = $"SELECT Id FROM {cacheTable} WHERE Id = @Id AND Key = @Key";
                    cmd.Parameters.Add("@Id", DbType.String).Value = id ?? string.Empty;
                    cmd.Parameters.Add("@Key", DbType.String).Value = key ?? string.Empty;
                    cmd.Parameters.Add("@Value", DbType.String).Value = value ?? string.Empty;
                    cmd.Parameters.Add("@Expire", DbType.Decimal).Value = DateTime.Now.AddMinutes(expireMiunte).ToUnixTimestamp();
                    var obj = cmd.ExecuteScalar();
                    if (obj == null || obj == DBNull.Value)
                    {
                        //存储新数据
                        cmd.CommandText = $"INSERT INTO {cacheTable} (Id,Key,Value,Expire) VALUES (@Id,@Key,@Value,@Expire)"; ;
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //修改旧数据
                        cmd.CommandText = $"UPDATE {cacheTable} SET Value = @Value,Expire = @Expire WHERE Id = @Id AND Key = @Key";
                        cmd.ExecuteNonQuery();
                    }
                    //清除过期数据
                    cmd.CommandText = $"DELETE FROM {cacheTable} WHERE Expire < '{DateTime.Now.ToUnixTimestamp()}'";
                    cmd.ExecuteNonQuery();
                }
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
        /// <summary>
        /// 根据值清理缓存
        /// </summary>
        /// <param name="id">标识(用于分组)</param>
        /// <param name="value">值</param>
        public static void Clear(string id, string value)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(id));
            }
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            //连接数据库
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = datasource;
                connstr.Password = cachePassword;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //创建表
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.Connection = conn;
                    //清除数据
                    cmd.CommandText = $"DELETE FROM {cacheTable} WHERE Id = @Id AND Value = @Value";
                    cmd.Parameters.Add("@Id", DbType.String).Value = id ?? string.Empty;
                    cmd.Parameters.Add("@Value", DbType.String).Value = value ?? string.Empty;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 根据值清理缓存(默认ID)
        /// </summary>
        /// <param name="value">值</param>
        public static void ClearDefault(string value)
        {
            Clear(defaultId, value);
        }
    }
}
