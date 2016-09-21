using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using R7.University;
using R7.University.Controls;

namespace R7.University.Employee
{
    public partial class ViewEmployeeDetails
    {
        protected Panel panelEmployeeDetails;
        protected Image imagePhoto;
        protected Image imageBarcode;
        protected Literal literalFullName;
        protected Label labelAcademicDegreeAndTitle;
        protected Label labelMessenger;
        protected Label labelPhone;
        protected Label labelFax;
        protected Label labelCellPhone;
        protected Label labelWorkingPlaceAndHours;
        protected Literal litAbout;
        protected Literal litDisciplines;
        protected HyperLink linkAbout;
        protected HyperLink linkDisciplines;
        protected HyperLink linkAchievements;
        protected HyperLink linkExperience;
        protected HyperLink linkEmail;
        protected HyperLink linkSecondaryEmail;
        protected HyperLink linkWebSite;
        protected HyperLink linkUserProfile;
        protected Repeater repeaterPositions;
        protected Label labelExperienceYears;
        protected HyperLink linkReturn;
        protected HyperLink linkVCard;
        protected HyperLink linkEdit;
        protected GridView gridExperience;
        protected GridView gridAchievements;
        protected GridView gridEduPrograms;
        protected HyperLink linkBarcode;
        protected AgplSignature agplSignature;
    }
}
