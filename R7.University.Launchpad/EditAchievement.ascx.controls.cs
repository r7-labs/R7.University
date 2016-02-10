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
	public partial class EditAchievement
	{
		protected LinkButton buttonUpdate;
		protected LinkButton buttonDelete;
		protected HyperLink linkCancel;

		protected LabelControl labelTitle;
		protected TextBox textTitle;
		protected LabelControl labelShortTitle;
		protected TextBox textShortTitle;
		protected LabelControl labelAchievementType;
		protected DropDownList comboAchievementType;
	}
}
