//
//  Utils.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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

using System.Web.UI.WebControls;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace R7.University.Utilities
{
    public static class Utils
    {
        /// <summary>
        /// Gets the display name of specified user.
        /// </summary>
        /// <returns>The user display name.</returns>
        /// <param name="userId">User identifier.</param>
        /// <param name="defName">Default user display name. Pass Null.NullInteger string to use with ModuleAuditControl.</param>
        public static string GetUserDisplayName (int userId, string defName)
        {
            var portalId = PortalController.Instance.GetCurrentPortalSettings ().PortalId;
            var user = UserController.GetUserById (portalId, userId);
	
            return (user != null) ? user.DisplayName : defName;
        }

        public static void ExpandToLevel (Telerik.Web.UI.RadTreeView tree, int maxLevel)
        {
            foreach (Telerik.Web.UI.RadTreeNode node in tree.Nodes)
                ExpandNodeToLevel (node, 0, maxLevel);
        }

        private static void ExpandNodeToLevel (Telerik.Web.UI.RadTreeNode node, int level, int maxLevel)
        {
            if (level < maxLevel) {
                node.Expanded = true;
                if (node.Nodes != null)
                    foreach (Telerik.Web.UI.RadTreeNode child in node.Nodes)
                        ExpandNodeToLevel (child, level + 1, maxLevel);
            }
        }

        public static int GetViewIndexByID (MultiView mview, string viewName)
        {
            for (var i = 0; i < mview.Views.Count; i++)
                if (mview.Views [i].ID == viewName)
                    return i;

            return -1;
        }
    }
    // class
}
// namespace
	