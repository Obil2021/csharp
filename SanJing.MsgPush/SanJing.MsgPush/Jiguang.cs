using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SanJing.MsgPush
{
    /// <summary>
    /// 极光推送
    /// </summary>
    public class Jiguang
    {
        private static readonly Random Random = new Random();
        private static readonly string jg_Url = "http://api.jpush.cn:8800/v2/push";
        /// <summary>
        /// 广播所有设备
        /// </summary>
        /// <param name="msg">消息内容</param>
        /// <param name="appId">app_key</param>
        /// <param name="appKey">masterSecret</param>
        /// <returns></returns>
        public static string Push(string appId, string appKey, string msg)
        {
            int sendno = Random.Next(10000000, 100000000);
            string receiver_type = "4"; //广播推送
            string ConvertString = sendno + receiver_type + appKey;
            int msg_type = 1;
            string msg_content = "{\"n_content\":\"" + msg + "\"}";//发送内容
            string platform = "android, ios, winphone";
            string verification_code = SanJing.Hash.Encrypt.MD5(ConvertString);

            SortedDictionary<string, string> request = new SortedDictionary<string, string>(StringComparer.Ordinal);
            request.Add("sendno", sendno.ToString());
            request.Add("app_key", Uri.EscapeUriString(appId));
            request.Add("receiver_type", receiver_type.ToString());

            request.Add("verification_code", verification_code.ToLower());
            request.Add("msg_type", msg_type.ToString());
            request.Add("msg_content", msg_content);
            request.Add("platform", platform);
            string jg_postDataStr = string.Join("&", request.Select(q => string.Format("{0}={1}", q.Key, q.Value)));
            byte[] jg_bytes = Encoding.UTF8.GetBytes(jg_postDataStr);
            WebClient jg_client = new WebClient();
            jg_client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            jg_client.Headers.Add("ContentLength", jg_postDataStr.Length.ToString());
            byte[] jg_responseData = jg_client.UploadData(jg_Url, "POST", jg_bytes);
            return Encoding.UTF8.GetString(jg_responseData);
        }
        /// <summary>
        /// 指定设备推送
        /// </summary>
        /// <param name="msg">详细内容</param>
        /// <param name="registrationId">已注册设备标识</param>
        /// <param name="appId">app_key</param>
        /// <param name="appKey">masterSecret</param>
        /// <returns></returns>
        public static string Push(string appId, string appKey, string msg, string registrationId)
        {
            int sendno = Random.Next(10000000, 100000000);
            string receiver_type = "5"; //指定RegistrationID 
            string ConvertString = sendno + receiver_type + registrationId + appKey;
            int msg_type = 1;
            string msg_content = "{\"n_content\":\"" + msg + "\"}";//发送内容
            string platform = "android, ios, winphone";
            string verification_code = SanJing.Hash.Encrypt.MD5(ConvertString);

            SortedDictionary<string, string> request = new SortedDictionary<string, string>(StringComparer.Ordinal);
            request.Add("sendno", sendno.ToString());
            request.Add("app_key", Uri.EscapeUriString(appId));
            request.Add("receiver_type", receiver_type.ToString());
            request.Add("receiver_value", registrationId);
            request.Add("verification_code", verification_code.ToLower());
            request.Add("msg_type", msg_type.ToString());
            request.Add("msg_content", msg_content);
            request.Add("platform", platform);
            string jg_postDataStr = string.Join("&", request.Select(q => string.Format("{0}={1}", q.Key, q.Value)));
            byte[] jg_bytes = Encoding.UTF8.GetBytes(jg_postDataStr);
            WebClient jg_client = new WebClient();
            jg_client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            jg_client.Headers.Add("ContentLength", jg_postDataStr.Length.ToString());
            byte[] jg_responseData = jg_client.UploadData(jg_Url, "POST", jg_bytes);
            return Encoding.UTF8.GetString(jg_responseData);
        }
    }
}
