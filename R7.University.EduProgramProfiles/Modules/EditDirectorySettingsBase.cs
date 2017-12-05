//
//  EditDirectorySettingsBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ControlExtensions;
using R7.University.Controls;
using R7.University.EduProgramProfiles.Models;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfiles.Modules
{
    public class EditDirectorySettingsBase<TSettings>: UniversityModuleSettingsBase<TSettings>
        where TSettings: DirectorySettingsBase, new ()
    {
        #region Controls

        protected DivisionSelector divisionSelector;
        protected RadioButtonList radioDivisionLevel;
        protected CheckBoxList listEduLevels;

        #endregion

        #region Properties

        UniversityModelContext _modelContext;
        protected UniversityModelContext ModelContext =>
            _modelContext ?? (_modelContext = new UniversityModelContext ());

        public override void Dispose ()
        {
            if (_modelContext != null) {
                _modelContext.Dispose ();
            }

            base.Dispose ();
        }

        ViewModelContext _viewModelContext;
        protected ViewModelContext ViewModelContext =>
            _viewModelContext ?? (_viewModelContext = new ViewModelContext (this));

        #endregion

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // fill division levels
            radioDivisionLevel.DataSource = EnumViewModel<DivisionLevel>.GetValues (ViewModelContext, false);
            radioDivisionLevel.DataBind ();

            // bind divisions
            divisionSelector.DataSource = new FlatQuery<DivisionInfo> (ModelContext).ListOrderBy (d => d.Title);
            divisionSelector.DataBind ();

            // fill edulevels list
            foreach (var eduLevel in new EduLevelQuery (ModelContext).List ()) {
                listEduLevels.AddItem (FormatHelper.FormatShortTitle (eduLevel.ShortTitle, eduLevel.Title), eduLevel.EduLevelID.ToString ());
            }
        }

        protected virtual void OnLoadSettings () {}

        public override void LoadSettings ()
        {
            try {
                if (!IsPostBack) {
                    radioDivisionLevel.SelectByValue (Settings.DivisionLevel.ToString ());
                    divisionSelector.DivisionId = Settings.DivisionId;

                    // check edulevels list items
                    foreach (var eduLevelId in Settings.EduLevelIds) {
                        var item = listEduLevels.Items.FindByValue (eduLevelId.ToString ());
                        if (item != null) {
                            item.Selected = true;
                        }
                    }

                    OnLoadSettings ();
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected virtual void OnUpdateSettings () {}

        public override void UpdateSettings ()
        {
            try {
                Settings.EduLevelIds = listEduLevels.Items.AsEnumerable ().Where (i => i.Selected).Select (i => int.Parse (i.Value)).ToList ();
                Settings.DivisionId = divisionSelector.DivisionId;
                Settings.DivisionLevel = (DivisionLevel) Enum.Parse (typeof (DivisionLevel), radioDivisionLevel.SelectedValue, true);

                OnUpdateSettings ();

                SettingsRepository.SaveSettings (ModuleConfiguration, Settings);

                OnSynchronizeModule ();
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected virtual void OnSynchronizeModule ()
        {
            ModuleController.SynchronizeModule (ModuleId);
            CacheHelper.RemoveCacheByPrefix ($"//r7_University/Modules/{UniversityModuleHelper.GetModuleName (ModuleConfiguration)}?ModuleId={ModuleId}");
        }
    }
}
