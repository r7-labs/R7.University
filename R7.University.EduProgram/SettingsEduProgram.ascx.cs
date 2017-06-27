//
//  SettingsEduProgram.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
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
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EduProgram.Components;
using R7.University.EduProgram.Queries;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.ViewModels;

namespace R7.University.EduProgram
{    
    public partial class SettingsEduProgram : UniversityModuleSettingsBase<EduProgramSettings>
    {
        #region Repository handling

        private IModelContext modelContext;
        protected IModelContext ModelContext
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

            comboEduLevel.DataSource = new EduLevelQuery (ModelContext).ListForEduProgram ();
            comboEduLevel.DataBind ();

            if (comboEduLevel.Items.Count > 0) {
                BindEduPrograms (int.Parse (comboEduLevel.SelectedValue));
            }
        }

        protected void comboEduLevel_SelectedIndexChanged (object sender, EventArgs e)
        {
            BindEduPrograms (int.Parse (comboEduLevel.SelectedValue));
        }

        protected void BindEduPrograms (int eduLevelId)
        {
            comboEduProgram.DataSource = new EduProgramQuery (ModelContext).ListByEduLevel (eduLevelId)
                .Select (ep => new ListItemViewModel (ep.EduProgramID, FormatHelper.FormatEduProgramTitle (ep.Code, ep.Title)));
            
            comboEduProgram.DataBind ();
            comboEduProgram.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
        }

        /// <summary>
        /// Handles the loading of the module setting for this control
        /// </summary>
        public override void LoadSettings ()
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Settings.EduProgramId != null) {
                        var eduProgram = ModelContext.Get<EduProgramInfo> (Settings.EduProgramId.Value);
                        comboEduLevel.SelectByValue (eduProgram.EduLevelID);
                        BindEduPrograms (eduProgram.EduLevelID);
                        comboEduProgram.SelectByValue (eduProgram.EduProgramID);
                    }

                    checkAutoTitle.Checked = Settings.AutoTitle;
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
      
        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings ()
        {
            try
            {
                Settings.EduProgramId = TypeUtils.ParseToNullable<int> (comboEduProgram.SelectedValue);
                Settings.AutoTitle = checkAutoTitle.Checked;

                SettingsRepository.SaveSettings (ModuleConfiguration, Settings);

                CacheHelper.RemoveCacheByPrefix ("//r7_University/Modules/EduProgram?ModuleId=" + ModuleId);

                ModuleController.SynchronizeModule (ModuleId);
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}

