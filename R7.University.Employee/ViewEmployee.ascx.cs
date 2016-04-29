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
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.Entities.Modules;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University;
using R7.University.Data;

namespace R7.University.Employee
{
    public partial class ViewEmployee: PortalModuleBase<EmployeeSettings>, IActionable
    {
        #region Properties

        private EmployeeInfo _employee;

        public EmployeeInfo Employee
        {
            get {
                if (_employee == null) {
                    // use module settings
                    _employee = GetEmployee ();
                }

                return _employee;
            }
        }

        #endregion

        protected EmployeeInfo GetEmployee ()
        {
            if (Settings.ShowCurrentUser) {
                var userId = TypeUtils.ParseToNullable<int> (Request.QueryString ["userid"]);
                if (userId != null)
                    return UniversityRepository.Instance.GetEmployeeByUserId (userId.Value);
            }

            return UniversityRepository.Instance.DataProvider.Get<EmployeeInfo> (Settings.EmployeeID);
        }

        #region Handlers

        /*
		/// <summary>
		/// Handles Init event for a control
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);

			//#if (DATACACHE)
			//AddActionHandler (ClearDataCache_Action);
			//#endif
		}*/

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            try {
                if (!IsPostBack || ViewState.Count == 0) { // Fix for issue #23
                    if (Cache_OnLoad ())
                        return;
					
                    IEnumerable<EmployeeAchievementInfo> achievements = null;

                    if (Employee == null) {
                        // employee isn't set or not found
                        if (IsEditable)
                            this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                    }
                    else if (!Employee.IsPublished) {
                        // employee isn't published
                        if (IsEditable)
                            this.Message ("EmployeeNotPublished.Text", MessageType.Warning, true);
                    }

                    var hasData = Employee != null;

                    // if we have something published to display
                    // then display module to common users
                    Cache_SetContainerVisible (hasData && Employee.IsPublished);
											
                    // display module only in edit mode
                    // only if we have published data to display
                    ContainerControl.Visible = IsEditable || (hasData && Employee.IsPublished);
											
                    // display module content only if it exists and published (or in edit mode)
                    var displayContent = hasData && (IsEditable || Employee.IsPublished);

                    panelEmployee.Visible = displayContent;
					
                    if (displayContent) {
                        if (Settings.AutoTitle)
                            EmployeeModuleHelper.UpdateModuleTitle (ModuleId, Employee.AbbrName);
						
                        // get employee achievements (titles) only then it about to display
                        achievements = UniversityRepository.Instance.DataProvider.GetObjects<EmployeeAchievementInfo> (CommandType.Text, 
                            "SELECT * FROM dbo.vw_University_EmployeeAchievements " +
                            "WHERE [EmployeeID] = @0 AND [IsTitle] = 1", Employee.EmployeeID);

                        // display employee info
                        Display (Employee, achievements);
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
        protected void Display (EmployeeInfo employee, IEnumerable<EmployeeAchievementInfo> achievements)
        {
            // occupied positions
            var occupiedPositions = UniversityRepository.Instance.DataProvider.GetObjects<OccupiedPositionInfoEx> (
                                        "WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC", employee.EmployeeID);
            
            if (occupiedPositions.Any ()) {
                repeaterPositions.DataSource = OccupiedPositionInfoEx.GroupByDivision (occupiedPositions);
                repeaterPositions.DataBind ();
            }
            else
                repeaterPositions.Visible = false;

            // Full name
            var fullName = employee.FullName;
            labelFullName.Text = fullName;

            EmployeePhotoLogic.Bind (employee, imagePhoto, Settings.PhotoWidth);

            var popupUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (), "EmployeeDetails");
				
            // alter popup window height
            linkPhoto.NavigateUrl = popupUrl;

            // Employee titles
            var titles = achievements.Select (ach => Utils.FirstCharToLower (ach.DisplayShortTitle));
			
            var strTitles = Utils.FormatList (", ", titles);
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
            var workingPlaceAndHours = Utils.FormatList (", ", employee.WorkingPlace, employee.WorkingHours);
            if (!string.IsNullOrWhiteSpace (workingPlaceAndHours))
                labelWorkingPlaceAndHours.Text = workingPlaceAndHours;
            else
                labelWorkingPlaceAndHours.Visible = false;

            /*
			// Working place
			if (!string.IsNullOrWhiteSpace (employee.WorkingPlace))
				labelWorkingPlace.Text = employee.WorkingPlace;
			else
				labelWorkingPlace.Visible = false;

			// Working hours
			if (!string.IsNullOrWhiteSpace (employee.WorkingHours))
				labelWorkingHours.Text = employee.WorkingHours;
			else
				labelWorkingHours.Visible = false;
			*/

            /*
			// WebSite
			if (!string.IsNullOrWhiteSpace (employee.WebSite))
			{
				// THINK: Do we have to check if WebSite starting with http:// or https://?
				linkWebSite.NavigateUrl = "http://" + employee.WebSite;
				linkWebSite.Text = employee.WebSite;
			}
			else
				linkWebSite.Visible = false;
			*/

            // WebSite
            if (!string.IsNullOrWhiteSpace (employee.WebSite)) {
                linkWebSite.NavigateUrl = employee.FormatWebSiteUrl;
                linkWebSite.Text = employee.FormatWebSiteLabel;
            }
            else
                linkWebSite.Visible = false;

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
            if (!Utils.IsNull<int> (employee.UserID))
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

                actions.Add (
                    GetNextActionID (), 
                    Localization.GetString ("AddEmployee.Action", this.LocalResourceFile),
                    ModuleActionType.AddContent, 
                    "", 
                    "", 
                    EditUrl ("EditEmployee"),
                    false, 
                    SecurityAccessLevel.Edit,
                    Employee == null,
                    false
                );

                if (Employee != null) {
                    // otherwise, add "edit" action
                    actions.Add (
                        GetNextActionID (), 
                        Localization.GetString ("EditEmployee.Action", this.LocalResourceFile),
                        ModuleActionType.EditContent, 
                        "", 
                        "", 
                        EditUrl ("employee_id", Employee.EmployeeID.ToString (), "EditEmployee"),
                        false, 
                        SecurityAccessLevel.Edit,
                        true, 
                        false
                    );

                    actions.Add (
                        GetNextActionID (), 
                        Localization.GetString ("Details.Action", this.LocalResourceFile),
                        ModuleActionType.ContentOptions, 
                        "", 
                        "", 
                        EditUrl ("employee_id", Employee.EmployeeID.ToString (), "EmployeeDetails"),
                        false, 
                        SecurityAccessLevel.View,
                        true, 
                        false
                    );

                    actions.Add (
                        GetNextActionID (), 
                        Localization.GetString ("VCard.Action", this.LocalResourceFile),
                        ModuleActionType.ContentOptions, 
                        "", 
                        "", 
                        EditUrl ("employee_id", Employee.EmployeeID.ToString (), "VCard"),
                        false,
                        SecurityAccessLevel.View,
                        true,
                        true
                    );
                }

                /*
				#if (DATACACHE)
				actions.Add (
					GetNextActionID (), 
					"Clear Data Cache", // Localization.GetString("ClearDataCache.Action", this.LocalResourceFile),
					"ClearDataCache.Action", "", 
					"/images/action_refresh.gif",
					"", //Utils.EditUrl (this, "VCard", "employee_id", EmployeeID.ToString ()),
					true,  // use action event
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					true, 
					false // open in new window
				);
				#endif*/

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

