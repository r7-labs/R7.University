using System.Collections.Generic;
using System.Linq;
using R7.Dnn.Extensions.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.EduProgramProfiles.Queries
{
    internal class EduProfileQuery: QueryBase
    {
        public EduProfileQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IList<EduProfileInfo> ListWithEduForms (IEnumerable<int> eduLevelIds, int? divisionId, DivisionLevel divisionLevel)
        {
            return ModelContext.Query<EduProfileInfo> ()
                               .IncludeEduProgramAndDivisions ()
                               .IncludeEduProgramProfileFormYears ()
                               .WhereEduLevelsOrAll (eduLevelIds)
                               .WhereDivisionOrAll (divisionId, divisionLevel)
                               .DefaultOrder ()
                               .ToList ();
        }

        public IList<EduProfileInfo> ListWithDocuments (IEnumerable<int> eduLevelIds, int? divisionId, DivisionLevel divisionLevel)
        {
            return ModelContext.Query<EduProfileInfo> ()
                               .IncludeEduProgramAndDivisions ()
                               .IncludeDocuments ()
                               .IncludeEduProgramProfileFormYearsAndForms ()
                               .WhereEduLevelsOrAll (eduLevelIds)
                               .WhereDivisionOrAll (divisionId, divisionLevel)
                               .DefaultOrder ()
                               .ToList ();
        }
    }
}
