using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using R7.University;

namespace R7.University.Launchpad
{
	public partial class EditPosition
	{
		protected LinkButton buttonUpdate;
		protected LinkButton buttonDelete;
		protected HyperLink linkCancel;
		protected LabelControl lblTitle;
		protected TextBox txtTitle;
		protected LabelControl lblShortTitle;
		protected TextBox txtShortTitle;
		protected LabelControl lblWeight;
		protected TextBox txtWeight;
		protected LabelControl labelIsTeacher;
		protected CheckBox checkIsTeacher;
	}
}
