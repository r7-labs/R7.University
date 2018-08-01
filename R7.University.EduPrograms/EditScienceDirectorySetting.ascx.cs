//
//  EditScienceDirectorySettings.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
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
using R7.Dnn.Extensions.Caching;
using R7.Dnn.Extensions.Controls;
using R7.University.ControlExtensions;
using R7.University.EduPrograms.Models;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.ViewModels;

namespace R7.University.EduPrograms
{
    public partial class EditScienceDirectorySettings : UniversityModuleSettingsBase<ScienceDirectorySettings>
    {
        #region Model context

        UniversityModelContext _modelContext;
        protected UniversityModelContext ModelContext => _modelContext ?? (_modelContext = new UniversityModelContext ());
       
        public override void Dispose ()
        {
            if (_modelContext != null) {
                _modelContext.Dispose ();
            }

            base.Dispose ();
        }

        #endregion

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // fill edulevels list
            foreach (var eduLevel in new EduLevelQuery (ModelContext).ListForEduProgram ()) {
                listEduLevels.AddItem (UniversityFormatHelper.FormatShortTitle (eduLevel.ShortTitle, eduLevel.Title), eduLevel.EduLevelID.ToString ());
            }

            // bind divisions
            divisionSelector.DataSource = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            divisionSelector.DataBind ();
        }

        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    // check edulevels list items
                    foreach (var eduLevelId in Settings.EduLevelIds) {
                        var item = listEduLevels.Items.FindByValue (eduLevelId.ToString ());
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

        public override void UpdateSettings ()
        {
            try {
                Settings.EduLevelIds = listEduLevels.Items.AsEnumerable ().Where (i => i.Selected).Select (i => int.Parse (i.Value));
                Settings.DivisionId = divisionSelector.DivisionId;

                SettingsRepository.SaveSettings (ModuleConfiguration, Settings);

                ModuleController.SynchronizeModule (ModuleId);

                CacheHelper.RemoveCacheByPrefix ($"//r7_University/Modules/ScienceDirectory?ModuleId={ModuleId}");
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

