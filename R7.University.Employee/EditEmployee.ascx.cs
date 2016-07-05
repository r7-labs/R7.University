//
//  EditEmployee.ascx.cs
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
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.Data;
using R7.University.Employee.Components;
using R7.University.Models;
using R7.University.SharedLogic;
using R7.University.Utilities;
using R7.University.Queries;

namespace R7.University.Employee
{
    public partial class EditEmployee: PortalModuleBase<EmployeeSettings>
    {
        #region Types

        public enum EditEmployeeTab
        {
            Common,
            Positions,
            Achievements,
            Disciplines,
            About
        }

        #endregion

        private int? itemId = null;

        #region Repository handling

        private UniversityDataRepository repository;
        protected UniversityDataRepository Repository
        {
            get { return repository ?? (repository = new UniversityDataRepository ()); }
        }

        public override void Dispose ()
        {
            if (repository != null) {
                repository.Dispose ();
            }

            base.Dispose ();
        }

        #endregion

        #region Properties

        protected EditEmployeeTab SelectedTab
        {
            get {
                // get postback initiator
                var eventTarget = Request.Form ["__EVENTTARGET"];

                if (!string.IsNullOrEmpty (eventTarget)) {
                    
                    if (eventTarget.Contains ("$" + buttonUserLookup.ID) ||
                        eventTarget.Contains ("$" + buttonPhotoLookup.ID)) {
                        ViewState ["SelectedTab"] = EditEmployeeTab.Common;
                        return EditEmployeeTab.Common;
                    }

                    if (eventTarget.Contains ("$" +  buttonCancelEditPosition.ID) ||
                        eventTarget.Contains ("$" +  buttonAddPosition.ID) ||
                        eventTarget.Contains ("$" +  gridOccupiedPositions.ID)) {
                        ViewState ["SelectedTab"] = EditEmployeeTab.Positions;
                        return EditEmployeeTab.Positions;
                    }

                    if (eventTarget.Contains ("$" +  buttonCancelEditAchievement.ID) ||
                        eventTarget.Contains ("$" +  buttonAddAchievement.ID) ||
                        eventTarget.Contains ("$" +  gridAchievements.ID) ||
                        eventTarget.Contains ("$" +  urlDocumentURL.ID) ||
                        eventTarget.Contains ("$" +  comboAchievement.ID)) {
                        ViewState ["SelectedTab"] = EditEmployeeTab.Achievements;
                        return EditEmployeeTab.Achievements;
                    }

                    if (eventTarget.Contains ("$" + buttonCancelEditDisciplines.ID) ||
                        eventTarget.Contains ("$" + buttonAddDisciplines.ID) ||
                        eventTarget.Contains ("$" + gridDisciplines.ID) ||
                        eventTarget.Contains ("$" + comboEduLevel.ID)) {
                        ViewState ["SelectedTab"] = EditEmployeeTab.Disciplines;
                        return EditEmployeeTab.Disciplines;
                    }
                }
                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                return (obj != null) ? (EditEmployeeTab) obj : EditEmployeeTab.Common;
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        private List<AchievementInfo> CommonAchievements
        {
            get { 
                var commonAchievements = ViewState ["commonAchievements"] as List<AchievementInfo>;
                if (commonAchievements == null) {
                    commonAchievements = UniversityRepository.Instance.DataProvider.GetObjects<AchievementInfo> ().ToList ();
                    ViewState ["commonAchievements"] = commonAchievements;
                }
				
                return commonAchievements;
            }
        }

        protected string EditIconUrl
        {
            get { return IconController.IconURL ("Edit"); }
        }

        protected string DeleteIconUrl
        {
            get { return IconController.IconURL ("Delete"); }
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles Init event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // set url for Cancel link
            linkCancel.NavigateUrl = Globals.NavigateURL ();

            // add confirmation dialog to delete button
            buttonDelete.Attributes.Add ("onClick", "javascript:return confirm('" +
                Localization.GetString ("DeleteItem") + "');");
           
            // setup filepicker
            pickerPhoto.FolderPath = UniversityConfig.Instance.EmployeePhoto.DefaultPath;
            pickerPhoto.FileFilter = Globals.glbImageFileTypes;

            checkShowBarcode.Checked = true;

            // add default item to user list
            comboUsers.Items.Add (new ListItem (LocalizeString ("NotSelected.Text"), Null.NullInteger.ToString ()));

            // init working hours
            WorkingHoursLogic.Init (this, comboWorkingHours);

            // if results are null or empty, lists were empty too

            var positions = Repository.Query<PositionInfo> ().OrderBy (p => p.Title).ToList ();

            var divisions = Repository.QueryDivisions ().ToList ();
            
            var commonAchievements = new List<AchievementInfo> (UniversityRepository.Instance.DataProvider.GetObjects<AchievementInfo> ()
                .OrderBy (a => a.Title));

            ViewState ["commonAchievements"] = commonAchievements;

            // add default items
            positions.Insert (0, new PositionInfo {
                Title = LocalizeString ("NotSelected.Text"), PositionID = Null.NullInteger
            });

            commonAchievements.Insert (0, new AchievementInfo {
                Title = LocalizeString ("NotSelected.Text"), AchievementID = Null.NullInteger
            });

            divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

            // bind positions
            comboPositions.DataSource = positions;
            comboPositions.DataBind ();
        
            // bind achievements
            comboAchievement.DataSource = commonAchievements;
            comboAchievement.DataBind ();
        
            // bind divisions
            treeDivisions.DataSource = divisions;
            treeDivisions.DataBind ();
            // select first (default) node - fix for issue #8
            treeDivisions.Nodes [0].Selected = true;

            // bind achievement types
            comboAchievementTypes.DataSource = AchievementTypeInfo.GetLocalizedAchievementTypes (LocalizeString);
            comboAchievementTypes.DataBind ();

            // get and bind edu levels
            var eduLevels = new EduProgramLevelsQuery (Repository).Execute ();
            comboEduLevel.DataSource = eduLevels;
            comboEduLevel.DataBind ();

            // get and bind edu profiles
            BindEduProgramProfiles (eduLevels.First ().EduLevelID);

            // localize bounded gridviews
            gridAchievements.LocalizeColumns (LocalResourceFile);
            gridOccupiedPositions.LocalizeColumns (LocalResourceFile);
            gridDisciplines.LocalizeColumns (LocalResourceFile);
        }

        /// <summary>
        /// Handles Load event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
             
            try {
                // parse querystring parameters
                itemId = TypeUtils.ParseToNullable<int> (Request.QueryString ["employee_id"]);
      
                if (!IsPostBack) {
                    // load the data into the control the first time we hit this page

                    // check we have an item to lookup
                    // ALT: if (!Null.IsNull (itemId) 
                    if (itemId.HasValue) {
                        // load the item
                        var item = UniversityRepository.Instance.DataProvider.Get<EmployeeInfo> (itemId.Value);

                        if (item != null) {
                            textLastName.Text = item.LastName;
                            textFirstName.Text = item.FirstName;
                            textOtherName.Text = item.OtherName;
                            textPhone.Text = item.Phone;
                            textCellPhone.Text = item.CellPhone;
                            textFax.Text = item.Fax;
                            textEmail.Text = item.Email;
                            textSecondaryEmail.Text = item.SecondaryEmail;
                            textWebSite.Text = item.WebSite;
                            textWebSiteLabel.Text = item.WebSiteLabel;
                            textMessenger.Text = item.Messenger;
                            textWorkingPlace.Text = item.WorkingPlace;
                            textBiography.Text = item.Biography;
                            checkShowBarcode.Checked = item.ShowBarcode;
							
                            // load working hours
                            WorkingHoursLogic.Load (comboWorkingHours, textWorkingHours, item.WorkingHours);

                            if (!Null.IsNull (item.ExperienceYears))
                                textExperienceYears.Text = item.ExperienceYears.ToString ();

                            if (!Null.IsNull (item.ExperienceYearsBySpec))
                                textExperienceYearsBySpec.Text = item.ExperienceYearsBySpec.ToString ();

                            datetimeStartDate.SelectedDate = item.StartDate;
                            datetimeEndDate.SelectedDate = item.EndDate;

                            // set photo
                            if (!TypeUtils.IsNull (item.PhotoFileID)) {
                                var photo = FileManager.Instance.GetFile (item.PhotoFileID.Value);
                                if (photo != null) {
                                    pickerPhoto.FileID = photo.FileId;
                                }
                            }

                            if (!Null.IsNull (item.UserID)) {
                                var user = UserController.GetUserById (this.PortalId, item.UserID.Value);
                                if (user != null) {
                                    // add previously selected user to user list...
                                    comboUsers.Items.Add (new ListItem (
                                            user.Username + " / " + user.Email,
                                            user.UserID.ToString ()));
                                    comboUsers.SelectedIndex = 1;
                                }
                            }

                            // read OccupiedPositions data
                            var occupiedPositionInfoExs = UniversityRepository.Instance.DataProvider.GetObjects<OccupiedPositionInfoEx> (
                                                              "WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC", itemId.Value);

                            // fill view list
                            var occupiedPositions = new List<OccupiedPositionView> ();
                            foreach (var op in occupiedPositionInfoExs)
                                occupiedPositions.Add (new OccupiedPositionView (op));

                            // bind occupied positions
                            ViewState ["occupiedPositions"] = occupiedPositions;
                            gridOccupiedPositions.DataSource = OccupiedPositionsDataTable (occupiedPositions);
                            gridOccupiedPositions.DataBind ();

                            // read employee achievements
                            var achievementInfos = EmployeeAchievementRepository.Instance
                                .GetEmployeeAchievements (itemId.Value);

                            // fill achievements list
                            var achievements = new List<EmployeeAchievementView> ();
                            foreach (var achievement in achievementInfos) {
                                var achView = new EmployeeAchievementView (achievement);
                                achView.Localize (LocalResourceFile);
                                achievements.Add (achView);
                            }

                            // bind achievements
                            ViewState ["achievements"] = achievements;
                            gridAchievements.DataSource = AchievementsDataTable (achievements);
                            gridAchievements.DataBind ();

                            // read employee educational programs 
                            var disciplineInfos = UniversityRepository.Instance.DataProvider.GetObjects<EmployeeDisciplineInfoEx> (
                                                      "WHERE [EmployeeID] = @0", itemId.Value);

                            // fill disciplines list
                            var disciplines = new List<EmployeeDisciplineView> ();
                            foreach (var eduprogram in disciplineInfos)
                                disciplines.Add (new EmployeeDisciplineView (eduprogram));

                            // bind disciplines
                            ViewState ["disciplines"] = disciplines;
                            gridDisciplines.DataSource = DisciplinesDataTable (disciplines);
                            gridDisciplines.DataBind ();

                            // setup audit control
                            ctlAudit.Bind (item);
                        }
                        else
                            Response.Redirect (Globals.NavigateURL (), true);
                    }
                    else {
                        buttonDelete.Visible = false;
                        ctlAudit.Visible = false;
                    }

                    // then edit / add from EmployeeList, divisionId query param
                    // can be set to current division ID
                    var divisionId = Request.QueryString ["division_id"];
                    Utils.SelectAndExpandByValue (treeDivisions, divisionId);
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        /// <summary>
        /// Handles Click event for Update button
        /// </summary>
        /// <param name='sender'>
        /// Sender.
        /// </param>
        /// <param name='e'>
        /// Event args.
        /// </param>
        protected void buttonUpdate_Click (object sender, EventArgs e)
        {
            try {
                EmployeeInfo item;

                // determine if we are adding or updating
                // ALT: if (Null.IsNull (itemId))
                if (!itemId.HasValue) {
                    // to add new record
                    item = new EmployeeInfo ();
                }
                else {
                    // update existing record
                    item = UniversityRepository.Instance.DataProvider.Get<EmployeeInfo> (itemId.Value);
                }

                // fill the object
                item.LastName = textLastName.Text.Trim ();
                item.FirstName = textFirstName.Text.Trim ();
                item.OtherName = textOtherName.Text.Trim ();
                item.Phone = textPhone.Text.Trim ();
                item.CellPhone = textCellPhone.Text.Trim ();
                item.Fax = textFax.Text.Trim ();
                item.Email = textEmail.Text.Trim ().ToLowerInvariant ();
                item.SecondaryEmail = textSecondaryEmail.Text.Trim ().ToLowerInvariant ();
                item.WebSite = textWebSite.Text.Trim ();
                item.WebSiteLabel = textWebSiteLabel.Text.Trim ();
                item.Messenger = textMessenger.Text.Trim ();
                item.WorkingPlace = textWorkingPlace.Text.Trim ();
                item.Biography = textBiography.Text.Trim ();
                item.ShowBarcode = checkShowBarcode.Checked;
				
                // update working hours
                item.WorkingHours = WorkingHoursLogic.Update (comboWorkingHours, textWorkingHours.Text, 
                    checkAddToVocabulary.Checked);

                item.ExperienceYears = TypeUtils.ParseToNullable<int> (textExperienceYears.Text);
                item.ExperienceYearsBySpec = TypeUtils.ParseToNullable<int> (textExperienceYearsBySpec.Text);

                item.StartDate = datetimeStartDate.SelectedDate;
                item.EndDate = datetimeEndDate.SelectedDate;

                // pickerPhoto.FileID may be 0 by default
                item.PhotoFileID = (pickerPhoto.FileID > 0) ? (int?) pickerPhoto.FileID : null;
                item.UserID = TypeUtils.ParseToNullable<int> (comboUsers.SelectedValue);

                if (!itemId.HasValue) {		
                    // update audit info
                    item.CreatedByUserID = item.LastModifiedByUserID = this.UserId;
                    item.CreatedOnDate = item.LastModifiedOnDate = DateTime.Now;
	
                    // add employee
                    EmployeeRepository.Instance.AddEmployee (item, GetOccupiedPositions (), 
                        GetEmployeeAchievements (), GetEmployeeDisciplines ());

                    // then adding new employee from Employee or EmployeeDetails modules, 
                    // set calling module to display new employee
                    if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.Employee" ||
                    ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.EmployeeDetails") {
                        Settings.EmployeeID = item.EmployeeID;

                        // we adding new employee, so he/she should be displayed in the module
                        Settings.ShowCurrentUser = false;
                    }
                }
                else {
                    // update audit info
                    item.LastModifiedByUserID = UserId;
                    item.LastModifiedOnDate = DateTime.Now;

                    // update employee
                    EmployeeRepository.Instance.UpdateEmployee (item, GetOccupiedPositions (), 
                        GetEmployeeAchievements (), GetEmployeeDisciplines ());
                }

                ModuleController.SynchronizeModule (ModuleId);

                Response.Redirect (Globals.NavigateURL (), true);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        private List<OccupiedPositionInfo> GetOccupiedPositions ()
        {
            var occupiedPositions = ViewState ["occupiedPositions"] as List<OccupiedPositionView>;
					
            var occupiedPositionInfos = new List<OccupiedPositionInfo> ();
            if (occupiedPositions != null)
                foreach (var op in occupiedPositions)
                    occupiedPositionInfos.Add (op.NewOccupiedPositionInfo ());

            return occupiedPositionInfos;
        }

        private List<EmployeeAchievementInfo> GetEmployeeAchievements ()
        {
            var achievements = ViewState ["achievements"] as List<EmployeeAchievementView>;
				
            var achievementInfos = new List<EmployeeAchievementInfo> ();
            if (achievements != null)
                foreach (var ach in achievements)
                    achievementInfos.Add (ach.NewEmployeeAchievementInfo ());

            return achievementInfos;
        }

        private List<EmployeeDisciplineInfo> GetEmployeeDisciplines ()
        {
            var disciplines = ViewState ["disciplines"] as List<EmployeeDisciplineView>;

            var disciplineInfos = new List<EmployeeDisciplineInfo> ();
            if (disciplines != null)
                foreach (var ep in disciplines)
                    disciplineInfos.Add (ep.NewEmployeeDisciplineInfo ());

            return disciplineInfos;
        }

        /// <summary>
        /// Handles Click event for Delete button
        /// </summary>
        /// <param name='sender'>
        /// Sender.
        /// </param>
        /// <param name='e'>
        /// Event args.
        /// </param>
        protected void buttonDelete_Click (object sender, EventArgs e)
        {
            try {
                // ALT: if (!Null.IsNull (itemId))
                if (itemId.HasValue) {
                    EmployeeRepository.Instance.DeleteEmployee (itemId.Value);
                    Response.Redirect (Globals.NavigateURL (), true);
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void buttonUserLookup_Click (object sender, EventArgs e)
        {
            try {
                var term = textUserLookup.Text.Trim ();
                var includeDeleted = checkIncludeDeletedUsers.Checked;

                // uncheck to minimize UX errors
                checkIncludeDeletedUsers.Checked = false;

                var usersFound = 0;
                var usersFoundTotal = 0;
			
                // TODO: Link to open admin users interface in a separate tab
                var users = UserController.GetUsersByEmail (PortalId, term, -1, -1, 
                    ref usersFound, includeDeleted, false);
                usersFoundTotal += usersFound;

                // find cross-portal users (host & others) by email
                users.AddRange (UserController.GetUsersByEmail (Null.NullInteger, term, -1, -1, 
                        ref usersFound, includeDeleted, false));
                usersFoundTotal += usersFound;

                // combine email lookup results with lookup by username
                users.AddRange (UserController.GetUsersByUserName (PortalId, term, -1, -1, 
                        ref usersFound, includeDeleted, false));
                usersFoundTotal += usersFound;

                // find cross-portal users  by username
                users.AddRange (UserController.GetUsersByUserName (Null.NullInteger, term, -1, -1, 
                        ref usersFound, includeDeleted, false));
                usersFoundTotal += usersFound;

                // clear user combox & add default item
                comboUsers.Items.Clear ();
                comboUsers.Items.Add (new ListItem (LocalizeString ("NotSelected.Text"), Null.NullInteger.ToString ()));

                if (usersFoundTotal > 0) {
                    foreach (var userObj in users) {
                        var user = userObj as UserInfo;
                        comboUsers.Items.Add (new ListItem (
                                user.Username + " / " + user.Email,
                                user.UserID.ToString ()));
                    }

                    // at least one user exists, so select first one:
                    // listUsers.SelectedIndex = 1;
                    comboUsers.SelectedIndex = 1;
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void buttonPhotoLookup_Click (object sender, EventArgs e)
        {
            try {
                var folderPath =  UniversityConfig.Instance.EmployeePhoto.DefaultPath;
                var folder = FolderManager.Instance.GetFolder (PortalId, folderPath);

                if (folder != null) {
                    var employeeName = EmployeeInfo.GetFileName (textFirstName.Text, 
                                           textLastName.Text, textOtherName.Text);

                    // REVIEW: EmployeeInfo should contain culture data?
                    var employeeNameTL = R7.University.Utilities.TextUtils.Transliterate (employeeName, 
                        R7.University.Utilities.TextUtils.RuTranslitTable).ToLowerInvariant ();

                    // get files from default folder recursively
                    foreach (var file in FolderManager.Instance.GetFiles (folder, true)) {
                        var fileName = Path.GetFileNameWithoutExtension (file.FileName).ToLowerInvariant ();
                        if (fileName == employeeName || fileName == employeeNameTL) {
                            pickerPhoto.FileID = file.FileId;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void buttonCancelEditDisciplines_Click (object sender, EventArgs e)
        {
            try {
                ResetEditDisciplinesForm ();
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void buttonCancelEditPosition_Click (object sender, EventArgs e)
        {
            try {
                ResetEditPositionForm ();
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        private void ResetEditDisciplinesForm ()
        {
            // restore default buttons visibility
            buttonAddDisciplines.Visible = true;
            buttonUpdateDisciplines.Visible = false;
            textDisciplines.Text = string.Empty;
        }

        private void ResetEditPositionForm ()
        {
            // restore default buttons visibility
            buttonAddPosition.Visible = true;
            buttonUpdatePosition.Visible = false;

            // reset divisions treeview
            var divisionId = Request.QueryString ["division_id"];
            Utils.SelectAndExpandByValue (treeDivisions, 
                !string.IsNullOrWhiteSpace (divisionId) ? divisionId : Null.NullInteger.ToString ());
		
            // reset other controls
            comboPositions.SelectedIndex = 0;
            textPositionTitleSuffix.Text = "";
            checkIsPrime.Checked = false;
            hiddenOccupiedPositionItemID.Value = "";
        }

        protected void buttonAddPosition_Command (object sender, CommandEventArgs e)
        {
            try {
                var positionID = int.Parse (comboPositions.SelectedValue);
                var divisionID = int.Parse (treeDivisions.SelectedValue);

                if (!Null.IsNull (positionID) && !Null.IsNull (divisionID)) {
                    OccupiedPositionView occupiedPosition;

                    var occupiedPositions = ViewState ["occupiedPositions"] as List<OccupiedPositionView>;

                    // creating new list, if none
                    if (occupiedPositions == null)
                        occupiedPositions = new List<OccupiedPositionView> ();

                    var command = e.CommandArgument.ToString ();
                    if (command == "Add") {
                        occupiedPosition = new OccupiedPositionView ();
                    }
                    else { // update 
                        // restore ItemID from hidden field
                        var hiddenItemID = int.Parse (hiddenOccupiedPositionItemID.Value);
                        occupiedPosition = occupiedPositions.Find (op => op.ItemID == hiddenItemID);
                    }
					
                    // fill the object
                    occupiedPosition.PositionID = positionID;
                    occupiedPosition.DivisionID = divisionID;
                    occupiedPosition.PositionShortTitle = comboPositions.SelectedItem.Text;
                    occupiedPosition.DivisionShortTitle = treeDivisions.SelectedNode.Text;
                    occupiedPosition.IsPrime = checkIsPrime.Checked;
                    occupiedPosition.TitleSuffix = textPositionTitleSuffix.Text.Trim ();
					
                    if (command == "Add") {
                        occupiedPositions.Add (occupiedPosition);
                    }

                    ResetEditPositionForm ();

                    ViewState ["occupiedPositions"] = occupiedPositions;
                    gridOccupiedPositions.DataSource = OccupiedPositionsDataTable (occupiedPositions);
                    gridOccupiedPositions.DataBind ();
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void gridOccupiedPositions_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            grids_RowDataBound (sender, e);
        }

        protected void gridAchievements_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // hide description
            e.Row.Cells [6].Visible = false;

            // exclude header
            if (e.Row.RowType == DataControlRowType.DataRow) {
                // add description as row tooltip
                e.Row.ToolTip = Server.HtmlDecode (e.Row.Cells [6].Text);

                // make link to the document
                // WTF: empty DocumentURL's cells contains non-breakable spaces?
                var documentUrl = Server.HtmlDecode (e.Row.Cells [7].Text.Replace ("&nbsp;", ""));
                if (!string.IsNullOrWhiteSpace (documentUrl))
                    e.Row.Cells [7].Text = string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>", 
                        R7.University.Utilities.UrlUtils.LinkClickIdnHack (documentUrl, TabId, ModuleId),
                        LocalizeString ("DocumentUrl.Text"));
            }

            grids_RowDataBound (sender, e);
        }

        protected void gridDisciplines_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            grids_RowDataBound (sender, e);
        }

        private void grids_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // hide ItemID column, also in header
            e.Row.Cells [1].Visible = false;

            // exclude header
            if (e.Row.RowType == DataControlRowType.DataRow) {
                // find edit and delete linkbuttons
                var linkDelete = e.Row.Cells [0].FindControl ("linkDelete") as LinkButton;
                var linkEdit = e.Row.Cells [0].FindControl ("linkEdit") as LinkButton;

                // set recordId to delete
                linkEdit.CommandArgument = e.Row.Cells [1].Text;
                linkDelete.CommandArgument = e.Row.Cells [1].Text;

                // add confirmation dialog to delete link
                linkDelete.Attributes.Add ("onClick", "javascript:return confirm('" +
                    Localization.GetString ("DeleteItem") + "');");
            }
        }

        protected void linkEditOccupiedPosition_Command (object sender, CommandEventArgs e)
        {
            try {
                var occupiedPositions = ViewState ["occupiedPositions"] as List<OccupiedPositionView>;
                if (occupiedPositions != null) {
                    var itemID = e.CommandArgument.ToString ();
	
                    // find position in a list
                    var occupiedPosition = occupiedPositions.Find (op => op.ItemID.ToString () == itemID);
	
                    if (occupiedPosition != null) {
                        // fill the form
                        treeDivisions.CollapseAllNodes ();
                        Utils.SelectAndExpandByValue (treeDivisions, occupiedPosition.DivisionID.ToString ());
                        comboPositions.SelectByValue (occupiedPosition.PositionID);
                        checkIsPrime.Checked = occupiedPosition.IsPrime;
                        textPositionTitleSuffix.Text = occupiedPosition.TitleSuffix;
						
                        // set hidden field value to ItemID of edited item
                        hiddenOccupiedPositionItemID.Value = occupiedPosition.ItemID.ToString ();

                        // show / hide buttonss
                        buttonAddPosition.Visible = false;
                        buttonUpdatePosition.Visible = true;
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void linkDeleteOccupiedPosition_Command (object sender, CommandEventArgs e)
        {
            try {
                var occupiedPositions = ViewState ["occupiedPositions"] as List<OccupiedPositionView>;
                if (occupiedPositions != null) {
                    var itemID = e.CommandArgument.ToString ();
	
                    // find position in a list
                    var opFound = occupiedPositions.Find (op => op.ItemID.ToString () == itemID);
				
                    if (opFound != null) {
                        occupiedPositions.Remove (opFound);
                        ViewState ["occupiedPositions"] = occupiedPositions;
	
                        gridOccupiedPositions.DataSource = OccupiedPositionsDataTable (occupiedPositions);
                        gridOccupiedPositions.DataBind ();

                        // reset form if we deleting currently edited position
                        if (buttonUpdatePosition.Visible && hiddenOccupiedPositionItemID.Value == itemID)
                            ResetEditPositionForm ();
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        private DataTable OccupiedPositionsDataTable (List<OccupiedPositionView> occupiedPositions)
        {
            return DataTableConstructor.FromIEnumerable (occupiedPositions);
        }

        private DataTable AchievementsDataTable (List<EmployeeAchievementView> achievements)
        {
            return DataTableConstructor.FromIEnumerable (achievements);
        }

        private DataTable DisciplinesDataTable (List<EmployeeDisciplineView> eduPrograms)
        {
            return DataTableConstructor.FromIEnumerable (eduPrograms);
        }

        protected void linkDeleteAchievement_Command (object sender, CommandEventArgs e)
        {
            try {
                var achievements = ViewState ["achievements"] as List<EmployeeAchievementView>;
                if (achievements != null) {
                    var itemID = e.CommandArgument.ToString ();
	
                    // find position in a list
                    var achievement = achievements.Find (ach => ach.ItemID.ToString () == itemID);
	
                    if (achievement != null) {
                        // remove achievement
                        achievements.Remove (achievement);
						
                        // refresh viewstate
                        ViewState ["achievements"] = achievements;
	
                        // bind achievements to the gridview
                        gridAchievements.DataSource = AchievementsDataTable (achievements);
                        gridAchievements.DataBind ();

                        // reset form if we deleting currently edited achievement
                        if (buttonUpdateAchievement.Visible && hiddenAchievementItemID.Value == itemID)
                            ResetEditAchievementForm ();
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void linkEditAchievement_Command (object sender, CommandEventArgs e)
        {
            try {
                var achievements = ViewState ["achievements"] as List<EmployeeAchievementView>;
                if (achievements != null) {
                    var itemID = e.CommandArgument.ToString ();
	
                    // find position in a list
                    var achievement = achievements.Find (ach => ach.ItemID.ToString () == itemID);
	
                    if (achievement != null) {
                        // fill achievements form
						
                        if (achievement.AchievementID != null) {
                            comboAchievement.SelectByValue (achievement.AchievementID);
	
                            panelAchievementTitle.Visible = false;
                            panelAchievementShortTitle.Visible = false;
                            panelAchievementTypes.Visible = false;
                        }
                        else {
                            comboAchievement.SelectByValue (Null.NullInteger);
	
                            textAchievementTitle.Text = achievement.Title;
                            textAchievementShortTitle.Text = achievement.ShortTitle;
                            comboAchievementTypes.SelectByValue (achievement.AchievementType);
							
                            panelAchievementTitle.Visible = true;
                            panelAchievementShortTitle.Visible = true;
                            panelAchievementTypes.Visible = true;
                        }
	
                        textAchievementTitleSuffix.Text = achievement.TitleSuffix;
                        textAchievementDescription.Text = achievement.Description;
                        textYearBegin.Text = achievement.YearBegin.ToString ();
                        textYearEnd.Text = achievement.YearEnd.ToString ();
                        checkIsTitle.Checked = achievement.IsTitle;

                        if (!string.IsNullOrWhiteSpace (achievement.DocumentURL))
                            urlDocumentURL.Url = achievement.DocumentURL;
                        else
                            urlDocumentURL.UrlType = "N";

                        // show update and cancel buttons (enter edit mode)
                        buttonAddAchievement.Visible = false;
                        buttonUpdateAchievement.Visible = true;

                        // store ItemID in the hidden field
                        hiddenAchievementItemID.Value = achievement.ItemID.ToString ();
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void buttonCancelEditAchievement_Click (object sender, EventArgs e)
        {
            try {
                ResetEditAchievementForm ();
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        private void ResetEditAchievementForm ()
        {
            // restore default buttons visibility
            buttonAddAchievement.Visible = true;
            buttonUpdateAchievement.Visible = false;

            // restore default panels visibility
            panelAchievementTitle.Visible = true;
            panelAchievementShortTitle.Visible = true;
            panelAchievementTypes.Visible = true;

            // reset controls
            comboAchievement.SelectedIndex = 0;
            comboAchievementTypes.SelectedIndex = 0;
            textAchievementTitle.Text = "";
            textAchievementShortTitle.Text = "";
            textAchievementTitleSuffix.Text = "";
            textAchievementDescription.Text = "";
            textYearBegin.Text = "";
            textYearEnd.Text = "";
            checkIsTitle.Checked = false;
            urlDocumentURL.UrlType = "N";
            hiddenAchievementItemID.Value = "";
        }

        protected void buttonAddAchievement_Command (object sender, CommandEventArgs e)
        {
            try {
                EmployeeAchievementView achievement;

                // get achievements list from viewstate
                var achievements = ViewState ["achievements"] as List<EmployeeAchievementView>;
				
                // creating new list, if none
                if (achievements == null)
                    achievements = new List<EmployeeAchievementView> ();

                var command = e.CommandArgument.ToString ();
                if (command == "Add") {
                    achievement = new EmployeeAchievementView ();
                }
                else {
                    // restore ItemID from hidden field
                    var hiddenItemID = int.Parse (hiddenAchievementItemID.Value);
                    achievement = achievements.Find (ach => ach.ItemID == hiddenItemID);
                }
	
                achievement.AchievementID = TypeUtils.ParseToNullable<int> (comboAchievement.SelectedValue);
                if (achievement.AchievementID == null) {
                    achievement.Title = textAchievementTitle.Text.Trim ();
                    achievement.ShortTitle = textAchievementShortTitle.Text.Trim ();
                    achievement.AchievementType = (AchievementType) Enum.Parse (typeof (AchievementType), 
                        comboAchievementTypes.SelectedValue);
                }
                else {
                    var ach = CommonAchievements.Find (a => a.AchievementID.ToString () ==
                        comboAchievement.SelectedValue);

                    achievement.Title = ach.Title;
                    achievement.ShortTitle = ach.ShortTitle;
                    achievement.AchievementType = ach.AchievementType;
                }

                achievement.TitleSuffix = textAchievementTitleSuffix.Text.Trim ();
                achievement.Description = textAchievementDescription.Text.Trim ();
                achievement.IsTitle = checkIsTitle.Checked;
                achievement.YearBegin = TypeUtils.ParseToNullable<int> (textYearBegin.Text);
                achievement.YearEnd = TypeUtils.ParseToNullable<int> (textYearEnd.Text);
                achievement.DocumentURL = urlDocumentURL.Url;

                achievement.Localize (LocalResourceFile);

                if (command == "Add") {
                    achievements.Add (achievement);
                }

                ResetEditAchievementForm ();

                // refresh viewstate
                ViewState ["achievements"] = achievements;

                // bind achievements to the gridview
                gridAchievements.DataSource = AchievementsDataTable (achievements);
                gridAchievements.DataBind ();
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void buttonAddDisciplines_Command (object sender, CommandEventArgs e)
        {
            try {
                if (!Null.IsNull (int.Parse (comboEduProgramProfile.SelectedValue))) {
                    EmployeeDisciplineView discipline;

                    // get disciplines list from viewstate
                    var disciplines = ViewState ["disciplines"] as List<EmployeeDisciplineView> ?? new List<EmployeeDisciplineView> ();

                    var command = e.CommandArgument.ToString ();
                    if (command == "Add") {
                        discipline = new EmployeeDisciplineView ();
                    }
                    else {
                        // restore ItemID from hidden field
                        var hiddenItemID = int.Parse (hiddenDisciplinesItemID.Value);
                        discipline = disciplines.Find (ep1 => ep1.ItemID == hiddenItemID);
                    }

                    var eduProgramProfileId = int.Parse (comboEduProgramProfile.SelectedValue);

                    // check for possible duplicates
                    var discCount = disciplines.Count (d => d.EduProgramProfileID == eduProgramProfileId);

                    if ((command == "Add" && discCount == 0) || (command == "Update" && discCount <= 1)) {
                        discipline.EduProgramProfileID = eduProgramProfileId;
                        discipline.Disciplines = textDisciplines.Text.Trim ();

                        var profile = Repository.QueryEduProgramProfile (discipline.EduProgramProfileID).Single ();

                        discipline.Code = profile.EduProgram.Code;
                        discipline.Title = profile.EduProgram.Title;
                        discipline.ProfileCode = profile.ProfileCode;
                        discipline.ProfileTitle = profile.ProfileTitle;

                        if (command == "Add") {
                            disciplines.Add (discipline);
                        }

                        ResetEditDisciplinesForm ();

                        // refresh viewstate
                        ViewState ["disciplines"] = disciplines;

                        // bind items to the gridview
                        gridDisciplines.DataSource = DisciplinesDataTable (disciplines);
                        gridDisciplines.DataBind ();
                    }
                    else {
                        valEduProgramProfile.IsValid = false;
                        valEduProgramProfile.ErrorMessage = LocalizeString ("EduProgramProfile.Warning");
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void linkEditDisciplines_Command (object sender, CommandEventArgs e)
        {
            try {
                var disciplines = ViewState ["disciplines"] as List<EmployeeDisciplineView>;
                if (disciplines != null) {
                    var itemID = e.CommandArgument.ToString ();

                    // find position in a list
                    var discipline = disciplines.Find (d => d.ItemID.ToString () == itemID);

                    if (discipline != null) {
                        var profile = Repository.QueryEduProgramProfile (discipline.EduProgramProfileID).Single ();
                        var eduLevelId = int.Parse (comboEduLevel.SelectedValue);
                        var newEduLevelId = profile.EduProgram.EduLevelID;
                        if (eduLevelId != newEduLevelId) {
                            comboEduLevel.SelectByValue (newEduLevelId);
                            BindEduProgramProfiles (newEduLevelId);
                        }

                        // fill achievements form
                        comboEduProgramProfile.SelectByValue (discipline.EduProgramProfileID);
                        textDisciplines.Text = discipline.Disciplines;

                        // store ItemID in the hidden field
                        hiddenDisciplinesItemID.Value = discipline.ItemID.ToString ();

                        // show / hide buttons
                        buttonAddDisciplines.Visible = false;
                        buttonUpdateDisciplines.Visible = true;
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void linkDeleteDisciplines_Command (object sender, CommandEventArgs e)
        {
            try {
                var disciplines = ViewState ["disciplines"] as List<EmployeeDisciplineView>;
                if (disciplines != null) {
                    var itemID = e.CommandArgument.ToString ();

                    // find position in a list
                    var disciplinesIndex = disciplines.FindIndex (ep => ep.ItemID.ToString () == itemID);

                    if (disciplinesIndex >= 0) {
                        // remove edu program
                        disciplines.RemoveAt (disciplinesIndex);

                        // refresh viewstate
                        ViewState ["disciplines"] = disciplines;

                        // bind edu discipline to the gridview
                        gridDisciplines.DataSource = DisciplinesDataTable (disciplines);
                        gridDisciplines.DataBind ();

                        // reset form if we deleting currently edited discipline
                        if (buttonUpdateDisciplines.Visible && hiddenDisciplinesItemID.Value == itemID) {
                            ResetEditDisciplinesForm ();
                        }
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void comboAchievement_SelectedIndexChanged (object sender, EventArgs e)
        {
            try {
                if (((DropDownList) sender).SelectedValue == Null.NullInteger.ToString ()) {
                    panelAchievementTitle.Visible = true;
                    panelAchievementShortTitle.Visible = true;
                    panelAchievementTypes.Visible = true;
                }
                else {
                    panelAchievementTitle.Visible = false;
                    panelAchievementShortTitle.Visible = false;
                    panelAchievementTypes.Visible = false;
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void comboEduLevel_SelectedIndexChanged (object sender, EventArgs e)
        {
            var eduLevelId = int.Parse (comboEduLevel.SelectedValue);
            BindEduProgramProfiles (eduLevelId);
        }

        private void BindEduProgramProfiles (int eduLevelId)
        {
            var epps = EduProgramProfileRepository.Instance.GetEduProgramProfiles_ByEduLevel (eduLevelId);
            comboEduProgramProfile.DataSource = epps;
            comboEduProgramProfile.DataBind ();
        }

        #endregion
    }
}

