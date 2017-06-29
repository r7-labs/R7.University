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
using R7.University.Division.Components;
using R7.University.Division.Queries;
using R7.University.Division.ViewModels;
using R7.University.Models;
using R7.University.Security;

namespace R7.University.Division.Controllers
{
    [DnnHandleError]
    public class DivisionController : DnnController
    {
        #region Model context

        UniversityModelContext modelContext;
        protected UniversityModelContext ModelContext
        {
            get { return modelContext ?? (modelContext = new UniversityModelContext ()); }
        }

        protected override void Dispose (bool disposing)
        {   
            if (disposing && modelContext != null) {
                modelContext.Dispose ();
            }

            base.Dispose (disposing);
        }

        #endregion

        ViewModelContext<DivisionSettings> viewModelContext;
        protected ViewModelContext<DivisionSettings> ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext =
                                              new ViewModelContext<DivisionSettings> (ModuleContext, LocalResourceFile, new DivisionSettingsRepository ().GetSettings (ActiveModule))); }
        }

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (User)); }
        }

        [ModuleActionItems]
        public ActionResult Index ()
        {
            var settings = new DivisionSettingsRepository ().GetSettings (ActiveModule);
            // TODO: Use cache
            var division = new DivisionQuery (ModelContext).SingleOrDefault (settings.DivisionID);

            // TODO: Move to the view
            /*
             var hasData = division != null;
             var now = Request.RequestContext.HttpContext.Timestamp;
            if (!hasData) {
                // division wasn't set or not found
                if (ModuleContext.IsEditable) {
                    this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                }
            }
            else if (!division.IsPublished (now)) {
                // division isn't published
                if (ModuleContext.IsEditable) {
                    this.Message ("DivisionNotPublished.Text", MessageType.Warning, true);
                }
            }

            // display module only in edit mode and only if we have published data to display
            ContainerControl.Visible = IsEditable || (hasData && division.IsPublished (now));

            // display module content only if it exists and published (or in edit mode)
            var displayContent = hasData && (IsEditable || division.IsPublished (now));

            panelDivision.Visible = displayContent;

            if (displayContent) {
                // display division info
                DisplayDivision (division
                }
             */
            
            if (division != null) {
                return View (new DivisionViewModel (division, ViewModelContext));
            }

            return View (new DivisionViewModel ());
        }

        public ModuleActionCollection GetIndexActions ()
        {
            var actions = new ModuleActionCollection ();
            var settings = new DivisionSettingsRepository ().GetSettings (ActiveModule);

            var existingDivision = !Null.IsNull (settings.DivisionID);

            actions.Add (
                -1,
                LocalizeString ("AddDivision.Action"),
                ModuleActionType.AddContent,
                "",
                IconController.IconURL ("Add"),
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
                IconController.IconURL ("Edit"),
                ModuleContext.EditUrl ("division_id", settings.DivisionID.ToString (), "EditDivision"),
                false,
                SecurityAccessLevel.Edit,
                existingDivision,
                false
            );

            return actions;
        }
    }
}
