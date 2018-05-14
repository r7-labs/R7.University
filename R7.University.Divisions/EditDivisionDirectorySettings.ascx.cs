//
//  EditDivisionDirectorySettings.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2018 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.Dnn.Extensions.Cache;
using R7.Dnn.Extensions.Controls;
using R7.University.Divisions.Models;
using R7.University.Modules;

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

                CacheHelper.RemoveCacheByPrefix ($"//r7_University/Modules/DivisionDirectory?ModuleId={ModuleId}");
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

