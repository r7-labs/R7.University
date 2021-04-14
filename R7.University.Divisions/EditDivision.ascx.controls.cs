using System.Web.UI.WebControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University.Controls;

namespace R7.University.Divisions
{
    public partial class EditDivision
    {
        protected ModuleAuditControl ctlAudit;
        protected LinkButton buttonUpdate;
        protected LinkButton buttonDelete;
        protected HyperLink linkCancel;
        protected TextBox txtTitle;
        protected TextBox txtShortTitle;
        protected DivisionSelector parentDivisionSelector;
        protected DnnUrlControl urlHomePage;
        protected TextBox txtWebSite;
        protected TextBox textWebSiteLabel;
        protected DnnUrlControl urlDocumentUrl;
        protected TextBox txtPhone;
        protected TextBox txtFax;
        protected TextBox txtEmail;
        protected RegularExpressionValidator valEmail;
        protected TextBox txtSecondaryEmail;
        protected RegularExpressionValidator valSecondaryEmail;
        protected TextBox textAddress;
        protected TextBox txtLocation;
        protected DropDownList comboWorkingHours;
        protected TextBox textWorkingHours;
        protected CheckBox checkAddToVocabulary;
        protected DnnDateTimePicker datetimeStartDate;
        protected DnnDateTimePicker datetimeEndDate;
        protected CheckBox checkIsSingleEntity;
        protected CheckBox checkIsInformal;
        protected CheckBox checkIsGoverning;
        protected DropDownList comboHeadPosition;
        protected CheckBox chkUseCurrentPageAsHomePage;
    }
}
