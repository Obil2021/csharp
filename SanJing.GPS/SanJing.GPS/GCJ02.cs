using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.GPS
{
    /// <summary>
    /// 火星坐标系（腾讯地图，高德地图）
    /// </summary>
    public class GCJ02 : GPS
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public GCJ02() : base() { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public GCJ02(double lat, double lng) : base(lat, lng) { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="gps"></param>
        public GCJ02(GPS gps) : base(gps.Lat, gps.Lng) { }
        /// <summary>
        /// 坐标距离计算
        /// </summary>
        /// <param name="gcj02">端点坐标</param>
        /// <returns>直线距离（米）</returns>
        public double Distance(GCJ02 gcj02)
        {
            if (gcj02 == null)
            {
                throw new ArgumentNullException(nameof(gcj02));
            }

            return distance(gcj02);
        }
        /// <summary>
        /// 转换为百度坐标系
        /// </summary>
        /// <returns></returns>
        public BD09 ToBD09()
        {
            double z = Math.Sqrt(Lng * Lng + Lat * Lat) + 0.00002 * Math.Sin(Lat * Math.PI);
            double theta = Math.Atan2(Lat, Lng) + 0.000003 * Math.Cos(Lng * Math.PI);
            double lng = z * Math.Cos(theta) + 0.0065;
            double lat = z * Math.Sin(theta) + 0.006;

            return new BD09(lat, lng);
        }
    }
}
