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
    }
}
