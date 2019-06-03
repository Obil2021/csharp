using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// 程序员可见的异常
    /// </summary>
    public class UserException : Exception
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="message"></param>
        public UserException(string message) : base(message) { }
    }
}
