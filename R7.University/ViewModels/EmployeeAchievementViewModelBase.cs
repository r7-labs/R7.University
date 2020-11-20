using R7.University.Models;

namespace R7.University.ViewModels
{
    public abstract class EmployeeAchievementViewModelBase: IEmployeeAchievement
    {
        public IEmployeeAchievement EmployeeAchievement { get; protected set; }

        protected EmployeeAchievementViewModelBase (IEmployeeAchievement model)
        {
            EmployeeAchievement = model;
        }

        #region IEmployeeAchievement implementation

        public int EmployeeAchievementID => EmployeeAchievement.EmployeeAchievementID;

        public int EmployeeID => EmployeeAchievement.EmployeeID;

        public int? AchievementID => EmployeeAchievement.AchievementID;

        public int? AchievementTypeId => EmployeeAchievement.AchievementTypeId;

        public string Title => (Achievement != null) ? EmployeeAchievement.Achievement.Title : EmployeeAchievement.Title;

        public string ShortTitle => (Achievement != null) ? EmployeeAchievement.Achievement.ShortTitle : EmployeeAchievement.ShortTitle;

        public string Description => EmployeeAchievement.Description;

        public int? YearBegin => EmployeeAchievement.YearBegin;

        public int? YearEnd => EmployeeAchievement.YearEnd;

        public bool IsTitle => EmployeeAchievement.IsTitle;

        public string DocumentURL => EmployeeAchievement.DocumentURL;

        public string TitleSuffix => EmployeeAchievement.TitleSuffix;

        public int? Hours => EmployeeAchievement.Hours;

        public int? EduLevelId => EmployeeAchievement.EduLevelId;

        public IAchievement Achievement => EmployeeAchievement.Achievement;

        public IAchievementType AchievementType => (EmployeeAchievement.Achievement != null) ? EmployeeAchievement.Achievement.AchievementType : EmployeeAchievement.AchievementType;

        public IEduLevel EduLevel => EmployeeAchievement.EduLevel;

        #endregion
    }
}
