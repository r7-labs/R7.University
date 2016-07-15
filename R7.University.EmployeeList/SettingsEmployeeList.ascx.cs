//
//  SettingsEmployeeList.ascx.cs
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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Components;
using R7.University.ControlExtensions;
using R7.University.EmployeeList.Components;
using R7.University.Models;
using R7.University.Queries;
using R7.University.Utilities;

namespace R7.University.EmployeeList
{
    public partial class SettingsEmployeeList: ModuleSettingsBase<EmployeeListSettings>
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

            // get divisions
            var divisions = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);

            // insert default item
            divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

            // bind divisions to the tree
            treeDivisions.DataSource = divisions;
            treeDivisions.DataBind ();

            // sort type
            comboSortType.AddItem (LocalizeString ("SortTypeByMaxWeight.Text"), "0");
            comboSortType.AddItem (LocalizeString ("SortTypeByTotalWeight.Text"), "1");
            comboSortType.AddItem (LocalizeString ("SortTypeByName.Text"), "2");
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    // select node and expand tree to it
                    Utils.SelectAndExpandByValue (treeDivisions, Settings.DivisionID.ToString ());

                    checkIncludeSubdivisions.Checked = Settings.IncludeSubdivisions;
                    checkHideHeadEmployee.Checked = Settings.HideHeadEmployee;
                    comboSortType.SelectByValue (Settings.SortType);
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
                Settings.DivisionID = int.Parse (treeDivisions.SelectedValue);
                Settings.IncludeSubdivisions = checkIncludeSubdivisions.Checked;
                Settings.HideHeadEmployee = checkHideHeadEmployee.Checked;
                Settings.SortType = int.Parse (comboSortType.SelectedValue);
                Settings.PhotoWidth = int.Parse (textPhotoWidth.Text);

                ModuleController.SynchronizeModule (ModuleId);
                CacheHelper.RemoveCacheByPrefix ("//r7_University/Modules/EmployeeList?TabModuleId=" + TabModuleId);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

