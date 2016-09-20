//
//  ViewEduProgramProfileDirectory.ascx.cs
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
using System.Web.Caching;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.EduProgramProfileDirectory.Components;
using R7.University.EduProgramProfileDirectory.Queries;
using R7.University.EduProgramProfileDirectory.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfileDirectory
{
    public partial class ViewEduProgramProfileDirectory: PortalModuleBase<EduProgramProfileDirectorySettings>, IActionable
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

        #region Get data

        internal EduProgramProfileDirectoryEduFormsViewModel GetEduFormsViewModel ()
        {
            return DataCache.GetCachedData<EduProgramProfileDirectoryEduFormsViewModel> (
                new CacheItemArgs ("//r7_University/Modules/EduProgramProfileDirectory?ModuleId=" + ModuleId, 
                    UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetEduFormsViewModel_Internal ()
            ).SetContext (ViewModelContext);
        }

        internal EduProgramProfileDirectoryDocumentsViewModel GetDocumentsViewModel ()
        {
            return DataCache.GetCachedData<EduProgramProfileDirectoryDocumentsViewModel> (
                new CacheItemArgs ("//r7_University/Modules/EduProgramProfileDirectory?ModuleId=" + ModuleId, 
                    UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetDocumentsViewModel_Internal ()
            ).SetContext (ViewModelContext);
        }

        internal EduProgramProfileDirectoryEduFormsViewModel GetEduFormsViewModel_Internal ()
        {
            var viewModel = new EduProgramProfileDirectoryEduFormsViewModel ();
            var indexer = new ViewModelIndexer (1);

            var eduProgramProfiles = new EduProgramProfileQuery (ModelContext)
                .ListWithEduForms (Settings.EduLevels, Settings.DivisionId, Settings.DivisionLevel);
               
            viewModel.EduProgramProfiles = new IndexedEnumerable<EduProgramProfileObrnadzorEduFormsViewModel> (indexer,
                eduProgramProfiles.Select (epp => new EduProgramProfileObrnadzorEduFormsViewModel (epp, viewModel, indexer))
            );

            return viewModel;
        }

        internal EduProgramProfileDirectoryDocumentsViewModel GetDocumentsViewModel_Internal ()
        {
            var viewModel = new EduProgramProfileDirectoryDocumentsViewModel ();
            var indexer = new ViewModelIndexer (1);

            var eduProgramProfiles = new EduProgramProfileQuery (ModelContext)
                .ListWithDocuments (Settings.EduLevels, Settings.DivisionId, Settings.DivisionLevel);

            viewModel.EduProgramProfiles = new IndexedEnumerable<EduProgramProfileObrnadzorDocumentsViewModel> (indexer,
                eduProgramProfiles.Select (epp => new EduProgramProfileObrnadzorDocumentsViewModel (epp, viewModel, indexer))
            );

            return viewModel;
        }

        #endregion

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
                    "", 
                    IconController.IconURL ("Add"), 
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

        protected void ObrnadzorEduFormsView ()
        {
            var now = HttpContext.Current.Timestamp;
            var eduProgramProfiles = GetEduFormsViewModel ().EduProgramProfiles
                .Where (epp => epp.IsPublished (now) || IsEditable);

            if (!eduProgramProfiles.IsNullOrEmpty ()) {
                gridEduProgramProfileObrnadzorEduForms.DataSource = eduProgramProfiles;
                gridEduProgramProfileObrnadzorEduForms.DataBind ();
            }
            else {
                this.Message ("NothingToDisplay.Text", MessageType.Info, true); 
            }
        }

        protected void ObrnadzorDocumentsView ()
        {
            var now = HttpContext.Current.Timestamp;
            var eduProgramProfiles = GetDocumentsViewModel ().EduProgramProfiles
                .Where (epp => epp.IsPublished (now) || IsEditable);

            if (!eduProgramProfiles.IsNullOrEmpty ()) {
                gridEduProgramProfileObrnadzorDocuments.DataSource = eduProgramProfiles;
                gridEduProgramProfileObrnadzorDocuments.DataBind ();
            }
            else {
                this.Message ("NothingToDisplay.Text", MessageType.Info, true); 
            }
        }

        protected void gridEduProgramProfileObrnadzorEduForms_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            var now = HttpContext.Current.Timestamp;

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

                if (!eduProgramProfile.IsPublished (now)) {
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
            var now = HttpContext.Current.Timestamp;

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

                if (!eduProgramProfile.IsPublished (now)) {
                    e.Row.CssClass = "not-published";
                }
            }
        }

        #endregion
    }
}

