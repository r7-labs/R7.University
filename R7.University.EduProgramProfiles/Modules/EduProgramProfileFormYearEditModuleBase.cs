using R7.Dnn.Extensions.Text;
using R7.University.Dnn.Modules;
using R7.University.EduProgramProfiles.Queries;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.EduProgramProfiles.Modules
{
    public abstract class EduProgramProfileFormYearEditModuleBase<T> : UniversityEditPortalModuleBase<T> where T : class, new()
    {
        protected EduProgramProfileFormYearEditModuleBase (string key) : base (key)
        {
        }

        protected int? GetEduProgramProfileFormYearId () =>
            ParseHelper.ParseToNullable<int> (Request.QueryString [Key])
                     ?? ParseHelper.ParseToNullable<int> (Request.QueryString ["eduprogramprofileformyear_id"]);

        protected IEduProgramProfileFormYear GetEduProgramProfileFormYear ()
        {
            var eppfyId = GetEduProgramProfileFormYearId ();
            return eppfyId != null ? new EduProgramProfileFormYearEditQuery (ModelContext).SingleOrDefault (eppfyId.Value) : null;
        }

        protected IYear GetLastYear ()
        {
            return new FlatQuery<YearInfo> (ModelContext).List ().LastYear ();
        }
    }
}
