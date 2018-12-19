//
//  DivisionController.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2018 Roman M. Yagodin
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
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search.Entities;
using R7.University.Divisions.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Divisions.Components
{
    public class DivisionController : ModuleSearchBase
    {
        #region ModuleSearchBase implementaion

        public override IList<SearchDocument> GetModifiedSearchDocuments (ModuleInfo moduleInfo, DateTime beginDateUtc)
        {
            var searchDocs = new List<SearchDocument> ();
            var settings = new DivisionSettingsRepository ().GetSettings (moduleInfo);
            var portalSettings = HttpOffContextHelper.GetPortalSettings (moduleInfo.PortalID, moduleInfo.TabID, moduleInfo.CultureCode);

            using (var modelContext = new UniversityModelContext ()) {
                var division = modelContext.Get<DivisionInfo,int> (settings.DivisionID);
                if (division != null && division.LastModifiedOnDate.ToUniversalTime () > beginDateUtc.ToUniversalTime ()) {
                    var sd = new SearchDocument {
                        PortalId = moduleInfo.PortalID,
                        AuthorUserId = division.LastModifiedByUserId,
                        Title = division.Title,
                        Body = division.SearchText (),
                        ModifiedTimeUtc = division.LastModifiedOnDate.ToUniversalTime (),
                        UniqueKey = string.Format ("University_Division_{0}", division.DivisionID),
                        Url = division.GetSearchUrl (moduleInfo, portalSettings),
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
