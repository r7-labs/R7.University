using System.Text.RegularExpressions;

namespace R7.University.Core.Markdown
{
    public static class MarkdownHelper
    {
        /// <summary>
        /// Preprocesses the HTML, preparing it to convertion into plain text with some basic Markdown formatting.
        /// </summary>
        /// <returns>The preprocessed HTML string.</returns>
        /// <param name="html">Html.</param>
        public static string PreprocessHtml (string html)
        {
            // bold
            html = html.Replace ("<strong>", "**");
            html = html.Replace ("</strong>", "**");

            // italic
            html = html.Replace ("<em>", "*");
            html = html.Replace ("</em>", "*");

            // unordered lists
            html = html.Replace ("<li>", "- ");
            html = Regex.Replace (html, "<li[^>]*>", "- ");

            // links
            html = Regex.Replace (html, "<a\\s+.*href=\"([^\"]*)\"[^>]*>([^<]*)</a>", "[$2]($1)");

            return html;
        }
    }
}
