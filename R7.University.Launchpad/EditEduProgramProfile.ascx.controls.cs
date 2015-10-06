using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;

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
        protected DnnDateTimePicker datetimeStartDate;
        protected DnnDateTimePicker datetimeEndDate;
        protected DropDownList comboEduLevel;
        protected ModuleAuditControl auditControl;
	}
}
