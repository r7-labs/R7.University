//
// ViewEmployeeDirectory.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2015
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
using System.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Icons;
using R7.University;
using R7.University.ControlExtensions;

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

            mviewEmployeeDirectory.ActiveViewIndex = Utils.GetViewIndexByID (mviewEmployeeDirectory, "view" + EmployeeDirectorySettings.Mode.ToString ());

            if (EmployeeDirectorySettings.Mode == EmployeeDirectoryMode.Search)
            {
                // display search hint
                Utils.Message (this, "SearchHint.Info", MessageType.Info, true); 

                var divisions = EmployeeDirectoryController.GetObjects <DivisionInfo> ()
                    .Where (d => d.IsPublished || IsEditable)
                    .OrderBy (d => d.Title).ToList ();
                
                divisions.Insert (0, new DivisionInfo {
                        DivisionID = Null.NullInteger, 
                        Title = LocalizeString ("AllDivisions.Text") 
                    });
               
                treeDivisions.DataSource = divisions;
                treeDivisions.DataBind ();

                // REVIEW: Level should be set in settings?
                Utils.ExpandToLevel (treeDivisions, 2);
            }
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
                    if (EmployeeDirectorySettings.Mode == EmployeeDirectoryMode.Search)
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
                    else if (EmployeeDirectorySettings.Mode == EmployeeDirectoryMode.TeachersByEduProgram)
                    {
                        var eduProfiles = EmployeeDirectoryController.GetObjects<EduProgramProfileInfoEx> ().OrderBy (epp => epp.Code).ToList ();

                        eduProfiles.Add (new EduProgramProfileInfoEx { 
                            EduProgramProfileID = Null.NullInteger,
                            Code = string.Empty,
                            Title = LocalizeString ("NoEduPrograms.Text")
                        });
 
                        if (eduProfiles.Count > 0)
                        {
                            repeaterEduPrograms.DataSource = eduProfiles;
                            repeaterEduPrograms.DataBind ();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        private int eduProfileId;

        protected void repeaterEduPrograms_ItemDataBound (object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var eduProfile = (EduProgramProfileInfoEx) e.Item.DataItem;

                // find controls in the template
                var labelEduProgram = (Label) e.Item.FindControl ("labelEduProgram");
                var literalEduProgramAnchor = (Literal) e.Item.FindControl ("literalEduProgramAnchor");
                var gridTeachersByEduProgram = (GridView) e.Item.FindControl ("gridTeachersByEduProgram");

                IEnumerable<EmployeeInfo> teachers;
                string anchorName;

                if (Null.IsNull (eduProfile.EduProgramProfileID))
                {
                    anchorName = "empty";

                    // select all teachers w/o edu programs
                    teachers = EmployeeDirectoryController.GetTeachersWithoutEduPrograms ();
                }
                else
                {
                    anchorName = eduProfile.EduProgramProfileID.ToString ();

                    // select teachers for current edu program
                    teachers = EmployeeDirectoryController.GetTeachersByEduProgramProfile (eduProfile.EduProgramProfileID);
                }

                // create anchor to simplify navigation
                literalEduProgramAnchor.Text = "<a id=\"eduprogram-" + anchorName + "\"" +
                    " name=\"eduprogram-" + anchorName + "\"></a>";

                if (teachers.Any ())
                {
                    // pass eduProfileId to gridTeachersByEduProgram_RowDataBound()
                    eduProfileId = eduProfile.EduProgramProfileID;

                    gridTeachersByEduProgram.LocalizeColumns (LocalResourceFile);

                    gridTeachersByEduProgram.DataSource = teachers;
                    gridTeachersByEduProgram.DataBind ();
                }
                else
                {
                    labelEduProgram.Visible = false;
                    gridTeachersByEduProgram.Visible = false;
                }
            }
        }

        protected void gridTeachersByEduProgram_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var teacher = (EmployeeInfo) e.Row.DataItem;

                if (IsEditable)
                {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = Utils.EditUrl (this, "EditEmployee", "employee_id", teacher.EmployeeID.ToString ());
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

                #region Order

                var literalOrder = (Literal) e.Row.FindControl ("literalOrder");
                literalOrder.Text = (e.Row.RowIndex + 1) + ".";

                #endregion

                #region Disciplines

                if (!Null.IsNull (eduProfileId))
                {
                    var literalDisciplines = (Literal) e.Row.FindControl ("literalDisciplines");

                    var discipline = EmployeeDirectoryController.GetObjects <EmployeeDisciplineInfo> (
                        "WHERE [EmployeeID] = @0 AND [EduProgramProfileID] = @1", teacher.EmployeeID, eduProfileId).FirstOrDefault ();

                    if (discipline != null) literalDisciplines.Text = discipline.Disciplines;
                }

                #endregion

                #region Positions

                var literalPositions = (Literal) e.Row.FindControl ("literalPositions");

                var positions = EmployeeDirectoryController.GetObjects <OccupiedPositionInfoEx> (
                    "WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC", teacher.EmployeeID).Select (op => Utils.FormatList (": ", op.PositionTitle, op.DivisionTitle));

                // TODO: Use OccupiedPositionInfoEx.GroupByDivision ();

                literalPositions.Text = Utils.FormatList ("; ", positions);

                #endregion

                #region Academic degrees, Academic titles, Education, Training

                // get all empoyee achievements
                var achievements = EmployeeDirectoryController.GetObjects<EmployeeAchievementInfo> (
                    CommandType.Text, "SELECT * FROM dbo.vw_University_EmployeeAchievements WHERE [EmployeeID] = @0",
                    teacher.EmployeeID).ToList ();

                var literalEducation = (Literal) e.Row.FindControl ("literalEducation");
                var literalTraining = (Literal) e.Row.FindControl ("literalTraining");
                var literalAcademicDegrees = (Literal) e.Row.FindControl ("literalAcademicDegrees");
                var literalAcademicTitles = (Literal) e.Row.FindControl ("literalAcademicTitles");

                var education = achievements.Where (ach => ach.AchievementType == AchievementType.Education).Select (ed => ed.DisplayShortTitle);
                var training = achievements.Where (ach => ach.AchievementType == AchievementType.Training).Select (ed => ed.DisplayShortTitle);
                var academicDegrees = achievements.Where (ach => ach.AchievementType == AchievementType.AcademicDegree).Select (ed => ed.DisplayShortTitle);
                var academicTitles = achievements.Where (ach => ach.AchievementType == AchievementType.AcademicTitle).Select (ed => ed.DisplayShortTitle);

                literalEducation.Text = Utils.FormatList ("; ", education);
                literalTraining.Text = Utils.FormatList ("; ", training);
                literalAcademicDegrees.Text = Utils.FormatList ("; ", academicDegrees);
                literalAcademicTitles.Text = Utils.FormatList ("; ", academicTitles);

                #endregion

                // apply obrnadzor.gov.ru microdata
                e.Row.Cells [2].Attributes.Add ("itemprop", "fio");
                e.Row.Cells [3].Attributes.Add ("itemprop", "Post");
                e.Row.Cells [4].Attributes.Add ("itemprop", "TeachingDiscipline");
                e.Row.Cells [5].Attributes.Add ("itemprop", "Degree");
                e.Row.Cells [6].Attributes.Add ("itemprop", "AcademStat");
                e.Row.Cells [7].Attributes.Add ("itemprop", "EmployeeQualification");
                e.Row.Cells [8].Attributes.Add ("itemprop", "ProfDevelopment");
                e.Row.Cells [9].Attributes.Add ("itemprop", "GenExperience");
                e.Row.Cells [10].Attributes.Add ("itemprop", "SpecExperience");

            }
        }

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
            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var employee = (EmployeeInfo) e.Row.DataItem;

                if (IsEditable)
                {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = Utils.EditUrl (this, "EditEmployee", "employee_id", employee.EmployeeID.ToString ());
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

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
                name.NavigateUrl = Utils.EditUrl (this, "EmployeeDetails", "employee_id", employee.EmployeeID.ToString ()).Replace ("550,950", "450,950");

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
                    position.Text = Utils.FormatList (": ", Utils.FormatList (" ", 
                        PositionInfo.FormatShortTitle (primePosition.PositionTitle, primePosition.PositionShortTitle), 
                        primePosition.TitleSuffix), primePosition.FormatDivisionLink (this));
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
