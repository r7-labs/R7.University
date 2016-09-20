//
//  SettingsEmployeeDirectory.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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
using DotNetNuke.Web.UI.WebControls;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Components;
using R7.University.Data;
using R7.University.EmployeeDirectory.Components;
using R7.University.ViewModels;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.EmployeeDirectory
{
    public partial class SettingsEmployeeDirectory: ModuleSettingsBase<EmployeeDirectorySettings>
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

            comboMode.DataSource = Enum.GetNames (typeof (EmployeeDirectoryMode));
            comboMode.DataBind ();

            // fill edulevels list
            var eduLevels = new EduLevelQuery (ModelContext).List ();
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

                    checkShowAllTeachers.Checked = Settings.ShowAllTeachers;
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
                Settings.Mode = (EmployeeDirectoryMode) Enum.Parse (
                    typeof (EmployeeDirectoryMode),
                    comboMode.SelectedValue);
                Settings.EduLevels = listEduLevels.CheckedItems.Select (i => int.Parse (i.Value)).ToList ();
                Settings.ShowAllTeachers = checkShowAllTeachers.Checked;

                ModuleController.SynchronizeModule (ModuleId);
                CacheHelper.RemoveCacheByPrefix ("//r7_University/Modules/EmployeeDirectory");
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

