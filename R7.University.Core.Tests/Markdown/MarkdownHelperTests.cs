using System.Collections.Generic;
using R7.University.Core.Markdown;
using Xunit;

namespace R7.University.Core.Tests.Markdown
{
    public class MarkdownHelperTests
    {
        [Theory]
        [MemberData (nameof (GetHtml))]
        public void PreprocessHtmlTest (string html, string preprocessedHtml)
        {
            Assert.Equal (preprocessedHtml, MarkdownHelper.PreprocessHtml (html));
        }

        public static IEnumerable<object []> GetHtml ()
        {
            yield return new object [] { "<strong>bold</strong>", "**bold**" };
            yield return new object [] { "<em>italic</em>", "*italic*" };
            yield return new object [] { "<ol><li>list item</li></ol>", "<ol>- list item</li></ol>" };
            yield return new object [] { "<ol><li class=\"css-class\">list item</li></ol>", "<ol>- list item</li></ol>" };
            yield return new object [] { "<a href=\"https://www.foo.com\">foo.com</a>", "[foo.com](https://www.foo.com)" };
            yield return new object [] { "<a href=\"https://www.foo.com\" target=\"_blank\">foo.com</a>", "[foo.com](https://www.foo.com)" };
            yield return new object [] { "<a title=\"a link\" href=\"https://www.foo.com\" target=\"_blank\">foo.com</a>", "[foo.com](https://www.foo.com)" };
        }
    }
}
