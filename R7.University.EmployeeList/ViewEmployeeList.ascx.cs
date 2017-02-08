//
//  ViewEmployeeList.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2017 Roman M. Yagodin
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
using System.Web.Caching;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.TextExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.EmployeeList.Components;
using R7.University.EmployeeList.Queries;
using R7.University.EmployeeList.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Security;
using R7.University.SharedLogic;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.EmployeeList
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

        protected string EditIconUrl
        {
            get { return IconController.IconURL ("Edit"); }
        }

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

        #region Handlers

        // REVIEW: Move to repository class?,
        internal EmployeeListViewModel GetViewModel ()
        {
            var cacheKey = "//r7_University/Modules/EmployeeList?TabModuleId=" + TabModuleId;
            return DataCache.GetCachedData<EmployeeListViewModel> (
                new CacheItemArgs (cacheKey, UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetViewModel_Internal ()
            ).SetContext (ViewModelContext);
        }

        internal EmployeeListViewModel GetViewModel_Internal ()
        {
            var employeeQuery = new EmployeeQuery (ModelContext);

            // get employees (w/o references, sorted)
            var sortedEmployees = employeeQuery.ListByDivisionId (Settings.DivisionID, Settings.IncludeSubdivisions, Settings.SortType).ToList ();

            // get employees (with references, unsorted)
            var filledEmployees = employeeQuery.ListByIds (sortedEmployees.Select (se => se.EmployeeID));

            // update sorted employees list
            for (var i = 0; i < sortedEmployees.Count; i++) {
                sortedEmployees [i] = filledEmployees.Single (fe => fe.EmployeeID == sortedEmployees [i].EmployeeID);
            }

            return new EmployeeListViewModel (
                sortedEmployees,
                ModelContext.Get<DivisionInfo> (Settings.DivisionID)
            );
        }

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
			
            try {
                if (!IsPostBack || ViewState.Count == 0) { // Fix for issue #23
                    var now = HttpContext.Current.Timestamp;
                    // get employees
                    var employees = GetViewModel ().Employees
                        .Where (empl => IsEditable || empl.IsPublished (now));
            
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
                    IconController.IconURL ("Add"),
                    Null.IsNull (Settings.DivisionID) ?
                        EditUrl ("EditEmployee")
                        // pass division_id to select division in which to add employee
                        : EditUrl ("division_id", Settings.DivisionID.ToString (), "EditEmployee"),
                    false, 
                    SecurityAccessLevel.Edit,
                    SecurityContext.CanAdd<EmployeeInfo> (),
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

            var employeeDetailsUrl = UniversityUrlHelper.IESafeEditUrl (this, Request, "employee_id", employee.EmployeeID.ToString (), "EmployeeDetails");
                
            // photo fallback
            if (string.IsNullOrWhiteSpace (imagePhoto.ImageUrl)) {
                linkDetails.Visible = false;
            }
            else {
                // link to employee details
                linkDetails.NavigateUrl = employeeDetailsUrl;
            }

            // employee fullname
            linkFullName.Text = FormatHelper.FullName (employee.FirstName, employee.LastName, employee.OtherName);
            linkFullName.NavigateUrl = employeeDetailsUrl;

            // get current employee title achievements
            var achievements = employee.Achievements
                .Select (ea => new EmployeeAchievementViewModel (ea))
                .Where (ach => ach.IsTitle);
            
            var titles = achievements.Select (ach => FormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix).FirstCharToLower ());
			
            // employee title achievements
            var strTitles = TextUtils.FormatList (", ", titles);
            if (!string.IsNullOrWhiteSpace (strTitles))
                labelAcademicDegreeAndTitle.Text = "&nbsp;&ndash; " + strTitles;
            else
                labelAcademicDegreeAndTitle.Visible = false;
			
            // phones
            var phones = TextUtils.FormatList (", ", employee.Phone, employee.CellPhone);
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
                linkWebSite.NavigateUrl = FormatHelper.FormatWebSiteUrl (employee.WebSite);
                linkWebSite.Text = FormatHelper.FormatWebSiteLabel (employee.WebSite, employee.WebSiteLabel);
            }
            else {
                linkWebSite.Visible = false;
            }

            // profile link
            if (!TypeUtils.IsNull<int> (employee.UserID)) {
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
                .GroupByDivision ();
                
            // build positions value
            var positionsVisible = false;
            if (!gops.IsNullOrEmpty ()) {
                var strOps = string.Empty;
                foreach (var gop in gops) {
                    // gop.Title is a comma-separated list of grouped positions
                    strOps = TextUtils.FormatList ("; ", strOps, TextUtils.FormatList (": ", gop.Title, 
                        // do not display division title also for current division
                        (gop.OccupiedPosition.DivisionID != Settings.DivisionID) ? gop.OccupiedPosition.FormatDivisionLink (this) : string.Empty));
                }

                if (!string.IsNullOrWhiteSpace (strOps)) {
                    labelPositions.Text = strOps;
                    positionsVisible = true;
                }
            }
            labelPositions.Visible = positionsVisible;
        }
    }
}

