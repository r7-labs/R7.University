using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.UI.WebControls.Extensions;
using R7.Dnn.Extensions.Collections;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Modules;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Employees.Models;
using R7.University.Employees.Queries;
using R7.University.Employees.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Queries;
using R7.University.Security;
using R7.University.ViewModels;

namespace R7.University.Employees
{
    // TODO: Make module instances co-exist on same page

    public partial class ViewEmployeeDirectory: PortalModuleBase<EmployeeDirectorySettings>, IActionable
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

        ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this)); }
        }

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo)); }
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

         #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
                var actions = new ModuleActionCollection ();
                actions.Add (
                    GetNextActionID (),
                    LocalizeString ("AddEmployee.Action"),
                    ModuleActionType.AddContent,
                    "",
                    UniversityIcons.Add,
                    EditUrl ("EditEmployee"),
                    false,
                    SecurityAccessLevel.Edit,
                    SecurityContext.CanAdd (typeof (EmployeeInfo)),
                    false
                );

                return actions;
            }
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

                var now = HttpContext.Current.Timestamp;
                treeDivisions.DataSource = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title)
                    .Where (d => d.IsPublished (now) || IsEditable);

                treeDivisions.DataBind ();

                // select first node
                if (treeDivisions.Nodes.Count > 0) {
                    treeDivisions.Nodes [0].Selected = true;
                }

                // TODO: Level should be set in settings?
                R7.University.Utilities.Utils.ExpandToLevel (treeDivisions, 2);
            }
        }

        internal EmployeeDirectoryTeachersViewModel GetViewModel ()
        {
            return DataCache.GetCachedData<EmployeeDirectoryTeachersViewModel> (
                new CacheItemArgs ("//r7_University/Modules/EmployeeDirectory?ModuleId=" + ModuleId,
                    UniversityConfig.Instance.DataCacheTime, System.Web.Caching.CacheItemPriority.Normal),
                c => GetViewModel_Internal ()
            ).SetContext (ViewModelContext);
        }

        internal EmployeeDirectoryTeachersViewModel GetViewModel_Internal ()
        {
            var viewModel = new EmployeeDirectoryTeachersViewModel ();

            var eduProgramProfiles = new EduProgramProfileQuery (ModelContext).ListByEduLevels (Settings.EduLevels)
                .Select (epp => new EduProgramProfileViewModel (epp, viewModel))
                .ToList ();

            if (Settings.ShowAllTeachers) {
                eduProgramProfiles.Add (new EduProgramProfileViewModel (
                    new EduProfileInfo {
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

                var teachers = new TeachersQuery (ModelContext).List ();

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
                var now = HttpContext.Current.Timestamp;
                if (Settings.Mode == EmployeeDirectoryMode.Search) {
                    if (!IsPostBack) {
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
                }
                else if (Settings.Mode == EmployeeDirectoryMode.Teachers) {
                    repeaterEduProgramProfiles.DataSource = GetViewModel ().EduProgramProfiles
                        .Where (epp => epp.IsPublished (now) || IsEditable);
                    repeaterEduProgramProfiles.DataBind ();
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        protected void repeaterEduProgramProfiles_ItemDataBound (object sender, RepeaterItemEventArgs e)
        {
            var now = HttpContext.Current.Timestamp;

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

                var publishedTeachers = eduProgramProfile.Teachers.Where (t => IsEditable || t.IsPublished (now));
                if (publishedTeachers.Any ()) {
                    // mark edu. program profile as not published
                    if (!eduProgramProfile.IsPublished (now)) {
                        panelTeachers.CssClass = "not-published";
                    }

                    gridTeachers.LocalizeColumnHeaders (LocalResourceFile);
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
            var now = HttpContext.Current.Timestamp;

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
                    iconEdit.ImageUrl = UniversityIcons.Edit;
                }

                if (!teacher.IsPublished (now)) {
                    // mark teacher as not published
                    e.Row.AddCssClass ("u8y-not-published");
                }

                // apply obrnadzor.gov.ru microdata
                e.Row.Attributes.Add ("itemprop", "teachingStaff");
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
            var now = HttpContext.Current.Timestamp;

            var employees = new EmployeeFindQuery (ModelContext).FindEmployees (searchText, teachersOnly, searchDivision)
                .Where (e => IsEditable || e.IsPublished (now));

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
            var now = HttpContext.Current.Timestamp;

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
                    iconEdit.ImageUrl = UniversityIcons.Edit;
                }

                var name = (HyperLink) e.Row.FindControl ("linkName");
                var phone = (Literal) e.Row.FindControl ("literalPhone");
                var cellPhone = (Literal) e.Row.FindControl ("litCellPhone");
                var email = (HyperLink) e.Row.FindControl ("linkEmail");
                var secondaryEmail = (HyperLink) e.Row.FindControl ("lnkSecondaryEmail");
                var workingPlace = (Literal) e.Row.FindControl ("literalWorkingPlace");

                name.Text = employee.AbbrName ();
                name.ToolTip = employee.FullName ();
                name.NavigateUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (), "EmployeeDetails");

                phone.Text = employee.Phone;
                cellPhone.Text = employee.CellPhone;

                if (!string.IsNullOrWhiteSpace (employee.Email)) {
                    email.Text = employee.Email;
                    email.NavigateUrl = "mailto:" + employee.Email;
                }
                else
                    email.Visible = false;

                if (!string.IsNullOrWhiteSpace (employee.SecondaryEmail)) {
                    secondaryEmail.Text = employee.SecondaryEmail;
                    secondaryEmail.NavigateUrl = "mailto:" + employee.SecondaryEmail;
                }
                else
                    secondaryEmail.Visible = false;

                workingPlace.Text = employee.WorkingPlace;

                // mark not published employees, as they visible only to editors
                if (!employee.IsPublished (now)) {
                    e.Row.AddCssClass ("u8y-not-published");
                }

            }
        }
    }
}
