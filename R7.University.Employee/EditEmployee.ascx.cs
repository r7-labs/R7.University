//
//  EditEmployee.ascx.cs
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Commands;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.Employee.Models;
using R7.University.Employee.Queries;
using R7.University.Employee.ViewModels;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.SharedLogic;
using R7.University.Utilities;

namespace R7.University.Employee
{
    public partial class EditEmployee: UniversityEditPortalModuleBase<EmployeeInfo>
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

                    if (eventTarget.Contains ("$" + buttonCancelEditPosition.ID) ||
                        eventTarget.Contains ("$" + buttonAddPosition.ID) ||
                        eventTarget.Contains ("$" + buttonUpdatePosition.ID) ||
                        eventTarget.Contains ("$" + divisionSelector.ID) ||
                        eventTarget.Contains ("$" + gridOccupiedPositions.ID)) {
                        ViewState ["SelectedTab"] = EditEmployeeTab.Positions;
                        return EditEmployeeTab.Positions;
                    }

                    if (eventTarget.Contains ("$" +  formEditAchievements.ID)) {
                        ViewState ["SelectedTab"] = EditEmployeeTab.Achievements;
                        return EditEmployeeTab.Achievements;
                    }

                    if (eventTarget.Contains ("$" + formEditDisciplines.ID)) {
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


        internal List<OccupiedPositionEditModel> OccupiedPositions
        {
            get { return XmlSerializationHelper.Deserialize<List<OccupiedPositionEditModel>> (ViewState ["occupiedPositions"]); }
            set { ViewState ["occupiedPositions"] = XmlSerializationHelper.Serialize (value); }
        }

        protected string EditIconUrl
        {
            get { return IconController.IconURL ("Edit"); }
        }

        protected string DeleteIconUrl
        {
            get { return IconController.IconURL ("Delete"); }
        }

        ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this)); }
        }

        #endregion

        protected EditEmployee () : base ("employee_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel, ctlAudit);
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // setup filepicker
            pickerPhoto.FolderPath = UniversityConfig.Instance.EmployeePhoto.DefaultPath;
            pickerPhoto.FileFilter = Globals.glbImageFileTypes;

            checkShowBarcode.Checked = true;

            // add default item to user list
            comboUsers.Items.Add (new ListItem (LocalizeString ("NotSelected.Text"), Null.NullInteger.ToString ()));

            // init working hours
            WorkingHoursLogic.Init (this, comboWorkingHours);

            // if results are null or empty, lists were empty too

            // bind positions
            comboPositions.DataSource = new FlatQuery<PositionInfo> (ModelContext).ListOrderBy (p => p.Title);
            comboPositions.DataBind ();
            comboPositions.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
        
            // bind divisions
            divisionSelector.DataSource = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            divisionSelector.DataBind ();

            var achievementTypes = new FlatQuery<AchievementTypeInfo> (ModelContext).List ();
            var achievements = new FlatQuery<AchievementInfo> (ModelContext).List ();
            formEditAchievements.OnInit (this, achievementTypes, achievements);
            formEditDisciplines.OnInit (this, new EduLevelQuery (ModelContext).List ());

            // localize bounded gridviews
            gridOccupiedPositions.LocalizeColumns (LocalResourceFile);
        }

        protected override void LoadItem (EmployeeInfo item)
        {
            var employee = GetItemWithDependencies (ItemId.Value);

            textLastName.Text = employee.LastName;
            textFirstName.Text = employee.FirstName;
            textOtherName.Text = employee.OtherName;
            textPhone.Text = employee.Phone;
            textCellPhone.Text = employee.CellPhone;
            textFax.Text = employee.Fax;
            textEmail.Text = employee.Email;
            textSecondaryEmail.Text = employee.SecondaryEmail;
            textWebSite.Text = employee.WebSite;
            textWebSiteLabel.Text = employee.WebSiteLabel;
            textMessenger.Text = employee.Messenger;
            textWorkingPlace.Text = employee.WorkingPlace;
            textBiography.Text = employee.Biography;
            checkShowBarcode.Checked = employee.ShowBarcode;

            // load working hours
            WorkingHoursLogic.Load (comboWorkingHours, textWorkingHours, employee.WorkingHours);

            if (!Null.IsNull (employee.ExperienceYears)) {
                textExperienceYears.Text = employee.ExperienceYears.ToString ();
            }

            if (!Null.IsNull (employee.ExperienceYearsBySpec)) {
                textExperienceYearsBySpec.Text = employee.ExperienceYearsBySpec.ToString ();
            }

            datetimeStartDate.SelectedDate = employee.StartDate;
            datetimeEndDate.SelectedDate = employee.EndDate;

            // set photo
            if (!TypeUtils.IsNull (employee.PhotoFileID)) {
                var photo = FileManager.Instance.GetFile (employee.PhotoFileID.Value);
                if (photo != null) {
                    pickerPhoto.FileID = photo.FileId;
                }
            }

            if (!Null.IsNull (employee.UserID)) {
                var user = UserController.GetUserById (this.PortalId, employee.UserID.Value);
                if (user != null) {
                    // add previously selected user to user list...
                    comboUsers.Items.Add (new ListItem (
                            user.Username + " / " + user.Email,
                            user.UserID.ToString ()));
                    comboUsers.SelectedIndex = 1;
                }
            }

            // fill view list
            var occupiedPositions = employee.Positions
                .Select (op => new OccupiedPositionEditModel (op)).ToList ();

            // bind occupied positions
            OccupiedPositions = occupiedPositions;
            gridOccupiedPositions.DataSource = occupiedPositions;
            gridOccupiedPositions.DataBind ();

            // TODO: Sort achievements
            formEditAchievements.SetData (employee.Achievements, employee.EmployeeID);

            // bind disciplines
            formEditDisciplines.SetData (employee.Disciplines, employee.EmployeeID);
      
            // setup audit control
            ctlAudit.Bind (employee);

            SetupDivisionSelector ();
        }

        protected override void LoadNewItem ()
        {
            SetupDivisionSelector ();
        }

        void SetupDivisionSelector ()
        {
            // then edit / add from EmployeeList, divisionId query param
            // can be set to current division ID
            var divisionId = Request.QueryString ["division_id"];
            divisionSelector.DivisionId = TypeUtils.ParseToNullable<int> (divisionId);
        }

        protected override void BeforeUpdateItem (EmployeeInfo item)
        {
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
            item.ExperienceYears = TypeUtils.ParseToNullable<int> (textExperienceYears.Text);
            item.ExperienceYearsBySpec = TypeUtils.ParseToNullable<int> (textExperienceYearsBySpec.Text);
            item.StartDate = datetimeStartDate.SelectedDate;
            item.EndDate = datetimeEndDate.SelectedDate;

            // pickerPhoto.FileID may be 0 by default
            item.PhotoFileID = (pickerPhoto.FileID > 0) ? (int?) pickerPhoto.FileID : null;
            item.UserID = TypeUtils.ParseToNullable<int> (comboUsers.SelectedValue);
        }

        protected override EmployeeInfo GetItemWithDependencies (int itemId)
        {
            return new EmployeeQuery (ModelContext).SingleOrDefault (itemId);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override void AddItem (EmployeeInfo item)
        {
            if (SecurityContext.CanAdd (typeof (EmployeeInfo))) {
                // update working hours
                item.WorkingHours = WorkingHoursLogic.Update (
                    comboWorkingHours,
                    textWorkingHours.Text,
                    checkAddToVocabulary.Checked
                );

                // add employeee
                new AddCommand<EmployeeInfo> (ModelContext, SecurityContext).Add (item);
                ModelContext.SaveChanges (false);

                // then adding new employee from Employee or EmployeeDetails modules, 
                // set calling module to display new employee
                if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7_University_Employee" ||
                ModuleConfiguration.ModuleDefinition.DefinitionName == "R7_University_EmployeeDetails") {
                    var settingsRepository = new EmployeeSettingsRepository ();
                    var settings = settingsRepository.GetSettings (ModuleConfiguration);
                    settings.EmployeeID = item.EmployeeID;

                    // we adding new employee, so he/she should be displayed in the module
                    settings.ShowCurrentUser = false;
                    settingsRepository.SaveSettings (ModuleConfiguration, settings);
                }

                new UpdateOccupiedPositionsCommand (ModelContext)
                    .UpdateOccupiedPositions (GetOccupiedPositions (), item.EmployeeID);

                new UpdateEmployeeAchievementsCommand (ModelContext)
                    .UpdateEmployeeAchievements (formEditAchievements.GetModifiedData (), item.EmployeeID);

                new UpdateEmployeeDisciplinesCommand (ModelContext)
                    .Update (formEditDisciplines.GetModifiedData (), item.EmployeeID);

                ModelContext.SaveChanges ();
            }
        }

        protected override void UpdateItem (EmployeeInfo item)
        {
            // update working hours
            item.WorkingHours = WorkingHoursLogic.Update (
                comboWorkingHours,
                textWorkingHours.Text,
                checkAddToVocabulary.Checked
            );

            // update audit info
            item.LastModifiedByUserID = UserId;
            item.LastModifiedOnDate = DateTime.Now;

            // update employee
            ModelContext.Update (item);

            new UpdateOccupiedPositionsCommand (ModelContext)
                        .UpdateOccupiedPositions (GetOccupiedPositions (), item.EmployeeID);

            new UpdateEmployeeAchievementsCommand (ModelContext)
                .UpdateEmployeeAchievements (formEditAchievements.GetModifiedData (), item.EmployeeID);

            new UpdateEmployeeDisciplinesCommand (ModelContext)
                .Update (formEditDisciplines.GetModifiedData (), item.EmployeeID);

            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EmployeeInfo item)
        {
            // TODO: Delete also photo and other assets
            new DeleteCommand<EmployeeInfo> (ModelContext, SecurityContext).Delete (item);
            ModelContext.SaveChanges ();
        }

        #endregion

        private List<OccupiedPositionInfo> GetOccupiedPositions ()
        {
            var occupiedPositions = OccupiedPositions;
					
            var occupiedPositionInfos = new List<OccupiedPositionInfo> ();
            if (occupiedPositions != null)
                foreach (var op in occupiedPositions)
                    occupiedPositionInfos.Add (op.NewOccupiedPositionInfo ());

            return occupiedPositionInfos;
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

                    // TODO: EmployeeInfo should contain culture data?
                    var employeeNameTL = CultureHelper.Transliterate (employeeName, CultureHelper.RuTranslitTable)
                                                      .ToLowerInvariant ();

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

        protected void buttonCancelEditPosition_Click (object sender, EventArgs e)
        {
            try {
                ResetEditPositionForm ();
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        private void ResetEditPositionForm ()
        {
            // restore default buttons visibility
            buttonAddPosition.Visible = true;
            buttonUpdatePosition.Visible = false;

            // reset selected division
            var divisionId = Request.QueryString ["division_id"];
            divisionSelector.DivisionId = TypeUtils.ParseToNullable<int> (divisionId);
	
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
                var divisionID = divisionSelector.DivisionId;

                if (!Null.IsNull (positionID) && divisionID != null) {
                    OccupiedPositionEditModel occupiedPosition;

                    var occupiedPositions = OccupiedPositions ?? new List<OccupiedPositionEditModel> ();

                    var command = e.CommandArgument.ToString ();
                    if (command == "Add") {
                        occupiedPosition = new OccupiedPositionEditModel ();
                    }
                    else { // update 
                        // restore ItemID from hidden field
                        var hiddenItemID = int.Parse (hiddenOccupiedPositionItemID.Value);
                        occupiedPosition = occupiedPositions.Find (op => op.ItemID == hiddenItemID);
                    }
					
                    // fill the object
                    occupiedPosition.PositionID = positionID;
                    occupiedPosition.DivisionID = divisionID.Value;
                    occupiedPosition.PositionShortTitle = comboPositions.SelectedItem.Text;
                    occupiedPosition.DivisionShortTitle = divisionSelector.DivisionTitle;
                    occupiedPosition.IsPrime = checkIsPrime.Checked;
                    occupiedPosition.TitleSuffix = textPositionTitleSuffix.Text.Trim ();
					
                    if (command == "Add") {
                        occupiedPositions.Add (occupiedPosition);
                    }

                    ResetEditPositionForm ();

                    OccupiedPositions = occupiedPositions;
                    gridOccupiedPositions.DataSource = occupiedPositions;
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
                var occupiedPositions = OccupiedPositions;
                if (occupiedPositions != null) {
                    var itemID = e.CommandArgument.ToString ();
	
                    // find position in a list
                    var occupiedPosition = occupiedPositions.Find (op => op.ItemID.ToString () == itemID);
	
                    if (occupiedPosition != null) {
                        // fill the form
                        divisionSelector.DivisionId = occupiedPosition.DivisionID;
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
                var occupiedPositions = OccupiedPositions;
                if (occupiedPositions != null) {
                    var itemID = e.CommandArgument.ToString ();
	
                    // find position in a list
                    var opFound = occupiedPositions.Find (op => op.ItemID.ToString () == itemID);
				
                    if (opFound != null) {
                        occupiedPositions.Remove (opFound);

                        OccupiedPositions = occupiedPositions;
	
                        gridOccupiedPositions.DataSource = occupiedPositions;
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
    }
}
