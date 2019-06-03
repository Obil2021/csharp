using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;

namespace SanJing.WebApi
{
    /// <summary>
    /// WebApi扩展
    /// </summary>
    public static class ApiControllerExpansion
    {
        private static readonly Random Random = new Random();
        private static long TimeStamp { get { return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000; } }
        /// <summary>
        /// 请求示例名称
        /// </summary>
        private const string MS_HTTPCONTEXT = "MS_HttpContext";
        /// <summary>
        /// 请求中服务器IP地址名称
        /// </summary>
        private const string LOCAL_ADDR = "Local_Addr";
        /// <summary>
        /// 默认IP地址
        /// </summary>
        private const string IP_ADDRESS = "0.0.0.0";
        /// <summary>
        /// 请求准备(含数IP地址验证、数据验证、签名验证、时间戳验证，随机串验证(Redis)、日志记录(NLog.Trace))
        /// </summary>
        /// <param name="webApiController"></param>
        /// <param name="request">请求数据</param>
        /// <param name="key">验签Key</param>
        /// <param name="appName">APPName</param>
        internal static void RequestReady(this WebApiController webApiController, RequestModel request, string key, string appName = "General")
        {

            HttpContextWrapper httpContext = webApiController.Request.Properties[MS_HTTPCONTEXT] as HttpContextWrapper;
            string ipAddress = IP_ADDRESS;
            if (httpContext != null)
            {
                ipAddress = httpContext.Request.UserHostAddress;
            }
            //请求记录
            NLog.LogManager.GetCurrentClassLogger().Trace(JsonConvert.SerializeObject(new { IPAddress = ipAddress, Request = request }));

            //APP验证（路由验证）
            if (!webApiController.Request.RequestUri.AbsolutePath.ToLower().StartsWith("/api/" + appName.ToLower()))
            {
                throw new UrlException();
            }

            //IP地址验证
            if (WebApiConfig.IPAddressBlackList.Contains(ipAddress))
                throw new IPAddressException();

            //数据验证
            if (!webApiController.ModelState.IsValid)
            {
                throw new ValidException(string.Join(Environment.NewLine,
                    webApiController.ModelState.Select(q => q.Value.Errors.First().ErrorMessage)));
            }

            //签名验证
            string sign = webApiController.GetSignString(key, request.TimeStamp, request.NonceString);
            if (sign != request.Sign)
            {
                throw new SignException();
            }

            //时间戳验证
            long max = request.TimeStamp + WebApiConfig.TimeStampMinute * 60;
            long min = request.TimeStamp - WebApiConfig.TimeStampMinute * 60;
            if (TimeStamp > max || TimeStamp < min)
            {
                throw new TimeStampException();
            }
            //随机串验证
            string hash = webApiController.NonceStingHashEncrypt(ipAddress, request.NonceString);
            if (webApiController.GetRedisString(hash) == request.NonceString)
            {
                throw new NonceStringException();
            }
            webApiController.SetRedisString(hash, request.NonceString, new TimeSpan(0, WebApiConfig.NonceStringMinute, 0));
        }
        /// <summary>
        /// 根据随机串生成唯一Key
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="ipAddress"></param>
        /// <param name="nonceString"></param>
        /// <returns></returns>
        private static string NonceStingHashEncrypt(this ApiController apiController, string ipAddress, string nonceString)
        {
            return Hash.Encrypt.MD5($"UrlPath={apiController.Request.RequestUri.AbsolutePath}&IPAddress={ipAddress}&NonceString={nonceString}");
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="apiController"></param>
        /// <returns></returns>
        public static long GetTimeStamp(this ApiController apiController) => TimeStamp;
        /// <summary>
        /// 获取随机种子
        /// </summary>
        /// <param name="apiController"></param>
        /// <returns></returns>
        public static Random GetRandom(this ApiController apiController) => Random;
        /// <summary>
        /// 获取随机数字
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="length">长度</param>
        /// <returns>随机数字</returns>
        public static string GetRandomNumber(this ApiController apiController, int length)
        {
            length = length < 0 ? 0 : length;
            var max = Math.Pow(10, length);
            var min = Math.Pow(10, length - 1);
            return Random.Next(Convert.ToInt32(min), Convert.ToInt32(max)).ToString();
        }
        /// <summary>
        /// 获取服务器实例
        /// </summary>
        /// <param name="apiController"></param>
        /// <returns></returns>
        public static HttpServerUtility GetServer(this ApiController apiController) => HttpContext.Current.Server;
        /// <summary>
        /// 获取服务器IP地址
        /// </summary>
        /// <param name="apiController"></param>
        /// <returns></returns>
        public static string GetServerIPAddress(this ApiController apiController)
        {
            return HttpContext.Current.Request.ServerVariables.Get(LOCAL_ADDR);
        }
        /// <summary>
        /// 获取服务器完整域名（ssss://xxx.xxx.xxx:pp）
        /// </summary> 
        /// <param name="apiController"></param>
        /// <returns></returns>
        public static string GetServerHostUrl(this ApiController apiController)
        {
            return apiController.Request.RequestUri.GetLeftPart(UriPartial.Authority);
        }
        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="key">密钥</param>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="nonceString">随机字符串</param>
        /// <returns>签名字符串</returns>
        public static string GetSignString(this ApiController apiController, string key, long timeStamp, string nonceString)
        {
            return Hash.Encrypt.MD5($"Key={key}&NonceString={nonceString}&TimeStamp={timeStamp}");
        }
        /// <summary>
        /// 随机生成短信验证码并缓存至Redis中
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="phone">手机号</param>
        /// <returns>短信验证码</returns>
        public static string SetSMSRedisString(this ApiController apiController, string phone)
        {
            string code = apiController.GetRandomNumber(WebApiConfig.SMSLength);
            apiController.SetRedisString(phone, code, new TimeSpan(0, WebApiConfig.SMSDay, 0));
            return code;
        }
        /// <summary>
        /// 从Redus中读取并验证验证码(验证失败则抛出异常)
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public static void GetSMSRedisString(this ApiController apiController, string phone, string code)
        {
            string rcode = apiController.GetRedisString(phone);
            if (string.IsNullOrWhiteSpace(code) || rcode != code)
                throw new UserException("短信验证码不正确");
        }
        /// <summary>
        /// 根据用户标识生成Token并缓存至Redis中
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="userId">用户标识</param>
        /// <param name="userRole">用户角色</param>
        /// <returns>Token</returns>
        public static string SetUserRedisString(this ApiController apiController, int userId, int userRole)
        {
            string token = Guid.NewGuid().ToString("N");
            apiController.SetRedisString(token, apiController.TokenHashEncrypt(userId, userRole), new TimeSpan(WebApiConfig.TokenDay, 0, 0, 0));
            return token;
        }
        /// <summary>
        /// 从Redus中读取Token并解析出用户标识
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="token">Tojen</param>
        /// <param name="userRole">用户角色</param>
        /// <returns>用户标识</returns>
        public static int GetUserRedisString(this ApiController apiController, string token, int userRole)
        {
            string hashtoken = apiController.GetRedisString(token);
            if (string.IsNullOrWhiteSpace(hashtoken))
                throw new TokenException();
            int userid = apiController.TokenHashDecrypt(hashtoken, userRole);
            //刷新有效期
            apiController.SetRedisString(token, apiController.TokenHashEncrypt(userid, userRole), new TimeSpan(WebApiConfig.TokenDay, 0, 0, 0));

            return userid;
        }
        /// <summary>
        /// Redis写入字符串（GetDatabase(0)）
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="key">键（重复时覆盖）</param>
        /// <param name="value">值</param>
        /// <param name="timeSpan">有效期，为null时永久有效</param>
        public static void SetRedisString(this ApiController apiController, string key, string value, TimeSpan timeSpan)
        {
            using (ConnectionMultiplexer _conn = ConnectionMultiplexer.Connect(WebApiConfig.RedisConfiguration))
            {
                var database = _conn.GetDatabase(0);//指定连接的库 0
                if (timeSpan == null)
                    database.StringSet(key, value);
                else
                    database.StringSet(key, value, timeSpan);
            }
        }
        /// <summary>
        /// Redis缓存字符串（GetDatabase(0)）
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="key">键（重复时覆盖）</param>
        /// <returns>值，不存在时返回null</returns>
        public static string GetRedisString(this ApiController apiController, string key)
        {
            using (ConnectionMultiplexer _conn = ConnectionMultiplexer.Connect(WebApiConfig.RedisConfiguration))
            {
                var database = _conn.GetDatabase(0);//指定连接的库 0
                return database.StringGet(key);
            }
        }
        /// <summary>
        /// 身份信息解密(用户提交)【可以根据用户提交的固定的字符串到缓存读取此凭据】
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="hash">hash字符串</param>
        /// <param name="userRole">用户角色</param>
        /// <returns>用户标识</returns>
        private static int TokenHashDecrypt(this ApiController apiController, string hash, int userRole)
        {
            try
            {
                string[] values = apiController.HashDecrypt(hash);
                if (values[1] != userRole.ToString())
                    throw new TokenException();
                return Convert.ToInt32(values[0]);
            }
            catch
            {
                throw new TokenException();
            }
        }
        /// <summary>
        /// 身份信息加密（返回给用户）【可以缓存此凭据后返回固定字符串】
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="userId">用户标识</param>
        /// <param name="userRole">用户角色</param>
        /// <returns>返回给用户的字符串</returns>
        private static string TokenHashEncrypt(this ApiController apiController, int userId, int userRole)
        {
            return apiController.HashEncrypt(userId.ToString(), userRole.ToString(), TimeStamp.ToString());//每次都不一致
        }
        /// <summary>
        /// Hash字符串（可用于Token凭据、SMS验证码）生成
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        private static string HashEncrypt(this ApiController apiController, params string[] values)
        {
            string hash = JsonConvert.SerializeObject(values);
            return Hash.Encrypt.AES128(hash);//aes
        }
        /// <summary>
        /// Hash字符串（可用于Token凭据、SMS验证码）解析
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        private static string[] HashDecrypt(this ApiController apiController, string hash)
        {
            string vaules = Hash.Decrypt.AES128(hash);//aes
            return JsonConvert.DeserializeObject<string[]>(vaules);
        }
    }
}
