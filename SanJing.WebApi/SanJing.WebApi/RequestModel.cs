using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// WebApi请求模型
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// 随机字符串
        /// </summary>
        [DisplayName("随机字符串")]
        public virtual string NonceString { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        [DisplayName("时间戳")]
        public virtual long TimeStamp { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [DisplayName("签名")]
        public virtual string Sign { get; set; }
        /// <summary>
        /// 验证签名
        /// MD5("Key={key}&NonceString={NonceString}&TimeStamp={TimeStamp}") == Sign
        /// </summary>
        public virtual bool CheckSign(string key)
        {
            string temp = $"Key={key}&NonceString={NonceString}&TimeStamp={TimeStamp}";
            return Hash.Encrypt.MD5(temp) == Sign;
        }
        /// <summary>
        /// 验证随机字符串（重复问题）
        /// </summary>
        /// <param name="urlPath">API接口</param>
        /// <param name="ipAddress">IP地址</param>
        /// <returns></returns>
        public virtual bool CheckNonceString(string urlPath, string ipAddress)
        {
            string hash = Hash.Encrypt.MD5($"UrlPath={urlPath}&IPAddress={ipAddress}&NonceString={NonceString}");
            return NonceStringCache.CheckAndAdd(hash, TimeStamp, TimeStampLimit());
        }
        /// <summary>
        /// IP地址黑名单
        /// </summary>
        /// <returns></returns>
        public virtual string IPAddressBlackList() { return string.Empty; }
        /// <summary>
        /// 时间戳限制范围（秒）
        /// </summary>
        /// <returns></returns>
        public virtual int TimeStampLimit() { return 60; }
    }
}
