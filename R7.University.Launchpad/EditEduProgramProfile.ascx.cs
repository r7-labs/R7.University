//
//  EditEduProgramProfile.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using System.Text.RegularExpressions;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.ControlExtensions;
using R7.University.Data;
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
            var eduProgramLevels = new EduProgramLevelsQuery (ModelContext).Execute ();
            comboEduProgramLevel.DataSource = eduProgramLevels;
            comboEduProgramLevel.DataBind ();

            // get and bind edu. profiles
            BindEduPrograms (eduProgramLevels.First ().EduLevelID);

            // init edit forms
            formEditEduForms.OnInit (this, new Query<EduFormInfo> (ModelContext).Execute ());
            formEditDocuments.OnInit (this, new Query<DocumentTypeInfo> (ModelContext).Execute ());

            // fill divisions dropdown
            var divisions = ModelContext.QueryDivisions ().ToList ();
            divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

            treeDivision.DataSource = divisions;
            treeDivision.DataBind ();
        }

        private void BindEduPrograms (int eduLevelId)
        {
            comboEduProgram.DataSource = new EduProgramsByEduLevelQuery (ModelContext).Execute (eduLevelId);
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
            textProfileCode.Text = item.ProfileCode;
            textProfileTitle.Text = item.ProfileTitle;
            textLanguages.Text = item.Languages;
            dateAccreditedToDate.SelectedDate = item.AccreditedToDate;
            dateCommunityAccreditedToDate.SelectedDate = item.CommunityAccreditedToDate;
            datetimeStartDate.SelectedDate = item.StartDate;
            datetimeEndDate.SelectedDate = item.EndDate;
            comboEduLevel.SelectByValue (item.EduLevelId);
            treeDivision.SelectAndExpandByValue (item.DivisionId.ToString ());

            // update comboEduProgram, if needed
            var currentEduLevelId = int.Parse (comboEduProgramLevel.SelectedValue);
            if (item.EduProgram.EduLevelID != currentEduLevelId) {
                comboEduProgramLevel.SelectByValue (item.EduProgram.EduLevelID);
                BindEduPrograms (item.EduProgram.EduLevelID);
            }

            comboEduProgram.SelectByValue (item.EduProgramID);
            comboEduLevel.SelectByValue (item.EduLevelId);

            auditControl.Bind (item);

            // sort documents
            var documents = item.Documents
                .OrderBy (d => d.Group)
                .ThenBy (d => d.DocumentType.DocumentTypeID)
                .ThenBy (d => d.SortIndex)
                .ToList ();

            formEditDocuments.SetData (documents, item.EduProgramProfileID);
            formEditEduForms.SetData (item.EduProgramProfileForms.ToList (), item.EduProgramProfileID);
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
            item.EduProgramID = int.Parse (comboEduProgram.SelectedValue);
            item.EduLevelId = int.Parse (comboEduLevel.SelectedValue);
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

        #region implemented abstract members of EditPortalModuleBase

        protected override EduProgramProfileInfo GetItem (int itemId)
        {
            return new EduProgramProfileEditQuery (ModelContext).SingleOrDefault (itemId);
        }

        protected override int AddItem (EduProgramProfileInfo item)
        {
            ModelContext.Add (item);
            ModelContext.SaveChanges (true);

            return item.EduProgramProfileID;
        }

        protected override void UpdateItem (EduProgramProfileInfo item)
        {
            ModelContext.Update (item);
            ModelContext.SaveChanges (true);

            // update referenced items
            DocumentRepository.Instance.UpdateDocuments (
                formEditDocuments.GetData (),
                "EduProgramProfile",
                item.EduProgramProfileID);
            EduProgramProfileFormRepository.Instance.UpdateEduProgramProfileForms (
                formEditEduForms.GetData (),
                item.EduProgramProfileID);
        }

        protected override void DeleteItem (EduProgramProfileInfo item)
        {
            ModelContext.Remove (item);
            ModelContext.SaveChanges (true);
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
