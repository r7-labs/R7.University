//
//  EditContingent.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using R7.Dnn.Extensions.Utilities;
using R7.University.Commands;
using R7.University.Components;
using R7.University.EduProgramProfiles.Models;
using R7.University.EduProgramProfiles.Modules;
using R7.University.Models;

namespace R7.University.EduProgramProfiles
{
    public partial class EditContingent : EduProgramProfileFormYearEditModuleBase<ContingentInfo>, IActionable
    {
        public enum EditContingentTab
        {
            Actual,
            Vacant,
            Admission,
            Movement,
        }

        #region Properties

        protected EditContingentTab SelectedTab
        {
            get {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                return (obj != null) ? (EditContingentTab) obj : GetDefaultTab ();
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        EditContingentTab GetDefaultTab ()
        {
            if (Settings.Mode == ContingentDirectoryMode.Admission) {
                return EditContingentTab.Admission;
            }

            if (Settings.Mode == ContingentDirectoryMode.Movement) {
                return EditContingentTab.Movement;
            }

            if (Settings.Mode == ContingentDirectoryMode.Vacant) {
                return EditContingentTab.Vacant;
            }

            return EditContingentTab.Actual;
        }

        ContingentDirectorySettings _settings;
        protected new ContingentDirectorySettings Settings =>
            _settings ?? (_settings = new ContingentDirectorySettingsRepository ().GetSettings (ModuleConfiguration));

        #endregion

        protected EditContingent () : base ("contingent_id")
        {
        }

        protected void BindTabs ()
        {
            var showAllTabs = SecurityContext.IsAdmin;
        
            tabActual.Visible = showAllTabs || Settings.Mode == ContingentDirectoryMode.Actual;
            panelActual.Visible = tabActual.Visible;

            tabAdmission.Visible = showAllTabs || Settings.Mode == ContingentDirectoryMode.Admission;
            panelAdmission.Visible = tabAdmission.Visible;

            tabMovement.Visible = showAllTabs || Settings.Mode == ContingentDirectoryMode.Movement;
            panelMovement.Visible = tabMovement.Visible;

            tabVacant.Visible = showAllTabs || Settings.Mode == ContingentDirectoryMode.Vacant;
            panelVacant.Visible = tabVacant.Visible;
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            BindTabs ();
        }

        protected override void LoadItem (ContingentInfo item)
        {
            var c = GetItemWithDependencies (ItemId.Value);

            textActualFB.Text = c.ActualFB.ToString ();
            textActualRB.Text = c.ActualRB.ToString ();
            textActualMB.Text = c.ActualMB.ToString ();
            textActualBC.Text = c.ActualBC.ToString ();

            textAdmittedFB.Text = c.AdmittedFB.ToString ();
            textAdmittedRB.Text = c.AdmittedRB.ToString ();
            textAdmittedMB.Text = c.AdmittedMB.ToString ();
            textAdmittedBC.Text = c.AdmittedBC.ToString ();
            textAvgAdmScore.Text = c.AvgAdmScore.ToString ();

            textVacantFB.Text = c.VacantFB.ToString ();
            textVacantRB.Text = c.VacantRB.ToString ();
            textVacantMB.Text = c.VacantMB.ToString ();
            textVacantBC.Text = c.VacantBC.ToString ();

            textMovedOut.Text = c.MovedOut.ToString ();
            textMovedIn.Text = c.MovedIn.ToString ();
            textRestored.Text = c.Restored.ToString ();
            textExpelled.Text = c.Expelled.ToString ();
        }

        protected override void BeforeUpdateItem (ContingentInfo item)
        {
            var updateAllTabs = SecurityContext.IsAdmin;

            if (updateAllTabs || Settings.Mode == ContingentDirectoryMode.Actual) {
                item.ActualFB = TypeUtils.ParseToNullable<int> (textActualFB.Text);
                item.ActualRB = TypeUtils.ParseToNullable<int> (textActualRB.Text);
                item.ActualMB = TypeUtils.ParseToNullable<int> (textActualMB.Text);
                item.ActualBC = TypeUtils.ParseToNullable<int> (textActualBC.Text);
            }

            if (updateAllTabs || Settings.Mode == ContingentDirectoryMode.Vacant) {
                item.VacantFB = TypeUtils.ParseToNullable<int> (textVacantFB.Text);
                item.VacantRB = TypeUtils.ParseToNullable<int> (textVacantRB.Text);
                item.VacantMB = TypeUtils.ParseToNullable<int> (textVacantMB.Text);
                item.VacantBC = TypeUtils.ParseToNullable<int> (textVacantBC.Text);
            }

            if (updateAllTabs || Settings.Mode == ContingentDirectoryMode.Admission) {
                item.AdmittedFB = TypeUtils.ParseToNullable<int> (textAdmittedFB.Text);
                item.AdmittedRB = TypeUtils.ParseToNullable<int> (textAdmittedRB.Text);
                item.AdmittedMB = TypeUtils.ParseToNullable<int> (textAdmittedMB.Text);
                item.AdmittedBC = TypeUtils.ParseToNullable<int> (textAdmittedBC.Text);
                item.AvgAdmScore = TypeUtils.ParseToNullable<decimal> (textAvgAdmScore.Text);
            }

            if (updateAllTabs || Settings.Mode == ContingentDirectoryMode.Movement) {
                item.MovedOut = TypeUtils.ParseToNullable<int> (textMovedOut.Text);
                item.MovedIn = TypeUtils.ParseToNullable<int> (textMovedIn.Text);
                item.Restored = TypeUtils.ParseToNullable<int> (textRestored.Text);
                item.Expelled = TypeUtils.ParseToNullable<int> (textExpelled.Text);
            }
        }

        protected override ContingentInfo GetItemWithDependencies (int itemId)
        {
            return ModelContext.Get<ContingentInfo> (itemId);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override void AddItem (ContingentInfo item)
        {
            var contingentId = TypeUtils.ParseToNullable<int> (Request.QueryString ["eduprogramprofileformyear_id"]);
            if (contingentId != null) {
                item.ContingentId = contingentId.Value;
            }

            new AddCommand<ContingentInfo> (ModelContext, SecurityContext).Add (item);
            ModelContext.SaveChanges ();
        }

        protected override void UpdateItem (ContingentInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (ContingentInfo item)
        {
            new DeleteCommand<ContingentInfo> (ModelContext, SecurityContext).Delete (item);
            ModelContext.SaveChanges ();
        }

        #endregion

        #region IActionable implementation

        public ModuleActionCollection ModuleActions {
            get {
                var actions = new ModuleActionCollection ();
                var eppfy = GetEduProgramProfileFormYear ();
                if (eppfy != null) {
                    actions.Add (new ModuleAction (GetNextActionID ()) {
                        Title = LocalizeString ("EditEduProgramProfile.Action"),
                        CommandName = ModuleActionType.EditContent,
                        Icon = UniversityIcons.Edit,
                        Secure = SecurityAccessLevel.Edit,
                        Url = EditUrl ("eduprogramprofile_id", eppfy.EduProgramProfileId.ToString (), "EditEduProgramProfile"),
                        Visible = SecurityContext.IsAdmin
                    });
                }
                return actions;
            }
        }

        #endregion
    }
}
