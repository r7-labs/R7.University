//
//  EditEduForms.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using DotNetNuke.Entities.Modules;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Controls
{
    public partial class EditEduForms: 
        GridAndFormEditControlBase<EduProgramProfileFormInfo,EduProgramProfileFormViewModel>
    {
        public void OnInit (PortalModuleBase module, IEnumerable<EduFormInfo> eduForms)
        {
            Module = module;

            var eduFormViewModels = EduFormViewModel.GetBindableList (eduForms, ViewModelContext, false);

            ViewState ["eduForms"] = XmlSerializationHelper.Serialize (eduFormViewModels.ToList ());

            radioEduForm.DataSource = eduFormViewModels;
            radioEduForm.DataBind ();
            radioEduForm.SelectedIndex = 0;
        }

        protected EduFormViewModel GetEduForm (int eduFormId)
        {
            var eduForms = XmlSerializationHelper.Deserialize<List<EduFormViewModel>> (ViewState ["eduForms"]);
            return eduForms.Single (ef => ef.EduFormID == eduFormId);
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override string TargetItemKey
        {
            get { return string.Empty; }
        }

        protected override void OnInitControls ()
        {
            InitControls (gridEduForms, hiddenEduFormItemID, 
                buttonAddEduForm, buttonUpdateEduForm, buttonCancelEditEduForm);
        }

        protected override void OnLoadItem (EduProgramProfileFormViewModel item)
        {
            radioEduForm.SelectByValue (item.EduFormID);
            checkIsAdmissive.Checked = item.IsAdmissive;

            if (item.TimeToLearnUnit [0] == (char) TimeToLearnUnit.Hours) {
                textTimeToLearnYears.Text = "0";
                textTimeToLearnMonths.Text = "0";
                textTimeToLearnHours.Text = item.TimeToLearn.ToString ();
            }
            else {
                textTimeToLearnYears.Text = (item.TimeToLearn / 12).ToString ();
                textTimeToLearnMonths.Text = (item.TimeToLearn % 12).ToString ();
                textTimeToLearnHours.Text = "0";
            }
        }

        protected override void OnUpdateItem (EduProgramProfileFormViewModel item)
        {
            item.EduFormID = int.Parse (radioEduForm.SelectedValue);
            item.EduFormViewModel = GetEduForm (item.EduFormID);
            item.IsAdmissive = checkIsAdmissive.Checked;

            var timeToLearnHours = int.Parse (textTimeToLearnHours.Text);
            if (timeToLearnHours > 0) {
                item.TimeToLearn = timeToLearnHours;
                item.TimeToLearnUnit = ((char) TimeToLearnUnit.Hours).ToString ();
            }
            else {
                item.TimeToLearn = int.Parse (textTimeToLearnYears.Text) * 12 + int.Parse (textTimeToLearnMonths.Text);
                item.TimeToLearnUnit = ((char) TimeToLearnUnit.Months).ToString ();
            }
        }

        protected override void OnResetForm ()
        {
            radioEduForm.SelectedIndex = 0;
            textTimeToLearnYears.Text = "0";
            textTimeToLearnMonths.Text = "0";
            textTimeToLearnHours.Text = "0";
            checkIsAdmissive.Checked = false;
        }

        #endregion
    }
}
