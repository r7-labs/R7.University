using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using R7.University;

namespace R7.University.Launchpad
{
	public partial class ViewLaunchpad
	{
		protected MultiView multiView;
		protected LinkButton linkPositions;
		protected LinkButton linkDivisions;
		protected LinkButton linkEmployees;
		protected GridView gridPositions;
		protected HyperLink buttonAddPosition;
		protected GridView gridDivisions;
		protected HyperLink buttonAddDivision;
		protected GridView gridEmployees;
		protected HyperLink buttonAddEmployee;

		protected HtmlControl liPositions;
		protected HtmlControl liDivisions;
		protected HtmlControl liEmployees;
	}
}
