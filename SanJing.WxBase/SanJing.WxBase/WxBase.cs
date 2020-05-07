using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SanJing.WxBase
{
    /// <summary>
    /// 微信API开发基础类
    /// </summary>
    public class Core
    {
        private const string MSG_ISNULLORWHITESPACE = "IsNullOrWhiteSpace";
        private const string MSG_NOTCONTAINSKEY = "Not ContainsKey";
        private const string MSG_ISNULL = "Is Null";
        private const string MSG_ERROR = "Error";
        private const string ID_SIGN = "sign";
        private const string VAL_SUCCESS = "SUCCESS";

        /// <summary>
        /// 缓存TOKEN至文件
        /// </summary>
        /// <param name="fileName">文件完整路径(建议文件名为APPID)</param>
        /// <param name="token">TOKEN</param>
        /// <param name="expire">到期时间</param>
        public static void WriteCacheToken(string fileName, string token, DateTime expire)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(fileName));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(token));
            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(new { token, expire }),
                Encoding.UTF8);
        }
        /// <summary>
        /// 缓存TICKET至文件
        /// </summary>
        /// <param name="fileName">文件完整路径(建议文件名为APPID)</param>
        /// <param name="ticket">TICKET</param>
        /// <param name="expire">到期时间</param>
        public static void WriteCacheTicket(string fileName, string ticket, DateTime expire)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(fileName));
            }

            if (string.IsNullOrWhiteSpace(ticket))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(ticket));
            }

            File.WriteAllText(fileName, JsonConvert.SerializeObject(new { ticket, expire }),
                Encoding.UTF8);
        }
        /// <summary>
        /// 读取缓存文件中的有效TOKEN
        /// </summary>
        /// <param name="fileName">文件完整路径(建议文件名为APPID)</param>
        /// <param name="token">TOKEN</param>
        /// <returns>不存在货已过期则返回FALSE</returns>
        public static bool ReadCacheToken(string fileName, out string token)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(fileName));
            }

            token = string.Empty;
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName, Encoding.UTF8);
                var tokenObj = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                var tokenExpire = Convert.ToDateTime(tokenObj["expire"], CultureInfo.CurrentCulture);
                if (DateTime.Now < tokenExpire)
                {
                    token = Convert.ToString(tokenObj["token"], CultureInfo.CurrentCulture);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 读取缓存文件中的有效TICKET
        /// </summary>
        /// <param name="fileName">文件完整路径(建议文件名为APPID)</param>
        /// <param name="ticket">TICKET</param>
        /// <returns></returns>
        public static bool ReadCacheTicket(string fileName, out string ticket)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(fileName));
            }

            ticket = string.Empty;
            if (File.Exists(fileName))
            {
                var json = File.ReadAllText(fileName, Encoding.UTF8);
                var tokenObj = JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
                var tokenExpire = Convert.ToDateTime(tokenObj["expire"], CultureInfo.CurrentCulture);
                if (DateTime.Now < tokenExpire)
                {
                    ticket = Convert.ToString(tokenObj["ticket"], CultureInfo.CurrentCulture);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <returns></returns>
        public static long TimeStamp() => (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <returns></returns>
        public static string NonceStr()
        {
            return Guid.NewGuid().ToString("N", CultureInfo.CurrentCulture).Substring(6, 22);
        }

        /// <summary>
        /// 返回XML|SUCCESS
        /// </summary>
        /// <returns>XML</returns>
        public static string SuccessString
        {
            get
            {
                var result = new Dictionary<string, object>
                {
                    { "return_code", VAL_SUCCESS },
                    { "return_msg", "OK" }
                };
                return DictionaryXml(result);
            }
        }

        /// <summary>
        /// TOKEN验证
        /// </summary>
        /// <param name="token"></param>
        /// <param name="signAture"></param>
        /// <param name="timeStamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public static bool TokenValidation(string token, string signAture, string timeStamp, string nonce)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(token));
            }

            if (string.IsNullOrWhiteSpace(signAture))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(signAture));
            }

            if (string.IsNullOrWhiteSpace(timeStamp))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(timeStamp));
            }

            if (string.IsNullOrWhiteSpace(nonce))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(nonce));
            }

            var arrtemp = new[] { token, timeStamp, nonce };//TOKEN

            Array.Sort(arrtemp);     //字典排序

            var temp = string.Join(string.Empty, arrtemp);

            return Sha1Encrypt(temp) == signAture;
        }
        /// <summary>
        /// SHA1签名
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Sha1Sign(IDictionary<string, object> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var kv = new SortedDictionary<string, object>(data).Select(e => string.Format(CultureInfo.CurrentCulture, "{0}={1}", e.Key, e.Value));
            return Sha1Encrypt(string.Join("&", kv));
        }
        /// <summary>
        /// MD5签名
        /// </summary>
        /// <param name="mchKey">商户号密钥</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Md5Sign(string mchKey, IDictionary<string, object> data)
        {
            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var kv = new SortedDictionary<string, object>(data).Select(e => string.Format(CultureInfo.CurrentCulture, "{0}={1}", e.Key, e.Value));
            var temp = string.Join("&", kv) + $"&key={mchKey}";
            return Md5Encrypt(temp);
        }
        /// <summary>
        /// 流转字典数据
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns>XML</returns>
        public static async Task<IDictionary<string, object>> StreamToDictionaryAsync(Stream inputStream)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            using (StreamReader read = new StreamReader(inputStream, Encoding.UTF8))
            {
                return XmlDictionary(await read.ReadToEndAsync().ConfigureAwait(true));
            }
        }
        /// <summary>
        /// 流转字典数据并进行验签
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="mchKey">商户密钥</param>
        /// <returns></returns>
        public static async Task<IDictionary<string, object>> StreamToDictionaryAsync(Stream inputStream, string mchKey)
        {
            if (inputStream == null)
            {
                throw new ArgumentNullException(nameof(inputStream));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            return MD5SignCheck(SuccessVerify(await StreamToDictionaryAsync(inputStream).ConfigureAwait(true)), mchKey);
        }
        /// <summary>
        /// 下载文件(含图片)
        /// </summary>
        /// <param name="fileName">保存文件的完整路劲</param>
        /// <param name="url"></param>
        public static async Task ApiGetFileRequestAsync(string fileName, string url)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(fileName));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(url));
            }

            using (var web = new WebClient())
            {
                web.Encoding = Encoding.UTF8;
                await web.DownloadFileTaskAsync(url, fileName).ConfigureAwait(true);
            }
        }
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isSuccessVerify">判断[errcode]和[return_code],不成功抛出异常[ArgumentException];</param>
        /// <returns>JSON</returns>
        public static async Task<IDictionary<string, object>> ApiGetRequestAsync(string url, bool isSuccessVerify = false)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(url));
            }

            if (isSuccessVerify)
                return SuccessVerify(await ApiGetRequestAsync(url, false).ConfigureAwait(true));
            else
                using (var web = new WebClient())
                {
                    web.Encoding = Encoding.UTF8;
                    return JsonConvert.DeserializeObject<IDictionary<string, object>>(await web.DownloadStringTaskAsync(url).ConfigureAwait(true));
                }
        }
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="isSuccessVerify">判断[errcode]和[return_code],不成功抛出异常[ArgumentException];</param>
        /// <returns>JSON</returns>
        public static async Task<IDictionary<string, object>> ApiPostJsonRequestAsync(IDictionary<string, object> data, string url,
            bool isSuccessVerify = false)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (isSuccessVerify)
                return SuccessVerify(await ApiPostJsonRequestAsync(data, url, false).ConfigureAwait(true));
            else
                using (var web = new WebClient())
                {
                    web.Encoding = Encoding.UTF8;
                    web.Headers.Add("Content-Type", "text/json");
                    var result = await web.UploadStringTaskAsync(url, "POST", JsonConvert.SerializeObject(data)).ConfigureAwait(true);
                    return JsonConvert.DeserializeObject<IDictionary<string, object>>(result);
                }
        }
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="url"></param>
        /// <param name="isSuccessVerify">判断[errcode]和[return_code],不成功抛出异常[ArgumentException];</param>
        /// <returns>XML</returns>
        public static async Task<IDictionary<string, object>> ApiPostXmlRequestAsync(
            IDictionary<string, object> data, string url, bool isSuccessVerify = false)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (isSuccessVerify)
                return SuccessVerify(await ApiPostXmlRequestAsync(data, url, false).ConfigureAwait(true));
            else
                using (var web = new WebClient())
                {
                    web.Encoding = Encoding.UTF8;
                    web.Headers.Add("Content-Type", "text/xml");
                    var result = await web.UploadStringTaskAsync(url, "POST", DictionaryXml(data)).ConfigureAwait(true);
                    return XmlDictionary(result);
                }
        }
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mchId">商户号</param>
        /// <param name="certPath">密钥文件完整路径</param>
        /// <param name="url"></param>
        /// <param name="isSuccessVerify">判断[errcode]和[return_code],不成功抛出异常[ArgumentException];</param>
        /// <returns>XML</returns>
        public static async Task<IDictionary<string, object>> ApiPostXmlRequestWithCertAsync(IDictionary<string, object> data,
            string mchId, string certPath, string url, bool isSuccessVerify = false)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrWhiteSpace(mchId))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchId));
            }

            if (string.IsNullOrWhiteSpace(certPath))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(certPath));
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(url));
            }

            if (isSuccessVerify)
                return SuccessVerify(await ApiPostXmlRequestWithCertAsync(data, mchId, certPath, url, false).ConfigureAwait(true));
            else
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                X509Certificate2 cer = new X509Certificate2(certPath, mchId, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
                HttpWebRequest webrequest = WebRequest.CreateHttp(new Uri(url));
                webrequest.ClientCertificates.Add(cer);
                webrequest.Method = "POST";
                webrequest.ContentType = "text/xml";
                var bytes = Encoding.UTF8.GetBytes(DictionaryXml(data));
                webrequest.ContentLength = bytes.Length;
                await (await webrequest.GetRequestStreamAsync().ConfigureAwait(true)).WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(true);
                using (WebResponse webreponse = await webrequest.GetResponseAsync().ConfigureAwait(true))
                {
                    using (Stream stream = webreponse.GetResponseStream())
                    {
                        string result = string.Empty;
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            result = await reader.ReadToEndAsync().ConfigureAwait(true);
                        }
                        return XmlDictionary(result);
                    }
                }
            }
        }
        private static string DictionaryXml(IDictionary<string, object> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            XmlDocument doc = new XmlDocument() { XmlResolver = null };
            var root = doc.CreateElement("xml");
            doc.AppendChild(root);
            XmlItem(doc, root, data);
            return doc.OuterXml;
        }
        private static IDictionary<string, object> XmlDictionary(string xml)
        {
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(xml));
            }

            var result = new Dictionary<string, object>();
            XmlDocument doc = new XmlDocument() { XmlResolver = null };
            doc.LoadXml(xml);
            XmlElement rootElement = doc.DocumentElement;
            foreach (XmlNode item in rootElement.ChildNodes)
            {
                result.Add(item.Name, item.LastChild.Value);
            }
            return result;
        }
        private static void XmlItem(XmlDocument document, XmlElement element, IDictionary<string, object> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            foreach (var item in data)
            {
                var elementtemp = document.CreateElement(item.Key);
                element.AppendChild(elementtemp);
                if (item.Value is IEnumerable<IDictionary<string, object>>)
                {
                    foreach (var item2 in item.Value as IEnumerable<IDictionary<string, object>>)
                    {
                        var xmlitem = document.CreateElement("item");
                        elementtemp.AppendChild(xmlitem);
                        XmlItem(document, xmlitem, item2 as IDictionary<string, object>);
                    }
                }
                else if (item.Value is Dictionary<string, object>)
                {
                    XmlItem(document, elementtemp, item.Value as IDictionary<string, object>);
                }
                else
                {
                    var nodetype = item.Value is string ? XmlNodeType.CDATA : XmlNodeType.Text;
                    var node = document.CreateNode(nodetype, item.Key, string.Empty);
                    node.Value = item.Value.ToString();
                    elementtemp.AppendChild(node);
                }
            }
        }
        private static IDictionary<string, object> SuccessVerify(IDictionary<string, object> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.ContainsKey("errcode"))
                if (data["errcode"].ToString() != "0")
                    throw new ArgumentException(JsonConvert.SerializeObject(data), "errcode");

            if (data.ContainsKey("result_code"))
                if (data["result_code"].ToString() != VAL_SUCCESS)
                    throw new ArgumentException(JsonConvert.SerializeObject(data), "result_code");

            if (data.ContainsKey("return_code"))
                if (data["return_code"].ToString() != VAL_SUCCESS)
                    throw new ArgumentException(JsonConvert.SerializeObject(data), "return_code");

            return data;
        }
        private static IDictionary<string, object> MD5SignCheck(IDictionary<string, object> data, string mchKey)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (string.IsNullOrWhiteSpace(mchKey))
            {
                throw new ArgumentException(MSG_ISNULLORWHITESPACE, nameof(mchKey));
            }

            var temp = new Dictionary<string, object>(data);
            if (!temp.ContainsKey(ID_SIGN))
                throw new ArgumentException(MSG_NOTCONTAINSKEY, ID_SIGN);
            var sign = temp[ID_SIGN] as string;
            if (string.IsNullOrEmpty(sign))
                throw new ArgumentException(MSG_ISNULL, ID_SIGN);
            temp.Remove(ID_SIGN);
            if (Md5Sign(mchKey, temp) == sign)
                return data;
            throw new ArgumentException(MSG_ERROR, ID_SIGN);
        }
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }
        private static string Md5Encrypt(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            using (var sha = new MD5CryptoServiceProvider())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(bytes).Replace("-", string.Empty).ToUpper(CultureInfo.CurrentCulture);
            }
        }
        private static string Sha1Encrypt(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            using (var sha = new SHA1CryptoServiceProvider())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(bytes).Replace("-", string.Empty).ToLower(CultureInfo.CurrentCulture);
            }
        }
    }
}
