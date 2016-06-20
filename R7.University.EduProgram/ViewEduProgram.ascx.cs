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
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Data;
using R7.University.EduProgram.Components;
using R7.University.Models;

namespace R7.University.EduProgram
{
    public partial class ViewEduProgram : PortalModuleBase<EduProgramSettings>, IActionable
    {
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
                    IEduProgram eduProgram = null;
                    if (Settings.EduProgramId != null) {
                        eduProgram = EduProgramRepository.Instance.GetEduProgram (Settings.EduProgramId.Value);
                    }

                    // check if we have some content to display, 
                    // otherwise display a message for module editors.
                    if (eduProgram == null)
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
                        formEduProgram.DataSource = new List<IEduProgram> { eduProgram };
                        formEduProgram.DataBind ();
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
        
        #endregion        
            
        #region IActionable implementation
        
        public ModuleActionCollection ModuleActions
        {
            get
            {
                // create a new action to add an item, this will be added 
                // to the controls dropdown menu
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

                if (Settings.EduProgramId != null) {
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

