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
		protected Panel panelDivision;
		protected Label labelTitle;
		protected HyperLink linkSearchByTerm;
		protected HyperLink linkHomePage;
		protected HyperLink linkEmail;
		protected HyperLink linkWebSite;
		protected HyperLink linkSecondaryEmail;
		protected HyperLink linkDocumentUrl;
		protected Label labelPhone;
		protected Label labelFax;
		protected Label labelLocation;
		protected Label labelWorkingHours;
		protected Image imageBarcode;
        protected Panel panelSubDivisions;
        protected Label labelSubDivisions;
        protected Repeater repeatSubDivisions;
	}
}
