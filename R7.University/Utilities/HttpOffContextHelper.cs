//
//  HttpOffContextHelper.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2018 Roman M. Yagodin
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
using DotNetNuke.Entities.Portals;

namespace R7.University.Utilities
{
    // TODO: Move to the base library
    public static class HttpOffContextHelper
    {
        /// <summary>
        /// Gets the portal settings with portal alias info outside HTTP context.
        /// Could be useful along with e.g. Globals.NavigateURL().
        /// </summary>
        /// <returns>The portal settings.</returns>
        /// <param name="portalId">Portal identifier.</param>
        /// <param name="tabId">Tab identifier.</param>
        public static PortalSettings GetPortalSettings (int portalId, int tabId, string cultureCode)
        {
            var portalAliases = PortalAliasController.Instance.GetPortalAliasesByPortalId (portalId);

            var portalAlias = portalAliases.FirstOrDefault (pa => pa.IsPrimary && pa.CultureCode == cultureCode);
            if (portalAlias == null) {
                portalAliases.FirstOrDefault (pa => pa.CultureCode == cultureCode);
                if (portalAlias == null) {
                    portalAliases.FirstOrDefault (pa => pa.IsPrimary);
                    if (portalAlias == null) {
                        portalAliases.FirstOrDefault ();
                    }
                }
            }

            return new PortalSettings (tabId, portalAlias);
        }
    }
}
