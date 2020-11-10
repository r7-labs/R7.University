using System.Linq;
using R7.Dnn.Extensions.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.EduProgramProfiles.Queries
{
    internal class EduProfileEditQuery: QueryBase
    {
        public EduProfileEditQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public EduProfileInfo SingleOrDefault (int eduProgramProfileId)
        {
            return ModelContext.Query<EduProfileInfo> ()
                .Where (epp => epp.EduProgramProfileID == eduProgramProfileId)
                .Include2 (epp => epp.EduProgram)
                .Include2 (epp => epp.EduProgram.EduLevel)
                .Include2 (epp => epp.EduLevel)
                .IncludeEduProgramProfileFormYears ()
                .IncludeContingent ()
                .IncludeDivisions ()
                .IncludeDocuments ()
                .SingleOrDefault ();
        }
    }
}

