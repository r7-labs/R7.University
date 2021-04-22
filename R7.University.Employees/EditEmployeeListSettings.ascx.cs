//
//  EditEmployeeListSettings.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2020 Roman M. Yagodin
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Text;
using R7.University.Dnn.Modules;
using R7.University.Employees.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Employees
{
    public partial class EditEmployeeListSettings: UniversityModuleSettingsBase<EmployeeListSettings>
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

            divisionSelector.DataSource = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            divisionSelector.DataBind ();

            // sort type
            comboSortType.AddItem (LocalizeString ("SortTypeByMaxWeightInDivision.Text"), ((int) EmployeeListSortType.ByMaxWeightInDivision).ToString ());
            comboSortType.AddItem (LocalizeString ("SortTypeByName.Text"), ((int) EmployeeListSortType.ByName).ToString ());
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    divisionSelector.DivisionId = Settings.DivisionID;
                    checkIncludeSubdivisions.Checked = Settings.IncludeSubdivisions;
                    checkHideHeadEmployee.Checked = Settings.HideHeadEmployee;
                    comboSortType.SelectByValue (Settings.SortType);

                    if (Settings.PhotoWidth > 0) {
                        textPhotoWidth.Text = Settings.PhotoWidth.ToString ();
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
                Settings.DivisionID = divisionSelector.DivisionId ?? Null.NullInteger;
                Settings.IncludeSubdivisions = checkIncludeSubdivisions.Checked;
                Settings.HideHeadEmployee = checkHideHeadEmployee.Checked;
                Settings.SortType = int.Parse (comboSortType.SelectedValue);

                Settings.PhotoWidth = ParseHelper.ParseToNullable<int> (textPhotoWidth.Text) ?? 0;

                SettingsRepository.SaveSettings (ModuleConfiguration, Settings);

                ModuleController.SynchronizeModule (ModuleId);

                DataCache.ClearCache ("//r7_University/Modules/EmployeeList?TabModuleId=" + TabModuleId);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

