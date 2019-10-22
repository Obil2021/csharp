using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace SanJing.GraphicCode
{
    /// <summary>
    /// 二维码
    /// </summary>
    public class QRCode
    {
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
    }
}
