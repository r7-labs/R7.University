//
// ViewEmployeeDirectory.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2016 Roman M. Yagodin
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
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.Data;
using R7.University.EmployeeDirectory.Components;
using R7.University.EmployeeDirectory.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EmployeeDirectory
{
    // TODO: Make module instances co-exist on same page

    public partial class ViewEmployeeDirectory: PortalModuleBase<EmployeeDirectorySettings>
    {
        #region Properties

        private ViewModelContext viewModelContext;

        protected ViewModelContext ViewModelContext
        {
            get { 
                if (viewModelContext == null)
                    viewModelContext = new ViewModelContext (this);

                return viewModelContext;
            }
        }

        protected string SearchText
        {
            get { 
                var objSearchText = Session ["EmployeeDirectory.SearchText." + TabModuleId];
                return (string) objSearchText ?? string.Empty;
            }
            set { Session ["EmployeeDirectory.SearchText." + TabModuleId] = value; }
        }

        protected string SearchDivision
        {
            get { 
                var objSearchDivision = Session ["EmployeeDirectory.SearchDivision." + TabModuleId];
                return (string) objSearchDivision ?? Null.NullInteger.ToString ();

            }
            set { Session ["EmployeeDirectory.SearchDivision." + TabModuleId] = value; }
        }

        protected bool SearchIncludeSubdivisions
        {
            get { 
                var objSearchIncludeSubdivisions = Session ["EmployeeDirectory.SearchIncludeSubdivisions." + TabModuleId];
                return objSearchIncludeSubdivisions != null ? (bool) objSearchIncludeSubdivisions : false;
            }
            set { Session ["EmployeeDirectory.SearchIncludeSubdivisions." + TabModuleId] = value; }
        }

        protected bool SearchTeachersOnly
        {
            get { 
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

            mviewEmployeeDirectory.ActiveViewIndex = R7.University.Utilities.Utils.GetViewIndexByID (
                mviewEmployeeDirectory,
                "view" + Settings.Mode);

            if (Settings.Mode == EmployeeDirectoryMode.Search) {
                // display search hint
                this.Message ("SearchHint.Info", MessageType.Info, true); 

                var divisions = UniversityRepository.Instance.DataProvider.GetObjects <DivisionInfo> ()
                    .Where (d => d.IsPublished || IsEditable)
                    .OrderBy (d => d.Title).ToList ();
                
                divisions.Insert (0, new DivisionInfo
                    {
                        DivisionID = Null.NullInteger, 
                        Title = LocalizeString ("AllDivisions.Text") 
                    });
               
                treeDivisions.DataSource = divisions;
                treeDivisions.DataBind ();

                // REVIEW: Level should be set in settings?
                R7.University.Utilities.Utils.ExpandToLevel (treeDivisions, 2);
            }
        }

        protected IList<EduProgramProfileViewModel> GetEduProgramProfileViewModels ()
        {
            return DataCache.GetCachedData<IList<EduProgramProfileViewModel>> (
                new CacheItemArgs ("//r7_University/Modules/EmployeeDirectory?ModuleId=" + ModuleId,
                    UniversityConfig.Instance.DataCacheTime, System.Web.Caching.CacheItemPriority.Normal),
                c => GetEduProgramProfileViewModels_Internal ()
            );
        }

        protected IList<EduProgramProfileViewModel> GetEduProgramProfileViewModels_Internal ()
        {
            var eduProgramProfiles = EduProgramProfileRepository.Instance.GetEduProgramProfiles_ByEduLevels (Settings.EduLevels)
                
                .WithEduLevel (UniversityRepository.Instance.DataProvider)
                .OrderBy (epp => epp.EduProgram.EduLevel.SortIndex)
                .ThenBy (epp => epp.EduProgram.Code)
                .ThenBy (epp => epp.EduProgram.Title)
                .ThenBy (epp => epp.ProfileCode)
                .ThenBy (epp => epp.ProfileTitle)
                .Select (epp => new EduProgramProfileViewModel (epp, ViewModelContext))
                .ToList ();

            if (Settings.ShowAllTeachers) {
                eduProgramProfiles.Add (new EduProgramProfileViewModel (
                    new EduProgramProfileInfo {
                        EduProgramProfileID = Null.NullInteger,
                        EduProgram = new EduProgramInfo
                            {
                                Code = string.Empty,
                                Title = LocalizeString ("NoDisciplines.Text")
                            }
                    }, ViewModelContext)
                );
            }

            if (eduProgramProfiles.Count > 0) {
                
                var teachers = EmployeeRepository.Instance.GetTeachers ()
                    .WithDisciplines (UniversityRepository.Instance.DataProvider
                        .GetObjects<EmployeeDisciplineInfo> ())
                    .WithOccupiedPositions (UniversityRepository.Instance.DataProvider
                        .GetObjects<OccupiedPositionInfoEx> ())
                    .WithAchievements (EmployeeAchievementRepository.Instance.GetEmployeeAchievements ());


                IEnumerable<IEmployee> eduProgramProfileTeachers;

                foreach (var eduProgramProfile in eduProgramProfiles) {
                    if (!Null.IsNull (eduProgramProfile.EduProgramProfileID)) {
                        eduProgramProfileTeachers = teachers
                            .Where (t => t.Disciplines.Any (
                                d => d.EduProgramProfileID == eduProgramProfile.EduProgramProfileID));
                    }
                    else {
                        // get teachers w/o disciplines
                        eduProgramProfileTeachers = teachers
                            .Where (t => t.Disciplines.IsNullOrEmpty ());
                    }

                    var indexer = new ViewModelIndexer (1);
                    eduProgramProfile.Teachers = new IndexedEnumerable<TeacherViewModel> (indexer,
                        eduProgramProfileTeachers
                            .OrderBy (t => t.LastName)
                            .ThenBy (t => t.FirstName)
                            .Select (t => new TeacherViewModel (t, eduProgramProfile, ViewModelContext, indexer))
                    );
                }
            }

            return eduProgramProfiles;
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
                    if (Settings.Mode == EmployeeDirectoryMode.Search) {
                        if (!string.IsNullOrWhiteSpace (SearchText) || !string.IsNullOrWhiteSpace (SearchDivision)) {
                            // restore current search
                            textSearch.Text = SearchText;
                            R7.University.Utilities.Utils.SelectAndExpandByValue (treeDivisions, SearchDivision);
                            checkIncludeSubdivisions.Checked = SearchIncludeSubdivisions;
                            checkTeachersOnly.Checked = SearchTeachersOnly;

                            // perform search
                            if (SearchParamsOK (SearchText, SearchDivision, SearchIncludeSubdivisions, false))
                                DoSearch (SearchText, SearchDivision, SearchIncludeSubdivisions, SearchTeachersOnly);
                        }
                    }
                    else if (Settings.Mode == EmployeeDirectoryMode.Teachers) {
                        repeaterEduProgramProfiles.DataSource = GetEduProgramProfileViewModels ()
                            .Where (epp => epp.IsPublished () || IsEditable);
                        repeaterEduProgramProfiles.DataBind ();
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        protected void repeaterEduProgramProfiles_ItemDataBound (object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
                var eduProgramProfile = (EduProgramProfileViewModel) e.Item.DataItem;

                // find controls in the template
                var panelTeachers = (Panel) e.Item.FindControl ("panelTeachers");
                var literalEduProgramProfileAnchor = (Literal) e.Item.FindControl ("literalEduProgramProfileAnchor");
                var gridTeachers = (GridView) e.Item.FindControl ("gridTeachers");

                // create anchor to simplify navigation
                var anchorName = (Null.IsNull (eduProgramProfile.EduProgramProfileID)) ? "none" : eduProgramProfile.EduProgramProfileID.ToString ();
                literalEduProgramProfileAnchor.Text = "<a id=\"eduprogramprofile-" + anchorName + "\"" +
                " name=\"eduprogramprofile-" + anchorName + "\"></a>";

                var publishedTeachers = eduProgramProfile.Teachers.Where (t => IsEditable || t.IsPublished ());
                if (publishedTeachers.Any ()) {
                    // mark edu. program profile as not published
                    if (!eduProgramProfile.IsPublished ()) {
                        panelTeachers.CssClass = "not-published";
                    }

                    gridTeachers.LocalizeColumns (LocalResourceFile);
                    gridTeachers.DataSource = publishedTeachers;
                    gridTeachers.DataBind ();
                }
                else {
                    panelTeachers.Visible = false;
                }
            }
        }

        protected void gridTeachers_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.DataRow) {
                var teacher = (TeacherViewModel) e.Row.DataItem;

                if (IsEditable) {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = EditUrl ("employee_id", teacher.EmployeeID.ToString (), "EditEmployee");
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

                if (!teacher.IsPublished ()) {
                    // mark teacher as not published
                    e.Row.CssClass += " not-published";
                }

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

        protected bool SearchParamsOK (
            string searchText,
            string searchDivision,
            bool includeSubdivisions,
            bool showMessages = true)
        {
            var divisionIsSpecified = TypeUtils.ParseToNullable<int> (searchDivision) != null;
            var searchTextIsEmpty = string.IsNullOrWhiteSpace (searchText);

            // no search params - shouldn't perform search
            if (searchTextIsEmpty && !divisionIsSpecified) {
                if (showMessages)
                    this.Message ("SearchParams.Warning", MessageType.Warning, true);

                gridEmployees.Visible = false;
                return false;
            }
                
            if ((!divisionIsSpecified || // no division specified
                (divisionIsSpecified && includeSubdivisions)) && // division specified, but subdivisions flag is set
                (searchTextIsEmpty || // search phrase is empty
                (!searchTextIsEmpty && searchText.Length < 3))) { // search phrase is too short
                if (showMessages)
                    this.Message ("SearchPhrase.Warning", MessageType.Warning, true);

                gridEmployees.Visible = false;
                return false;
            }

            return true;
        }

        protected void DoSearch (string searchText, string searchDivision, bool includeSubdivisions, bool teachersOnly)
        {
            var employees = EmployeeRepository.Instance.FindEmployees (searchText, 
                                IsEditable, teachersOnly, includeSubdivisions, searchDivision);
            
            if (employees.IsNullOrEmpty ()) {
                this.Message ("NoEmployeesFound.Warning", MessageType.Warning, true);
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

            if (SearchParamsOK (searchText, searchDivision, includeSubdivisions)) {
                // save current search
                SearchText = searchText;
                SearchDivision = searchDivision;
                SearchIncludeSubdivisions = includeSubdivisions;
                SearchTeachersOnly = teachersOnly;

                // perform search
                DoSearch (SearchText, SearchDivision, SearchIncludeSubdivisions, SearchTeachersOnly);
            }
        }

        protected void grid_RowCreated (object sender, GridViewRowEventArgs e)
        {
            // table header row should be inside <thead> tag
            if (e.Row.RowType == DataControlRowType.Header) {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gridEmployees_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.DataRow) {
                var employee = (EmployeeInfo) e.Row.DataItem;

                if (IsEditable) {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (), "EditEmployee");
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

                var name = (HyperLink) e.Row.FindControl ("linkName");
                var position = (Literal) e.Row.FindControl ("literalPosition");
                var phone = (Literal) e.Row.FindControl ("literalPhone");
                var email = (HyperLink) e.Row.FindControl ("linkEmail");
                var workingPlace = (Literal) e.Row.FindControl ("literalWorkingPlace");

                name.Text = employee.AbbrName;
                name.ToolTip = employee.FullName;
                name.NavigateUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (), "EmployeeDetails");

                phone.Text = employee.Phone;

                if (!string.IsNullOrWhiteSpace (employee.Email)) {
                    email.Text = employee.Email;
                    email.NavigateUrl = "mailto:" + employee.Email;
                }
                else
                    email.Visible = false;

                workingPlace.Text = employee.WorkingPlace;

                // try to get prime position:
                var primePosition = UniversityRepository.Instance.DataProvider.GetObjects <OccupiedPositionInfoEx> (
                                        "WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC",
                                        employee.EmployeeID).FirstOrDefault ();

                if (primePosition != null) {
                    position.Text = TextUtils.FormatList (": ",
                        FormatHelper.FormatShortTitle (primePosition.PositionShortTitle, primePosition.PositionTitle, 
                            primePosition.TitleSuffix), primePosition.FormatDivisionLink (this));
                }

                // mark not published employees, as they visible only to editors
                if (!employee.IsPublished ()) {
                    e.Row.CssClass = "not-published";
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
