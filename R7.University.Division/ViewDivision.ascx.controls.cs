using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;

namespace R7.University.Division
{
	public partial class ViewDivision
	{
		protected Label labelTitle;
		protected Label labelShortTitle;
		protected HyperLink linkTerm;
		protected HyperLink linkHomePage;
		protected HyperLink linkEmail;
		protected HyperLink linkSecondaryEmail;
		protected Label labelPhone;
		protected Label labelFax;
		protected Label labelLocation;
		protected Label labelWorkingHours;
		protected Image imageBarcode;
		protected Repeater repeatSubDivisions;
	}
}
