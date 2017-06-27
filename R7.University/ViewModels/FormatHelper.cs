//
//  FormatHelper.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;

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

        public static string FormatTimeToLearn (
            int timeToLearn, 
            TimeToLearnUnit timeToLearnUnit,
            string keyBase,
            string resourceFile)
        {
            var culture = CultureInfo.CurrentUICulture;

            if (timeToLearnUnit == TimeToLearnUnit.Hours) {
                var hoursPlural = CultureHelper.GetPlural (timeToLearn, culture) + 1;
                var hoursKey = keyBase + "Hours" + hoursPlural + ".Format";
                return string.Format (Localization.GetString (hoursKey, resourceFile), timeToLearn);
            }
           
            var years = timeToLearn / 12;
            var months = timeToLearn % 12;

            var yearsPlural = CultureHelper.GetPlural (years, culture) + 1;
            var monthsPlural = CultureHelper.GetPlural (months, culture) + 1;

            var yearsKey = keyBase + "Years" + yearsPlural + ".Format";
            var monthsKey = keyBase + "Months" + monthsPlural + ".Format";

            if (months == 0) {
                return string.Format (Localization.GetString (yearsKey, resourceFile), years);
            }

            if (years == 0) {
                return string.Format (Localization.GetString (monthsKey, resourceFile), months);
            }

            return string.Format (Localization.GetString (yearsKey, resourceFile), years)
                + " " + string.Format (Localization.GetString (monthsKey, resourceFile), months);
        }

        public static string FormatEduProgramTitle (string code, string title)
        {
            return TextUtils.FormatList (" ", code, title);
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
            string defaultTitle, bool preferDocumentTitle, DocumentGroupPlacement groupPlacement, int tabId, int moduleId, string microdata, DateTime now)
        {
            var title = (preferDocumentTitle && !string.IsNullOrWhiteSpace (documentTitle)) 
                ? ((groupPlacement == DocumentGroupPlacement.InTitle)
                    ? TextUtils.FormatList (": ", document.Group, documentTitle)
                    : documentTitle)
                : ((groupPlacement == DocumentGroupPlacement.InTitle && !string.IsNullOrWhiteSpace (document.Group))
                    ? document.Group
                    : defaultTitle);
              
            if (!string.IsNullOrWhiteSpace (document.Url)) {
                var linkMarkup = "<a href=\"" + UniversityUrlHelper.LinkClickIdnHack (document.Url, tabId, moduleId) + "\" "
                + TextUtils.FormatList (" ", !document.IsPublished (now) ? "class=\"u8y-not-published-element\"" : string.Empty, microdata)
                + " target=\"_blank\">" + title + "</a>";
                
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

        public static string FormatDocumentLinks (IEnumerable<IDocument> documents, ViewModelContext context, string itemTemplate, string listTemplateOne, string listTemplateMany, string microdata, DocumentGroupPlacement groupPlacement, GetDocumentTitle getDocumentTitle = null)
        {
            var markupBuilder = new StringBuilder ();
            var count = 0;
            foreach (var document in documents) {
                var linkMarkup = document.FormatDocumentLink_WithMicrodata (
                    (getDocumentTitle == null)? document.Title : getDocumentTitle (document),
                    Localization.GetString ("LinkOpen.Text", context.LocalResourceFile),
                    true,
                    groupPlacement,
                    context.Module.TabId,
                    context.Module.ModuleId,
                    microdata,
                    HttpContext.Current.Timestamp
                );

                if (!string.IsNullOrEmpty (linkMarkup)) {
                    markupBuilder.Append (string.Format (itemTemplate, linkMarkup));
                    count++;
                }
            }

            var markup = markupBuilder.ToString ();
            if (!string.IsNullOrEmpty (markup)) {
                return string.Format ((count == 1)? listTemplateOne : listTemplateMany, markup);
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

        public static string FormatYears (int? yearBegin, int? yearEnd, string atTheMoment)
        {
            if (yearBegin != null && yearEnd == null)
                return yearBegin.ToString (); 

            if (yearBegin == null && yearEnd != null) {
                if (yearEnd.Value != 0)
                    return "? - " + yearEnd; 
            }

            if (yearBegin != null && yearEnd != null) {
                if (yearEnd.Value != 0)
                    return string.Format ("{0} - {1}", yearBegin, yearEnd);

                return yearBegin + " - " + atTheMoment;
            }

            return string.Empty;
        }
    }
}

