//
//  ScienceController.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using System.Web.Mvc;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Models;
using R7.University.EduPrograms.Models;
using R7.University.EduPrograms.Queries;
using R7.University.EduPrograms.ViewModels;

namespace R7.University.EduPrograms.Controllers
{
    [DnnHandleError]
    public class ScienceController : DnnController
    {
        public ActionResult ScienceDirectory ()
        {
            return View (GetCachedScienceDirectoryViewModel ().WithFilter (ModuleContext.IsEditable, HttpContext.Timestamp));
        }

        ScienceDirectoryViewModel GetCachedScienceDirectoryViewModel ()
        {
            var cacheKey = $"//r7_University/Modules/ScienceDirectory?ModuleId={ActiveModule.ModuleID}";
            return DataCache.GetCachedData<ScienceDirectoryViewModel> (
                new CacheItemArgs (cacheKey, UniversityConfig.Instance.DataCacheTime),
                (c) => GetScienceDirectoryViewModel ()
            );
        }

        ScienceDirectoryViewModel GetScienceDirectoryViewModel ()
        {
            var settings = new ScienceDirectorySettingsRepository ().GetSettings (ActiveModule);
            var viewModelContext = new ViewModelContext<ScienceDirectorySettings> (ModuleContext, LocalResourceFile, settings);

            var viewModel = new ScienceDirectoryViewModel ();
            viewModel.EduProgramScienceViewModels = GetEduProgramsForScienceDirectory (settings.DivisionId, settings.EduLevelIds)
                .Select (ep => new EduProgramScienceViewModel (ep, viewModelContext));

            return viewModel;
        }

        IEnumerable<EduProgramInfo> GetEduProgramsForScienceDirectory (int? divisionId, IEnumerable<int> eduLevelIds)
        {
            using (var modelContext = new UniversityModelContext ()) {
                var eduPrograms = new ScienceDirectoryQuery (modelContext).ListByDivisionAndEduLevels (divisionId, eduLevelIds);
                return eduPrograms ?? Enumerable.Empty<EduProgramInfo> ();
            }
        }
    }
}
