using System;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EditModels;
using R7.University.Models;

namespace R7.University.Controls.EditModels
{
    [Serializable]
    public class EduProgramProfileFormEditModel: EditModelBase<EduProgramProfileFormInfo>, IEduProgramProfileFormWritable
    {
        #region EditModelBase implementation

        public override IEditModel<EduProgramProfileFormInfo> Create (
            EduProgramProfileFormInfo model, ViewModelContext dnn)
        {
            var viewModel = new EduProgramProfileFormEditModel ();
            CopyCstor.Copy<IEduProgramProfileFormWritable> (model, viewModel);
            viewModel.EduFormViewModel = new EduFormViewModel (model.EduForm, dnn);
            viewModel.Dnn = dnn;

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

        public int EduProgramProfileFormID { get; set; }

        public int EduProgramProfileID { get; set; }

        public int EduFormID { get; set; }

        public int TimeToLearn { get; set; }

        public int TimeToLearnHours { get; set; }

        public bool IsAdmissive { get; set; }

        [JsonIgnore]
        [Obsolete ("Don't use this property directly", true)]
        public IEduForm EduForm { get; set; }

        public EduFormViewModel EduFormViewModel { get; set; }

        #endregion

        #region Derieved properties

        [JsonIgnore]
        public string EduFormTitleLocalized
        {
            get {
                EduFormViewModel.Context = Dnn;
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
