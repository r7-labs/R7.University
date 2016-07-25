//
//  DivisionController.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search.Entities;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Division.Components
{
    public class DivisionController : ModuleSearchBase
    {
        #region ModuleSearchBase implementaion

        public override IList<SearchDocument> GetModifiedSearchDocuments (ModuleInfo modInfo, DateTime beginDate)
        {
            var searchDocs = new List<SearchDocument> ();
            var settings = new DivisionSettings (modInfo);

            using (var modelContext = new UniversityModelContext ()) {

                var division = modelContext.Get<DivisionInfo> (settings.DivisionID);
                if (division != null && division.LastModifiedOnDate.ToUniversalTime () > beginDate.ToUniversalTime ()) {
                    var aboutDivision = division.SearchDocumentText;
                    var sd = new SearchDocument ()
                    {
                        PortalId = modInfo.PortalID,
                        AuthorUserId = division.LastModifiedByUserID,
                        Title = division.Title,
                        // Description = HtmlUtils.Shorten (aboutDivision, 255, "..."),
                        Body = aboutDivision,
                        ModifiedTimeUtc = division.LastModifiedOnDate.ToUniversalTime (),
                        UniqueKey = string.Format ("University_Division_{0}", division.DivisionID),
                        Url = string.Format ("/Default.aspx?tabid={0}#{1}", modInfo.TabID, modInfo.ModuleID),
                        IsActive = division.IsPublished (DateTime.Now)
                    };
	
                    searchDocs.Add (sd);
                }
			
                return searchDocs;
            }
        }

        #endregion
    }
}

