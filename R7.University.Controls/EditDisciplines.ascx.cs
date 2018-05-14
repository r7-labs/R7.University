//
//  EditDisciplines.ascx.cs
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
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using R7.Dnn.Extensions.Controls;
using R7.University.ControlExtensions;
using R7.University.Controls.EditModels;
using R7.University.Controls.Queries;
using R7.University.Controls.ViewModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Controls
{
    public partial class EditDisciplines : GridAndFormControlBase<EmployeeDisciplineInfo, EmployeeDisciplineEditModel>
    {
        public void OnInit (PortalModuleBase module, IEnumerable<EduLevelInfo> eduLevels)
        {
            Module = module;

            comboEduLevel.DataSource = eduLevels;
            comboEduLevel.DataBind ();

            if (eduLevels.Any ()) {
                BindEduProgramProfiles (eduLevels.First ().EduLevelID);
            }
        }

        void BindEduProgramProfiles (int eduLevelId)
        {
            using (var modelContext = new UniversityModelContext ()) {
                var epps = new EduProgramProfileQuery (modelContext).ListByEduLevel (eduLevelId)
                                                                    .Select (epp => new EduProgramProfileViewModel (epp))
                                                                    .OrderBy (epp => epp.EduProgram.Code)
                                                                    .ThenBy (epp => epp.EduProgram.Title)
                                                                    .ThenBy (epp => epp.ProfileCode)
                                                                    .ThenBy (epp => epp.ProfileTitle);

                comboEduProgramProfile.DataSource = epps;
                comboEduProgramProfile.DataBind ();
            }
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnLoadItem (EmployeeDisciplineEditModel item)
        {
            using (var modelContext = new UniversityModelContext ()) {
                var profile = new EduProgramProfileQuery (modelContext).SingleOrDefault (item.EduProgramProfileID);

                var eduLevelId = int.Parse (comboEduLevel.SelectedValue);
                var newEduLevelId = profile.EduLevelId;
                if (eduLevelId != newEduLevelId) {
                    comboEduLevel.SelectByValue (newEduLevelId);
                    BindEduProgramProfiles (newEduLevelId);
                }

                comboEduProgramProfile.SelectByValue (item.EduProgramProfileID);
                textDisciplines.Text = item.Disciplines;

                hiddenEduProgramProfileID.Value = item.EduProgramProfileID.ToString ();
            }
        }

        protected override void OnUpdateItem (EmployeeDisciplineEditModel item)
        {
            item.EduProgramProfileID = int.Parse (comboEduProgramProfile.SelectedValue);
            item.Disciplines = textDisciplines.Text.Trim ();

            using (var modelContext = new UniversityModelContext ()) {
                var profile = new EduProgramProfileQuery (modelContext).SingleOrDefault (item.EduProgramProfileID);
                item.Code = profile.EduProgram.Code;
                item.Title = profile.EduProgram.Title;
                item.ProfileCode = profile.ProfileCode;
                item.ProfileTitle = profile.ProfileTitle;
                item.ProfileStartDate = profile.StartDate;
                item.ProfileEndDate = profile.EndDate;
                item.EduLevelString = UniversityFormatHelper.FormatShortTitle (profile.EduLevel.ShortTitle, profile.EduLevel.Title);
            }
        }

        protected override void OnResetForm ()
        {
            comboEduLevel.SelectByValue (-1);
            comboEduProgramProfile.SelectByValue (-1);
            textDisciplines.Text = string.Empty;
        }

        protected override void BindItems (IEnumerable<EmployeeDisciplineEditModel> items)
        {
            base.BindItems (items);

            gridItems.Attributes.Add ("data-items", Json.Serialize (items));
        }

        #endregion

        protected void comboEduLevel_SelectedIndexChanged (object sender, EventArgs e)
        {
            // store currently selected edu. program profile title
            var selectedEduProgramProfileTitle = comboEduProgramProfile.SelectedItem?.Text;

            var eduLevelId = int.Parse (comboEduLevel.SelectedValue);
            BindEduProgramProfiles (eduLevelId);

            // try to select edu. program profile with same title
            if (!string.IsNullOrEmpty (selectedEduProgramProfileTitle)) {
                comboEduProgramProfile.SelectByText (selectedEduProgramProfileTitle, StringComparison.CurrentCulture);
            }
        }
    }
}
