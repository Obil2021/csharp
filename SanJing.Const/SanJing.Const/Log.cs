using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SanJing
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 缓存路径
        /// </summary>
        private const string logPath = "sanjing.log.db";
        /// <summary>
        /// 数据库密码
        /// </summary>
        private const string logPassword = "LOG.PASSWORD";
        /// <summary>
        /// 缓存表名
        /// </summary>
        private const string logTable = "SANJINGLOG";
        /// <summary>
        /// 数据库文件地址
        /// </summary>
        private static string datasource { get; set; }
        /// <summary>
        /// 是否启动
        /// </summary>
        private static bool isInitialization { get; set; } = false;
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
            datasource = rootPath + logPath;
            if (!File.Exists(datasource))
                System.Data.SQLite.SQLiteConnection.CreateFile(datasource);
            //连接数据库
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = datasource;
                connstr.Password = logPassword;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //创建表
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.CommandText = $"CREATE TABLE IF NOT EXISTS {logTable}(Timestamp DECIMAL(18,0),UserId DECIMAL(18,0),IPAddress NVARCHAR(100),Description NVARCHAR(4000),Content NVARCHAR(4000),AbsolutePath NVARCHAR(500),Role DECIMAL(18,0))";
                    cmd.Connection = conn;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="log"></param>
        public static void SaveAs(LogItem log)
        {
            //连接数据库
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = datasource;
                connstr.Password = logPassword;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //执行语句
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.Connection = conn;
                    //执行语句
                    cmd.CommandText = $"INSERT INTO {logTable} (UserId,Role,Timestamp,AbsolutePath,IPAddress,Description,Content) VALUES (@UserId,@Role,@Timestamp,@AbsolutePath,@IPAddress,@Description,@Content)";
                    cmd.Parameters.Add("@UserId", DbType.Decimal).Value = log.UserId;
                    cmd.Parameters.Add("@Role", DbType.Decimal).Value = log.Role;
                    cmd.Parameters.Add("@Timestamp", DbType.Decimal).Value = log.Timestamp;
                    cmd.Parameters.Add("@AbsolutePath", DbType.String).Value = log.AbsolutePath??string.Empty;
                    cmd.Parameters.Add("@IPAddress", DbType.String).Value = log.IPAddress ?? string.Empty;
                    cmd.Parameters.Add("@Description", DbType.String).Value = log.Description ?? string.Empty;
                    cmd.Parameters.Add("@Content", DbType.String).Value = log.Content ?? string.Empty;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 读取所有日志记录
        /// </summary>
        /// <returns></returns>
        public static IList<LogItem> ReadAs()
        {
            var result = new List<LogItem>();
            //连接数据库
            using (System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection())
            {
                System.Data.SQLite.SQLiteConnectionStringBuilder connstr = new System.Data.SQLite.SQLiteConnectionStringBuilder();
                connstr.DataSource = datasource;
                connstr.Password = logPassword;//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //执行语句
                using (System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand())
                {
                    cmd.Connection = conn;
                    //查询旧数据
                    cmd.CommandText = $"SELECT UserId,Role,Timestamp,AbsolutePath,IPAddress,Description,Content FROM {logTable} ORDER BY Timestamp DESC";
                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        var item = new LogItem();
                        item.UserId = rd.GetDecimal(0);
                        item.Role = rd.GetDecimal(1);
                        item.Timestamp = rd.GetDecimal(2);
                        item.AbsolutePath = rd.GetString(3);
                        item.IPAddress = rd.GetString(4);
                        item.Description = rd.GetString(5);
                        item.Content = rd.GetString(6);
                        result.Add(item);
                    }
                }
            }
            return result;
        }
    }
    /// <summary>
    /// 日志
    /// </summary>
    public class LogItem
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public decimal UserId { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        public decimal Role { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public decimal Timestamp { get; set; } = DateTime.Now.ToUnixTimestamp();
        /// <summary>
        /// API地址
        /// </summary>
        public string AbsolutePath { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
