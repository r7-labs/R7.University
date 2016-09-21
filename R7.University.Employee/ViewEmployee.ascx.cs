//
//  ViewEmployee.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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
using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Employee.Components;
using R7.University.Employee.Queries;
using R7.University.Employee.SharedLogic;
using R7.University.Employee.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.SharedLogic;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.Employee
{
    public partial class ViewEmployee: PortalModuleBase<EmployeeSettings>, IActionable
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

        #region Get data

        protected EmployeeInfo GetEmployee ()
        {
            if (Settings.ShowCurrentUser) {
                return GetEmployee_CurrentUser_Internal ();
            }

            return DataCache.GetCachedData<EmployeeInfo> (new CacheItemArgs ("//r7_University/Modules/Employee?ModuleId=" + ModuleId,
                    UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetEmployee_Internal ()
            );
        }

        protected EmployeeInfo GetEmployee_Internal ()
        {
            return new EmployeeQuery (ModelContext).SingleOrDefault (Settings.EmployeeID);
        }

        protected EmployeeInfo GetEmployee_CurrentUser_Internal ()
        {
            var userId = TypeUtils.ParseToNullable<int> (Request.QueryString ["userid"]);
            if (userId != null) {
                return new EmployeeQuery (ModelContext).SingleOrDefaultByUserId (userId.Value);
            }

            return null;
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            try {
                if (!IsPostBack || ViewState.Count == 0) { // Fix for issue #23

                    var employee = GetEmployee ();
                    var now = HttpContext.Current.Timestamp;

                    if (employee == null) {
                        // employee isn't set or not found
                        if (IsEditable) {
                            this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                        }
                    }
                    else if (!employee.IsPublished (now)) {
                        // employee isn't published
                        if (IsEditable) {
                            this.Message ("EmployeeNotPublished.Text", MessageType.Warning, true);
                        }
                    }

                    var hasData = employee != null;
                    						
                    // display module only in edit mode
                    // only if we have published data to display
                    ContainerControl.Visible = IsEditable || (hasData && employee.IsPublished (now));
											
                    // display module content only if it exists and published (or in edit mode)
                    var displayContent = hasData && (IsEditable || employee.IsPublished (now));

                    panelEmployee.Visible = displayContent;
					
                    if (displayContent) {
                        if (Settings.AutoTitle) {
                            ModuleHelper.UpdateModuleTitle (TabModuleId, 
                                FormatHelper.AbbrName (employee.FirstName, employee.LastName, employee.OtherName)
                            );
                        }

                        // display employee info
                        Display (employee);
                    }

                } // if (!IsPostBack)
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        /// <summary>
        /// Displays the specified employee.
        /// </summary>
        /// <param name="employee">Employee.</param>
        protected void Display (IEmployee employee)
        {
            if (employee.Positions.Any ()) {
                repeaterPositions.DataSource = employee.Positions
                    .OrderByDescending (op => op.Position.Weight)
                    .GroupByDivision ();
                repeaterPositions.DataBind ();
            }
            else
                repeaterPositions.Visible = false;

            // Full name
            var fullName = FormatHelper.FullName (employee.FirstName, employee.LastName, employee.OtherName);
            labelFullName.Text = fullName;

            EmployeePhotoLogic.Bind (employee, imagePhoto, Settings.PhotoWidth);

            var popupUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (), "EmployeeDetails");
				
            // alter popup window height
            linkPhoto.NavigateUrl = popupUrl;

            // Employee titles
            var titles = employee.Achievements
                .Select (ach => new EmployeeAchievementViewModel (ach, new ViewModelContext (this)))
                .Where (ach => ach.IsTitle)
                .Select (ach => R7.University.Utilities.Utils.FirstCharToLower (
                    FormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix)));
			
            var strTitles = TextUtils.FormatList (", ", titles);
            if (!string.IsNullOrWhiteSpace (strTitles))
                labelAcademicDegreeAndTitle.Text = "&nbsp;&ndash; " + strTitles;
            else
                labelAcademicDegreeAndTitle.Visible = false;
	
            // Phone
            if (!string.IsNullOrWhiteSpace (employee.Phone))
                labelPhone.Text = employee.Phone;
            else
                labelPhone.Visible = false;

            // CellPhome
            if (!string.IsNullOrWhiteSpace (employee.CellPhone))
                labelCellPhone.Text = employee.CellPhone;
            else
                labelCellPhone.Visible = false;

            // Fax
            if (!string.IsNullOrWhiteSpace (employee.Fax))
                labelFax.Text = string.Format (Localization.GetString ("Fax.Format", LocalResourceFile), employee.Fax);
            else
                labelFax.Visible = false;

            // Messenger
            if (!string.IsNullOrWhiteSpace (employee.Messenger))
                labelMessenger.Text = employee.Messenger;
            else
                labelMessenger.Visible = false;

            // Working place and Hours
            var workingPlaceAndHours = TextUtils.FormatList (", ", employee.WorkingPlace, employee.WorkingHours);
            if (!string.IsNullOrWhiteSpace (workingPlaceAndHours))
                labelWorkingPlaceAndHours.Text = workingPlaceAndHours;
            else
                labelWorkingPlaceAndHours.Visible = false;
            
            // WebSite
            if (!string.IsNullOrWhiteSpace (employee.WebSite)) {
                linkWebSite.NavigateUrl = FormatHelper.FormatWebSiteUrl (employee.WebSite);
                linkWebSite.Text = FormatHelper.FormatWebSiteLabel (employee.WebSite, employee.WebSiteLabel);
            }
            else {
                linkWebSite.Visible = false;
            }

            // Email
            if (!string.IsNullOrWhiteSpace (employee.Email)) {
                linkEmail.NavigateUrl = "mailto:" + employee.Email;
                linkEmail.Text = employee.Email;
            }
            else
                linkEmail.Visible = false;

            // Secondary email
            if (!string.IsNullOrWhiteSpace (employee.SecondaryEmail)) {
                linkSecondaryEmail.NavigateUrl = "mailto:" + employee.SecondaryEmail;
                linkSecondaryEmail.Text = employee.SecondaryEmail;
            }
            else
                linkSecondaryEmail.Visible = false;

            // Profile link
            if (!TypeUtils.IsNull<int> (employee.UserID))
                linkUserProfile.NavigateUrl = Globals.UserProfileURL (employee.UserID.Value);
            else
                linkUserProfile.Visible = false;
        }

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
                // create a new action to add an item, this will be added 
                // to the controls dropdown menu
                var actions = new ModuleActionCollection ();
                var employee = GetEmployee ();

                actions.Add (
                    GetNextActionID (), 
                    Localization.GetString ("AddEmployee.Action", this.LocalResourceFile),
                    ModuleActionType.AddContent, 
                    "", 
                    IconController.IconURL ("Add"), 
                    EditUrl ("EditEmployee"),
                    false, 
                    SecurityAccessLevel.Edit,
                    employee == null,
                    false
                );

                if (employee != null) {
                    // otherwise, add "edit" action
                    actions.Add (
                        GetNextActionID (), 
                        LocalizeString ("EditEmployee.Action"),
                        ModuleActionType.EditContent, 
                        "", 
                        IconController.IconURL ("Edit"),
                        EditUrl ("employee_id", employee.EmployeeID.ToString (), "EditEmployee"),
                        false, 
                        SecurityAccessLevel.Edit,
                        true, 
                        false
                    );

                    actions.Add (
                        GetNextActionID (), 
                        LocalizeString ("Details.Action"),
                        ModuleActionType.ContentOptions, 
                        "", 
                        IconController.IconURL ("View"),
                        EditUrl ("employee_id", employee.EmployeeID.ToString (), "EmployeeDetails"),
                        false, 
                        SecurityAccessLevel.View,
                        true, 
                        false
                    );

                    actions.Add (
                        GetNextActionID (), 
                        LocalizeString ("VCard.Action"),
                        ModuleActionType.ContentOptions, 
                        "", 
                        IconController.IconURL ("View"),
                        EditUrl ("employee_id", employee.EmployeeID.ToString (), "VCard"),
                        false,
                        SecurityAccessLevel.View,
                        true,
                        true
                    );
                }

                return actions;
            }
        }

        #endregion

        protected void repeaterPositions_ItemDataBound (object sender, RepeaterItemEventArgs e)
        {
            RepeaterPositionsLogic.ItemDataBound (this, sender, e);
        }

    }
    // class
}
// namespace

