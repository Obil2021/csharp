using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// 配置
    /// </summary>
    public sealed class WebApiConfig
    {
        /// <summary>
        /// Redis服务器地址（默认：127.0.0.1:6379）
        /// </summary>
        public static string RedisConfiguration { get; set; } = "127.0.0.1:6379";
        /// <summary>
        /// IP地址黑名单（默认：空）
        /// </summary>
        public static string[] IPAddressBlackList { get; set; } = new string[0];
        /// <summary>
        /// Token有效期（天）(默认：7)
        /// </summary>
        public static int TokenDay { get; set; } = 7;
        /// <summary>
        /// 时间戳误差（分钟）（默认：5）
        /// </summary>
        public static int TimeStampMinute { get; set; } = 5;
        /// <summary>
        /// 随机串生命周期（分钟）（默认：1）
        /// </summary>
        public static int NonceStringMinute { get; set; } = 1;
        /// <summary>
        /// 短信验证码有效期（分钟）（默认：5）
        /// </summary>
        public static int SMSDay { get; set; } = 5;
        /// <summary>
        /// 短信验证码长度（默认：6）
        /// </summary>
        public static int SMSLength { get; set; } = 6;
        /// <summary>
        /// 默认APPKEY【447AD97D40C592FBDA322EC14794132A】
        /// </summary>
        public static string AppKey { get; set; } = "447AD97D40C592FBDA322EC14794132A";
    }
}
