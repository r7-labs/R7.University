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
using System.Collections.Generic;
using System.Linq;
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
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.EduProgram
{
    // available tabs
    public enum EditEduProgramTab
    {
        Common,
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
                    if (eventTarget.Contains ("$" + urlHomePage.ID)) {
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

        private ViewModelContext viewModelContext;

        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this)); }
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
            linkCancel.NavigateUrl = Globals.NavigateURL ();

            // add confirmation dialog to delete button
            buttonDelete.Attributes.Add ("onClick", "javascript:return confirm('"
                + Localization.GetString ("DeleteItem") + "');");

            // bind education levels
            comboEduLevel.DataSource = new EduLevelQuery (ModelContext).ListForEduProgram ();
            comboEduLevel.DataBind ();

            var documentTypes = new FlatQuery<DocumentTypeInfo> (ModelContext).List ();
            formEditDocuments.OnInit (this, documentTypes);

            // fill divisions treeview
            var divisions = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

            treeDivision.DataSource = divisions;
            treeDivision.DataBind ();
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
                        var item = new R7.University.EduProgram.Queries.EduProgramQuery (ModelContext)
                            .SingleOrDefault (itemId.Value);

                        if (item != null) {
                            textCode.Text = item.Code;
                            textTitle.Text = item.Title;
                            textGeneration.Text = item.Generation;
                            datetimeStartDate.SelectedDate = item.StartDate;
                            datetimeEndDate.SelectedDate = item.EndDate;
                            comboEduLevel.SelectByValue (item.EduLevelID);
                            urlHomePage.Url = item.HomePage;
                            treeDivision.SelectAndExpandByValue (item.DivisionId.ToString ());

                            auditControl.Bind (item);

                            var documents = item.Documents
                                .OrderBy (d => d.Group)
                                .ThenBy (d => d.DocumentType.DocumentTypeID)
                                .ThenBy (d => d.SortIndex)
                                .ToList ();
                            
                            formEditDocuments.SetData (documents, item.EduProgramID);
                        }
                        else
                            Response.Redirect (Globals.NavigateURL (), true);
                    }
                    else {
                        auditControl.Visible = false;
                        buttonDelete.Visible = false;
                    }
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
                var isNew = false;

                // determine if we are adding or updating
                // ALT: if (Null.IsNull (itemId))
                if (!itemId.HasValue) {
                    // add new record
                    item = new EduProgramInfo ();
                    isNew = true;
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
                item.DivisionId = TypeUtils.ParseToNullable<int> (treeDivision.SelectedValue);

                if (itemId == null) {
                    item.CreatedOnDate = DateTime.Now;
                    item.LastModifiedOnDate = item.CreatedOnDate;
                    item.CreatedByUserID = UserInfo.UserID;
                    item.LastModifiedByUserID = item.CreatedByUserID;

                    ModelContext.Add<EduProgramInfo> (item);
                    ModelContext.SaveChanges (false);
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

                // update EduProgram module settings then adding new item
                if (isNew && ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.EduProgram") {
                    var settings = new EduProgramSettings (this);
                    settings.EduProgramId = item.EduProgramID;
                }

                // update related documents
                new UpdateDocumentsCommand (ModelContext)
                    .UpdateDocuments (formEditDocuments.GetData (), DocumentModel.EduProgram, item.EduProgramID);
                
                ModelContext.SaveChanges ();

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

                    var item = ModelContext.Get<EduProgramInfo> (itemId.Value);
                    ModelContext.Remove (item);
                    ModelContext.SaveChanges ();

                    ModuleController.SynchronizeModule (ModuleId);

                    Response.Redirect (Globals.NavigateURL (), true);
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion
    }
}

