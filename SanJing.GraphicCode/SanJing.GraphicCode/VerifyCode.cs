using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.GraphicCode
{
    /// <summary>
    /// 生成图片验证码【联网版】
    /// https://github.com/MZCretin/RollToolsApi#%E7%94%9F%E6%88%90%E9%9A%8F%E6%9C%BA%E5%9B%BE%E7%89%87%E9%AA%8C%E8%AF%81%E7%A0%81
    /// </summary>
    public class VerifyCode
    {
        /// <summary>
        /// 获取5位长度验证码图片的base64字符串（注：Base64字符串前面默认添加了“data:image/jpg;base64,”，取值的时候请根据需要对这个内容进行处理）
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <returns>base64字符串</returns>
        public static string GetBase64String(out string verifyCode)
        {
            verifyCode = string.Empty;
            using (var web = new System.Net.WebClient())
            {
                web.Encoding = System.Text.Encoding.UTF8;
                web.Headers[System.Net.HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                var json = web.DownloadString("https://www.mxnzp.com/api/verifycode/code?len=5&type=1");
                var vcode = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
                if (vcode.code != 1)
                    throw new Exception(json);
                verifyCode = vcode.data.verifyCode;
                return vcode.data.verifyCodeBase64;
            }
        }
        /// <summary>
        /// 获取5位长度验证码图片URL地址
        /// </summary>
        /// <param name="verifyCode">验证码</param>
        /// <returns>URL地址</returns>
        public static string GetUrlAddress(out string verifyCode)
        {
            verifyCode = string.Empty;
            using (var web = new System.Net.WebClient())
            {
                web.Encoding = System.Text.Encoding.UTF8;
                web.Headers[System.Net.HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                var json = web.DownloadString("https://www.mxnzp.com/api/verifycode/code?len=5&type=0");
                var vcode = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
                if (vcode.code != 1)
                    throw new Exception(json);
                verifyCode = vcode.data.verifyCode;
                return vcode.data.verifyCodeImgUrl;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            public string verifyCode { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string verifyCodeImgUrl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string verifyCodeBase64 { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string whRatio { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class RootObject
        {
            /// <summary>
            /// 
            /// </summary>
            public int code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string msg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Data data { get; set; }
        }
    }
}
