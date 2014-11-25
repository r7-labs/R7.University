//
// ViewR7.EmployeeDirectory.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.University;

namespace R7.University.EmployeeDirectory
{
    public partial class ViewEmployeeDirectory : PortalModuleBase, IActionable
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

                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
        
        #endregion        
            
        #region IActionable implementation
        
        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
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
                    "", 
                    Utils.EditUrl (this, "Edit"),
                    false, 
                    DotNetNuke.Security.SecurityAccessLevel.Edit,
                    true, 
                    false
                );

                return actions;
            }
        }

        #endregion

        protected void linkSearch_Click (object sender, EventArgs e)
        {
            var ctrl = new EmployeeDirectoryController ();

            var employees = ctrl.GetObjects<EmployeeInfo> (
                                string.Format ("WHERE [FirstName] LIKE '%{0}%'", textSearch.Text));

            if (employees != null && employees.Any ())
            {
                gridEmployees.DataSource = employees;
                gridEmployees.DataBind ();
            }
        }

        protected void gridEmployees_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var employee = (EmployeeInfo) e.Row.DataItem;

                var name = (HyperLink) e.Row.FindControl ("linkName");
                var position = (Literal) e.Row.FindControl ("literalPosition");
                var phone = (Literal) e.Row.FindControl ("literalPhone");
                var email = (HyperLink) e.Row.FindControl ("linkEmail");
                var workingPlace = (Literal) e.Row.FindControl ("literalWorkingPlace");

                name.Text = employee.AbbrName;
                name.ToolTip = employee.FullName;

                phone.Text = employee.Phone;

                if (!string.IsNullOrWhiteSpace (employee.Email))
                {
                    email.Text = employee.Email;
                    email.NavigateUrl = "mailto:" + employee.Email;
                }
                else
                    email.Visible = false;

                workingPlace.Text = employee.WorkingPlace;

                // get prime position:
                var ctrl = new EmployeeDirectoryController ();
                var primePosition = ctrl.GetObjects <OccupiedPositionInfoEx> ("WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC", employee.EmployeeID).FirstOrDefault ();

                if (primePosition != null)
                {
                    position.Text = Utils.FormatList (" ", primePosition.PositionShortTitle, primePosition.TitleSuffix);
                }

            }
        }
    }
}

