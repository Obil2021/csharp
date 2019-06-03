using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// WebApi请求模型
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// 随机字符串
        /// </summary>
        [DisplayName("随机字符串")]
        public virtual string NonceString { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        [DisplayName("时间戳")]
        public virtual long TimeStamp { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        [DisplayName("签名")]
        public virtual string Sign { get; set; }
    }
}
