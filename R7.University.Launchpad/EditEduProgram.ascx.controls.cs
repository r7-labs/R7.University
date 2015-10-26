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
	public partial class EditEduProgram
	{
        protected LinkButton buttonUpdate;
        protected LinkButton buttonDelete;
        protected HyperLink linkCancel;
        protected ModuleAuditControl auditControl;
        protected TextBox textCode;
		protected TextBox textTitle;
        protected TextBox textGeneration;
        protected DnnDatePicker dateAccreditedToDate;
        protected DnnDateTimePicker datetimeStartDate;
        protected DnnDateTimePicker datetimeEndDate;
        protected DropDownList comboEduLevel;
       
        #region Documents

        protected GridView gridDocuments;
        protected TextBox textDocumentTitle;
        protected DropDownList comboDocumentType;
        protected UrlControl urlDocumentUrl;
        protected TextBox textDocumentSortIndex;
        protected DnnDateTimePicker datetimeDocumentStartDate;
        protected DnnDateTimePicker datetimeDocumentEndDate;
        protected HiddenField hiddenDocumentItemID;
        protected LinkButton buttonAddDocument;
        protected LinkButton buttonUpdateDocument;
        protected LinkButton buttonCancelEditDocument;

        #endregion
       
	}
}
