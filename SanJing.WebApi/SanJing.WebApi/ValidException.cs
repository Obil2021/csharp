using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// 数据验证
    /// </summary>
    public class ValidException:Exception
    {
        /// <summary>
        /// 实例化
        /// </summary>
        /// <param name="message"></param>
        public ValidException(string message) : base(message) { }
    }
}
