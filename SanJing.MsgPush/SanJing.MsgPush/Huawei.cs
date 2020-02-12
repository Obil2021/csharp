using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SanJing.MsgPush
{
    public class Huawei
    {
        /// <summary>
        /// 华为推送
        /// </summary>
        /// <param name="appid">APPID</param>
        /// <param name="appkey">APPKEY</param>
        /// <param name="msg">消息</param>
        /// <param name="title">标题</param>
        /// <param name="tags">目标ID</param>
        /// <returns></returns>
        public static string Push(string appid, string appkey, string msg, string title, string[] tags)
        {

            string postDataStr = string.Format("grant_type=client_credentials&client_secret={0}&client_id={1}", appkey, appid);
            string Url = "https://login.cloud.huawei.com/oauth2/v2/token";
            byte[] bytes = Encoding.UTF8.GetBytes(postDataStr);
            WebClient client = new WebClient();
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            client.Headers.Add("Host", "Login.cloud.huawei.com");
            byte[] responseData = client.UploadData(Url, "POST", bytes);
            string retString = Encoding.UTF8.GetString(responseData);
            Newtonsoft.Json.Linq.JObject jobject = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(retString);
            string access_token = jobject["access_token"].ToString();
            string retStrings = System.Web.HttpUtility.UrlEncode(access_token, System.Text.Encoding.UTF8); //access_token编码
            string nsp_svc = "openpush.message.api.send";  //本接口固定为openpush.message.api.send。
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string nsp_ts = Convert.ToInt64(ts.TotalSeconds).ToString();  //时间戳
            string payload = "{\"hps\": {\"msg\":{\"type\":3,\"body\":{\"content\":\"" + msg + "\", \"title\":\"" + title + "\"},";
            payload += "\"action\":{\"type\":3, \"param\":{\"appPkgName\":\"com.isdsc.dcwa_app\"}}}}}";


            string nsp_ctx = System.Web.HttpUtility.UrlEncode("{\"ver\":\"1\", \"appId\":\"" + appid + "\"}");//"%7b%22ver%22%3a%221%22%2c+%22appId%22%3a%22100535109%22%7d";
            int page = 0; //
            int limit = 5;  //数量
            int Tokenpage = tags.Length / limit;
            Tokenpage = tags.Length % limit > 0 ? Tokenpage + 1 : Tokenpage;
            List<string> result = new List<string>();
            for (int i = 0; i < Tokenpage; i++)
            {
                page = page + i;
                int Gettoken = page * limit;
                // var s = string.Format(@"select top {0} Token  from HuaweiUserToken where ID not in(select top {1} ID from HuaweiUserToken)", limit, Gettoken);
                string[] ListStr = tags.Skip(Gettoken).Take(limit).ToArray();
                string device_token_list = Newtonsoft.Json.JsonConvert.SerializeObject(ListStr);  //数组转化为json字符串

                SortedDictionary<string, string> request_s = new SortedDictionary<string, string>(StringComparer.Ordinal);
                request_s.Add("access_token", retStrings);
                request_s.Add("nsp_svc", nsp_svc);
                request_s.Add("nsp_ts", nsp_ts);
                request_s.Add("device_token_list", device_token_list);
                request_s.Add("payload", payload);
                string Url_s = string.Format("https://api.push.hicloud.com/pushsend.do?nsp_ctx={0}", nsp_ctx);
                string apiUrl = Url_s + "&" + string.Join("&", request_s.Select(q => string.Format("{0}={1}", q.Key, q.Value)));

                string result_s;
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    result_s = wc.DownloadString(apiUrl);
                }
                result.Add(result_s);
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }
    }
}
