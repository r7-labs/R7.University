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
using System.Resources;
using System.Threading;

namespace R7.University.EmployeeDirectory
{
    // TODO: Make module instances co-exist on same page

    public partial class ViewEmployeeDirectory : EmployeeDirectoryPortalModuleBase
    {
        #region Properties

        protected string SearchText 
        {
            get 
            { 
                var objSearchText = Session ["EmployeeDirectory.SearchText." + TabModuleId];
                if (objSearchText != null)
                    return (string) objSearchText;

                return string.Empty;
            }
            set { Session ["EmployeeDirectory.SearchText." + TabModuleId] = value; }
        }

        protected string SearchDivision
        {
            get 
            { 
                var objSearchDivision = Session ["EmployeeDirectory.SearchDivision." + TabModuleId];
                if (objSearchDivision != null)
                    return (string) objSearchDivision;

                return Null.NullInteger.ToString ();
            }
            set { Session ["EmployeeDirectory.SearchDivision." + TabModuleId] = value; }
        }

        protected bool SearchIncludeSubdivisions
        {
            get 
            { 
                var objSearchIncludeSubdivisions = Session ["EmployeeDirectory.SearchIncludeSubdivisions." + TabModuleId];
                if (objSearchIncludeSubdivisions != null)
                    return (bool) objSearchIncludeSubdivisions;

                return false;
            }
            set { Session ["EmployeeDirectory.SearchIncludeSubdivisions." + TabModuleId] = value; }
        }

        protected bool SearchTeachersOnly
        {
            get 
            { 
                var objSearchTeachersOnly = Session ["EmployeeDirectory.SearchTeachersOnly." + TabModuleId];
                if (objSearchTeachersOnly != null)
                    return (bool) objSearchTeachersOnly;

                return false;
            }
            set { Session ["EmployeeDirectory.SearchTeachersOnly." + TabModuleId] = value; }
        }

        #endregion

        #region Handlers 
        
        /// <summary>
        /// Handles Init event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit (e);

            // display search hint
            Utils.Message (this, "SearchHint.Info", MessageType.Info, true); 

            var divisions = EmployeeDirectoryController.GetObjects <DivisionInfo> ("ORDER BY [Title] ASC").ToList ();
            divisions.Insert (0, new DivisionInfo () { 
                DivisionID = Null.NullInteger, 
                Title = LocalizeString ("AllDivisions.Text") 
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
                    if (!string.IsNullOrWhiteSpace (SearchText) || !string.IsNullOrWhiteSpace (SearchDivision))
                    {
                        // restore current search
                        textSearch.Text = SearchText;
                        Utils.SelectAndExpandByValue (treeDivisions, SearchDivision);
                        checkIncludeSubdivisions.Checked = SearchIncludeSubdivisions;
                        checkTeachersOnly.Checked = SearchTeachersOnly;

                        DoSearch (SearchText, SearchDivision, SearchIncludeSubdivisions);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
        
        #endregion        

        protected IEnumerable<EmployeeInfo> FindEmployees (string searchText, string divisionId, bool recursive)
        {
            var employees = EmployeeDirectoryController.GetObjects<EmployeeInfo> (System.Data.CommandType.StoredProcedure, 
                recursive ? "University_GetRecursiveEmployeesByDivisionID" : "University_GetEmployeesByDivisionID", 
                divisionId, 0 /* sort type */, false /* show unpublished */
            );

            if (employees != null && !string.IsNullOrWhiteSpace (searchText))
            {
                searchText = searchText.ToLower ();

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
            var employees = EmployeeDirectoryController.GetObjects<EmployeeInfo>(
                string.Format(@"WHERE [FirstName] + ' ' + [LastName] + ' ' + [OtherName] LIKE N'%{0}%' 
                                   OR [Email] LIKE N'%{0}%' OR [SecondaryEmail] LIKE N'%{0}%' 
                                   OR [Phone] LIKE N'%{0}%' OR [CellPhone] LIKE N'%{0}%' 
                                   OR [WorkingPlace] LIKE N'%{0}%' 
                                   ORDER BY [LastName]", searchText));

            // REVIEW: Should also sort by the position weight! (INNER JOIN OccupiedPositions)

            return employees;
        }

        protected void DoSearch (string searchText, string searchDivision, bool recursive)
        {
            IEnumerable<EmployeeInfo> employees = null;

            if (Utils.ParseToNullableInt (searchDivision) != null)
                employees = FindEmployees (searchText, searchDivision, recursive); 
            else if (!string.IsNullOrEmpty (searchText))
                employees = FindEmployees (searchText); 

            if (employees != null && employees.Any())
            {
                gridEmployees.Visible = true;
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

        protected void ResetSearch ()
        {
            // reset controls
            textSearch.Text = string.Empty;
            Utils.SelectAndExpandByValue (treeDivisions, Null.NullInteger.ToString ());
            checkIncludeSubdivisions.Checked = false;
            checkTeachersOnly.Checked = false;

            gridEmployees.Visible = false;

            // reset saved search
            SearchText = string.Empty;
            SearchDivision = Null.NullInteger.ToString ();
            SearchIncludeSubdivisions = false;
            SearchTeachersOnly = false;
        }

        protected void linkSearch_Click (object sender, EventArgs e)
        {
            var searchText = textSearch.Text.Trim ();

            // if no division selected, check for search phrase length
            if (treeDivisions.SelectedNode.Value == Null.NullInteger.ToString () && 
                (searchText == null || searchText.Length < 3))
            {
                Utils.Message (this, "SearchPhrase.Warning", MessageType.Warning, true);

                ResetSearch ();
            }
            else
            {
                // save current search
                SearchText = searchText;
                SearchDivision = (treeDivisions.SelectedNode != null) ? 
                treeDivisions.SelectedNode.Value : Null.NullInteger.ToString();

                SearchIncludeSubdivisions = checkIncludeSubdivisions.Checked;
                SearchTeachersOnly = checkTeachersOnly.Checked;

                DoSearch(SearchText, SearchDivision, SearchIncludeSubdivisions);
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
                name.NavigateUrl = Utils.EditUrl (this, "Details", "employee_id", employee.EmployeeID.ToString ()).Replace ("550,950", "450,950");

                phone.Text = employee.Phone;

                if (!string.IsNullOrWhiteSpace (employee.Email))
                {
                    email.Text = employee.Email;
                    email.NavigateUrl = "mailto:" + employee.Email;
                }
                else
                    email.Visible = false;

                workingPlace.Text = employee.WorkingPlace;

                // try to get prime position:
                var primePosition = EmployeeDirectoryController.GetObjects <OccupiedPositionInfoEx> ("WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC", employee.EmployeeID).FirstOrDefault ();

                if (primePosition != null)
                {
                    position.Text = Utils.FormatList (" ", primePosition.PositionShortTitle, primePosition.TitleSuffix);
                }

            }
        }
    }
}

