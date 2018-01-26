//
//  ViewEmployeeDetails.ascx.cs
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
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Framework;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.ModuleExtensions;
using R7.Dnn.Extensions.Modules;
using R7.Dnn.Extensions.TextExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
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
    public partial class ViewEmployeeDetails : PortalModuleBase<EmployeeSettings>, IActionable
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

        #region Properties

        protected int PhotoWidth
        {
            get {
                switch (ModuleConfiguration.ModuleDefinition.DefinitionName) {
                    case "R7_University_Employee": return Settings.PhotoWidth;
                    case "R7_University_EmployeeDetails": return Settings.PhotoWidth;
                    default: return UniversityConfig.Instance.EmployeePhoto.DefaultWidth;
                }
            }
        }
       
        protected bool IsInPopup
        {
            get { return UrlUtils.InPopUp (); }
        }

        protected bool InViewModule
        {
            get { return ModuleConfiguration.ModuleDefinition.DefinitionName == "R7_University_EmployeeDetails"; }
        }

        #endregion

        #region Get data

        EmployeeInfo _employee;

        public EmployeeInfo GetEmployee ()
        {
            return _employee ?? (_employee = GetEmployee_Internal ());
        }

        protected EmployeeInfo GetEmployee_Internal ()
        {
            if (InViewModule) {
                if (Settings.ShowCurrentUser) {
                    return GetEmployee_FromCurrentUser ();
                }
                return GetEmployee_FromSettings ();
            }

            var employee = GetEmployee_FromQueryString ();
            if (employee != null) {
                return employee;
            }

            if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.Employees") {
               return GetEmployee_FromSettings_Internal ();
            }

            return null;
        }

        protected EmployeeInfo GetEmployee_FromSettings ()
        {
            var cacheKey = $"//r7_University/Modules/Employee?ModuleId={ModuleId}";
            return DataCache.GetCachedData<EmployeeInfo> (
                new CacheItemArgs (cacheKey, UniversityConfig.Instance.DataCacheTime),
                (c) => GetEmployee_FromSettings_Internal ()
            );
        }

        protected EmployeeInfo GetEmployee_FromSettings_Internal ()
        {
            return new EmployeeQuery (ModelContext).SingleOrDefault (Settings.EmployeeID);
        }

        protected EmployeeInfo GetEmployee_FromQueryString ()
        {
            var employeeId = TypeUtils.ParseToNullable<int> (Request.QueryString ["employee_id"]);
            if (employeeId != null) {
                return new EmployeeQuery (ModelContext).SingleOrDefault (employeeId.Value);
            }
            return null;
        }

        protected EmployeeInfo GetEmployee_FromCurrentUser ()
        {
            var userId = TypeUtils.ParseToNullable<int> (Request.QueryString ["userid"]);
            if (userId != null) {
                return new EmployeeQuery (ModelContext).SingleOrDefaultByUserId (userId.Value);
            }
            return null;
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
                    EditUrl ("EditEmployee"),
                    false, 
                    SecurityAccessLevel.Edit,
                    GetEmployee () == null && SecurityContext.CanAdd (typeof (EmployeeInfo)), 
                    false
                );

                actions.Add (
                    GetNextActionID (), 
                    LocalizeString ("EditEmployee.Action"),
                    ModuleActionType.EditContent, 
                    "", 
                    UniversityIcons.Edit,
                    EditUrl ("employee_id", GetEmployee ()?.EmployeeID.ToString (), "EditEmployee"),
                    false, 
                    SecurityAccessLevel.Edit,
                    GetEmployee () != null, 
                    false
                );

                return actions;
            }
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles Init event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            if (InViewModule) {
                linkReturn.Visible = false;
            }
            else {
                linkReturn.NavigateUrl = UrlHelper.GetCancelUrl (UrlUtils.InPopUp ());
            }

            agplSignature.Visible = IsInPopup;

            gridDisciplines.LocalizeColumnHeaders (LocalResourceFile);
            gridExperience.LocalizeColumnHeaders (LocalResourceFile);
            gridAchievements.LocalizeColumnHeaders (LocalResourceFile);
        }

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            try {
                var now = HttpContext.Current.Timestamp;
                var employee = GetEmployee ();

                // can we display module content?
                var displayContent = employee != null && (IsEditable || employee.IsPublished (now));

                // can we display something (content or messages)?
                var displaySomething = IsEditable || (employee != null && employee.IsPublished (now));

                // something went wrong in popup mode - reload page
                if (IsInPopup && !displaySomething) {
                    ReloadPage ();
                    return;
                }

                if (InViewModule) {
                    // display module only in edit mode
                    // only if we have published data to display
                    ContainerControl.Visible = displaySomething;
                }

                // display messages
                if (IsEditable) {
                    if (employee == null) {
                        // employee isn't set or not found
                        this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                    }
                    else if (!employee.IsPublished (now)) {
                        // employee don't published
                        this.Message ("EmployeeNotPublished.Text", MessageType.Warning, true);
                    }
                }

                panelEmployeeDetails.Visible = displayContent;

                if (displayContent) {
                    Display (employee);
					
                    // don't show action buttons in view module
                    if (!InViewModule) {
                        // show edit button only for editors or superusers (in popup)
                        if (IsEditable || UserInfo.IsSuperUser) {
                            linkEdit.Visible = true;
                            linkEdit.NavigateUrl = EditUrl (
                                "employee_id",
                                employee.EmployeeID.ToString (),
                                "EditEmployee");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        /// <summary>
        /// Reloads the page.
        /// </summary>
        protected void ReloadPage ()
        {
            Response.Redirect (Globals.NavigateURL (), false);
            Context.ApplicationInstance.CompleteRequest ();
        }

        protected void Display (EmployeeInfo employee)
        {
            var fullname = employee.FullName;

            if (IsInPopup) {
                // set popup title to employee name
                ((CDefault) this.Page).Title = fullname;
            }
            else if (InViewModule) {
                if (Settings.AutoTitle)
                    UniversityModuleHelper.UpdateModuleTitle (TabModuleId, employee.FullName);
            }
            else {
                // display employee name in label
                literalFullName.Text = "<h2>" + fullname + "</h2>";
            }

            // occupied positions
            var positions = employee.Positions
                              .OrderByDescending (op => op.Position.Weight)
                              .GroupByDivision ()
                              .Where (p => IsEditable || p.OccupiedPosition.Division.IsPublished (HttpContext.Current.Timestamp));
            
            if (positions.Any ()) {
                repeaterPositions.DataSource = positions;
                repeaterPositions.DataBind ();
            }
            else {
                panelPositions.Visible = false;
            }

            EmployeePhotoLogic.Bind (employee, imagePhoto, PhotoWidth);
					
            BindContacts (employee);
            BindBarcode (employee);

            BindExperience (employee);
            BindDisciplines (employee);

            // about
            if (!string.IsNullOrWhiteSpace (employee.Biography))
                litAbout.Text = Server.HtmlDecode (employee.Biography);
            else {
                // hide entire About tab
                tabAbout.Visible = false;
            }
        }

        void BindContacts (IEmployee employee)
        {
            var displayContacts = false;

            if (!string.IsNullOrWhiteSpace (employee.Phone)) {
                labelPhone.Text = employee.Phone;
                displayContacts = true;
            }
            else
                labelPhone.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.CellPhone)) {
                labelCellPhone.Text = employee.CellPhone;
                displayContacts = true;
            }
            else
                labelCellPhone.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.Fax)) {
                labelFax.Text = string.Format (Localization.GetString ("Fax.Format", LocalResourceFile), employee.Fax);
                displayContacts = true;
            }
            else
                labelFax.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.Messenger)) {
                displayContacts = true;
                labelMessenger.Text = employee.Messenger;
            }
            else
                labelMessenger.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.Email)) {
                linkEmail.NavigateUrl = "mailto:" + employee.Email;
                linkEmail.Text = employee.Email;
                displayContacts = true;
            }
            else
                linkEmail.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.SecondaryEmail)) {
                linkSecondaryEmail.NavigateUrl = "mailto:" + employee.SecondaryEmail;
                linkSecondaryEmail.Text = employee.SecondaryEmail;
                displayContacts = true;
            }
            else
                linkSecondaryEmail.Visible = false;

            if (!string.IsNullOrWhiteSpace (employee.WebSite)) {
                linkWebSite.NavigateUrl = FormatHelper.FormatWebSiteUrl (employee.WebSite);
                linkWebSite.Text = FormatHelper.FormatWebSiteLabel (employee.WebSite, employee.WebSiteLabel);
                displayContacts = true;
            }
            else {
                linkWebSite.Visible = false;
            }

            if (!TypeUtils.IsNull<int> (employee.UserID)) {
                linkUserProfile.NavigateUrl = Globals.UserProfileURL (employee.UserID.Value);
                displayContacts = true;
            }
            else
                linkUserProfile.Visible = false;

            var workingPlaceAndHours = TextUtils.FormatList (", ", employee.WorkingPlace, employee.WorkingHours);
            if (!string.IsNullOrWhiteSpace (workingPlaceAndHours)) {
                labelWorkingPlaceAndHours.Text = workingPlaceAndHours;
                displayContacts = true;
            }
            else
                labelWorkingPlaceAndHours.Visible = false;

            panelContacts.Visible = displayContacts;
        }

        void BindDisciplines (IEmployee employee)
        {
            var now = HttpContext.Current.Timestamp;

            // get employee disciplines
            var disciplines = employee.Disciplines
                                      .Where (ed => ed.EduProgramProfile.IsPublished (now))
                                      .OrderBy (ed => ed.EduProgramProfile.EduProgram.Code);

            if (disciplines.Any ()) {
                gridDisciplines.DataSource = disciplines.Select (ed => new EmployeeDisciplineViewModel (ed));
                gridDisciplines.DataBind ();
            }
            else {
                tabDisciplines.Visible = false;
            }
        }

        // TODO: Use IEmployee
        void BindBarcode (EmployeeInfo employee)
        {
            if (employee.ShowBarcode) {
                labelBarcodeEmployeeName.Text = employee.FullName;
                linkBarcode.Attributes.Add ("data-target", "#employee-barcode-dialog-" + ModuleId);

                // barcode image
                var barcodeWidth = UniversityConfig.Instance.Barcode.DefaultWidth;
                imageBarcode.ImageUrl = UniversityUrlHelper.FullUrl (string.Format (
                        "/imagehandler.ashx?barcode=1&width={0}&height={1}&type=qrcode&encoding=UTF-8&content={2}",
                        barcodeWidth, barcodeWidth, 
                        Server.UrlEncode (employee.VCard.ToString ()
						.Replace ("+", "%2b")) // fix for "+" signs in phone numbers
                    ));

                imageBarcode.ToolTip = LocalizeString ("imageBarcode.ToolTip");
                imageBarcode.AlternateText = LocalizeString ("imageBarcode.AlternateText");
            }
            else {
                linkBarcode.Visible = false;
            }
        }

        void BindExperience (IEmployee employee)
        {
            // experience years
            var exp1 = false;
            var exp2 = false;
            var noExpYears = false;
		
            if (employee.ExperienceYears != null && employee.ExperienceYears.Value > 0)
                exp1 = true;
			
            if (employee.ExperienceYearsBySpec != null && employee.ExperienceYearsBySpec.Value > 0)
                exp2 = true;
			
            if (exp1 && !exp2) {
                labelExperienceYears.Text = string.Format (
                    LocalizeString ("ExperienceYears1.Format"), employee.ExperienceYears.Value);
            }
            else if (!exp1 && exp2) {
                labelExperienceYears.Text = string.Format (
                    LocalizeString ("ExperienceYears2.Format"), employee.ExperienceYearsBySpec);
            }
            else if (exp1 && exp2) {
                labelExperienceYears.Text = string.Format (
                    LocalizeString ("ExperienceYears3.Format"), 
                    employee.ExperienceYears.Value, employee.ExperienceYearsBySpec);
            }
            else {
                // hide label for experience years
                labelExperienceYears.Visible = false;
				
                // about to hide Experience tab
                noExpYears = true;
            }

            var viewModelContext = new ViewModelContext (this);

            // get all empoyee achievements
            var achievements = employee.Achievements.Select (ach => new EmployeeAchievementViewModel (ach, viewModelContext));
            
            // employee titles
            var titles = achievements.Where (ach => ach.IsTitle)
                                     .Select (ach => TextUtils.FormatList (" ", ach.Title.FirstCharToLower (), ach.TitleSuffix));
            
            var strTitles = TextUtils.FormatList (", ", titles);
            if (!string.IsNullOrWhiteSpace (strTitles))
                labelAcademicDegreeAndTitle.Text = strTitles.FirstCharToUpper ();
            else
                labelAcademicDegreeAndTitle.Visible = false;

            // get only experience-related achievements
            var experiences = achievements
                .Where (ach => ach.AchievementType.IsOneOf (SystemAchievementType.Education,
                                                            SystemAchievementType.AcademicDegree,
                                                            SystemAchievementType.Training,
                                                            SystemAchievementType.ProfTraining,
                                                            SystemAchievementType.ProfRetraining,
                                                            SystemAchievementType.Work))
                .OrderByDescending (exp => exp.YearBegin);
            
            if (experiences.Any ()) {
                gridExperience.DataSource = experiences;
                gridExperience.DataBind ();
            }
            else if (noExpYears) {
                // hide experience tab
                tabExperience.Visible = false;
            }
		
            // get all other achievements
            var otherAchievements = achievements
                .Where (ach => !ach.AchievementType.IsOneOf (SystemAchievementType.Education,
                                                             SystemAchievementType.AcademicDegree,
                                                             SystemAchievementType.Training,
                                                             SystemAchievementType.ProfTraining,
                                                             SystemAchievementType.ProfRetraining,
                                                             SystemAchievementType.Work))
                .OrderByDescending (ach => ach.YearBegin)
                .ToList ();
			
            if (otherAchievements.Any ()) {
                gridAchievements.DataSource = otherAchievements;
                gridAchievements.DataBind ();
            }
            else {	
                // hide achievements tab
                tabAchievements.Visible = false;
            }
        }

        protected void grid_RowCreated (object sender, GridViewRowEventArgs e)
        {
            // table header row should be inside <thead> tag
            if (e.Row.RowType == DataControlRowType.Header) {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void repeaterPositions_ItemDataBound (object sender, RepeaterItemEventArgs e)
        {
            RepeaterPositionsLogic.ItemDataBound (this, sender, e);
        }
    }
}
