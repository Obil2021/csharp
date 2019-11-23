using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace SanJing.GraphicCode
{
    /// <summary>
    /// BASE64转图片
    /// </summary>
    public class ImageBase64
    {
        /// <summary>
        /// 保存为JPG图面
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="fullFileName"></param>
        public static void SaveAsJpeg(string base64, string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(base64))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(base64));
            }

            if (string.IsNullOrWhiteSpace(fullFileName))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(fullFileName));
            }

            if (Path.GetExtension(fullFileName).ToLower() != ".jpg")
            {
                throw new ArgumentException("Not the .jpg", nameof(fullFileName));
            }
            base64 = base64.Split(',')[1];
            byte[] bytes = Convert.FromBase64String(base64);
            using (Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                image.Save(fullFileName, ImageFormat.Jpeg);
            }
        }
        /// <summary>
        /// 保存为PNG图面
        /// </summary>
        /// <param name="base64"></param>
        /// <param name="fullFileName"></param>
        public static void SaveAsPng(string base64, string fullFileName)
        {
            if (string.IsNullOrWhiteSpace(base64))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(base64));
            }

            if (string.IsNullOrWhiteSpace(fullFileName))
            {
                throw new ArgumentException("IsNullOrWhiteSpace", nameof(fullFileName));
            }

            if (Path.GetExtension(fullFileName).ToLower() != ".png")
            {
                throw new ArgumentException("Not the .png", nameof(fullFileName));
            }
            base64 = base64.Split(',')[1];
            byte[] bytes = Convert.FromBase64String(base64);
            using (Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                image.Save(fullFileName, ImageFormat.Png);
            }
        }
    }
}
