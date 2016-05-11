//
// FormatHelper.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Globalization;
using DotNetNuke.Common;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;

namespace R7.University.ViewModels
{
    public static class FormatHelper
    {
        private static int getPlural (int n, CultureInfo culture)
        {
            // http://localization-guide.readthedocs.org/en/latest/l10n/pluralforms.html

            // TODO: Add more languages here
            if (culture.TwoLetterISOLanguageName == "ru") {   
                // nplurals=3;
                return (n % 10 == 1 && n % 100 != 11 ? 0 : n % 10 >= 2 && n % 10 <= 4 && (n % 100 < 10 || n % 100 >= 20) ? 1 : 2);
            }

            // nplurals=2;
            return (n != 1) ? 1 : 0;
        }

        public static string FormatTimeToLearn (
            int timeToLearn,
            string yearsKeyBase,
            string monthsKeyBase,
            string resourceFile)
        {
            var culture = CultureInfo.CurrentUICulture;

            var years = timeToLearn / 12;
            var months = timeToLearn % 12;

            var yearsPlural = getPlural (years, culture) + 1;
            var monthsPlural = getPlural (months, culture) + 1;

            if (months == 0) {
                return string.Format (Localization.GetString (yearsKeyBase + yearsPlural, resourceFile), years);
            }

            if (years == 0) {
                return string.Format (Localization.GetString (monthsKeyBase + monthsPlural, resourceFile), months);
            }

            return string.Format (Localization.GetString (yearsKeyBase + yearsPlural, resourceFile), years)
            + " " + string.Format (Localization.GetString (monthsKeyBase + monthsPlural, resourceFile), months);
        }

        public static string FormatEduProgramProfileTitle (string title, 
            string profileCode, string profileTitle)
        {
            var profileString = TextUtils.FormatList (" ", profileCode, profileTitle);

            var profileStringInBrackets = 
                !string.IsNullOrWhiteSpace (profileString) ? "(" + profileString + ")" : string.Empty;

            return TextUtils.FormatList (" ", title, profileStringInBrackets);
        }

        public static string FormatEduProgramProfileTitle (string code, string title, 
            string profileCode, string profileTitle)
        {
            var profileString = TextUtils.FormatList (" ", profileCode, profileTitle);

            var profileStringInBrackets = 
                !string.IsNullOrWhiteSpace (profileString) ? "(" + profileString + ")" : string.Empty;

            return TextUtils.FormatList (" ", code, title, profileStringInBrackets);
        }

        public static string FormatLinkWithMicrodata (this IDocument document, 
            string defaultTitle, bool preferOwnTitle, int tabId, int moduleId, string microdata)
        {
            var title = (preferOwnTitle && !string.IsNullOrWhiteSpace (document.Title)) ? document.Title : defaultTitle;
                
            if (!string.IsNullOrWhiteSpace (document.Url)) {
                return "<a href=\""
                + R7.University.Utilities.UrlUtils.LinkClickIdnHack (document.Url, tabId, moduleId)
                + "\" "
                + TextUtils.FormatList (" ",
                    Globals.GetURLType (document.Url) == TabType.Url ? "target=\"_blank\"" : string.Empty,
                    !document.IsPublished () ? "class=\"not-published-document\"" : string.Empty,
                    microdata)
                + ">"
                + title
                + "</a>";
            }

            return string.Empty;
        }
    }
}

