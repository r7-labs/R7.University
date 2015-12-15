//
// EditDocuments.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Services.Localization;
using R7.University.ControlExtensions;
using DotNetNuke.Common.Utilities;
using DotNetNuke.R7;

namespace R7.University.Controls
{
    public partial class EditEduForms: 
        GridAndFormEditControlBase<EduProgramProfileFormInfo,EduProgramProfileFormViewModel>
    {
        public void OnInit (PortalModuleBase module, IEnumerable<EduFormInfo> eduForms)
        {
            Module = module;

            SetEduForms (eduForms);

            comboEduForm.DataSource = EduFormViewModel.GetBindableList (eduForms, ViewModelContext, false);
            comboEduForm.DataBind ();
        }

        protected void SetEduForms (IEnumerable<EduFormInfo> eduForms)
        {
            ViewState ["eduForms"] = eduForms.ToList ();
        }

        protected EduFormInfo GetEduForm (int eduFormId)
        {
            var eduForms = (List<EduFormInfo>) ViewState ["eduForms"];
            return eduForms.Single (ef => ef.EduFormID == eduFormId);
        }

        #region implemented abstract members of GridAndFormEditControlBase

        protected override void OnInitControls ()
        {
            InitControls (gridEduForms, hiddenEduFormItemID, 
                buttonAddEduForm, buttonUpdateEduForm, buttonCancelEditEduForm);
        }

        protected override void OnLoadItem (EduProgramProfileFormViewModel item)
        {
            comboEduForm.SelectByValue (item.EduFormID);
            checkIsAdmissive.Checked = item.IsAdmissive;
            textTimeToLearnYears.Text = (item.TimeToLearn / 12).ToString ();
            textTimeToLearnMonths.Text = (item.TimeToLearn % 12).ToString ();
        }

        protected override void OnUpdateItem (EduProgramProfileFormViewModel item)
        {
            item.EduFormID = int.Parse (comboEduForm.SelectedValue);
            item.EduFormTitle = GetEduForm (item.EduFormID).Title;
            item.IsAdmissive = checkIsAdmissive.Checked;
            item.TimeToLearn = int.Parse (textTimeToLearnYears.Text) * 12 + int.Parse (textTimeToLearnMonths.Text);
        }

        protected override void OnResetForm ()
        {
            comboEduForm.SelectedIndex = 0;
            textTimeToLearnYears.Text = "0";
            textTimeToLearnMonths.Text = "0";
            checkIsAdmissive.Checked = false;
        }

        #endregion
    }
}
