using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
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
        protected HyperLink linkEmail;
        protected HyperLink linkSecondaryEmail;
        protected HyperLink linkWebSite;
        protected HyperLink linkUserProfile;
        protected Repeater repeaterPositions;
        protected Label labelExperienceYears;
        protected HyperLink linkReturn;
        protected HyperLink linkEdit;
        protected GridView gridExperience;
        protected GridView gridAchievements;
        protected GridView gridDisciplines;
        protected HyperLink linkBarcode;
        protected AgplSignature agplSignature;
        protected HtmlControl tabAbout;
        protected HtmlControl tabDisciplines;
        protected HtmlControl tabAchievements;
        protected HtmlControl tabExperience;
        protected Panel panelPositions;
        protected Panel panelContacts;
    }
}
