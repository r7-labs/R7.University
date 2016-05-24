//
// ViewEduProgramDirectory.ascx.cs
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
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Security;
using R7.University;
using R7.University.ModelExtensions;
using R7.University.ControlExtensions;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.University.Data;
using R7.University.ViewModels;
using R7.University.EduProgramDirectory.Components;
using R7.University.Models;

namespace R7.University.EduProgramDirectory
{
    public partial class ViewEduProgramDirectory: PortalModuleBase<EduProgramDirectorySettings>, IActionable
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
                    var viewModelIndexer = new ViewModelIndexer (1);

                    // REVIEW: Order / group by edu level first?
                    var eduPrograms = EduProgramRepository.Instance.GetEduPrograms_ByEduLevels (Settings.EduLevels, IsEditable)
                        .WithDocuments (UniversityRepository.Instance.DataProvider)
                        .WithEduLevel (UniversityRepository.Instance.DataProvider)
                        .OrderBy (ep => ep.EduLevel.SortIndex)
                        .ThenBy (ep => ep.Code)
                        .ThenBy (ep => ep.Title)
                        .Select (ep => new EduProgramStandardObrnadzorViewModel (
                                              ep,
                                              ViewModelContext,
                                              viewModelIndexer))
                        .ToList ();
                    
                    if (eduPrograms.Count > 0) {
                        gridEduStandards.DataSource = eduPrograms;
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
                    "", "", 
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

