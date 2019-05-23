using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.WebApi
{
    /// <summary>
    /// API说明
    /// </summary>
    public class Swagger
    {
        /// <summary>
        /// XML文件路径
        /// </summary>
        /// <param name="name">XML文件名（一般与项目名称相同）</param>
        /// <returns></returns>
        public static string GetXmlCommentsPath(string name)
        {
            return $"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\{name}.xml";
        }
    }
}
