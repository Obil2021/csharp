using SanJing.VOD.TencentResult;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

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
            strContent += ("&Version=" + "2018-07-17");
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
        /// <summary>
        /// 修改指定视频名称
        /// </summary>
        /// <param name="videoid">视频ID</param>
        /// <param name="name">视频名称</param>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        public static void UpdateName(string videoid, string name, string appId, string appKey)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "IsNullOrWhiteSpace");
            }
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var m_qwNowTime = Convert.ToInt64(ts.TotalSeconds);
            var m_iRandom = random.Next(10000000, 100000000);

            string strContent = "Action=ModifyMediaInfo";
            strContent += ("&FileId=" + videoid);
            strContent += ("&Name=" + name);
            strContent += ("&Nonce=" + m_iRandom);
            strContent += ("&SecretId=" + Uri.EscapeDataString(appId));
            strContent += ("&Timestamp=" + m_qwNowTime);
            strContent += ("&Version=" + "2018-07-17");
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
        /// <summary>
        /// 修改指定视频封面
        /// </summary>
        /// <param name="videoid">视频ID</param>
        ///  <param name="fullfilename">图片完整地址【jpeg|png】</param>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        public static string UpdateCover(string videoid, string fullfilename, string appId, string appKey)
        {
            if (string.IsNullOrWhiteSpace(fullfilename))
            {
                throw new ArgumentNullException("fullfilename", "IsNullOrWhiteSpace");
            }
            if (!File.Exists(fullfilename))
                throw new ArgumentNullException("fullfilename", $"{fullfilename} 不存在");
            string ex = Path.GetExtension(fullfilename).ToLower();
            if (!new[] { ".jpeg", ".jpg", ".png" }.Contains(ex))
            {
                throw new ArgumentException("fullfilename", $"{fullfilename} 不支持的图片格式");
            }
            string coverdata = string.Empty;
            using (Bitmap bmp = new Bitmap(fullfilename))
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    if (ex == ".jpeg" || ex == ".jpg")
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    coverdata = Convert.ToBase64String(arr);
                }
            }
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var m_qwNowTime = Convert.ToInt64(ts.TotalSeconds);
            var m_iRandom = random.Next(10000000, 100000000);

            var strContent = $"POSTvod.tencentcloudapi.com/?Action=ModifyMediaInfo&CoverData={coverdata}&FileId={videoid}&Language=zh-CN&Nonce={m_iRandom}&Region=&SecretId={appId}&Timestamp={m_qwNowTime}&Token=&Version=2018-07-17";
            var strSignature = signature(appKey, strContent);
            var kvContent = strContent.Split('?')[1].Split('&');
            var kvSortedDictionary = new SortedDictionary<string, string>();
            foreach (var item in kvContent)
            {
                kvSortedDictionary.Add(item.Split('=')[0], HttpUtility.UrlEncode(item.Split('=')[1]));
            }
            kvSortedDictionary.Add("Signature", HttpUtility.UrlEncode(strSignature));
            string strPost = string.Join("&", kvSortedDictionary.Select(q => $"{q.Key}={q.Value}"));
            string apiUrl = "https://vod.tencentcloudapi.com/?";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                wc.Headers[System.Net.HttpRequestHeader.Host] = "vod.tencentcloudapi.com";
                wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var result = wc.UploadString(apiUrl, "POST", strPost);
                if (result.Contains("Error"))
                {
                    throw new Exception(result);
                }
                var updateCoverResult = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateCoverResult>(result);
                return updateCoverResult.Response.CoverUrl;
            }
        }
        /// <summary>
        /// 修改指定视频名称及封面
        /// </summary>
        /// <param name="videoid">视频ID</param>
        /// <param name="name">视频名称</param>
        ///  <param name="fullfilename">图片完整地址【jpeg|png】</param>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        public static string UpdateNameAndCover(string videoid, string name, string fullfilename, string appId, string appKey)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name", "IsNullOrWhiteSpace");
            }
            if (string.IsNullOrWhiteSpace(fullfilename))
            {
                throw new ArgumentNullException("fullfilename", "IsNullOrWhiteSpace");
            }
            if (!File.Exists(fullfilename))
                throw new ArgumentNullException("fullfilename", $"{fullfilename} 不存在");
            string ex = Path.GetExtension(fullfilename).ToLower();
            if (!new[] { ".jpeg", ".jpg", ".png" }.Contains(ex))
            {
                throw new ArgumentException("fullfilename", $"{fullfilename} 不支持的图片格式");
            }
            string coverdata = string.Empty;
            using (Bitmap bmp = new Bitmap(fullfilename))
            {

                using (MemoryStream ms = new MemoryStream())
                {
                    if (ex == ".jpeg" || ex == ".jpg")
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    else
                    {
                        bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    coverdata = Convert.ToBase64String(arr);
                }
            }
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var m_qwNowTime = Convert.ToInt64(ts.TotalSeconds);
            var m_iRandom = random.Next(10000000, 100000000);

            var strContent = $"POSTvod.tencentcloudapi.com/?Action=ModifyMediaInfo&CoverData={coverdata}&FileId={videoid}&Language=zh-CN&Name={name}&Nonce={m_iRandom}&Region=&SecretId={appId}&Timestamp={m_qwNowTime}&Token=&Version=2018-07-17";
            var strSignature = signature(appKey, strContent);
            var kvContent = strContent.Split('?')[1].Split('&');
            var kvSortedDictionary = new SortedDictionary<string, string>();
            foreach (var item in kvContent)
            {
                kvSortedDictionary.Add(item.Split('=')[0], HttpUtility.UrlEncode(item.Split('=')[1]));
            }
            kvSortedDictionary.Add("Signature", HttpUtility.UrlEncode(strSignature));
            string strPost = string.Join("&", kvSortedDictionary.Select(q => $"{q.Key}={q.Value}"));
            string apiUrl = "https://vod.tencentcloudapi.com/?";
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                wc.Headers[System.Net.HttpRequestHeader.Host] = "vod.tencentcloudapi.com";
                wc.Headers[System.Net.HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var result = wc.UploadString(apiUrl, "POST", strPost);
                if (result.Contains("Error"))
                {
                    throw new Exception(result);
                }
                var updateCoverResult = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateCoverResult>(result);
                return updateCoverResult.Response.CoverUrl;
            }
        }
        /// <summary>
        /// 查询指定视频基础信息
        /// </summary>
        /// <param name="videoid">视频ID</param>
        /// <param name="appId">APPID</param>
        /// <param name="appKey">APPKEY</param>
        public static BasicInfo SelectBasicInfo(string videoid, string appId, string appKey)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var m_qwNowTime = Convert.ToInt64(ts.TotalSeconds);
            var m_iRandom = random.Next(10000000, 100000000);

            string strContent = "Action=DescribeMediaInfos";
            strContent += ("&FileIds.0=" + videoid);
            strContent += ("&Filters.0=" + "basicInfo");
            strContent += ("&Nonce=" + m_iRandom);
            strContent += ("&SecretId=" + Uri.EscapeDataString(appId));
            strContent += ("&Timestamp=" + m_qwNowTime);
            strContent += ("&Version=" + "2018-07-17");
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
                var selectBasicInfoResult = Newtonsoft.Json.JsonConvert.DeserializeObject<SelectBasicInfoResult>(result);
                return selectBasicInfoResult.Response.MediaInfoSet[0].BasicInfo;
            }
        }
    }
}
