using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
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
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.EduProgramProfiles.Models;
using R7.University.EduProgramProfiles.Queries;
using R7.University.EduProgramProfiles.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Security;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfiles
{
    public partial class ViewEduProgramProfileDirectory: PortalModuleBase<EduProgramProfileDirectorySettings>, IActionable
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

        ViewModelContext<EduProgramProfileDirectorySettings> viewModelContext;
        protected ViewModelContext<EduProgramProfileDirectorySettings> ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext<EduProgramProfileDirectorySettings> (this, Settings)); }
        }

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo)); }
        }

        #endregion

        #region Get data

        internal EduProgramProfileDirectoryEduFormsViewModel GetEduFormsViewModel ()
        {
            return DataCache.GetCachedData<EduProgramProfileDirectoryEduFormsViewModel> (
                new CacheItemArgs ("//r7_University/Modules/EduProgramProfileDirectory?ModuleId=" + ModuleId
                                   + "&Culture=" + CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
                    UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetEduFormsViewModel_Internal ()
            ).SetContext (ViewModelContext);
        }

        internal EduProgramProfileDirectoryDocumentsViewModel GetDocumentsViewModel ()
        {
            return DataCache.GetCachedData<EduProgramProfileDirectoryDocumentsViewModel> (
                new CacheItemArgs ("//r7_University/Modules/EduProgramProfileDirectory?ModuleId=" + ModuleId
                                   + "&Culture=" + CultureInfo.CurrentCulture.TwoLetterISOLanguageName,
                    UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetDocumentsViewModel_Internal ()
            ).SetContext (ViewModelContext);
        }

        internal EduProgramProfileDirectoryEduFormsViewModel GetEduFormsViewModel_Internal ()
        {
            var viewModel = new EduProgramProfileDirectoryEduFormsViewModel ();
            var indexer = new ViewModelIndexer (1);

            var eduProfiles = new EduProfileQuery (ModelContext)
                .ListWithEduForms (Settings.EduLevelIds, Settings.DivisionId, Settings.DivisionLevel);

            viewModel.EduProfiles = new IndexedEnumerable<EduProgramProfileEduFormsViewModel> (indexer,
                eduProfiles.Select (epp => new EduProgramProfileEduFormsViewModel (epp, viewModel, indexer))
            );

            return viewModel;
        }

        internal EduProgramProfileDirectoryDocumentsViewModel GetDocumentsViewModel_Internal ()
        {
            var viewModel = new EduProgramProfileDirectoryDocumentsViewModel ();
            var indexer = new ViewModelIndexer (1);

            var eduProfiles = new EduProfileQuery (ModelContext)
                .ListWithDocuments (Settings.EduLevelIds, Settings.DivisionId, Settings.DivisionLevel);

            viewModel.EduProfiles = new IndexedEnumerable<EduProfileDocumentsViewModel> (indexer,
                eduProfiles.Select (epp => new EduProfileDocumentsViewModel (epp, viewModel, indexer))
            );

            return viewModel;
        }

        #endregion

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
                var actions = new ModuleActionCollection ();
                actions.Add (
                    GetNextActionID (),
                    LocalizeString ("AddEduProgramProfile.Action"),
                    ModuleActionType.AddContent,
                    "",
                    UniversityIcons.Add,
                    EditUrl ("EditEduProgramProfile"),
                    false,
                    SecurityAccessLevel.Edit,
                    SecurityContext.CanAdd (typeof (EduProfileInfo)),
                    false
                );

                return actions;
            }
        }

        #endregion

        #region Handlers

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            gridEduProgramProfileObrnadzorEduForms.Attributes.Add ("itemprop", "eduAccred");
            gridEduProgramProfileObrnadzorDocuments.Attributes.Add ("itemprop", "eduOP");

            switch (Settings.Mode) {
                case EduProgramProfileDirectoryMode.ObrnadzorEduForms:
                    mviewEduProgramProfileDirectory.ActiveViewIndex = 1;
                    gridEduProgramProfileObrnadzorEduForms.LocalizeColumnHeaders (LocalResourceFile);
                    break;

                case EduProgramProfileDirectoryMode.ObrnadzorDocuments:
                    mviewEduProgramProfileDirectory.ActiveViewIndex = 2;
                    gridEduProgramProfileObrnadzorDocuments.LocalizeColumnHeaders (LocalResourceFile);
                    break;

                default:
                    mviewEduProgramProfileDirectory.ActiveViewIndex = 0;
                    break;
            }
        }

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);

            try {
                switch (Settings.Mode) {
                    case EduProgramProfileDirectoryMode.ObrnadzorEduForms:
                        ObrnadzorEduFormsView ();
                        break;

                    case EduProgramProfileDirectoryMode.ObrnadzorDocuments:
                        ObrnadzorDocumentsView ();
                        break;
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void ObrnadzorEduFormsView ()
        {
            var now = HttpContext.Current.Timestamp;
            var eduProgramProfiles = GetEduFormsViewModel ().EduProfiles
                .Where (epp => epp.IsPublished (now) || IsEditable);

            if (!eduProgramProfiles.IsNullOrEmpty ()) {
                gridEduProgramProfileObrnadzorEduForms.DataSource = eduProgramProfiles;
                gridEduProgramProfileObrnadzorEduForms.DataBind ();
            }
            else {
                this.Message ("NothingToDisplay.Text", MessageType.Info, true);
            }
        }

        protected void ObrnadzorDocumentsView ()
        {
            var now = HttpContext.Current.Timestamp;
            var eduProgramProfiles = GetDocumentsViewModel ().EduProfiles
                .Where (epp => epp.IsPublished (now) || IsEditable);

            if (!eduProgramProfiles.IsNullOrEmpty ()) {
                gridEduProgramProfileObrnadzorDocuments.DataSource = eduProgramProfiles;
                gridEduProgramProfileObrnadzorDocuments.DataBind ();
            }
            else {
                this.Message ("NothingToDisplay.Text", MessageType.Info, true);
            }
        }

        protected void gridEduProgramProfileObrnadzorEduForms_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            var now = HttpContext.Current.Timestamp;

            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            // hiding the columns of second row header (created on binding)
            if (e.Row.RowType == DataControlRowType.Header) {
                // set right table section for header row
                e.Row.TableSection = TableRowSection.TableHeader;
            }

            if (e.Row.RowType == DataControlRowType.DataRow) {
                var eduProgramProfile = (EduProgramProfileEduFormsViewModel) e.Row.DataItem;

                if (IsEditable) {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = EditUrl ("eduprofile_id",
                        eduProgramProfile.EduProgramProfileID.ToString (), "EditEduProgramProfile");
                    iconEdit.ImageUrl = UniversityIcons.Edit;
                }

                if (!eduProgramProfile.IsPublished (now)) {
                    e.Row.AddCssClass ("u8y-not-published");
                }
            }
        }

        protected void gridEduProgramProfileObrnadzorDocuments_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            var now = HttpContext.Current.Timestamp;

            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.Header) {
                // set right table section for header row
                e.Row.TableSection = TableRowSection.TableHeader;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow) {
                var eduProgramProfile = (EduProfileDocumentsViewModel) e.Row.DataItem;

                e.Row.Attributes.Add ("data-title", UniversityFormatHelper.FormatEduProgramProfileTitle (
                    eduProgramProfile.EduProgram.Code, eduProgramProfile.EduProgram.Title,
                    eduProgramProfile.ProfileCode, eduProgramProfile.ProfileTitle)
                                      .Append (eduProgramProfile.IsAdopted ? LocalizeString ("IsAdopted.Text") : null, " - ")
                                      .Append (eduProgramProfile.EduLevel.Title, ": ")
                );

                if (IsEditable) {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = EditUrl ("eduprofile_id",
                        eduProgramProfile.EduProgramProfileID.ToString (), "EditEduProgramProfileDocuments");
                    iconEdit.ImageUrl = UniversityIcons.Edit;
                }

                if (!eduProgramProfile.IsPublished (now)) {
                    e.Row.AddCssClass ("u8y-not-published");
                }
            }
        }

        #endregion
    }
}

