using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WxHelper
{
    public class WxHelper
    {
        private const string URL_LOGIN_LITE = "https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";
        private const string URL_TEMPLATE_SEND = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        private const string URL_ACCESS_TOKEN = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
        private const string URL_LOGIN_H5_AUTH = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect";
        private const string URL_LOGIN_H5ORMOBILE = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
        private const string URL_GETTICKET = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
        private const string URL_LOGIN_H5ORMOBILE_USERINFO = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
        private const string MSG_ISNULLORWHITESPACE = "IsNullOrWhiteSpace";

        /// <summary>
        /// 微信公众号接入时的TOKEN验证
        /// </summary>
        /// <param name="signature">微信GET过来的签名</param>
        /// <param name="timestamp">微信GET过来的时间戳</param>
        /// <param name="nonce">微信GET过来的随机数</param>
        /// <param name="token">自定义的TOKEN,需要在微信公众号后台填写的值</param>
        /// <param name="echostr">微信GET过来的随机字符串</param>
        /// <returns>echostr</returns>
        public static string CheckToken(string signature, string timestamp, string nonce, string token, string echostr)
        {
            if (string.IsNullOrWhiteSpace(signature))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(signature));
            }

            if (string.IsNullOrWhiteSpace(timestamp))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(timestamp));
            }
            if (string.IsNullOrWhiteSpace(echostr))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(echostr));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(token));
            }

            if (signature == WxBase.WxBase.Sha1Sign(new Dictionary<string, object>() { { "timestamp", timestamp }, { "nonce", nonce }, { "token", token } }))
                return echostr;
            else
                return string.Empty;
        }
        /// <summary>
        /// 获取Access_Token（仅支持公众号）
        /// </summary>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        /// <param name="accessTokenCacheFileName">TOKEN缓存文件完整路劲（全局）</param>
        /// <returns></returns>
        public static string GetAccessToken(string appId, string appKey, string accessTokenCacheFileName)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(appKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appKey));
            }

            if (string.IsNullOrWhiteSpace(accessTokenCacheFileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(accessTokenCacheFileName));
            }

            string accessToken;
            if (!WxBase.WxBase.ReadCacheToken(accessTokenCacheFileName, out accessToken))
            {
                var tokenurl = string.Format(URL_ACCESS_TOKEN, appId, appKey);
                var tokentemp = WxBase.WxBase.ApiGetRequest(tokenurl, true);
                accessToken = tokentemp["access_token"].ToString();
                WxBase.WxBase.WriteCacheToken(accessTokenCacheFileName, accessToken,
                    DateTime.Now.AddSeconds(Convert.ToDouble(tokentemp["expires_in"])));
            }
            return accessToken;
        }
        /// <summary>
        /// JSConfig参数（仅支持公众号）
        /// </summary>
        /// <param name="url">页面完整地址（含参数）</param>
        /// <param name="appId">APPID</param>
        /// <param name="accessToken">Access_Token</param>
        /// <param name="jsapiTicketCacheFileName">TICKET缓存文件完整路劲（全局）</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetJsConfig(string url, string appId,
            string accessToken, string jsapiTicketCacheFileName)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(url));
            }
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(accessToken));
            }

            if (string.IsNullOrWhiteSpace(jsapiTicketCacheFileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(jsapiTicketCacheFileName));
            }

            string jsapi_ticket;
            if (!WxBase.WxBase.ReadCacheTicket(jsapiTicketCacheFileName, out jsapi_ticket))
            {
                var ticketurl = string.Format(URL_GETTICKET, accessToken);
                var tickettemp = WxBase.WxBase.ApiGetRequest(ticketurl, true);
                jsapi_ticket = tickettemp["ticket"].ToString();
                WxBase.WxBase.WriteCacheTicket(jsapiTicketCacheFileName, jsapi_ticket, DateTime.Now.AddSeconds(Convert.ToDouble(tickettemp["expires_in"])));
            }
            var data = new Dictionary<string, object>();
            data.Add("noncestr", WxBase.WxBase.NonceStr());
            data.Add("jsapi_ticket", jsapi_ticket);
            data.Add("timestamp", WxBase.WxBase.TimeStamp());
            data.Add("url", url);


            data.Add("signature", WxBase.WxBase.Sha1Sign(data));
            data.Add("appId", appId);
            return data;
        }
        /// <summary>
        /// JSConfig参数（仅支持公众号）
        /// </summary>
        /// <param name="url">页面完整地址（含参数）</param>
        /// <param name="appId">APPID</param>
        /// <param name="appkey">APPKEY</param>
        /// <param name="accessTokenCacheFileName">Access_Token缓存文件完整路劲（全局）</param>
        /// <param name="jsapiTicketCacheFileName">TICKET缓存文件完整路劲（全局）</param>
        /// <returns></returns>
        public static Dictionary<string, object> GetJsConfig(string url, string appId,
           string appkey, string accessTokenCacheFileName, string jsapiTicketCacheFileName)
        {
            return GetJsConfig(url, appId, GetAccessToken(appId, appkey, accessTokenCacheFileName), jsapiTicketCacheFileName);
        }

        /// <summary>
        /// JSConfig代码（仅支持公众号）
        /// </summary>
        /// <param name="url">页面完整地址（含参数）</param>
        /// <param name="jsApiList">需要使用的JS接口列表</param>
        /// <param name="debug">开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。</param>
        /// <param name="appId">APPID</param>
        /// <param name="accessToken">Access_Token</param>
        /// <param name="jsapiTicketCacheFileName">TICKET缓存文件完整路劲（全局）</param>
        /// <returns></returns>
        public static string GetJsConfigCode(string url, string[] jsApiList, bool debug, string appId,
            string accessToken, string jsapiTicketCacheFileName)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(url));
            }

            if (jsApiList == null)
            {
                throw new ArgumentNullException(nameof(jsApiList));
            }

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(accessToken));
            }

            if (string.IsNullOrWhiteSpace(jsapiTicketCacheFileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(jsapiTicketCacheFileName));
            }

            string jsapi_ticket;
            if (!WxBase.WxBase.ReadCacheTicket(jsapiTicketCacheFileName, out jsapi_ticket))
            {
                var ticketurl = string.Format(URL_GETTICKET, accessToken);
                var tickettemp = WxBase.WxBase.ApiGetRequest(ticketurl, true);
                jsapi_ticket = tickettemp["ticket"].ToString();
                WxBase.WxBase.WriteCacheTicket(jsapiTicketCacheFileName, jsapi_ticket, DateTime.Now.AddSeconds(Convert.ToDouble(tickettemp["expires_in"])));
            }
            var data = new Dictionary<string, object>();
            data.Add("noncestr", WxBase.WxBase.NonceStr());
            data.Add("jsapi_ticket", jsapi_ticket);
            data.Add("timestamp", WxBase.WxBase.TimeStamp());
            data.Add("url", url);
            return string.Format("wx.config({{debug:{0},appId:'{1}',timestamp:{2},nonceStr:'{3}',signature:'{4}',jsApiList:[{5}]}});",
                debug ? "true" : "false", appId, data["timestamp"], data["noncestr"], WxBase.WxBase.Sha1Sign(data), string.Join(",",
                jsApiList.Select(q => string.Format("'{0}'", q.Trim('\'')))));
        }
        /// <summary>
        ///  JSConfig代码（仅支持公众号）
        /// </summary>
        /// <param name="url">页面完整地址（含参数）</param>
        /// <param name="jsApiList">需要使用的JS接口列表</param>
        /// <param name="debug">开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。</param>
        /// <param name="appId">APPID</param>
        /// <param name="appkey">APPKEY</param>
        /// <param name="accessTokenCacheFileName">TOKEN缓存文件完整路劲（全局）</param>
        /// <param name="jsapiTicketCacheFileName">TICKET缓存文件完整路劲（全局）</param>
        /// <returns></returns>
        public static string GetJsConfigCode(string url, string[] jsApiList, bool debug, string appId,
            string appkey, string accessTokenCacheFileName, string jsapiTicketCacheFileName)
        {
            return GetJsConfigCode(url, jsApiList, debug, appId, GetAccessToken(appId, appkey, accessTokenCacheFileName), jsapiTicketCacheFileName);
        }
        /// <summary>
        /// 发送模板消息（仅支持公众号）
        /// </summary>
        /// <param name="templateId">模板消息ID</param>
        /// <param name="openId">目标用户OPENID</param>
        /// <param name="url">消息跳转的完整链接地址</param>
        /// <param name="accessToken">ACCESS_TOKEN</param>
        /// <param name="templateMessages">模板消息参数</param>
        /// <returns>含有msgid</returns>
        public static IDictionary<string, object> SendTempMsg(string templateId, string openId, string url,
            string accessToken, params WxTempMsg[] templateMessages)
        {
            if (string.IsNullOrWhiteSpace(templateId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(templateId));
            }

            if (string.IsNullOrWhiteSpace(openId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(openId));
            }

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(accessToken));
            }

            if (templateMessages == null)
            {
                throw new ArgumentNullException(nameof(templateMessages));
            }

            string msgapiurl = string.Format(URL_TEMPLATE_SEND, accessToken);
            var data = new Dictionary<string, object>();
            data.Add("touser", openId);
            data.Add("template_id", templateId);
            if (!string.IsNullOrWhiteSpace(url))
                data.Add("url", url);
            var tdata = new Dictionary<string, object>();
            foreach (var item in templateMessages)
            {
                tdata.Add(item.Name, new { value = item.Value, color = item.Color });
            }
            data.Add("data", tdata);
            return WxBase.WxBase.ApiPostJsonRequest(data, msgapiurl, true);
        }
        /// <summary>
        /// 微信公众号登录授权地址
        /// </summary>
        /// <param name="appId">APPID</param>
        /// <param name="redirectUrl">回调地址（服务端接收CODE）</param>
        /// <param name="scope">snsapi_base、snsapi_userinfo</param>
        /// <param name="state">回调时原样返回</param>
        /// <returns>微信公众号登录授权地址</returns>
        public static string GetLoginUrl(string appId, string redirectUrl, string scope, string state)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(redirectUrl))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(redirectUrl));
            }

            if (string.IsNullOrWhiteSpace(scope))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(scope));
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            return string.Format(URL_LOGIN_H5_AUTH, appId, redirectUrl, scope, state);
        }
        /// <summary>
        /// 微信公众号或移动APP或H5网页（扫码）登录（获取openid）
        /// </summary>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        /// <param name="code">服务端或移动应用或H5页面JS获取的CODE</param>
        /// <returns>openid和unionid（Moble登录且已获得userinfo授权【手机端完成】时则有）</returns>
        public static IDictionary<string, object> LoginByAPPOrWxH5OrH5(string appId, string appKey, string code)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(appKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appKey));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(code));
            }

            var url = string.Format(URL_LOGIN_H5ORMOBILE, appId, appKey, code);
            return WxBase.WxBase.ApiGetRequest(url, true);
        }
        /// <summary>
        /// 微信公众号或移动APP登录，含基本资料（获取openid）
        /// </summary>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKE</param>
        /// <param name="code">移动应用或服务端获取的CODE</param>
        /// <returns>openid和unionid(关联开放平台则有)和nickname、sex、headimgurl</returns>
        public static IDictionary<string, object> LoginByAPPOrWxH5WithUserInfo(string appId, string appKey, string code)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(appKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appKey));
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(code));
            }

            var temp = LoginByAPPOrWxH5OrH5(appId, appKey, code);
            var url = string.Format(URL_LOGIN_H5ORMOBILE_USERINFO, temp["access_token"], temp["openid"]);
            return WxBase.WxBase.ApiGetRequest(url, true);
        }
        /// <summary>
        /// 微信小程序登录（获取openid）
        /// </summary>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        /// <param name="code">小程序获取的CODE</param>
        /// <returns>openid和unionid(关联开放平台则有)</returns>
        public static IDictionary<string, object> LoginByWxAPP(string appId, string appKey, string code)
        {
            if (string.IsNullOrWhiteSpace(appId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appId));
            }

            if (string.IsNullOrWhiteSpace(appKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(appKey));
            }

            var url = string.Format(URL_LOGIN_LITE, appId, appKey, code);
            return WxBase.WxBase.ApiGetRequest(url, true);
        }
    }
}
