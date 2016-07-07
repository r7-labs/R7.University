//
//  ViewEmployeeDirectory.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.ControlExtensions;
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
using R7.University.Queries;

namespace R7.University.EmployeeDirectory
{
    // TODO: Make module instances co-exist on same page

    public partial class ViewEmployeeDirectory: PortalModuleBase<EmployeeDirectorySettings>
    {   
        #region Model context

        private UniversityModelContext modelContext;
        protected UniversityModelContext ModelContext
        {
            get { return modelContext ?? (modelContext = new UniversityModelContext ()); }
        }

        public override void Dispose ()
        {
            if (modelContext != null) {
                modelContext.Dispose ();
            }

            base.Dispose ();
        }

        #endregion

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

        #endregion

        #region Session state properties

        protected string SearchText
        {
            get { 
                var objSearchText = Session ["EmployeeDirectory.SearchText." + TabModuleId];
                return (string) objSearchText ?? string.Empty;
            }
            set { Session ["EmployeeDirectory.SearchText." + TabModuleId] = value; }
        }

        protected int SearchDivision
        {
            get { 
                var objSearchDivision = Session ["EmployeeDirectory.SearchDivision." + TabModuleId];
                return objSearchDivision != null ? (int) objSearchDivision : Null.NullInteger;

            }
            set { Session ["EmployeeDirectory.SearchDivision." + TabModuleId] = value; }
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

                treeDivisions.DataSource = UniversityRepository.Instance.DataProvider.GetObjects <DivisionInfo> ()
                    .Where (d => d.IsPublished || IsEditable)
                    .OrderBy (d => d.Title);
                treeDivisions.DataBind ();

                // select first node
                if (treeDivisions.Nodes.Count > 0) {
                    treeDivisions.Nodes [0].Selected = true;
                }

                // REVIEW: Level should be set in settings?
                R7.University.Utilities.Utils.ExpandToLevel (treeDivisions, 2);
            }
        }

        protected EmployeeDirectoryTeachersViewModel GetViewModel ()
        {
            return DataCache.GetCachedData<EmployeeDirectoryTeachersViewModel> (
                new CacheItemArgs ("//r7_University/Modules/EmployeeDirectory?ModuleId=" + ModuleId,
                    UniversityConfig.Instance.DataCacheTime, System.Web.Caching.CacheItemPriority.Normal),
                c => GetViewModel_Internal ()
            ).SetContext (ViewModelContext);
        }

        protected EmployeeDirectoryTeachersViewModel GetViewModel_Internal ()
        {
            var viewModel = new EmployeeDirectoryTeachersViewModel ();

            var eduProgramProfiles = new EduProgramProfileQuery (ModelContext).Execute (Settings.EduLevels)
                .Select (epp => new EduProgramProfileViewModel (epp, viewModel))
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
                    }, viewModel)
                );
            }

            if (eduProgramProfiles.Count > 0) {
                
                var teachers = EmployeeRepository.Instance.GetTeachers ()
                    .WithDisciplines (UniversityRepository.Instance.DataProvider
                        .GetObjects<EmployeeDisciplineInfo> ())
                    // .WithOccupiedPositions (UniversityRepository.Instance.DataProvider
                    //     .GetObjects<OccupiedPositionInfoEx> ())
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
                            .Select (t => new TeacherViewModel (t, eduProgramProfile, viewModel, indexer))
                    );
                }
            }

            viewModel.EduProgramProfiles = eduProgramProfiles;
            return viewModel;
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
                        if (!string.IsNullOrWhiteSpace (SearchText) || !Null.IsNull (SearchDivision)) {

                            // restore current search
                            textSearch.Text = SearchText;
                            checkTeachersOnly.Checked = SearchTeachersOnly;

                            if (Null.IsNull (SearchDivision)) {
                                // select first node
                                if (treeDivisions.Nodes.Count > 0) {
                                    treeDivisions.Nodes [0].Selected = true;
                                }
                            }
                            else {
                                treeDivisions.SelectAndExpandByValue (SearchDivision.ToString ());
                            }

                            // perform search
                            if (SearchParamsOK (SearchText, SearchDivision, false)) {
                                DoSearch (SearchText, SearchDivision, SearchTeachersOnly);
                            }
                        }
                    }
                    else if (Settings.Mode == EmployeeDirectoryMode.Teachers) {
                        repeaterEduProgramProfiles.DataSource = GetViewModel ().EduProgramProfiles
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

        protected bool SearchParamsOK (string searchText, int searchDivision, bool showMessages = true)
        {
            var divisionNotSpecified = Null.IsNull (searchDivision);
            var searchTextIsEmpty = string.IsNullOrWhiteSpace (searchText);

            // no search params - shouldn't perform search
            if (searchTextIsEmpty && divisionNotSpecified) {
                if (showMessages) {
                    this.Message ("SearchParams.Warning", MessageType.Warning, true);
                }

                gridEmployees.Visible = false;
                return false;
            }

            return true;
        }

        protected void DoSearch (string searchText, int searchDivision, bool teachersOnly)
        {
            var employees = EmployeeRepository.Instance.FindEmployees (searchText, 
                                IsEditable, teachersOnly, searchDivision);
            
            if (employees.IsNullOrEmpty ()) {
                this.Message ("NoEmployeesFound.Warning", MessageType.Warning, true);
            }

            gridEmployees.DataSource = employees;
            gridEmployees.DataBind ();

            // make employees grid visible anyway
            gridEmployees.Visible = true;
        }

        protected void linkSearch_Click (object sender, EventArgs e)
        {
            var searchText = textSearch.Text.Trim ();
            var searchDivision = (treeDivisions.SelectedNode != null) ? 
                int.Parse (treeDivisions.SelectedNode.Value) : Null.NullInteger;
            var teachersOnly = checkTeachersOnly.Checked;

            if (SearchParamsOK (searchText, searchDivision)) {
                // save current search
                SearchText = searchText;
                SearchDivision = searchDivision;
                SearchTeachersOnly = teachersOnly;

                // perform search
                DoSearch (SearchText, SearchDivision, SearchTeachersOnly);
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
                var primePosition = new OccupiedPositionsByEmployeeQuery (ModelContext)
                    .Execute (employee.EmployeeID)
                    .FirstOrDefault ();

                if (primePosition != null) {
                    position.Text = TextUtils.FormatList (": ",
                        FormatHelper.FormatShortTitle (primePosition.Position.ShortTitle, primePosition.Position.Title, 
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
