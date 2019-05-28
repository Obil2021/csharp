using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.GPS
{
    public class WGS84 : GPS
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public WGS84() : base() { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public WGS84(double lat, double lng) : base(lat, lng) { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="gps"></param>
        public WGS84(GPS gps) : base(gps.Lat, gps.Lng) { }
        /// <summary>
        /// 坐标距离计算
        /// </summary>
        /// <param name="wgs84">端点坐标</param>
        /// <returns>直线距离（米）</returns>
        public double Distance(WGS84 wgs84)
        {
            if (wgs84 == null)
            {
                throw new ArgumentNullException(nameof(wgs84));
            }
            return distance(wgs84);
        }
    }
}
