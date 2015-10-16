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
        protected TextBox textSearch;
        protected Button buttonSearch;
        protected Button buttonResetSearch;
		protected GridView gridPositions;
		protected HyperLink buttonAddPosition;
		protected GridView gridDivisions;
		protected HyperLink buttonAddDivision;
		protected GridView gridEmployees;
		protected HyperLink buttonAddEmployee;
		protected GridView gridAchievements;
		protected HyperLink buttonAddAchievement;
        protected GridView gridEduLevels;
        protected HyperLink buttonAddEduLevel;
        protected GridView gridEduPrograms;
        protected HyperLink buttonAddEduProgram;
        protected GridView gridEduProgramProfiles;
        protected HyperLink buttonAddEduProgramProfile;
        protected GridView gridDocumentTypes;
        protected HyperLink buttonAddDocumentType;
		protected Repeater repeatTabs;
	}
}
