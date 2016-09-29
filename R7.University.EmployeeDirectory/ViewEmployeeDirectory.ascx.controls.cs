using System.Web.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;

namespace R7.University.EmployeeDirectory
{
    public partial class ViewEmployeeDirectory
    {
        protected MultiView mviewEmployeeDirectory;
        protected GridView gridEmployees;
        protected TextBox textSearch;
        protected LinkButton linkSearch;
        protected DnnTreeView treeDivisions;
        protected CheckBox checkTeachersOnly;
        protected Repeater repeaterEduProgramProfiles;
    }
}
