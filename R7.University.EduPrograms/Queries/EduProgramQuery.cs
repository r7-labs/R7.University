using System.Linq;
using R7.Dnn.Extensions.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.EduPrograms.Queries
{
    internal class EduProgramQuery: EduProgramCommonQuery
    {
        public EduProgramQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public EduProgramInfo SingleOrDefault (int eduProgramId)
        {
            return ModelContext.QueryWhere<EduProgramInfo> (ep => ep.EduProgramID == eduProgramId)
                               .IncludeEduLevelDivisionsAndDocuments ()
                               .IncludeEduProfiles ()
                               // FIXME: Should be just SingleOrDefault: https://github.com/aspnet/EntityFrameworkCore/issues/11516
                               .ToList ()
                               .SingleOrDefault ();
        }
    }
}

