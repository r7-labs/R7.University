using DotNetNuke.Services.Localization;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.ModelExtensions
{
    public static class AchievementTypeDnnExtensions
    {
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
