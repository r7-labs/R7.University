using System;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.R7;
using R7.University;

namespace R7.University.EduProgramDirectory
{
    public partial class SettingsEduProgramDirectory : ExtendedModuleSettingsBase<EduProgramDirectoryController, EduProgramDirectorySettings>
    {
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try
            {
                if (!IsPostBack)
                {
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings ()
        {
            try
            {
                Utils.SynchronizeModule (this);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

