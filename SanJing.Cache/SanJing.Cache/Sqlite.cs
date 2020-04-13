using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SanJing.Cache
{
    /// <summary>
    /// SQLite缓存
    /// </summary>
    public class Sqlite
    {
        private const string DB_TABLENAME = "SANJINGCACHE";
        private static string DB_FILENAME { get; set; }
        private static string DB_PASSWORD { get; set; }
        /// <summary>
        /// 初始化（请在程序启动时执行此代码）
        /// </summary>
        /// <param name="dbFilename"></param>
        /// <param name="dbPassword"></param>
        public static void Initialization(string dbFilename, string dbPassword)
        {
            if (string.IsNullOrWhiteSpace(dbFilename))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(dbFilename));
            }
            if (string.IsNullOrWhiteSpace(dbPassword))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(dbPassword));
            }

            DB_FILENAME = dbFilename;
            DB_PASSWORD = dbPassword;

            if (!System.IO.File.Exists(dbFilename))
            {
                var dir = Path.GetDirectoryName(DB_FILENAME);
                Directory.CreateDirectory(dir);
                System.Data.SQLite.SQLiteConnection.CreateFile(DB_FILENAME);
            }
            //连接数据库
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = DB_FILENAME;
                connstr.Password = DB_PASSWORD;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //创建表
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS {DB_TABLENAME}(Id VARCHAR(100) NOT NULL,Key VARCHAR(500) NOT NULL,Value NVARCHAR(4000),Expire DECIMAL(18,0))";
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
            if (string.IsNullOrWhiteSpace(DB_FILENAME))
            {
                throw new NullReferenceException("未初始化，请在程序启动时执行Initialization方法");
            }
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
                connstr.DataSource = DB_FILENAME;
                connstr.Password = DB_PASSWORD;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //执行语句
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.Connection = conn;

                    //查询旧数据
                    cmd.CommandText = $"SELECT Value FROM {DB_TABLENAME} WHERE Id = @Id AND Key = @Key AND Expire >= @Expire";
                    cmd.Parameters.Add("@Id", DbType.String).Value = id ?? string.Empty;
                    cmd.Parameters.Add("@Key", DbType.String).Value = key ?? string.Empty;
                    cmd.Parameters.Add("@Expire", DbType.Decimal).Value = DateTime.Now.Ticks;
                    var obj = cmd.ExecuteScalar();
                    if (obj == null || obj == DBNull.Value) return false;
                    value = obj.ToString();
                    return true;
                }
            }
        }
        /// <summary>
        /// 存储（key不存在）或修改（key存在）
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireSecond">有效期（秒）</param>
        /// <param name="id">标识(用于分组)</param>
        public static void SaveAs(string key, string value, int expireSecond, string id)
        {
            if (string.IsNullOrWhiteSpace(DB_FILENAME))
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
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(id));
            }

            //连接数据库
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = DB_FILENAME;
                connstr.Password = DB_PASSWORD;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //创建表
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.Connection = conn;

                    //查询旧数据
                    cmd.CommandText = $"SELECT Id FROM {DB_TABLENAME} WHERE Id = @Id AND Key = @Key";
                    cmd.Parameters.Add("@Id", DbType.String).Value = id ?? string.Empty;
                    cmd.Parameters.Add("@Key", DbType.String).Value = key ?? string.Empty;
                    cmd.Parameters.Add("@Value", DbType.String).Value = value ?? string.Empty;
                    cmd.Parameters.Add("@Expire", DbType.Decimal).Value = DateTime.Now.AddSeconds(expireSecond).Ticks;
                    var obj = cmd.ExecuteScalar();
                    if (obj == null || obj == DBNull.Value)
                    {
                        //存储新数据
                        cmd.CommandText = $"INSERT INTO {DB_TABLENAME} (Id,Key,Value,Expire) VALUES (@Id,@Key,@Value,@Expire)"; ;
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //修改旧数据
                        cmd.CommandText = $"UPDATE {DB_TABLENAME} SET Value = @Value,Expire = @Expire WHERE Id = @Id AND Key = @Key";
                        cmd.ExecuteNonQuery();
                    }
                    //清除过期数据
                    cmd.CommandText = $"DELETE FROM {DB_TABLENAME} WHERE Expire < '{DateTime.Now.Ticks}'";
                    cmd.ExecuteNonQuery();
                }
            }
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
                connstr.DataSource = DB_FILENAME;
                connstr.Password = DB_PASSWORD;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //创建表
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.Connection = conn;
                    //清除数据
                    cmd.CommandText = $"DELETE FROM {DB_TABLENAME} WHERE Id = @Id AND Value = @Value";
                    cmd.Parameters.Add("@Id", DbType.String).Value = id ?? string.Empty;
                    cmd.Parameters.Add("@Value", DbType.String).Value = value ?? string.Empty;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
