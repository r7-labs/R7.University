using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;
using R7.University.Controls;

namespace R7.University.Launchpad
{
	public partial class EditEduProgramProfile
	{
		protected LinkButton buttonUpdate;
		protected LinkButton buttonDelete;
		protected HyperLink linkCancel;
        protected AjaxControlToolkit.ComboBox comboEduProgram;
        protected TextBox textProfileCode;
		protected TextBox textProfileTitle;
        protected TextBox textLanguages;
        protected DnnDateTimePicker datetimeStartDate;
        protected DnnDateTimePicker datetimeEndDate;
        protected DnnDatePicker dateAccreditedToDate;
        protected DnnDatePicker dateCommunityAccreditedToDate;
        protected DropDownList comboEduLevel;
        protected EditDocuments formEditDocuments;
        protected EditEduForms formEditEduForms;
        protected ModuleAuditControl auditControl;
	}
}
