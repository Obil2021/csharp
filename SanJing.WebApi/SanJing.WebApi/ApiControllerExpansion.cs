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
            if (apiController.TimeStamp() > max || apiController.TimeStamp() < min)
                throw new TimeStampException();

            //随机串验证
            if (!request.CheckNonceString(apiController.Request.RequestUri.AbsolutePath, ipAddress))
                throw new NonceStringException();
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long TimeStamp(this ApiController apiController) => (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
    }
}
