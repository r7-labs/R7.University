//
// DivisionObrnadzorViewModel.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
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
using System.Collections.Generic;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;

namespace R7.University.DivisionDirectory
{
    public class DivisionObrnadzorViewModel: DivisionInfo
    {
        private const string linkFormat = "<a href=\"{0}\" {2}>{1}</a>";

        #region Properties
            
        protected ViewModelContext Context { get; set; }

        public string Order { get; protected set; }

        // TODO: Implement this
        // public string HeadEmployee
        // {
        //      get { throw new NotImplementedException (); }
        // }

        public string TitleLink
        {
            get
            {
                var divisionTitle = Title + ((HasUniqueShortTitle)? string.Format (" ({0})", ShortTitle) : string.Empty);
                var divisionString = "<span itemprop=\"Name\">" + divisionTitle + "</span>";

                if (!string.IsNullOrWhiteSpace (HomePage))
                {
                    divisionString = string.Format (linkFormat, 
                        Globals.LinkClick (HomePage, Context.ModuleContext.TabId, Context.ModuleContext.ModuleId),
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
                        Globals.LinkClick (DocumentUrl, Context.ModuleContext.TabId, Context.ModuleContext.ModuleId),
                        Localization.GetString ("Regulations.Text", Context.Control.LocalResourceFile),
                        "itemprop=\"DivisionClause_DocLink\""
                    );
                }

                return string.Empty;
            }
        }

        public string LocationString
        {
            get
            { 
                if (!string.IsNullOrWhiteSpace (Location))
                    return "<span itemprop=\"AddressStr\">" + Location + "</span>";

                return string.Empty;
            }
        }

        #endregion

        public DivisionObrnadzorViewModel (DivisionInfo division, ViewModelContext context)
        {
            CopyCstor.Copy<DivisionInfo> (division, this);
            Context = context;
        }

        public static IEnumerable<DivisionObrnadzorViewModel> Create (IEnumerable<DivisionInfo> divisions, ViewModelContext viewModelContext)
        {
            var divisionViewModels = divisions.Select (d => new DivisionObrnadzorViewModel (d, viewModelContext)).ToList ();
            CalculateOrder (divisionViewModels);

            return divisionViewModels;
        }

        /// <summary>
        /// Calculates the hierarchical order of divisions.
        /// </summary>
        /// <param name="divisions">Divisions. Must be properly sorted before the call.</param>
        protected static void CalculateOrder (IList<DivisionObrnadzorViewModel> divisions)
        {
            const string separator = ".";
            var orderCounter = 1;
            var orderStack = new List<int> ();
           
            DivisionObrnadzorViewModel prevDivision = null;

            foreach (var division in divisions)
            {
                if (prevDivision != null)
                {
                    // moving on same level
                    if (division.ParentDivisionID == prevDivision.ParentDivisionID)
                    {
                        orderCounter++;
                    }
                    // moving down
                    else if (division.ParentDivisionID == prevDivision.DivisionID)
                    {
                        orderStack.Add (orderCounter);
                        orderCounter = 1;
                    }
                    // moving up
                    else
                    {
                        orderCounter = orderStack [orderStack.Count - 1];
                        orderStack.RemoveAt (orderStack.Count - 1);
                        orderCounter++;
                    }
                }

                // format order value
                if (orderStack.Count == 0)
                {
                    division.Order = orderCounter + separator;
                }
                else
                {
                    division.Order = Utils.FormatList (separator, orderStack) + separator + orderCounter + separator;
                }

                prevDivision = division;
            }
        }
    }
}

