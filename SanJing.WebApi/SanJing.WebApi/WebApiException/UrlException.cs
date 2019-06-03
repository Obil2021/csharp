using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// 路由异常
    /// </summary>
    public class UrlException : Exception
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public UrlException() : base("APPNAME 与 URL 不匹配") { }
    }
}
