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
    public partial class ViewEmployeeDirectory : PortalModuleBase
    {
        #region Handlers 
        
        /// <summary>
        /// Handles Init event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit (e);

            var ctrl = new EmployeeDirectoryController ();

            var divisions = ctrl.GetObjects <DivisionInfo> ("ORDER BY [Title] ASC").ToList ();
            divisions.Insert (0, new DivisionInfo () { 
                DivisionID = Null.NullInteger, 
                Title = LocalizeString ("NotSelected.Text") 
            });
           
            treeDivisions.DataSource = divisions;
            treeDivisions.DataBind ();

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

        protected IEnumerable<EmployeeInfo> FindEmployees (int divisionId, string searchText)
        {
            var ctrl = new EmployeeDirectoryController ();
            const bool recursive = false;

            var employees = ctrl.GetObjects<EmployeeInfo> (System.Data.CommandType.StoredProcedure, 
                recursive ? "University_GetRecursiveEmployeesByDivisionID" : "University_GetEmployeesByDivisionID", 
                divisionId, 0 /* sort type */, false /* show unpublished */
            );

            if (employees != null)
            {
                return employees.Where (em => 
                    em.FullName.ToLower().Contains (searchText) || 
                    em.Email.ToLower().Contains (searchText) || 
                    em.SecondaryEmail.ToLower().Contains (searchText) || 
                    em.Phone.ToLower().Contains (searchText) ||
                    em.CellPhone.ToLower().Contains (searchText) ||
                    em.WorkingPlace.ToLower().Contains (searchText)
                );
            }

            return employees;
        }

        protected IEnumerable<EmployeeInfo> FindEmployees (string searchText)
        {
            var ctrl = new EmployeeDirectoryController ();

            var employees = ctrl.GetObjects<EmployeeInfo>(
                string.Format(@"WHERE [FirstName] + ' ' + [LastName] + ' ' + [OtherName] LIKE N'%{0}%' 
                                   OR [Email] LIKE N'%{0}%' OR [SecondaryEmail] LIKE N'%{0}%'
                                   OR [Phone] LIKE N'%{0}%' OR [CellPhone] LIKE N'%{0}%'
                                   OR [WorkingPlace] LIKE N'%{0}%'
                                   ORDER BY [LastName]", searchText));

            // REVIEW: Should also sort by the position weight! (INNER JOIN OccupiedPositions)

            return employees;
        }

        protected void linkSearch_Click (object sender, EventArgs e)
        {
            var searchText = textSearch.Text.ToLower ();

            IEnumerable<EmployeeInfo> employees;

            if (treeDivisions.SelectedNode != null && treeDivisions.SelectedNode.Value != Null.NullInteger.ToString())
                employees = FindEmployees (int.Parse (treeDivisions.SelectedNode.Value), searchText); 
            else
                employees = FindEmployees (searchText); 

            if (employees != null && employees.Any())
            {
                gridEmployees.DataSource = employees;
                gridEmployees.DataBind();
            }
            else
            {
                // REVIEW: Show message

                // nothing found
                gridEmployees.Visible = false;
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

