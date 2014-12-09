//
// ViewEmployeeDirectory.ascx.cs
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
    // TODO: Make module instances co-exist on same page

    public partial class ViewEmployeeDirectory : EmployeeDirectoryPortalModuleBase
    {
        #region Properties

        protected string SearchText
        {
            get
            { 
                var objSearchText = Session ["EmployeeDirectory.SearchText." + TabModuleId];
                return (string) objSearchText ?? string.Empty;
            }
            set { Session ["EmployeeDirectory.SearchText." + TabModuleId] = value; }
        }

        protected string SearchDivision
        {
            get
            { 
                var objSearchDivision = Session ["EmployeeDirectory.SearchDivision." + TabModuleId];
                return (string) objSearchDivision ?? Null.NullInteger.ToString ();

            }
            set { Session ["EmployeeDirectory.SearchDivision." + TabModuleId] = value; }
        }

        protected bool SearchIncludeSubdivisions
        {
            get
            { 
                var objSearchIncludeSubdivisions = Session ["EmployeeDirectory.SearchIncludeSubdivisions." + TabModuleId];
                return objSearchIncludeSubdivisions != null ? (bool) objSearchIncludeSubdivisions : false;

            }
            set { Session ["EmployeeDirectory.SearchIncludeSubdivisions." + TabModuleId] = value; }
        }

        protected bool SearchTeachersOnly
        {
            get
            { 
                var objSearchTeachersOnly = Session ["EmployeeDirectory.SearchTeachersOnly." + TabModuleId];
                return objSearchTeachersOnly != null ? (bool) objSearchTeachersOnly : false;

            }
            set { Session ["EmployeeDirectory.SearchTeachersOnly." + TabModuleId] = value; }
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles Init event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // display search hint
            Utils.Message (this, "SearchHint.Info", MessageType.Info, true); 

            var divisions = EmployeeDirectoryController.GetObjects <DivisionInfo> ("ORDER BY [Title] ASC").ToList ();
            divisions.Insert (0, new DivisionInfo {
                DivisionID = Null.NullInteger, 
                Title = LocalizeString ("AllDivisions.Text") 
            });
           
            treeDivisions.DataSource = divisions;
            treeDivisions.DataBind ();

            // REVIEW: Level should be set in settings?
            Utils.ExpandToLevel (treeDivisions, 2);
        }

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
            
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

                        // perform search
                        if (SearchParamsOK (SearchText, SearchDivision, SearchIncludeSubdivisions, false))
                            DoSearch (SearchText, SearchDivision, SearchIncludeSubdivisions, SearchTeachersOnly);
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        protected bool SearchParamsOK (string searchText, string searchDivision, bool includeSubdivisions, bool showMessages = true)
        {
            var divisionIsSpecified = Utils.ParseToNullableInt (searchDivision) != null;
            var searchTextIsEmpty = string.IsNullOrWhiteSpace (searchText);

            // no search params - shouldn't perform search
            if (searchTextIsEmpty && !divisionIsSpecified)
            {
                if (showMessages)
                    Utils.Message (this, "SearchParams.Warning", MessageType.Warning, true);

                gridEmployees.Visible = false;
                return false;
            }
                
            if ((!divisionIsSpecified || // no division specified
                (divisionIsSpecified && includeSubdivisions)) && // division specified, but subdivisions flag is set
                (searchTextIsEmpty || // search phrase is empty
                (!searchTextIsEmpty && searchText.Length < 3))) // search phrase is too short
            {
                if (showMessages)
                    Utils.Message (this, "SearchPhrase.Warning", MessageType.Warning, true);

                gridEmployees.Visible = false;
                return false;
            }

            return true;
        }

        protected void DoSearch (string searchText, string searchDivision, bool includeSubdivisions, bool teachersOnly)
        {
            var employees = EmployeeDirectoryController.FindEmployees (searchText,
                                IsEditable, teachersOnly, includeSubdivisions, searchDivision); 

            if (employees == null || !employees.Any ())
            {
                Utils.Message (this, "NoEmployeesFound.Warning", MessageType.Warning, true);
            }

            gridEmployees.DataSource = employees;
            gridEmployees.DataBind ();

            // make employees grid visible anyway
            gridEmployees.Visible = true;
        }

        /*
        protected void ResetSearch ()
        {
            // reset controls
            textSearch.Text = string.Empty;
            Utils.SelectAndExpandByValue (treeDivisions, Null.NullInteger.ToString ());
            checkIncludeSubdivisions.Checked = false;
            checkTeachersOnly.Checked = false;

            // hide employees grid
            gridEmployees.Visible = false;

            // reset saved search
            SearchText = string.Empty;
            SearchDivision = Null.NullInteger.ToString ();
            SearchIncludeSubdivisions = false;
            SearchTeachersOnly = false;
        }*/

        protected void linkSearch_Click (object sender, EventArgs e)
        {
            var searchText = textSearch.Text.Trim ();
            var searchDivision = (treeDivisions.SelectedNode != null) ? 
                treeDivisions.SelectedNode.Value : Null.NullInteger.ToString ();
            var includeSubdivisions = checkIncludeSubdivisions.Checked;
            var teachersOnly = checkTeachersOnly.Checked;

            if (SearchParamsOK (searchText, searchDivision, includeSubdivisions))
            {
                // save current search
                SearchText = searchText;
                SearchDivision = searchDivision;
                SearchIncludeSubdivisions = includeSubdivisions;
                SearchTeachersOnly = teachersOnly;

                // perform search
                DoSearch (SearchText, SearchDivision, SearchIncludeSubdivisions, SearchTeachersOnly);
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

                // mark non-published employees, as they visible only to editors
                if (!employee.IsPublished)
                {
                    if (e.Row.DataItemIndex % 2 == 0)
                        e.Row.CssClass = gridEmployees.RowStyle.CssClass + " _nonpublished";
                    else
                        e.Row.CssClass = gridEmployees.AlternatingRowStyle.CssClass + " _nonpublished";
                }

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
                var primePosition = EmployeeDirectoryController.GetObjects <OccupiedPositionInfoEx> (
                                        "WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC", employee.EmployeeID).FirstOrDefault ();

                if (primePosition != null)
                {
                    position.Text = Utils.FormatList (" ", PositionInfo.FormatShortTitle (primePosition.PositionTitle, 
                        primePosition.PositionShortTitle), primePosition.TitleSuffix);
                }

            }
            /* HACK: Set empty CssClass for gridEmployees to remove borders around empty data message
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                gridEmployees.CssClass = string.Empty;
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                gridEmployees.CssClass = "dnnGrid";
            }
            */
        }
    }
}

