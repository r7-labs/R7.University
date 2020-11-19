using System.Collections.Generic;
using System.Linq;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class YearExtensions
    {
        public static IYear LastYear (this IEnumerable<IYear> years)
        {
            return years.OrderByDescending (y => y.Year).FirstOrDefault (y => !y.AdmissionIsOpen);
        }

        public static string FormatWithCourse (this IYear year, IYear lastYear)
        {
            if (year != null) {
                var course = UniversityModelHelper.SafeGetCourse (year, lastYear);
                if (course != null) {
                    return $"{year.Year} ({course})";
                }
                return year.Year.ToString ();
            }

            return "-";
        }
    }
}
