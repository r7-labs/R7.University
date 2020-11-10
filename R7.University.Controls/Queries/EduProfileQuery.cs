using System.Collections.Generic;
using System.Linq;
using R7.Dnn.Extensions.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Controls.Queries
{
    internal class EduProfileQuery: QueryBase
    {
        public EduProfileQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IList<EduProfileInfo> ListByEduProgram (int eduProgramId)
        {
            return ModelContext.Query<EduProfileInfo> ()
                .Include2 (epp => epp.EduProgram)
                .Include2 (epp => epp.EduProgram.EduLevel)
                .Include2 (epp => epp.EduLevel)
                .Where (epp => epp.EduProgramID == eduProgramId)
                .ToList ();
        }

        public EduProfileInfo SingleOrDefault (int eduProgramProfileId)
        {
            return ModelContext.QueryWhere<EduProfileInfo> (epp => epp.EduProgramProfileID == eduProgramProfileId)
                .Include2 (epp => epp.EduProgram)
                .Include2 (epp => epp.EduProgram.EduLevel)
                .Include2 (epp => epp.EduLevel)
                .SingleOrDefault ();
        }
    }
}
