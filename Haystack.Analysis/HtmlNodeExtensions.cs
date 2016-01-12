using HtmlAgilityPack;

namespace Haystack.Analysis
{
    internal static class HtmlNodeExtensions
    {
        public static HtmlNode NextSibling(this HtmlNode node)
        {
            do
            {
                node = node.NextSibling;
            } while (node.NodeType != HtmlNodeType.Element);
            return node;
        }
    }
}
