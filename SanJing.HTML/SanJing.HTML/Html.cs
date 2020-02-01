using MariGold.HtmlParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SanJing.HTML
{
    /// <summary>
    /// HTML解析
    /// </summary>
    public static class Html
    {
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <returns></returns>
        public static HtmlTag[] GetHtmlTags(string htmlCode)
        {
            HtmlParser parser = new HtmlTextParser(htmlCode);
            List<HtmlTag> result = new List<HtmlTag>();
            while (parser.Traverse())
            {
                IHtmlNode htmlNode = parser.Current;
                GetHtmlTags(htmlNode, result);
            }
            return result.ToArray();
        }

        /// <summary>
        /// 获取指定标签
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <param name="tagName">标签名（小写）</param>
        /// <returns></returns>
        public static HtmlTag[] GetHtmlTags(string htmlCode, string tagName)
        {
            return GetHtmlTags(htmlCode).Where(q => q.Name == tagName).ToArray();
        }

        /// <summary>
        /// 通过id获取标签
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <param name="id">区分大小写</param>
        /// <returns></returns>
        public static HtmlTag GetHtmlTagById(string htmlCode, string id)
        {
            return GetHtmlTags(htmlCode).Where(q => q.Attributes.ContainsKey("id") && q.Attributes["id"] == id).FirstOrDefault();
        }
        /// <summary>
        /// 通过class获取标签
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <param name="class">区分大小写</param>
        /// <returns></returns>
        public static HtmlTag[] GetHtmlTagsByClass(string htmlCode, string @class)
        {
            return GetHtmlTags(htmlCode).Where(q => q.Attributes.ContainsKey("class"))
                .Where(q => q.Attributes["class"] == @class || q.Attributes["class"].StartsWith($"{@class} ") ||
                q.Attributes["class"].EndsWith($" {@class}") || q.Attributes["class"].Contains($" {@class} "))
                .ToArray();
        }

        /// <summary>
        /// 获取纯文本
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <returns></returns>
        public static string GetHtmlText(string htmlCode)
        {
            return string.Join(string.Empty, GetHtmlTags(htmlCode).Where(q => q.IsText).Select(q => q.InnerText));
        }

        /// <summary>
        /// 获取指定标签的指定属性值
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <param name="attrName">属性名（小写）</param>
        /// <param name="tagName">标签名（小写）</param>
        /// <returns></returns>
        public static string[] GetAttrValues(string htmlCode, string attrName, string tagName)
        {
            return GetHtmlTags(htmlCode, tagName).Where(q => q.Attributes.ContainsKey(attrName))
                .Select(q => q.Attributes[attrName]).ToArray();
        }

        /// <summary>
        /// 根据标签名获取节点
        /// </summary>
        /// <param name="htmlNode">节点</param>
        /// <param name="result"></param>
        private static void GetHtmlTags(IHtmlNode htmlNode, List<HtmlTag> result)
        {
            result.Add(new HtmlTag(htmlNode));

            if (htmlNode.HasChildren)
            {
                if (htmlNode.Tag.ToLower() == "script")
                    return;
                if (htmlNode.Tag.ToLower() == "style")
                    return;
                if (htmlNode.Tag.ToLower() == "iframe")
                    return;
                foreach (var childrenNode in htmlNode.Children)
                {
                    GetHtmlTags(childrenNode, result);
                }
            }
        }
    }
}
