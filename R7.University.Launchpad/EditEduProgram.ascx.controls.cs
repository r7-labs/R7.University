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
	public partial class EditEduProgram
	{
		protected LinkButton buttonUpdate;
		protected LinkButton buttonDelete;
		protected HyperLink linkCancel;
        protected TextBox textCode;
		protected TextBox textTitle;
        protected TextBox textProfileCode;
        protected TextBox textProfileTitle;
        protected DropDownList comboEduLevel;
	}
}
