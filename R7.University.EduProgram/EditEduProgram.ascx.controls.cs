using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;
using R7.University.Controls;

namespace R7.University.EduProgram
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
        protected EditDocuments formEditDocuments;
        protected DnnUrlControl urlHomePage;
        protected DnnTreeView treeDivision;
    }
}
