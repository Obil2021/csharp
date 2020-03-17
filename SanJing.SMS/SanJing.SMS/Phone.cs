using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SanJing.SMS
{
    /// <summary>
    /// 手机号
    /// </summary>
    public class Phone
    {
        /// <summary>
        /// 手机号验证【https://tcc.taobao.com/cc/json/mobile_tel_segment.htm?tel=】
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValid(string phone)
        {
            using (var wc = new WebClient())
            {
                wc.Encoding = Encoding.GetEncoding("gbk");

                var res = wc.DownloadString("https://tcc.taobao.com/cc/json/mobile_tel_segment.htm?tel=" + phone);

                return res.Contains(phone);
            }
        }
    }
}
