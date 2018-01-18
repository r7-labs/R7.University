//
//  EditEduProgramProfile.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2018 Roman M. Yagodin
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
using System.Text.RegularExpressions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Commands;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.EduProgramProfiles.Queries;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfiles
{
    public partial class EditEduProgramProfile: UniversityEditPortalModuleBase<EduProgramProfileInfo>, IActionable
    {
        public enum EditEduProgramProfileTab
        {
            Common,
            EduFormYears,
            Divisions,
            Audit
        }

        #region Properties

        protected EditEduProgramProfileTab SelectedTab {
            get {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                if (!string.IsNullOrEmpty (eventTarget)) {
                    if (eventTarget.Contains ("$" + formEditDivisions.ID)) {
                        ViewState ["SelectedTab"] = EditEduProgramProfileTab.Divisions;
                        return EditEduProgramProfileTab.Divisions;
                    }
                    if (eventTarget.Contains ("$" + formEditEduFormYears.ID)) {
                        ViewState ["SelectedTab"] = EditEduProgramProfileTab.EduFormYears;
                        return EditEduProgramProfileTab.EduFormYears;
                    }
                }

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                return (obj != null) ? (EditEduProgramProfileTab) obj : EditEduProgramProfileTab.Common;
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        #endregion

        protected EditEduProgramProfile () : base ("eduprogramprofile_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel, auditControl);
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // get and bind edu. levels
            var eduProgramLevels = new EduLevelQuery (ModelContext).ListForEduProgram ();
            comboEduProgramLevel.DataSource = eduProgramLevels;
            comboEduProgramLevel.DataBind ();

            // try to select edu. program
            var eduProgramId = GetQueryId ("eduprogram_id");
            if (eduProgramId != null) {
                var eduProgram = ModelContext.Get<EduProgramInfo> (eduProgramId.Value);
                BindEduPrograms (eduProgram.EduLevelID);
                comboEduProgram.SelectByValue (eduProgramId);
                comboEduProgramLevel.SelectByValue (eduProgram.EduLevelID);

                // set edu. level for edu. program profile same as for edu. program
                comboEduLevel.SelectByValue (eduProgram.EduLevelID);
            }
            else {
                if (eduProgramLevels.Count > 0) {
                    BindEduPrograms (eduProgramLevels.First ().EduLevelID);
                }
            }

            // TODO: Disable edu. program selection then adding or editing from EditEduProgram

            formEditEduFormYears.OnInit (this, new FlatQuery<EduFormInfo> (ModelContext).ListOrderBy (ef => ef.SortIndex), ((UniversityModelContext) ModelContext).Years);
            formEditDivisions.OnInit (this, new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title));
        }

        int? GetQueryId (string key)
        {
            var idParam = Request.QueryString [key];
            if (idParam != null) {
                int Id;
                if (int.TryParse (idParam, out Id)) {
                    return Id;
                }
            }

            return null;
        }

        void BindEduPrograms (int eduLevelId)
        {
            comboEduProgram.DataSource = new EduProgramCommonQuery (ModelContext).ListByEduLevel (eduLevelId)
                .Select (ep => new ListItemViewModel (ep.EduProgramID, FormatHelper.FormatEduProgramTitle (ep.Code, ep.Title)));

            comboEduProgram.DataBind ();

            comboEduLevel.DataSource = ModelContext.Query<EduLevelInfo> ()
                    .Where (el => el.ParentEduLevelId == eduLevelId || el.EduLevelID == eduLevelId)
                    .OrderBy (el => el.ParentEduLevelId != null)
                    .ToList ();

            comboEduLevel.DataBind ();
        }

        protected void comboEduProgramLevel_SelectedIndexChanged (object sender, EventArgs e)
        {
            BindEduPrograms (int.Parse (comboEduProgramLevel.SelectedValue));
        }

        protected override void LoadItem (EduProgramProfileInfo item)
        {
            var epp = GetItemWithDependencies (ItemId.Value);

            textProfileCode.Text = epp.ProfileCode;
            textProfileTitle.Text = epp.ProfileTitle;
            textLanguages.Text = epp.Languages;
            checkIsAdopted.Checked = epp.IsAdopted;
            checkELearning.Checked = epp.ELearning;
            checkDistanceEducation.Checked = epp.DistanceEducation;
            dateAccreditedToDate.SelectedDate = epp.AccreditedToDate;
            dateCommunityAccreditedToDate.SelectedDate = epp.CommunityAccreditedToDate;
            datetimeStartDate.SelectedDate = epp.StartDate;
            datetimeEndDate.SelectedDate = epp.EndDate;
            comboEduLevel.SelectByValue (epp.EduLevelId);
            formEditDivisions.SetData (epp.Divisions, epp.EduProgramProfileID);

            // update comboEduProgram, if needed
            var currentEduLevelId = int.Parse (comboEduProgramLevel.SelectedValue);
            if (epp.EduProgram.EduLevelID != currentEduLevelId) {
                comboEduProgramLevel.SelectByValue (epp.EduProgram.EduLevelID);
                BindEduPrograms (epp.EduProgram.EduLevelID);
            }

            comboEduProgram.SelectByValue (epp.EduProgramID);
            comboEduLevel.SelectByValue (epp.EduLevelId);

            auditControl.Bind (epp);

            formEditEduFormYears.SetData (epp.EduProgramProfileFormYears, epp.EduProgramProfileID);
        }

        protected override void BeforeUpdateItem (EduProgramProfileInfo item)
        {
            // fill the object
            item.ProfileCode = textProfileCode.Text.Trim ();
            item.ProfileTitle = textProfileTitle.Text.Trim ();
            item.Languages = textLanguages.Text.Replace (" ", string.Empty).Trim ();
            item.IsAdopted = checkIsAdopted.Checked;
            item.ELearning = checkELearning.Checked;
            item.DistanceEducation = checkDistanceEducation.Checked;
            item.AccreditedToDate = dateAccreditedToDate.SelectedDate;
            item.CommunityAccreditedToDate = dateCommunityAccreditedToDate.SelectedDate;
            item.StartDate = datetimeStartDate.SelectedDate;
            item.EndDate = datetimeEndDate.SelectedDate;

            // update references
            item.EduProgramID = int.Parse (comboEduProgram.SelectedValue);
            item.EduProgram = ModelContext.Get<EduProgramInfo> (item.EduProgramID);

            item.EduLevelId = int.Parse (comboEduLevel.SelectedValue);
            item.EduLevel = ModelContext.Get<EduLevelInfo> (item.EduLevelId);

            if (ItemId == null) {
            }
            else {
                item.LastModifiedOnDate = DateTime.Now;
                item.LastModifiedByUserId = UserInfo.UserID;
            }
        }

        protected override EduProgramProfileInfo GetItemWithDependencies (int itemId)
        {
            return new EduProgramProfileEditQuery (ModelContext).SingleOrDefault (itemId);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override void AddItem (EduProgramProfileInfo item)
        {
            if (SecurityContext.CanAdd (typeof (EduProgramProfileInfo))) {

                new AddCommand<EduProgramProfileInfo> (ModelContext, SecurityContext).Add (item);
                ModelContext.SaveChanges (false);

                new UpdateEduProgramProfileFormYearsCommand (ModelContext)
                    .Update (formEditEduFormYears.GetModifiedData (), item.EduProgramProfileID);

                new UpdateEduProgramDivisionsCommand (ModelContext)
                    .Update (formEditDivisions.GetModifiedData (), ModelType.EduProgramProfile, item.EduProgramProfileID);

                ModelContext.SaveChanges ();
            }
        }

        protected override void UpdateItem (EduProgramProfileInfo item)
        {
            // TODO: Use single transaction to update main entity along with all dependent ones?

            ModelContext.Update (item);

            new UpdateEduProgramProfileFormYearsCommand (ModelContext)
                .Update (formEditEduFormYears.GetModifiedData (), item.EduProgramProfileID);

            new UpdateEduProgramDivisionsCommand (ModelContext)
                .Update (formEditDivisions.GetModifiedData (), ModelType.EduProgramProfile, item.EduProgramProfileID);

            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EduProgramProfileInfo item)
        {
            // TODO: Also remove documents?
            new DeleteCommand<EduProgramProfileInfo> (ModelContext, SecurityContext).Delete (item);
            ModelContext.SaveChanges ();
        }

        #endregion

        protected void linkEditEduProgram_Click (object sender, EventArgs e)
        {
            var modalEditUrl = EditUrl (
                                   "eduprogram_id",
                                   int.Parse (comboEduProgram.SelectedValue).ToString (),
                                   "EditEduProgram");
            var rawEditUrl = Regex.Match (modalEditUrl, @"'(.*?)'").Groups [1].ToString ();
            Response.Redirect (rawEditUrl, true);
        }

        int? GetEduProgramProfileId () => TypeUtils.ParseToNullable<int> (Request.QueryString [Key]);

        #region IActionable implementation

        public ModuleActionCollection ModuleActions {
            get {
                var actions = new ModuleActionCollection ();
                var eppId = GetEduProgramProfileId ();
                if (eppId != null) {
                    actions.Add (new ModuleAction (GetNextActionID ()) {
                        Title = LocalizeString ("EditEduProgramProfileDocuments.Action"),
                        CommandName = ModuleActionType.EditContent,
                        Icon = UniversityIcons.Edit,
                        Secure = SecurityAccessLevel.Edit,
                        Url = EditUrl ("eduprogramprofile_id", eppId.ToString (), "EditEduProgramProfileDocuments"),
                        Visible = SecurityContext.IsAdmin
                    });
                }
                return actions;
            }
        }

        #endregion
    }
}
