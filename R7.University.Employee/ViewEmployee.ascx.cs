//
// ViewEmployee.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Components;
using R7.University.Data;
using R7.University.Employee.Components;
using R7.University.Employee.SharedLogic;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.SharedLogic;
using R7.University.ViewModels;

namespace R7.University.Employee
{
    public partial class ViewEmployee: PortalModuleBase<EmployeeSettings>, IActionable
    {
        #region Get data

        protected IEmployee GetEmployee ()
        {
            if (Settings.ShowCurrentUser) {
                return GetEmployee_CurrentUser_Internal ();
            }

            return DataCache.GetCachedData<IEmployee> (new CacheItemArgs ("//r7_University/Employee?ModuleId=" + ModuleId,
                UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetEmployee_Internal ()
            );
        }

        protected IEmployee GetEmployee_Internal ()
        {
            return EmployeeRepository.Instance.GetEmployee (Settings.EmployeeID)
                .WithAchievements ()
                .WithOccupiedPositions ();
        }

        protected IEmployee GetEmployee_CurrentUser_Internal ()
        {
            var userId = TypeUtils.ParseToNullable<int> (Request.QueryString ["userid"]);
            if (userId != null) {
                return EmployeeRepository.Instance.GetEmployee_ByUserId (userId.Value)
                    .WithAchievements ()
                    .WithOccupiedPositions ();
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

                    if (employee == null) {
                        // employee isn't set or not found
                        if (IsEditable) {
                            this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                        }
                    }
                    else if (!employee.IsPublished ()) {
                        // employee isn't published
                        if (IsEditable) {
                            this.Message ("EmployeeNotPublished.Text", MessageType.Warning, true);
                        }
                    }

                    var hasData = employee != null;
                    						
                    // display module only in edit mode
                    // only if we have published data to display
                    ContainerControl.Visible = IsEditable || (hasData && employee.IsPublished ());
											
                    // display module content only if it exists and published (or in edit mode)
                    var displayContent = hasData && (IsEditable || employee.IsPublished ());

                    panelEmployee.Visible = displayContent;
					
                    if (displayContent) {
                        if (Settings.AutoTitle) {
                            EmployeeModuleHelper.UpdateModuleTitle (ModuleId, 
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
            if (employee.OccupiedPositions.Any ()) {
                repeaterPositions.DataSource = employee.OccupiedPositions;
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
                .Where (ach => ach.IsTitle)
                .Select (ach => R7.University.Utilities.Utils.FirstCharToLower (ach.DisplayShortTitle));
			
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
                    "", 
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
                        "", 
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
                        "", 
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
                        "", 
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

