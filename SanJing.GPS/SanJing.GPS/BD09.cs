using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.GPS
{
    /// <summary>
    /// 百度坐标系（百度地图）
    /// </summary>
    public class BD09 : GPS
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public BD09() : base() { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public BD09(double lat, double lng) : base(lat, lng) { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="gps"></param>
        public BD09(GPS gps) : base(gps.Lat, gps.Lng) { }
        /// <summary>
        /// 坐标距离计算
        /// </summary>
        /// <param name="bd09">端点坐标</param>
        /// <returns>直线距离（米）</returns>
        public double Distance(BD09 bd09)
        {
            if (bd09 == null)
            {
                throw new ArgumentNullException(nameof(bd09));
            }

            return distance(bd09);
        }
        /// <summary>
        /// 转换为火星坐标系
        /// </summary>
        /// <returns></returns>
        public GCJ02 ToGCJ02()
        {
            double x = Lng - 0.0065, y = Lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * Math.PI);
            double theta = Math.Atan2(y, x) - (0.000003 * Math.Cos(x * Math.PI));
            double lng = z * Math.Cos(theta);
            double lat = z * Math.Sin(theta);

            return new GCJ02(lat, lng);
        }
    }
}
