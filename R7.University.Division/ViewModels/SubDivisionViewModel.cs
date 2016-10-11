//
//  SubDivisionViewModel.cs
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

using System;
using System.Web;
using DotNetNuke.Common;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Division
{
    internal class SubDivisionViewModel: DivisionInfo
    {
        protected ViewModelContext Context { get; set; }

        public string HomePageLink
        { 
            get {
                int homeTabId;
                if (int.TryParse (HomePage, out homeTabId) && Context.Module.TabId != homeTabId) {
                    return string.Format ("<a href=\"{0}\">{1}</a>", Globals.NavigateURL (homeTabId), Title);
                }

                return Title;
            }
        }

        public string CssClass
        {
            get { return !this.IsPublished (HttpContext.Current.Timestamp) ? "u8y-not-published-element" : string.Empty; }
        }

        public SubDivisionViewModel (DivisionInfo division, ViewModelContext context)
        {
            Context = context;

            Title = division.Title;
            HomePage = division.HomePage;
            StartDate = division.StartDate;
            EndDate = division.EndDate;
        }
    }
}

