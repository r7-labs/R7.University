//
//  UrlUtils.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015 Roman M. Yagodin
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
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;

namespace R7.University.Utilities
{
    public static class UrlUtils
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

    }
}

