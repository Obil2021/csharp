using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// NLog读取器
    /// </summary>
    public sealed class NLogReader
    {
        /// <summary>
        /// 读取今天的日式记录【/logs/yyyy-mm-dd.log】
        /// </summary>
        /// <returns></returns>
        public static string TodayLogHtml()
        {
            return string.Join(Environment.NewLine, TodayLogLines().Select(q => $"<p>{q}</p>"));
        }
        /// <summary>
        /// 读取今天的日式记录【/logs/yyyy-mm-dd.log】
        /// </summary>
        /// <returns></returns>
        public static string TodayLogText()
        {
            return string.Join(Environment.NewLine, TodayLogLines());
        }
        /// <summary>
        /// 读取今天的日式记录【/logs/yyyy-mm-dd.log】
        /// </summary>
        /// <returns></returns>
        private static string[] TodayLogLines()
        {
            string logFileName = $"{AppDomain.CurrentDomain.BaseDirectory}\\logs\\{DateTime.Today.ToString("yyyy-MM-dd")}.log";
            if (File.Exists(logFileName))
            {
                return File.ReadAllLines(logFileName, Encoding.GetEncoding("gb2312"));
            }
            return new string[0];
        }
    }
}
