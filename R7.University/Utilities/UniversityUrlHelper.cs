//
//  UniversityUrlHelper.cs
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

using System.Linq;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
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
        public static string LinkClickIdnHack (string url, int tabId, int moduleId)
        {
            switch (Globals.GetURLType (url)) {
                case TabType.Url:
                    return url;
                    
                default:
                    return Globals.LinkClick (url, tabId, moduleId);
            }
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
    }
}

