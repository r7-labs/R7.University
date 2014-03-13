using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;

namespace R7.University.EmployeeList
{
	public partial class SettingsEmployeeList
	{
		protected LabelControl labelDivision;
		protected DnnTreeView treeDivisions;
		protected LabelControl labelIncludeSubdivisions;
		protected CheckBox checkIncludeSubdivisions;
		protected LabelControl labelSortType;
		protected DnnComboBox comboSortType;
	}
}
