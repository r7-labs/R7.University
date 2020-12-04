//
//  ContingentController.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
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
using R7.University.EduProgramProfiles.Models;
using R7.University.EduProgramProfiles.Queries;
using R7.University.EduProgramProfiles.ViewModels;
using R7.University.Models;
using R7.University.Queries;
using R7.University.ModelExtensions;
using R7.Dnn.Extensions.Models;

namespace R7.University.EduPrograms.Controllers
{
    [DnnHandleError]
    public class ContingentController : DnnController
    {
        #region Actions

        public ActionResult ContingentDirectory ()
        {
            return View (GetCachedContingentViewModel ().WithFilter (ModuleContext.IsEditable, HttpContext.Timestamp));
        }

        #endregion

        ContingentDirectoryViewModel GetCachedContingentViewModel ()
        {
            var cacheKey = $"//r7_University/Modules/ContingentDirectory?ModuleId={ActiveModule.ModuleID}";
            return DataCache.GetCachedData<ContingentDirectoryViewModel> (
                new CacheItemArgs (cacheKey, UniversityConfig.Instance.DataCacheTime),
                (c) => GetContingentViewModel ()
            );
        }

        ContingentDirectoryViewModel GetContingentViewModel ()
        {
            var settings = new ContingentDirectorySettingsRepository ().GetSettings (ActiveModule);
            var viewModelContext = new ViewModelContext<ContingentDirectorySettings> (ModuleContext, LocalResourceFile, settings);

            using (var modelContext = new UniversityModelContext ()) {
                var viewModel = new ContingentDirectoryViewModel ();
                viewModel.Settings = settings;
                viewModel.LastYear = new FlatQuery<YearInfo> (modelContext).List ().LastYear ();
                viewModel.ContingentViewModels =
                             GetContingentsForContingentDirectory (modelContext, settings)
                                .Select (ev => new ContingentViewModel (ev, viewModelContext, viewModel));
                return viewModel;
            }
        }

        IEnumerable<EduProgramProfileFormYearInfo> GetContingentsForContingentDirectory (IModelContext modelContext, ContingentDirectorySettings settings)
        {
            var contingents = new ContingentQuery (modelContext).ListByDivisionAndEduLevels (
                settings.EduLevelIds,
                settings.DivisionId,
                settings.DivisionLevel
            );

            if (settings.Mode == ContingentDirectoryMode.Vacant) {
                contingents = contingents.Where (c => c.Year != null && !c.Year.AdmissionIsOpen);
            }
            else {
                contingents = contingents.Where (c => c.Year == null);
            }

            return contingents ?? Enumerable.Empty<EduProgramProfileFormYearInfo> ();
        }
    }
}
