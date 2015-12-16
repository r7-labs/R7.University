using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;

namespace R7.University.Controls
{
    public partial class EditEduForms
    {
        protected GridView gridEduForms;
        protected DropDownList comboEduForm;
        protected CheckBox checkIsAdmissive;
        protected TextBox textTimeToLearnYears;
        protected TextBox textTimeToLearnMonths;
        protected HiddenField hiddenEduFormItemID;
        protected LinkButton buttonAddEduForm;
        protected LinkButton buttonUpdateEduForm;
        protected LinkButton buttonCancelEditEduForm;
    }
}
