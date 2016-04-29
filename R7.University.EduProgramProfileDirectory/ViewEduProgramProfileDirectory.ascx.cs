//
// ViewEduProgramProfileDirectory.ascx.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University;
using R7.University.ControlExtensions;
using R7.University.Data;
using R7.University.ModelExtensions;
using R7.University.ViewModels;
using R7.University.EduProgramProfileDirectory.Components;

namespace R7.University.EduProgramProfileDirectory
{
    public partial class ViewEduProgramProfileDirectory: PortalModuleBase<EduProgramProfileDirectorySettings>, IActionable
    {
        #region Properties

        protected string EditIconUrl
        {
            get { return IconController.IconURL ("Edit"); }
        }

        private ViewModelContext viewModelContext;

        protected ViewModelContext ViewModelContext
        {
            get { 
                if (viewModelContext == null)
                    viewModelContext = new ViewModelContext (this);

                return viewModelContext;
            }
        }

        #endregion

        #region Handlers

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            switch (Settings.Mode) {
                case EduProgramProfileDirectoryMode.ObrnadzorEduForms:
                    mviewEduProgramProfileDirectory.ActiveViewIndex = 1;
                    gridEduProgramProfileObrnadzorEduForms.LocalizeColumns (LocalResourceFile);
                    break;

                case EduProgramProfileDirectoryMode.ObrnadzorDocuments:
                    mviewEduProgramProfileDirectory.ActiveViewIndex = 2;
                    gridEduProgramProfileObrnadzorDocuments.LocalizeColumns (LocalResourceFile);
                    break;

                default:
                    mviewEduProgramProfileDirectory.ActiveViewIndex = 0;
                    break;
            }
        }

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
			
            try {
                if (!IsPostBack) {
                    switch (Settings.Mode) {
                        case EduProgramProfileDirectoryMode.ObrnadzorEduForms:
                            ObrnadzorEduFormsView ();
                            break;
                        
                        case EduProgramProfileDirectoryMode.ObrnadzorDocuments:
                            ObrnadzorDocumentsView ();
                            break;
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        protected void ObrnadzorEduFormsView ()
        {
            var indexer = new ViewModelIndexer (1);
            var eduLevelIds = Settings.EduLevels;

            var eduProgramProfiles = UniversityRepository.Instance.DataProvider.GetObjects<EduProgramProfileInfo> ()
                .WithEduPrograms (UniversityRepository.Instance.DataProvider)
                .Where (epp => eduLevelIds.Contains (epp.EduProgram.EduLevelID))
                .Where (epp => epp.IsPublished () || IsEditable)
                .WithEduLevel (UniversityRepository.Instance.DataProvider)
                .WithEduProgramProfileForms (UniversityRepository.Instance.DataProvider)
                .OrderBy (epp => epp.EduProgram.EduLevel.SortIndex)
                .ThenBy (epp => epp.EduProgram.Code)
                .ThenBy (epp => epp.EduProgram.Title)
                .ThenBy (epp => epp.ProfileTitle)
                .Select (epp => new EduProgramProfileObrnadzorEduFormsViewModel (epp, ViewModelContext, indexer))
                .ToList ();

            if (eduProgramProfiles.Count > 0) {
                gridEduProgramProfileObrnadzorEduForms.DataSource = eduProgramProfiles;
                gridEduProgramProfileObrnadzorEduForms.DataBind ();
            }
            else {
                this.Message ("NothingToDisplay.Text", MessageType.Info, true); 
            }
        }

        protected void ObrnadzorDocumentsView ()
        {
            var indexer = new ViewModelIndexer (1);
            var eduLevelIds = Settings.EduLevels;

            var eduProgramProfiles = UniversityRepository.Instance.DataProvider.GetObjects<EduProgramProfileInfo> ()
                .WithEduPrograms (UniversityRepository.Instance.DataProvider)
                .Where (epp => eduLevelIds.Contains (epp.EduProgram.EduLevelID))
                .Where (epp => epp.IsPublished () || IsEditable)
                .WithEduLevel (UniversityRepository.Instance.DataProvider)
                .WithDocuments (UniversityRepository.Instance.DataProvider)
                .OrderBy (epp => epp.EduProgram.EduLevel.SortIndex)
                .ThenBy (epp => epp.EduProgram.Code)
                .ThenBy (epp => epp.EduProgram.Title)
                .ThenBy (epp => epp.ProfileTitle)
                .Select (epp => new EduProgramProfileObrnadzorDocumentsViewModel (epp, ViewModelContext, indexer))
                .ToList ();

            if (eduProgramProfiles.Count > 0) {
                gridEduProgramProfileObrnadzorDocuments.DataSource = eduProgramProfiles;
                gridEduProgramProfileObrnadzorDocuments.DataBind ();
            }
            else {
                this.Message ("NothingToDisplay.Text", MessageType.Info, true); 
            }
        }

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
                // create a new action to add an item, 
                // this will be added to the controls dropdown menu
                var actions = new ModuleActionCollection ();
                actions.Add (
                    GetNextActionID (), 
                    LocalizeString ("AddEduProgramProfile.Action"),
                    ModuleActionType.AddContent, 
                    "", "", 
                    EditUrl ("EditEduProgramProfile"),
                    false, 
                    SecurityAccessLevel.Edit,
                    true, 
                    false
                );
			
                return actions;
            }
        }

        #endregion

        protected void gridEduProgramProfileObrnadzorEduForms_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // hiding the columns of second row header (created on binding)
            if (e.Row.RowType == DataControlRowType.Header) {
                // set right table section for header row
                e.Row.TableSection = TableRowSection.TableHeader;

                // TODO: Don't hardcode cell indexes
                e.Row.Cells [0].Visible = false;
                e.Row.Cells [1].Visible = false;
                e.Row.Cells [2].Visible = false;
                e.Row.Cells [3].Visible = false;
                e.Row.Cells [4].Visible = false;
                e.Row.Cells [8].Visible = false;
                e.Row.Cells [9].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow) {
                // show / hide edit column
                e.Row.Cells [0].Visible = IsEditable;

                var eduProgramProfile = (EduProgramProfileObrnadzorEduFormsViewModel) e.Row.DataItem;

                if (IsEditable) {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = EditUrl ("eduprogramprofile_id", 
                        eduProgramProfile.EduProgramProfileID.ToString (), "EditEduProgramProfile");
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

                if (!eduProgramProfile.IsPublished ()) {
                    e.Row.CssClass = "not-published";
                }
            }
        }

        protected void gridEduProgramProfileObrnadzorEduForms_RowCreated (object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header) {
                // create cells for first row
                var cellsRow1 = new []
                {
                    new TableHeaderCell
                    {
                        RowSpan = 2,
                        Visible = IsEditable
                    },
                    new TableHeaderCell
                    {
                        RowSpan = 2,
                        Text = Localization.GetString ("Index.Column", LocalResourceFile)
                    },
                    new TableHeaderCell
                    {
                        RowSpan = 2,
                        ColumnSpan = 2,
                        Text = Localization.GetString ("Title.Column", LocalResourceFile)
                    },
                    new TableHeaderCell
                    {
                        RowSpan = 2,
                        Text = Localization.GetString ("EduLevel.Column", LocalResourceFile)
                    },
                    new TableHeaderCell
                    {
                        ColumnSpan = 3,
                        Text = Localization.GetString ("TimeToLearn.Column", LocalResourceFile)
                    },
                    new TableHeaderCell
                    {
                        RowSpan = 2,
                        Text = Localization.GetString ("AccreditedToDate.Column", LocalResourceFile)
                    },
                    new TableHeaderCell
                    {
                        RowSpan = 2,
                        Text = Localization.GetString ("CommunityAccreditedToDate.Column", LocalResourceFile)
                    }
                };

                var grid = (GridView) sender;

                // create header row
                var headerRow = new GridViewRow (0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                headerRow.Cells.AddRange (cellsRow1);
                headerRow.TableSection = TableRowSection.TableHeader;

                // add new header row to the grid table
                ((Table) grid.Controls [0]).Rows.AddAt (0, headerRow);
            }
        }

        protected void gridEduProgramProfileObrnadzorDocuments_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.Header) {
                // set right table section for header row
                e.Row.TableSection = TableRowSection.TableHeader;

                // merge Code header cell into Title
                e.Row.Cells [2].Visible = false;
                e.Row.Cells [3].ColumnSpan = 2;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow) {
                var eduProgramProfile = (EduProgramProfileObrnadzorDocumentsViewModel) e.Row.DataItem;

                if (IsEditable) {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = EditUrl ("eduprogramprofile_id", 
                        eduProgramProfile.EduProgramProfileID.ToString (), "EditEduProgramProfile");
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

                if (!eduProgramProfile.IsPublished ()) {
                    e.Row.CssClass = "not-published";
                }
            }
        }
    }
}

