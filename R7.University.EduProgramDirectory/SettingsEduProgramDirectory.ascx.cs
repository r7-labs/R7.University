using System;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Web.UI.WebControls;
using DotNetNuke.R7;
using R7.University;

namespace R7.University.EduProgramDirectory
{
    public partial class SettingsEduProgramDirectory : ExtendedModuleSettingsBase<EduProgramDirectoryController, EduProgramDirectorySettings>
    {
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // fill edulevels list
            var eduLevels = Controller.GetObjects<EduLevelInfo> ().OrderBy (el => el.SortIndex);
           
            foreach (var eduLevel in eduLevels)
            {
                listEduLevels.Items.Add (new DnnListBoxItem { 
                    Text = eduLevel.DisplayShortTitle, 
                    Value = eduLevel.EduLevelID.ToString ()
                });
            }
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
                    // check edulevels list items
                    foreach (var eduLevelIdString in Settings.EduLevels)
                    {
                        var item = listEduLevels.FindItemByValue (eduLevelIdString);
                        if (item != null)
                        {
                            item.Checked = true;
                        }
                    }
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
                Settings.EduLevels = listEduLevels.CheckedItems.Select (i => i.Value).ToList ();

                Utils.SynchronizeModule (this);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

