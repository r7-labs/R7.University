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
using R7.DotNetNuke.Extensions.Data;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Data;
using R7.University.EduProgram.Components;

namespace R7.University.EduProgram
{
    public partial class ViewEduProgram : PortalModuleBase<EduProgramSettings>, IActionable
    {
        #region Handlers 
        
        /// <summary>
        /// Handles Init event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit (e);
        }
                
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
                    var dataProvider = new Dal2DataProvider ();
                    var items = dataProvider.GetObjects<EduProgramInfo> ();

                    // check if we have some content to display, 
                    // otherwise display a message for module editors.
                    if (items == null && IsEditable)
                    {
                        this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                    }
                    else
                    {
                        // bind the data
                        lstContent.DataSource = items;
                        lstContent.DataBind ();
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
                    Localization.GetString (ModuleActionType.AddContent, this.LocalResourceFile),
                    ModuleActionType.AddContent, 
                    "", 
                    IconController.IconURL ("Add"), 
                    EditUrl ("Edit"),
                    false, 
                    SecurityAccessLevel.Edit,
                    true, 
                    false
                );

                return actions;
            }
        }

        #endregion
    }
}

