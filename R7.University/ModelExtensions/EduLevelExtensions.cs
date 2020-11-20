using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.ModelExtensions
{
    public static class EduLevelExtensions
    {
        public static string FormatShortTitle (this IEduLevel eduLevel)
        {
            return UniversityFormatHelper.FormatShortTitle (eduLevel.ShortTitle, eduLevel.Title);
        }
    }
}
