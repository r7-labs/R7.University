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
using System.Web.Mvc;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.Science.Models;
using R7.University.Science.Queries;
using R7.University.Science.ViewModels;
using R7.University.Security;

namespace R7.University.Science.Controllers
{
    [DnnHandleError]
    public class ScienceController : DnnController
    {
        ViewModelContext<ScienceDirectorySettings> _viewModelContext;
        protected ViewModelContext<ScienceDirectorySettings> ViewModelContext =>
            _viewModelContext ?? (_viewModelContext = new ViewModelContext<ScienceDirectorySettings> (ModuleContext, LocalResourceFile, Settings));

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext =>
            securityContext ?? (securityContext = new ModuleSecurityContext (User));

        ScienceDirectorySettings _settings;
        protected ScienceDirectorySettings Settings =>
            _settings ?? (_settings = new ScienceDirectorySettingsRepository ().GetSettings (ActiveModule));

        public ActionResult ScienceDirectory ()
        {
            var viewModel = new ScienceDirectoryViewModel ();

            viewModel.EduProgramScienceViewModels = GetEduPrograms ();

            return View (viewModel);
        }

        IEnumerable<EduProgramInfo> GetEduPrograms ()
        {
            using (var modelContext = new UniversityModelContext ()) {
                return new EduProgramScienceQuery (modelContext).ListByDivisionAndEduLevels (Settings.DivisionId, Settings.EduLevelIds);
            }
        }
    }
}
