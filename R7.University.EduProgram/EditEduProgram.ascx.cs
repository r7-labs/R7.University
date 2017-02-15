//
//  EditEduProgram.ascx.cs
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
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Commands;
using R7.University.ControlExtensions;
using R7.University.EduProgram.Components;
using R7.University.EduProgram.Queries;
using R7.University.EduProgram.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Queries;
using R7.University.Security;

namespace R7.University.EduProgram
{
    // available tabs
    public enum EditEduProgramTab
    {
        Common,
        EduProgramProfiles,
        Bindings,
        Documents
    }

    public partial class EditEduProgram : PortalModuleBase
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

        protected EditEduProgramTab SelectedTab
        {
            get {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                if (!string.IsNullOrEmpty (eventTarget)) {

                    // check if postback initiator is on Documents tab
                    if (eventTarget.Contains ("$" + formEditDocuments.ID)) {
                        ViewState ["SelectedTab"] = EditEduProgramTab.Documents;
                        return EditEduProgramTab.Documents;
                    }

                    // check if postback initiator is on Bindings tab
                    if (eventTarget.Contains ("$" + urlHomePage.ID) ||
                        eventTarget.Contains ("$" + divisionSelector.ID)) {
                        ViewState ["SelectedTab"] = EditEduProgramTab.Bindings;
                        return EditEduProgramTab.Bindings;
                    }
                }

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                if (obj != null) {
                    return (EditEduProgramTab) obj;
                }

                return EditEduProgramTab.Common;
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this)); }
        }

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo)); }
        }

        private int? itemId = null;

        #region Overrides

        /// <summary>
        /// Handles Init event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // set url for Cancel link
            linkCancel.NavigateUrl = UrlHelper.GetCancelUrl (UrlHelper.IsInPopup (Request));

            // add confirmation dialog to delete button
            buttonDelete.Attributes.Add ("onClick", "javascript:return confirm('"
                + Localization.GetString ("DeleteItem") + "');");

            // bind education levels
            comboEduLevel.DataSource = new EduLevelQuery (ModelContext).ListForEduProgram ();
            comboEduLevel.DataBind ();

            var documentTypes = new FlatQuery<DocumentTypeInfo> (ModelContext).List ();
            formEditDocuments.OnInit (this, documentTypes);

            // bind divisions
            divisionSelector.DataSource = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            divisionSelector.DataBind ();

            gridEduProgramProfiles.LocalizeColumns (LocalResourceFile);
        }

        /// <summary>
        /// Handles Load event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
            	
            if (AJAX.IsInstalled ())
                AJAX.RegisterScriptManager ();
            
            try {
                // parse querystring parameters
                itemId = TypeUtils.ParseToNullable<int> (Request.QueryString ["eduprogram_id"]);
      
                if (!IsPostBack) {
                    // load the data into the control the first time we hit this page

                    // check we have an item to lookup
                    // ALT: if (!Null.IsNull (itemId) 
                    if (itemId.HasValue) {
                        
                        // load the item
                        var item = new EduProgramQuery (ModelContext)
                            .SingleOrDefault (itemId.Value);

                        if (item != null) {
                            textCode.Text = item.Code;
                            textTitle.Text = item.Title;
                            textGeneration.Text = item.Generation;
                            datetimeStartDate.SelectedDate = item.StartDate;
                            datetimeEndDate.SelectedDate = item.EndDate;
                            comboEduLevel.SelectByValue (item.EduLevelID);
                            urlHomePage.Url = item.HomePage;
                            divisionSelector.DivisionId = item.DivisionId;

                            auditControl.Bind (item);

                            var documents = item.Documents
                                .OrderBy (d => d.Group)
                                .ThenBy (d => d.DocumentType.DocumentTypeID)
                                .ThenBy (d => d.SortIndex)
                                .ToList ();
                            
                            formEditDocuments.SetData (documents, item.EduProgramID);

                            // setup link for adding new edu. program profile
                            linkAddEduProgramProfile.NavigateUrl = EditUrl ("eduprogram_id", item.EduProgramID.ToString (), "EditEduProgramProfile");

                            gridEduProgramProfiles.DataSource = item.EduProgramProfiles
                                .Select (epp => new EduProgramProfileEditViewModel (epp, ViewModelContext))
                                .OrderBy (epp => epp.ProfileCode)
                                .ThenBy (epp => epp.ProfileTitle);

                            gridEduProgramProfiles.DataBind ();

                            buttonDelete.Visible = SecurityContext.CanDelete (item);
                        }
                        else
                            Response.Redirect (Globals.NavigateURL (), true);
                    }
                    else {
                        auditControl.Visible = false;
                        buttonDelete.Visible = false;
                    }

                    // show/hide add default profile controls
                    linkAddEduProgramProfile.Visible = itemId != null && SecurityContext.CanAdd (typeof (EduProgramProfileInfo));
                    panelAddDefaultProfile.Visible = itemId == null && SecurityContext.CanAdd (typeof (EduProgramProfileInfo));
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        #region Handlers

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
            // HACK: Dispose current model context used in load to create new one for update
            if (modelContext != null) {
                modelContext.Dispose ();
                modelContext = null;
            }

            try {
                EduProgramInfo item;

                // determine if we are adding or updating
                // ALT: if (Null.IsNull (itemId))
                if (itemId == null) {
                    // add new record
                    item = new EduProgramInfo ();
                }
                else {
                    // update existing record
                    item = ModelContext.Get<EduProgramInfo> (itemId.Value);
                }

                // fill the object
                item.Code = textCode.Text.Trim ();
                item.Title = textTitle.Text.Trim ();
                item.Generation = textGeneration.Text.Trim ();
                item.StartDate = datetimeStartDate.SelectedDate;
                item.EndDate = datetimeEndDate.SelectedDate;
                item.HomePage = urlHomePage.Url;

                // update references
                item.EduLevelID = int.Parse (comboEduLevel.SelectedValue);
                item.EduLevel = ModelContext.Get<EduLevelInfo> (item.EduLevelID);
                item.DivisionId = divisionSelector.DivisionId;

                if (itemId == null && SecurityContext.CanAdd (typeof (EduProgramInfo))) {

                    var now = DateTime.Now;
                    new AddCommand<EduProgramInfo> (ModelContext, SecurityContext).Add (item, now);
                    ModelContext.SaveChanges (false);

                    if (checkAddDefaultProfile.Checked) {
                        var defaultProfile = new EduProgramProfileInfo {
                            ProfileCode = string.Empty,
                            ProfileTitle = string.Empty,
                            EduProgramID = item.EduProgramID,
                            EduLevelId = item.EduLevelID,
                            // unpublish profile
                            EndDate = item.CreatedOnDate.Date
                        };

                        new AddCommand<EduProgramProfileInfo> (ModelContext, SecurityContext).Add (defaultProfile, now);
                        ModelContext.SaveChanges (false);
                    }

                    // update EduProgram module settings then adding new item
                    if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.EduProgram") {
                        var settingsRepository = new EduProgramSettingsRepository ();
                        var settings = settingsRepository.GetSettings (ModuleConfiguration);
                        settings.EduProgramId = item.EduProgramID;
                        settingsRepository.SaveSettings (ModuleConfiguration, settings);
                    }
                }
                else {
                    item.LastModifiedOnDate = DateTime.Now;
                    item.LastModifiedByUserID = UserInfo.UserID;

                    // REVIEW: Solve on SqlDataProvider level on upgrage to 2.0.0?
                    if (item.CreatedOnDate == default (DateTime)) {
                        item.CreatedOnDate = item.LastModifiedOnDate;
                        item.CreatedByUserID = item.LastModifiedByUserID;
                    }

                    ModelContext.Update<EduProgramInfo> (item);
                }

                // update related documents
                if (itemId != null || SecurityContext.CanAdd (typeof (EduProgramInfo))) {
                    new UpdateDocumentsCommand (ModelContext)
                        .UpdateDocuments (formEditDocuments.GetData (), DocumentModel.EduProgram, item.EduProgramID);

                    ModelContext.SaveChanges ();
                }

                ModuleController.SynchronizeModule (ModuleId);

                Response.Redirect (Globals.NavigateURL (), true);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
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
                if (itemId != null) {

                    // TODO: Also remove documents

                    var eduProgram = ModelContext.Get<EduProgramInfo> (itemId.Value);
                    new DeleteCommand<EduProgramInfo> (ModelContext, SecurityContext).Delete (eduProgram);
                    ModelContext.SaveChanges ();

                    ModuleController.SynchronizeModule (ModuleId);

                    Response.Redirect (Globals.NavigateURL (), true);
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void gridEduProgramProfiles_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                var eduProfile = (IEduProgramProfile) e.Row.DataItem;
                if (!eduProfile.IsPublished (HttpContext.Current.Timestamp)) {
                    e.Row.CssClass = gridEduProgramProfiles.GetDataRowStyle (e.Row).CssClass + " u8y-not-published";
                }
            }
        }

        #endregion
    }
}

