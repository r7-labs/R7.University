using System.Web.UI.WebControls;

namespace R7.University.Controls
{
    public partial class EditDivisions
    {
        protected DivisionSelector divisionSelector;
        protected TextBox textDivisionRole;
        protected GridView gridDivisions;
        protected HiddenField hiddenDivisionItemID;
        protected LinkButton buttonAddDivision;
        protected LinkButton buttonUpdateDivision;
        protected LinkButton buttonCancelEditDivision;
        protected LinkButton buttonResetForm;
        protected HiddenField hiddenDivisionID;
        protected CustomValidator valDivision;
    }
}
