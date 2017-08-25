using System.Web.UI.WebControls;

namespace R7.University.Controls
{
    public partial class EditDisciplines
    {
        protected GridView gridDisciplines;
        protected TextBox textDisciplines;
        protected DropDownList comboEduLevel;
        protected DropDownList comboEduProgramProfile;
        protected CustomValidator valEduProgramProfile;
        protected HiddenField hiddenDisciplineItemID;
        protected LinkButton buttonAddDiscipline;
        protected LinkButton buttonUpdateDiscipline;
        protected LinkButton buttonCancelEditDiscipline;
        protected LinkButton buttonResetForm;
        protected HiddenField hiddenEduProgramProfileID;
    }
}
