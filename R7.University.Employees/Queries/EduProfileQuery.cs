using System.Collections.Generic;
using System.Linq;
using R7.Dnn.Extensions.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Employees.Queries
{
    internal class EduProfileQuery: QueryBase
    {
        public EduProfileQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IList<EduProfileInfo> ListByEduLevels (IEnumerable<int> eduLevelIds)
        {
            return ModelContext.Query<EduProfileInfo> ()
                               .Include2 (epp => epp.EduLevel)
                               .Include2 (epp => epp.EduProgram)
                               .WhereEduLevelsOrAll (eduLevelIds)
                               .DefaultOrder ()
                               .ToList ();
        }
    }
}
