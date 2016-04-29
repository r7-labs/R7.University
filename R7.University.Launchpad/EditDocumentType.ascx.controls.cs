using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;

namespace R7.University.Launchpad
{
    public partial class EditDocumentType
    {
        protected LinkButton buttonUpdate;
        protected LinkButton buttonDelete;
        protected HyperLink linkCancel;
        protected TextBox textType;
        protected TextBox textDescription;
        protected CheckBox checkIsSystem;
    }
}
