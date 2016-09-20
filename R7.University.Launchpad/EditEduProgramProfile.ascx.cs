//
//  EditEduProgramProfile.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using System.Linq;
using System.Text.RegularExpressions;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Commands;
using R7.University.ControlExtensions;
using R7.University.Launchpad.Queries;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Launchpad
{
    public partial class EditEduProgramProfile: EditPortalModuleBase<EduProgramProfileInfo,int>
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

        protected int SelectedTab
        {
            get {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                if (!string.IsNullOrEmpty (eventTarget)) {

                    // check if postback initiator is on EduForms tab
                    if (eventTarget.Contains ("$" + formEditEduForms.ID)) {
                        ViewState ["SelectedTab"] = 2;
                        return 2;
                    }

                    // check if postback initiator is on Documents tab
                    if (eventTarget.Contains ("$" + formEditDocuments.ID)) {
                        ViewState ["SelectedTab"] = 3;
                        return 3;
                    }
                }

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                if (obj != null) {
                    return (int) obj;
                }

                return 0;
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

        /// <summary>
        /// Handles Init event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // get and bind edu. levels
            var eduProgramLevels = new EduLevelQuery (ModelContext).ListForEduProgram ();
            comboEduProgramLevel.DataSource = eduProgramLevels;
            comboEduProgramLevel.DataBind ();

            // get and bind edu. profiles
            BindEduPrograms (eduProgramLevels.First ().EduLevelID);

            // init edit forms
            formEditEduForms.OnInit (this, new FlatQuery<EduFormInfo> (ModelContext).List ());
            formEditDocuments.OnInit (this, new FlatQuery<DocumentTypeInfo> (ModelContext).List ());

            // fill divisions dropdown
            var divisions = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

            treeDivision.DataSource = divisions;
            treeDivision.DataBind ();
        }

        private void BindEduPrograms (int eduLevelId)
        {
            comboEduProgram.DataSource = new EduProgramCommonQuery (ModelContext).ListByEduLevel (eduLevelId);
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
            dateAccreditedToDate.SelectedDate = epp.AccreditedToDate;
            dateCommunityAccreditedToDate.SelectedDate = epp.CommunityAccreditedToDate;
            datetimeStartDate.SelectedDate = epp.StartDate;
            datetimeEndDate.SelectedDate = epp.EndDate;
            comboEduLevel.SelectByValue (epp.EduLevelId);
            treeDivision.SelectAndExpandByValue (epp.DivisionId.ToString ());

            // update comboEduProgram, if needed
            var currentEduLevelId = int.Parse (comboEduProgramLevel.SelectedValue);
            if (epp.EduProgram.EduLevelID != currentEduLevelId) {
                comboEduProgramLevel.SelectByValue (epp.EduProgram.EduLevelID);
                BindEduPrograms (epp.EduProgram.EduLevelID);
            }

            comboEduProgram.SelectByValue (epp.EduProgramID);
            comboEduLevel.SelectByValue (epp.EduLevelId);

            auditControl.Bind (epp);

            // sort documents
            var documents = epp.Documents
                .OrderBy (d => d.Group)
                .ThenBy (d => d.DocumentType.DocumentTypeID)
                .ThenBy (d => d.SortIndex)
                .ToList ();

            formEditDocuments.SetData (documents, epp.EduProgramProfileID);
            formEditEduForms.SetData (epp.EduProgramProfileForms.ToList (), epp.EduProgramProfileID);
        }

        protected override void OnButtonUpdateClick (object sender, EventArgs e)
        {
            // HACK: Dispose current model context used in load to create new one for update
            if (modelContext != null) {
                modelContext.Dispose ();
                modelContext = null;
            }

            base.OnButtonUpdateClick (sender, e);
        }

        protected override void BeforeUpdateItem (EduProgramProfileInfo item)
        {
            // fill the object
            item.ProfileCode = textProfileCode.Text.Trim ();
            item.ProfileTitle = textProfileTitle.Text.Trim ();
            item.Languages = textLanguages.Text.Replace (" ", string.Empty).Trim ();
            item.AccreditedToDate = dateAccreditedToDate.SelectedDate;
            item.CommunityAccreditedToDate = dateCommunityAccreditedToDate.SelectedDate;
            item.StartDate = datetimeStartDate.SelectedDate;
            item.EndDate = datetimeEndDate.SelectedDate;

            // update references
            item.EduProgramID = int.Parse (comboEduProgram.SelectedValue);
            item.EduProgram = ModelContext.Get<EduProgramInfo> (item.EduProgramID);

            item.EduLevelId = int.Parse (comboEduLevel.SelectedValue);
            item.EduLevel = ModelContext.Get<EduLevelInfo> (item.EduLevelId);

            item.DivisionId = TypeUtils.ParseToNullable<int> (treeDivision.SelectedValue);

            if (ItemId == null) {
                item.CreatedOnDate = DateTime.Now;
                item.LastModifiedOnDate = item.CreatedOnDate;
                item.CreatedByUserID = UserInfo.UserID;
                item.LastModifiedByUserID = item.CreatedByUserID;
            }
            else {
                item.LastModifiedOnDate = DateTime.Now;
                item.LastModifiedByUserID = UserInfo.UserID;

                // HACK: Set missing CreatedOnDate value
                // REVIEW: Solve on SqlDataProvider level on upgrage to 2.0.0?
                if (item.CreatedOnDate == default (DateTime)) {
                    item.CreatedOnDate = item.LastModifiedOnDate;
                    item.CreatedByUserID = item.LastModifiedByUserID;
                }
            }
        }

        protected EduProgramProfileInfo GetItemWithDependencies (int itemId)
        {
            return new EduProgramProfileEditQuery (ModelContext).SingleOrDefault (itemId);
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override EduProgramProfileInfo GetItem (int itemId)
        {
            return ModelContext.Get<EduProgramProfileInfo> (itemId);
        }

        protected override int AddItem (EduProgramProfileInfo item)
        {
            ModelContext.Add (item);

            ModelContext.SaveChanges (false);

            new UpdateDocumentsCommand (ModelContext)
                .UpdateDocuments (formEditDocuments.GetData (), DocumentModel.EduProgramProfile, item.EduProgramProfileID);

            new UpdateEduProgramProfileFormsCommand (ModelContext)
                .UpdateEduProgramProfileForms (formEditEduForms.GetData (), item.EduProgramProfileID);
            
            ModelContext.SaveChanges ();

            return item.EduProgramProfileID;
        }

        protected override void UpdateItem (EduProgramProfileInfo item)
        {
            // REVIEW: Use single transaction to update main entity along with all dependent ones?

            ModelContext.Update (item);

            new UpdateDocumentsCommand (ModelContext)
                .UpdateDocuments (formEditDocuments.GetData (), DocumentModel.EduProgramProfile, item.EduProgramProfileID);

            new UpdateEduProgramProfileFormsCommand (ModelContext)
                .UpdateEduProgramProfileForms (formEditEduForms.GetData (), item.EduProgramProfileID);

            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EduProgramProfileInfo item)
        {
            // TODO: Also remove documents

            ModelContext.Remove (item);
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
    }
}
