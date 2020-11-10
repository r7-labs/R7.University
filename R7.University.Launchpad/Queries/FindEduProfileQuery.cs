using System.Collections.Generic;
using System.Linq;
using R7.Dnn.Extensions.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Launchpad.Queries
{
    internal class FindEduProfileQuery: QueryBase
    {
        public FindEduProfileQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IList<EduProfileInfo> List (string search)
        {
            // FIXME: Cannot set comparison options
            return ((search != null)
                ? ModelContext.Query<EduProfileInfo> ()
                    .Include2 (epp => epp.EduProgram)
                    .Where (p => p.ProfileCode.Contains (search)
                        || p.ProfileTitle.Contains (search)
                        || p.EduProgram.Code.Contains (search)
                        || p.EduProgram.Title.Contains (search))
                : ModelContext.Query<EduProfileInfo> ()
                    .Include2 (epp => epp.EduProgram)
            ).ToList ();
        }
    }
}

