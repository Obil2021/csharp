using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WxHelper
{
    /// <summary>
    /// 模板消息参数模型
    /// </summary>
    public class WxTempMsg
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public WxTempMsg()
        {
            Color = "#000000";
        }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// 显示颜色（默认值：#000000）
        /// </summary>
        public string Color { get; set; }
    }
}
