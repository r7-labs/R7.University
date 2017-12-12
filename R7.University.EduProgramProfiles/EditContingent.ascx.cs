//
//  EditContingent.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
using R7.University.Models;
using R7.University.Modules;

namespace R7.University.EduProgramProfiles
{
    public partial class EditContingent : UniversityEditPortalModuleBase<ContingentInfo>, IActionable
    {
        public enum EditContingentTab
        {
            Actual,
            Admission,
            Movement,
            Vacant
        }

        #region Properties

        protected EditContingentTab SelectedTab
        {
            get {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                /*
                if (!string.IsNullOrEmpty (eventTarget)) {
                    if (eventTarget.Contains ("$" + <practicesControl>.ID)) {
                        ViewState ["SelectedTab"] = EditEduVolumeTab.Practices;
                        return EditEduVolumeTab.Practices;
                    }
                }
                */

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                return (obj != null) ? (EditContingentTab) obj : EditContingentTab.Actual;
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        #endregion

        protected EditContingent () : base ("contingent_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);
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
            item.ActualFB = TypeUtils.ParseToNullable<int> (textActualFB.Text);
            item.ActualRB = TypeUtils.ParseToNullable<int> (textActualRB.Text);
            item.ActualMB = TypeUtils.ParseToNullable<int> (textActualMB.Text);
            item.ActualBC = TypeUtils.ParseToNullable<int> (textActualBC.Text);

            item.VacantFB = TypeUtils.ParseToNullable<int> (textVacantFB.Text);
            item.VacantRB = TypeUtils.ParseToNullable<int> (textVacantRB.Text);
            item.VacantMB = TypeUtils.ParseToNullable<int> (textVacantMB.Text);
            item.VacantBC = TypeUtils.ParseToNullable<int> (textVacantBC.Text);

            item.AdmittedFB = TypeUtils.ParseToNullable<int> (textAdmittedFB.Text);
            item.AdmittedRB = TypeUtils.ParseToNullable<int> (textAdmittedRB.Text);
            item.AdmittedMB = TypeUtils.ParseToNullable<int> (textAdmittedMB.Text);
            item.AdmittedBC = TypeUtils.ParseToNullable<int> (textAdmittedBC.Text);
            item.AvgAdmScore = TypeUtils.ParseToNullable<decimal> (textAvgAdmScore.Text);

            item.MovedOut = TypeUtils.ParseToNullable<int> (textMovedOut.Text);
            item.MovedIn = TypeUtils.ParseToNullable<int> (textMovedIn.Text);
            item.Restored = TypeUtils.ParseToNullable<int> (textRestored.Text);
            item.Expelled = TypeUtils.ParseToNullable<int> (textExpelled.Text);
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
                var itemId = TypeUtils.ParseToNullable<int> (Request.QueryString [Key])
                             ?? TypeUtils.ParseToNullable<int> (Request.QueryString ["eduprogramprofileformyear_id"]);
                if (itemId != null) {
                    var formYear = ModelContext.Get<EduProgramProfileFormYearInfo> (itemId.Value);
                    if (formYear != null) {
                        actions.Add (new ModuleAction (GetNextActionID ()) {
                            Title = LocalizeString ("EditEduProgramProfile.Action"),
                            CommandName = ModuleActionType.EditContent,
                            Icon = UniversityIcons.Edit,
                            Secure = SecurityAccessLevel.Edit,
                            Url = EditUrl ("eduprogramprofile_id", formYear.EduProgramProfileId.ToString (), "EditEduProgramProfile"),
                            Visible = SecurityContext.IsAdmin
                        });
                    }
                }
                return actions;
            }
        }

        #endregion
    }
}
