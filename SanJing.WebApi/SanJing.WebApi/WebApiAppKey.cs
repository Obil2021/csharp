using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// APPKEY
    /// </summary>
    public abstract class WebApiAppKey
    {
        /// <summary>
        /// AppKey
        /// </summary>
        public abstract string AppKey { get; }
        /// <summary>
        /// AppName（请与文件夹名称一致【AppControllers：AppName=App】且继承接口所使用的WebApiAppKey也必须一致，否则不能显示也不能访问）
        /// </summary>
        public abstract string AppName { get;}
    }
}
