using System;
using DotNetNuke.Services.Localization;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EditModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.Controls.EditModels
{
    [Serializable]
    public class EmployeeAchievementEditModel : EditModelBase<EmployeeAchievementInfo>, IEmployeeAchievementWritable
    {
        #region IEditControlViewModel implementation

        public override void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            EmployeeID = targetItemId;
        }

        public override EmployeeAchievementInfo CreateModel ()
        {
            var achievement = new EmployeeAchievementInfo ();
            CopyCstor.Copy<IEmployeeAchievementWritable> (this, achievement);

            if (achievement.AchievementID != null) {
                achievement.Title = null;
                achievement.ShortTitle = null;
                achievement.AchievementTypeId = null;
            }

            return achievement;
        }

        public override IEditModel<EmployeeAchievementInfo> Create (EmployeeAchievementInfo model, ViewModelContext context)
        {
            var viewModel = new EmployeeAchievementEditModel ();
            CopyCstor.Copy<IEmployeeAchievementWritable> (model, viewModel);
            viewModel.Context = context;

            if (model.Achievement != null) {
                viewModel.Title = model.Achievement.Title;
                viewModel.ShortTitle = model.Achievement.ShortTitle;
                viewModel.AchievementTypeId = model.Achievement.AchievementTypeId;
                if (model.Achievement.AchievementType != null) {
                    viewModel.Type = model.Achievement.AchievementType.Type;
                }
            }
            else if (model.AchievementType != null) {
                viewModel.Type = model.AchievementType.Type;
            }

            viewModel.EduLevel_String = model.EduLevel?.FormatShortTitle () ?? string.Empty;

            return viewModel;
        }

        #endregion

        #region IEmployeeAchievementWritable implementation

        public int EmployeeAchievementID { get; set; }

        public int EmployeeID { get; set; }

        public int? AchievementID { get; set; }

        public int? AchievementTypeId { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public string Description { get; set; }

        public int? YearBegin { get; set; }

        public int? YearEnd { get; set; }

        public bool IsTitle { get; set; }

        public string DocumentURL { get; set; }

        public string TitleSuffix { get; set; }

        public int? Hours { get; set; }

        public int? EduLevelId { get; set; }

        [JsonIgnore]
        [Obsolete ("Don't use this property directly", true)]
        public IAchievement Achievement { get; set; }

        [JsonIgnore]
        [Obsolete ("Don't use this property directly", true)]
        public IAchievementType AchievementType { get; set; }

        [JsonIgnore]
        [Obsolete ("Don't use this property directly", true)]
        public IEduLevel EduLevel { get; set; }

        #endregion

        #region Flattened external properties

        public string Type { get; set; }

        public string EduLevel_String { get; set; }

        #endregion

        #region Derieved properties

        [JsonIgnore]
        public string Years_String
        {
            get {
                return UniversityFormatHelper.FormatYears (YearBegin, YearEnd,
                                                 Localization.GetString ("AtTheMoment.Text", Context.LocalResourceFile));
            }
        }

        [JsonIgnore]
        public string AchievementType_String
        {
            get {

                // TODO: Don't create new object here?
                var achievementType = (AchievementTypeId != null) ? new AchievementTypeInfo { Type = Type } : null;
                return achievementType.Localize (Context.LocalResourceFile);
            }
        }

        [JsonIgnore]
        public string Title_HtmlString
        {
            get {
                var title = FormatHelper.JoinNotNullOrEmpty (" ", Title, TitleSuffix);
                return IsTitle ? $"<strong>{title}</strong>" : title;
            }
        }

        [JsonIgnore]
        public string FormattedUrl =>
            UniversityUrlHelper.FormatNiceDocumentUrl (DocumentURL, Context.Module.ModuleId, Context.Module.TabId,
                Context.Module.PortalId, Context.LocalResourceFile);

        #endregion
    }
}
