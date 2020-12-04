using System.Data;
using System.Linq;
using DotNetNuke.Entities.Modules;
using R7.University.Launchpad.Components;
using R7.University.Launchpad.Queries;
using R7.University.Launchpad.ViewModels;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public class EduProfilesTable: LaunchpadTableBase
    {
        public EduProfilesTable () : base (typeof (EduProfileInfo))
        {
            EditControlKey = "editEduProgramProfile";
        }

        public override DataTable GetDataTable (PortalModuleBase module, UniversityModelContext modelContext, string search)
        {
            var eduProfiles = new FindEduProfileQuery (modelContext).List (search)
                .Select (epp => new EduProfileViewModel (epp));

            return DataTableConstructor.FromIEnumerable (eduProfiles);
        }
    }
}
