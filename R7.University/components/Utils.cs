using System;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DotNetNuke.UI.Modules;
using DotNetNuke.UI.Skins;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Portals.Internal;
using DotNetNuke.Entities.Tabs;

namespace R7.University
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
            try 
            {
                // get tab info by tabId
                var tab = new TabController ().GetTab (tabId, Null.NullInteger, false);

                // check if this tab belongs to another portal
                if (tab.PortalID != module.ModuleContext.PortalId)
                {
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
            catch
            {
                return string.Empty;
            }
        }

		/// <summary>
		/// Expands node with specified value and all it's parent nodes
		/// </summary>
		/// <param name="treeview">DNN or RAD treeview.</param>
		/// <param name="value">Value of the node.</param>
		/// <param name="ignoreCase">If set to <c>true</c> ignore value case.</param>
        public static void SelectAndExpandByValue (Telerik.Web.UI.RadTreeView treeview, string value, bool ignoreCase = false)
		{
			if (!string.IsNullOrWhiteSpace (value))
			{
				var treeNode = treeview.FindNodeByValue (value, ignoreCase);
				if (treeNode != null)
				{
					treeNode.Selected = true;
	
					// expand all parent nodes
					treeNode = treeNode.ParentNode;
					while (treeNode != null)
					{
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
            if (level < maxLevel)
            {
                node.Expanded = true;
                if (node.Nodes != null)
                    foreach (Telerik.Web.UI.RadTreeNode child in node.Nodes)
                        ExpandNodeToLevel (child, level + 1, maxLevel);
            }
        }

		public static bool IsNull<T> (T? n) where T: struct
		{
			// REVIEW: n.HasValue is equvalent to n != null
			if (n.HasValue && !Null.IsNull (n.Value))
				return false;

			return true;
		}

		/*
		public static Nullable<T> ParseToNullable<T>(string value) where T: struct
		{
			T n;

			if (Convert.ChangeType(value, typeof(T))
				return Null.IsNull (n)? null : (Nullable<T>) n;
			else
				return null;
		}*/

		/// <summary>
		/// Formats the list of arguments, excluding empty
		/// </summary>
		/// <returns>Formatted list.</returns>
		/// <param name="separator">Separator.</param>
		/// <param name="args">Arguments.</param>
		public static string FormatList (string separator, params object[] args)
		{
			return FormatList (separator, (IEnumerable)args);
		}

		public static string FormatList (string separator, IEnumerable args)
		{
			var sb = new StringBuilder ();

			var i = 0;
			foreach (var a in args)
			{
				if (a != null && !string.IsNullOrWhiteSpace (a.ToString ()))
				{
					if (i++ > 0)
						sb.Append (separator);

					sb.Append (a);
				}
			}

			return sb.ToString ();
		}

		public static string FirstCharToUpper (string s)
		{
			if (!string.IsNullOrWhiteSpace (s))
			if (s.Length == 1)
				return s.ToUpper ();
			else
				return s.ToUpper () [0].ToString () + s.Substring (1);
		
			return s;
		}

		public static string FirstCharToUpperInvariant (string s)
		{
			if (!string.IsNullOrWhiteSpace (s))
			if (s.Length == 1)
				return s.ToUpperInvariant ();
			else
				return s.ToUpperInvariant () [0].ToString () + s.Substring (1);
			return s;
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

		public static string FirstCharToLowerInvariant (string s)
		{
			if (!string.IsNullOrWhiteSpace (s))
			if (s.Length == 1)
				return s.ToLowerInvariant ();
			else
				return s.ToLowerInvariant () [0].ToString () + s.Substring (1);
			return s;
		}

		public static void SynchronizeModule (IModuleControl module)
		{
			ModuleController.SynchronizeModule (module.ModuleContext.ModuleId);

			// REVIEW: update module cache (temporary fix before 7.2.0)?
			// more info: https://github.com/dnnsoftware/Dnn.Platform/pull/21
			var moduleController = new ModuleController ();
			moduleController.ClearCache (module.ModuleContext.TabId);

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
	