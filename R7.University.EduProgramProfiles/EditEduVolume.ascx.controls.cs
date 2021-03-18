using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University.Controls;

namespace R7.University.EduProgramProfiles
{
    public partial class EditEduVolume
    {
        protected LinkButton buttonUpdate;
        protected LinkButton buttonDelete;
        protected HyperLink linkCancel;
        protected TextBox textTimeToLearnYears;
        protected TextBox textTimeToLearnMonths;
        protected TextBox textTimeToLearnHours;
        protected TextBox textYear1Cu;
        protected TextBox textYear2Cu;
        protected TextBox textYear3Cu;
        protected TextBox textYear4Cu;
        protected TextBox textYear5Cu;
        protected TextBox textYear6Cu;
        protected TextBox textPracticeType1Cu;
        protected TextBox textPracticeType2Cu;
        protected TextBox textPracticeType3Cu;
        protected HtmlControl tabCommon;
        protected HtmlControl tabYears;
        protected HtmlControl tabPractices;
        protected Panel pnlCommon;
        protected Panel pnlYears;
        protected Panel pnlPractices;
    }
}
