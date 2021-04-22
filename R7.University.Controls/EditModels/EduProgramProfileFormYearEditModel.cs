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
            viewModel.Dnn = context;

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
        public IEduProfile EduProfile { get; set; }

        [JsonIgnore]
        public IEduVolume EduVolume { get; set; }

        [JsonIgnore]
        public IContingent Contingent { get; set; }

        #endregion

        #region  Flattened external properties

        public bool HasContingent { get; set; }

        public bool HasEduVolume { get; set; }

        public string YearString { get; set; }

        #endregion

        #region Derieved properties

        [JsonIgnore]
        public string EduFormTitleLocalized {
            get {
                EduFormViewModel.Context = Dnn;
                return EduFormViewModel.TitleLocalized;
            }
        }

        [JsonIgnore]
        public string EditEduVolumeIconUrl => HasEduVolume ? UniversityIcons.Edit : UniversityIcons.AddAlternate;

        [JsonIgnore]
        public string EditContingentIconUrl => HasContingent ? UniversityIcons.Edit : UniversityIcons.AddAlternate;

        [JsonIgnore]
        public string EditEduVolumeUrl =>
            Dnn.Module.EditUrl (HasEduVolume ? "eduvolume_id" : "eduprogramprofileformyear_id", EduProgramProfileFormYearId.ToString (), "EditEduVolume");

        [JsonIgnore]
        public string EditContingentUrl =>
            Dnn.Module.EditUrl (HasContingent ? "contingent_id" : "eduprogramprofileformyear_id", EduProgramProfileFormYearId.ToString (), "EditContingent");

        [JsonIgnore]
        public bool EditReferencedEntitiesActionsVisible => EditState != ModelEditState.Added;

        #endregion
    }
}
