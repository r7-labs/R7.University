namespace R7.University.Models
{
    public interface IEmployeeAchievement
    {
        int EmployeeAchievementID { get; }

        int EmployeeID { get; }

        int? AchievementID { get; }

        int? AchievementTypeId { get; }

        string Title { get; }

        string ShortTitle { get; }

        string Description { get; }

        int? YearBegin { get; }

        int? YearEnd { get; }

        bool IsTitle { get; }

        string DocumentURL { get; }

        string TitleSuffix { get; }

        int? Hours { get; }

        int? EduLevelId { get; }

        IAchievement Achievement { get; }

        IAchievementType AchievementType { get; }

        IEduLevel EduLevel { get; }
    }

    public interface IEmployeeAchievementWritable: IEmployeeAchievement
    {
        new int EmployeeAchievementID { get; set; }

        new int EmployeeID { get; set; }

        new int? AchievementID { get; set; }

        new int? AchievementTypeId { get; set; }

        new string Title { get; set; }

        new string ShortTitle { get; set; }

        new string Description { get; set; }

        new int? YearBegin { get; set; }

        new int? YearEnd { get; set; }

        new bool IsTitle { get; set; }

        new string DocumentURL { get; set; }

        new string TitleSuffix { get; set; }

        new int? Hours { get; set; }

        new int? EduLevelId { get; set; }

        new IAchievement Achievement { get; set; }

        new IAchievementType AchievementType { get; set; }

        new IEduLevel EduLevel { get; set; }
    }

    public class EmployeeAchievementInfo: IEmployeeAchievementWritable
    {
        public int EmployeeAchievementID { get; set; }

        public int EmployeeID  { get; set; }

        public int? AchievementID { get; set; }

        public int? AchievementTypeId { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string ShortTitle  { get; set; }

        public int? YearBegin { get; set; }

        public int? YearEnd { get; set; }

        public bool IsTitle { get; set; }

        public string DocumentURL { get; set; }

        public string TitleSuffix { get; set; }

        public int? Hours { get; set; }

        public int? EduLevelId { get; set; }

        public virtual AchievementInfo Achievement { get; set; }

        public virtual AchievementTypeInfo AchievementType { get; set; }

        public virtual EduLevelInfo EduLevel { get; set; }

        IAchievementType IEmployeeAchievement.AchievementType => AchievementType;

        IAchievementType IEmployeeAchievementWritable.AchievementType {
            get { return AchievementType; }
            set { AchievementType = (AchievementTypeInfo) value; }
        }

        IAchievement IEmployeeAchievement.Achievement => Achievement;

        IAchievement IEmployeeAchievementWritable.Achievement {
            get { return Achievement; }
            set { Achievement = (AchievementInfo) value; }
        }

        IEduLevel IEmployeeAchievement.EduLevel => EduLevel;

        IEduLevel IEmployeeAchievementWritable.EduLevel {
            get { return EduLevel; }
            set { EduLevel = (EduLevelInfo) value; }
        }
    }
}
