//
//  EditEduForms.ascx.cs
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

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using R7.Dnn.Extensions.ControlExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Controls
{
    public partial class EditEduForms: 
        GridAndFormControlBase<EduProgramProfileFormInfo,EduProgramProfileFormViewModel>
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

        protected override void OnInitControls ()
        {
            InitControls (gridEduForms, hiddenEduFormItemID, 
                          buttonAddEduForm, buttonUpdateEduForm, buttonCancelEditEduForm, buttonResetForm);
        }

        protected override void OnLoadItem (EduProgramProfileFormViewModel item)
        {
            radioEduForm.SelectByValue (item.EduFormID);
            checkIsAdmissive.Checked = item.IsAdmissive;

            textTimeToLearnYears.Text = (item.TimeToLearn / 12).ToString ();
            textTimeToLearnMonths.Text = (item.TimeToLearn % 12).ToString ();
            textTimeToLearnHours.Text = item.TimeToLearnHours.ToString ();

            hiddenEduFormID.Value = item.EduFormID.ToString ();
        }

        protected override void OnUpdateItem (EduProgramProfileFormViewModel item)
        {
            item.EduFormID = int.Parse (radioEduForm.SelectedValue);
            item.EduFormViewModel = GetEduForm (item.EduFormID);
            item.IsAdmissive = checkIsAdmissive.Checked;

            item.TimeToLearnHours = int.Parse (textTimeToLearnHours.Text);
            item.TimeToLearn = int.Parse (textTimeToLearnYears.Text) * 12 + int.Parse (textTimeToLearnMonths.Text);
        }

        protected override void OnResetForm ()
        {
            radioEduForm.SelectedIndex = 0;
            textTimeToLearnYears.Text = "0";
            textTimeToLearnMonths.Text = "0";
            textTimeToLearnHours.Text = "0";
            checkIsAdmissive.Checked = false;
        }

        protected override void BindItems (IEnumerable<EduProgramProfileFormViewModel> items)
        {
            base.BindItems (items);

            gridEduForms.Attributes.Add ("data-items", Json.Serialize (items));
        }

        #endregion
    }
}
