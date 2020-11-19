using System;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class AchievementTypeExtensions
    {
        public static SystemAchievementType GetSystemType (this IAchievementType achievementType)
        {
            if (achievementType != null) {
                SystemAchievementType result;
                if (Enum.TryParse (achievementType.Type, out result)) {
                    return result;
                }
            }

            return SystemAchievementType.Custom;
        }

        public static bool IsOneOf (this IAchievementType achievementType, params SystemAchievementType [] systemAchievementTypes)
        {
            var achievementTypeParsed = GetSystemType (achievementType);
            foreach (var systemAchievementType in systemAchievementTypes) {
                if (systemAchievementType == achievementTypeParsed) {
                    return true;
                }
            }

            return false;
        }

        public static bool IsEducation (this IAchievementType achievementType)
        {
            return achievementType.IsOneOf (
                SystemAchievementType.Education,
                SystemAchievementType.ProfTraining);
        }

        public static bool IsTraining (this IAchievementType achievementType)
        {
            return achievementType.IsOneOf (
                SystemAchievementType.ProfRetraining,
                SystemAchievementType.Training,
                SystemAchievementType.ShortTermTraining,
                SystemAchievementType.Internship);
        }

        public static bool IsExperience (this IAchievementType achievementType)
        {
            return achievementType.IsOneOf (
                SystemAchievementType.Education,
                SystemAchievementType.AcademicDegree,
                SystemAchievementType.Training,
                SystemAchievementType.ShortTermTraining,
                SystemAchievementType.Internship,
                SystemAchievementType.ProfTraining,
                SystemAchievementType.ProfRetraining,
                SystemAchievementType.Work);
        }
    }
}
