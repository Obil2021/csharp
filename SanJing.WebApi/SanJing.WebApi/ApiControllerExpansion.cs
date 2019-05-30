using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public const string MS_HTTPCONTEXT = "MS_HttpContext";
        /// <summary>
        /// 默认IP地址
        /// </summary>
        public const string IP_ADDRESS = "0.0.0.0";
        /// <summary>
        /// 请求准备(含数IP地址验证、数据验证、签名验证、时间戳验证，随机串验证、日志记录)
        /// </summary>
        /// <param name="apiController">API</param>
        /// <param name="request">请求数据</param>
        /// <param name="key">验签Key</param>
        public static void RequestReady(this ApiController apiController, RequestModel request, string key)
        {

            HttpContextWrapper httpContext = apiController.Request.Properties[MS_HTTPCONTEXT] as HttpContextWrapper;
            string ipAddress = IP_ADDRESS;
            if (httpContext != null)
            {
                ipAddress = httpContext.Request.UserHostAddress;
            }
            //请求记录
            NLog.LogManager.GetCurrentClassLogger().Info(JsonConvert.SerializeObject(new { IPAddress = ipAddress, Request = request }));

            //IP地址验证
            if (request.IPAddressBlackList().Contains(ipAddress))
                throw new IPAddressException();

            //数据验证
            if (!apiController.ModelState.IsValid)
            {
                throw new ValidException(string.Join(Environment.NewLine,
                    apiController.ModelState.Select(q => q.Value.Errors.First().ErrorMessage)));
            }

            //签名验证
            if (!request.CheckSign(key))
            {
                throw new SignException();
            }

            //时间戳验证
            long max = request.TimeStamp + request.TimeStampLimit();
            long min = request.TimeStamp - request.TimeStampLimit();
            if (TimeStamp > max || TimeStamp < min)
                throw new TimeStampException();

            //随机串验证
            if (!request.CheckNonceString(apiController.Request.RequestUri.AbsolutePath, ipAddress))
                throw new NonceStringException();
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
        /// 获取服务器实例
        /// </summary>
        /// <param name="apiController"></param>
        /// <returns></returns>
        public static HttpServerUtility GetServer(this ApiController apiController) => HttpContext.Current.Server;
        /// <summary>
        /// 身份信息解密(用户提交)【可以根据用户提交的固定的字符串到缓存读取此凭据】
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="hash">hash字符串</param>
        /// <param name="userRole">用户角色</param>
        /// <param name="day">有效期（天）</param>
        /// <returns>用户标识</returns>
        public static int TokenHashDecrypt(this ApiController apiController, string hash, int userRole, int day = 7)
        {
            try
            {
                string[] values = apiController.HashDecrypt(hash);
                if (values[1] != userRole.ToString() || Convert.ToInt64(values[2]) + day * 24 * 60 * 60 < TimeStamp)
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
        public static string TokenHashEncrypt(this ApiController apiController, int userId, int userRole)
        {
            return apiController.HashEncrypt(userId.ToString(), userRole.ToString(), TimeStamp.ToString());
        }
        /// <summary>
        /// 短信验证码解密(用户提交)
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="hash">hash字符串</param>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <param name="minute">有效期（分钟）</param>
        public static void SMSHashDecrypt(this ApiController apiController, string hash, string phone, string code, int minute = 5)
        {
            try
            {
                string[] values = apiController.HashDecrypt(hash);
                if (values[0] != phone || values[1] != code || Convert.ToInt64(values[2]) + minute * 60 < TimeStamp)
                    throw new UserException("验证码不正确");//过期
            }
            catch
            {
                throw new UserException("验证码不正确");
            }
        }
        /// <summary>
        /// 短信验证码加密（返回给用户）
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="phone">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns>返回给用户的字符串</returns>
        public static string SMSHashEncrypt(this ApiController apiController, string phone, string code)
        {
            return apiController.HashEncrypt(phone, code, TimeStamp.ToString());
        }

        /// <summary>
        /// Hash字符串（可用于Token凭据、SMS验证码）生成
        /// </summary>
        /// <param name="apiController"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string HashEncrypt(this ApiController apiController, params string[] values)
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
        public static string[] HashDecrypt(this ApiController apiController, string hash)
        {
            string vaules = Hash.Decrypt.AES128(hash);//aes
            return JsonConvert.DeserializeObject<string[]>(hash);
        }
    }
}
