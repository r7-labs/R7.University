//
//  DivisionController.cs
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

using System.Web.Mvc;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Division.Models;
using R7.University.Division.Queries;
using R7.University.Division.ViewModels;
using R7.University.Models;
using R7.University.Security;

namespace R7.University.Division.Controllers
{
    [DnnHandleError]
    public class DivisionController : DnnController
    {
        ViewModelContext<DivisionSettings> viewModelContext;
        protected ViewModelContext<DivisionSettings> ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext<DivisionSettings> (ModuleContext, LocalResourceFile, Settings)); }
        }

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (User)); }
        }

        DivisionSettings _settings;
        protected DivisionSettings Settings =>
            _settings ?? (_settings = new DivisionSettingsRepository ().GetSettings (ActiveModule));

        [ModuleActionItems]
        public ActionResult Index ()
        {
            var division = DataCache.GetCachedData<IDivision> (
                new CacheItemArgs ("//r7_University/Division?ModuleId=" + ActiveModule.ModuleID, UniversityConfig.Instance.DataCacheTime),
                (c) => GetDivision ()
            );

            if (division != null) {
                return View (
                    new DivisionViewModel (division,
                        new ViewModelContext<DivisionSettings> (ModuleContext, LocalResourceFile, Settings)
                    )
                );
            }

            return View (new DivisionViewModel ());
        }

        IDivision GetDivision ()
        {
            using (var modelContext = new UniversityModelContext ()) {
                return new DivisionQuery (modelContext).SingleOrDefault (Settings.DivisionID);
            }
        }

        public ModuleActionCollection GetIndexActions ()
        {
            var actions = new ModuleActionCollection ();
            var existingDivision = !Null.IsNull (Settings.DivisionID);

            actions.Add (
                -1,
                LocalizeString ("AddDivision.Action"),
                ModuleActionType.AddContent,
                "",
                UniversityIcons.Add,
                ModuleContext.EditUrl ("EditDivision"),
                false,
                SecurityAccessLevel.Edit,
                !existingDivision && SecurityContext.CanAdd (typeof (DivisionInfo)),
                false
            );

            actions.Add (
                -1,
                LocalizeString ("EditDivision.Action"),
                ModuleActionType.EditContent,
                "",
                UniversityIcons.Edit,
                ModuleContext.EditUrl ("division_id", Settings.DivisionID.ToString (), "EditDivision"),
                false,
                SecurityAccessLevel.Edit,
                existingDivision,
                false
            );

            return actions;
        }
    }
}
