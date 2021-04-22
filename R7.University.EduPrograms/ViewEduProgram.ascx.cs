using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using R7.Dnn.Extensions.Modules;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Configuration;
using R7.University.Dnn;
using R7.University.EduPrograms.Models;
using R7.University.EduPrograms.Queries;
using R7.University.EduPrograms.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Security;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.EduPrograms
{
    public partial class ViewEduProgram : PortalModuleBase<EduProgramSettings>, IActionable
    {
        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo)); }
        }

        #region Get data

        internal EduProgramModuleViewModel GetViewModel ()
        {
            return DataCache.GetCachedData<EduProgramModuleViewModel> (
                new CacheItemArgs ("//r7_University/Modules/EduProgram?ModuleId=" + ModuleId,
                    UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetViewModel_Internal ()).SetContext (new ViewModelContext<EduProgramSettings> (this, Settings));
        }

        internal EduProgramModuleViewModel GetViewModel_Internal ()
        {
            // TODO: Restore sorting of edu. program profiles
            if (Settings.EduProgramId != null) {

                EduProgramInfo eduProgram;
                using (var modelContext = new UniversityModelContext ()) {
                    eduProgram = new EduProgramQuery (modelContext).SingleOrDefault (Settings.EduProgramId.Value);
                }

                if (eduProgram == null) {
                    // edu. program not found - return empty view model
                    return new EduProgramModuleViewModel ();
                }

                var viewModel = new EduProgramModuleViewModel ();
                viewModel.EduProgram = new EduProgramViewModel (eduProgram, viewModel);

                return viewModel;
            }

            // edu. program not set - return empty view model
            return new EduProgramModuleViewModel ();
        }

        #endregion

        #region Handlers

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                var shouldBind = true;
                var now = HttpContext.Current.Timestamp;

                var viewModel = GetViewModel ();
                if (viewModel.IsEmpty ()) {
                    if (IsEditable) {
                        this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                    }
                    shouldBind = false;
                }
                else if (!viewModel.EduProgram.IsPublished (now)) {
                    if (IsEditable) {
                        this.Message ("NotPublished.Text", MessageType.Warning, true);
                    }
                    else {
                        shouldBind = false;
                    }
                }

                if (shouldBind)
                {
                    // update module title
                    if (Settings.AutoTitle) {
                        UniversityModuleHelper.UpdateModuleTitle (TabModuleId,
                            UniversityFormatHelper.FormatEduProgramTitle (viewModel.EduProgram.Code, viewModel.EduProgram.Title)
                        );
                    }

                    Bind (viewModel);
                }
                else {
                    ContainerControl.Visible = IsEditable;
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        void Bind (EduProgramModuleViewModel viewModel)
        {
            formEduProgram.DataSource = new List<EduProgramViewModel> { viewModel.EduProgram };
            formEduProgram.DataBind ();
        }

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get
            {
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
                    Settings.EduProgramId == null && SecurityContext.CanAdd (typeof (EduProgramInfo)),
                    false
                );

                actions.Add (
                    GetNextActionID (),
                    LocalizeString ("EditEduProgram.Action"),
                    ModuleActionType.EditContent,
                    "",
                    UniversityIcons.Edit,
                    EditUrl ("eduprogram_id", Settings.EduProgramId.ToString (), "EditEduProgram"),
                    false,
                    SecurityAccessLevel.Edit,
                    Settings.EduProgramId != null,
                    false
                );

                return actions;
            }
        }

        #endregion
    }
}
