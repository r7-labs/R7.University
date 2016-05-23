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
using R7.University.ModelExtensions;

namespace R7.University.ViewModels
{
    public static class FormatHelper
    {
        public static string FormatShortTitle (string shortTitle, string title)
        {
            return !string.IsNullOrWhiteSpace (shortTitle) ? shortTitle : title;
        }

        public static string FormatShortTitle (string shortTitle, string title, string titleSuffix)
        {
            var shortTitleWoSuffix = FormatShortTitle (shortTitle, title);
            return !string.IsNullOrWhiteSpace (titleSuffix) ? shortTitleWoSuffix + " " + titleSuffix : shortTitleWoSuffix; 
        }

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

        public static string FormatDocumentLink_WithMicrodata (this IDocument document, string documentTitle,
            string defaultTitle, bool preferDocumentTitle, DocumentGroupPlacement groupPlacement, int tabId, int moduleId, string microdata)
        {
            var title = (preferDocumentTitle && !string.IsNullOrWhiteSpace (documentTitle)) 
                ? ((groupPlacement == DocumentGroupPlacement.InTitle)
                    ? TextUtils.FormatList (": ", document.Group, documentTitle)
                    : documentTitle)
                : ((groupPlacement == DocumentGroupPlacement.InTitle && !string.IsNullOrWhiteSpace (document.Group))
                    ? document.Group
                    : defaultTitle);
              
            if (!string.IsNullOrWhiteSpace (document.Url)) {
                var linkMarkup = "<a href=\""
                + R7.University.Utilities.UrlUtils.LinkClickIdnHack (document.Url, tabId, moduleId)
                + "\" "
                + TextUtils.FormatList (" ",
                    Globals.GetURLType (document.Url) == TabType.Url ? "target=\"_blank\"" : string.Empty,
                    !document.IsPublished () ? "class=\"not-published-document\"" : string.Empty,
                    microdata)
                + ">"
                + title
                + "</a>";
                
                if (groupPlacement == DocumentGroupPlacement.BeforeTitle) {
                    return TextUtils.FormatList (": ", document.Group, linkMarkup);
                }

                if (groupPlacement == DocumentGroupPlacement.AfterTitle) {
                    return TextUtils.FormatList (": ", linkMarkup, document.Group);
                }

                return linkMarkup;
            }

            return string.Empty;
        }

        public static string FullName (string firstName, string lastName, string otherName)
        {
            return TextUtils.FormatList (" ", lastName, firstName, otherName);
        }

        public static string AbbrName (string firstName, string lastName, string otherName)
        {
            if (!string.IsNullOrWhiteSpace (otherName)) {
                return string.Format ("{0} {1}.{2}.", lastName, firstName.Substring (0, 1), otherName.Substring (0, 1)); 
            }

            return string.Format ("{0} {1}.", lastName, firstName.Substring (0, 1));
        }

        public static string FormatWebSiteUrl  (string website)
        {
            return website.Contains ("://") ? website.ToLowerInvariant () : 
                "http://" + website.ToLowerInvariant ();
        }

        public static string FormatWebSiteLabel (string website, string websiteLabel)
        {
            return (!string.IsNullOrWhiteSpace (websiteLabel)) ? websiteLabel : 
                website.Contains ("://") ? website.Remove (0, website.IndexOf ("://") + 3) : website;
        }
    }
}

