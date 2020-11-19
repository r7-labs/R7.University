using System;
using DotNetNuke.Services.Localization;
using R7.University.Models;
using R7.University.Utilities;

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

        public static string Localize (this IAchievementType achievementType, string resourceFile)
        {
            if (achievementType != null) {
                return LocalizationHelper.GetStringWithFallback (
                    "SystemAchievementType_" + achievementType.Type + ".Text", resourceFile, achievementType.Type
                );
            }

            return Localization.GetString ("SystemAchievementType_Custom.Text", resourceFile);
        }
    }
}
