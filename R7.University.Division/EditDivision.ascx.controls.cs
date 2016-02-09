using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;

namespace R7.University.Division
{
	public partial class EditDivision
	{
		protected ModuleAuditControl ctlAudit;
		protected LinkButton buttonUpdate;
		protected LinkButton buttonDelete;
		protected HyperLink linkCancel;

		protected LabelControl lblTitle;
		protected TextBox txtTitle;
		protected LabelControl lblShortTitle;
		protected TextBox txtShortTitle;

		protected LabelControl lblParentDivision;
        protected DnnTreeView treeParentDivisions;
		protected LabelControl lblDivisionTerm;
		protected DnnTreeView treeDivisionTerms;

		protected LabelControl lblHomePage;
		protected DnnUrlControl urlHomePage;
		protected LabelControl lblWebSite;
		protected TextBox txtWebSite;
		protected LabelControl labelWebSiteLabel;
		protected TextBox textWebSiteLabel;
		protected LabelControl labelDocumentUrl;
		protected DnnUrlControl urlDocumentUrl;
		protected LabelControl lblPhone;
		protected TextBox txtPhone;
		protected LabelControl lblFax;
		protected TextBox txtFax;
		protected LabelControl lblEmail;
		protected TextBox txtEmail;
		protected LabelControl lblSecondaryEmail;
		protected TextBox txtSecondaryEmail;
		protected LabelControl lblLocation;
		protected TextBox txtLocation;
		protected LabelControl labelWorkingHours;
		protected LabelControl labelCustomWorkingHours;
        protected DropDownList comboWorkingHours;
		protected TextBox textWorkingHours;
		protected CheckBox checkAddToVocabulary;
        protected DnnDateTimePicker datetimeStartDate;
        protected DnnDateTimePicker datetimeEndDate;
        protected CheckBox checkIsVirtual;
        protected AjaxControlToolkit.ComboBox comboHeadPosition;
	}
}
