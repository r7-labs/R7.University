//
//  ViewEmployeeList.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2019 Roman M. Yagodin
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
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Collections;
using R7.Dnn.Extensions.Modules;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.Urls;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Employees.Models;
using R7.University.Employees.Queries;
using R7.University.Employees.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Security;
using R7.University.SharedLogic;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.Employees
{
    public partial class ViewEmployeeList: PortalModuleBase<EmployeeListSettings>, IActionable
    {
        #region Model context

        private UniversityModelContext modelContext;
        protected UniversityModelContext ModelContext
        {
            get { return modelContext ?? (modelContext = new UniversityModelContext ()); }
        }

        public override void Dispose ()
        {
            if (modelContext != null) {
                modelContext.Dispose ();
            }

            base.Dispose ();
        }

        #endregion

        #region Properties

        ViewModelContext<EmployeeListSettings> viewModelContext;
        protected ViewModelContext<EmployeeListSettings> ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext<EmployeeListSettings> (this, Settings)); }
        }

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo)); }
        }

        #endregion

        internal EmployeeListViewModel GetViewModel ()
        {
            // TODO: Restore caching
            /*var cacheKey = "//r7_University/Modules/EmployeeList?TabModuleId=" + TabModuleId;
            return DataCache.GetCachedData<EmployeeListViewModel> (
                new CacheItemArgs (cacheKey, UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetViewModel_Internal ()
            ).SetContext (ViewModelContext);
*/
            return GetViewModel_Internal ().SetContext (ViewModelContext);
        }

        internal EmployeeListViewModel GetViewModel_Internal ()
        {
            var employeeQuery = new EmployeeQuery (ModelContext);

            // TODO: Can get all employee data in EmployeeQuery.ListByDivisionId()

            // get employees (w/o references, sorted)
            var sortedEmployees = employeeQuery.ListByDivisionId (
                Settings.DivisionID, Settings.IncludeSubdivisions, (EmployeeListSortType) Settings.SortType).ToList ();

            // get employees (with references, unsorted)
            var filledEmployees = employeeQuery.ListByIds (sortedEmployees.Select (se => se.EmployeeID));

            // update sorted employees list
            for (var i = 0; i < sortedEmployees.Count; i++) {
                sortedEmployees [i] = filledEmployees.Single (fe => fe.EmployeeID == sortedEmployees [i].EmployeeID);
            }

            return new EmployeeListViewModel (
                sortedEmployees,
                ModelContext.Get<DivisionInfo,int> (Settings.DivisionID)
            );
        }

         #region Handlers

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
			
            try {
                var now = HttpContext.Current.Timestamp;
                // get employees
                var employees = GetViewModel ().Employees
                    .Where (empl => IsEditable || (empl.IsPublished (now) && empl.Positions.Any (p => p.Division.IsPublished (now))));
        
                // check if we have some content to display, 
                // otherwise display a message for module editors or hide module from regular users
                if (employees.IsNullOrEmpty ()) {
                    if (IsEditable) {
                        this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                    }
                    else {
						// hide entire module
						ContainerControl.Visible = false;
                    }
                }
                else {
                    // bind the data
                    listEmployees.DataSource = employees;
                    listEmployees.DataBind ();
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
                var actions = new ModuleActionCollection ();

                actions.Add (
                    GetNextActionID (), 
                    LocalizeString ("AddEmployee.Action"),
                    ModuleActionType.AddContent,
                    "", 
                    UniversityIcons.Add,
                    Null.IsNull (Settings.DivisionID) ?
                        EditUrl ("EditEmployee")
                        // pass division_id to select division in which to add employee
                        : EditUrl ("division_id", Settings.DivisionID.ToString (), "EditEmployee"),
                    false, 
                    SecurityAccessLevel.Edit,
                    SecurityContext.CanAdd (typeof (EmployeeInfo)),
                    false
                );

                actions.Add (
                    GetNextActionID (),
                    LocalizeString ("EditDivision.Action"),
                    ModuleActionType.EditContent,
                    "",
                    UniversityIcons.Edit,
                    EditUrl ("division_id", Settings.DivisionID.ToString (), "EditDivision"),
                    false,
                    SecurityAccessLevel.Edit,
                    !Null.IsNull (Settings.DivisionID) && SecurityContext.IsAdmin,
                    false
                );

                return actions;
            }
        }

        #endregion

        /// <summary>
        /// Handles the items being bound to the datalist control. In this method we merge the data with the
        /// template defined for this control to produce the result to display to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void listEmployees_ItemDataBound (object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            var now = HttpContext.Current.Timestamp;

            // e.Item.DataItem is of EmployeeListInfo class
            var employee = (IEmployee) e.Item.DataItem;

            // find controls in DataList item template
            var linkEdit = (HyperLink) e.Item.FindControl ("linkEdit");
            var imageEdit = (Image) e.Item.FindControl ("imageEdit");
            var imagePhoto = (Image) e.Item.FindControl ("imagePhoto");
            var linkDetails = (HyperLink) e.Item.FindControl ("linkDetails"); 
            var linkFullName = (HyperLink) e.Item.FindControl ("linkFullName");
            var labelAcademicDegreeAndTitle = (Label) e.Item.FindControl ("labelAcademicDegreeAndTitle");
            var labelPositions = (Label) e.Item.FindControl ("labelPositions");
            var labelPhones = (Label) e.Item.FindControl ("labelPhones");
            var linkEmail = (HyperLink) e.Item.FindControl ("linkEmail");
            var linkSecondaryEmail = (HyperLink) e.Item.FindControl ("linkSecondaryEmail");
            var linkWebSite = (HyperLink) e.Item.FindControl ("linkWebSite");
            var linkUserProfile = (HyperLink) e.Item.FindControl ("linkUserProfile");

            // edit link
            if (IsEditable) {
                if (Null.IsNull (Settings.DivisionID)) {
                    linkEdit.NavigateUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (), "EditEmployee");
                }
                else {
                    linkEdit.NavigateUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (),
                        "EditEmployee", "division_id", Settings.DivisionID.ToString ());
                }
            }

            // make edit link visible in edit mode
            linkEdit.Visible = IsEditable;
            imageEdit.Visible = IsEditable;
            
            // mark non-published employees, as they visible only to editors
            if (!employee.IsPublished (now)) {
                if (e.Item.ItemType == ListItemType.Item) {
                    e.Item.CssClass = listEmployees.ItemStyle.CssClass + " _nonpublished";
                }
                else {
                    e.Item.CssClass = listEmployees.AlternatingItemStyle.CssClass + " _nonpublished";
                }
            }

            // fill the controls

            // employee photo
            EmployeePhotoLogic.Bind (employee, imagePhoto, Settings.PhotoWidth, true);

            var employeeDetailsUrl = UrlHelper.AdjustPopupUrl (
                UniversityUrlHelper.IESafeEditUrl (this, Request, "employee_id", employee.EmployeeID.ToString (), "EmployeeDetails"),
                responseRedirect: false
            );

            // photo fallback
            if (string.IsNullOrWhiteSpace (imagePhoto.ImageUrl)) {
                linkDetails.Visible = false;
            }
            else {
                // link to employee details
                linkDetails.NavigateUrl = employeeDetailsUrl;
            }

            // employee fullname
            linkFullName.Text = UniversityFormatHelper.FullName (employee.FirstName, employee.LastName, employee.OtherName);
            linkFullName.NavigateUrl = employeeDetailsUrl;

            // get current employee title achievements
            var achievements = employee.Achievements
                .Select (ea => new EmployeeAchievementViewModel (ea, ViewModelContext))
                .Where (ach => ach.IsTitle);
            
            var titles = achievements.Select (ach => UniversityFormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix).FirstCharToLower ());
			
            // employee title achievements
            var strTitles = FormatHelper.JoinNotNullOrEmpty (", ", titles);
            if (!string.IsNullOrWhiteSpace (strTitles))
                labelAcademicDegreeAndTitle.Text = "&nbsp;&ndash; " + strTitles;
            else
                labelAcademicDegreeAndTitle.Visible = false;
			
            // phones
            var phones = FormatHelper.JoinNotNullOrEmpty (", ", employee.Phone, employee.CellPhone);
            if (!string.IsNullOrWhiteSpace (phones))
                labelPhones.Text = phones;
            else
                labelPhones.Visible = false;

            // email
            if (!string.IsNullOrWhiteSpace (employee.Email)) {
                linkEmail.NavigateUrl = "mailto:" + employee.Email;
                linkEmail.Text = employee.Email; 
            }
            else
                linkEmail.Visible = false;

            // secondary email
            if (!string.IsNullOrWhiteSpace (employee.SecondaryEmail)) {
                linkSecondaryEmail.NavigateUrl = "mailto:" + employee.SecondaryEmail;
                linkSecondaryEmail.Text = employee.SecondaryEmail; 
            }
            else
                linkSecondaryEmail.Visible = false;

            // webSite
            if (!string.IsNullOrWhiteSpace (employee.WebSite)) {
                linkWebSite.NavigateUrl = UniversityFormatHelper.FormatWebSiteUrl (employee.WebSite);
                linkWebSite.Text = UniversityFormatHelper.FormatWebSiteLabel (employee.WebSite, employee.WebSiteLabel);
            }
            else {
                linkWebSite.Visible = false;
            }

            // profile link
            if (employee.UserID != null && !Null.IsNull (employee.UserID.Value)) {
                linkUserProfile.NavigateUrl = Globals.UserProfileURL (employee.UserID.Value);
                // TODO: Replace profile text with something more sane
                linkUserProfile.Text = Localization.GetString ("VisitProfile.Text", LocalResourceFile);
            }
            else
                linkUserProfile.Visible = false;

            // get current employee occupied positions, grouped
            var gops = employee.Positions
                .OrderByDescending (op => op.DivisionID == Settings.DivisionID)
                .ThenByDescending (op => op.Position.Weight)
                .GroupByDivision (HttpContext.Current.Timestamp, IsEditable);
                
            // build positions value
            var positionsVisible = false;
            if (!gops.IsNullOrEmpty ()) {
                var strOps = string.Empty;
                foreach (var gop in gops) {
                    var cssClass = !gop.OccupiedPosition.Division.IsPublished (HttpContext.Current.Timestamp)
                        ? " class=\"u8y-not-published-element\"" : string.Empty;
                        strOps = FormatHelper.JoinNotNullOrEmpty ("; ", strOps,
                            $"<span{cssClass}>"
                            // gop.Title is a comma-separated list of grouped positions
                            + FormatHelper.JoinNotNullOrEmpty (": ", gop.Title,
                                // TODO: Move to the module display settings?
                                // don't display division title also for current division
                                (gop.OccupiedPosition.DivisionID != Settings.DivisionID) ? gop.OccupiedPosition.FormatDivisionLink (this) : string.Empty)
                            + "</span>");
                }

                labelPositions.Text = $"<label>{LocalizeString ("OccupiedPositions.Text")}</label> {strOps}";
                positionsVisible = true;
            }
            labelPositions.Visible = positionsVisible;
        }
    }
}

