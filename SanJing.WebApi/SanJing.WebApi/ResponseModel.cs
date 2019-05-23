using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// WebApi返回模型
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// 状态码
        /// 1000表示成功
        /// </summary>
        public virtual int StatuCode { get; set; } = 1000;
        /// <summary>
        /// 状态表述
        /// Success表示成功
        /// </summary>
        public virtual string StatuMsg { get; set; } = "Success";
    }
}
