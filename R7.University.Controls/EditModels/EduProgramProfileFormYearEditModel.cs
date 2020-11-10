//
//  EduProgramProfileFormEditModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2018 Roman M. Yagodin
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
using System.Web;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.EditModels;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Controls.EditModels
{
    [Serializable]
    public class EduProgramProfileFormYearEditModel: EditModelBase<EduProgramProfileFormYearInfo>, IEduProgramProfileFormYearWritable
    {
        #region EditModelBase implementation

        public IEditModel<EduProgramProfileFormYearInfo> Create (
            EduProgramProfileFormYearInfo model, ViewModelContext context, IYear lastYear)
        {
            var viewModel = (EduProgramProfileFormYearEditModel) Create (model, context);
            viewModel.YearString = model.Year.FormatWithCourse (lastYear);
            return viewModel;
        }

        public override IEditModel<EduProgramProfileFormYearInfo> Create (
            EduProgramProfileFormYearInfo model, ViewModelContext context)
        {
            var viewModel = new EduProgramProfileFormYearEditModel ();
            CopyCstor.Copy<IEduProgramProfileFormYearWritable> (model, viewModel);
            CopyCstor.Copy<IPublishableEntityWritable> (model, viewModel);
            viewModel.EduFormViewModel = new EduFormViewModel (model.EduForm, context);
            viewModel.HasEduVolume = model.EduVolume != null;
            viewModel.HasContingent = model.Contingent != null;
            viewModel.Context = context;

            return viewModel;
        }

        public override EduProgramProfileFormYearInfo CreateModel ()
        {
            var model = new EduProgramProfileFormYearInfo ();
            CopyCstor.Copy<IEduProgramProfileFormYearWritable> (this, model);
            CopyCstor.Copy<IPublishableEntityWritable> (this, model);

            return model;
        }

        public override void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            EduProgramProfileId = targetItemId;
        }

        [JsonIgnore]
        public override bool IsPublished => this.IsPublished (HttpContext.Current.Timestamp);

        #endregion

        #region IEduProgramProfileFormYearWritable implementation

        public int EduProgramProfileFormYearId { get; set; }

        public int EduProgramProfileId { get; set; }

        public int EduFormId { get; set; }

        public int? YearId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [JsonIgnore]
        [Obsolete ("Use EduFormViewModel property instead", true)] 
        public IEduForm EduForm { get; set; }

        public EduFormViewModel EduFormViewModel { get; set; }

        [JsonIgnore]
        public IYear Year { get; set; }

        [JsonIgnore]
        public IEduProfile EduProgramProfile { get; set; }

        [JsonIgnore]
        public IEduVolume EduVolume { get; set; }

        [JsonIgnore]
        public IContingent Contingent { get; set; }

        #endregion

        public bool HasContingent { get; set; }

        public bool HasEduVolume { get; set; }

        #region Bindable properties

        public string YearString { get; set; }

        [JsonIgnore]
        public string EduFormTitleLocalized {
            get {
                EduFormViewModel.Context = Context;
                return EduFormViewModel.TitleLocalized;
            }
        }

        [JsonIgnore]
        public string EditEduVolumeIconUrl => HasEduVolume ? UniversityIcons.Edit : UniversityIcons.AddAlternate;

        [JsonIgnore]
        public string EditContingentIconUrl => HasContingent ? UniversityIcons.Edit : UniversityIcons.AddAlternate;

        [JsonIgnore]
        public string EditEduVolumeUrl => 
            Context.Module.EditUrl (HasEduVolume ? "eduvolume_id" : "eduprogramprofileformyear_id", EduProgramProfileFormYearId.ToString (), "EditEduVolume");

        [JsonIgnore]
        public string EditContingentUrl =>
            Context.Module.EditUrl (HasContingent ? "contingent_id" : "eduprogramprofileformyear_id", EduProgramProfileFormYearId.ToString (), "EditContingent");

        [JsonIgnore]
        public bool EditReferencedEntitiesActionsVisible => EditState != ModelEditState.Added;

        #endregion
    }
}
