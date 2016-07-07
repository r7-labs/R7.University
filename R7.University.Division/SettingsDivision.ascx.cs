//
//  SettingsDivision.ascx.cs
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
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Division.Components;
using R7.University.Models;
using R7.University.Queries;
using R7.University.Utilities;

namespace R7.University.Division
{
    public partial class SettingsDivision : ModuleSettingsBase<DivisionSettings>
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

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    // get divisions
                    var divisions = new Query<DivisionInfo> (ModelContext).Execute (d => d.Title);

                    // insert default item
                    divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

                    // bind divisions to the tree
                    treeDivisions.DataSource = divisions;
                    treeDivisions.DataBind ();

                    // select node and expand tree to it
                    Utils.SelectAndExpandByValue (treeDivisions, Settings.DivisionID.ToString ());
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

                ModuleController.SynchronizeModule (ModuleId);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

