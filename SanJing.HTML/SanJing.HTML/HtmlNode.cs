using MariGold.HtmlParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SanJing.HTML
{
    /// <summary>
    /// 节点
    /// </summary>
    public class HtmlTag
    {
        /// <summary>
        /// 节点
        /// </summary>
        private IHtmlNode _htmlNode { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="htmlNode"></param>
        public HtmlTag(IHtmlNode htmlNode)
        {
            _htmlNode = htmlNode;

            Attributes = new Dictionary<string, string>();
            foreach (var item in htmlNode.Attributes)
            {
                var key = item.Key.ToLower();

                if (Attributes.ContainsKey(key))
                    continue;

                Attributes.Add(key, item.Value ?? string.Empty);
            }
        }
        /// <summary>
        /// 标签名称（全部为小写）
        /// </summary>
        public string Name { get { return _htmlNode.Tag.ToLower(); } }
        /// <summary>
        /// 是否是TEXT节点
        /// </summary>
        public bool IsText { get { return _htmlNode.IsText; } }
        /// <summary>
        /// TEXT节点的TEXT内容
        /// </summary>
        public string InnerText { get { return IsText ? HttpUtility.HtmlDecode(_htmlNode.Html ?? string.Empty) : string.Empty; } }
        /// <summary>
        /// HTML节点的HTML内容
        /// </summary>
        public string InnerHtml { get { return IsText ? string.Empty : _htmlNode.InnerHtml; } }
        /// <summary>
        /// 所有属性(Key全部为小写,如果重复只取第一个)
        /// </summary>
        public Dictionary<string, string> Attributes { get; private set; }
    }
}
