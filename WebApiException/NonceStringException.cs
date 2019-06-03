using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// 随机串异常（重复）
    /// </summary>
    public class NonceStringException : Exception
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public NonceStringException() : base("重复提交") { }
    }
}
