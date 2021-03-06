﻿using HtmlAgilityPack;

namespace Haystack.Analysis.CodeCoverage.OpenCover
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
