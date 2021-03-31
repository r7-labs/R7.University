using System;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Commands;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.Employees.Models;
using R7.University.Employees.Queries;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.SharedLogic;

namespace R7.University.Employees
{
    public partial class EditEmployee: UniversityEditPortalModuleBase<EmployeeInfo>
    {
        #region Types

        public enum EditEmployeeTab
        {
            Common,
            Contacts,
            WorkExperience,
            Positions,
            Achievements,
            Disciplines,
            About,
            Audit
        }

        #endregion

        #region Properties

        protected EditEmployeeTab SelectedTab
        {
            get {
                // get postback initiator
                var eventTarget = Request.Form ["__EVENTTARGET"];

                if (!string.IsNullOrEmpty (eventTarget)) {

                    if (eventTarget.Contains ("$" + buttonUserLookup.ID)) {
                        ViewState ["SelectedTab"] = EditEmployeeTab.Common;
                        return EditEmployeeTab.Common;
                    }

                    if (eventTarget.Contains ("$" +  formEditPositions.ID)) {
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

            pickerPhoto.FolderPath = UniversityConfig.Instance.EmployeePhoto.DefaultPath;
            pickerPhoto.FileFilter = Globals.glbImageFileTypes;

            pickerAltPhoto.FolderPath = UniversityConfig.Instance.EmployeePhoto.DefaultPath;
            pickerAltPhoto.FileFilter = Globals.glbImageFileTypes;

            checkShowBarcode.Checked = true;

            // add default item to user list
            comboUsers.Items.Add (new ListItem (LocalizeString ("NotSelected.Text"), Null.NullInteger.ToString ()));

            // init working hours
            WorkingHoursLogic.Init (this, comboWorkingHours);

            var achievementTypes = new FlatQuery<AchievementTypeInfo> (ModelContext).List ();
            var achievements = new FlatQuery<AchievementInfo> (ModelContext).List ();
            var positions = new FlatQuery<PositionInfo> (ModelContext).ListOrderBy (p => p.Title);
            var divisions = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);

            formEditAchievements.OnInit (this, achievementTypes, achievements);
            formEditDisciplines.OnInit (this);
            formEditPositions.OnInit (this, positions, divisions);
        }

        protected override string GetContextString (EmployeeInfo item)
        {
            return item?.FullName ();
        }

        protected override void LoadItem (EmployeeInfo item)
        {
            var employee = GetItemWithDependencies (ItemKey.Value);
            base.LoadItem (employee);

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
            txtScienceIndexAuthorId.Text = employee.ScienceIndexAuthorId.ToString ();

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
            if (employee.PhotoFileID != null && !Null.IsNull (employee.PhotoFileID.Value)) {
                var photo = FileManager.Instance.GetFile (employee.PhotoFileID.Value);
                if (photo != null) {
                    pickerPhoto.FileID = photo.FileId;
                }
            }

            if (employee.AltPhotoFileId != null && !Null.IsNull (employee.AltPhotoFileId.Value)) {
                var photo = FileManager.Instance.GetFile (employee.AltPhotoFileId.Value);
                if (photo != null) {
                    pickerAltPhoto.FileID = photo.FileId;
                }
            }

            if (employee.UserID != null && !Null.IsNull (employee.UserID.Value)) {
                var user = UserController.GetUserById (PortalId, employee.UserID.Value);
                if (user != null) {
                    // add previously selected user to user list...
                    comboUsers.Items.Add (new ListItem (
                            user.Username + " / " + user.Email,
                            user.UserID.ToString ()));
                    comboUsers.SelectedIndex = 1;
                }
            }

            formEditAchievements.SetData (employee.Achievements.OrderByDescending (ach => ach.YearBegin), employee.EmployeeID);

            formEditPositions.SetData (employee.Positions, employee.EmployeeID);

            // apply default ordering
            var employeeDisciplines = employee.Disciplines
                .OrderBy (ed => ed.EduProfile.EduProgram.EduLevel.SortIndex)
                .ThenBy (ed => ed.EduProfile.EduProgram.Code)
                .ThenBy (ed => ed.EduProfile.EduProgram.Title)
                .ThenBy (ed => ed.EduProfile.ProfileCode)
                .ThenBy (ed => ed.EduProfile.ProfileTitle)
                .ThenBy (ed => ed.EduProfile.EduLevel.SortIndex);

            formEditDisciplines.SetData (employeeDisciplines, employee.EmployeeID);

            // setup audit control
            ctlAudit.Bind (employee, PortalId, LocalizeString ("Unknown")); ;

            SetupDivisionSelector ();
        }

        protected override void LoadNewItem ()
        {
            base.LoadNewItem ();

            SetupDivisionSelector ();
        }

        void SetupDivisionSelector ()
        {
            var divisionId = Request.QueryString ["division_id"];
            formEditPositions.SetDivision (ParseHelper.ParseToNullable<int> (divisionId));
        }

        protected override void BeforeUpdateItem (EmployeeInfo item, bool isNew)
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
            item.ExperienceYears = ParseHelper.ParseToNullable<int> (textExperienceYears.Text);
            item.ExperienceYearsBySpec = ParseHelper.ParseToNullable<int> (textExperienceYearsBySpec.Text);
            item.StartDate = datetimeStartDate.SelectedDate;
            item.EndDate = datetimeEndDate.SelectedDate;
            item.ScienceIndexAuthorId = ParseHelper.ParseToNullable<int> (txtScienceIndexAuthorId.Text);

            // FileID may be 0 by default
            item.PhotoFileID = (pickerPhoto.FileID > 0) ? (int?) pickerPhoto.FileID : null;
            item.AltPhotoFileId = (pickerAltPhoto.FileID > 0) ? (int?) pickerAltPhoto.FileID : null;

            item.UserID = ParseHelper.ParseToNullable<int> (comboUsers.SelectedValue, true);
        }

        protected EmployeeInfo GetItemWithDependencies (int itemId)
        {
            return new EmployeeQuery (ModelContext).SingleOrDefault (itemId);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (EmployeeInfo item) => item.EmployeeID;

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
                if (ModuleConfiguration.ModuleDefinition.DefinitionName == ModuleDefinitions.Employee ||
                ModuleConfiguration.ModuleDefinition.DefinitionName == ModuleDefinitions.EmployeeDetails) {
                    var settingsRepository = new EmployeeSettingsRepository ();
                    var settings = settingsRepository.GetSettings (ModuleConfiguration);
                    settings.EmployeeID = item.EmployeeID;

                    // we adding new employee, so he/she should be displayed in the module
                    settings.ShowCurrentUser = false;
                    settingsRepository.SaveSettings (ModuleConfiguration, settings);
                }

                new UpdateOccupiedPositionsCommand (ModelContext)
                    .Update (formEditPositions.GetModifiedData (), item.EmployeeID);

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
            item.LastModifiedByUserId = UserId;
            item.LastModifiedOnDate = DateTime.Now;

            // update employee
            ModelContext.Update (item);

            new UpdateOccupiedPositionsCommand (ModelContext)
                .Update (formEditPositions.GetModifiedData (), item.EmployeeID);

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

                // find cross-portal users including host user by email
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
    }
}
