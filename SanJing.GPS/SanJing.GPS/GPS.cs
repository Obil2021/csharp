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

        /// <summary>
        /// 坐标距离计算
        /// </summary>
        /// <param name="gps">端点坐标</param>
        /// <returns>直线距离（米）</returns>
        protected double distance(GPS gps)
        {
            double radLatbegin = Rad(Lat);
            double radLngbegin = Rad(Lng);
            double radLatend = Rad(gps.Lat);
            double radLngend = Rad(gps.Lng);
            double a = radLatbegin - radLatend;
            double b = radLngbegin - radLngend;
            double c = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLatbegin) * Math.Cos(radLatend) * Math.Pow(Math.Sin(b / 2), 2)));
            return c * RADIUS;
        }
        /// <summary>
        /// 经纬度转化成弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        private double Rad(double d)
        {
            return d * Math.PI / 180d;
        }
        /// <summary>
        /// 转墨卡托投影
        /// </summary>
        /// <returns></returns>
        public Mercator ToMercator()
        {
            Mercator mercator = new Mercator();
            mercator.X = Lng * Mercator.PERIMETER / 180;
            mercator.Y = Math.Log(Math.Tan((90 + Lat) * Math.PI / 360)) / (Math.PI / 180);
            mercator.Y = mercator.Y * Mercator.PERIMETER / 180;
            return mercator;
        }
    }
}
