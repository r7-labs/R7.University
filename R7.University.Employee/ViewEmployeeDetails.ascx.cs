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
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Framework;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.ModuleExtensions;
using R7.Dnn.Extensions.Modules;
using R7.Dnn.Extensions.TextExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.Employee.Components;
using R7.University.Employee.Queries;
using R7.University.Employee.SharedLogic;
using R7.University.Employee.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Security;
using R7.University.SharedLogic;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.Employee
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

        private EmployeeInfo _employee;

        public EmployeeInfo Employee
        {
            get {
                if (_employee == null) {
                    if (InViewModule) {
                        // use module settings
                        _employee = GetEmployee ();
                    }
                    else {
                        // try get employee id from querystring first
                        var employeeId = TypeUtils.ParseToNullable<int> (Request.QueryString ["employee_id"]);

                        if (employeeId != null) {
                            // get employee by querystring param
                            _employee = new EmployeeQuery (ModelContext).SingleOrDefault (employeeId.Value);
                        }
                        else if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.Employee") {
                            // if employee id is not in the querystring, 
                            // use module settings (Employee module only)
                            _employee = GetEmployee ();
                        }
                    }
                }

                return _employee;
            }
        }

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo)); }
        }

        #endregion

        protected EmployeeInfo GetEmployee ()
        {
            if (Settings.ShowCurrentUser) {
                var userId = TypeUtils.ParseToNullable<int> (Request.QueryString ["userid"]);
                if (userId != null) {
                    return new EmployeeQuery (ModelContext).SingleOrDefaultByUserId (userId.Value);
                }
            }

            return new EmployeeQuery (ModelContext).SingleOrDefault (Settings.EmployeeID);
        }

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
                    EditUrl ("EditEmployee"),
                    false, 
                    SecurityAccessLevel.Edit,
                    Employee == null && SecurityContext.CanAdd (typeof (EmployeeInfo)), 
                    false
                );

                actions.Add (
                    GetNextActionID (), 
                    LocalizeString ("EditEmployee.Action"),
                    ModuleActionType.EditContent, 
                    "", 
                    IconController.IconURL ("Edit"), 
                    EditUrl ("employee_id", Employee?.EmployeeID.ToString (), "EditEmployee"),
                    false, 
                    SecurityAccessLevel.Edit,
                    Employee != null, 
                    false
                );

                actions.Add (
                    GetNextActionID (), 
                    LocalizeString ("VCard.Action"),
                    ModuleActionType.ContentOptions, 
                    "", 
                    IconController.IconURL ("View"), 
                    EditUrl ("employee_id", Employee?.EmployeeID.ToString (), "VCard"),
                    false,
                    SecurityAccessLevel.View,
                    Employee != null,
                    NewWindow: true
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

            gridDisciplines.LocalizeColumns (LocalResourceFile);
            gridExperience.LocalizeColumns (LocalResourceFile);
            gridAchievements.LocalizeColumns (LocalResourceFile);
        }

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            try {
                if (!IsPostBack) {
                    var now = HttpContext.Current.Timestamp;

                    // can we display module content?
                    var displayContent = Employee != null && (IsEditable || Employee.IsPublished (now));

                    // can we display something (content or messages)?
                    var displaySomething = IsEditable || (Employee != null && Employee.IsPublished (now));

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
                        if (Employee == null) {
                            // employee isn't set or not found
                            this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                        }
                        else if (!Employee.IsPublished (now)) {
                            // employee don't published
                            this.Message ("EmployeeNotPublished.Text", MessageType.Warning, true);
                        }
                    }

                    panelEmployeeDetails.Visible = displayContent;

                    if (displayContent) {
                        Display (Employee);
						
                        // don't show action buttons in view module
                        if (!InViewModule) {
                            // show edit button only for editors or superusers (in popup)
                            if (IsEditable || UserInfo.IsSuperUser) {
                                linkEdit.Visible = true;
                                linkEdit.NavigateUrl = EditUrl (
                                    "employee_id",
                                    Employee.EmployeeID.ToString (),
                                    "EditEmployee");
                            }
                        }
                    }

                } // if (!IsPostBack)
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
                    ModuleHelper.UpdateModuleTitle (TabModuleId, employee.FullName);
            }
            else {
                // display employee name in label
                literalFullName.Text = "<h2>" + fullname + "</h2>";
            }

            // occupied positions
            if (employee.Positions.Any ()) {
                repeaterPositions.DataSource = employee.Positions
                    .OrderByDescending (op => op.Position.Weight)
                    .GroupByDivision ();
                repeaterPositions.DataBind ();
            }
            else
                repeaterPositions.Visible = false;

            EmployeePhotoLogic.Bind (employee, imagePhoto, PhotoWidth);
					
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

            // about
            if (!string.IsNullOrWhiteSpace (employee.Biography))
                litAbout.Text = Server.HtmlDecode (employee.Biography);
            else {
                // hide entire About tab
                tabAbout.Visible = false;
            }
			
            Experience (employee);
            Disciplines (employee);
            Barcode (employee);
        }

        void Disciplines (EmployeeInfo employee)
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

        void Barcode (EmployeeInfo employee)
        {
            if (employee.ShowBarcode) {
                linkBarcode.Attributes.Add ("data-module-id", ModuleId.ToString ());
                linkBarcode.Attributes.Add ("data-dialog-title", employee.FullName);

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

        void Experience (EmployeeInfo employee)
        {
            // experience years
            var exp1 = false;
            var exp2 = false;
            var noExpYears = false;
			
            // Общий стаж работы (лет): {0}
            // Общий стаж работы по специальности (лет): {0}
            // Общий стаж работы (лет): {0}, из них по специальности: {1}
			
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
