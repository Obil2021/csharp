using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace SanJing.MsgPush
{
    public class Xiaomi
    {
        private static readonly Random Random = new Random();
        /// <summary>
        /// 小米推送，安卓版
        /// </summary>
        /// <param name="appKey">密钥</param>
        /// <param name="appPackageName">App的包名</param>
        /// <param name="msg">消息</param>
        /// <param name="title">标题</param>
        /// <param name="tags">目标ID</param>
        /// <returns></returns>
        public static string Push(string appKey, string appPackageName, string msg, string title, string[] tags)
        {
            string rids = string.Join(",", tags);

            using (var web = new WebClient())
            {
                web.Encoding = Encoding.UTF8;
                web.Headers[HttpRequestHeader.Authorization] = "key=" + appKey;

                var formData = new NameValueCollection();
                formData.Add("payload", System.Web.HttpUtility.UrlEncode(msg));//消息的内容
                formData.Add("restricted_package_name", appPackageName);//App的包名
                formData.Add("title", title);
                formData.Add("description", msg);
                formData.Add("notify_type", "-1");
                formData.Add("notify_id", Random.Next(10000000, 100000000).ToString());
                formData.Add("registration_id", rids);
                var res = web.UploadValues("https://api.xmpush.xiaomi.com/v2/message/regid", formData);

                return Encoding.UTF8.GetString(res);
            }
        }

    }
}
