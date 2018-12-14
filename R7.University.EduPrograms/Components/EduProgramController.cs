//
//  R7.University.EduProgramController.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2018 Roman M. Yagodin
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
using R7.University.EduPrograms.Models;
using R7.University.EduPrograms.Queries;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.EduPrograms.Components
{
    public class EduProgramController: ModuleSearchBase
    {
        #region ModuleSearchBase implementaion

        public override IList<SearchDocument> GetModifiedSearchDocuments (ModuleInfo moduleInfo, DateTime beginDateUtc)
        {
            var searchDocs = new List<SearchDocument> ();
            var settings = new EduProgramSettingsRepository ().GetSettings (moduleInfo);
            var portalSettings = HttpOffContextHelper.GetPortalSettings (moduleInfo.PortalID, moduleInfo.TabID, moduleInfo.CultureCode);

            var eduProgram = default (EduProgramInfo);
            if (settings.EduProgramId != null) {
                using (var modelContext = new UniversityModelContext ()) {
                    eduProgram = new EduProgramQuery (modelContext).SingleOrDefault (settings.EduProgramId.Value);
                }
            }

            if (eduProgram != null && eduProgram.LastModifiedOnDate.ToUniversalTime () > beginDateUtc.ToUniversalTime ()) {
                var sd = new SearchDocument {
                    PortalId = moduleInfo.PortalID,
                    AuthorUserId = eduProgram.LastModifiedByUserId,
                    Title = eduProgram.FormatTitle (),
                    Body = eduProgram.SearchText (),
                    ModifiedTimeUtc = eduProgram.LastModifiedOnDate.ToUniversalTime (),
                    UniqueKey = string.Format ("University_EduProgram_{0}", eduProgram.EduProgramID),
                    Url = eduProgram.GetSearchUrl (moduleInfo, portalSettings),
                    IsActive = eduProgram.IsPublished (DateTime.Now)
                };

                searchDocs.Add (sd);
            }

            return searchDocs;
        }

        #endregion
    }
}
