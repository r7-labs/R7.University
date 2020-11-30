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
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Modules;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.EduPrograms.Models;
using R7.University.EduPrograms.Queries;
using R7.University.EduPrograms.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Security;
using R7.University.ViewModels;

namespace R7.University.EduPrograms
{
    public partial class ViewEduProgramDirectory: PortalModuleBase<EduProgramDirectorySettings>, IActionable
    {
        #region Controls

        protected GridView gridEduStandards;

        #endregion

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

        #region Handlers

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            gridEduStandards.LocalizeColumnHeaders (LocalResourceFile);
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

                var viewModelIndexer = new ViewModelIndexer (1);
                var eduPrograms = GetEduPrograms ()
                    .OrderBy (ep => ep.EduLevel.SortIndex)
                    .ThenBy (ep => ep.Code)
                    .ThenBy (ep => ep.Title)
                    .ToList ();

                var eduProgramViewModels = eduPrograms
                    .Select (ep => new EduProgramStandardObrnadzorViewModel (
                        ep,
                        ViewModelContext,
                        viewModelIndexer));

                if (eduProgramViewModels.Any ()) {
                    gridEduStandards.DataSource = eduProgramViewModels.Where (ep => ep.IsPublished (now) || IsEditable);
                    gridEduStandards.DataBind ();
                }
                else {
                    this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        IEnumerable<EduProgramInfo> GetEduPrograms ()
        {
            var cacheKey = $"//r7_University/Modules/EduProgramDirectory?ModuleId={ModuleId}";
            return DataCache.GetCachedData<IEnumerable<EduProgramInfo>> (
                new CacheItemArgs (cacheKey, UniversityConfig.Instance.DataCacheTime),
                (c) => GetEduPrograms_Internal ()
            );
        }

        IEnumerable<EduProgramInfo> GetEduPrograms_Internal ()
        {
            var query = new EduProgramDirectoryQuery (ModelContext);
            if (Settings.DivisionId != null) {
                return query.ListByDivisionAndEduLevels (Settings.DivisionId.Value, Settings.EduLevels);
            }
            return query.ListByEduLevels (Settings.EduLevels);
        }

        #endregion

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
                var actions = new ModuleActionCollection ();
                actions.Add (
                    GetNextActionID (),
                    LocalizeString ("AddEduProgram.Action"),
                    ModuleActionType.AddContent,
                    "",
                    UniversityIcons.Add,
                    EditUrl ("EditEduProgram"),
                    false,
                    SecurityAccessLevel.Edit,
                    SecurityContext.CanAdd (typeof (EduProgramInfo)),
                    false
                );

                return actions;
            }
        }

        #endregion

        protected void grid_RowCreated (object sender, GridViewRowEventArgs e)
        {
            // table header row should be inside <thead> tag
            if (e.Row.RowType == DataControlRowType.Header) {
                e.Row.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gridEduStandards_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            var now = HttpContext.Current.Timestamp;

            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.Header) {
                e.Row.Cells [3].CssClass = "u8y-column u8y-expand";
            }

            if (e.Row.RowType == DataControlRowType.DataRow) {
                var eduProgram = (IEduProgram) e.Row.DataItem;

                if (IsEditable) {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = EditUrl (
                        "eduprogram_id",
                        eduProgram.EduProgramID.ToString (),
                        "EditEduProgram");
                    iconEdit.ImageUrl = UniversityIcons.Edit;
                }

                if (!eduProgram.IsPublished (now)) {
                    e.Row.AddCssClass ("u8y-not-published");
                }
            }
        }
    }
}

