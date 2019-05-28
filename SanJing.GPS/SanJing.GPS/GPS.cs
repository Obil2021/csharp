using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.GPS
{
    public class GPS
    {
        /// <summary>
        /// 半径
        /// </summary>
        public const double RADIUS = 6378137;
        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        protected static double Rad(double d)
        {
            return d * Math.PI / 180d;
        }

        /// <summary>
        /// 创建一个地球坐标系（谷歌国际）
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static WGS84 NewWGS84(double lat, double lng) { return new WGS84(lat, lng); }
        /// <summary>
        /// 创建一个火星坐标系（腾讯，高德、谷歌中国）
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static GCJ02 NewGCJ02(double lat, double lng) { return new GCJ02(lat, lng); }
        /// <summary>
        /// 创建一个百度坐标系（百度）
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static BD09 NewBD09(double lat, double lng) { return new BD09(lat, lng); }
        /// <summary>
        /// 初始化
        /// </summary>
        public GPS() { }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        public GPS(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }
    }
}
