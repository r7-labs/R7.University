//
// EditEduProgramProfile.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015-2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University;
using R7.University.ControlExtensions;
using R7.University.Data;
using R7.University.ModelExtensions;

namespace R7.University.Launchpad
{
    public partial class EditEduProgramProfile: EditPortalModuleBase<EduProgramProfileInfo,int>
    {
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

            // get and bind edu levels
            var eduLevels = EduLevelRepository.Instance.GetEduLevels ();
            comboEduLevel.DataSource = eduLevels;
            comboEduLevel.DataBind ();

            // get and bind edu profiles
            BindEduPrograms (eduLevels.First ().EduLevelID);

            // init edit forms
            formEditEduForms.OnInit (this, UniversityRepository.Instance.DataProvider.GetObjects<EduFormInfo> ());
            formEditDocuments.OnInit (this, UniversityRepository.Instance.DataProvider.GetObjects<DocumentTypeInfo> ());
        }

        private void BindEduPrograms (int eduLevelId)
        {
            var eps = EduProgramRepository.Instance.GetEduPrograms_ByEduLevel (eduLevelId);
            comboEduProgram.DataSource = eps;
            comboEduProgram.DataBind ();
        }

        protected void comboEduLevel_SelectedIndexChanged (object sender, EventArgs e)
        {
            BindEduPrograms (int.Parse (comboEduLevel.SelectedValue));
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

            // update comboEduProgram, if needed
            var currentEduLevelId = int.Parse (comboEduLevel.SelectedValue);
            if (item.EduProgram.EduLevelID != currentEduLevelId) {
                BindEduPrograms (item.EduProgram.EduLevelID);
            }

            comboEduProgram.SelectByValue (item.EduProgramID);

            auditControl.Bind (item);

            var documents = DocumentRepository.Instance.GetDocuments_ForItemType ("EduProgramProfileID")
                .WithDocumentType (UniversityRepository.Instance.DataProvider.GetObjects<DocumentTypeInfo> ())
                .Cast<DocumentInfo> ()
                .ToList ();

            formEditDocuments.SetData (documents, item.EduProgramProfileID);

            var eppForms = UniversityRepository.Instance.DataProvider.GetObjects<EduProgramProfileFormInfo> (
                               "WHERE EduProgramProfileID = @0", item.EduProgramProfileID)
                .WithEduForms (UniversityRepository.Instance.DataProvider)
                .ToList ();
            
            formEditEduForms.SetData (eppForms, item.EduProgramProfileID);
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
            return EduProgramProfileRepository.Instance.Get (itemId);
        }

        protected override int AddItem (EduProgramProfileInfo item)
        {
            UniversityRepository.Instance.DataProvider.Add<EduProgramProfileInfo> (item);
            return item.EduProgramProfileID;
        }

        protected override void UpdateItem (EduProgramProfileInfo item)
        {
            UniversityRepository.Instance.DataProvider.Update<EduProgramProfileInfo> (item);

            // update referenced items
            DocumentRepository.Instance.UpdateDocuments (
                formEditDocuments.GetData (),
                "EduProgramProfileID",
                item.EduProgramProfileID);
            EduProgramProfileFormRepository.Instance.UpdateEduProgramProfileForms (
                formEditEduForms.GetData (),
                item.EduProgramProfileID);
        }

        protected override void DeleteItem (EduProgramProfileInfo item)
        {
            UniversityRepository.Instance.DataProvider.Delete<EduProgramProfileInfo> (item);
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

        protected int SelectedTab
        {
            get {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                if (!string.IsNullOrEmpty (eventTarget)) {
                    // check if postback initiator is on EduForms tab
                    if (eventTarget.Contains ("$" + formEditEduForms.ID)) {
                        ViewState ["SelectedTab"] = 1;
                        return 1;
                    }

                    // check if postback initiator is on Documents tab
                    if (eventTarget.Contains ("$" + formEditDocuments.ID)) {
                        ViewState ["SelectedTab"] = 2;
                        return 2;
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
    }
}
