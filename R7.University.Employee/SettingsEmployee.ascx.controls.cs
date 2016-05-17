using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;

namespace R7.University.Employee
{
    public partial class SettingsEmployee
    {
        protected LabelControl labelEmployee;
        protected DropDownList comboEmployees;
        protected LabelControl labelAutoTitle;
        protected CheckBox checkAutoTitle;
        protected LabelControl labelPhotoWidth;
        protected TextBox textPhotoWidth;
        protected LabelControl labelShowCurrentUser;
        protected CheckBox checkShowCurrentUser;
    }
}
