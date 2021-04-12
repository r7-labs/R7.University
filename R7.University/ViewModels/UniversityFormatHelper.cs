using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Localization;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.ViewModels
{
    // TODO: Implement model-specific formatting method as extensions
    public static class UniversityFormatHelper
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

        public static string FormatTitleWithShortTitle (string title, string shortTitle)
        {
            return !string.IsNullOrWhiteSpace (shortTitle) ? $"{title} ({shortTitle})" : title;
        }

        public static string FormatTimeToLearnMonths (int totalMonths, string keyBase, string resourceFile)
        {
            var years = totalMonths / 12;
            var months = totalMonths % 12;

            var timeBuilder = new StringBuilder ();

            if (years > 0) {
                var yearsKey = keyBase + "Years" + GetPlural (years) + ".Format";
                timeBuilder.AppendFormat (" " + Localization.GetString (yearsKey, resourceFile), years);
            }

            if (months > 0) {
                var monthsKey = keyBase + "Months" + GetPlural (months) + ".Format";
                timeBuilder.AppendFormat (" " + Localization.GetString (monthsKey, resourceFile), months);
            }

            return timeBuilder.ToString ().TrimStart ();
        }

        public static string FormatTimeToLearnHours (int hours, string keyBase, string resourceFile)
        {
            if (hours > 0) {
                var hoursKey = keyBase + GetPlural (hours) + ".Format";
                return string.Format (Localization.GetString (hoursKey, resourceFile), hours);
            }

            return string.Empty;
        }

        static int GetPlural (int value)
        {
            return CultureHelper.GetPluralIndex (value, CultureInfo.CurrentCulture) + 1;
        }

        public static string FormatTimeToLearn (int totalMonths, int hours, TimeToLearnDisplayMode displayMode, string keyBase, string resourceFile)
        {
            var timeBuilder = new StringBuilder ();

            if (displayMode == TimeToLearnDisplayMode.YearsMonths || displayMode == TimeToLearnDisplayMode.Both) {
                timeBuilder.Append (FormatTimeToLearnMonths (totalMonths, keyBase, resourceFile));
            }

            if (hours > 0) {
                if (displayMode == TimeToLearnDisplayMode.Both && totalMonths > 0) {
                    timeBuilder.Append (" (");
                }
                if (displayMode == TimeToLearnDisplayMode.Hours || displayMode == TimeToLearnDisplayMode.Both) {
                    timeBuilder.Append (FormatTimeToLearnHours (hours, keyBase + "Hours", resourceFile));
                }
                if (displayMode == TimeToLearnDisplayMode.Both && totalMonths > 0) {
                    timeBuilder.Append (")");
                }
            }

            return timeBuilder.ToString ();
        }

        public static string FormatEduProgramTitle (string code, string title)
        {
            return FormatHelper.JoinNotNullOrEmpty (" ", code, title);
        }

        public static string FormatEduProfileTitle (string title,
            string profileCode, string profileTitle)
        {
            var profileString = FormatHelper.JoinNotNullOrEmpty (" ", profileCode, profileTitle);

            var profileStringInBrackets =
                !string.IsNullOrWhiteSpace (profileString) ? "(" + profileString + ")" : string.Empty;

            return FormatHelper.JoinNotNullOrEmpty (" ", title, profileStringInBrackets);
        }

        public static string FormatEduProfileTitle (string code, string title,
            string profileCode, string profileTitle)
        {
            var profileString = FormatHelper.JoinNotNullOrEmpty (" ", profileCode, profileTitle);

            var profileStringInBrackets =
                !string.IsNullOrWhiteSpace (profileString) ? "(" + profileString + ")" : string.Empty;

            return FormatHelper.JoinNotNullOrEmpty (" ", code, title, profileStringInBrackets);
        }

        public static string FormatEduProfilePartialTitle (string profileCode, string profileTitle)
        {
            return FormatHelper.JoinNotNullOrEmpty (profileCode, profileTitle);
        }

        public static string FormatEduProfilePartialTitle (string profileCode, string profileTitle, string eduLevelTitle)
        {
            return FormatHelper.JoinNotNullOrEmpty (profileCode, profileTitle) + ": " + eduLevelTitle;
        }

        public static string FormatDocumentLink_WithMicrodata (this IDocument document, string documentTitle,
            string defaultTitle, bool preferDocumentTitle, DocumentGroupPlacement groupPlacement, int tabId, int moduleId, string microdata, DateTime now)
        {
            var title = (preferDocumentTitle && !string.IsNullOrWhiteSpace (documentTitle))
                ? ((groupPlacement == DocumentGroupPlacement.InTitle)
                   ? FormatHelper.JoinNotNullOrEmpty (": ", document.Group, documentTitle)
                    : documentTitle)
                : ((groupPlacement == DocumentGroupPlacement.InTitle && !string.IsNullOrWhiteSpace (document.Group))
                    ? document.Group
                    : defaultTitle);

            if (!string.IsNullOrWhiteSpace (document.Url)) {
                var linkMarkup = "<a href=\"" + UniversityUrlHelper.LinkClick (document.Url, tabId, moduleId) + "\" "
                                 + FormatHelper.JoinNotNullOrEmpty (" ", !document.IsPublished (now) ? "class=\"u8y-not-published-element\"" : string.Empty, microdata)
                                 + " target=\"_blank\">" + title + "</a>";

                if (groupPlacement == DocumentGroupPlacement.BeforeTitle) {
                    return FormatHelper.JoinNotNullOrEmpty (": ", document.Group, linkMarkup);
                }

                if (groupPlacement == DocumentGroupPlacement.AfterTitle) {
                    return FormatHelper.JoinNotNullOrEmpty (": ", linkMarkup, document.Group);
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
            return FormatHelper.JoinNotNullOrEmpty (" ", lastName, firstName, otherName);
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

        public static string FormatWebSiteLabel (string websiteUrl, string websiteLabel)
        {
            return (!string.IsNullOrWhiteSpace (websiteLabel)) ? websiteLabel :
                websiteUrl.Contains ("://") ? websiteUrl.Remove (0, websiteUrl.IndexOf ("://") + 3) : websiteUrl;
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

        public static string FormatEduProgramDivisionLink (IDivision division, string role = null, bool isPublished = true) {
            if (division != null) {
                var classAttr = isPublished ? string.Empty : " class=\"u8y-not-published-element\"";
                var roleLabel = !string.IsNullOrEmpty (role) ? role + ": " : string.Empty;
                if (!string.IsNullOrWhiteSpace (division.HomePage)) {
                    var url = Globals.NavigateURL (int.Parse (division.HomePage));
                    return $"<span{classAttr}>{roleLabel}<a href=\"{url}\" target=\"_blank\">{division.Title}</a></span>";
                }
                return $"<span{classAttr}>{roleLabel}{division.Title}</span>";
            }
            return string.Empty;
        }

        public static string ValueOrDash<T> (T? value) where T : struct
        {
            return value != null ? value.ToString () : "-";
        }

        public static string ValueOrDash<T> (T? value, Func<T, string> toString) where T : struct
        {
            return value != null ? toString (value.Value) : "-";
        }

        // TODO: Move to the base library
        public static string RemoveTrailingZeroes (string decimalStr)
        {
            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (decimalStr.Contains (decimalSeparator)) {
                decimalStr = decimalStr.TrimEnd ('0');
                if (decimalStr.EndsWith (decimalSeparator, StringComparison.CurrentCulture)) {
                    decimalStr = decimalStr.Substring (0, decimalStr.Length - decimalSeparator.Length);
                }
            }

            return decimalStr;
        }

        // TODO: Move to base library?
        public static string FormatNotNullOrEmpty (string format, string text)
        {
            if (!string.IsNullOrEmpty (text)) {
                return string.Format (format, text);
            }

            return string.Empty;
        }
    }
}
