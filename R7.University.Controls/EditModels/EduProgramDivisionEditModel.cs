using System;
using System.Web;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EditModels;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Controls.EditModels
{
    [Serializable]
    public class EduProgramDivisionEditModel: EditModelBase<EduProgramDivisionInfo>, IEduProgramDivisionWritable
    {
        #region EditModelBase implementation

        public override IEditModel<EduProgramDivisionInfo> Create (EduProgramDivisionInfo model, ViewModelContext context)
        {
            var viewModel = new EduProgramDivisionEditModel ();

            CopyCstor.Copy<IEduProgramDivisionWritable> (model, viewModel);
            viewModel.DivisionTitle = model.Division.Title;
            viewModel.StartDate = model.Division.StartDate;
            viewModel.EndDate = model.Division.EndDate;
            viewModel.Dnn = context;

            return viewModel;
        }

        public override EduProgramDivisionInfo CreateModel ()
        {
            var epd = new EduProgramDivisionInfo ();
            CopyCstor.Copy<IEduProgramDivisionWritable> (this, epd);

            return epd;
        }

        public override void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            this.SetModelId ((ModelType) Enum.Parse (typeof (ModelType), targetItemKey), targetItemId);
        }

        [JsonIgnore]
        public override bool IsPublished => ModelHelper.IsPublished (HttpContext.Current.Timestamp, StartDate, EndDate);

        #endregion

        #region IEduProgramDivisionWritable implementation

        public long EduProgramDivisionId { get; set; }

        public int? EduProgramId { get; set; }

        public int? EduProgramProfileId { get; set; }

        public int DivisionId { get; set; }

        [JsonIgnore]
        [Obsolete ("Don't use this property directly", true)]
        public IDivision Division { get; set; }

        public string DivisionRole { get; set; }

        #endregion

        #region Flattened external properties

        public string DivisionTitle { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        #endregion
    }
}
