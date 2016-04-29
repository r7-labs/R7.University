//
// Utils.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2016 Roman M. Yagodin
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
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.Modules;

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

        /// <summary>
        /// Formats the URL by DNN rules.
        /// </summary>
        /// <returns>Formatted URL.</returns>
        /// <param name="module">A module reference.</param>
        /// <param name="link">A link value. May be TabID, FileID=something or in other valid forms.</param>
        /// <param name="trackClicks">If set to <c>true</c> then track clicks.</param>
        public static string FormatURL (IModuleControl module, string link, bool trackClicks)
        {
            return Globals.LinkClick 
				(link, module.ModuleContext.TabId, module.ModuleContext.ModuleId, trackClicks);
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

        /// <summary>
        /// Expands node with specified value and all it's parent nodes
        /// </summary>
        /// <param name="treeview">DNN or RAD treeview.</param>
        /// <param name="value">Value of the node.</param>
        /// <param name="ignoreCase">If set to <c>true</c> ignore value case.</param>
        public static void SelectAndExpandByValue (
            Telerik.Web.UI.RadTreeView treeview,
            string value,
            bool ignoreCase = false)
        {
            if (!string.IsNullOrWhiteSpace (value)) {
                var treeNode = treeview.FindNodeByValue (value, ignoreCase);
                if (treeNode != null) {
                    treeNode.Selected = true;
	
                    // expand all parent nodes
                    treeNode = treeNode.ParentNode;
                    while (treeNode != null) {
                        treeNode.Expanded = true;
                        treeNode = treeNode.ParentNode;
                    } 
                }
            }
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

        public static string FirstCharToLower (string s)
        {
            if (!string.IsNullOrWhiteSpace (s))
            if (s.Length == 1)
                return s.ToLower ();
            else
                return s.ToLower () [0].ToString () + s.Substring (1);
		
            return s;
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
	