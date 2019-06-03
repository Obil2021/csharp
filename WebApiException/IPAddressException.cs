using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// IP地址异常（黑名单）
    /// </summary>
    public class IPAddressException : Exception
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public IPAddressException() : base("IP拒绝访问") { }
    }
}
