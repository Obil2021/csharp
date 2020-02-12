using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace SanJing.MsgPush
{
    public class Meizu
    {
        /// <summary>
        /// 魅族推送
        /// </summary>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        /// <param name="msg">消息</param>
        /// <param name="tags">目标ID</param>
        /// <returns></returns>
        public static string Push(string appId, string appKey, string msg, string[] tags)
        {
            List<string> result = new List<string>();
            int limit = 1000;//单次上线
            int pagecount = tags.Length / limit;
            pagecount = tags.Length % limit == 0 ? pagecount : pagecount + 1;
            for (int i = 0; i < pagecount; i++)
            {
                var sendids = tags.Skip(i * limit).Take(limit).ToArray();
                string rids = string.Join(",", sendids);

                using (var web = new WebClient())
                {
                    web.Encoding = Encoding.UTF8;

                    var formData = new NameValueCollection();

                    formData.Add("appId", appId);
                    formData.Add("pushIds", rids);
                    formData.Add("messageJson", Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        content = msg,
                        pushTimeInfo = new
                        {
                            offLine = 1,
                            validTime = 24,
                        }
                    }));
                    formData.Add("sign", SanJing.Hash.Encrypt.MD5($"appId={formData["appId"]}pushIds={formData["pushIds"]}messageJson={formData["messageJson"]}{appKey}").ToLower());


                    var res = web.UploadValues(" http://server-apimzups.meizu.com/ups/api/server/push/unvarnished/pushByPushId", formData);
                    result.Add(Encoding.UTF8.GetString(res));
                }
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }
}
