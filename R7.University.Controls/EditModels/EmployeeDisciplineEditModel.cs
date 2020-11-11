using System;
using System.Web;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EditModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Controls.EditModels
{
    [Serializable]
    public class EmployeeDisciplineEditModel : EditModelBase<EmployeeDisciplineInfo>, IEmployeeDisciplineWritable
    {
        #region EditModelBase implementation

        [JsonIgnore]
        public override bool IsPublished => ModelHelper.IsPublished (HttpContext.Current.Timestamp, ProfileStartDate, ProfileEndDate);

        public override IEditModel<EmployeeDisciplineInfo> Create (EmployeeDisciplineInfo model, ViewModelContext context)
        {
            var viewModel = new EmployeeDisciplineEditModel ();
            CopyCstor.Copy<IEmployeeDisciplineWritable> (model, viewModel);

            viewModel.Code = model.EduProfile.EduProgram.Code;
            viewModel.Title = model.EduProfile.EduProgram.Title;
            viewModel.ProfileCode = model.EduProfile.ProfileCode;
            viewModel.ProfileTitle = model.EduProfile.ProfileTitle;
            viewModel.ProfileStartDate = model.EduProfile.StartDate;
            viewModel.ProfileEndDate = model.EduProfile.EndDate;
            viewModel.EduLevelString = UniversityFormatHelper.FormatShortTitle (model.EduProfile.EduLevel.ShortTitle, model.EduProfile.EduLevel.Title);

            return viewModel;
        }

        public override EmployeeDisciplineInfo CreateModel ()
        {
            var model = new EmployeeDisciplineInfo ();
            CopyCstor.Copy<IEmployeeDisciplineWritable> (this, model);
            return model;
        }

        public override void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            EmployeeID = targetItemId;
        }

        #endregion

        #region IEmployeeDisciplineWritable implementation

        public long EmployeeDisciplineID { get; set; }

        public int EmployeeID { get; set; }

        public int EduProgramProfileID { get; set; }

        public string Disciplines { get; set; }

        [JsonIgnore]
        [Obsolete]
        public IEmployee Employee { get; set; }

        [JsonIgnore]
        [Obsolete]
        public IEduProfile EduProfile { get; set; }

        #endregion

        #region External properties

        public string Code { get; set; }

        public string Title { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileTitle { get; set; }

        public DateTime? ProfileStartDate { get; set; }

        public DateTime? ProfileEndDate { get; set; }

        public string EduLevelString { get; set; }

        #endregion

        #region Bindable properties

        [JsonIgnore]
        public string EduProgramProfileString
        {
            get { return UniversityFormatHelper.FormatEduProfileTitle (Code, Title, ProfileCode, ProfileTitle); }
        }

        #endregion
    }
}
