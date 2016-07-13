//
//  ViewEmployeeDetails.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
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
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.TextExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.Employee.Components;
using R7.University.Employee.Queries;
using R7.University.Employee.SharedLogic;
using R7.University.Employee.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.SharedLogic;
using R7.University.ViewModels;
using DnnUrlUtils = DotNetNuke.Common.Utilities.UrlUtils;

namespace R7.University.Employee
{
    public partial class ViewEmployeeDetails: PortalModuleBase<EmployeeSettings>, IActionable
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

        protected bool InPopup
        {
            get { return Request.QueryString ["popup"] != null; }
        }

        protected bool InViewModule
        {
            get { return ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.EmployeeDetails"; }
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

        #endregion

        protected EmployeeInfo GetEmployee ()
        {
            if (Settings.ShowCurrentUser) {
                var userId = TypeUtils.ParseToNullable<int> (Request.QueryString ["userid"]);
                if (userId != null) {
                    return new EmployeeQuery (ModelContext).ByUserId (userId.Value);
                }
            }

            return new EmployeeQuery (ModelContext).SingleOrDefault (Settings.EmployeeID);
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
                    LocalizeString ("AddEmployee.Action"),
                    ModuleActionType.AddContent,
                    "",
                    IconController.IconURL ("Add"),
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
                        LocalizeString ("EditEmployee.Action"),
                        ModuleActionType.EditContent, 
                        "", 
                        IconController.IconURL ("Edit"), 
                        EditUrl ("employee_id", Employee.EmployeeID.ToString (), "EditEmployee"),
                        false, 
                        SecurityAccessLevel.Edit,
                        true, 
                        false
                    );

                    actions.Add (
                        GetNextActionID (), 
                        LocalizeString ("VCard.Action"),
                        ModuleActionType.ContentOptions, 
                        "", 
                        IconController.IconURL ("View"), 
                        EditUrl ("employee_id", Employee.EmployeeID.ToString (), "VCard"),
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

        #region Handlers

        /// <summary>
        /// Handles Init event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            if (InPopup) {
                linkReturn.Attributes.Add ("onclick", "javascript:return " +
                    DnnUrlUtils.ClosePopUp (refresh: false, url: "", onClickEvent: true));
            }
            else if (InViewModule) {
                linkReturn.Visible = false;
            }
            else {
                linkReturn.NavigateUrl = Globals.NavigateURL ();
            }

            gridEduPrograms.LocalizeColumns (LocalResourceFile);
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
                    // can we display module content?
                    var displayContent = Employee != null && (IsEditable || Employee.IsPublished ());

                    // can we display something (content or messages)?
                    var displaySomething = IsEditable || (Employee != null && Employee.IsPublished ());

                    // something went wrong in popup mode - reload page
                    if (InPopup && !displaySomething) {
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
                        else if (!Employee.IsPublished ()) {
                            // employee don't published
                            this.Message ("EmployeeNotPublished.Text", MessageType.Warning, true);
                        }
                    }

                    panelEmployeeDetails.Visible = displayContent;

                    if (displayContent) {
                        Display (Employee);
						
                        // don't show action buttons in view module
                        if (!InViewModule) {
                            // show vCard button only for editors
                            if (IsEditable) {
                                linkVCard.Visible = true;
                                linkVCard.NavigateUrl = EditUrl (
                                    "employee_id",
                                    Employee.EmployeeID.ToString (),
                                    "VCard");
                            }

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
            // TODO: Move to extension methods
            Response.Redirect (Globals.NavigateURL (), false);
            Context.ApplicationInstance.CompleteRequest ();
        }

        protected void Display (EmployeeInfo employee)
        {
            var fullname = employee.FullName;

            if (InPopup) {
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
            var occupiedPositions = employee.Positions.ToList ();
            if (occupiedPositions.Count > 0) {
                repeaterPositions.DataSource = occupiedPositions.GroupByDivision (); 
                repeaterPositions.DataBind ();
            }
            else
                repeaterPositions.Visible = false;
			
            EmployeePhotoLogic.Bind (employee, imagePhoto, Settings.PhotoWidth);
					
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
                linkAbout.Visible = false;
            }
			
            Experience (employee);
            EduPrograms (employee);
            Barcode (employee);
        }

        void EduPrograms (EmployeeInfo employee)
        {
            // get employee edu programs
            var disciplines = employee.Disciplines.OrderBy (ed => ed.EduProgramProfile.EduProgram.Code);

            if (disciplines.Any ()) {
                gridEduPrograms.DataSource = disciplines.Select (ed => new EmployeeDisciplineViewModel (ed));
                gridEduPrograms.DataBind ();
            }
            else {
                linkDisciplines.Visible = false;
            }
        }

        void Barcode (EmployeeInfo employee)
        {
            if (employee.ShowBarcode) {
                linkBarcode.Attributes.Add ("data-module-id", ModuleId.ToString ());
                linkBarcode.Attributes.Add ("data-dialog-title", employee.FullName);

                // barcode image
                var barcodeWidth = UniversityConfig.Instance.Barcode.DefaultWidth;
                imageBarcode.ImageUrl = R7.University.Utilities.UrlUtils.FullUrl (string.Format (
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
                    LocalizeString ("ExperienceYears.Format1"), employee.ExperienceYears.Value);
            }
            else if (!exp1 && exp2) {
                labelExperienceYears.Text = string.Format (
                    LocalizeString ("ExperienceYears.Format2"), employee.ExperienceYearsBySpec);
            }
            else if (exp1 && exp2) {
                labelExperienceYears.Text = string.Format (
                    LocalizeString ("ExperienceYears.Format3"), 
                    employee.ExperienceYears.Value, employee.ExperienceYearsBySpec);
            }
            else {
                // hide label for experience years
                labelExperienceYears.Visible = false;
				
                // about to hide Experience tab
                noExpYears = true;
            }

            // get all empoyee achievements
            var achievements = employee.Achievements;
            
            // employee titles
            var titles = achievements.Where (ach => ach.IsTitle)
                .Select (ach => R7.University.Utilities.Utils.FirstCharToLower (ach.Title));
            
            var strTitles = TextUtils.FormatList (", ", titles);
            if (!string.IsNullOrWhiteSpace (strTitles))
                labelAcademicDegreeAndTitle.Text = strTitles.FirstCharToUpper ();
            else
                labelAcademicDegreeAndTitle.Visible = false;

            // get only experience-related achievements
            var experiences = achievements
                .Where (ach => ach.AchievementType == AchievementType.Education ||
                                  ach.AchievementType == AchievementType.AcademicDegree ||
                                  ach.AchievementType == AchievementType.Training ||
                                  ach.AchievementType == AchievementType.Work)
                .OrderByDescending (exp => exp.YearBegin);

            var viewModelContext = new ViewModelContext (this);

            if (experiences.Any ()) {
                gridExperience.DataSource = experiences.Select (exp => new EmployeeAchievementViewModel (exp, viewModelContext));
                gridExperience.DataBind ();
            }
            else if (noExpYears) {
                // hide experience tab
                linkExperience.Visible = false;
            }
		
            // get all other achievements
            achievements = achievements
                .Where (ach => ach.AchievementType != AchievementType.Education &&
                ach.AchievementType != AchievementType.AcademicDegree &&
                ach.AchievementType != AchievementType.Training &&
                ach.AchievementType != AchievementType.Work)
                .OrderByDescending (ach => ach.YearBegin)
                .ToList ();
			
            if (achievements.Any ()) {
                gridAchievements.DataSource = achievements.Select (ach => new EmployeeAchievementViewModel (ach, viewModelContext));
                gridAchievements.DataBind ();
            }
            else {	
                // hide achievements tab
                linkAchievements.Visible = false;
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
