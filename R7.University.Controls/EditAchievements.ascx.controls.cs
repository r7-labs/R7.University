using System.Web.UI.WebControls;
using DotNetNuke.Web.UI.WebControls;

namespace R7.University.Controls
{
    public partial class EditAchievements
    {
        protected DropDownList comboAchievementTypes;
        protected TextBox textYearBegin;
        protected TextBox textYearEnd;
        protected TextBox textAchievementTitle;
        protected TextBox textAchievementShortTitle;
        protected TextBox textAchievementDescription;
        protected CheckBox checkIsTitle;
        protected DropDownList comboAchievement;
        protected Panel panelAchievementTitle;
        protected Panel panelAchievementShortTitle;
        protected Panel panelAchievementTypes;
        protected TextBox textAchievementTitleSuffix;
        protected DnnUrlControl urlDocumentUrl;
        protected TextBox txtDocumentUrl;
        protected TextBox txtHours;

        // TODO: Replace with dropdown
        protected TextBox txtEduLevelId;
    }
}
