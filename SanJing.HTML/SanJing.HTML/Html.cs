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
    public class Html
    {
        /// <summary>
        /// 根据标签名获取节点
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <param name="tagName">标签名</param>
        /// <returns></returns>
        public static IHtmlNode[] GetHtmlNodeByTag(string htmlCode, string tagName)
        {
            HtmlParser parser = new HtmlTextParser(htmlCode);
            List<IHtmlNode> result = new List<IHtmlNode>();
            while (parser.Traverse())
            {
                IHtmlNode htmlNode = parser.Current;
                GetHtmlNodeByTag(htmlNode, tagName, result);
            }
            return result.ToArray(); ;
        }
        /// <summary>
        /// 根据标签名获取节点
        /// </summary>
        /// <param name="htmlNode">节点</param>
        /// <param name="tagName">标签名</param>
        /// <param name="result"></param>
        private static void GetHtmlNodeByTag(IHtmlNode htmlNode, string tagName, List<IHtmlNode> result)
        {
            if (htmlNode.Tag.ToLower() == tagName.ToLower())
            {
                result.Add(htmlNode);
            }
            if (htmlNode.HasChildren)
            {
                if (htmlNode.Tag.ToLower() == "head")
                    return;
                if (htmlNode.Tag.ToLower() == "script")
                    return;
                if (htmlNode.Tag.ToLower() == "style")
                    return;
                if (htmlNode.Tag.ToLower() == "iframe")
                    return;
                foreach (var childrenNode in htmlNode.Children)
                {
                    GetHtmlNodeByTag(childrenNode, tagName, result);
                }
            }
        }
        /// <summary>
        /// 根据属性和值获取节点
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <param name="attrName">属性名</param>
        /// <param name="attrValue">属性值|class用匹配，其他用相等</param>
        /// <returns></returns>
        public static IHtmlNode[] GetHtmlNodeByAttr(string htmlCode, string attrName, string attrValue)
        {
            HtmlParser parser = new HtmlTextParser(htmlCode);
            List<IHtmlNode> result = new List<IHtmlNode>();
            while (parser.Traverse())
            {
                IHtmlNode htmlNode = parser.Current;
                GetHtmlNodeByAttr(htmlNode, attrName, attrValue, result);
            }
            return result.ToArray(); ;
        }
        /// <summary>
        /// 根据属性和值获取节点
        /// </summary>
        /// <param name="htmlNode">节点</param>
        /// <param name="attrName">属性名</param>
        /// <param name="attrValue">属性值|class用匹配，其他用相等</param>
        /// <param name="result"></param>
        private static void GetHtmlNodeByAttr(IHtmlNode htmlNode, string attrName, string attrValue, List<IHtmlNode> result)
        {
            if (htmlNode.Attributes.Where(q => !string.IsNullOrEmpty(q.Key)).Any(q => q.Key.ToLower() == attrName.ToLower() &&
            (q.Value == attrValue ||
            attrName.ToLower() == "class" ? ((q.Value ?? string.Empty).StartsWith(attrValue + " ") || //class用匹配，其他用相等
            (q.Value ?? string.Empty).Contains(" " + attrValue + " ") ||
            (q.Value ?? string.Empty).EndsWith(" " + attrValue)) : false)))
            {
                result.Add(htmlNode);
            }
            else if (htmlNode.HasChildren)
            {
                if (htmlNode.Tag.ToLower() == "head")
                    return;
                if (htmlNode.Tag.ToLower() == "script")
                    return;
                if (htmlNode.Tag.ToLower() == "style")
                    return;
                if (htmlNode.Tag.ToLower() == "iframe")
                    return;
                foreach (var childrenNode in htmlNode.Children)
                {
                    GetHtmlNodeByAttr(childrenNode, attrName, attrValue, result);
                }
            }
        }

        /// <summary>
        /// 获取HTML中所有文本
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <returns></returns>
        public static string GetText(string htmlCode)
        {
            HtmlParser parser = new HtmlTextParser(htmlCode);

            var res = new StringBuilder();
            while (parser.Traverse())
            {
                IHtmlNode htmlNode = parser.Current;
                GetText(htmlNode, res);
            }
            return res.ToString();
        }
        /// <summary>
        /// 获取节点中所有文本
        /// </summary>
        /// <param name="htmlNode">节点</param>
        /// <param name="result"></param>
        public static void GetText(IHtmlNode htmlNode, StringBuilder result)
        {
            if (htmlNode.IsText)
            {
                if (string.IsNullOrWhiteSpace(htmlNode.Html))
                    return;

                result.Append(HttpUtility.HtmlDecode(htmlNode.Html.Trim()));
            }
            else if (htmlNode.HasChildren)
            {
                if (htmlNode.Tag.ToLower() == "head")
                    return;
                if (htmlNode.Tag.ToLower() == "script")
                    return;
                if (htmlNode.Tag.ToLower() == "style")
                    return;
                if (htmlNode.Tag.ToLower() == "iframe")
                    return;
                foreach (var childrenNode in htmlNode.Children)
                {
                    GetText(childrenNode, result);
                }
            }
        }

        /// <summary>
        /// 获取HTML中指定标签名的指定属性
        /// </summary>
        /// <param name="htmlCode">HTML代码</param>
        /// <param name="tagName">标签名</param>
        /// <param name="attrName">属性名</param>
        /// <returns></returns>
        public static string[] GetTagAttr(string htmlCode, string tagName, string attrName)
        {
            HtmlParser parser = new HtmlTextParser(htmlCode);

            var res = new List<string>();
            while (parser.Traverse())
            {
                IHtmlNode htmlNode = parser.Current;
                GetTagAttr(htmlNode, tagName, attrName, res);
            }
            return res.ToArray();
        }
        /// <summary>
        /// 获取节点中指定标签名的指定属性
        /// </summary>
        /// <param name="htmlNode">节点</param>
        /// <param name="tagName">标签名</param>
        /// <param name="attrName">属性名</param>
        /// <param name="result"></param>
        public static void GetTagAttr(IHtmlNode htmlNode, string tagName, string attrName, List<string> result)
        {
            if (htmlNode.IsText) return;

            if (htmlNode.Tag.ToLower() == tagName.ToLower())
            {
                if (htmlNode.Attributes != null && htmlNode.Attributes.Count > 0)
                {
                    foreach (var attr in htmlNode.Attributes)
                    {
                        if (string.IsNullOrEmpty(attr.Key)) continue;

                        if (attr.Key.ToLower() == attrName.ToLower())
                        {
                            result.Add(attr.Value ?? string.Empty);
                        }
                    }
                }
            }
            else if (htmlNode.HasChildren)
            {
                if (htmlNode.Tag.ToLower() == "head")
                    return;
                if (htmlNode.Tag.ToLower() == "script")
                    return;
                if (htmlNode.Tag.ToLower() == "style")
                    return;
                if (htmlNode.Tag.ToLower() == "iframe")
                    return;
                foreach (var childrenNode in htmlNode.Children)
                {
                    GetTagAttr(childrenNode, tagName, attrName, result);
                }
            }
        }
    }
}
