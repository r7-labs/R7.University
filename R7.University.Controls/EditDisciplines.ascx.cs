//
//  EditDisciplines.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2020 Roman M. Yagodin
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
using R7.Dnn.Extensions.Collections;
using R7.Dnn.Extensions.Controls;
using R7.University.Controls.EditModels;
using R7.University.Controls.Queries;
using R7.University.Controls.ViewModels;
using R7.University.Models;
using R7.University.Queries;
using R7.University.ViewModels;

namespace R7.University.Controls
{
    public partial class EditDisciplines: GridAndFormControlBase<EmployeeDisciplineInfo, EmployeeDisciplineEditModel>
    {
        public void OnInit (PortalModuleBase module)
        {
            Module = module;

            using (var modelContext = new UniversityModelContext ()) {
                var eduPrograms = GetEduPrograms (modelContext);

                if (!eduPrograms.IsNullOrEmpty ()) {
                    ddlEduProgram.DataSource = eduPrograms;
                    ddlEduProgram.DataBind ();

                    BindEduProfiles (eduPrograms.First ().EduProgramID, modelContext);
                }
                else {
                    ddlEduProgram.Enabled = false;
                    ddlEduProfile.Enabled = false;
                    buttonAddItem.Enabled = false;
                    buttonUpdateItem.Enabled = false;
                    buttonResetForm.Enabled = false;
                }
            }
        }

        void BindEduProfiles (int eduProgramId, UniversityModelContext modelContext)
        {
            var eduProfiles = GetEduProfiles (eduProgramId, modelContext);

            ddlEduProfile.DataSource = eduProfiles;
            ddlEduProfile.DataBind ();
        }

        IEnumerable<EduProgramViewModel> GetEduPrograms (UniversityModelContext modelContext)
        {
            return new EduProgramQuery (modelContext).ListWithEduLevels ()
                .Select (ep => new EduProgramViewModel (ep))
                .OrderBy (ep => ep.EduLevel.SortIndex)
                .ThenBy (ep => ep.Code)
                .ThenBy (ep => ep.Title);
        }

        IEnumerable<EduProgramProfileViewModel> GetEduProfiles (int eduProgramId, UniversityModelContext modelContext)
        {
            return new EduProgramProfileQuery (modelContext).ListByEduProgram (eduProgramId)
                .Select (epp => new EduProgramProfileViewModel (epp, ViewModelContext))
                .OrderBy (epp => epp.EduProgram.Code)
                .ThenBy (epp => epp.EduProgram.Title)
                .ThenBy (epp => epp.ProfileCode)
                .ThenBy (epp => epp.ProfileTitle)
                .ThenBy (epp => epp.EduLevel.SortIndex);
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnLoadItem (EmployeeDisciplineEditModel item)
        {
            using (var modelContext = new UniversityModelContext ()) {
                var eduProfile = new EduProgramProfileQuery (modelContext).SingleOrDefault (item.EduProgramProfileID);
                var newEduProgramId = eduProfile.EduProgramID;
                var eduProgramId = int.Parse (ddlEduProgram.SelectedValue);

                if (eduProgramId != newEduProgramId) {
                    ddlEduProgram.SelectByValue (newEduProgramId);
                    BindEduProfiles (newEduProgramId, modelContext);
                }

                ddlEduProfile.SelectByValue (item.EduProgramProfileID);
                textDisciplines.Text = item.Disciplines;

                hiddenEduProgramProfileID.Value = item.EduProgramProfileID.ToString ();
            }
        }

        protected override void OnUpdateItem (EmployeeDisciplineEditModel item)
        {
            item.EduProgramProfileID = int.Parse (ddlEduProfile.SelectedValue);
            item.Disciplines = textDisciplines.Text.Trim ();

            using (var modelContext = new UniversityModelContext ()) {
                var eduProfile = new EduProgramProfileQuery (modelContext).SingleOrDefault (item.EduProgramProfileID);
                item.Code = eduProfile.EduProgram.Code;
                item.Title = eduProfile.EduProgram.Title;
                item.ProfileCode = eduProfile.ProfileCode;
                item.ProfileTitle = eduProfile.ProfileTitle;
                item.ProfileStartDate = eduProfile.StartDate;
                item.ProfileEndDate = eduProfile.EndDate;
                item.EduLevelString = UniversityFormatHelper.FormatShortTitle (eduProfile.EduLevel.ShortTitle, eduProfile.EduLevel.Title);
            }
        }

        protected override void OnResetForm ()
        {
            ddlEduProgram.SelectedIndex = 0;
            var eduProgramId = int.Parse (ddlEduProgram.SelectedValue);
            using (var modelContext = new UniversityModelContext ()) {
                BindEduProfiles (eduProgramId, modelContext);
            }

            textDisciplines.Text = string.Empty;
        }

        protected override void BindItems (IEnumerable<EmployeeDisciplineEditModel> items)
        {
            base.BindItems (items);

            gridItems.Attributes.Add ("data-items", Json.Serialize (items));
        }

        #endregion

        protected void ddlEduProgram_SelectedIndexChanged (object sender, EventArgs e)
        {
            using (var modelContext = new UniversityModelContext ()) {
                var eduProgramId = int.Parse (ddlEduProgram.SelectedValue);
                BindEduProfiles (eduProgramId, modelContext);
            }
        }
    }
}
