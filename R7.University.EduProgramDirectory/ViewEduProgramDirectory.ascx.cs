//
//  ViewEduProgramDirectory.ascx.cs
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
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.ControlExtensions;
using R7.University.Data;
using R7.University.EduProgramDirectory.Components;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgramDirectory
{
    public partial class ViewEduProgramDirectory: PortalModuleBase<EduProgramDirectorySettings>, IActionable
    {
        #region Repository handling

        private UniversityDataRepository repository;
        protected UniversityDataRepository Repository
        {
            get { return repository ?? (repository = new UniversityDataRepository ()); }
        }

        public override void Dispose ()
        {
            if (repository != null) {
                repository.Dispose ();
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

        #region Handlers

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            gridEduStandards.LocalizeColumns (LocalResourceFile);
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
                    IEnumerable<IEduProgram> baseEduPrograms;
                    if (Settings.DivisionId == null) {
                        baseEduPrograms = EduProgramRepository.Instance.GetEduPrograms_ByEduLevels (Settings.EduLevels);
                    }
                    else {
                        baseEduPrograms = EduProgramRepository.Instance.GetEduPrograms_ByDivisionAndEduLevels (Settings.DivisionId.Value, Settings.EduLevels);
                    }

                    var viewModelIndexer = new ViewModelIndexer (1);
                    var eduPrograms = baseEduPrograms
                        .WithDocuments (Repository.QueryDocuments_ByItemType ("EduProgramID"))
                        .WithEduLevel (Repository.Query<EduLevelInfo> ().ToList ())
                        .OrderBy (ep => ep.EduLevel.SortIndex)
                        .ThenBy (ep => ep.Code)
                        .ThenBy (ep => ep.Title)
                        .Select (ep => new EduProgramStandardObrnadzorViewModel (
                                              ep,
                                              ViewModelContext,
                                              viewModelIndexer))
                        .ToList ();
                    
                    if (eduPrograms.Count > 0) {
                        gridEduStandards.DataSource = eduPrograms.Where (ep => ep.IsPublished () || IsEditable);
                        gridEduStandards.DataBind ();
                    }
                    else {
                        this.Message ("NothingToDisplay.Text", MessageType.Info, true); 
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
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
                    LocalizeString ("AddEduProgram.Action"),
                    ModuleActionType.AddContent, 
                    "", 
                    IconController.IconURL ("Add"),
                    EditUrl ("EditEduProgram"),
                    false, 
                    SecurityAccessLevel.Edit,
                    true, 
                    false
                );
			
                return actions;
            }
        }

        #endregion

        protected void grid_RowCreated (object sender, GridViewRowEventArgs e)
        {
            // table header row should be inside <thead> tag
            if (e.Row.RowType == DataControlRowType.Header) {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gridEduStandards_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.Header) {
                e.Row.Cells [3].CssClass = "u8y-column u8y-expand"; 
            }

            // show or hide additional columns
            e.Row.Cells [4].Visible = Settings.Columns.Contains (EduProgramDirectoryColumn.EduLevel.ToString ());
            e.Row.Cells [5].Visible = Settings.Columns.Contains (EduProgramDirectoryColumn.Generation.ToString ());
            e.Row.Cells [6].Visible = Settings.Columns.Contains (EduProgramDirectoryColumn.ProfStandard.ToString ());

            if (e.Row.RowType == DataControlRowType.DataRow) {
                var eduProgram = (IEduProgram) e.Row.DataItem;

                if (IsEditable) {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = EditUrl (
                        "eduprogram_id",
                        eduProgram.EduProgramID.ToString (),
                        "EditEduProgram");
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

                if (!eduProgram.IsPublished ()) {
                    e.Row.CssClass = "not-published";
                }
            }
        }
    }
}

