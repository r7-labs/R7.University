//
//  EditEduFormYears.ascx.cs
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

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using R7.Dnn.Extensions.ControlExtensions;
using R7.Dnn.Extensions.Utilities;
using R7.University.Controls.EditModels;
using R7.University.Models;

namespace R7.University.Controls
{
    public partial class EditEduFormYears: 
        GridAndFormControlBase<EduProgramProfileFormYearInfo,EduProgramProfileFormYearEditModel>
    {
        public void OnInit (PortalModuleBase module, IEnumerable<EduFormInfo> eduForms, IEnumerable<YearInfo> years)
        {
            Module = module;

            var eduFormViewModels = EduFormViewModel.GetBindableList (eduForms, ViewModelContext, false);

            ViewState ["eduForms"] = Json.Serialize (eduFormViewModels.ToList ());
            ViewState ["years"] = Json.Serialize (years.ToList ());

            radioEduForm.DataSource = eduFormViewModels;
            radioEduForm.DataBind ();
            radioEduForm.SelectedIndex = 0;

            comboYear.DataSource = years;
            comboYear.DataBind ();
            comboYear.InsertDefaultItem ("-");
        }

        protected EduFormViewModel GetEduForm (int eduFormId)
        {
            var eduForms = Json.Deserialize<List<EduFormViewModel>> ((string) ViewState ["eduForms"]);
            return eduForms.Single (ef => ef.EduFormID == eduFormId);
        }

        protected YearInfo GetYear (int yearId)
        {
            var years = Json.Deserialize<List<YearInfo>> ((string) ViewState ["years"]);
            return years.Single (y => y.YearId == yearId);
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnLoadItem (EduProgramProfileFormYearEditModel item)
        {
            comboYear.SelectByValue (item.YearId);
            radioEduForm.SelectByValue (item.EduFormId);

            datetimeStartDate.SelectedDate = item.StartDate;
            datetimeEndDate.SelectedDate = item.EndDate;

            hiddenEduFormID.Value = item.EduFormId.ToString ();
        }

        protected override void OnUpdateItem (EduProgramProfileFormYearEditModel item)
        {
            item.YearId = TypeUtils.ParseToNullable<int> (comboYear.SelectedValue);
            if (item.YearId != null) {
                item.YearString = GetYear (item.YearId.Value).Year.ToString ();
            }
            else {
                item.YearString = "-";
            }

            item.EduFormId = int.Parse (radioEduForm.SelectedValue);
            item.EduFormViewModel = GetEduForm (item.EduFormId);

            item.StartDate = datetimeStartDate.SelectedDate;
            item.EndDate = datetimeEndDate.SelectedDate;
        }

        protected override void OnResetForm ()
        {
            comboYear.SelectedIndex = 0;
            radioEduForm.SelectedIndex = 0;
            datetimeStartDate.SelectedDate = null;
            datetimeEndDate.SelectedDate = null;
        }

        IEnumerable<EduProgramProfileFormYearEditModel> SortItems (IEnumerable<EduProgramProfileFormYearEditModel> items)
        {
            return items.OrderBy (eppfy => eppfy.EduFormViewModel.SortIndex)
                        .ThenByDescending (eppfy => eppfy.YearString);
        }

        protected override void BindItems (IEnumerable<EduProgramProfileFormYearEditModel> items)
        {
            base.BindItems (SortItems (items));

            gridItems.Attributes.Add ("data-items", Json.Serialize (items));
        }

        #endregion
    }
}
