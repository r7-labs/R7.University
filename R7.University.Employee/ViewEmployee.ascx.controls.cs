using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using R7.University;

namespace R7.University.Employee
{
	public partial class ViewEmployee
	{
		protected Panel panelEmployee;
		protected Image imagePhoto;
		protected Label labelFullName;
		protected Label labelPositions;
		protected Label labelAcademicDegreeAndTitle;
		// protected Label labelAcademicTitle;
		// protected Label labelAcademicDegree;
		protected Label labelPhone;
		protected Label labelCellPhone;
		protected Label labelFax;
		protected Label labelMessenger;
		//protected Label labelWorkingHours;
		//protected Label labelWorkingPlace;
		protected Label labelWorkingPlaceAndHours;

		protected HyperLink linkWebSite;
		protected HyperLink linkEmail;
		protected HyperLink linkSecondaryEmail;
		protected HyperLink linkUserProfile;
		protected Repeater repeaterPositions;
	}
}
