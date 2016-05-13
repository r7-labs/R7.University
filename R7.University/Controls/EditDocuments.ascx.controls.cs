using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using DotNetNuke.UI.UserControls;
using DotNetNuke.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;

namespace R7.University.Controls
{
    public partial class EditDocuments
    {
        protected GridView gridDocuments;
        protected TextBox textDocumentTitle;
        protected TextBox textDocumentGroup;
        protected DropDownList comboDocumentType;
        protected DnnUrlControl urlDocumentUrl;
        protected TextBox textDocumentSortIndex;
        protected DnnDateTimePicker datetimeDocumentStartDate;
        protected DnnDateTimePicker datetimeDocumentEndDate;
        protected HiddenField hiddenDocumentItemID;
        protected LinkButton buttonAddDocument;
        protected LinkButton buttonUpdateDocument;
        protected LinkButton buttonCancelEditDocument;
    }
}
