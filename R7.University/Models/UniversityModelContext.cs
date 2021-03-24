using DotNetNuke.Common.Utilities;
using R7.Dnn.Extensions.Data;
using R7.Dnn.Extensions.Models;
using R7.University.Data;

namespace R7.University.Models
{
    public class UniversityModelContext: ModelContextBase
    {
        public UniversityModelContext (IDataContext dataContext): base (dataContext)
        {
        }

        public UniversityModelContext ()
        {
        }

        #region ModelContextBase implementation

        public override IDataContext CreateDataContext ()
        {
            return UniversityDataContextFactory.Instance.Create ();
        }

        public override bool SaveChanges (bool dispose = true)
        {
            var isFinal = dispose;
            var result = base.SaveChanges (isFinal);
            if (isFinal) {
                DataCache.ClearCache ("//r7_University");
            }
            return result;
        }

        #endregion
    }
}
