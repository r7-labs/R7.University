//
//  EduProgramProfileFormEditModel.cs
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
using Newtonsoft.Json;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;

namespace R7.University.Controls.ViewModels
{
    [Serializable]
    public class EduProgramProfileFormEditModel: EditModelBase<EduProgramProfileFormInfo>, IEduProgramProfileFormWritable
    {
        #region EditModelBase implementation

        public override IEditModel<EduProgramProfileFormInfo> Create (
            EduProgramProfileFormInfo model, ViewModelContext context)
        {
            var viewModel = new EduProgramProfileFormEditModel ();
            CopyCstor.Copy<IEduProgramProfileFormWritable> (model, viewModel);
            viewModel.EduFormViewModel = new EduFormViewModel (model.EduForm, context);
            viewModel.Context = context;

            return viewModel;
        }

        public override EduProgramProfileFormInfo CreateModel ()
        {
            var model = new EduProgramProfileFormInfo ();
            CopyCstor.Copy<IEduProgramProfileFormWritable> (this, model);

            return model;
        }

        public override void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            EduProgramProfileID = targetItemId;
        }

        #endregion

        #region IEduProgramProfileFormWritable implementation

        public long EduProgramProfileFormID { get; set; }

        public int EduProgramProfileID { get; set; }

        public int EduFormID { get; set; }

        public int TimeToLearn { get; set; }

        public int TimeToLearnHours { get; set; }

        public bool IsAdmissive { get; set; }

        [JsonIgnore]
        [Obsolete ("Use EduFormViewModel property instead", true)] 
        public EduFormInfo EduForm { get; set; }

        public EduFormViewModel EduFormViewModel { get; set; }

        #endregion

        #region Bindable properties

        [JsonIgnore]
        public string EduFormTitleLocalized
        {
            get {
                EduFormViewModel.Context = Context;
                return EduFormViewModel.TitleLocalized;
            }
        }

        [JsonIgnore]
        public string TimeToLearnYears_String => (TimeToLearn / 12 > 0) ? (TimeToLearn / 12).ToString () : string.Empty;

        [JsonIgnore]
        public string TimeToLearnMonths_String => (TimeToLearn % 12 > 0) ? (TimeToLearn % 12).ToString () : string.Empty;

        [JsonIgnore]
        public string TimeToLearnHours_String => (TimeToLearnHours > 0) ? TimeToLearnHours.ToString () : string.Empty;

        #endregion
    }
}
