using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Commands;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.EduPrograms.Models;
using R7.University.EduPrograms.Queries;
using R7.University.EduPrograms.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;

namespace R7.University.EduPrograms
{
    // available tabs
    public enum EditEduProgramTab
    {
        Common,
        EduProgramProfiles,
        Divisions,
        Bindings,
        Documents,
        Audit
    }

    public partial class EditEduProgram : UniversityEditPortalModuleBase<EduProgramInfo>, IActionable
    {
        protected EditEduProgramTab SelectedTab
        {
            get {
                // get postback initiator control
                var eventTarget = Request.Form ["__EVENTTARGET"];

                if (!string.IsNullOrEmpty (eventTarget)) {

                    if (eventTarget.Contains ("$" + formEditDocuments.ID)) {
                        ViewState ["SelectedTab"] = EditEduProgramTab.Documents;
                        return EditEduProgramTab.Documents;
                    }

                    if (eventTarget.Contains ("$" + formEditDivisions.ID)) {
                        ViewState ["SelectedTab"] = EditEduProgramTab.Divisions;
                        return EditEduProgramTab.Divisions;
                    }

                    if (eventTarget.Contains ("$" + urlHomePage.ID)) {
                        ViewState ["SelectedTab"] = EditEduProgramTab.Bindings;
                        return EditEduProgramTab.Bindings;
                    }
                }

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                if (obj != null) {
                    return (EditEduProgramTab) obj;
                }

                return EditEduProgramTab.Common;
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this)); }
        }

        protected EditEduProgram () : base ("eduprogram_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel, auditControl);
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // bind education levels
            comboEduLevel.DataSource = new EduLevelQuery (ModelContext).ListForEduProgram ();
            comboEduLevel.DataBind ();

            var documentTypes = new FlatQuery<DocumentTypeInfo> (ModelContext).List ();
            formEditDocuments.OnInit (this, documentTypes);

            var divisions = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            formEditDivisions.OnInit (this, divisions);

            gridEduProfiles.LocalizeColumnHeaders (LocalResourceFile);
        }

        protected override string GetContextString (EduProgramInfo item)
        {
            return item?.FormatTitle ();
        }

        protected override void LoadItem (EduProgramInfo item)
        {
            var ep = GetItemWithDependencies (ItemKey.Value);
            base.LoadItem (ep);

            textCode.Text = ep.Code;
            textTitle.Text = ep.Title;
            textGeneration.Text = ep.Generation;
            datetimeStartDate.SelectedDate = ep.StartDate;
            datetimeEndDate.SelectedDate = ep.EndDate;
            comboEduLevel.SelectByValue (ep.EduLevelID);
            urlHomePage.Url = ep.HomePage;
            formEditDivisions.SetData (ep.Divisions, ep.EduProgramID);

            auditControl.Bind (ep, PortalId, LocalizeString ("Unknown")); ;

            formEditDocuments.SetData (ep.Documents, ep.EduProgramID);

            // setup link for adding new edu. program profile
            linkAddEduProgramProfile.NavigateUrl = EditUrl ("eduprogram_id", ep.EduProgramID.ToString (), "EditEduProgramProfile");

            gridEduProfiles.DataSource = ep.EduProfiles
                .Select (epp => new EduProfileEditModel (epp, ViewModelContext))
                .OrderBy (epp => epp.ProfileCode)
                .ThenBy (epp => epp.ProfileTitle);

            gridEduProfiles.DataBind ();

            buttonDelete.Visible = SecurityContext.CanDelete (ep);
            linkAddEduProgramProfile.Visible = SecurityContext.CanAdd (typeof (EduProfileInfo));
            panelAddDefaultProfile.Visible = false;
        }

        protected override void LoadNewItem ()
        {
            base.LoadNewItem ();

            linkAddEduProgramProfile.Visible = false;
            panelAddDefaultProfile.Visible = SecurityContext.CanAdd (typeof (EduProfileInfo));
        }

        protected override void BeforeUpdateItem (EduProgramInfo item, bool isNew)
        {
            // fill the object
            item.Code = textCode.Text.Trim ();
            item.Title = textTitle.Text.Trim ();
            item.Generation = textGeneration.Text.Trim ();
            item.StartDate = datetimeStartDate.SelectedDate;
            item.EndDate = datetimeEndDate.SelectedDate;
            item.HomePage = chkUseCurrentPageAsHomePage.Checked ? TabId.ToString () : urlHomePage.Url;

            // update references
            item.EduLevelID = int.Parse (comboEduLevel.SelectedValue);
            item.EduLevel = ModelContext.Get<EduLevelInfo,int> (item.EduLevelID);

            if (!isNew) {
                item.LastModifiedOnDate = DateTime.Now;
                item.LastModifiedByUserId = UserInfo.UserID;
            }
        }

        protected EduProgramInfo GetItemWithDependencies (int itemId)
        {
            return new EduProgramQuery (ModelContext).SingleOrDefault (itemId);
        }

        #region Implemented abstract members of UniversityEditPortalModuleBase

        protected override int GetItemId (EduProgramInfo item) => item.EduProgramID;

        protected override void AddItem (EduProgramInfo item)
        {
            if (SecurityContext.CanAdd (typeof (EduProgramInfo))) {

                var now = DateTime.Now;
                new AddCommand<EduProgramInfo> (ModelContext, SecurityContext).Add (item, now);
                ModelContext.SaveChanges (false);

                if (checkAddDefaultProfile.Checked) {
                    var defaultProfile = new EduProfileInfo {
                        ProfileCode = string.Empty,
                        ProfileTitle = string.Empty,
                        EduProgramID = item.EduProgramID,
                        EduLevelId = item.EduLevelID,
                        Languages = UniversityConfig.Instance.EduProgramProfiles.DefaultLanguages,
                        // unpublish profile
                        EndDate = item.CreatedOnDate.Date
                    };

                    new AddCommand<EduProfileInfo> (ModelContext, SecurityContext).Add (defaultProfile, now);
                    ModelContext.SaveChanges (false);
                }

                // update EduProgram module settings then adding new item
                if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7_University_EduProgram") {
                    var settingsRepository = new EduProgramSettingsRepository ();
                    var settings = settingsRepository.GetSettings (ModuleConfiguration);
                    settings.EduProgramId = item.EduProgramID;
                    settingsRepository.SaveSettings (ModuleConfiguration, settings);
                }

                new UpdateDocumentsCommand (ModelContext)
                    .Update (formEditDocuments.GetModifiedData(), ModelType.EduProgram, item.EduProgramID, UserId);

                new UpdateEduProgramDivisionsCommand (ModelContext)
                    .Update (formEditDivisions.GetModifiedData (), ModelType.EduProgram, item.EduProgramID);

                ModelContext.SaveChanges ();
            }
        }

        protected override void UpdateItem (EduProgramInfo item)
        {
            // TODO: Use single transaction to update main entity along with all dependent ones?

            ModelContext.Update (item);

            new UpdateDocumentsCommand (ModelContext)
                .Update (formEditDocuments.GetModifiedData(), ModelType.EduProgram, item.EduProgramID, UserId);

            new UpdateEduProgramDivisionsCommand (ModelContext)
                .Update (formEditDivisions.GetModifiedData (), ModelType.EduProgram, item.EduProgramID);

            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (EduProgramInfo item)
        {
            // TODO: Also remove documents & divisions
            new DeleteCommand<EduProgramInfo> (ModelContext, SecurityContext).Delete (item);
            ModelContext.SaveChanges ();
        }

        #endregion

        #region Handlers

        protected void gridEduProgramProfiles_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                var eduProfile = (IEduProfile) e.Row.DataItem;
                if (!eduProfile.IsPublished (HttpContext.Current.Timestamp)) {
                    e.Row.CssClass = gridEduProfiles.GetRowStyle (e.Row).CssClass + " u8y-not-published";
                }
            }
        }

        #endregion

        #region IActionable implementation

        public ModuleActionCollection ModuleActions {
            get {
                var itemId = ParseHelper.ParseToNullable<int> (Request.QueryString [Key]);

                var actions = new ModuleActionCollection ();
                actions.Add (new ModuleAction (GetNextActionID ()) {
                    Title = LocalizeString ("EditScience.Action"),
                    CommandName = ModuleActionType.EditContent,
                    Icon = UniversityIcons.Edit,
                    Secure = SecurityAccessLevel.Edit,
                    Url = EditUrl ("eduprogram_id", itemId.ToString (), "EditScience"),
                    Visible = itemId != null
                });

                return actions;
            }
        }

        #endregion
    }
}

