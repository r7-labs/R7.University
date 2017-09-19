//
//  SettingsEduProgramDirectory.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
using R7.Dnn.Extensions.ControlExtensions;
using R7.University.ControlExtensions;
using R7.University.EduProgramDirectory.Models;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.ViewModels;
using R7.Dnn.Extensions.Utilities;

namespace R7.University.EduProgramDirectory
{
    public partial class SettingsEduProgramDirectory : UniversityModuleSettingsBase<EduProgramDirectorySettings>
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

            // fill edulevels list
            foreach (var eduLevel in new EduLevelQuery (ModelContext).ListForEduProgram ()) {
                listEduLevels.AddItem (FormatHelper.FormatShortTitle (eduLevel.ShortTitle, eduLevel.Title), eduLevel.EduLevelID.ToString ());
            }

            // fill columns list
            foreach (var column in Enum.GetNames (typeof (EduProgramDirectoryColumn))) {
                listColumns.AddItem (LocalizeString ("EduProgram" + column + ".Column"), column);
            }

            // bind divisions
            divisionSelector.DataSource = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            divisionSelector.DataBind ();
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    // check edulevels list items
                    foreach (var eduLevelId in Settings.EduLevels) {
                        var item = listEduLevels.Items.FindByValue (eduLevelId.ToString ());
                        if (item != null) {
                            item.Selected = true;
                        }
                    }

                    // check columns list items
                    foreach (var columnString in Settings.Columns) {
                        var item = listColumns.Items.FindByValue (columnString);
                        if (item != null) {
                            item.Selected = true;
                        }
                    }

                    divisionSelector.DivisionId = Settings.DivisionId;
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
                Settings.EduLevels = listEduLevels.Items.AsEnumerable ().Where (i => i.Selected).Select (i => int.Parse (i.Value)).ToList ();
                Settings.Columns = listColumns.Items.AsEnumerable ().Where (i => i.Selected).Select (i => i.Value).ToList ();
                Settings.DivisionId = divisionSelector.DivisionId;

                SettingsRepository.SaveSettings (ModuleConfiguration, Settings);

                ModuleController.SynchronizeModule (ModuleId);

                CacheHelper.RemoveCacheByPrefix ($"//r7_University/Modules/EduProgramDirectory?ModuleId={ModuleId}");
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

