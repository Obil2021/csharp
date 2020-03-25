using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.GPS
{
    /// <summary>
    /// 墨卡托
    /// </summary>
    public class Mercator
    {
        /// <summary>
        /// 地球周长
        /// </summary>
        public const double PERIMETER = 20037508.34;
        /// <summary>
        /// X轴
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y轴
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// 转GPS坐标
        /// </summary>
        /// <returns></returns>
        public GPS ToGPS()
        {
            var gps = new GPS();
            gps.Lng = X / PERIMETER * 180;
            gps.Lat = Y / PERIMETER * 180;
            gps.Lat = 180 / Math.PI * (2 * Math.Atan(Math.Exp(gps.Lat * Math.PI / 180)) - Math.PI / 2);
            return gps;
        }
    }
}
