using System.Linq;
using Microsoft.EntityFrameworkCore;
using R7.University.Models;

namespace R7.University.Queries
{
    public static class EduProgramQueryableExtensions
    {
        public static IQueryable<EduProgramInfo> IncludeDivisions (this IQueryable<EduProgramInfo> eduPrograms)
        {
            return eduPrograms.Include (ep => ep.Divisions)
                                .ThenInclude (d => d.Division);
        }

        public static IQueryable<EduProgramInfo> IncludeDocuments (this IQueryable<EduProgramInfo> eduPrograms)
        {
            return eduPrograms.Include (ep => ep.Documents)
                                .ThenInclude (d => d.DocumentType);
        }

        public static IQueryable<EduProgramInfo> IncludeEduLevelDivisionsAndDocuments (this IQueryable<EduProgramInfo> eduPrograms)
        {
            return eduPrograms.Include (ep => ep.EduLevel)
                              .IncludeDivisions ()
                              .IncludeDocuments ();
        }

        public static IQueryable<EduProgramInfo> IncludeEduProfiles (this IQueryable<EduProgramInfo> eduPrograms)
        {
            return eduPrograms.Include (ep => ep.EduProfiles)
                                .ThenInclude (epp => epp.EduLevel)
                              .Include (ep => ep.EduProfiles)
                                .ThenInclude (eppd => eppd.Divisions)
                                    .ThenInclude (d => d.Division)
                              .Include (ep => ep.EduProfiles)
                                .ThenInclude (epp => epp.EduProgramProfileFormYears)
                                    .ThenInclude (eppfy => eppfy.EduForm)
                              .Include (ep => ep.EduProfiles)
                                .ThenInclude (epp => epp.EduProgramProfileFormYears)
                                    .ThenInclude (eppfy => eppfy.EduVolume)
                              .Include (ep => ep.EduProfiles)
                                .ThenInclude (epp => epp.EduProgramProfileFormYears)
                                    .ThenInclude (eppfy => eppfy.Year);
        }
    }
}

