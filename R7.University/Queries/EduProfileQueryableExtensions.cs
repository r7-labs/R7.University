using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using R7.Dnn.Extensions.Collections;
using R7.University.Models;

namespace R7.University.Queries
{
    public static class EduProfileQueryableExtensions
    {
        public static IQueryable<EduProfileInfo> IncludeEduProgramAndDivisions (this IQueryable<EduProfileInfo> eduProfiles)
        {
            return eduProfiles.Include (epp => epp.EduLevel)
                                     .Include (epp => epp.EduProgram)
                                        .ThenInclude (ep => ep.EduLevel)
                                     .Include (epp => epp.Divisions)
                                     .Include (epp => epp.EduProgram)
                                        .ThenInclude (epd => epd.Divisions);
        }

        public static IQueryable<EduProfileInfo> IncludeEduProgramProfileFormYears (this IQueryable<EduProfileInfo> eduProfiles)
        {
            return eduProfiles.Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.Year)
                                     .Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.EduForm)
                                     .Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.EduVolume);
        }

        public static IQueryable<EduProfileInfo> IncludeContingent (this IQueryable<EduProfileInfo> eduProfiles)
        {
            return eduProfiles.Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.Contingent);
        }

        public static IQueryable<EduProfileInfo> IncludeEduProgramProfileFormYearsAndForms (this IQueryable<EduProfileInfo> eduProfiles)
        {
            return eduProfiles.Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.Year)
                                     .Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.EduForm);
        }

        public static IQueryable<EduProfileInfo> IncludeDivisions (this IQueryable<EduProfileInfo> eduProfiles)
        {
            return eduProfiles.Include (epp => epp.Divisions)
                                        .ThenInclude (d => d.Division);
        }

        public static IQueryable<EduProfileInfo> IncludeDocuments (this IQueryable<EduProfileInfo> eduProfiles)
        {
            return eduProfiles.Include (epp => epp.Documents)
                                        .ThenInclude (d => d.DocumentType);
        }

        public static IQueryable<EduProfileInfo> WhereEduLevelsOrAll (this IQueryable<EduProfileInfo> eduProfiles, IEnumerable<int> eduLevelIds)
        {
            if (!eduLevelIds.IsNullOrEmpty ()) {
                return eduProfiles.Where (epp => eduLevelIds.Contains (epp.EduLevelId));
            }

            return eduProfiles;
        }

        public static IQueryable<EduProfileInfo> WhereDivisionOrAll (this IQueryable<EduProfileInfo> eduProfiles, int? divisionId, DivisionLevel divisionLevel)
        {
            if (divisionId != null) {
                if (divisionLevel == DivisionLevel.EduProgram) {
                    return eduProfiles.Where (epp => epp.EduProgram.Divisions.Any (epd => epd.DivisionId == divisionId));
                }
                if (divisionLevel == DivisionLevel.EduProgramProfile) {
                    return eduProfiles.Where (epp => epp.Divisions.Any (epd => epd.DivisionId == divisionId));
                }
            }

            return eduProfiles;
        }

        public static IQueryable<EduProfileInfo> DefaultOrder (this IQueryable<EduProfileInfo> source)
        {
            return source.OrderBy (epp => epp.EduProgram.EduLevel.SortIndex)
                         .ThenBy (epp => epp.EduProgram.Code)
                         .ThenBy (epp => epp.EduProgram.Title)
                         .ThenBy (epp => epp.ProfileCode)
                         .ThenBy (epp => epp.ProfileTitle)
                         .ThenBy (epp => epp.EduLevel.SortIndex);
        }
    }
}
