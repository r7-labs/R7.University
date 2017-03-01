//
//  DivisionObrnadzorViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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

using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.DivisionDirectory.Components;

namespace R7.University.DivisionDirectory
{
    internal class DivisionObrnadzorViewModel: DivisionInfo
    {
        private const string linkFormat = "<a href=\"{0}\" target=\"_blank\" {2}>{1}</a>";

        #region Properties
            
        protected ViewModelContext<DivisionDirectorySettings> Context { get; set; }

        public string Order { get; protected set; }

        public string TitleLink
        {
            get
            {
                var divisionTitle = Title + ((HasUniqueShortTitle)? string.Format (" ({0})", ShortTitle) : string.Empty);
                var divisionString = "<span itemprop=\"Name\">" + divisionTitle + "</span>";

                if (!string.IsNullOrWhiteSpace (HomePage))
                {
                    divisionString = string.Format (linkFormat, 
                        Globals.LinkClick (HomePage, Context.Module.TabId, Context.Module.ModuleId),
                        divisionString, string.Empty);
                }

                if (IsVirtual)
                {
                    // distinct virtual divisions 
                    divisionString = "<strong>" + divisionString + "</strong>";
                }

                return divisionString;
            }
        }

        public string WebSiteLink
        {
            get
            {
                if (!string.IsNullOrWhiteSpace (WebSite))
                {
                    var webSiteUrl = WebSite.Contains ("://") ? WebSite.ToLowerInvariant () : 
                        "http://" + WebSite.ToLowerInvariant ();
                    var webSiteLabel = (!string.IsNullOrWhiteSpace (WebSiteLabel)) ? WebSiteLabel : 
                        WebSite.Contains ("://") ? WebSite.Remove (0, WebSite.IndexOf ("://") + 3) : WebSite;
                    
                    return string.Format (linkFormat, webSiteUrl, webSiteLabel, "itemprop=\"Site\"");
                }

                return string.Empty;
            }
        }

        public string EmailLink 
        { 
            get
            { 
                if (!string.IsNullOrWhiteSpace (Email))
                {
                    return string.Format (linkFormat, "mailto:" + Email, Email, "itemprop=\"E-mail\"");
                }

                return string.Empty;
            }
        }

        public string DocumentLink
        {
            get
            {
                // (main) document
                if (!string.IsNullOrWhiteSpace (DocumentUrl))
                {
                    return string.Format (linkFormat, 
                        Globals.LinkClick (DocumentUrl, Context.Module.TabId, Context.Module.ModuleId),
                        Localization.GetString ("Regulations.Text", Context.LocalResourceFile),
                        "itemprop=\"DivisionClause_DocLink\""
                    );
                }

                return string.Empty;
            }
        }

        public string LocationString
        {
            get {
                var location = TextUtils.FormatList (", ", Address, Location);
                if (!string.IsNullOrWhiteSpace (location)) {
                    return "<span itemprop=\"AddressStr\">" + location  + "</span>";
                }

                return string.Empty;
            }
        }

        #endregion

        public DivisionObrnadzorViewModel (DivisionInfo division, ViewModelContext<DivisionDirectorySettings> context)
        {
            CopyCstor.Copy<DivisionInfo> (division, this);
            Context = context;
        }

        public static IEnumerable<DivisionObrnadzorViewModel> Create (IEnumerable<DivisionInfo> divisions, ViewModelContext<DivisionDirectorySettings> viewModelContext)
        {
            var now = HttpContext.Current.Timestamp;

            // TODO: If parent division not published, ensure what child divisions also not
            var divisionViewModels = divisions.Select (d => new DivisionObrnadzorViewModel (d, viewModelContext))
                .Where (d => d.IsPublished (now) || viewModelContext.Module.IsEditable)
                .Where (d => !d.IsInformal || viewModelContext.Settings.ShowInformal || viewModelContext.Module.IsEditable)
                .ToList ();

            CalculateOrder (divisionViewModels);

            return divisionViewModels;
        }

        /// <summary>
        /// Calculates the hierarchical order of divisions.
        /// </summary>
        /// <param name="divisions">Divisions that must be properly sorted before the call.</param>
        protected static void CalculateOrder (IList<DivisionObrnadzorViewModel> divisions)
        {
            // TODO: Get hierarchical data from DB, without recalculating it?

            const string separator = ".";
            var orderCounter = 1;
            var orderStack = new List<int> ();
            var returnStack = new Stack<DivisionObrnadzorViewModel> ();
           
            DivisionObrnadzorViewModel prevDivision = null;
           
            foreach (var division in divisions)
            {
                if (prevDivision != null)
                {
                    if (division.ParentDivisionID == prevDivision.ParentDivisionID) {
                        // moving on same level
                        orderCounter++;
                    }
                    else if (division.ParentDivisionID == prevDivision.DivisionID) {
                        // moving down
                        orderStack.Add (orderCounter);
                        returnStack.Push (prevDivision);
                        orderCounter = 1;
                    }
                    else {
                        // moving up
                        while (returnStack.Count > 0 && orderStack.Count > 0) {
                            orderCounter = orderStack [orderStack.Count - 1];
                            orderStack.RemoveAt (orderStack.Count - 1);

                            if (division.ParentDivisionID == returnStack.Pop ().ParentDivisionID) {
                                break;
                            }
                        }

                        orderCounter++;
                    }
                }

                // format order value
                if (orderStack.Count == 0) {
                    division.Order = orderCounter + separator;
                    division.Level = 0;
                }
                else {
                    division.Order = TextUtils.FormatList (separator, orderStack) + separator + orderCounter + separator;
                }

                prevDivision = division;
            }
        }
    }
}

