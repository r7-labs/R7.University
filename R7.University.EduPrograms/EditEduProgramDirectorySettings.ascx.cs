using System;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.Dnn.Extensions.Controls;
using R7.University.ControlExtensions;
using R7.University.EduPrograms.Models;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.ViewModels;

namespace R7.University.EduPrograms
{
    public partial class EditEduProgramDirectorySettings : UniversityModuleSettingsBase<EduProgramDirectorySettings>
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

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // fill edulevels list
            foreach (var eduLevel in new EduLevelQuery (ModelContext).ListForEduProgram ()) {
                listEduLevels.AddItem (UniversityFormatHelper.FormatShortTitle (eduLevel.ShortTitle, eduLevel.Title), eduLevel.EduLevelID.ToString ());
            }

            // bind divisions
            divisionSelector.DataSource = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            divisionSelector.DataBind ();
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    // check edulevels list items
                    foreach (var eduLevelId in Settings.EduLevels) {
                        var item = listEduLevels.Items.FindByValue (eduLevelId.ToString ());
                        if (item != null) {
                            item.Selected = true;
                        }
                    }

                    divisionSelector.DivisionId = Settings.DivisionId;
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings ()
        {
            try {
                Settings.EduLevels = listEduLevels.Items.AsEnumerable ().Where (i => i.Selected).Select (i => int.Parse (i.Value)).ToList ();
                Settings.DivisionId = divisionSelector.DivisionId;

                SettingsRepository.SaveSettings (ModuleConfiguration, Settings);

                ModuleController.SynchronizeModule (ModuleId);

                DataCache.ClearCache ($"//r7_University/Modules/EduProgramDirectory?ModuleId={ModuleId}");
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

