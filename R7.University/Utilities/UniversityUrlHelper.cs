//
//  UniversityUrlHelper.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2020 Roman M. Yagodin
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
using System.Linq;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Modules;

namespace R7.University.Utilities
{
    public static class UniversityUrlHelper
    {
        public static string FullUrl (string url)
        {
            return Globals.AddHTTP (PortalSettings.Current.PortalAlias.HTTPAlias + url);
        }

        /// <summary>
        /// Temp workaround for LinkClick and internationalized domain names (IDN) issue:
        /// https://dnntracker.atlassian.net/browse/DNN-7919
        /// </summary>
        /// <returns>Return raw (untrackable) URL for external URLs.</returns>
        private static string LinkClickIdnHack (string url, int tabId, int moduleId)
        {
            var urlType = Globals.GetURLType (url);
            if (urlType == TabType.Url) {
                return url;
            }
            return Globals.LinkClick (url, tabId, moduleId);
        }

        public static string LinkClick (string url, int tabId, int moduleId)
        {
            return LinkClickIdnHack (url, tabId, moduleId);
        }

        public static string LinkClickFile (int fileId, int tabId, int moduleId)
        {
            return LinkClick ("fileid=" + fileId, tabId, moduleId);
        }

        /// <summary>
        /// Formats the URL by DNN rules.
        /// </summary>
        /// <returns>Formatted URL.</returns>
        /// <param name="module">A module reference.</param>
        /// <param name="link">A link value. May be TabID, FileID=something or in other valid forms.</param>
        /// <param name="trackClicks">If set to <c>true</c> then track clicks.</param>
        public static string FormatURL (IModuleControl module, string link, bool trackClicks)
        {
            return Globals.LinkClick (link, module.ModuleContext.TabId, module.ModuleContext.ModuleId, trackClicks);
        }

        // TODO: Move to the base library
        public static string FormatCrossPortalTabUrl (IModuleControl module, int tabId, bool trackClicks)
        {
            try {
                // get tab info by tabId
                var tab = new TabController ().GetTab (tabId, Null.NullInteger, false);

                // check if this tab belongs to another portal
                if (tab.PortalID != module.ModuleContext.PortalId) {
                    // get portal alias, primary first (we don't know exactly,
                    // which portal aliases are globally-available, and which are not)
                    var portalAlias = PortalAliasController.Instance.GetPortalAliasesByPortalId (tab.PortalID)
                        .OrderBy (pa => !pa.IsPrimary).First ();

                    // target portal URL (let target portal use right protocol and do URL rewriting)
                    return "http://" + portalAlias.HTTPAlias + (trackClicks ?
                        string.Format ("/LinkClick.aspx?link={0}&tabid={1}", tabId, module.ModuleContext.TabId) :
                        string.Format ("/Default.aspx?tabid={0}", tabId));
                }

                // tab is on same portal
                return FormatURL (module, tabId.ToString (), trackClicks);
            }
            catch {
                return string.Empty;
            }
        }

        // TODO: Move to the base library
        /// <summary>
        /// Temp workaround for issue with IE and Unicode characters in EditURL-generated URL:
        /// https://dnntracker.atlassian.net/browse/DNN-9280
        /// </summary>
        /// <returns>The raw edit URL.</returns>
        /// <param name="module">Module control.</param>
        /// <param name="request">HTTP request.</param>
        /// <param name="keyName">Key name.</param>
        /// <param name="keyValue">Key value.</param>
        /// <param name="controlKey">Control key.</param>
        public static string IESafeEditUrl (IModuleControl module, HttpRequest request, string keyName, string keyValue, string controlKey)
        {
            if (PortalSettings.Current.EnablePopUps) {
                // for any IE browser except Edge, return non-popup edit URL
                if (!request.UserAgent.Contains ("Edge")) {
                    var browserName = request.Browser.Browser.ToUpperInvariant ();
                    if (browserName.StartsWith ("IE", StringComparison.Ordinal)
                        || browserName.Contains ("MSIE")
                        || browserName == "INTERNETEXPLORER") {
                        return Globals.NavigateURL (controlKey, keyName, keyValue,
                                                    "mid", module.ModuleContext.ModuleId.ToString ());
                    }
                }
            }

            // popups disabled, it's safe to use default implementation
            return module.ModuleContext.EditUrl (keyName, keyValue, controlKey);
        }

        public static string FormatNiceDocumentUrl (string url, int moduleId, int tabId, int portalId, string resourceFile)
        {
            var label = string.Empty;
            var title = string.Empty;

            if (Globals.GetURLType (url) == TabType.File) {
                var file = FileManager.Instance.GetFile (int.Parse (url.ToUpperInvariant ().Replace ("FILEID=", "")));
                if (file != null) {
                label = file.FileName;
                    title = file.RelativePath;
                }
                else {
                    label = Localization.GetString ("FileNotFound.Text", resourceFile);
                }
            }
            else if (Globals.GetURLType (url) == TabType.Tab) {
                var tab = TabController.Instance.GetTab (int.Parse (url), portalId);
                if (tab != null) {
                    label = Localization.GetString ("Page.Text", resourceFile) + " " + tab.LocalizedTabName;
                    title = tab.TabPath.Replace ("//", "/");
                }
                else {
                    label = Localization.GetString ("PageNotFound.Text", resourceFile);
                }
            }
            else {
                label = HttpUtility.HtmlEncode (HtmlUtils.Shorten (url, 32, "…"));
                title = HttpUtility.HtmlAttributeEncode (url);
            }

            return $"<a href={LinkClickIdnHack (url, tabId, moduleId)} target=\"_blank\" title=\"{title}\">{label}</a>";
        }
    }
}

