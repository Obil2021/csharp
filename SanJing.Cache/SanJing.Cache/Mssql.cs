using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SanJing.Cache
{
    public class Mssql
    {
        private const string DB_TABLENAME = "SANJINGCACHE";
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
            //连接数据库
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
            {
                conn.ConnectionString = DB_CONNECTIONSTRING;
                conn.Open();
                //创建表
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $@"IF NOT EXISTS (SELECT * FROM [sysobjects] where id = object_id('{DB_TABLENAME}') 
and OBJECTPROPERTY(id, 'IsUserTable') = 1) CREATE TABLE {DB_TABLENAME}(Id VARCHAR(100) NOT NULL,[Key] VARCHAR(500) NOT NULL,Value NVARCHAR(4000),Expire DECIMAL(18,0))";
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
            if (string.IsNullOrWhiteSpace(DB_CONNECTIONSTRING))
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
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
            {
                conn.ConnectionString = DB_CONNECTIONSTRING;
                conn.Open();
                //创建表
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.Connection = conn;

                    //查询旧数据
                    cmd.CommandText = $"SELECT Value FROM {DB_TABLENAME} WHERE Id = @Id AND [Key] = @Key AND Expire >= @Expire";
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Id", id ?? string.Empty));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Key", key ?? string.Empty));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Expire", Convert.ToDecimal(DateTime.Now.Ticks)));
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
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(key));
            }
            if (string.IsNullOrWhiteSpace(DB_CONNECTIONSTRING))
            {
                throw new NullReferenceException("未初始化，请在程序启动时执行Initialization方法");
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
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
            {
                conn.ConnectionString = DB_CONNECTIONSTRING;
                conn.Open();
                //创建表
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.Connection = conn;

                    //查询旧数据
                    cmd.CommandText = $"SELECT Id FROM {DB_TABLENAME} WHERE Id = @Id AND [Key] = @Key";
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Id", id ?? string.Empty));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Key", key ?? string.Empty));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Value", value ?? string.Empty));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Expire", Convert.ToDecimal(DateTime.Now.AddSeconds(expireSecond).Ticks)));
                    var obj = cmd.ExecuteScalar();
                    if (obj == null || obj == DBNull.Value)
                    {
                        //存储新数据
                        cmd.CommandText = $"INSERT INTO {DB_TABLENAME} (Id,[Key],Value,Expire) VALUES (@Id,@Key,@Value,@Expire)"; ;
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //修改旧数据
                        cmd.CommandText = $"UPDATE {DB_TABLENAME} SET Value = @Value,Expire = @Expire WHERE Id = @Id AND [Key] = @Key";
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
            if (string.IsNullOrWhiteSpace(DB_CONNECTIONSTRING))
            {
                throw new NullReferenceException("未初始化，请在程序启动时执行Initialization方法");
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(id));
            }
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            //连接数据库
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection())
            {
                conn.ConnectionString = DB_CONNECTIONSTRING;
                conn.Open();
                //创建表
                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.Connection = conn;
                    //清除数据
                    cmd.CommandText = $"DELETE FROM {DB_TABLENAME} WHERE Id = @Id AND Value = @Value";
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Id", id ?? string.Empty));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Value", value ?? string.Empty));
                    cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// 生成唯一序列号
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1000000000+</returns>
        public static decimal SerialNum(string key)
        {
            decimal result = 1000000000;
            if (ReadAs(key, "SanJing.SerialNum", out string num))
            {
                result = Convert.ToDecimal(num) + 1;
            }
            SaveAs(key, result.ToString(), int.MaxValue, "SanJing.SerialNum");
            return result;
        }
    }
}
