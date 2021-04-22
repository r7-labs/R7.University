using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.Dnn.Extensions.Controls;
using R7.University.Divisions.Models;
using R7.University.Dnn.Modules;

namespace R7.University.Divisions
{
    public partial class EditDivisionDirectorySettings : UniversityModuleSettingsBase<DivisionDirectorySettings>
    {
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            comboMode.DataSource = Enum.GetNames (typeof (DivisionDirectoryMode));
            comboMode.DataBind ();
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    comboMode.SelectByValue (Settings.Mode);
                    checkShowInformal.Checked = Settings.ShowInformal;
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
                Settings.Mode = (DivisionDirectoryMode) Enum.Parse (
                    typeof (DivisionDirectoryMode),
                    comboMode.SelectedValue);
                Settings.ShowInformal = checkShowInformal.Checked;

                SettingsRepository.SaveSettings (ModuleConfiguration, Settings);

                ModuleController.SynchronizeModule (ModuleId);

                DataCache.ClearCache ($"//r7_University/Modules/DivisionDirectory?ModuleId={ModuleId}");
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

