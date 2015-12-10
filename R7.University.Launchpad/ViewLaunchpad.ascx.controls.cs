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
        protected HyperLink linkAddItem;
        protected TextBox textSearch;
        protected LinkButton buttonSearch;
        protected LinkButton buttonResetSearch;
		protected GridView gridPositions;
		protected GridView gridDivisions;
		protected GridView gridEmployees;
		protected GridView gridAchievements;
		protected GridView gridEduLevels;
        protected GridView gridEduPrograms;
        protected GridView gridEduProgramProfiles;
        protected GridView gridDocumentTypes;
        protected GridView gridDocuments;
        protected GridView gridEduForms;
        protected Repeater repeatTabs;
	}
}
