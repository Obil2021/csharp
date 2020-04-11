using System;
namespace SanJing
{
    /// <summary>
    /// 常量
    /// </summary>
    public class Const
    {
        /// <summary>
        /// 日期格式
        /// </summary>
        public const string DATE_YYYYMMDD = "yyyy-MM-dd";
        /// <summary>
        /// /// <summary>
        /// 日期格式
        /// </summary>
        /// </summary>
        public const string DATE_YYYYMMDDHHMM = "yyyy-MM-dd HH:mm";
        /// <summary>
        /// 日期格式
        /// </summary>
        public const string DATE_YYYYMMDDHHMMSS = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// 数字或账号组成
        /// </summary>
        public const string REGULAR_NUM_ABC = "^[A-Za-z0-9]+$";
        /// <summary>
        /// 数字组成
        /// </summary>
        public const string REGULAR_NUM = "^[0-9]*$";
        /// <summary>
        /// 邮箱
        /// </summary>
        public const string REGULAR_EMAIL = @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        /// <summary>
        /// 身份证
        /// </summary>
        public const string REGULAR_IDCARD = @"^\d{15}|\d{18}$";
        /// <summary>
        /// 日期
        /// </summary>
        public const string REGULAR_DATE = @"^\d{4}-\d{1,2}-\d{1,2}";
        /// <summary>
        /// 随机种子
        /// </summary>
        public static Random RANDOM_SEED = new Random();
        /// <summary>
        /// 唯一序列ID的KEY
        /// </summary>
        private const string SERIALNUM_KEY = "FE198AABA4A94C92B71CC1C8C9653941";
        /// <summary>
        /// 唯一序列ID
        /// </summary>
        public static decimal SerialNum
        {
            get
            {
                decimal value = 0m;
                if (Cache.ReadAsDefault(SERIALNUM_KEY, out string num))
                    value = Convert.ToDecimal(num) + 1;
                else
                    value = DateTime.Now.ToUnixTimestamp();

                Cache.SaveAsDefault(SERIALNUM_KEY, value.ToString(), 60 * 24 * 365 * 20);//20年
                return value;
            }
        }
    }
}

namespace System
{
    /// <summary>
    /// DateTime扩展
    /// </summary>
    public static class SanJingDateTime
    {
        /// <summary>
        /// 将当前 DateTime 对象的值转换为Unix时间戳
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime owner)
        {
            return (owner.Ticks - 621356256000000000) / 10000;
        }
        /// <summary>
        /// 将Unix时间戳转换为 DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime Parse(long timestamp)
        {
            return new DateTime(timestamp * 10000 + 621356256000000000);
        }
        /// <summary>
        /// 将Unix时间戳转换为 DateTime
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime Parse(string timestamp)
        {
            return Parse(long.Parse(timestamp));
        }
    }
}
