using System.Web.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;

namespace R7.University.Controls
{
    public partial class EditDocuments
    {
        protected TextBox textDocumentTitle;
        protected TextBox textDocumentGroup;
        protected DropDownList comboDocumentType;
        protected DnnUrlControl urlDocumentUrl;
        protected TextBox textDocumentSortIndex;
        protected DnnDateTimePicker datetimeDocumentStartDate;
        protected DnnDateTimePicker datetimeDocumentEndDate;
        protected CustomValidator valDocumentUrl;
    }
}
