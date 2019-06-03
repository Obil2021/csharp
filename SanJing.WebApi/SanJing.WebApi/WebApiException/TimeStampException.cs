using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// 时间戳异常（超出限定范围）
    /// </summary>
    public class TimeStampException : Exception
    {
        /// <summary>
        /// 实例化
        /// </summary>
        public TimeStampException() : base("时间戳无效") { }
    }
}
