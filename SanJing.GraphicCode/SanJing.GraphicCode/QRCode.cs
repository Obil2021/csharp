using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace SanJing.GraphicCode
{
    /// <summary>
    /// 二维码
    /// </summary>
    public class QRCode
    {
        /// <summary>
        /// 获取二维码码图片的base64字符串（注：Base64字符串前面默认添加了“data:image/jpg;base64,”，取值的时候请根据需要对这个内容进行处理）
        /// </summary>
        /// <param name="codeContent">条码类容</param>
        /// <param name="size">尺寸（默认500）</param>
        /// <returns>base64字符串</returns>
        public static string GetBase64String(string codeContent, int size = 500)
        {
            if (string.IsNullOrWhiteSpace(codeContent))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(codeContent));
            }
            if (size < 100 || size > 1000)
            {
                throw new ArgumentException("Value:100-1000", nameof(size));
            }
            using (var web = new System.Net.WebClient())
            {
                web.Encoding = System.Text.Encoding.UTF8;
                web.Headers[System.Net.HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                var json = web.DownloadString($"https://www.mxnzp.com/api/qrcode/create/single?content={codeContent}&size={size}&type=1");
                var vcode = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
                if (vcode.code != 1)
                    throw new Exception(json);
                return vcode.data.qrCodeBase64;
            }
        }
        /// <summary>
        /// 获取二维码码图片URL地址
        /// </summary>
        /// <param name="codeContent">验证码</param>
        /// <param name="size">尺寸（默认500）</param>
        /// <returns>URL地址</returns>
        public static string GetUrlAddress(string codeContent, int size = 500)
        {
            if (string.IsNullOrWhiteSpace(codeContent))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(codeContent));
            }
            if (size < 100 || size > 1000)
            {
                throw new ArgumentException("Value:100-1000", nameof(size));
            }
            using (var web = new System.Net.WebClient())
            {
                web.Encoding = System.Text.Encoding.UTF8;
                web.Headers[System.Net.HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                var json = web.DownloadString($"https://www.mxnzp.com/api/qrcode/create/single?content={codeContent}&size={size}&type=0");
                var vcode = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
                if (vcode.code != 1)
                    throw new Exception(json);
                return vcode.data.qrCodeUrl;
            }
        }
        /// <summary>
        /// 生成二维码图片（PNG）
        /// </summary>
        /// <param name="fileName">存储图片文件的完整路径</param>
        /// <param name="codeContent">条码内容</param>
        /// <param name="moduleSize">1=33*33;2=66*66;3=99*99;4=132*132;5=165*165(默认2)</param>
        public static void SaveAs(string fileName, string codeContent, int moduleSize = 2)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);

            QrCode qrCode = new QrCode();
            qrEncoder.TryEncode(codeContent, out qrCode);

            Renderer renderer = new Renderer(5, Brushes.Black, Brushes.White);
            renderer.CreateImageFile(qrCode.Matrix, fileName, ImageFormat.Png);
        }
        /// <summary>
        /// 
        /// </summary>
        public class Data
        {
            /// <summary>
            /// 
            /// </summary>
            public string qrCodeUrl { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string content { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string qrCodeBase64 { get; set; }
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
