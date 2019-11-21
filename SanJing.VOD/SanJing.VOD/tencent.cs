using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SanJing.VOD
{
    /// <summary>
    /// 腾讯云视频点播
    /// </summary>
    public class Tencent
    {
        /// <summary>
        /// 随机种子
        /// </summary>
        private static readonly Random random = new Random();
        /// <summary>
        /// HMACSHA1
        /// </summary>
        /// <param name="signatureString"></param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        private static byte[] hash_hmac_byte(string signatureString, string secretKey)
        {
            var enc = Encoding.UTF8;
            HMACSHA1 hmac = new HMACSHA1(enc.GetBytes(secretKey));
            hmac.Initialize();
            byte[] buffer = enc.GetBytes(signatureString);
            return hmac.ComputeHash(buffer);
        }
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="m_strSecKey">APPKEY</param>
        /// <param name="m_strSecKey">STRContent</param>
        /// <returns></returns>
        private static string signature(string m_strSecKey, string strContent)
        {
            byte[] bytesSign = hash_hmac_byte(strContent, m_strSecKey);
            return Convert.ToBase64String(bytesSign);
        }
        /// <summary>
        /// （前端）获取上传视频签名(有效期（两天）内缓存)
        /// 上传示例：
        /// </summary>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        /// <param name="appKey">签名缓存完整文件路劲（全局）</param>
        /// <returns></returns>
        public static string GetUploadSignature(string signatureCacheFileName, string appId, string appKey)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var m_qwNowTime = Convert.ToInt64(ts.TotalSeconds);

            if (System.IO.File.Exists(signatureCacheFileName))
            {
                string cache = System.IO.File.ReadAllText(signatureCacheFileName);
                if (!string.IsNullOrWhiteSpace(cache))
                {
                    var items = cache.Split(' ');
                    if (items.Length == 2)
                    {
                        if (long.TryParse(items[0], out long cacheTime) && m_qwNowTime < cacheTime)
                        {
                            return items[1];//返回缓存
                        }
                    }
                }
            }

            var m_iSignValidDuration = 3600 * 24 * 2;//两天
            var m_qwExpireTime = (m_qwNowTime + m_iSignValidDuration);
            var m_iRandom = random.Next(10000000, 100000000);

            string strContent = ("secretId=" + Uri.EscapeDataString(appId));
            strContent += ("&currentTimeStamp=" + m_qwNowTime);
            strContent += ("&expireTime=" + m_qwExpireTime);
            strContent += ("&random=" + m_iRandom);

            byte[] bytesSign = hash_hmac_byte(strContent, appKey);
            byte[] byteContent = System.Text.Encoding.Default.GetBytes(strContent);
            byte[] nCon = new byte[bytesSign.Length + byteContent.Length];
            bytesSign.CopyTo(nCon, 0);
            byteContent.CopyTo(nCon, bytesSign.Length);

            string result = Convert.ToBase64String(nCon);
            //缓存至文件
            System.IO.File.WriteAllText(signatureCacheFileName, $"{m_qwExpireTime} {result}");
            return result;

        }
        /// <summary>
        /// 删除指定视频
        /// </summary>
        /// <param name="videoid">视频ID</param>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        public static void Delete(string videoid, string appId, string appKey)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var m_qwNowTime = Convert.ToInt64(ts.TotalSeconds);
            var m_iRandom = random.Next(10000000, 100000000);

            string strContent = "Action=DeleteMedia";
            strContent += ("&FileId=" + videoid);
            strContent += ("&Nonce=" + m_iRandom);
            strContent += ("&SecretId=" + Uri.EscapeDataString(appId));
            strContent += ("&Timestamp=" + m_qwNowTime);
            strContent += "&Version=2018-07-17";
            strContent += ("&Signature=" + signature(appKey, $"GETvod.tencentcloudapi.com/?{strContent}"));
            string apiUrl = "https://vod.tencentcloudapi.com/?" + strContent;
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                var result = wc.DownloadString(apiUrl);
                if (result.Contains("Error"))
                {
                    throw new Exception(result);
                }
            }
        }
    }
}
