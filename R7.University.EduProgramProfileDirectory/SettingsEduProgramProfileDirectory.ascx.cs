//
//  SettingsEduProgramProfileDirectory.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Web.UI.WebControls;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Data;
using R7.University.EduProgramProfileDirectory.Components;
using R7.University.ViewModels;
using R7.University.Models;

namespace R7.University.EduProgramProfileDirectory
{
    public partial class SettingsEduProgramProfileDirectory: ModuleSettingsBase<EduProgramProfileDirectorySettings>
    {
        #region Repository handling

        private UniversityDataRepository repository;
        protected UniversityDataRepository Repository
        {
            get { return repository ?? (repository = new UniversityDataRepository ()); }
        }

        public override void Dispose ()
        {
            if (repository != null) {
                repository.Dispose ();
            }

            base.Dispose ();
        }

        #endregion

        private ViewModelContext viewModelContext;

        protected ViewModelContext ViewModelContext
        {
            get { 
                if (viewModelContext == null)
                    viewModelContext = new ViewModelContext (this);

                return viewModelContext;
            }
        }

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // fill display modes dropdown
            comboMode.DataSource = EnumViewModel<EduProgramProfileDirectoryMode>.GetValues (ViewModelContext, true);
            comboMode.DataBind ();

            // fill edulevels list
            var eduLevels = Repository.QueryEduLevels ().ToList();

            foreach (var eduLevel in eduLevels) {
                listEduLevels.Items.Add (new DnnListBoxItem
                    { 
                        Text = FormatHelper.FormatShortTitle (eduLevel.ShortTitle, eduLevel.Title),
                        Value = eduLevel.EduLevelID.ToString ()
                    });
            }
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    comboMode.SelectByValue (Settings.Mode);

                    // check edulevels list items
                    foreach (var eduLevelId in Settings.EduLevels) {
                        var item = listEduLevels.FindItemByValue (eduLevelId.ToString ());
                        if (item != null) {
                            item.Checked = true;
                        }
                    }
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
                EduProgramProfileDirectoryMode mode;
                Settings.Mode = Enum.TryParse<EduProgramProfileDirectoryMode> (comboMode.SelectedValue, out mode) ? 
                    (EduProgramProfileDirectoryMode?) mode : null;

                Settings.EduLevels = listEduLevels.CheckedItems.Select (i => int.Parse (i.Value)).ToList ();

                ModuleController.SynchronizeModule (ModuleId);
                CacheHelper.RemoveCacheByPrefix ("//r7_University/Modules/EduProgramProfileDirectory?ModuleId=" + ModuleId);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

