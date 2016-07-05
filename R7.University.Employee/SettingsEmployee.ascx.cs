//
//  SettingsEmployee.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Framework;
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.Employee.Components;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Employee
{
    public partial class SettingsEmployee : ModuleSettingsBase<EmployeeSettings>
    {
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // bind employees to the combobox
            using (var modelContext = new UniversityModelContext ()) {
                comboEmployees.DataSource = new Query<EmployeeInfo> (modelContext).Execute (empl => empl.LastName);
                comboEmployees.DataBind ();
            }

            comboEmployees.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (AJAX.IsInstalled ())
                    AJAX.RegisterScriptManager ();

                if (!IsPostBack) {
                    if (!Null.IsNull (Settings.EmployeeID))
                        comboEmployees.SelectByValue (Settings.EmployeeID);
                    else
                        comboEmployees.SelectedIndex = 0;

                    checkAutoTitle.Checked = Settings.AutoTitle;
                    checkShowCurrentUser.Checked = Settings.ShowCurrentUser;
					textPhotoWidth.Text = Settings.PhotoWidth.ToString ();
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
                Settings.ShowCurrentUser = checkShowCurrentUser.Checked;
                Settings.EmployeeID = int.Parse (comboEmployees.SelectedValue);
                Settings.AutoTitle = checkAutoTitle.Checked;
                Settings.PhotoWidth = int.Parse (textPhotoWidth.Text);

                ModuleController.SynchronizeModule (ModuleId);
                CacheHelper.RemoveCacheByPrefix ("//r7_University/Modules/Employee?ModuleId=" + ModuleId);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

