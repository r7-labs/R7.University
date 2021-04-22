//
//  ViewEmployee.ascx.cs
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
using System.Web.Caching;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Modules;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.Urls;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Dnn;
using R7.University.Employees.Models;
using R7.University.Employees.Queries;
using R7.University.Employees.SharedLogic;
using R7.University.Employees.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Security;
using R7.University.SharedLogic;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.Employees
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

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo)); }
        }

        #region Get data

        EmployeeInfo _employee;

        protected EmployeeInfo GetEmployee ()
        {
            return _employee ?? (_employee = GetEmployee_Internal ());
        }

        protected EmployeeInfo GetEmployee_Internal ()
        {
            if (Settings.ShowCurrentUser) {
                return GetEmployee_FromCurrentUser ();
            }
            return GetEmployee_FromSettings ();
        }

        protected EmployeeInfo GetEmployee_FromSettings ()
        {
            return DataCache.GetCachedData<EmployeeInfo> (
                new CacheItemArgs ("//r7_University/Modules/Employee?ModuleId=" + ModuleId,
                    UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                (c) => GetEmployee_FromSettings_Internal ()
            );
        }

        protected EmployeeInfo GetEmployee_FromSettings_Internal ()
        {
            return new EmployeeQuery (ModelContext).SingleOrDefault (Settings.EmployeeID);
        }

        protected EmployeeInfo GetEmployee_FromCurrentUser ()
        {
            var userId = ParseHelper.ParseToNullable<int> (Request.QueryString ["userid"]);
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
                var employee = GetEmployee ();
                var hasData = employee != null;
                var now = HttpContext.Current.Timestamp;

                if (!hasData) {
                    // employee wasn't set or not found
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

                // display module only in edit mode and only if we have published data to display
                ContainerControl.Visible = IsEditable || (hasData && employee.IsPublished (now));

                // display module content only if it exists and published (or in edit mode)
                var displayContent = hasData && (IsEditable || employee.IsPublished (now));

                panelEmployee.Visible = displayContent;

                if (displayContent) {
                    if (Settings.AutoTitle) {
                        UniversityModuleHelper.UpdateModuleTitle (TabModuleId,
                            UniversityFormatHelper.AbbrName (employee.FirstName, employee.LastName, employee.OtherName)
                        );
                    }

                    // display employee info
                    Display (employee);
                }

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
            // occupied positions
            var positions = employee.Positions
                                    .OrderByDescending (op => op.Position.Weight)
                                    .GroupByDivision (HttpContext.Current.Timestamp, IsEditable);

            // TODO: Grey out not published divisions
            if (positions.Any ()) {
                repeaterPositions.DataSource = positions;
                repeaterPositions.DataBind ();
            }
            else {
                repeaterPositions.Visible = false;
            }

            // Full name
            var fullName = UniversityFormatHelper.FullName (employee.FirstName, employee.LastName, employee.OtherName);
            labelFullName.Text = fullName;

            EmployeePhotoLogic.Bind (employee, imagePhoto, Settings.PhotoWidth);

            var employeeDetailsUrl = UrlHelper.AdjustPopupUrl (
                UniversityUrlHelper.IESafeEditUrl (this, Request, "employee_id", employee.EmployeeID.ToString (), "EmployeeDetails"),
                responseRedirect: false
            );

            linkPhoto.NavigateUrl = employeeDetailsUrl;

            // Employee titles
            var titles = employee.Achievements
                .Select (ach => new EmployeeAchievementViewModel (ach, new ViewModelContext (this)))
                .Where (ach => ach.IsTitle)
                .Select (ach => UniversityFormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix).FirstCharToLower ());

            var strTitles = FormatHelper.JoinNotNullOrEmpty (", ", titles);
            if (!string.IsNullOrWhiteSpace (strTitles))
                labelAcademicDegreeAndTitle.Text = "&nbsp;&ndash; " + strTitles;
            else
                labelAcademicDegreeAndTitle.Visible = false;

            BindContacts (employee);
        }

        void BindContacts (IEmployee employee)
        {
            if (!string.IsNullOrWhiteSpace (employee.Phone))
                labelPhone.Text = employee.Phone;
            else
                labelPhone.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.CellPhone))
                labelCellPhone.Text = employee.CellPhone;
            else
                labelCellPhone.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.Fax))
                labelFax.Text = string.Format (Localization.GetString ("Fax.Format", LocalResourceFile), employee.Fax);
            else
                labelFax.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.Messenger))
                labelMessenger.Text = employee.Messenger;
            else
                labelMessenger.Visible = false;

            var workingPlaceAndHours = FormatHelper.JoinNotNullOrEmpty (", ", employee.WorkingPlace, employee.WorkingHours);
            if (!string.IsNullOrWhiteSpace (workingPlaceAndHours))
                labelWorkingPlaceAndHours.Text = workingPlaceAndHours;
            else
                labelWorkingPlaceAndHours.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.WebSite)) {
                linkWebSite.NavigateUrl = UniversityFormatHelper.FormatWebSiteUrl (employee.WebSite);
                linkWebSite.Text = UniversityFormatHelper.FormatWebSiteLabel (employee.WebSite, employee.WebSiteLabel);
            }
            else {
                linkWebSite.Visible = false;
            }

            if (!string.IsNullOrWhiteSpace (employee.Email)) {
                linkEmail.NavigateUrl = "mailto:" + employee.Email;
                linkEmail.Text = employee.Email;
            }
            else
                linkEmail.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.SecondaryEmail)) {
                linkSecondaryEmail.NavigateUrl = "mailto:" + employee.SecondaryEmail;
                linkSecondaryEmail.Text = employee.SecondaryEmail;
            }
            else
                linkSecondaryEmail.Visible = false;

            if (employee.UserID != null && !Null.IsNull (employee.UserID.Value))
                linkUserProfile.NavigateUrl = Globals.UserProfileURL (employee.UserID.Value);
            else
                linkUserProfile.Visible = false;
        }

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
                var actions = new ModuleActionCollection ();
                var employee = GetEmployee ();

                actions.Add (
                    GetNextActionID (),
                    LocalizeString ("AddEmployee.Action"),
                    ModuleActionType.AddContent,
                    "",
                    UniversityIcons.Add,
                    EditUrl ("EditEmployee"),
                    false,
                    SecurityAccessLevel.Edit,
                    employee == null && SecurityContext.CanAdd (typeof (EmployeeInfo)),
                    false
                );

                actions.Add (
                    GetNextActionID (),
                    LocalizeString ("EditEmployee.Action"),
                    ModuleActionType.EditContent,
                    "",
                    UniversityIcons.Edit,
                    EditUrl ("employee_id", employee?.EmployeeID.ToString (), "EditEmployee"),
                    false,
                    SecurityAccessLevel.Edit,
                    employee != null,
                    false
                );

                actions.Add (
                    GetNextActionID (),
                    LocalizeString ("Details.Action"),
                    ModuleActionType.ContentOptions,
                    "",
                    UniversityIcons.Details,
                    EditUrl ("employee_id", employee?.EmployeeID.ToString (), "EmployeeDetails"),
                    false,
                    SecurityAccessLevel.View,
                    employee != null,
                    false
                );

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

