//
//  EditContingent.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2018 Roman M. Yagodin
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
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.Utilities;
using R7.University.Commands;
using R7.University.Components;
using R7.University.EduProgramProfiles.Models;
using R7.University.EduProgramProfiles.Modules;
using R7.University.Models;
using R7.University.ViewModels;

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
            if (Settings != null) {
                if (Settings.Mode == ContingentDirectoryMode.Admission) {
                    return EditContingentTab.Admission;
                }

                if (Settings.Mode == ContingentDirectoryMode.Movement) {
                    return EditContingentTab.Movement;
                }

                if (Settings.Mode == ContingentDirectoryMode.Vacant) {
                    return EditContingentTab.Vacant;
                }
            }

            return EditContingentTab.Actual;
        }

        ContingentDirectorySettings _settings;
        protected new ContingentDirectorySettings Settings =>
            _settings ?? (_settings = ModuleConfiguration.ModuleDefinition.DefinitionName == "R7_University_ContingentDirectory"
                          ? new ContingentDirectorySettingsRepository ().GetSettings (ModuleConfiguration)
                          : null);

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
            var c = GetItemWithDependencies (ItemKey.Value);

            textActualFB.Text = c.ActualFB.ToString ();
            textActualRB.Text = c.ActualRB.ToString ();
            textActualMB.Text = c.ActualMB.ToString ();
            textActualBC.Text = c.ActualBC.ToString ();

            textAdmittedFB.Text = c.AdmittedFB.ToString ();
            textAdmittedRB.Text = c.AdmittedRB.ToString ();
            textAdmittedMB.Text = c.AdmittedMB.ToString ();
            textAdmittedBC.Text = c.AdmittedBC.ToString ();
            textAvgAdmScore.Text = c.AvgAdmScore.ToDecimalString ();

            textVacantFB.Text = c.VacantFB.ToString ();
            textVacantRB.Text = c.VacantRB.ToString ();
            textVacantMB.Text = c.VacantMB.ToString ();
            textVacantBC.Text = c.VacantBC.ToString ();

            textMovedOut.Text = c.MovedOut.ToString ();
            textMovedIn.Text = c.MovedIn.ToString ();
            textRestored.Text = c.Restored.ToString ();
            textExpelled.Text = c.Expelled.ToString ();
        }

        protected override void BeforeUpdateItem (ContingentInfo item, bool isNew)
        {
            var updateAllTabs = SecurityContext.IsAdmin;

            if (updateAllTabs || Settings.Mode == ContingentDirectoryMode.Actual) {
                item.ActualFB = ParseHelper.ParseToNullable<int> (textActualFB.Text);
                item.ActualRB = ParseHelper.ParseToNullable<int> (textActualRB.Text);
                item.ActualMB = ParseHelper.ParseToNullable<int> (textActualMB.Text);
                item.ActualBC = ParseHelper.ParseToNullable<int> (textActualBC.Text);
            }

            if (updateAllTabs || Settings.Mode == ContingentDirectoryMode.Vacant) {
                item.VacantFB = ParseHelper.ParseToNullable<int> (textVacantFB.Text);
                item.VacantRB = ParseHelper.ParseToNullable<int> (textVacantRB.Text);
                item.VacantMB = ParseHelper.ParseToNullable<int> (textVacantMB.Text);
                item.VacantBC = ParseHelper.ParseToNullable<int> (textVacantBC.Text);
            }

            if (updateAllTabs || Settings.Mode == ContingentDirectoryMode.Admission) {
                item.AdmittedFB = ParseHelper.ParseToNullable<int> (textAdmittedFB.Text);
                item.AdmittedRB = ParseHelper.ParseToNullable<int> (textAdmittedRB.Text);
                item.AdmittedMB = ParseHelper.ParseToNullable<int> (textAdmittedMB.Text);
                item.AdmittedBC = ParseHelper.ParseToNullable<int> (textAdmittedBC.Text);
                item.AvgAdmScore = ParseHelper.ParseToNullable<decimal> (textAvgAdmScore.Text);
            }

            if (updateAllTabs || Settings.Mode == ContingentDirectoryMode.Movement) {
                item.MovedOut = ParseHelper.ParseToNullable<int> (textMovedOut.Text);
                item.MovedIn = ParseHelper.ParseToNullable<int> (textMovedIn.Text);
                item.Restored = ParseHelper.ParseToNullable<int> (textRestored.Text);
                item.Expelled = ParseHelper.ParseToNullable<int> (textExpelled.Text);
            }
        }

        protected ContingentInfo GetItemWithDependencies (int itemId)
        {
            return ModelContext.Get<ContingentInfo,int> (itemId);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (ContingentInfo item) => item.ContingentId;

        protected override void AddItem (ContingentInfo item)
        {
            var contingentId = ParseHelper.ParseToNullable<int> (Request.QueryString ["eduprogramprofileformyear_id"]);
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
