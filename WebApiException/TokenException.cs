using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// 身份认证异常（失败）
    /// </summary>
    public class TokenException : Exception
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public TokenException() : base("凭据无效") { }
    }
}
