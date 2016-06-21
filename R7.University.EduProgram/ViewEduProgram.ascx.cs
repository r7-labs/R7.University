//
//  ViewEduProgram.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
using R7.University.Data;
using R7.University.EduProgram.Components;
using R7.University.EduProgram.ViewModels;
using R7.University.ModelExtensions;
using R7.DotNetNuke.Extensions.ViewModels;

namespace R7.University.EduProgram
{
    public partial class ViewEduProgram : PortalModuleBase<EduProgramSettings>, IActionable
    {
        #region Get data

        public EduProgramModuleViewModel GetViewModel ()
        {
            if (Settings.EduProgramId != null) {
                var eduProgram = EduProgramRepository.Instance.GetEduProgram (Settings.EduProgramId.Value);
                eduProgram.EduLevel = UniversityRepository.Instance.GetEduProgramLevels ()
                    .First (el => el.EduLevelID == eduProgram.EduLevelID);

                var viewModel = new EduProgramModuleViewModel ();
                viewModel.EduProgram = new EduProgramViewModel (eduProgram, viewModel);

                viewModel.EduProgram.EduProgramProfileViewModels = EduProgramProfileRepository.Instance
                    .GetEduProgramProfiles_ByEduProgram (viewModel.EduProgram.EduProgramID)
                    .WithEduLevel (UniversityRepository.Instance.GetEduLevels ())
                    .WithEduProgram (viewModel.EduProgram)
                    .Select (epp => new EduProgramProfileViewModel (epp, viewModel));

                return viewModel;
            }

            return null;
        }

        #endregion

        #region Handlers 
     
        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad(e);
            
            try
            {
                if (!IsPostBack)
                {
                    EduProgramModuleViewModel viewModel = null;
                    if (Settings.EduProgramId != null) {
                        viewModel = GetViewModel ().SetContext (new ViewModelContext (this));
                    }

                    // check if we have some content to display, 
                    // otherwise display a message for module editors.
                    if (viewModel == null)
                    {
                        if (IsEditable) {
                            this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                        }
                        else {
                            ContainerControl.Visible = false;
                        }
                    }
                    else
                    {
                        // bind the data
                        formEduProgram.DataSource = new List<EduProgramViewModel> { viewModel.EduProgram };
                        formEduProgram.DataBind ();
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void formEduProgram_DataBound (object sender, EventArgs e)
        {
            if (formEduProgram.DataItem != null) {

                var listEduProgramProfiles = (ListView) formEduProgram.FindControl ("listEduProgramProfiles");

                var viewModel = (EduProgramViewModel) formEduProgram.DataItem;
               
                listEduProgramProfiles.DataSource = viewModel.EduProgramProfileViewModels;
                listEduProgramProfiles.DataBind ();
            }
        }

        #endregion        
            
        #region IActionable implementation
        
        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection ();
                if (Settings.EduProgramId == null) {
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
                }
                else {
                    actions.Add (
                        GetNextActionID (),
                        LocalizeString ("EditEduProgram.Action"),
                        ModuleActionType.EditContent, 
                        "", 
                        IconController.IconURL ("Edit"), 
                        EditUrl ("eduprogram_id", Settings.EduProgramId.Value.ToString (), "EditEduProgram"),
                        false, 
                        SecurityAccessLevel.Edit,
                        true, 
                        false
                    );
                }

                return actions;
            }
        }

        #endregion
    }
}

