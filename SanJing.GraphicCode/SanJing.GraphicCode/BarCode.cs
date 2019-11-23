using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace SanJing.GraphicCode
{
    /// <summary>
    /// 一维码
    /// </summary>
    public class BarCode
    {
        /// <summary>
        /// 获取一维条码图片的base64字符串（注：Base64字符串前面默认添加了“data:image/jpg;base64,”，取值的时候请根据需要对这个内容进行处理）
        /// https://github.com/MZCretin/RollToolsApi#%E7%94%9F%E6%88%90%E6%8C%87%E5%AE%9A%E6%9D%A1%E5%BD%A2%E7%A0%81
        /// </summary>
        /// <param name="codeContent">条码类容</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>base64字符串</returns>
        public static string GetBase64String(string codeContent, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(codeContent))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(codeContent));
            }
            if (width < 100 || width > 1000)
            {
                throw new ArgumentException("Value:100-1000", nameof(width));
            }
            if (height < 50 || height > 500)
            {
                throw new ArgumentException("Value:50-500", nameof(height));
            }
            using (var web = new System.Net.WebClient())
            {
                web.Encoding = System.Text.Encoding.UTF8;
                web.Headers[System.Net.HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                var json = web.DownloadString($"https://www.mxnzp.com/api/barcode/create?content={codeContent}&width={width}&height={height}&type=1");
                var vcode = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
                if (vcode.code != 1)
                    throw new Exception(json);
                return vcode.data.barCodeBase64;
            }
        }
        /// <summary>
        /// 获取一维条码图片URL地址
        /// https://github.com/MZCretin/RollToolsApi#%E7%94%9F%E6%88%90%E6%8C%87%E5%AE%9A%E6%9D%A1%E5%BD%A2%E7%A0%81
        /// </summary>
        /// <param name="codeContent">验证码</param>
        /// <param name="width">宽度(100-1000)</param>
        /// <param name="height">高度(50-500)</param>
        /// <returns>URL地址</returns>
        public static string GetUrlAddress(string codeContent, int width, int height)
        {
            if (string.IsNullOrWhiteSpace(codeContent))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(codeContent));
            }
            if (width < 100 || width > 1000)
            {
                throw new ArgumentException("Value:100-1000", nameof(width));
            }
            if (height < 50 || height > 500)
            {
                throw new ArgumentException("Value:50-500", nameof(height));
            }
            using (var web = new System.Net.WebClient())
            {
                web.Encoding = System.Text.Encoding.UTF8;
                web.Headers[System.Net.HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36";
                var json = web.DownloadString($"https://www.mxnzp.com/api/barcode/create?content={codeContent}&width={width}&height={height}&type=0");
                var vcode = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(json);
                if (vcode.code != 1)
                    throw new Exception(json);
                return vcode.data.barCodeUrl;
            }
        }

        /// <summary>
        /// 生成一维码（CODE128）图片（PNG）
        /// </summary>
        /// <param name="fileName">存储图片文件的完整路径</param>
        /// <param name="codeNumber">条码内容</param>
        /// <param name="includeLabel">条码上是否显示条码内容（默认显示）</param>
        /// <param name="width">宽度（默认250）</param>
        /// <param name="height">高度（默认100）</param>
        public static void SaveAsCODE128(string fileName, string codeNumber, bool includeLabel = true, int width = 250, int height = 100)
        {
            using (var barcode = new Barcode()
            {
                IncludeLabel = includeLabel,
                Alignment = AlignmentPositions.CENTER,
                Width = width,
                Height = height,
                RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                BackColor = Color.White,
                ForeColor = Color.Black,
            })
            {
                barcode.Encode(TYPE.CODE128, codeNumber).Save(fileName, ImageFormat.Png);
            }
        }
        /// <summary>
        /// 生成一维码（CODE39）图片（PNG）
        /// </summary>
        /// <param name="fileName">存储图片文件的完整路径</param>
        /// <param name="codeNumber">条码内容</param>
        /// <param name="includeLabel">条码上是否显示条码内容（默认显示）</param>
        /// <param name="width">宽度（默认250）</param>
        /// <param name="height">高度（默认100）</param>
        public static void SaveAsCODE39(string fileName, string codeNumber, bool includeLabel = true, int width = 250, int height = 100)
        {
            using (var barcode = new Barcode()
            {
                IncludeLabel = includeLabel,
                Alignment = AlignmentPositions.CENTER,
                Width = width,
                Height = height,
                RotateFlipType = RotateFlipType.RotateNoneFlipNone,
                BackColor = Color.White,
                ForeColor = Color.Black,
            })
            {
                barcode.Encode(TYPE.CODE39, codeNumber).Save(fileName, ImageFormat.Png);
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
            public string barCodeUrl { get; set; }
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
            public string barCodeBase64 { get; set; }
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
