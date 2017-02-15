//
//  SettingsLaunchpad.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2017 Roman M. Yagodin
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
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.ControlExtensions;
using R7.University.Launchpad.Components;
using R7.University.Security;

namespace R7.University.Launchpad
{
    public partial class SettingsLaunchpad : ModuleSettingsBase<LaunchpadSettings>
    {
        #region Properties

        protected LaunchpadTables LaunchpadTables = new LaunchpadTables ();

        IModuleSecurityContext securityContext;
        protected IModuleSecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo, this)); }
        }

        #endregion

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // fill PageSize combobox
            comboPageSize.AddItem ("10", "10");
            comboPageSize.AddItem ("25", "25");
            comboPageSize.AddItem ("50", "50");
            comboPageSize.AddItem ("100", "100");

            // fill tables list
            foreach (var table in LaunchpadTables.Tables) {
                listTables.AddItem (LocalizeString (table.ResourceKey), table.Name);
            }

            panelGeneralSettings.Visible = SecurityContext.CanManageModule ();
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        { 
            try {
                if (!IsPostBack) {
                    // TODO: Allow select nearest pagesize value
                    comboPageSize.SelectByValue (Settings.PageSize);

                    // check table list items
                    foreach (var table in Settings.Tables) {
                        var item = listTables.Items.FindByValue (table);
                        if (item != null) {
                            item.Selected = true;
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
                Settings.PageSize = int.Parse (comboPageSize.SelectedValue);
                Settings.Tables = listTables.Items.AsEnumerable ().Where (i => i.Selected).Select (i => i.Value).ToList ();

                // remove session variable for active view,
                // since view set may be changed
                Session.Remove ("Launchpad_ActiveView_" + TabModuleId);

                SettingsRepository.SaveSettings (ModuleConfiguration, Settings);

                ModuleController.SynchronizeModule (ModuleId);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

