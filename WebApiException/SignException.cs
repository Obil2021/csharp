using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// 验签异常（失败）
    /// </summary>
    public class SignException : Exception
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public SignException() : base("签名错误") { }
    }
}
